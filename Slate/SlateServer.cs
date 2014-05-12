using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using System.Collections.Concurrent;
//#define DEBUG

namespace Slate
{
    public class SlateServer
    {

        const int REGISTRATION_TIMEOUT = 30;        //  Registration timeout in seconds

        const byte DOC_WIDTH        = (byte)0;
        const byte DOC_HEIGHT       = (byte)1;

        const byte REG              = (byte)2;
        const byte REG_SUCCESS      = (byte)3;
        const byte REG_NOT_FOUND    = (byte)4;
        const byte REG_TIMEOUT      = (byte)5;

        const byte PG               = (byte)6;
        const byte OBJ              = (byte)7;
        const byte WIDTH            = (byte)8;
        const byte COLOR            = (byte)9;
        const byte PTS              = (byte)10;
        const byte ENDPTS           = (byte)11;
        const byte ENDOBJ           = (byte)12;
        const byte ENDPG            = (byte)13;

        const byte NXT              = (byte)14;
        const byte PRV              = (byte)15;
        const byte FST              = (byte)16;
        const byte LST              = (byte)17;
        const byte GOTO             = (byte)18;
        const byte NWPG             = (byte)19;

        const byte SO_GRANT         = (byte)20;
        const byte SO_ACCEPT        = (byte)21;
        const byte SO_REJECT        = (byte)22;
        const byte SO_REVOKE        = (byte)23;
        const byte SO_WIDTH         = (byte)24;
        const byte SO_HEIGHT        = (byte)25;
        const byte SO_READY         = (byte)26;
        const byte SO_DONE          = (byte)27;

        const byte TSO_REQUEST      = (byte)28;
        const byte TSO_REQUEST1     = (byte)29;
        const byte TSO_GRANT        = (byte)30;
        const byte TSO_ACCEPT       = (byte)31;
        const byte TSO_REJECT       = (byte)32;
        const byte TSO_REVOKE       = (byte)33;
        const byte TSO_WIDTH        = (byte)34;
        const byte TSO_HEIGHT       = (byte)35;
        const byte TSO_READY        = (byte)36;
        const byte TSO_DONE         = (byte)37;


        private const int message_buffer = 4096;
        IPAddress address;                          // Server IP address.
        int port;                                   // Server PORT number.
        //public List<SlateClient> clients = new List<SlateClient>();          // List of SlateClient clients in the server.
        private TcpListener tcpListener;            // TCP listener.
        private Thread listenThread;                // Thread for server to listen on.
        public static volatile bool isListening = false;    //  Server is listening
        public static volatile bool isRunning = false;      //  Server is running
        public Canvas canvas;
        public frmMain parent;
        private bool shouldListen = false;
        private List<Thread> threads = new List<Thread>();
        public List<TcpClient> clients = new List<TcpClient>();
        //public List<TcpClient> timedOutClients = new List<TcpClient>();

        private int so_height, so_width;
        private int tso_height, tso_width;
        private bool sessionOperatorReady;
        private bool temporarySessionOperatorReady;
        private Queue<TcpClient> potentialTSOs = new Queue<TcpClient>();
        private bool sessionOperatorAwaitingTSORequestApproval = false;
        public TcpClient sessionOperator;
        public TcpClient temporarySessionOperator;
        private bool getPts = false;
        public bool requiresRegUponConnect = false;
        SlateClient currentSlateClient;

        public Session currentSession;
        public int doc_width;
        public int doc_height;

        private int state = 0;

        private Queue<String> commands = new Queue<String>();

        ASCIIEncoding encoder;

        public static DateTime regTime;

        public List<List<RegObject>> groups = new List<List<RegObject>>();
        public bool groupWorkActivated = false;


        public struct ClientTimeout
        {
            public TcpClient client;
            public int timeout;
            public int attempts;
        }

        //static volatile List<ClientTimeout> clientTimeouts = new List<ClientTimeout>();
        ConcurrentBag<ClientTimeout> clientTimeouts = new ConcurrentBag<ClientTimeout>();
        //BlockingCollection<ClientTimeout> clientTimeouts = new BlockingCollection<ClientTimeout>();
        Thread timeoutThread;

