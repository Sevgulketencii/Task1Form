using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task1Form.Entities;

namespace Task1Form
{
    public partial class Form2 : Form
    {
        #region MODELS
        static SqlConnection baglan = new SqlConnection("server=DESKTOP-5N8R6K8;database=FormTask1;integrated security=True");
        AppUser user = new AppUser();
        static int id;
        #endregion

        #region CTOR
        public Form2()
        {
            
            InitializeComponent();
        }
        public Form2(AppUser user)
        {
            this.user = user;
            InitializeComponent();
        }
        #endregion

        #region LOAD
        private void Form2_Load(object sender, EventArgs e)
        {
            List<Notes> veriler = GetNotes();
         
            dataGridView1.DataSource = veriler;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        #endregion

        #region GETNOTES
        private List<Notes> GetNotes()
        {
           

            baglan.Open();

           
            SqlCommand getUser = new SqlCommand("SELECT * FROM Notes where UserID='" + user.ID + "' order by ID desc", baglan);
   
            SqlDataReader dr = getUser.ExecuteReader();
                List<Notes> veriler= new List<Notes>();

            while (dr.Read())
            {
                Notes veri = new Notes();
                veri.ID = dr.GetInt32("ID");
                veri.Title = dr.GetString("Title");
                veri.Description = dr.GetString("Description");
                veriler.Add(veri);

            }

            baglan.Close();
            return veriler;

        }


        #endregion

        #region ADDNOTES
        private void button2_Click(object sender, EventArgs e)
        {
            Notes note = new Notes
            {
                Description = richTextBox1.Text,
                Title = textBox1.Text,
                UserID=user.ID,
            };


            baglan.Open();
            if (textBox1.Text.Length <= 250 && richTextBox1.Text.Length <= 250)
            {
                string insertQuery = "INSERT INTO Notes (Title, Description,UserID) VALUES (@title, @description,@UserID)";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, baglan))
                {

                    insertCommand.Parameters.AddWithValue("@title", note.Title);
                    insertCommand.Parameters.AddWithValue("@description", note.Description);
                    insertCommand.Parameters.AddWithValue("@userID", note.UserID);

                    insertCommand.ExecuteNonQuery();
                }
            }
            else
            {
                MessageBox.Show("En fazla 250 karakter");
               
            }
            baglan.Close();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = GetNotes();

        }
        #endregion

        #region SELECTROWS
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int select = dataGridView1.SelectedCells[0].RowIndex;
            id = Convert.ToInt32(dataGridView1.Rows[select].Cells[0].Value);
            string title = dataGridView1.Rows[select].Cells[1].Value.ToString();
            string decription = dataGridView1.Rows[select].Cells[2].Value.ToString();


            textBox1.Text = title;
            richTextBox1.Text = decription;
        }
        #endregion

        #region UPDATE
        private void button4_Click(object sender, EventArgs e)
        {

            Notes note = new Notes
            {
                Description = richTextBox1.Text,
                Title = textBox1.Text,

            };
            baglan.Open();
           
            if (textBox1.Text.Length <= 250 && richTextBox1.Text.Length <= 250) { 
            SqlCommand updateCommand = new SqlCommand("update Notes set Title='"+note.Title+"',Description='"+note.Description+"' where ID='"+id+"'",baglan);
            updateCommand.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("En fazla 250 karakter");
            }
            baglan.Close();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = GetNotes();
        }
        #endregion

        #region DELETE
        private void button5_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand deleteCommand = new SqlCommand("delete from Notes where ID='" + id + "'", baglan);
            deleteCommand.ExecuteNonQuery();
            baglan.Close();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = GetNotes();
        }

        #endregion

        #region FORMCLOSE
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
        #endregion

    }
}
