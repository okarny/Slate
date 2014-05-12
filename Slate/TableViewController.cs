using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Slate
{
    class TableViewController
    {
        public List<string> bindValues;
        public ListView listView;
        public List<TableViewItem> items = new List<TableViewItem>();
        public String name;

        public TableViewController(ListView listView)
        {
            this.listView = listView;
        }

        public void addRow(params string[] labels)
        {
            TableViewItem item = new TableViewItem(labels);
            items.Add(item);
            reloadTable();      // Change this so that the table only reloads the added row.
        }

        private ListViewItem listViewItem(TableViewItem item)
        {
            ListViewItem litem = new ListViewItem(item.cells[0].label);

            foreach (TableViewCell cell in item.cells)
            {
                if (cell != item.cells[0])
                    litem.SubItems.Add(cell.label);
            }

            return litem;
        }

        public void reloadTable()
        {
            listView.Items.Clear();
            foreach (TableViewItem item in items)
            {
                ListViewItem litem = listViewItem(item);
                listView.Items.Add(litem);
            }
            listView.Update();
        }
    }

    class TableViewItem
    {
        public object binding;
        public List<string> bindValues;
        public List<TableViewCell> cells = new List<TableViewCell>();

        public TableViewItem(TableViewCell cell)
        {
            cells.Add(cell);
        }

        public TableViewItem(List<TableViewCell> cells)
        {
            cells.AddRange(cells);
        }

        public TableViewItem(string[] labels)
        {
            foreach (string label in labels)
            {
                TableViewCell cell = new TableViewCell(label);
                cells.Add(cell);
            }
        }
    }

    class TableViewCell
    {
        public string label;

        public TableViewCell(string label)
        {
            this.label = label;
        }
    }
}
