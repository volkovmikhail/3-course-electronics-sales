using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ИС_формирование_заказов_компании_по_продаже_электроники
{
    public partial class Product : Form
    {
        SqlConnection conn;
        int id;
        string name;
        string brand;
        string image;
        string discription;
        decimal price;
        string quantity;
        string category;
        string popularity;
        User user;
        public Product(SqlConnection conn, int id,User user)
        {
            this.user = user;
            this.id = id;
            this.conn = conn;
            InitializeComponent();
        }

        private void Product_Load(object sender, EventArgs e)
        {
            
            SqlDataReader dataReader = null;
            try //readData
            {
                SqlCommand cmd = new SqlCommand($"UPDATE Products SET popularity = popularity +1 WHERE id={id}",conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand($"SELECT * FROM Products WHERE id = {id}", conn);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    name = Convert.ToString(dataReader[1]);
                    brand = Convert.ToString(dataReader[2]);
                    image = Convert.ToString(dataReader[3]);
                    discription = Convert.ToString(dataReader[4]);
                    price = Convert.ToDecimal(dataReader[5]);
                    quantity = Convert.ToString(dataReader[6]);
                    category = Convert.ToString(dataReader[7]);
                    popularity = Convert.ToString(dataReader[8]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
            this.Text = name;
            labelProtuctName.Text = name;
            labelBrand.Text = brand;
            labelDiscription.Text = discription;
            labelPrice.Text = price.ToString();
            pictureBox1.Image = ImageSizeUtil.stratchImage(Image.FromFile("resources\\images\\" + image));
            labelQuantity.Text = quantity;
        }

        private void button1_Click(object sender, EventArgs e) //оформить заказ
        {   
            if (user.id != -1)
            {
                if (Convert.ToInt32(quantity) > 0)
                {
                    if (MessageBox.Show("Заказ будет отправлен на обработку,\nданные о заказе появятся у вас в личном кабинете", "Информация о заказе", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        SqlCommand cmd = new SqlCommand($"UPDATE Products SET popularity = popularity + 5, quantity = quantity - 1 WHERE id={id}", conn);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("INSERT INTO Orders (userID,address,phoneNumber,ProductID,productName,price,orderDate,state) VALUES(@userID,@address,@phoneNumber,@ProductID,@productName,@price,@orderDate,@state)", conn);
                        cmd.Parameters.Add("@userID", SqlDbType.NVarChar).Value = user.id;
                        cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = user.address;
                        cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar).Value = user.phoneNumber;
                        cmd.Parameters.Add("@ProductID", SqlDbType.NVarChar).Value = id;
                        cmd.Parameters.Add("@productName", SqlDbType.NVarChar).Value = name;
                        cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = price;
                        cmd.Parameters.Add("@orderDate", SqlDbType.Date).Value = DateTime.Now.ToShortDateString();
                        cmd.Parameters.Add("@state", SqlDbType.Bit).Value = 1;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("error");
                        }
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Приносим извинения,\nТовар закончился","Упс...",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }   
            }
            else
            {
                MessageBox.Show("Чтобы оформить заказ зарегистрируйтесь\nв вкладке 'Личный кабинет'");
            }
            
        }
    }
}
