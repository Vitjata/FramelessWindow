# FramelessForm

A custom Windows Forms base class that creates modern, borderless windows with built-in window controls, drag functionality, and customizable button interface.

## Overview

`FramelessForm` is designed as a base class for creating sleek, frameless applications with custom window management. It provides all the essential window functionality while maintaining a modern, borderless appearance.

## Features

### Window Management
- **Frameless Design**: No traditional window borders or title bar
- **Custom Window Controls**: Red circle (close) and blue circle (maximize/restore)
- **Drag Support**: Click and drag anywhere on the form to move the window
- **Transparency**: Uses transparency key for rounded corners
- **Rounded Corners**: Smooth rounded window edges with anti-aliased rendering

### Interface Elements
- **Custom Title Bar**: Centered title box with customizable caption
- **Dynamic Buttons**: Configurable button array at the bottom
- **Responsive Layout**: Elements automatically reposition on window resize
- **Smooth Graphics**: Anti-aliased rendering throughout

### Interaction Features
- **Click-to-Close**: Red circle in top-right corner
- **Toggle Maximize**: Blue circle in top-left corner switches between normal/maximized
- **Button Events**: Each button triggers `drawFigure` state changes
- **Auto-Invalidation**: Automatic repainting when state changes

## Architecture

### Inheritance Structure
```csharp
public class FramelessForm : Form
```

This class is designed to be inherited by other forms that need frameless functionality.

### Key Properties
- `caption` (public): Window title text
- `drawFigure` (public): Current drawing state (0 = none, 1+ = button index)
- `buttonNames`: Array of button labels

### Constructor
```csharp
public FramelessForm(int width, int height)
```

**Parameters:**
- `width`: Window width in pixels  
- `height`: Window height in pixels

**Initialization:**
- Sets up frameless window properties
- Centers window on screen
- Initializes window controls and event handlers
- Creates default button set

## Usage Example

### Basic Implementation
```csharp
// Create a basic frameless form
FramelessForm form = new FramelessForm(800, 600);
form.caption = "My Custom Window";
form.Show();
```

### Custom Inheritance Example
```csharp
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
        
        switch (drawFigure)
        {
            case 1:
                // Draw yellow circle
                Brush yellowBrush = new SolidBrush(Color.Yellow); 
                int x = (ClientSize.Width - 200) / 2; 
                int y = (ClientSize.Height - 200) / 2; 
                g.FillEllipse(yellowBrush, x, y, 200, 200);
                break;
            case 2:
                // Draw "Hello, World!" text
                string text = "Hello, World!"; 
                Font font = new Font("Arial", 22); 
                // ... text rendering code
                break;
        }
    }
}
```

## Key Methods

### Protected Methods (For Inheritance)
- `InitializeButtons(string[] buttonNames)`: Creates button array with custom labels
- `OnPaint(PaintEventArgs e)`: Override this for custom drawing

### Event Handlers
- **Mouse Events**: Handle dragging and window control clicks
- **Button Events**: Process button clicks and update `drawFigure`
- **Resize Events**: Maintain layout on window size changes

### Drawing Pattern
1. Call `base.OnPaint(e)` first
2. Use `switch (drawFigure)` for different states
3. Call `this.Invalidate()` to trigger repaint

### Best Practices
1. Always call `base.OnPaint(e)` in overrides
2. Use `drawFigure` to control custom drawing states
3. Set custom buttons with `InitializeButtons()` in constructor
4. Dispose of custom graphics resources properly