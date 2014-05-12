using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace Slate
{
    public class RegObject
    {

        public RegObject()
        {
            
        }
        
        public RegObject(String name, String email, String code)
        {
            this.name = name;
            this.email = email;
            this.code = code;
        }

        public int id;
        [XmlIgnore] public String _name;
        public String name { get { return _name; } set { _name = value; } }
        [XmlIgnore] public String _address;
        public String address { get { return _address; } set { _address = value; } }
        [XmlIgnore] public String _code;
        public String code { get { return _code; } set { _code = value; } }
        [XmlIgnore] public String _status;
        public String status { get { return _status; } set { _status = value; } }
        [XmlIgnore] public String _email;
        public String email { get { return _email; } set { _email = value; } }
        [XmlIgnore] public bool _sessionOperator;
        public bool sessionOperator { get { return _sessionOperator; } set { _sessionOperator = value; } }
        [XmlIgnore] public TcpClient _client;
        [XmlIgnore] public TcpClient client { get { return _client; } set { _client = value; } }
        

    }
}