        public SlateServer()
        {
            address = IPAddress.Any;        // Collect any ip address.
            port = 6000;                    // Default IP address

            isRunning = true;
            //tcpListener = new TcpListener(address, port);
        }

        public SlateServer(String addr, int prt)
        {
            address = IPAddress.Parse(addr);
            port = prt;
        }

        public SlateServer(IPAddress addr, int prt)
        {
            address = addr;
            port = prt;
        }

        public void pollTimeouts()
        {
            //  Start the timeout thread only if it currently isn't and if there is at least 1 client to timeout.
            if (timeoutThread == null && clientTimeouts.Count > 0)
            {
                timeoutThread = new Thread(new ThreadStart(maintainTimeouts));
                timeoutThread.Start();
            }
        }

        public void addClientTimeout(TcpClient client, int timeout, int attempts)
        {
            ClientTimeout clientTimeout = new ClientTimeout();
            clientTimeout.client = client;
            clientTimeout.timeout = timeout;
            clientTimeout.attempts = attempts;

            clientTimeouts.Add(clientTimeout);

            pollTimeouts();
        }

        public void removeClientFromTimeout(TcpClient client)
        {
            Console.WriteLine("inside");
            ClientTimeout clientTimeout;
            for (int i=0;i<clientTimeouts.Count;i++)
            {
                clientTimeout = clientTimeouts.ElementAt(i);
                if (clientTimeout.client == client)
                {
                    while (clientTimeouts.TryTake(out clientTimeout));
                    break;
                }
            }
        }

        public void updateClientTimeout(TcpClient client, int timeout)
        {
            Console.WriteLine("updating client");
            ClientTimeout clientTimeout;
            int currentAttempts = 3;
            for (int i = 0; i < clientTimeouts.Count; i++)
            {
                clientTimeout = clientTimeouts.ElementAt(i);
                if (clientTimeout.client == client)
                {
                    currentAttempts = clientTimeout.attempts;
                    while (clientTimeouts.TryTake(out clientTimeout));
                    if (currentAttempts > 1)
                    {
                        addClientTimeout(client, REGISTRATION_TIMEOUT, currentAttempts - 1);
                        sendMessageToClient(client, "reg-not-found\r\n");
                    }
                    else
                    {
                        sendMessageToClient(client, "reg-failure\r\n");
                        clients.Remove(client);
                        client.Close();
                    }
                    //clientTimeout.timeout = timeout;
                    break;
                }
            }
        }

        public void maintainTimeouts()
        {
            ClientTimeout clientTimeout;
            while (clientTimeouts.Count > 0)
            {
                for (int i = 0; i < clientTimeouts.Count; i++)
                {
                    clientTimeout = clientTimeouts.ElementAt(i);

                    if (clientTimeout.timeout == 0)
                    {
                        //  Notify client of timeout
                        sendMessageToClient(clientTimeout.client, "reg-timeout\r\n");
                        clientTimeout.client.Close();
                        clients.Remove(clientTimeout.client);

                        //  Remove timeout from list
                        while (clientTimeouts.TryTake(out clientTimeout)) ;
                    }
                    else
                    {
                        clientTimeouts.TryTake(out clientTimeout);
                        clientTimeout.timeout--;
                        clientTimeouts.Add(clientTimeout);
                    }
                }
                Thread.Sleep(1000);
            }
            timeoutThread = null;
        }

        
        public void beginListening()
        {
            // Start listening.
            tcpListener = new TcpListener(address, port);
            listenThread = new Thread(new ThreadStart(listen));
            listenThread.Start();

            shouldListen = true;

            // Initialize client list.
            clients = new List<TcpClient>();
        }

        public int connectedClients()
        {
            int connected = 0;
            foreach (RegObject obj in parent.regBS.List)
                connected += (obj.status.Equals("Connected") || obj.status.Equals("Registered")) ? 1 : 0;
            return connected;
        }

