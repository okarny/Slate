using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slate
{
    public class GroupObject
    {
         
        public GroupObject(String name, List<RegObject> clients)
        {
            this.name = name;
            this.clients = clients;
        }

        public GroupObject(String name)
        {
            this.name = name;
            this.clients = new List<RegObject>();
        }

        public int id;
        public String _name;
        public String name { get { return _name; } set { _name = value; } }
        public List<RegObject> _clients;
        public List<RegObject> clients { get { return _clients; } set { _clients = value; } }
        
    }
}
