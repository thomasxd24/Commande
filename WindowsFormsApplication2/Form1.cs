using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FournisseurSelectCombo.SelectedText))
            {
                MessageBox.Show("Vous avez choisi aucune fournisseur!");
            }
            else
            {
                try
                {
                    string connString =
                    @"Data Source=(localdb)\ProjectsV12;Initial Catalog=test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
                    SqlConnection connection = new SqlConnection(connString);
                    SqlCommand cmdfid = connection.CreateCommand();
                    cmdfid.CommandText = "SELECT DISTINCT Id FROM FN WHERE FournisseurName='" +
                                         FournisseurSelectCombo.SelectedItem + "' ;";
                    // Open the connection of requesting fid
                    connection.Open();
                    int FID = ((int)cmdfid.ExecuteScalar());
                    connection.Close();
                    //close it
                    string query = "select * from articlenew where Fid=" + FID + " and MinimumStock > Stock";

                    SqlConnection conn = new SqlConnection(connString);
                    SqlCommand cmd = new SqlCommand(query, conn);
                    //open the connection for 1. filtering \"Minimum Stock\" > \"Current Stock\ , 2. getting the table to datatable/datagridview
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    conn.Close();
                    da.Dispose();
                    button2.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur sur SQL:" + ex);
                    throw;
                }
               
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connStringFN = @"Data Source=(localdb)\ProjectsV12;Initial Catalog=test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
            using (SqlConnection sqlConnection = new SqlConnection(connStringFN))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT FournisseurName FROM FN", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    FournisseurSelectCombo.Items.Add(sqlReader["FournisseurName"].ToString());
                }

                sqlReader.Close();
            }
           // dataGridView1.Columns["Ignorer"].ReadOnly = false;

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


        private void button2_Click_2(object sender, EventArgs e)
        {
            DataTable tempdt = DataGridView2DataTable(dataGridView1, "tempdt");
            Form2 f2 = new Form2(tempdt);
            f2.ShowDialog();
        }
    }
}
