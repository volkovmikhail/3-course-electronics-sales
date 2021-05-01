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
using System.IO;

namespace ИС_формирование_заказов_компании_по_продаже_электроники
{
    public partial class AdminForm : Form
    {
        TextBox[] niggaAddressBoxes;
        TextBox[] niggaContactsBoxes;
        SqlConnection conn;
        bool imgIsOk;
        public AdminForm(SqlConnection conn)
        {
            this.conn = conn;
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            niggaAddressBoxes = new TextBox[] { textBoxAddress1, textBoxAddress2, textBoxAddress3, textBoxAddress4, textBoxAddress5 };
            niggaContactsBoxes = new TextBox[] { textBoxContact1, textBoxContact2, textBoxContact3, textBoxContact4, textBoxContact5 };
            imgIsOk = false;
            updateOrdersData();
            updateProductsData();
            updateFuckingContacts();
        }

        void updateFuckingContacts()
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP 5 * FROM Contacts", conn);
            SqlDataReader dataReader = null;
            try
            {
                dataReader = cmd.ExecuteReader();
                for (int i = 0; i < 5; i++)
                {
                    dataReader.Read();
                    niggaContactsBoxes[i].Text = Convert.ToString(dataReader[1]);
                    niggaAddressBoxes[i].Text = Convert.ToString(dataReader[2]);
                    if (i==0)
                    {
                        textBoxAboutUs.Text = Convert.ToString(dataReader[3]);
                        pictureBox1.Image = Image.FromFile("resources\\images\\" + Convert.ToString(dataReader[4]));
                    }
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
        }

        void updateProductsData()
        {
            listView2.Items.Clear();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Products", conn);
            SqlDataReader dataReader = null;
            try
            {
                ListViewItem item;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    item = new ListViewItem(new string[]
                    {
                        Convert.ToString(dataReader[0]),
                        Convert.ToString(dataReader[1]),
                        Convert.ToString(dataReader[2]),
                        Convert.ToString(dataReader[3]),
                        Convert.ToString(dataReader[4]),
                        Convert.ToString(dataReader[5]),
                        Convert.ToString(dataReader[6]),
                        Convert.ToString(dataReader[7]),
                        Convert.ToString(dataReader[8])
                    });
                    item.Tag = dataReader[0];
                    listView2.Items.Add(item);
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
        }

        void updateOrdersData()
        {
            listView1.Items.Clear();
            SqlCommand cmd = new SqlCommand("SELECT Orders.*, Users.userName FROM Orders INNER JOIN Users ON Orders.userID = Users.Id", conn);
            SqlDataReader dataReader = null;
            try
            {
                ListViewItem item;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string state;
                    if (Convert.ToBoolean(dataReader[8]))
                    {
                        state = "Активный";
                    }
                    else
                    {
                        state = "Неактивный";
                    }
                    item = new ListViewItem(new string[]
                    {
                        Convert.ToString(dataReader[0]),
                        Convert.ToString(dataReader[1]),
                        Convert.ToString(dataReader[9]),
                        Convert.ToString(dataReader[2]),
                        Convert.ToString(dataReader[3]),
                        Convert.ToString(dataReader[4]),
                        Convert.ToString(dataReader[5]),
                        Convert.ToString(dataReader[6]),
                        Convert.ToDateTime(dataReader[7]).ToShortDateString(),
                        state
                    });
                    item.Tag = dataReader[0];
                    listView1.Items.Add(item);
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                SqlCommand cmd = new SqlCommand($"DELETE FROM Orders WHERE id={Convert.ToString(listView1.SelectedItems[0].Tag)}",conn);
                try
                {
                    cmd.ExecuteNonQuery();
                    updateOrdersData();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonActive_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                SqlCommand cmd = new SqlCommand($"UPDATE Orders SET state=1 WHERE id={Convert.ToString(listView1.SelectedItems[0].Tag)}", conn);
                try
                {
                    cmd.ExecuteNonQuery();
                    updateOrdersData();
                }
                catch (Exception ex)
                {
                    //пробел уёбок
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonUnactive_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                SqlCommand cmd = new SqlCommand($"UPDATE Orders SET state=0 WHERE id={Convert.ToString(listView1.SelectedItems[0].Tag)}", conn);
                try
                {
                    cmd.ExecuteNonQuery();
                    updateOrdersData();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != '\b' && number!=',')
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != '\b')
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != string.Empty)
            {
                string[] ext = openFileDialog1.SafeFileName.Split(new char[] { '.' });
                if (ext[ext.Length-1] == "png" || ext[ext.Length - 1] == "jpeg" || ext[ext.Length - 1] == "bmp")
                {
                    imgIsOk = true;
                    labelImg.Text = openFileDialog1.SafeFileName;
                    try
                    {
                        File.Copy(openFileDialog1.FileName, @"resources\images\" + openFileDialog1.SafeFileName);
                    }
                    catch (Exception)
                    {

                        
                    }
                }
                else
                {
                    MessageBox.Show("Такой файл недопустим");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text != "" && textBoxBrand.Text != "" && textBoxDiscription.Text != "" && textBoxPrice.Text != "" && textBoxQuantity.Text != "" && textBoxCategory.Text != "" && imgIsOk)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Products VALUES(@name,@brand,@image,@discription,@price,@quantity,@category,@popularity)",conn);
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = textBoxName.Text;
                cmd.Parameters.Add("@brand", SqlDbType.NVarChar).Value = textBoxBrand.Text;
                cmd.Parameters.Add("@image", SqlDbType.NVarChar).Value = labelImg.Text;
                cmd.Parameters.Add("@discription", SqlDbType.NVarChar).Value = textBoxDiscription.Text;
                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = textBoxPrice.Text;
                cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = textBoxQuantity.Text;
                cmd.Parameters.Add("@category", SqlDbType.NVarChar).Value = textBoxCategory.Text;
                cmd.Parameters.Add("@popularity", SqlDbType.Int).Value = 0.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Данный продукт добвален в базу данных!");
                    textBoxBrand.Text = "";
                    textBoxCategory.Text = "";
                    textBoxDiscription.Text = "";
                    textBoxName.Text = "";
                    textBoxPrice.Text = "";
                    textBoxQuantity.Text = "";
                    labelImg.Text = "_";
                    updateProductsData();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                labelErr.Visible = false;
            }
            else
            {
                labelErr.Visible = true;
            }
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)//edit
        {
            if (button4.Text == "Редактировать")
            {
                for (int i = 0; i < niggaAddressBoxes.Length; i++)
                {
                    niggaAddressBoxes[i].Enabled = true;
                    niggaContactsBoxes[i].Enabled = true;
                }
                textBoxAboutUs.Enabled = true;
                button4.Text = "Сохранить";
            }
            else
            {
                for (int i = 0; i < niggaAddressBoxes.Length; i++)
                {
                    niggaAddressBoxes[i].Enabled =false;
                    niggaContactsBoxes[i].Enabled = false;
                }
                textBoxAboutUs.Enabled = false;
                button4.Text = "Редактировать";

                SqlCommand cmd = new SqlCommand("UPDATE Contacts SET contact = @contact, address = @address, aboutUs = @about WHERE id = 1", conn);
                cmd.Parameters.Add("@contact", SqlDbType.NVarChar).Value = textBoxContact1.Text;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = textBoxAddress1.Text;
                cmd.Parameters.Add("@about", SqlDbType.NVarChar).Value = textBoxAboutUs.Text;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                for (int i = 1; i < 5; i++)
                {
                    cmd = new SqlCommand("UPDATE Contacts SET contact = @contact, address = @address WHERE id = @id", conn);
                    cmd.Parameters.Add("@contact", SqlDbType.NVarChar).Value = niggaContactsBoxes[i].Text;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = niggaAddressBoxes[i].Text;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = i+1;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)//load image
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != string.Empty)
            {
                string[] ext = openFileDialog1.SafeFileName.Split(new char[] { '.' });
                if (ext[ext.Length - 1] == "png" || ext[ext.Length - 1] == "jpeg" || ext[ext.Length - 1] == "bmp")
                {
                    imgIsOk = true;
                    labelImg.Text = openFileDialog1.SafeFileName;
                    try
                    {
                        File.Copy(openFileDialog1.FileName, @"resources\images\" + openFileDialog1.SafeFileName);
                        SqlCommand cmd = new SqlCommand("UPDATE Contacts SET image = @img WHERE id = 1", conn);
                        cmd.Parameters.Add("@img", SqlDbType.NVarChar).Value = openFileDialog1.SafeFileName;
                        cmd.ExecuteNonQuery();
                        pictureBox1.Image =Image.FromFile(@"resources\images\" + openFileDialog1.SafeFileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }
                }
                else
                {
                    MessageBox.Show("Такой файл недопустим");
                }
            }
        }
    }
}

