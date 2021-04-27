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

namespace ИС_формирование_заказов_компании_по_продаже_электроники
{
    public partial class LoginForm : Form
    {
        public User user { get; set; }
        SqlConnection conn;
        public LoginForm(SqlConnection conn)
        {
            InitializeComponent();
            this.conn = conn;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            user = new User();
        }

        private void linkLabelForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPassWord forgotPassWord = new ForgotPassWord(conn);
            forgotPassWord.ShowDialog();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            bool dataIsOk = false;
            SqlCommand cmd = new SqlCommand($"SELECT * FROM Users WHERE phoneNumber = '{maskedTextBoxPhoneNum.Text}'", conn);
            SqlDataReader dataReader = null;
            try
            {
                dataReader = cmd.ExecuteReader();
                dataReader.Read();
                if (Convert.ToString(dataReader[2]) == textBox1.Text)
                {
                    dataIsOk = true;
                }
                if (dataIsOk)
                {
                    labelErr.Text = "";
                    labelErr.Visible = false;
                    user = new User(
                        Convert.ToInt32(dataReader[0]),
                        Convert.ToString(dataReader[1]),
                        Convert.ToString(dataReader[3]),
                        Convert.ToString(dataReader[4]),
                        Convert.ToString(dataReader[5]),
                        conn
                    );
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    labelErr.Text = "Неверные данные";
                    labelErr.Visible = true;
                }
            }
            catch (Exception)
            {
                labelErr.Text = "Неверные данные";
                labelErr.Visible = true;
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }
    }
}