        public void disconnectAllClients()
        {
            foreach (TcpClient client in clients)
            {
                foreach (RegObject obj in parent.regBS.List)
                {
                    if (obj.client == client)
                    {
                        transmitMessageToAllConnectedClients("disconnect\r\n");
                        obj.client = null;
                        obj.status = "Disconnected";
                    }
                    client.Close();
                }
            }
        }

        public void stopServer()
        {
            //  Disconnect all connected clients
            disconnectAllClients();

            //  Stop the TCP from listening
            tcpListener.Stop();

            //  Abort the listen thread
            listenThread.Abort();
        }

        void doAcceptClient(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on  
            // the console.
            TcpClient client = listener.EndAcceptTcpClient(ar);

           // SlateClient clt;
            // Add client to list.
            if (client != null)
            {
             //   clt = new SlateClient();
               // clt.client = client;
                clients.Add(client);

                // If there is a problem here, move these two lines of code to outside of this nest.
                Thread clientThread = new Thread(new ParameterizedThreadStart(handleClientComm));
                clientThread.Start(client);
            }
        }

        public void sendMessageToClient(TcpClient client, String msg)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            if (client.Connected)
            {
                NetworkStream clientStream = client.GetStream();

                byte[] message = new byte[4096];
                message = encoder.GetBytes(msg);
                clientStream.Write(message, 0, Encoding.UTF8.GetByteCount(msg));
            }
        }

