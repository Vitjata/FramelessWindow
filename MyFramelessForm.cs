using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MyFramelessApp
{
    public class MyFramelessForm : FramelessForm
    {

        public MyFramelessForm(int width, int height) : base(width, height)
        {
            caption = "My Frameless Window";
            string[] buttonNames = { "Circle", "Hello", "To Do" };
            InitializeButtons(buttonNames);           
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int clientWidth = this.ClientSize.Width;
            int clientHeight = this.ClientSize.Height;
            switch (drawFigure)
            {
                case 0:
                    // No figure
                    break;
                case 1:
                    Brush yellowBrush = new SolidBrush(Color.Yellow); 
                    int x = (clientWidth - 200) / 2; 
                    int y = (clientHeight - 200) / 2; 
                    g.FillEllipse(yellowBrush, x, y, 200, 200);
                    break;
                case 2:
                    string text = "Hello, World!"; 
                    Font font = new Font("Arial", 22); 
                    Brush textBrush = new SolidBrush(Color.Black); 
                    SizeF textSize = g.MeasureString(text, font); 
                    float textX = (this.ClientSize.Width - textSize.Width) / 2; 
                    float textY = (this.ClientSize.Height - textSize.Height) / 2;
                    g.DrawString(text, font, textBrush, textX, textY);
                    break;
                case 3:
                    // To Do
                    break;
            }
        }
    }
}
