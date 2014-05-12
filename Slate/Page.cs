using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Slate
{
    public class Page
    {
        public List<Curve> objects;
        public int index;
        public DateTime date;
        
        public Page()
        {
            objects = new List<Curve>();
        }

        public Page(int index)
        {
            objects = new List<Curve>();
            this.index = index;
        }

        public Page(int index, DateTime date)
        {
            objects = new List<Curve>();
            this.index = index;
            this.date = date;
        }

        public void addObject(Curve obj)
        {
            /*
            if (obj.GetType() == typeof(Curve))
            {

            }*/
            Console.WriteLine(obj.color);
            objects.Add(obj);
        }

        public List<Curve> getObjectsOfType(Type type)
        {
            List<Curve> objs = new List<Curve>();

            foreach (Curve obj in objects)
            {
                if (obj.GetType() == type)
                {
                    objs.Add(obj);
                }
            }

            return objs;
        }
    }
}
