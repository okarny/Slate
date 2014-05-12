using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

//  CONVERT THIS TO A SERIALIZABLE OBJECT SO THAT WE CAN USE IT FOR DATAGRIDVIEW DATASOURCES

namespace Slate
{
    public enum Status
    {
        Connected, Disconnected, Registered
    };

    public class SlateClient
    {
        public TcpClient _client = null;
        public TcpClient client { get { return _client; } set { _client = value; } }
        public DateTime _start;
        public DateTime start { get { return _start; } set { _start = value; } }
        public bool _isRegistered = false;
        public bool isRegistered { get { return _isRegistered; } set { _isRegistered = value; } }
        public bool _isAuthenticated = false;
        public bool isAuthenticated { get { return _isAuthenticated; } set { _isAuthenticated = value; } }
        public String _name = null;
        public String name { get { return _name; } set { _name = value; } }
        public String _code = null;
        public String code { get { return _code; } set { _code = value; } }
        public int _status = 0;
        public int status { get { return _status; } set { _status = value; } }
        public bool _isSessionOperator;
        public bool isSessionOperator { get { return _isSessionOperator; } set { _isSessionOperator = value; } }
        public String _email;
        public String email { get { return _email; } set { _email = value; } }
        //public Status status = new Status();

        public SlateClient()
        {
            isSessionOperator = false;
        }

        public SlateClient(string name, string code)
        {
            this.name = name;
            this.code = code;
        }
    }
}
