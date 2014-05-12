using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Slate
{
    public partial class Welcome : Form
    {

        public Welcome()
        {
            InitializeComponent();

        }

        Progress progress;

        private void btnNewBoardSession_Click(object sender, EventArgs e)
        {
            NewBoardSession newBoard = new NewBoardSession();
            if (newBoard.ShowDialog() == DialogResult.OK)
            {
                string hash = null;
                Console.WriteLine(String.Format("Creating a new {0} board session named: {1}", ((newBoard.chkPublic.Enabled) ? "public" : "private"), newBoard.txtSessionName.Text));
                if (newBoard.chkPasswordProtect.Enabled)
                {
                    MD5 md5Hash = MD5.Create();
                    hash = GetMd5Hash(md5Hash, newBoard.txtPassword.Text);
                    Console.WriteLine(String.Format("The hashed session password is: {0}", hash));
                }
                /*
                publishService = new NetService("", "boardservice._tcp", newBoard.txtSessionName.Text, 6000);

                isPublishing = true;
                publishService.Publish();
                */




                frmMain newBoardSession = new frmMain();
                newBoardSession.requiresRegUponConnect = newBoard.chkRequiresReg.Checked;
                newBoardSession.passwordProtected = true;
                newBoardSession.sessionPassword = hash;
                newBoardSession.requiresRegUponConnect = newBoard.chkRequiresReg.Checked;
                newBoardSession.Text = String.Format("Slate: {0}", newBoard.txtSessionName.Text);
                newBoardSession.sessionName = newBoard.txtSessionName.Text;
                newBoardSession.sessionID = generateCode(6);
                newBoardSession.welcomeScreen = this;
                newBoardSession.Show();
                this.Hide();
            }
        }

        public String generateCode(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
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

        private void dataView_SelectionChanged(object sender, EventArgs e)
        {
            btnResumeSession.Enabled = (dataView.SelectedRows.Count > 0);
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            
        }

        private void dataView_DoubleClick(object sender, EventArgs e)
        {
            startSessionWithSelection();
        }

        private void startSessionWithSelection()
        {
            if (dataView.SelectedRows.Count > 0)
            {
                String location = dataView.SelectedRows[0].Cells["Location"].Value.ToString();

                XmlSerializer serializer = new XmlSerializer(typeof(Session));

                FileStream fs = new FileStream(location, FileMode.Open);
                Session session = (Session)serializer.Deserialize(fs);

                fs.Close();

                progress = new Progress();
                progress.lblDetails.Text = "Opening session...";
                progress.progressBar.Style = ProgressBarStyle.Continuous;


                frmMain newBoardSession = new frmMain(session);
                newBoardSession.passwordProtected = true;
                newBoardSession.welcomeScreen = this;
                newBoardSession.sessionPath = location;

                newBoardSession.Show();

                this.Invoke(new MethodInvoker(delegate
                {
                    if (progress != null)
                    {
                        if (progress.Visible)
                            progress.Close();
                    }
                }));

                this.Hide();
            }
        }

        private void Welcome_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                string path = Application.UserAppDataPath + "/recent.xml";

                if (!File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    doc.AppendChild(docNode);

                    XmlNode recentsNode = doc.CreateElement("Recents");
                    /*
                    XmlNode recentNode = doc.CreateElement("Recent");

                    XmlNode nameNode = doc.CreateElement("Name");
                    nameNode.AppendChild(doc.CreateTextNode("ECE 306 - Dr. Cheng"));

                    XmlNode locationNode = doc.CreateElement("Location");
                    locationNode.AppendChild(doc.CreateTextNode("C:/Location/"));

                    recentNode.AppendChild(nameNode);
                    recentNode.AppendChild(locationNode);

                    recentsNode.AppendChild(recentNode);
                    */
                    doc.AppendChild(recentsNode);

                    doc.Save(path);
                    // Console.WriteLine("File does not exists");
                }

                MostRecent mostRecent = new MostRecent(path);
                if (mostRecent.dataSet.Tables.Count > 0)
                {
                    dataView.DataSource = mostRecent.dataSet.Tables[0];
                    //s Console.WriteLine(mostRecent.dataSet.Tables[0]);
                    dataView.Columns["Location"].Visible = false;
                }

                dataView.Refresh();
            }
        }

        private void btnResumeSession_Click(object sender, EventArgs e)
        {
            startSessionWithSelection();
        }
    }
}
