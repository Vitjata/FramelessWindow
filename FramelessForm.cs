using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyFramelessApp
{
    public class FramelessForm : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint = default;
        private Point dragFormPoint = default;
        private Rectangle circleRed;
        private Rectangle circleBlue;
        private Brush paintForm = Brushes.LightCyan;
        private string[] buttonNames = new string[] { "Button 1", "Button 2", "Button 3" };
        public string caption = "My Window";  //public - uses in class: ComplexFramelessForm 
        public int drawFigure = 0;  //public - uses in class: ComplexFramelessForm 

        public FramelessForm(int width, int height)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Brown;
            this.TransparencyKey = BackColor;
            this.ShowInTaskbar = false;
            this.DoubleBuffered = true;
            this.Width = width;
            this.Height = height;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, 50);
            circleRed = new Rectangle(this.ClientSize.Width - 22, 2, 20, 20);
            circleBlue = new Rectangle(2, 2, 20, 20);
            this.MouseDown += new MouseEventHandler(FramelessForm_MouseDown);
            this.MouseMove += new MouseEventHandler(FramelessForm_MouseMove);
            this.MouseUp += new MouseEventHandler(FramelessForm_MouseUp);
            this.Resize += new EventHandler(FramelessForm_Resize);
            InitializeButtons(buttonNames);
        }

        private void FramelessForm_Resize(object sender, EventArgs e)
        {
            circleRed = new Rectangle(this.ClientSize.Width - 22, 2, 20, 20);
            circleBlue = new Rectangle(2, 2, 20, 20);
            drawFigure = 0;
            var buttonsToRemove = new List<Control>();
            foreach (Control control in this.Controls)
            {
                if (control is Button)
                {
                    buttonsToRemove.Add(control);
                }
            }
            foreach (var button in buttonsToRemove)
            {
                this.Controls.Remove(button);
            }
            InitializeButtons(buttonNames);
        }

        protected void InitializeButtons(string[] buttonNames) //must be protected, because we call it from ComplexFramelessForm 
        {
            this.buttonNames = buttonNames;
            // Make new buttons
            int buttonWidth = 110;
            int buttonHeight = 30;
            int buttonSpacing = 20;
            int totalWidth = (buttonWidth + buttonSpacing) * buttonNames.Length - buttonSpacing;
            int startX = (this.ClientSize.Width - totalWidth) / 2;
            int startY = this.ClientSize.Height - buttonHeight - 10; // Adjusted Y position 
            for (int i = 0; i < buttonNames.Length; i++)
            {
                Button button = new Button
                {
                    Text = buttonNames[i],
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(startX + i * (buttonWidth + buttonSpacing), startY),
                    BackColor = SystemColors.ButtonFace,
                    FlatStyle = FlatStyle.Flat,
                    Tag = i + 1 // Assign tag to identify button
                };
                button.Click += Button_Click;
                this.Controls.Add(button);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                drawFigure = (int)button.Tag;
                this.Invalidate(); // method is used to request a repaint of the control 
            }
        }

        private void FramelessForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (circleRed.Contains(e.Location))
            {
                Application.Exit();
            }
            if (circleBlue.Contains(e.Location))
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, 50); 
                }
                else
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                this.Invalidate();
            }
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void FramelessForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void FramelessForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e); // Request to repaint the entire form
            drawFigure = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = new GraphicsPath())
            {
                int rr = 40;  // diameter
                int off = 16;  // offset
                path.StartFigure();
                path.AddArc(off, off, rr, rr, 180, 90);
                path.AddArc(this.ClientSize.Width - off - rr, off, rr, rr, 270, 90);
                path.AddArc(this.ClientSize.Width - off - rr, this.ClientSize.Height - 2 - rr, rr, rr, 0, 90);
                path.AddArc(off, this.ClientSize.Height - 2 - rr, rr, rr, 90, 90);
                path.CloseFigure();
                g.FillPath(paintForm, path);
                using (Pen pen = new Pen(Color.DarkViolet, 3))
                {
                    g.DrawPath(pen, path);
                }
            }
            Rectangle topBox = new Rectangle((this.ClientSize.Width - 300) / 2, 1, 300, 30);
            g.FillRectangle(paintForm, topBox);
            using (Pen purplePen = new Pen(Color.Purple, 3))
            {
                g.DrawRectangle(purplePen, topBox);
            }
            using (Font font = new Font("Arial", 13, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.Black))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(caption, font, textBrush, topBox, sf);
            }
            g.FillEllipse(Brushes.Red, circleRed);
            using (Pen pen = new Pen(Color.DarkViolet, 2))
            {
                g.DrawEllipse(pen, circleRed);
            }
            g.FillEllipse(Brushes.Blue, circleBlue);
            using (Pen pen = new Pen(Color.DarkViolet, 2))
            {
                g.DrawEllipse(pen, circleBlue);
            }
        }
    }
}

