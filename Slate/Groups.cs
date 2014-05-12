using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Slate
{
    public partial class Groups : Form
    {
        public frmMain parent;
        public List<ListViewItem> items;
       // List<ListViewItem> groups;
        List<List<ListViewItem>> members;
        List<GroupObject> groups;
        public BindingSource groupSource;
        public BindingSource membersSource;
        public BindingSource memberPoolSource;

        public Groups()
        {
            InitializeComponent();
           // groups = new List<ListViewItem>();
            members = new List<List<ListViewItem>>();
            items = new List<ListViewItem>();
            btnRemoveGroup.Enabled = false;
            btnAddMember.Enabled = false;
            btnRemoveMember.Enabled = false;
            btnEditGroup.Enabled = false;
            groupSource = new BindingSource();
            dataGridGroups.DataSource = groupSource;
            dataGridGroups.AutoGenerateColumns = true;
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            /*
            NewGroup newGroup = new NewGroup();
            if (newGroup.ShowDialog() == DialogResult.OK)
            {
                ListViewItem item = new ListViewItem();
                item.Text = newGroup.txtGroupName.Text;
               // groups.Add(item);
               // updateGroupsTable();
               // members.Add(new List<ListViewItem>());
            }8
             */
            GroupObject gObject = new GroupObject("Group " + (groupSource.Count + 1));
            groupSource.Add(gObject);
        }
        /*
                private void updateGroupsTable()
                {
                    // Clear group list.
                    listViewGroups.Items.Clear();

                    foreach (ListViewItem item in groups)
                    {
                        listViewGroups.Items.Add(item);
                    }
                }

                private void listViewGroups_SelectedIndexChanged(object sender, EventArgs e)
                {
                    if (listViewGroups.SelectedItems.Count > 0)
                    {
                        btnRemoveGroup.Enabled = true;
                        btnEditGroup.Enabled = true;
                    }
                    else
                    {
                        btnRemoveGroup.Enabled = false;
                        btnEditGroup.Enabled = false;
                    }

                    updateMembersTable();
                }

                private void listViewGroupMembers_SelectedIndexChanged(object sender, EventArgs e)
                {
                    btnRemoveMember.Enabled = (listViewGroups.SelectedItems.Count > 0) ? true : false;
                }

                private void btnRemoveGroup_Click(object sender, EventArgs e)
                {
                    groups.RemoveAt(listViewGroups.SelectedIndices[0]);
                    updateGroupsTable();
                }

                public void updateClientsTable()
                {
                    listViewClients.Items.Clear();

                    foreach (ListViewItem item in items)
                    {
                        listViewClients.Items.Add(item);
                    }
                }

                private void listViewClients_SelectedIndexChanged(object sender, EventArgs e)
                {
                    btnAddMember.Enabled = (listViewGroups.SelectedItems.Count > 0) ? true : false;
                }
        
                private void btnAddMember_Click(object sender, EventArgs e)
                {
                    int groupIndex = listViewGroups.SelectedIndices[0];
                    foreach (ListViewItem item in listViewClients.SelectedItems)
                    {
                        members[groupIndex].Add(item);
                    }

                    updateMembersTable();
                }

                public void updateMembersTable()
                {
                    int groupIndex = -1;
                    if (listViewGroups.SelectedItems.Count > 0)
                    {
                        groupIndex = listViewGroups.SelectedIndices[0];
                        listViewGroupMembers.Items.Clear();

                        if (members[groupIndex].Count > 0)
                        {
                            foreach (ListViewItem item in members[groupIndex])
                            {
                                listViewClients.Items.Remove(item);
                                listViewGroupMembers.Items.Add(item);
                            }
                        }
                    }
            
                }*/

        private void Groups_Load(object sender, EventArgs e)
        {
            memberPoolSource = new BindingSource();
            memberPoolSource.DataSource = typeof(RegObject);
            foreach (RegObject obj in parent.regBS.List)
            {
                Console.WriteLine(obj.name);
                memberPoolSource.Add(obj);
            }
            dataGridClients.DataSource = memberPoolSource;
            dataGridClients.AutoGenerateColumns = true;
        }

        private void dataGridGroups_SelectionChanged(object sender, EventArgs e)
        {
            btnAddMember.Enabled = (dataGridGroups.SelectedRows.Count > 0) && (dataGridClients.SelectedRows.Count > 0);

            updateGroupMembers();
        }

        private void updateGroupMembers()
        {
            membersSource = new BindingSource();
            foreach (RegObject obj in ((GroupObject)dataGridGroups.SelectedRows[0].DataBoundItem).clients)
                membersSource.Add(obj);


            dataGridGroupMembers.DataSource = membersSource;
            dataGridGroupMembers.AutoGenerateColumns = true;
        }

        private void dataGridClients_SelectionChanged(object sender, EventArgs e)
        {
            btnAddMember.Enabled = (dataGridGroups.SelectedRows.Count > 0) && (dataGridClients.SelectedRows.Count > 0);
        }

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            RegObject regObj = (RegObject)dataGridClients.SelectedRows[0].DataBoundItem;
            ((GroupObject)dataGridGroups.SelectedRows[0].DataBoundItem).clients.Add(regObj);
            updateGroupMembers();

            memberPoolSource.Remove(regObj);
        }

        private void dataGridGroupMembers_SelectionChanged(object sender, EventArgs e)
        {
            btnRemoveMember.Enabled = (dataGridGroupMembers.SelectedRows.Count > 0);
        }

        private void btnRemoveMember_Click(object sender, EventArgs e)
        {
            RegObject regObj = (RegObject)dataGridGroupMembers.SelectedRows[0].DataBoundItem;
            ((GroupObject)dataGridGroups.SelectedRows[0].DataBoundItem).clients.Remove(regObj);
            memberPoolSource.Add(regObj);
            updateGroupMembers();
        }

        private void btnRandomizeGroups_Click(object sender, EventArgs e)
        {
            RandomizeGroups randGroups = new RandomizeGroups();

            if (randGroups.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("number of groups: " + randGroups.txtNumberOfGroups.Text);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            //  Activate groups

            //  Add groups respectively
            parent.server.groups = new List<List<RegObject>>();
            List<RegObject> group;
            foreach (GroupObject obj in groupSource.List)
            {
                Console.WriteLine("Start group");
                group = new List<RegObject>();
                foreach (RegObject regObj in obj.clients)
                {
                    Console.WriteLine("adding " + regObj.name);
                    group.Add(regObj);
                }
                Console.WriteLine("End group");
                parent.server.groups.Add(group);
            }

            //  Activate group work in server
            parent.server.groupWorkActivated = true;
        }
    }
}