        public void transmitMessageToAllConnectedClientsExcept(String msg, params TcpClient[] clints)
        {
            if (clients.Count() > 0)
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                foreach (TcpClient clt in clients)
                {
                    bool flag = true;
                    foreach (TcpClient clt1 in clints)
                    {
                        if (clt == clt1)
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        TcpClient client = clt;
                        if (client.Connected)
                        {
                            NetworkStream clientStream = client.GetStream();

                            byte[] message = new byte[4096];
                            message = encoder.GetBytes(msg);
                            clientStream.Write(message, 0, Encoding.UTF8.GetByteCount(msg));
                        }
                    }

                }
            }
        }

        public void transmitMessageToAllConnectedClients(String msg)
        {
            if (clients.Count() > 0)
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                foreach (TcpClient clt in clients)
                {
                    if (clt != sessionOperator)
                    {
                        TcpClient client = clt;
                        if (client.Connected)
                        {
                            NetworkStream clientStream = client.GetStream();

                            byte[] message = new byte[4096];
                            message = encoder.GetBytes(msg);
                            clientStream.Write(message, 0, Encoding.UTF8.GetByteCount(msg));
                        }
                    }
                }
            }
        }

        public void transmitEncodedMessageToNonOperators(byte[] message)
        {
            if (clients.Count() > 0)
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                foreach (TcpClient client in clients)
                {
                    if (client != sessionOperator)
                    {
                        NetworkStream clientStream = client.GetStream();

                        clientStream.Write(message, 0, message.Length);
                    }
                }
            }
        }

        public void listen()
        {
            //   Server started listening
            this.tcpListener.Start();
            isListening = true;

            Console.WriteLine("Starting to listen");

            while (shouldListen)
            {
                Console.WriteLine("Listening...");
 

                // Wait until client attempts to connect.
               // IAsyncResult ar = tcpListener.BeginAcceptTcpClient(new AsyncCallback(doAcceptClient), tcpListener);
                if (isListening)
                {
                    TcpClient client = null;
                    try
                    {
                        client = tcpListener.AcceptTcpClient();
                        Console.WriteLine("accepted new client");
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e);
                    }

                    // Add client to list.
                    if (client != null)
                    {
                        if (requiresRegUponConnect)
                        {
                            //  Require registraion from user.
                            sendMessageToClient(client, "reg\r\n");

                            //  Add client to global timeout list
                            addClientTimeout(client, REGISTRATION_TIMEOUT, 3);
                           
                        }
                        clients.Add(client);

                        // If there is a problem here, move these two lines of code to outside of this nest.
                        Thread clientThread = new Thread(new ParameterizedThreadStart(handleClientComm));
                        clientThread.Start(client);
                    }
                }
            }

            Console.WriteLine("End of listening");

            isListening = false;
        }

        private void timeOutReg(System.Timers.Timer sender, ElapsedEventArgs e, TcpClient client)
        {
            bool shouldTimeout = true;
            foreach (RegObject obj in parent.regBS.List)
            {
                if (obj.status != null && obj.client !=null)
                {
                    if (obj.status.Equals("Connected") && obj.client.Equals(client))
                    {
                        shouldTimeout = false;
                        break;
                    }
                }
            }

            if (shouldTimeout)
            {
                sendMessageToClient(client, "reg-timeout\r\n");
                //timedOutClients.Add(client);
                clients.Remove(client);
                client.ReceiveTimeout = -1;
                client.Close();
            }
            sender.Dispose();
        }

        /*
        private GroupObject groupForClient(SlateClient client)
        {
            foreach (GroupObject group in groups)
                foreach (RegObject reg in group.clients)
                    if (reg.address.Equals(client.client.Client.RemoteEndPoint))
                        return group;
            return null;
        }*/

        private bool isClientRegistered(TcpClient client)
        {
            foreach (RegObject obj in parent.regBS.List)
            {
                if (obj.client == client)
                    return true;
            }
            return false;
        }

        private void handleClientComm(object client)
        {
            Console.WriteLine("handling some communication...");
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            String command = "";

            byte[] message = new byte[message_buffer];
            int bytesRead;

            while (isRunning)
            {
                bytesRead = 0;

               /*
                if (tcpClient.ReceiveTimeout == -1)
                {
                    sendMessageToClient(tcpClient, "timeout\r\n");
                    tcpClient.Close();
                    return;
                }*/
                try
                {
                    // Blocks until message is received
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;  // Error
                }

                if (bytesRead == 0)
                {
                    // The client has disconnected from the server
                    //sclients.Remove(currentSlateClient);                     // This should resolve the reconnection problem.
                    if (tcpClient == sessionOperator)
                    {

                        sessionOperator = null;
                        sessionOperatorReady = false;
                    }

                    if (tcpClient == temporarySessionOperator)
                    {
                        temporarySessionOperator = null;
                        temporarySessionOperatorReady = false;
                    }

                    sendMessageToClient(tcpClient, "disconnecting\r\n");

                    foreach (RegObject reg in parent.regBS.List)
                    {
                       // if (reg.client != null)
                       // {
                            Console.WriteLine("Checking client");
                        if (tcpClient.Connected && reg.client != null)
                        {
                            if (reg.client.Equals(tcpClient))
                            {
                                reg.status = "Disconnected";
                                if (reg.sessionOperator)
                                    reg.sessionOperator = false;
                                parent.Invoke(new MethodInvoker(delegate
                                {
                                    parent.updateRegistrationWindow();
                                }));
                                break;
                            }
                        }
                    }
                    clients.Remove(tcpClient);
                    break;
                }


                try {

                    /*
                    foreach (TcpClient clnt in timedOutClients)
                    {
                        if (clnt == client)
                        {
                            state = 0;
                        }
                    }*/

                    //  Incoming data
                    encoder = new ASCIIEncoding();
                    String read = encoder.GetString(message, 0, bytesRead);
                    String stra = encoder.GetString(message, 0, bytesRead);

                    //  Add to buffer
                    //receivedData += read;
                    /*
                    if (tcpClient == sessionOperator && sessionOperatorReady)
                    {
                        Console.WriteLine("Echoing to non operators.");
                        transmitEncodedMessageToNonOperators(message);
                    }*/

                    //  Trim end
                    stra = stra.TrimEnd('\r', '\n');
                    read = read.TrimEnd('\r', '\n');
                    Console.WriteLine("Client: " + stra);

                    String[] lines;
                    //  Through each line into the queue
                    foreach (String line in read.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                        commands.Enqueue(line);

                    if (!isClientRegistered(tcpClient) && requiresRegUponConnect)
                    {
                        state = 3;
                    }

                    //Console.WriteLine("reg started");

                    /*
                    timeout = (new DateTime() - start);     // Update timeout.
                    Console.WriteLine("seconds " + timeout.Seconds);

                    if (regTimeoutStarted && timeout.Seconds >= 10)
                    {
                        sendMessageToClient(tcpClient, "reg-timeout\r\n");
                        tcpClient.Close();
                    }*/

                    //  Determine if server is awaiting SO approval of tSO request.
                    if (tcpClient == sessionOperator && sessionOperatorAwaitingTSORequestApproval)
                        state = 4;

                    while (commands.Count > 0)
                    {
                        command = commands.Dequeue();

                        switch (state)
                        {
                            case 0:

                                //  State 0
                                //  General command state

                                if (command.Equals("quit"))
                                {
                                    disconnectClient(tcpClient);
                                    return;
                                }

                                if (command.Contains("obj Curve"))
                                {
                                    if (tcpClient == sessionOperator || tcpClient == temporarySessionOperator)
                                    {
                                        Console.WriteLine("Creating a line");
                                        parent.currentCurve = parent.newCurve();
                                    }
                                }
                                /*
                                if (read.Contains("obj Curve"))
                                {
                                    if (tcpClient == sessionOperator)
                                        parent.currentCurve = parent.newCurve();
                                    //receivedData = receivedData.Substring(receivedData.IndexOf("\r\n"));
                                }*/
                                //if (read.Contains("color"))
                                //   parent.currentCurv
                                if (command.Contains("pts"))
                                {
                                    state = 1;
                                    // receivedData = receivedData.Substring(receivedData.IndexOf("\r\n"));
                                }
                                if (command.Equals("accept-mso"))
                                {
                                    //  If there is currently a session operator, notify it of duties relieved.
                                    if (sessionOperator != null)
                                    {
                                        sendMessageToClient(sessionOperator, "so-finish\r\n");
                                    }
                                    sessionOperatorReady = false;
                                    sessionOperator = tcpClient;
                                    state = 2;
                                    //receivedData = receivedData.Substring(receivedData.IndexOf("\r\n"));
                                    //continue;
                                }

                                if (command.Equals("tso-accept"))
                                {
                                    if (tcpClient == temporarySessionOperator)
                                    {
                                        temporarySessionOperatorReady = false;
                                        state = 2;
                                    }
                                 //   continue;
                                }

                                if (command.Contains("reject-mso"))
                                {
                                    parent.notifyRegOfSessionOperator(tcpClient);
                                  //  continue;
                                }

                                if (command.Equals("so-ready"))
                                {
                                    sessionOperatorReady = true;
                                    //continue;
                                }
                                if (command.Equals("tso-ready"))
                                {
                                    temporarySessionOperatorReady = true;
                                    //continue;
                                }

                                //  Request from temporary session operator
                                if (command.Equals("tso-request"))
                                {
                                    String name = null;

                                    //  Locate current registered user's information
                                    foreach (RegObject obj in parent.regBS.List)
                                    {
                                        if (obj.client.Client.RemoteEndPoint == tcpClient.Client.RemoteEndPoint)
                                        {
                                            name = obj.name;
                                            break;
                                        }
                                    }

                                    //  Notify current session operator of tSO request
                                    sendMessageToClient(sessionOperator, "tso-request:" + name + "\r\n");

                                    //  Add client to queue of potential TSOs
                                    potentialTSOs.Enqueue(tcpClient);

                                    //  Send to state 4 where it will await SO approval.
                                    sessionOperatorAwaitingTSORequestApproval = true;

                                    //continue;
                                }

                                if (command.Equals("tso-revoke"))
                                {
                                    sendMessageToClient(temporarySessionOperator, "tso-finish\r\n");
                                    continue;
                                }

                                if (tcpClient == sessionOperator)
                                {
                                    if (command.Contains("nxt"))
                                        nextPage(false);
                                    if (command.Contains("prv"))
                                        previousPage(false);
                                    if (command.Contains("fst"))
                                        firstPage(false);
                                    if (command.Contains("lst"))
                                        lastPage(false);
                                    if (command.Contains("goto"))
                                    {
                                        short number = short.Parse(command.Substring("goto:".Length));
                                        gotoPage(number, false);
                                    }
                                }
                                break;
                            case 1:
                                if (command.Contains("endpts"))
                                {
                                    if (parent == null)
                                        Console.WriteLine("parent is null");
                                    if (parent.currentCurve == null)
                                        Console.WriteLine("parent current curve is null");
                                    if (parent.currentPage == null)
                                        Console.WriteLine("parent is current page null");
                                    //Console.WriteLine("page count: "+currentSession.document.pages.Count);
                                    parent.currentSession.document.pages[parent.currentPage - 1].addObject(parent.currentCurve);
                                    parent.currentCurve = null;
                                    parent.Invoke(new MethodInvoker(delegate
                                    {
                                        parent.refreshDisplay();
                                    }));
                                    state = 0;
                                    break;
                                }

                                if (tcpClient == sessionOperator || tcpClient == temporarySessionOperator)
                                {
                                    // Draw to screen
                                    String result;
                                    lines = read.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                    //while (commands.Count > 0)
                                    //{
                                    //command = commands.Dequeue();
                                    if (command.Contains("endpts") || command.Contains("endobj"))
                                        break;
                                    if (command.Contains("pts"))
                                        break;
                                    if (!command.Contains(","))
                                    {
                                        state = 0;
                                        break;
                                    }

                                    if (command.Length > 0)
                                    {

                                        String[] pts = command.Split(',');

                                        //Console.WriteLine("so x " + pts[0]);
                                        //Console.WriteLine("so y " + pts[1]);

                                        int x = Convert.ToInt16(pts[0]);
                                        int y = Convert.ToInt16(pts[1]);

                                        if (tcpClient == sessionOperator)
                                        {
                                            x = (int)(((double)x / so_width) * parent.doc_width);
                                            y = (int)(((double)y / so_height) * parent.doc_height);
                                        }
                                        else if (tcpClient == temporarySessionOperator)
                                        {
                                            x = (int)(((double)x / tso_width) * parent.doc_width);
                                            y = (int)(((double)y / tso_height) * parent.doc_height);
                                        }


                                        //Console.WriteLine("sox: "+x);
                                        // Console.WriteLine("soy: "+y);

                                        Point point = new Point(x, y);
                                        // if (parent.currentCurve != null)
                                        //{
                                        //Console.WriteLine(point);
                                        parent.currentCurve.points.Add(point);
                                        parent.Invoke(new MethodInvoker(delegate
                                        {
                                            parent.refreshDisplay();
                                        }));
                                        //}

                                        //Console.WriteLine("Operator: "+read);

                                        //  Draw to screen.


                                        result = String.Format("{0},{1}", x, y);
                                        command = result;
                                        Console.WriteLine("result: " + result);
                                        // command = result;

                                    }
                                }

                                break;
                            case 2:
                                
                                //  State 2
                                //  Make proper preparations for session and temporary session operators.

                                if (tcpClient == temporarySessionOperator)
                                {
                                    if (command.Contains("tso-width:"))
                                        tso_width = Convert.ToInt16(command.Substring("tso-width:".Length));
                                    if (command.Contains("tso-height:"))
                                        tso_height = Convert.ToInt16(command.Substring("tso-height:".Length));

                                    if (command.Equals("tso-ready"))
                                    {
                                        temporarySessionOperatorReady = true;
                                        state = 0;
                                    }
                                }

                                if (tcpClient == sessionOperator)
                                {
                                    if (command.Contains("so-width:"))
                                        so_width = Convert.ToInt16(command.Substring("so-width:".Length));
                                    if (command.Contains("so-height:"))
                                        so_height = Convert.ToInt16(command.Substring("so-height:".Length));

                                    if (command.Equals("so-ready"))
                                    {
                                        sessionOperatorReady = true;
                                        state = 0;
                                    }
                                }

                                break;

                            case 3:

                                //  State 3
                                //  Registration state

                                //  Next command expected is a code. Check for that code in the reg db.
                                Console.WriteLine("Checking reg");
                                bool shouldContinue = true;
                                bool codeFound = false;
                                foreach (RegObject rclient in parent.regBS)
                                {
                                    Console.WriteLine(String.Format("Checking for code: {0}", rclient.code));
                                    if (command.Equals(rclient.code))
                                    {
                                        Console.WriteLine("Found code");
                                        Console.WriteLine(String.Format("Checking for ip: {0}", tcpClient.Client.RemoteEndPoint));
                                        /*foreach (TcpClient clt in clients)
                                        {
                                            if (clt != null)
                                            {
                                                if (tcpClient.Client.RemoteEndPoint == clt.Client.RemoteEndPoint)
                                                {*/
                                                    rclient.status = "Connected";
                                                    rclient.client = tcpClient;
                                                    rclient.address = tcpClient.Client.RemoteEndPoint.ToString();
                                                    //rclient.status = 0;
                                                    sendMessageToClient(tcpClient, "reg-success\r\n");

                                                    //  Consider moving this to its own functio

                                                    //  Send session initialization methods
                                                    sendMessageToClient(tcpClient, "doc-width:" + doc_width + "\r\n");
                                                    sendMessageToClient(tcpClient, "doc-height:" + doc_height + "\r\n");

                                                    state = 0;
                                                    //synchronizeClient(tcpClient);

                                                    removeClientFromTimeout(tcpClient);
                                                    codeFound = true;
                                                    parent.Invoke(new MethodInvoker(delegate
                                                    {
                                                        parent.updateRegistrationWindow();
                                                    }));

                                                    shouldContinue = false;
                                                    break;
                                        /*
                                                }
                                            }

                                            if (!shouldContinue)
                                                break;
                                        }
                                        if (!shouldContinue)
                                            break;*/
                                    }
                                }

                                if (!codeFound)
                                {
                                    Console.WriteLine("REG TIMEOUT RESATDSF");
                                    updateClientTimeout(tcpClient, REGISTRATION_TIMEOUT);
                                }
                                break;


                            case 4:
                                
                                //  State 4
                                //  Server is awaiting SO approval of tSO request

                                if (command.Contains("tso-granted"))
                                {
                                    grantTSO();
                                   // continue;
                                }

                                if (command.Contains("tso-denied"))
                                {
                                    denyTSO();
                                  //  continue;
                                }

                                //  If there are no more TSO requests in the queue, leave state.
                                if (potentialTSOs.Count == 0)
                                {
                                    state = 0;
                                    sessionOperatorAwaitingTSORequestApproval = false;      // May be able to remove this if we just rely on the TSO request queue.
                                }

                                break;

                            case 5:

                                //  State 5
                                //  Synchronization. Server is waiting for document sync commands.

                                if (command.Equals("sync-complete"))
                                {
                                    state = 0;
                                }


                                break;
                        }

                        //  If incoming message is from session or temporary session operator, pass it through to all other connected devices.
                        if ((tcpClient == sessionOperator && sessionOperatorReady && !command.Contains("so-ready") && !command.Contains("tso-granted") && !command.Contains("tso-revoke")))
                        {
                            Console.WriteLine("Pushing from session operator");
                            if (command.Length > 0)
                                transmitMessageToAllConnectedClients(command + "\r\n");
                        }

                        if (tcpClient == temporarySessionOperator && temporarySessionOperatorReady && !command.Contains("tso-ready"))
                        {
                            Console.WriteLine("Pushing from temp session operator");
                            if (command.Length > 0)
                                transmitMessageToAllConnectedClientsExcept(command + "\r\n", temporarySessionOperator);
                        }
                    }

                    //  IF GROUP WORK IS ACTIVATED, REROUTE DATA TO RESPECTIVE CLIENTS
                    if (groupWorkActivated)
                    {
                        /*
                        GroupObject group = groupForClient(sclient);
                        foreach (RegObject obj in group.clients)
                        {

                        }*/

                        foreach (List<RegObject> groupList in groups)
                        {
                            bool isGroup = false;
                            foreach (RegObject obj in groupList)
                                if (obj.client == tcpClient)
                                    isGroup = true;

                            if (isGroup)
                            {
                                foreach (RegObject obj in groupList)
                                {
                                    Console.WriteLine("Sending command to " + obj.name);
                                    //  Send message to all other in group
                                    if (obj.client != tcpClient)
                                        sendMessageToClient(obj.client, command + "\r\n");
                                }
                                break;
                            }
                        }
                    } 

                } catch (Exception e)
                {
                    MessageBox.Show("Details: " + e.ToString(), "An error occurred");
                }
            }


            tcpClient.Close();
        }

        private void synchronizeClient(TcpClient client)
        {
            //  Send client synchronization information.
            sendMessageToClient(client, "start-sync\r\n");


            //  Send number of pages


            //  Finish synchronization
            sendMessageToClient(client, "end-sync\r\n");
        }

        private bool clientInGroup(ref TcpClient client, ref List<RegObject> group)
        {
            foreach (RegObject obj in group)
            {
                if (client.Equals(obj.client))
                    return true;
            }

            return false;
        }

        private void disconnectClient(TcpClient client)
        {
            foreach (RegObject obj in parent.regBS.List)
            {
                if (obj.client == client)
                {
                    obj.client = null;
                    obj.status = "Disconnected";
                }
            }

            client.Close();
        }  

        private void grantTSO()
        {
            //  Dequeue potential TSO from list
            temporarySessionOperator = potentialTSOs.Dequeue();

            //  Deny the client's TSO request
            sendMessageToClient(temporarySessionOperator, "tso-granted\r\n");
        }

        private void denyTSO()
        {
            //  Dequeue potential TSO from list
            TcpClient client = potentialTSOs.Dequeue();

            //  Deny the client's TSO request
            sendMessageToClient(client, "tso-denied\r\n");
        }

        public void makeSessionOperator(TcpClient client)
        {
            sendMessageToClient(client, "mso\r\n");
        }

        public void revokeSessionOperator(TcpClient client)
        {
            sendMessageToClient(client, "so-finish\r\n");
        }

        private void sendEncodedMessageToClient(TcpClient client, byte[] message)
        {
            NetworkStream clientStream = client.GetStream();
            clientStream.Write(message, 0, message.Length);
        }

        private void nextPage()
        {
            nextPage(true);
        }

        private void nextPage(bool shouldTransmit)
        {
            parent.Invoke(new MethodInvoker(delegate
            {
                parent.nextPage(shouldTransmit);
            }));
        }

        private void previousPage()
        {
            previousPage(true);
        }

        private void previousPage(bool shouldTransmit)
        {
            parent.Invoke(new MethodInvoker(delegate
            {
                parent.previousPage(shouldTransmit);
            }));
        }

        private void firstPage()
        {
            firstPage(true);
        }

        private void firstPage(bool shouldTransmit)
        {
            parent.Invoke(new MethodInvoker(delegate
            {
                parent.firstPage(shouldTransmit);
            }));
        }

        private void lastPage()
        {
            lastPage(true);
        }

        private void lastPage(bool shouldTransmit)
        {
            parent.Invoke(new MethodInvoker(delegate
            {
                parent.lastPage(shouldTransmit);
            }));
        }

        private void gotoPage(short number)
        {
            gotoPage(number, true);
        }

        private void gotoPage(short number, bool shouldTransmit)
        {
            parent.Invoke(new MethodInvoker(delegate
            {
                parent.gotoPage(number, shouldTransmit);
            }));
        }
    }
}
