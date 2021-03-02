using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Camera_Check_Component
{
    public partial class Sign_up : Form
    {
        SQL_action sQL_Action = new SQL_action();
        public Sign_up()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mesage = "Do you want to sign up this account";
            string cap = "SIGN UP";
            var result = MessageBox.Show(mesage, cap, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.OK&&(textBox2.Text==textBox3.Text)) 
            {
                string sql = "INSERT INTO tb_user_ID (user,pass) VALUES (N'" + textBox1.Text + "','" + textBox2.Text + "')";
                Boolean sign_up = sQL_Action.excute_data(sql);
                byte[] temp = ASCIIEncoding.ASCII.GetBytes(textBox2.Text);
                byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
                string hasPass = "";
                foreach (byte item in hasData)
                {
                    hasPass += item;
                }
                if (comboBox1.SelectedIndex == 0)
                {
                    string ID_user = sQL_Action.getID_user(textBox1.Text, hasPass);
                    string sql2 = "INSERT INTO tbl_per_rel (ID_user_rel,ID_per_rel) VALUES (N'" + ID_user + "','2')";
                    Boolean sign_up1 = sQL_Action.excute_data(sql2);
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    string ID_user = sQL_Action.getID_user(textBox1.Text, hasPass);
                    string sql2 = "INSERT INTO tbl_per_rel (ID_user_rel,ID_per_rel) VALUES (N'" + ID_user + "','4')";
                    Boolean sign_up2 = sQL_Action.excute_data(sql2);
                }
                if (comboBox1.SelectedIndex == 2)
                {
                    string ID_user = sQL_Action.getID_user(textBox1.Text, hasPass);
                    string sql2 = "INSERT INTO tbl_per_rel (ID_user_rel,ID_per_rel) VALUES (N'" + ID_user + "','1')";
                    Boolean sign_up3 = sQL_Action.excute_data(sql2);
                }
                if (comboBox1.SelectedIndex == 3)
                {
                    string ID_user = sQL_Action.getID_user(textBox1.Text, hasPass);
                    string sql2 = "INSERT INTO tbl_per_rel (ID_user_rel,ID_per_rel) VALUES (N'" + ID_user + "','1')";
                    Boolean sign_up4 = sQL_Action.excute_data(sql2);
                }
            }
            else 
            {
                MessageBox.Show("Password incorrect");               
            }
           
        }
    }
}
