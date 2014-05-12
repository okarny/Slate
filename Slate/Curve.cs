using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Slate
{
    public class Curve
    {
        public List<Point> points;
       // public Pen pen;
        public float width;
        public Color color;
        public string colorString;
        public Rectangle boundingRect;

        public Curve()
        {
            points = new List<Point>();
           // pen = new Pen(Brushes.Black);
            color = Color.Black;
            width = 1.0f;
            colorString = "Black";
            boundingRect = new Rectangle(0, 0, 0, 0);
        }
    }
}
