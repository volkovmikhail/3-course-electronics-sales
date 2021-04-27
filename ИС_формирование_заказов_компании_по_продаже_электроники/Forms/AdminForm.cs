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
        SqlConnection conn;
        bool imgIsOk;
        public AdminForm(SqlConnection conn)
        {
            this.conn = conn;
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            imgIsOk = false;
            updateOrdersData();
            updateProductsData();
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
                        Convert.ToString(dataReader[8]),
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

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (!textBoxQuery.Text.ToUpper().Contains("DROP"))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(textBoxQuery.Text, conn);
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    if (dataSet.Tables.Count != 0)
                    {
                        dataGridView1.DataSource = dataSet.Tables[0];
                    }
                    else
                    {
                        MessageBox.Show("Комманда выполнена");
                    }
                }
                else
                {
                    throw new Exception("Ты дурак?");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
    }
}

