using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1Form.Entities;
using System.Windows.Forms;

namespace Task1Form
{
    public partial class Form1 : Form
    {
        #region MODEL
        static SqlConnection baglan = new SqlConnection("server=DESKTOP-5N8R6K8;database=FormTask1;integrated security=True");
        #endregion

        #region CTOR
        public Form1()
        {
            
            InitializeComponent();
        }
        #endregion

        #region LOAD
        private void Form1_Load(object sender, EventArgs e)
        {
          
        }
        #endregion

        #region LOGIN
        private void button1_Click(object sender, EventArgs e)
        {

            baglan.Open();
         
            var userName = textBox1.Text;
            var password = textBox2.Text;
            SqlCommand getUser = new SqlCommand("SELECT * FROM Users WHERE Username = @userName AND Password = @password", baglan);
            getUser.Parameters.AddWithValue("@userName", userName);
            getUser.Parameters.AddWithValue("@password", password);
            SqlDataReader dr = getUser.ExecuteReader();
            AppUser kullanici = new AppUser();
            while (dr.Read())
            {
                
                kullanici.kullaniciAdi = dr.GetString("Username");
                kullanici.sifre = dr.GetString("Password");

            }

            if (kullanici.kullaniciAdi!=null)
            {
                Form2 form2 = new Form2();
                form2.FormClosed += Form2_FormClosed;
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı Adı yada Şifre Hatalı");
            }
            baglan.Close();
           
        }
        #endregion

        #region FORM2CLOSE
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
        #endregion

   
    }

}
