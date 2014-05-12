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
    public partial class Connections : Form
    {
        public List<SlateClient> clients;
        private TableViewController tableViewController;

        public Connections()
        {
            InitializeComponent();
            //tableViewController = new TableViewController(listViewConnections);
            clients = new List<SlateClient>();
            
            clients.Add(new SlateClient("Kevin Sellon", "008291543"));
            clients.Add(new SlateClient("Orr Karny", "12345"));

            /*
            var datasource = from p in clients
                    select new
                    {
                        Column1 = p.GetType().GetProperty("name").GetValue(p, null),
                        Column2 = p.GetType().GetProperty("code").GetValue(p, null),
                        //Column3 = p.GetType().GetProperty("property3").GetValue(p, null),
                        //Column4 = p.GetType().GetProperty("property4").GetValue(p, null),
                    };

            dataGridView.DataSource = null;
            dataGridView.DataSource = datasource;
            dataGridView.Update();*/

            foreach (SlateClient p in clients)
                dataGridView.Rows.Add(p.name, p.code);

       
            //dataGridView.Columns[0].DataPropertyName = "Name";

            //dataGridView.Columns[1].DataPropertyName = "Code";


            //tableViewController.addRow(slateClient.name);
        }

        private void listViewConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
           //btnMakeSessionOperator.Enabled = (listViewConnections.SelectedItems.Count > 0);
        }

        private void btnMakeSessionOperator_Click(object sender, EventArgs e)
        {
            // Code to make the selected client the session operator.
            //tableViewController.itemAt(listViewConnections.SelectedIndices[0]);

            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Font = new Font(dataGridView.Font, FontStyle.Bold);
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (dataGridView.SelectedRows.Contains(row))
                    row.Cells[0].Style = style;
                else
                    row.Cells[0].Style = new DataGridViewCellStyle();
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            btnMakeSessionOperator.Enabled = (dataGridView.Rows.Count > 0);
        }
    }
}
