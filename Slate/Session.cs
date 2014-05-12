using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slate
{
    public class Session
    {
        public Document document;
        public DateTime lastAccessed;
        public String sessionName;
        public String sessionID;
        public List<RegObject> guests = new List<RegObject>();

        public Session()
        {
            document = new Document();
        }
    }
}
