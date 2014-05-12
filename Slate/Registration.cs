using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Slate
{
    public partial class Registration : Form
    {
        public frmMain mainForm;

        public enum Status
        {
            Registered, Connecting, Disconnected
        }

        public struct RegistrationItem
        {
            public String name;
            public String code;
            public Status status;
        };

        public List<SlateClient> expectedClients = new List<SlateClient>();

        public Registration()
        {
            InitializeComponent();
           // expectedClients = mainForm.sclients;
            //expectedClients = new List<SlateClient>();
           // expectedClients = mainForm.sexpectedClients;

            // Start thread to start
            //this.updateItems();


            

            //ds.Data
            //dataView.DataSource = ds;
        }

        public void populateTable()
        {
            lstView.Items.Clear();
            foreach (SlateClient client in mainForm.sclients)
            {

                if (client.client != null)
                    Console.WriteLine("Item for: {0}", client.client.Client.RemoteEndPoint.ToString());
                ListViewItem item = new ListViewItem(client.name);
                item.SubItems.Add(client.email);
                item.SubItems.Add(client.code);
                item.SubItems.Add(getStatusString(client.status));
                item.SubItems.Add(((client.isSessionOperator) ? "Yes" : "No"));
                if (client.client != null)
                    item.SubItems.Add(client.client.Client.RemoteEndPoint.ToString());
                lstView.Items.Add(item);
            }
            lstView.Update();
        }

        private void chkGenerateRandom_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.CheckState == CheckState.Checked)
            {
                generateCodeAndSet();
            }
            else
                txtCode.Clear();
        }

        public void generateCodeAndSet()
        {
            if (chkGenerateRandom.CheckState == CheckState.Checked)
            {
                String code = generateCode(5);
                txtCode.Text = code;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RegObject newObj = new RegObject(txtStudent.Text, txtEmail.Text, txtCode.Text);
            mainForm.currentSession.guests.Add(newObj);
            mainForm.regBS.Add(newObj);
            txtStudent.Clear();
            txtEmail.Clear();
            if (chkGenerateRandom.CheckState == CheckState.Checked)
                generateCodeAndSet();
            else
                txtCode.Clear();
            /*
            if (lstView.SelectedItems.Count > 0)
            {
                ListViewItem item = lstView.SelectedItems[0];
                item.SubItems[0].Text = txtStudent.Text;
                item.SubItems[1].Text = txtEmail.Text;
                item.SubItems[2].Text = txtCode.Text;
            }
            else
            {
                // Create new registration item.
                SlateClient item = new SlateClient();
                //RegistrationItem item = new RegistrationItem();
                item.name = txtStudent.Text;
                item.code = txtCode.Text;
                item.status = 0;
                item.email = txtEmail.Text;
                //item.status = SlateClient.Status.Disconnected;
                //expectedClients.Add(item);
                mainForm.sclients.Add(item);

                /*
                // Create ListViewItem with name and code.
                ListViewItem lItem = new ListViewItem(item.name);
                lItem.Checked = true;
                lItem.SubItems.Add(item.code);
                lItem.SubItems.Add(getStatusString(item.status));

                // Add item to list view.
                lstView.Items.Add(lItem);
                populateTable();

                // Clear text fields.
                txtCode.Clear();
                txtStudent.Clear();
                txtEmail.Clear();

                // Set the random generation if required.
                generateCodeAndSet();

                btnSendInvitations.Enabled = (lstView.Items.Count != 0);
            }
             * */
        }

        public String getStatusString(int num)
        {
            string[] labels = { "Waiting...", "Connected", "Registering...", "Registered" };
            return labels[num];
            /*
            switch (num)
            {
                case -1:
                    st = "Disconnected";
                    break;
                case 0:
                    st = "Registering";
                    break;
                case 1:
                    st = "Connected";
                    break;
            }
            return st;*/
        }

        private void btnStartRegistration_Click(object sender, EventArgs e)
        {

        }

        public void updateRegistrationStatus()
        {
            // foreach (item
        }

        public void updateItems()
        {
            foreach (SlateClient client in expectedClients)
            {
                foreach (ListViewItem item in lstView.Items)
                {
                    if (item.SubItems[1].Text == client.code)
                    {
                        item.SubItems[2].Text = getStatusString(client.status);
                    }
                }
            }
            lstView.Update();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lstView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstView.SelectedItems.Count > 0)
            {
                ListViewItem item = lstView.SelectedItems[0];
                txtStudent.Text = item.SubItems[0].Text;
                txtEmail.Text = item.SubItems[1].Text;
                txtCode.Text = item.SubItems[2].Text;
                btnAdd.Text = "Update";
                btnRemove.Enabled = true;
            }
            else
            {
                txtStudent.Clear();
                txtEmail.Clear();
                txtCode.Clear();
                btnAdd.Text = "Add";
                btnRemove.Enabled = false;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            lstView.Items.Remove(lstView.SelectedItems[0]);

            btnSendInvitations.Enabled = (lstView.Items.Count != 0);
        }

        private void Registration_Load(object sender, EventArgs e)
        {
            update();
        }

        public void update()
        {
            Console.WriteLine("Updated registartion");
            if (mainForm != null)
            {
                dataView.DataSource = mainForm.regBS;
                dataView.AutoGenerateColumns = true;
                dataView.Refresh();
            }
        }

        private void dataView_SelectionChanged(object sender, EventArgs e)
        {
            
            updateMakeOperatorButtonForSelectedItem();

        }

        private void btnSendInvitations_Click(object sender, EventArgs e)
        {
            foreach (RegObject obj in mainForm.regBS.List)
            {
                Console.WriteLine("Sending email to: " + obj.email);
               
            }
        }

        private void btnMakeOperator_Click(object sender, EventArgs e)
        {
            if (dataView.SelectedRows.Count > 0)
            {
                RegObject client = (RegObject)dataView.SelectedRows[0].DataBoundItem;
                if (client.sessionOperator)
                {
                    client.sessionOperator = false;
                    mainForm.revokeSessionOperator(client.client);

                    updateMakeOperatorButton(client.sessionOperator);
                }
                else
                {
                    client.sessionOperator = true;
                    mainForm.makeSessionOperator(client.client);
                }

                
                dataView.Refresh();
            }
        }

        public void updateMakeOperatorButton(bool sessionOperator)
        {
            btnMakeOperator.Text = sessionOperator ? "Revoke Session Operator" : "Make Session Operator";
        }

        public void updateMakeOperatorButtonForSelectedItem()
        {
            if(dataView.SelectedRows.Count > 0)
            { 
                RegObject obj = (RegObject)dataView.SelectedRows[0].DataBoundItem;
                btnMakeOperator.Text = obj.sessionOperator ? "Revoke Session Operator" : "Make Session Operator";
                btnSendInvitations.Enabled = btnMakeOperator.Enabled = (dataView.SelectedRows.Count > 0 && obj.status == "Connected");
            }
        }

        public void notifyRegOfSessionOperator(TcpClient tcpClient)
        {
            RegObject reg;
            foreach(object obj in mainForm.regBS.List)
            {
                reg = (RegObject)obj;
                if (reg.client.Equals(tcpClient))
                {
                    reg.sessionOperator = tcpClient.Equals(mainForm.server.sessionOperator);
                    updateMakeOperatorButtonForSelectedItem();
                }
            }
        }

        public void updateDataView()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                updateMakeOperatorButtonForSelectedItem();
                dataView.Refresh();
            }));
        
        }
    }
}
