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
    public partial class Form3 : Form
    {

        #region MODELS
        static SqlConnection baglan = new SqlConnection("server=DESKTOP-5N8R6K8;database=FormTask1;integrated security=True");
        int id;
        AppUser user = new AppUser();
        #endregion

        #region CTOR
        public Form3()
        {
            InitializeComponent();
        }
        public Form3(AppUser user)
        {
            this.user = user;
            InitializeComponent();
        }
        #endregion

        #region LOAD
        private void Form3_Load(object sender, EventArgs e)
        {
            List<Products> ProductList = GetProduct();
            listBox1.DataSource = ProductList;
            listBox1.DisplayMember = "Name";

        }
        #endregion

        #region GETPRODUCTS
        public List<Products> GetProduct()
        {
            baglan.Open();

            SqlCommand getProduct = new SqlCommand("SELECT * FROM Products where UserID='" + user.ID + "' order by ID desc", baglan);
            SqlDataReader dr = getProduct.ExecuteReader();
            List<Products> list = new List<Products>();
            while (dr.Read())
            {
                Products urun = new Products()
                {
                    ID = dr.GetInt32("ID"),
                    Name = dr.GetString("Name"),
                    ProductDescription = dr.GetString("ProductDescription"),
                    Stock = dr.GetInt32("Stock"),

                };
                list.Add(urun);
            }
            baglan.Close();
            return list;
        }
        #endregion

        #region SELECTROWS
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Products selectProduct = (Products)listBox1.SelectedItem;
            if (selectProduct != null)
            {
                string selectedName = selectProduct.Name;
                string selectedProductDescription = selectProduct.ProductDescription;
                int selectedProductStock = selectProduct.Stock;
                id = selectProduct.ID;
                richTextBox1.Text = selectedName;
                richTextBox2.Text = selectedProductDescription;
                textBox1.Text = selectedProductStock.ToString();
            }
        }
        #endregion

        #region ADDPRODUCT
        private void button1_Click(object sender, EventArgs e)
        {
            Products product = new Products
            {
                Name = richTextBox1.Text,
                ProductDescription = richTextBox2.Text,
                Stock = Convert.ToInt32(textBox1.Text), // Bu satır stok değeri olarak kullanılıyor
                UserID = user.ID,
            };

            baglan.Open();
            if (textBox1.Text.Length <= 250 && richTextBox1.Text.Length <= 250)
            {
                string insertQuery = "INSERT INTO Products (Name, ProductDescription, Stock, UserID) VALUES (@name, @productdescription, @stock, @userID)";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, baglan))
                {
                    insertCommand.Parameters.AddWithValue("@name", product.Name);
                    insertCommand.Parameters.AddWithValue("@productdescription", product.ProductDescription);
                    insertCommand.Parameters.AddWithValue("@stock", product.Stock); // Stok değeri burada kullanılıyor
                    insertCommand.Parameters.AddWithValue("@userID", product.UserID);

                    insertCommand.ExecuteNonQuery();
                }
            }
            else
            {
                MessageBox.Show("En fazla 250 karakter");

            }
            baglan.Close();
            listBox1.DataSource = null;
            listBox1.DataSource = GetProduct();
            listBox1.DisplayMember = "Name";
        }

        #endregion

        #region DELETE
        private void button3_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand deleteCommand = new SqlCommand("delete from Products where ID='" + id + "'", baglan);
            deleteCommand.ExecuteNonQuery();
            baglan.Close();
            listBox1.DataSource = null;
            listBox1.DataSource = GetProduct();
            listBox1.DisplayMember = "Name";
        }

        #endregion

        #region UPDATE
        private void button2_Click(object sender, EventArgs e)
        {
            Products product = new Products
            {
                Name = richTextBox1.Text,
                ProductDescription = richTextBox2.Text,
                Stock = Convert.ToInt32(textBox1.Text),
                UserID = user.ID,
            };


            baglan.Open();
            if (textBox1.Text.Length <= 250 && richTextBox1.Text.Length <= 250)
            {
                SqlCommand updateCommand = new SqlCommand("update Products set Name='" + product.Name + "',ProductDescription='" + product.ProductDescription + "',Stock='"  +product.Stock+ "' where ID='" + id + "'", baglan);
                updateCommand.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("En fazla 250 karakter");
            }
            baglan.Close();
            listBox1.DataSource = null;
            listBox1.DataSource = GetProduct();
            listBox1.DisplayMember = "Name";
        }

        #endregion

        #region CLEAR
        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            textBox1.Text = "";
        }
        #endregion
    }





}





