using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Reflection;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Office.Core;

using PluginContracts;


namespace Slate
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            isNewDocument = true;
            regBS = new BindingSource();
            regBS.DataSource = typeof(RegObject);
        }

        public frmMain(Session session)
        {
            currentSession = session;
            InitializeComponent();
            isNewDocument = false;
            regBS = new BindingSource();
            regBS.DataSource = typeof(RegObject);
            foreach (RegObject obj in currentSession.guests)
            {
                obj.status = "Disconnected";
                regBS.Add(obj);
            }
        }

        public bool passwordProtected = false;
        public string sessionPassword = "slate";
        public bool requiresRegUponConnect = true;

        public SlateServer server;
        volatile bool serverRunning = false;
        volatile bool serverListening = false;
        public Welcome welcomeScreen;
        public BindingSource regBS;

        public int doc_width;
        public int doc_height;
        int so_width;
        int so_height;
        bool getPts = false;

        // Socket sListener;
        bool isDrawing;
        bool currentCurveExists = false;
        Progress progress;
        public Session currentSession;
        public string sessionName;
        public String sessionID;
        public string sessionPath;
        public Curve currentCurve;
        Color currentColor;
        double currentWidth = 1.0;
        List<Page> pages;
        List<Curve> curves;
        Point lP;
        DrawingTool selectedTool;
        List<Point> eraserCurve;
        bool isBoardRunning = false;

        TcpClient sessionOperator;
        bool sessionOperatorReady = false;
        TcpListener tcpListener;
        List<TcpClient> clients;
        public List<SlateClient> sclients = new List<SlateClient>();
        public List<GroupObject> groups = new List<GroupObject>();
        public Registration reg;
        AddonMarketplace aoMarketplace;
        Groups groupWindow;
        public int currentPage = 1, totalPages = 1;
        Text currentTextObject = null;
        Dictionary<string, PluginContracts.ISIPlugin> _Plugins = new Dictionary<string, PluginContracts.ISIPlugin>();

        bool isNewDocument = false;
        int smallestX = 0, smallestY = 0, highestX = 0, highestY = 0;
        float aspectRatio = 1;

        bool groupWorkActivated = false;

        private void loadPlugins()
        {
            Console.WriteLine("Loading plugins...");
           // string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\plugins\\";
            string path = Application.UserAppDataPath + "\\plugins\\";
            Console.WriteLine("Plugins Path: " + path);
            if (!Directory.Exists(path))
               return;
            string[] dllFileNames = null;
            if (Directory.Exists(path))
                dllFileNames = Directory.GetFiles(path, "*.dll");

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length); 
            foreach(string dllFile in dllFileNames) 
            { 
                 AssemblyName an = AssemblyName.GetAssemblyName(dllFile); 
                 Assembly assembly = Assembly.Load(an);
                 Console.WriteLine(assembly.FullName);
                 assemblies.Add(assembly); 
            }

            Type pluginType = typeof(PluginContracts.ISIPlugin); 
            ICollection<Type> pluginTypes = new List<Type>(); 
            foreach(Assembly assembly in assemblies) 
            { 
              if(assembly != null) 
              { 
                Type[] types = assembly.GetTypes(); 
                foreach(Type type in types) 
                { 
                  if(type.IsInterface || type.IsAbstract) 
                  { 
                    continue; 
                  } 
                  else 
                  { 
                    if(type.GetInterface(pluginType.FullName) != null) 
                    {
                        Console.WriteLine("adding plugin type: "+type);
                      pluginTypes.Add(type); 
                    } 
                  } 
                } 
              } 
            }

            ICollection<PluginContracts.ISIPlugin> plugins = new List<PluginContracts.ISIPlugin>(pluginTypes.Count);
            foreach (Type type in pluginTypes)
            {
                Console.WriteLine("trying to add");
                PluginContracts.ISIPlugin plugin = (PluginContracts.ISIPlugin)Activator.CreateInstance(type);
                if (plugin == null)
                       Console.WriteLine("pluding is null");
                Console.WriteLine("Adding plugin: "+plugin);
                plugins.Add(plugin);
            }

            if (plugins.Count > 0)
            {
               toolsToolStripMenuItem.DropDownItems.Insert(0, new ToolStripSeparator());
            }
            
            //plugins = PluginLoader.LoadPlugins("Plugins");
            foreach (var item in plugins)
            {
                _Plugins.Add(item.Name, item);
                ToolStripMenuItem dropDown = new ToolStripMenuItem(item.Name);
                dropDown.Click += new System.EventHandler(this.b_Click);

                //toolsToolStripMenuItem.DropDownItems.Add(dropDown);
                toolsToolStripMenuItem.DropDownItems.Insert(0, dropDown);
            }
        }

        private void b_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem b = sender as ToolStripMenuItem;
            if (b != null)
            {
                string key = b.Text;
                //Console.WriteLine("Trying plugin named: "+key);
                if (_Plugins.ContainsKey(key))
                {
                    PluginContracts.ISIPlugin plugin = _Plugins[key];
                    Console.WriteLine("run!");
                    plugin.Do();
                }
            }
        }

        private void startServer()
        {
            serverRunning = true;

            
            tcpListener = new TcpListener(IPAddress.Any, 6000);
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
         
            //  Load plugins
            loadPlugins();

            //  Start server on port 6000 (On another thread)
            server = new SlateServer();
            server.parent = this;
            server.requiresRegUponConnect = requiresRegUponConnect;
            server.doc_width = this.Width;
            server.doc_height = this.Height;
            server.beginListening();
            /*
            server.parent = this;
            //listenThread = new Thread(server.listen);
            listenThread.Start();*/
             
            //  Initialize clients
            clients = new List<TcpClient>();
            sclients = new List<SlateClient>();

            // Init curves.
            if (currentSession == null)
            {
                currentSession = new Session();
                //pages = new List<Page>();
                currentSession.sessionName = sessionName;
                currentSession.sessionID = sessionID;
                server.currentSession = currentSession;
                currentSession.document.newPage();
                curves = new List<Curve>();
            }
            else
            {
                curves = currentSession.document.pages[0].objects;
                sessionName = currentSession.sessionName;
                sessionID = currentSession.sessionID;
                pnlCanvas.Invalidate();
                pnlCanvas.Update();
                totalPages = currentSession.document.pages.Count();
            }
            selectedTool = DrawingTool.Pen;
            reg = new Registration();

            // determine aspect ratio.
            aspectRatio = (pnlCanvas.Size.Width / pnlCanvas.Size.Height);
            Console.WriteLine("w - h: {0} - {1}", pnlCanvas.Size.Width, pnlCanvas.Size.Height);


            //  Set document height and width
            doc_width = this.Width;
            doc_height = this.Height;

            //Console.WriteLine(doc_width);

            //  Post session to remote server.
            if (sessionName.Length > 0)
                postSession(sessionName, LocalIPAddress(), sessionID);
            //Console.WriteLine("local ip = " + LocalIPAddress());

            if (isNewDocument)
            {
                sessionPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Slate\\";
                if (!File.Exists(sessionPath))
                    Directory.CreateDirectory(sessionPath);
                Console.WriteLine(sessionPath);

                string localized = sessionName.Replace(".", "");
                localized = localized.Replace("-", "");
                localized = localized.Replace(" ", "_");
                localized = localized.Replace("__", "_");
                sessionPath += localized + ".slate";
            }
        }

        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        public void validatePoints()
        {
            Point lastPoint = currentCurve.points[0];
            foreach (Point point in currentCurve.points)
            {
                if (lastPoint != point)
                {
                    if (!((point.X >= lastPoint.X + 10 || point.X <= lastPoint.X - 10) && (point.Y >= lastPoint.Y + 10 || point.Y <= lastPoint.Y - 10)))
                    {
                        currentCurve.points.Remove(point);
                        validatePoints();
                        break;
                    }
                }
            }
        }

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            
            
        }

        public void RefreshDisplay()
        {
            pnlCanvas.Invalidate();
            pnlCanvas.Update();
        }

        public void removeCurvesIntersectingWithEraser()
        {
            List<Curve> removeCurves = new List<Curve>();
            foreach (Curve curve in curves)
            {
                foreach (Point point in curve.points)
                {
                    foreach (Point ep in eraserCurve)
                    {
                        // Intersection occurs where eraser has width of 20.
                        float width = 20;
                        if ((point.X <= ep.X + width && point.X >= ep.X - width) && (point.Y <= ep.Y + width && point.Y >= ep.Y - width))
                        {
                            removeCurves.Add(curve);
                            break;
                        }
                    }
                }
            }

            foreach (Curve curve in removeCurves)
            {
                deleteCurve(curve);
            }
        }

        public void deleteCurve(Curve curve)
        {
            // Remove passed curve.
            curves.Remove(curve);

            // Update screen.
            this.Invalidate();
            this.Update();
        }

        public enum DrawingTool
        {
            Eraser, Pen, Highlighter
        };

        public Curve newCurve()
        {
            Curve curve = new Curve();
            curve.points = new List<Point>();
            //curve.pen = new Pen(Brushes.Black);
            curve.color = Color.Black;
            curve.colorString = "Black";
            curve.width = 1.0f;
            //curve.pen.Width = (int)currentWidth;
            //curve.pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            return curve;
        }

        public void drawCurve(PaintEventArgs e, Curve c)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.Black);
            pen.Width = c.width;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.Color = Color.FromName(c.colorString);
            //pen.Color = Color.Black;
            //Console.WriteLine(pen.Width);
            //Console.WriteLine(pen.Color);
            if (c.points.Count() > 0)
            {
                Point lastPoint = c.points[0];

                foreach (Point point in c.points)
                {
                    if (lastPoint != point)
                    {

                        // Point lp1 = new Point(lastPoint.X + 1, lastPoint.Y);
                        //Point lp2 = new Point(point.X + 1, point.Y);
                        // Point lp3 = new Point(lastPoint.X + 2, lastPoint.Y);
                        //Point lp4 = new Point(point.X + 2, point.Y);
                        e.Graphics.DrawLine(pen, lastPoint, point);
                        //e.Graphics.DrawLine(c.pen, lp1, lp2);
                        //e.Graphics.DrawLine(c.pen, lp3, lp4);
                        //e.Graphics.DrawEllipse(c.pen, new Rectangle(lastPoint.X, lastPoint.Y, 4, 1));
                        lastPoint = point;
                    }
                }
            }
        }

        public void drawCurveWithBoundingRect(PaintEventArgs e, Curve c)
        {
            drawCurve(e, c);

            Pen pen = new Pen(c.color);
            pen.Width = c.width;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            e.Graphics.DrawRectangle(pen, c.boundingRect);

        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void mnuPenColorBlack_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem m = (ToolStripMenuItem)sender;
            switch (m.Text)
            {
                case "Black":
                    currentColor = Color.Black;
                    break;
                case "Red":
                    currentColor = Color.Red;
                    break;
                case "Green":
                    currentColor = Color.Green;
                    break;
                case "Blue":
                    currentColor = Color.Blue;
                    break;
                case "Highlight":
                    currentColor = Color.FromArgb(64, Color.Yellow);
                    currentWidth = 20;
                    break;

            }
        }

        private void mnuPenWidthSelect(object sender, EventArgs e)
        {
            ToolStripComboBox cmb = (ToolStripComboBox)sender;
            currentWidth = Convert.ToDouble(cmb.Text);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //ToolStrip toolstrip = (ToolStrip)sender;
            switch (e.ClickedItem.Text)
            {
                case "Eraser":
                    selectedTool = DrawingTool.Eraser;
                    break;
                case "Pen":
                    selectedTool = DrawingTool.Pen;
                    break;
            }
        }

        private void mnuOperationsRegistration_Click(object sender, EventArgs e)
        {
            reg = new Registration();
            reg.mainForm = this;
            Console.WriteLine(sclients);
            if (sclients.Count > 0)
            {
                reg.expectedClients = sclients;
            }
            else
            {
                reg.expectedClients = new List<SlateClient>();
            }
            reg.populateTable();
            reg.Show();
        }

        private void mnuBoardStartStop_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;

            if (item.Text == "Start")
            {
                toggleBoard();
                item.Text = "Stop";
            }
            else
            {
                item.Text = "Start";
            }
        }

        public void toggleBoard()
        {
            // Start board if not running. Stop board if running.
            if (!isBoardRunning)
            {

            }
            else
            {

            }
        }

        public SQLiteConnection openDatabase(String location)
        {
            return new SQLiteConnection("Data Source=" + location + ";Version=3;New=True;Compress=True;");
        }

        private void mnuNewClass_Click(object sender, EventArgs e)
        {
            // Open window for new class.

            // Once closed, create db.
            String className = "ECE306";
            String databaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            String databaseFile = databaseFolder + "\\" + className + ".db";
            MessageBox.Show(databaseFile);
            //Console.WriteLine("GetFolderPath: {0}",);

            /*
            // Create database file.
            SQLiteConnection.CreateFile(databaseFile);

            // Open database file.

            dbc = openDatabase(databaseFile);
            dbc.Open();
            dbc.Close();*/
            /*
            string sql = "CREATE TABLE Students (StudentId PRIMARY KEY AUTOINCREMENT INTEGER, Name NVARCHAR(20), Code NVARCHAR(5))";

            SQLiteCommand command = new SQLiteCommand(sql, dbc);
            command.ExecuteNonQuery();
            /*
            string sql = "insert into highscores (name, score) values ('Me', 9001)";

            SQLiteCommand command = new SQLiteCommand(sql, dbc);
            command.ExecuteNonQuery();
            
            dbc.Close();
            */
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            Stream myStream;

            saveFileDialog1.Filter = "ISI files (*.isi)|*.isi";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    System.Diagnostics.Debug.WriteLine(myStream);
                    // Code to write the stream goes here.
                    StreamWriter writer = new StreamWriter(myStream);
                    //StringBuilder sb = new StringBuilder();
                    writer.WriteLine("%ISI-v-1.0$");
                    writer.WriteLine(String.Format("objs Curves {0}", curves.Count()));
                    //writer.Write(sb.ToString());
                    Pen pen;
                    foreach (Curve c in curves)
                    {
                        pen = new Pen(c.color);
                        pen.Width = c.width;
                        pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;

                        // sb = new StringBuilder();
                        writer.WriteLine("obj Curve");
                        writer.WriteLine(String.Format("color:{0}", pen.Color.ToString()));
                        writer.WriteLine(String.Format("width:{0}", pen.Width.ToString()));
                        String pts = "points:";
                        foreach (Point point in c.points)
                        {
                            pts += String.Format("{0}:{1};", point.X, point.Y);
                        }
                        writer.WriteLine(pts);
                        writer.WriteLine("endobj");
                        // writer.Write(sb.ToString());
                    }
                    //sb = new StringBuilder();
                    writer.WriteLine("endobjs");

                    // Write to file.
                    //writer.Write(sb.ToString());

                    writer.Close();
                    // Close stream.
                    myStream.Close();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            StreamReader reader = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Filter = "ISI Files (*.isi)|*.isi|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                Console.WriteLine("reading");
                myStream = openFileDialog1.OpenFile();
                // Insert code to read the stream here.
                reader = new StreamReader(myStream);
                Console.Write(reader);

                //if (reader.ReadLine() == "%ISI-v-1.0$")
                //{
                Console.WriteLine("Correct document type; version 1.0");
                bool loadingCurves = false;
                bool curveLoaded = false;
                Curve loadedCurve = this.newCurve();
                while (reader.Peek() >= 0)
                {
                    //System.Diagnostics.Debug.WriteLine(reader.ReadLine());
                    String line = reader.ReadLine();
                    if (loadingCurves)
                    {
                        if (line.Contains("obj") && line.Contains("Curve"))
                        {
                            System.Diagnostics.Debug.WriteLine("found a curve");
                            curveLoaded = true;
                            continue;
                            //break;
                        }
                        if (line.Contains("endobj"))
                        {
                            curves.Add(loadedCurve);
                            curveLoaded = false;
                            continue;
                            //break;
                        }

                        if (!curveLoaded)
                        {
                            loadedCurve = this.newCurve();
                            curveLoaded = true;
                            continue;
                        }
                        else
                        {
                            if (line.Contains("color"))
                            {
                                String color = line.Substring(line.IndexOf("color") + 6);
                                color = color.Substring(color.IndexOf("[") + 1);
                                color = color.Substring(0, color.IndexOf("]"));
                                System.Diagnostics.Debug.WriteLine(String.Format("writing color: {0}", color));
                                loadedCurve.color = Color.FromName(color);
                            }
                            else if (line.Contains("width"))
                            {
                                String width = line.Substring(line.IndexOf("width") + 6);
                                System.Diagnostics.Debug.WriteLine(String.Format("writing width: {0}", width));
                                loadedCurve.width = Convert.ToInt16(width);
                            }
                            else if (line.Contains("points"))
                            {
                                String points = line.Substring(line.IndexOf("points") + 7);
                                List<string> pts = new List<string>(points.Split(';'));
                                List<Point> cpts = new List<Point>();
                                foreach (string point in pts)
                                {
                                    if (point.IndexOf(":") > 0)
                                    {
                                        string x = point.Substring(0, point.IndexOf(":"));
                                        string y = point.Substring(point.IndexOf(":") + 1);
                                        System.Diagnostics.Debug.WriteLine(String.Format("writing point: {0}, {1}", x, y));
                                        Point pt = new Point(Convert.ToInt16(x), Convert.ToInt16(y));

                                        cpts.Add(pt);
                                    }
                                }
                                loadedCurve.points = cpts;
                            }
                        }
                    }
                    else
                    {
                        if (line.Contains("objs"))
                        {
                            if (line.Contains("Curves"))
                            {
                                System.Diagnostics.Debug.WriteLine("Found Curves");
                                loadingCurves = true;
                            }
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(String.Format("curves loaded: {0}", curves.Count()));
                }
                this.Refresh();
                //}
            }
        }

        private void addClient(TcpClient client)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<TcpClient>(addClient), client);
            }
            else
            {
                Console.WriteLine("SUCCESS");
                clients.Add(client);
                SlateClient clt = new SlateClient();
                clt.client = client;
                clt.status = 1;
                //if (sclients.Count == 0)
                //    clt.isSessionOperator = true;
                sclients.Add(clt);

                // Update registration if possible.
                if (reg.Visible)
                {
                    Console.WriteLine("updating reg...");
                    reg.populateTable();
                }
            }

        }

        private void addSlateClient(SlateClient client)
        {
            sclients.Add(client);
        }

        public void sendMessageToClient(TcpClient client, String msg)
        {
            /*
            ASCIIEncoding encoder = new ASCIIEncoding();
            NetworkStream clientStream = client.GetStream();

            byte[] message = new byte[4096];
            message = encoder.GetBytes(msg);
            clientStream.Write(message, 0, Encoding.UTF8.GetByteCount(msg));*
             */
            server.sendMessageToClient(client, msg);
        }

        private void sendEncodedMessageToClient(TcpClient client, byte[] message)
        {
            NetworkStream clientStream = client.GetStream();
            clientStream.Write(message, 0, message.Length);
        }

        private void listenForClientAuthentication(object clt)
        {
            SlateClient client = (SlateClient)clt;
            NetworkStream clientStream = client.client.GetStream();

            sendMessageToClient(client.client, "auth-send-pswd\r\n");

            int attemptsAllowed = 3;
            int currentAttempt = 1;

            byte[] message = new byte[4096];
            int bytesRead;
            DateTime start = new DateTime();
            TimeSpan timeout = new TimeSpan();
            Console.WriteLine("starting listening for authentication");

            // Keep checking for input as long as the client is still not authenticated or timeout after 1 minute.
            while (serverRunning)
            {
                bytesRead = 0;

                try     { bytesRead = clientStream.Read(message, 0, 4096); }    //blocks until a client sends a message }
                catch   { break; }  //a socket error has occured }

                if (bytesRead == 0)
                    break;  //the client has disconnected from the server


                if (bytesRead > 0)
                {
                ASCIIEncoding encoder = new ASCIIEncoding();
                String str = encoder.GetString(message, 0, bytesRead);
                str = str.Replace("\r\n", "");
                System.Diagnostics.Debug.WriteLine("{0} = {0}", str, sessionPassword);

                if (currentAttempt <= attemptsAllowed - 1)
                {
                    if (str == sessionPassword)
                    {
                        client.isAuthenticated = true;
                        sendMessageToClient(client.client, "pswd-correct\r\n");
                        addClient(client.client);
                        Console.WriteLine(sclients.Count);
                        return;
                    }
                    else
                    {
                        sendMessageToClient(client.client, "pswd-incorrect\r\n");
                        currentAttempt++;
                    }
                }
                else
                {
                    sendMessageToClient(client.client, "pswd-limit-reached\r\n");
                    break;
                }
                }

                timeout = (new DateTime() - start);     // Update timeout.
                Console.WriteLine("Timeout: {0}", timeout);
            }

            if ((timeout.Seconds >= 10 && !client.isAuthenticated) || (currentAttempt == attemptsAllowed))
                client.client.Close();
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();
            serverListening = true;
            while (serverRunning)
            {
                //blocks until a client has connected to the server
                TcpClient client = tcpListener.AcceptTcpClient();
                //client = new TcpClient();

                // Add client to list.
                if (client != null)
                {
                    /*
                     *  ACCEPTED CLIENT CONNECTION
                     *  WRITE ALL INITIALIZATION COMMUNICATION HERE
                     *  
                     * 
                     */
                    //  Send some initialization data (synchronization stuff...)
                    //  TODO: Synchronous stuff...
                    sendMessageToClient(client, "doc-width:" + doc_width + "\r\n");
                    sendMessageToClient(client, "doc-height:" + doc_height + "\r\n");
                    sendMessageToClient(client, "mso\r\n");
                    sessionOperator = client;
                    sessionOperatorReady = false;

                    // Create a SlateClient placeholder for current client.
                    SlateClient clt = new SlateClient();
                    clt.client = client;

                    // If password protected, don't "add" client until authorization has been successful.
                    if (passwordProtected)
                    {
                        // Create a new thread in which to wait for a session reply.
                        Thread acceptThread = new Thread(new ParameterizedThreadStart(listenForClientAuthentication));
                        acceptThread.Start(clt);
                    }
                    else
                        addClient(client);  // Add client to client list.

                    
                }

                //create a thread to handle communication 
                //with connected client
               /* RegistrationSession session = new RegistrationSession();
                session.UpdateProgress += (s, e) =>
                {
                    
                    Dispatcher.Invoke((Action)delegate() { updateRegistration(); });
                };*/
                if (!passwordProtected)
                {
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                }
            }
            serverListening = false;
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            SlateClient currentSlateClient = null;
            NetworkStream clientStream = tcpClient.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();

            byte[] message = new byte[4096];
            int bytesRead;

            bool found = false;
            foreach (SlateClient clnt in sclients)
            {
                if (clnt.client != null)
                {
                    if (clnt.client.Client.RemoteEndPoint == tcpClient.Client.RemoteEndPoint)
                    {
                        currentSlateClient = clnt;
                        found = true;
                    }
                }
            }


            if (!found)
            {
                currentSlateClient = new SlateClient("Unknown", "12345");
            }
            //sendMessageToClient(tcpClient, "reg-request\r\n");


            while (true)
            {
                bytesRead = 0;

                try { bytesRead = clientStream.Read(message, 0, 4096); }    //blocks until a client sends a message
                catch { break; }   //a socket error has occured

                if (bytesRead == 0)
                {
                    // The client has disconnected from the server
                    sclients.Remove(currentSlateClient);                     // This should resolve the reconnection problem.
                    clients.Remove(currentSlateClient.client);
                    break;
                }


                String stra = encoder.GetString(message, 0, bytesRead);
                stra = stra.TrimEnd('\r', '\n');
                Console.WriteLine("Client: " + stra);


                if (stra.Contains("so-width:"))
                {
                    stra = stra.TrimEnd('\r', '\n');
                    Console.WriteLine(stra.Substring("so-width:".Length, stra.IndexOf("\r\n") - "so-width:".Length));
                    so_width = Convert.ToInt16(stra.Substring("so-width:".Length, stra.IndexOf("\r\n") - "so-width:".Length));
                }

                if (stra.Contains("so-height:"))
                {
                    so_height = Convert.ToInt16(stra.Substring("so-height:".Length, stra.IndexOf("\r\n") - "so-height:".Length));
                    sessionOperatorReady = true;
                }

                

                //If the data being read in comes from the operator, then send it back out to all other devices.
                if (tcpClient == sessionOperator && sessionOperatorReady)
                //if (currentSlateClient.isSessionOperator)
                {
                    String str = encoder.GetString(message, 0, bytesRead);
                    Console.WriteLine(String.Format("operator: {0}",str));

                    String result = "";
                    if (getPts)
                    {
                        String[] lines = stra.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        foreach (String line in lines)
                        {
                            if (line.Length > 0)
                            {

                                String[] pts = stra.Split(',');

                                Console.WriteLine("so x " + pts[0]);
                                Console.WriteLine("so y " + pts[1]);

                                int x = Convert.ToInt16(pts[0]);
                                int y = Convert.ToInt16(pts[1]);

                                x = (x / so_width) * doc_width;
                                y = (y / so_height) * doc_height;

                                result += String.Format("{0},{1}\r\n", x, y);
                            }
                        }
                        
                        message = ASCIIEncoding.ASCII.GetBytes(result);
                    }

                    // Send out all data to all clients EXCEPT for operator.
                    /*
                    foreach (SlateClient aClient in sclients)
                    {
                        TcpClient clnt = aClient.client;
                        //if (clnt != tcpClient)
                        //if (!aClient.isSessionOperator)
                        if ()
                            sendEncodedMessageToClient(clnt, message);
                    }*/



                    foreach (TcpClient clnt in clients)
                    {
                        if (clnt != sessionOperator)
                            sendEncodedMessageToClient(clnt, message);
                    }
                }
                else
                {
                    //message has successfully been received
                    //String str = encoder.GetString(message, 0, bytesRead);
                    //Console.WriteLine("Client: " + str);

                    if (requiresRegUponConnect)
                    {
                        if (!currentSlateClient.isRegistered)
                        {
                            foreach (SlateClient rclient in reg.expectedClients)
                            {
                                Console.WriteLine(String.Format("Checking for code: {0}", rclient.code));
                                if (stra.Contains(rclient.code))
                                {
                                    Console.WriteLine("Found code");
                                    Console.WriteLine(String.Format("Checking for ip: {0}", tcpClient.Client.RemoteEndPoint));
                                    foreach (SlateClient clt in sclients)
                                    {
                                        if (tcpClient.Client.RemoteEndPoint == clt.client.Client.RemoteEndPoint)
                                        {
                                            clt.code = rclient.code;
                                            clt.name = rclient.name;
                                            rclient.status = 0;

                                            //updateRegistration();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    /* 
                     
                     * Figure out what to do from here. Registration isn't required.
                     * Ideally, we would wait for a TEMPORARY SESSION OPERATOR (TSO) request.
                     * Once this request has been accepted, we would temporary accept incoming data
                     * to be of the same type of session operator input, thereby sending all this incoming data
                     * out to all devices.
                    
                     */


                    /*  // This is to check the registration of clients without the interface.
                    foreach (SlateClient clt in sclients)
                    {
                        Console.Write(String.Format("{0}, {1}\r\n", clt.name, clt.code));
                    }*/
                }

                if (stra.Contains("pts"))
                    getPts = true;

                if (stra.Contains("endpts"))
                    getPts = false;
            }

            

            tcpClient.Close();
        }

        void updateRegistration()
        {
            reg.updateItems();
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupWindow = new Groups();
            groupWindow.parent = this;
            foreach (SlateClient client in sclients)
            {
                ListViewItem item = new ListViewItem();
                item.Text = client.name;
                groupWindow.items.Add(item);
            }
            //groups.updateClientsTable();
            groupWindow.Show();
        }

        private void drawingTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Tick...");
            if (isDrawing)
            {
                Point point = frmMain.MousePosition;
                if (selectedTool == DrawingTool.Pen || selectedTool == DrawingTool.Eraser)
                {

                    //Point point = new Point(e.X, e.Y);
                    //if ((point.X >= lP.X + 10 || point.X <= lP.X - 10) && (point.Y >= lP.Y + 10 || point.Y <= lP.Y - 10))
                    //{
                    currentCurve.points.Add(point);
                    //validatePoints();
                    //}
                    this.Invalidate();
                    this.Update();
                    lP = point;
                }
                if (selectedTool == DrawingTool.Eraser)
                {
                    eraserCurve.Add(point);
                    removeCurvesIntersectingWithEraser();
                }

                if (clients.Count() > 0)
                {
                    ASCIIEncoding encoder = new ASCIIEncoding();
                    foreach (TcpClient client in clients)
                    {
                        NetworkStream clientStream = client.GetStream();

                        byte[] message = new byte[4096];
                        String msg = String.Format("{0},{1}\r\n", point.X, point.Y);
                        message = encoder.GetBytes(msg);
                        clientStream.Write(message, 0, Encoding.UTF8.GetByteCount(msg));
                    }
                }
            }
        }

        private void connectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connections conn = new Connections();
            conn.clients = sclients;
            conn.Show();
        }

        private void boardSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewBoardSession newBoard = new NewBoardSession();
            if (newBoard.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(String.Format("Creating a new {0} board session named: {1}",((newBoard.chkPublic.Enabled)?"public":"private"),newBoard.txtSessionName.Text));
                if (newBoard.chkPasswordProtect.Enabled)
                {
                    MD5 md5Hash = MD5.Create();
                    string hash = GetMd5Hash(md5Hash, newBoard.txtPassword.Text);
                    Console.WriteLine(String.Format("The hashed session password is: {0}", hash));
                }

                frmMain newBoardSession = new frmMain();
                newBoardSession.Text = String.Format("Slate - {0}",newBoard.txtSessionName.Text);
                newBoardSession.Show();
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        public void drawText(PaintEventArgs e, Text t)
        {
            Brush brush = new System.Drawing.SolidBrush(t.color);
            e.Graphics.DrawString(t.content, t.font, brush, t.location.X, t.location.Y);

            brush.Dispose();
        }

        private void pnlCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            string str = null;
            System.Drawing.Font font = null;
            System.Drawing.Brush brush = null;
            float x, y;

            /*
            str = "Draw a CMOS inverter using a PMOS and NMOS transistor.";
            font = new System.Drawing.Font("Calibri", 12, FontStyle.Bold);
            brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            x = 50.0f;
            y = 50.0f;
            e.Graphics.DrawString(str, font, brush, x, y);
            */



            str = String.Format("{0} / {1}", currentPage, totalPages);
            font = new System.Drawing.Font("Calibri", 10);
            brush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
            x = pnlCanvas.Size.Width - 75.0f;
            y = pnlCanvas.Size.Height - 50.0f;
            e.Graphics.DrawString(str, font, brush, x, y);

            font.Dispose();
            brush.Dispose();


            //Console.WriteLine("Paint");
            Pen pen = new Pen(Brushes.Black);
            pen.Width = 2;

            foreach (object obj in currentSession.document.pages[currentPage - 1].objects)
            {
                if (obj.GetType() == typeof(Curve))
                {
                    //Console.WriteLine("eets a coirve!");
                    drawCurve(e, (Curve)obj);
                    //drawCurveWithBoundingRect(e, (Curve)obj);
                }
                else if (obj.GetType() == typeof(Text))
                    drawText(e, (Text)obj);
            }


            /*
            foreach (Curve curve in curves)
            {
                drawCurve(e, curve);
            }*/

            if (currentCurve != null)
            {
                //Console.WriteLine("Drawing current curve.");
                drawCurve(e, currentCurve);
            }
        }

        private void pnlCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (pnlCanvas.Cursor == Cursors.IBeam)
                {
                    currentTextObject.setLocation(e.X, e.Y);
                    //currentSession.document.pages[currentPage - 1].addObject(currentTextObject);
                    pnlCanvas.Cursor = Cursors.Arrow;
                    refreshDisplay();
                }
                else
                {

                    if (selectedTool == DrawingTool.Pen || selectedTool == DrawingTool.Eraser)
                    {
                        currentCurve = newCurve();
                        if (selectedTool == DrawingTool.Pen)
                        {
                            currentCurve.color = currentColor;
                            currentCurve.colorString = "Black";
                            currentCurve.width = (float)currentWidth;
                        }
                        else
                        {
                            currentCurve.color = Color.Green;
                            currentCurve.width = 10;
                        }
                        currentCurveExists = true;
                    }
                    if (selectedTool == DrawingTool.Eraser)
                        eraserCurve = new List<Point>();
                    //drawingTimer.Enabled = true;
                    isDrawing = true;
                    // this.Refresh();

                    // TODO: CHANGE CURRENTPAGE LOGIC SO THAT WE DONT NEED CURRENTPAGE-1 HERE.

                    //  Transmit message to all clients connected to the server.
                    String msg = String.Format("pg {0}\r\nobj Curve\r\nwidth:{1}\r\ncolor:{2}\r\npts\r\n", currentPage - 1, currentWidth, "Black");
                    server.transmitMessageToAllConnectedClients(msg);
                }
            }
        }

        private void pnlCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isDrawing)
                {
                    Point point = e.Location;
                    if (selectedTool == DrawingTool.Pen || selectedTool == DrawingTool.Eraser)
                    {

                        //Point point = new Point(e.X, e.Y);
                        //if ((point.X >= lP.X + 10 || point.X <= lP.X - 10) && (point.Y >= lP.Y + 10 || point.Y <= lP.Y - 10))
                        //{
                        currentCurve.points.Add(point);
                        if (currentCurve.points.Count() <= 1)
                        {
                            smallestX = highestX = point.X;
                            smallestY = highestY = point.Y;
                            //currentCurve.boundingRect = new Rectangle(point.X, point.Y, 0, 0);
                        }
                        else
                        {
                            // Construct bounding rect.
                            /*
                            currentCurve.boundingRect.X = (point.X < currentCurve.boundingRect.X) ? point.X - currentCurve.boundingRect.Width : currentCurve.boundingRect.X;
                            currentCurve.boundingRect.Y = (point.Y < currentCurve.boundingRect.Y) ? point.Y : currentCurve.boundingRect.Y;
                            currentCurve.boundingRect.Width = (point.X > currentCurve.boundingRect.X + currentCurve.boundingRect.Width) ? point.X - currentCurve.boundingRect.X : currentCurve.boundingRect.Width;
                            currentCurve.boundingRect.Height = (point.Y > currentCurve.boundingRect.Y + currentCurve.boundingRect.Height) ? point.Y - currentCurve.boundingRect.Y : currentCurve.boundingRect.Height;
                             * */
                            smallestX = (point.X < smallestX) ? point.X : smallestX;
                            smallestY = (point.Y < smallestY) ? point.Y : smallestY;

                            highestX = (point.X > highestX) ? point.X : highestX;
                            highestY = (point.Y > highestY) ? point.Y : highestY;

                            currentCurve.boundingRect.X = smallestX;
                            currentCurve.boundingRect.Y = smallestY;
                            currentCurve.boundingRect.Width = highestX - currentCurve.boundingRect.X;
                            currentCurve.boundingRect.Height = highestY - currentCurve.boundingRect.Y;
                        }
                        //validatePoints();
                        //}
                        pnlCanvas.Invalidate();
                        pnlCanvas.Update();
                        lP = point;
                    }
                    if (selectedTool == DrawingTool.Eraser)
                    {
                        eraserCurve.Add(point);
                        eraseLines(point);
                        //removeCurvesIntersectingWithEraser();
                    }


                    //  Send to all clients connected to server
                    String msg = String.Format("{0},{1}\r\n", point.X, point.Y);
                    server.transmitMessageToAllConnectedClients(msg);
                }
            }
        }

        public void eraseLines(Point point)
        {
            int eraserWidth = 10;       // For the time being.
            Rectangle rect = new Rectangle(point.X - eraserWidth/2, point.Y - eraserWidth/2, eraserWidth, eraserWidth);
            bool flag = false;
            Point selectedPoint = new Point(-100,-100);
            List<Curve> addList = new List<Curve>();
            foreach (Curve curve in currentSession.document.pages[currentPage - 1].objects)
            {
                if (curve.boundingRect.Contains(point))
                {
                    // Create a new line starting from the next point.
                    selectedPoint = point;

                    // Start a new line from this point.
                    
                    foreach (Point pt in curve.points)
                    {
                        if (rect.Contains(pt))
                        {
                            selectedPoint = pt;
                            /*
                            Curve newCurve = new Curve();
                            if (curve.points.IndexOf(selectedPoint) > 0)
                                newCurve.points = curve.points.GetRange(curve.points.IndexOf(selectedPoint), curve.points.Count() - curve.points.IndexOf(selectedPoint) - 1);
                            flag = true;*/
                            flag = true;
                            //addList.Add(newCurve);
                            break;
                            
                        }
                    }
                    /*
                    //Console.WriteLine("erasing...");
                    curve.points.
                    int itemsErased = curve.points.RemoveAll(x => rect.Contains(x));
                    Console.WriteLine("{0} items deleted", itemsErased);
                    /*foreach (Point pt in curve.points)
                    {
                        if (rect.Contains(pt))
                            curve.points.Remove(pt);
                    }*/
                }
                if (flag)
                {
                    Curve newCurve = new Curve();
                    if (curve.points.IndexOf(selectedPoint) > 0)
                        newCurve.points = curve.points.GetRange(curve.points.IndexOf(selectedPoint), curve.points.Count() - curve.points.IndexOf(selectedPoint) - 1);
                    curve.points.RemoveAll(x => curve.points.IndexOf(x) > curve.points.IndexOf(selectedPoint));
                    newCurve.points.RemoveAll(x => rect.Contains(x));
                    addList.Add(newCurve);
                    flag = false;
                }
            }

            currentSession.document.pages[currentPage - 1].objects.AddRange(addList);

        }

        private void pnlCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            Rectangle rect = new Rectangle(0,0,0,0);
            if (e.Button == MouseButtons.Left)
            {
                if (isDrawing)
                {
                    if (currentCurve.points.Count == 0)
                        return;

                    if (selectedTool == DrawingTool.Pen)
                    {
                        rect = currentCurve.boundingRect;
                        Console.WriteLine("Color: " + currentCurve.color.ToString());
                        currentSession.document.pages[currentPage - 1].addObject(currentCurve);
                        currentCurve = null;
                    }

                    if (selectedTool == DrawingTool.Eraser)
                        currentCurve = null;
                    //else if (selectedTool == DrawingTool.Eraser)
                    //  eraserCurve;
                    isDrawing = false;
                    drawingTimer.Enabled = false;
                    this.refreshDisplay();
                }
                
                //  Send out to all clients connected to server
                String msg = String.Format("endpts\r\nbound-rect:{0},{1},{2},{3}\r\nendobj\r\n", rect.X, rect.Y, rect.Width, rect.Height);
                server.transmitMessageToAllConnectedClients(msg);
            }
            else
                return;
        }

        private void panelCanvasVerticalScroll_Scroll(object sender, ScrollEventArgs e)
        {
           // pnlCanvas.Location = new Point(pnlCanvas.Location.X, pnlCanvas.Location.Y-panelCanvasVerticalScroll.Value);
        }

        private void toolboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Toolbox toolbox = new Toolbox();
            toolbox.Show();
        }

        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controls controls = new Controls();
            controls.parent = this;
            controls.Show();
        }

        public void refreshDisplay()
        {
            pnlCanvas.Invalidate();
            pnlCanvas.Update();
        }

        public void loadPage()
        {
            // This is where the objects of a current page is loaded. Objects form other pages is unloaded if necessary.
            refreshDisplay();
        }

        public void nextPage()
        {
            nextPage(true);
        }

        public void nextPage(bool shouldTransmit)
        {
            if (currentPage > totalPages - 1)
            {
                currentSession.document.newPage();
                //addPage();
                if (shouldTransmit)
                    server.transmitMessageToAllConnectedClients("nwpg\r\n");
            }
            totalPages += ((currentPage++) >= totalPages) ? 1 : 0;
            if (server != null && shouldTransmit)
                server.transmitMessageToAllConnectedClients("nxt\r\n");
            loadPage();
        }

        public void previousPage()
        {
            previousPage(true);
        }

        public void previousPage(bool shouldTransmit)
        {
            currentPage -= (currentPage > 1) ? 1 : 0;

            if (server != null && shouldTransmit)
                server.transmitMessageToAllConnectedClients("prv\r\n");
            loadPage();
        }

        public void firstPage()
        {
            firstPage(true);
        }

        public void firstPage(bool shouldTransmit)
        {
            currentPage = 1;
            if (server != null && shouldTransmit)
                server.transmitMessageToAllConnectedClients("fst\r\n");
            loadPage();
        }


        public void lastPage()
        {
            lastPage(true);
        }

        public void lastPage(bool shouldTransmit)
        {
            currentPage = totalPages;
            if (server != null && shouldTransmit)
                server.transmitMessageToAllConnectedClients("lst\r\n");
            loadPage();
        }

        public void gotoPage(short page)
        {
            gotoPage(page, true);
        }

        public void gotoPage(short page, bool shouldTransmit)
        {
            if (page <= totalPages)
            {
                if (server != null && shouldTransmit)
                    server.transmitMessageToAllConnectedClients("goto:"+page+"\r\n");

                currentPage = page;
                
                //  Load page
                loadPage();
            }
        }

        public void addPage()
        {
            pages.Add(new Page());
        }

        private void pnlCanvas_Resize(object sender, EventArgs e)
        {
            aspectRatio = ((float)pnlCanvas.Size.Width / (float)pnlCanvas.Size.Height);
            Console.WriteLine("aspect ratio: {0}", aspectRatio);
        }

        private void contextPanelInsertTextbox_Click(object sender, EventArgs e)
        {
            InsertTextbox txtBox = new InsertTextbox();
            if (txtBox.ShowDialog() == DialogResult.OK)
            {
                currentTextObject = new Text();
                currentTextObject.font = txtBox.fontDialog.Font;
                currentTextObject.content = txtBox.txtContent.Text;
                currentTextObject.setLocation(-1, -1);
                pnlCanvas.Cursor = Cursors.IBeam;
            }
        }

        private void transmitMessageToAllConnectedClients(String msg)
        {
            if (clients.Count() > 0)
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                foreach (TcpClient client in server.clients)
                {
                    NetworkStream clientStream = client.GetStream();

                    byte[] message = new byte[4096];
                    message = encoder.GetBytes(msg);
                    clientStream.Write(message, 0, Encoding.UTF8.GetByteCount(msg));
                }
            }
        }

        private void eraserToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectedTool = DrawingTool.Eraser;
        }

        private void penToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedTool = DrawingTool.Pen;
        }

        private void addonMarketplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aoMarketplace == null)
            {
                aoMarketplace = new AddonMarketplace();
                aoMarketplace.Show();
            }
            else
            {
                aoMarketplace.Activate();
            }
        }

        private void postSession(String sessionName, String address, String sessionID)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://kevinrsellon.com/slate/request.php");
            request.Method = "POST";

            String formContent = "type=PUT&name=" + sessionName +
                "&local=" + address +
                "&sid=" + sessionID;

            byte[] byteArray = Encoding.UTF8.GetBytes(formContent);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = HttpUtility.UrlDecode(reader.ReadToEnd());
            //You may need HttpUtility.HtmlDecode depending on the response

            reader.Close();
            dataStream.Close();
            response.Close();

        }

        private void unpublishSession(String sessionName, String sessionID)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://kevinrsellon.com/slate/request.php");
            request.Method = "POST";

            String formContent = "type=REMOVE&name=" + sessionName +
                "&sid=" + sessionID;

            byte[] byteArray = Encoding.UTF8.GetBytes(formContent);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = HttpUtility.UrlDecode(reader.ReadToEnd());
            //You may need HttpUtility.HtmlDecode depending on the response

            reader.Close();
            dataStream.Close();
            response.Close();

        }

        public void saveSession()
        {
           XmlSerializer writer = new XmlSerializer(typeof(Session));

            Console.WriteLine(sessionPath);
            System.IO.StreamWriter file = new System.IO.StreamWriter(sessionPath);
             writer.Serialize(file, currentSession);
            file.Close();

            updateRecents();

            /*
            this.Invoke(new MethodInvoker(delegate {
                if (progress != null)
                {
                    if (progress.Visible)
                        progress.Close();
                }
            }));*/
        }

        public void updateRecents()
        {
            string apppath = Application.UserAppDataPath + "\\recent.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(apppath);
            Console.WriteLine(doc);

            XmlNode rootNode = doc.DocumentElement;
            Console.WriteLine(rootNode.Name);

            bool flag = true;

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                Console.WriteLine("checking current nodes...");
                if (node.ChildNodes.Count > 0)
                {
                    foreach (XmlNode snode in node.ChildNodes)
                    {
                        if (snode.Name.Equals("Location"))
                        {
                            Console.WriteLine("We got a location, folks!");
                            if (snode.ChildNodes[0].Value.Equals(sessionPath))
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                }
            }

            if (flag)
            {
            XmlNode recentNode = doc.CreateElement("Recent");
            XmlNode nameNode = doc.CreateElement("Name");
            nameNode.AppendChild(doc.CreateTextNode(sessionName));

            XmlNode locationNode = doc.CreateElement("Location");
            locationNode.AppendChild(doc.CreateTextNode(sessionPath));

            recentNode.AppendChild(nameNode);
            recentNode.AppendChild(locationNode);

            rootNode.AppendChild(recentNode);


            doc.Save(apppath);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //  Save session before closing the form.
            progress = new Progress();
            progress.lblDetails.Text = "Saving session...";
            progress.progressBar.Style = ProgressBarStyle.Continuous;
            saveSession();
            //Thread saveThread = new Thread(new ThreadStart(saveSession));
            //saveThread.Start();
            //progress.ShowDialog();
            //progress.Close();
            //saveSession();

            //
            //
            //this.Close();

            //  Close down the listening thread
           //S listenThread.Abort();

            unpublishSession(sessionName, sessionID);


            server.disconnectAllClients();
            
            server.stopServer();
            while (server.connectedClients() > 0)
            {
                Console.WriteLine("Connected clients: "+server.connectedClients());
            }
            
            welcomeScreen.Show();
        }

        public void updateRegistrationWindow()
        {
            reg.update();
        }

        public void makeSessionOperator(TcpClient client)
        {
            server.makeSessionOperator(client);
        }

        public void revokeSessionOperator(TcpClient client)
        {
            server.revokeSessionOperator(client);
        }

        public void notifyRegOfSessionOperator(TcpClient tcpClient)
        { 
            reg.notifyRegOfSessionOperator(tcpClient);
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                previousPage();
            else if (e.KeyCode == Keys.Right)
                nextPage();
            else if (e.KeyCode == Keys.Up)
                firstPage();
            else if (e.KeyCode == Keys.Down)
                lastPage();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Preferences preferences = new Preferences();
            preferences.Show();
        }
    }   
}

class RegistrationSession
{
    public event EventHandler UpdateProgress;

    public void register(object input)
    {
        if (UpdateProgress != null)
            UpdateProgress(this, new EventArgs());
            //UpdateProgress(this, 0);
    }
}