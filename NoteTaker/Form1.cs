using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteTaker
{
    public partial class Notes : Form
    {
        DataTable table;

        public Notes()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            table = new DataTable();
            table.Columns.Add("Title", typeof(String));
            table.Columns.Add("Messages", typeof(String));

            dataGrid.DataSource = table;

            dataGrid.Columns["Messages"].Visible = false;
            dataGrid.Columns["Title"].Width = 180;
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            txtTitle.Clear();
            txtMessage.Clear();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            table.Rows.Add(txtTitle.Text, txtMessage.Text);

            txtTitle.Clear();
            txtMessage.Clear();
        }

        private void readBtn_Click(object sender, EventArgs e)
        {
            if(dataGrid.CurrentCell != null)
            {
                int selectedRow = dataGrid.CurrentCell.RowIndex;

                if (selectedRow > -1)
                {
                    txtTitle.Text = table.Rows[selectedRow].ItemArray[0].ToString();
                    txtMessage.Text = table.Rows[selectedRow].ItemArray[1].ToString();
                }
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if(dataGrid.CurrentCell != null){
                int selectedRow = dataGrid.CurrentCell.RowIndex;
                table.Rows[selectedRow].Delete();
            }
        }
    }
}
