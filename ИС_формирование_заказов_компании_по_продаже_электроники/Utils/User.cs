using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ИС_формирование_заказов_компании_по_продаже_электроники
{
    public class User
    {
        public int id { get; private set; }
        public string userName { get; private set; }
        public string phoneNumber { get; private set; }
        public string address { get; private set; }
        public string email { get; private set; }

        SqlConnection conn;

        public User(int id,string userName,string phoneNumber,string address,string email,SqlConnection conn)
        {
            this.id = id;
            this.userName = userName;
            this.phoneNumber = phoneNumber;
            this.address = address;
            this.email = email;
            this.conn = conn;
        }
        
        public User()
        {
            this.id = -1;
            this.userName = string.Empty;
            this.phoneNumber = string.Empty;
            this.address = address = string.Empty;
            this.email = string.Empty;
        }

        public void updateData(string newUserName, string newPhoneNum, string newAddress, string newEmail)
        {
            try
            {
                conn.Close();
            }
            catch (Exception)
            { }
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Users SET userName = @name, phoneNumber = @phone, address = @address, email = @email WHERE Id = @id", conn);
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = newUserName;
            cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = newPhoneNum;
            cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = newAddress;
            cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = newEmail;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = this.id;
            try
            {
                cmd.ExecuteNonQuery();
                this.userName = newUserName;
                this.phoneNumber = newPhoneNum;
                this.address = newAddress;
                this.email = newEmail;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
