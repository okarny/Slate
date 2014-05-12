using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slate
{
    public class Document
    {
        public List<Page> pages;
        public int numberPages;
        
        public Document()
        {
            pages = new List<Page>();
        }

        public void newPage()
        {
            pages.Add(new Page());
        }
    }
}
