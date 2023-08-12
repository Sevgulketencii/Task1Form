using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task1Form.Entities;

namespace Task1Form
{
    public partial class Form4 : Form
    {
        #region MODELS
        AppUser user = new AppUser();
        #endregion
         
        #region CTORS
           public Form4()
        {
            InitializeComponent();
        }

        public Form4(AppUser user)
        {
            this.user = user;
            InitializeComponent();
        }
        #endregion


        #region NOTES
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(user);

            form2.FormClosed += Form_FormClosed;
            form2.Show();
            this.Hide();
        }
        #endregion

        #region PRODUCTS
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(user);

            form3.FormClosed += Form_FormClosed;
            form3.Show();
            this.Hide();
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
