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
    public partial class RegistrationFrom : Form
    {
        SqlConnection conn;
        public RegistrationFrom(SqlConnection conn)
        {
            InitializeComponent();
            this.conn = conn;
        }

        bool nameIsOk;
        bool phoneNumIsOk;
        bool emailIsOk;
        bool passwordIsOk;
        bool addressIsOk;

        private void RegistrationFrom_Load(object sender, EventArgs e)
        {
            nameIsOk = false;
            phoneNumIsOk= false;
            emailIsOk = false;
            passwordIsOk = false;
            addressIsOk = false;
        }

        private void buttonRegisterSubmit_Click(object sender, EventArgs e)
        {
            if (nameIsOk && phoneNumIsOk && passwordIsOk && emailIsOk && addressIsOk)
            {
                labelErrorReg.Text = "";
                labelErrorReg.Visible = false;
                int res = 0;
                SqlCommand cmd = new SqlCommand("INSERT INTO Users (userName,password,phoneNumber,address,email) VALUES(@name,@password,@phoneNumber,@address,@email)", conn);
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = textBoxUserName.Text;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = textBoxPassword.Text;
                cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar).Value = maskedTextBoxPhoneNum.Text;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = textBoxAddress.Text;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = textBoxEmail.Text;
                try
                {
                    res = cmd.ExecuteNonQuery();
                    phoneNumIsOk = true;
                }
                catch (Exception)
                {
                    if (res != 1)
                    {
                        labelErrorReg.Text = "Этот номер уже занят";
                        labelErrorReg.Visible = true;
                        phoneNumIsOk = false;
                    }
                }

                if (res == 1)
                {
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                labelErrorReg.Text = "Проверьте введённые данные";
                labelErrorReg.Visible = true;
            }
        }

        private void textBoxUserName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUserName.Text.Length < 2 || textBoxUserName.Text.Length > 255)
            {
                nameIsOk = false;
                labelErrorReg.Text = "Не допустимое значение имени";
                labelErrorReg.Visible = true;
            }
            else
            {
                nameIsOk = true;
                labelErrorReg.Text = "";
                labelErrorReg.Visible = false;
            }
        }

        private void maskedTextBoxPhoneNum_Leave(object sender, EventArgs e)
        {
            bool isFull = true;
            for (int i = 0; i < maskedTextBoxPhoneNum.Text.Length; i++)
            {
                if (maskedTextBoxPhoneNum.Text[i] == ' ')
                {
                    isFull = false;
                }
            }
            if (isFull)
            {
                phoneNumIsOk = true;
                labelErrorReg.Text = "";
                labelErrorReg.Visible = false;
            }
            else
            {
                phoneNumIsOk = false;
                labelErrorReg.Text = "Неполный номер";
                labelErrorReg.Visible = true;
            }
        }

        private void textBoxAddress_Leave(object sender, EventArgs e)
        {
            if (textBoxAddress.Text.Length < 2 || textBoxAddress.Text.Length > 255)
            {
                addressIsOk = false;
                labelErrorReg.Text = "Не допустимое значение адреса";
                labelErrorReg.Visible = true;
            }
            else
            {
                addressIsOk = true;
                labelErrorReg.Text = "";
                labelErrorReg.Visible = false;
            }
        }

        private void textBoxEmail_Leave(object sender, EventArgs e)
        {
            bool emailCheck = false;
            for (int i = 0; i < textBoxEmail.Text.Length; i++)
            {
                if (textBoxEmail.Text[i] == '@')
                {
                    emailCheck = true;
                }
            }
            if (emailCheck)
            {
                emailIsOk = true;
                labelErrorReg.Text = "";
                labelErrorReg.Visible = false;
            }
            else
            {
                emailIsOk = false;
                labelErrorReg.Text = "Неверный email";
                labelErrorReg.Visible = true;
            }
        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text.Length < 8 )
            {
                passwordIsOk = false;
                labelErrorReg.Text = "Пароль слишком маленький";
                labelErrorReg.Visible = true;
            }
            else if (textBoxPassword.Text.Length > 255)
            {
                passwordIsOk = false;
                labelErrorReg.Text = "Пароль слишком большой";
                labelErrorReg.Visible = true;
            }
            else if (textBoxPassword.Text != textBoxPasswordCheck.Text)
            {
                passwordIsOk = false;
                labelErrorReg.Text = "Подтвердите пароль";
                labelErrorReg.Visible = true;
            }
            else
            {
                passwordIsOk = true;
                labelErrorReg.Text = "";
                labelErrorReg.Visible = false;
            }
        }

        private void textBoxPasswordCheck_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text.Length < 8)
            {
                passwordIsOk = false;
                labelErrorReg.Text = "Пароль слишком маленький";
                labelErrorReg.Visible = true;
            }
            else if (textBoxPassword.Text.Length > 50)
            {
                passwordIsOk = false;
                labelErrorReg.Text = "Пароль слишком большой";
                labelErrorReg.Visible = true;
            }
            else if (textBoxPassword.Text != textBoxPasswordCheck.Text)
            {
                passwordIsOk = false;
                labelErrorReg.Text = "Вы не потдвердили пароль";
                labelErrorReg.Visible = true;
            }
            else
            {
                passwordIsOk = true;
                labelErrorReg.Text = "";
                labelErrorReg.Visible = false;
            }
        }
    }
}
