using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Slate
{
    public class Text
    {
        public String content = "";
        public Font font = new System.Drawing.Font("Calibri", 10);
        public Color color = Color.Black;
        public Point location = new Point(0,0);

        public Text()
        {
            //font = Drawin
        }

        public void setLocation(int x, int y)
        {
            location = new Point(x, y);
        }
    }
}
