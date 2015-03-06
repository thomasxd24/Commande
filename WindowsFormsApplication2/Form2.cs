using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelLibrary;
namespace WindowsFormsApplication2
{
    public partial class Form2 : Form
    {
        public Form2(DataTable tempdt)
        {
            InitializeComponent();
            dataGridView1.DataSource = tempdt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("final");
            ds.Tables.Add(DataGridView2DataTable(dataGridView1, "final"));
            ExcelLibrary.DataSetHelper.CreateWorkbook("Le bon de commande.xls", ds);
        }

        public DataTable DataGridView2DataTable(DataGridView dgv, String tblName)
        {

             DataTable dt = new DataTable(tblName);

            // Header columns
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                DataColumn dc = new DataColumn(column.Name.ToString());
                dt.Columns.Add(dc);
            }

            // Data cells
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    dr[j] = (row.Cells[j].Value == null) ? "" : row.Cells[j].Value.ToString();
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

    }
}
