using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using QueryBuilder;
using ИС_формирование_заказов_компании_по_продаже_электроники.Forms;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Drawing.Printing;

namespace ИС_формирование_заказов_компании_по_продаже_электроники
{
    public partial class Main : Form
    {
        LoginForm loginForm;
        Label[] labelContactArr;
        Label[] labelAddressArr;
        SqlConnection conn;
        public Main()
        {
            InitializeComponent();
        }
        private void main_Load(object sender, EventArgs e)
        {
            string html = File.ReadAllText("resources/map.html");
            webBrowser1.DocumentText = html;
            labelAddressArr = new Label[5] { labelAddress1, labelAddress2, labelAddress3, labelAddress4, labelAddress5 };
            labelContactArr = new Label[5]{labelContact1, labelContact2,labelContact3,labelContact4,labelContact5};
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString);
            loginForm = new LoginForm(conn);
            loginForm.user = new User();
            conn.Open();
            listBox1.SelectedIndex = 0;
            SqlDataReader dataReader = null;

            try
            {
                updateCategory();
                updateBrands();
                SqlCommand cmd = new SqlCommand("SELECT TOP 5 * FROM Contacts", conn);
                dataReader = cmd.ExecuteReader();
                int i = 0;
                while (dataReader.Read())
                {
                    if (i == 0 && dataReader[4] != null)
                    {
                        //pictureBox1.Image = Image.FromFile("resources\\images\\" + Convert.ToString(dataReader[4]));
                    }
                    if (i == 0 && dataReader[3] != null)
                    {
                        labelAboutUs.Text = Convert.ToString(dataReader[3]);
                    }
                    labelContactArr[i].Text = Convert.ToString(dataReader[1]);
                    labelAddressArr[i].Text = Convert.ToString(dataReader[2]);
                    i++;
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

        void updateCategory()
        {
            listBox1.Items.Clear();
            SqlDataReader dataReader = null;
            try
            {
                listBox1.Items.Add("Все категории");
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT category FROM Products", conn);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    listBox1.Items.Add(Convert.ToString(dataReader[0]));
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
            listBox1.SelectedIndex = 0;
        }
        void updateBrands()
        {
            checkedListBox1.Items.Clear();

            SqlDataReader dataReader = null;
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT brand FROM Products", conn);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    checkedListBox1.Items.Add(Convert.ToString(dataReader[0]));
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
        private void outputFromProducts()
        {
            ListViewItem viewItem;
            listView1.Items.Clear();
            listView1.Clear();
            imageList1.Images.Clear();
            SqlDataReader dataReader = null;
            try
            {
                string command = QueryBuilderClass.QueryBuilder(
                    listBox1.SelectedItem.ToString(),
                    textBoxSearch.Text, radioButtonPopularity.Checked, 
                    radioButtonExpansive.Checked, radioButtonPure.Checked,
                    checkedListBox1.CheckedItems
                    );
                SqlCommand cmd = new SqlCommand(command, conn);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    imageList1.Images.Add(ImageSizeUtil.stratchImage(Image.FromFile("resources\\images\\" + Convert.ToString(dataReader[1]))));
                    viewItem = new ListViewItem();
                    viewItem.Text = $"{Convert.ToString(dataReader[0])}\n[{dataReader[2]}]";
                    viewItem.ImageIndex = imageList1.Images.Count - 1;
                    viewItem.Tag = dataReader[3];
                    listView1.Items.Add(viewItem);
                }
                GC.Collect();
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

        void updateProfileData()
        {
            textBoxUserName.Text = loginForm.user.userName;
            textBoxPhoneNum.Text = loginForm.user.phoneNumber;
            textBoxAddress.Text = loginForm.user.address;
            textBoxEmail.Text = loginForm.user.email;
        }

        private void buttonReg_Click(object sender, EventArgs e)
        {
            RegistrationFrom regForm = new RegistrationFrom(conn);
            regForm.ShowDialog();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (loginForm.user.id == -1)
            {
                loginForm = new LoginForm(conn);
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    updateProfileData();
                    updateUserOrders();
                    buttonLogin.Text = "Выйти";
                    buttonEdit.Enabled = true;
                    buttonEditPassword.Enabled = true;
                    buttonReg.Enabled = false;
                   
                    buttonCancelOrders.Visible = true;
                }
            }
            else
            {
                loginForm.user = new User();
                updateProfileData();
               
                buttonCancelOrders.Visible = false;
                listViewUserOrders.Items.Clear();
                buttonLogin.Text = "Войти";
                buttonEdit.Enabled = false;
                buttonEditPassword.Enabled = false;
                buttonReg.Enabled = true;
            }
            if (loginForm.user.phoneNumber == "+375(00)000-00-00")
            {
                buttonReg.Enabled = true;
                AdminForm admin = new AdminForm(conn);
                this.Hide();
                admin.ShowDialog();
                this.Show();
                loginForm.user = new User();
                updateProfileData();
                updateBrands();
                updateCategory();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (buttonEdit.Text == "Редактировать" && loginForm.user.id != -1)
            {
                buttonEdit.Text = "Сохранить";
                textBoxUserName.BorderStyle = BorderStyle.Fixed3D;
                textBoxPhoneNum.BorderStyle = BorderStyle.Fixed3D;
                textBoxAddress.BorderStyle = BorderStyle.Fixed3D;
                textBoxEmail.BorderStyle = BorderStyle.Fixed3D;
                textBoxUserName.ReadOnly = false;
                textBoxPhoneNum.ReadOnly = false;
                textBoxAddress.ReadOnly = false;
                textBoxEmail.ReadOnly = false;
            }
            else if(loginForm.user.id != -1)
            {//проверка на норм данные в личном кабинете при обновлении данных

                bool name = true;
                bool address = true;
                bool email = false;
                bool phone = true;
                if (textBoxPhoneNum.Text.Length < 17)
                {
                    phone = false;
                }
                else
                {
                    for (int i = 0; i < textBoxPhoneNum.Text.Length; i++)
                    {
                        if (textBoxPhoneNum.Text[i] == ' ')
                        {

                            phone = false;
                        }
                    }
                }
               
                if (textBoxAddress.Text.Length < 2 || textBoxAddress.Text.Length > 255)
                {
                    address = false;
                }
                for (int i = 0; i < textBoxEmail.Text.Length; i++)
                {
                    if (textBoxEmail.Text[i] == '@')
                    {
                        email = true;
                    }
                }
                if (textBoxUserName.Text.Length < 2 || textBoxUserName.Text.Length > 255)
                {
                    name = false;
                }

                if (email && address && phone && name)
                {
                    loginForm.user.updateData(
                    textBoxUserName.Text,
                    textBoxPhoneNum.Text,
                    textBoxAddress.Text,
                    textBoxEmail.Text
                );
                    updateProfileData();
                    buttonEdit.Text = "Редактировать";
                    textBoxUserName.ReadOnly = true;
                    textBoxPhoneNum.ReadOnly = true;
                    textBoxAddress.ReadOnly = true;
                    textBoxEmail.ReadOnly = true;
                    textBoxUserName.BorderStyle = BorderStyle.None;
                    textBoxPhoneNum.BorderStyle = BorderStyle.None;
                    textBoxAddress.BorderStyle = BorderStyle.None;
                    textBoxEmail.BorderStyle = BorderStyle.None;
                }
                else
                {
                    MessageBox.Show("Некорректные данные");
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            outputFromProducts();
        }

        private void radioButtonPopularity_CheckedChanged(object sender, EventArgs e)
        {
            outputFromProducts();
        }

        private void radioButtonPure_CheckedChanged(object sender, EventArgs e)
        {
            outputFromProducts();
        }

        private void radioButtonExpansive_CheckedChanged(object sender, EventArgs e)
        {
            outputFromProducts();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            outputFromProducts();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            outputFromProducts();
        }

        private void buttonEditPassword_Click(object sender, EventArgs e)
        {
            if (loginForm.user.id != -1)
            {
                ForgotPassWord forgot = new ForgotPassWord(conn);
                forgot.ShowDialog();
            } 
        }

        private void buttonAboutProgramm_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Product product = new Product(
                    conn,
                    Convert.ToInt32(Convert.ToInt32(listView1.SelectedItems[0].Tag)),
                    loginForm.user
                );
                product.ShowDialog();
            }
            listView1.SelectedItems.Clear();
            GC.Collect();
        }

        void updateUserOrders()
        {
            SqlDataReader dataReader = null;
            listViewUserOrders.Items.Clear();
            if (loginForm.user.id != -1)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand($"SELECT Id, productName, address, price, orderDate, state FROM Orders WHERE userID = {loginForm.user.id} AND state = 1", conn);
                    dataReader = cmd.ExecuteReader();
                    ListViewItem viewItem;
                    while (dataReader.Read())
                    {
                        string stat = string.Empty;
                        if (Convert.ToBoolean(dataReader[5]))
                        {
                            stat = "Активный";
                        }
                        else
                        {
                            stat = "Неактивный";
                        }
                        viewItem = new ListViewItem(new string[] {
                        Convert.ToString(dataReader[0]),
                        Convert.ToString(dataReader[1]),
                        Convert.ToString(dataReader[2]),
                        Convert.ToString(dataReader[3]),
                        Convert.ToDateTime(dataReader[4]).ToShortDateString(),
                        stat,
                    });
                        listViewUserOrders.Items.Add(viewItem);
                    }
                    GC.Collect();
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
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateUserOrders();
        }

        private void buttonCancelOrders_Click(object sender, EventArgs e)
        {
            if (buttonCancelOrders.Text == "Отменить заказы")
            {
                buttonCancelOrders.Text = "Отменить выбранные заказы";
                listViewUserOrders.CheckBoxes = true;
            }
            else
            {
                buttonCancelOrders.Text = "Отменить заказы";
                ListViewItem viewItem;
                SqlCommand cmd;
                bool chkd = false;
                foreach (var item in listViewUserOrders.CheckedItems)
                {
                    chkd = true;
                    viewItem = (ListViewItem)item;
                    cmd = new SqlCommand($"DELETE FROM Orders WHERE Id = {viewItem.SubItems[0].Text}",conn);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (chkd)
                {
                    updateUserOrders();
                    MessageBox.Show("Заказы отменены", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                listViewUserOrders.CheckBoxes = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)// save txt
        {
            if (loginForm.user.id != -1)
            {
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != string.Empty)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                    writer.WriteLine($"Пользователь: {loginForm.user.userName}") ;
                    writer.WriteLine($"Номер телефона: {loginForm.user.phoneNumber}");
                    writer.WriteLine($"Адрес: {loginForm.user.address}");
                    writer.WriteLine($"Эл. почта: {loginForm.user.email}");
                    writer.WriteLine();
                    writer.WriteLine($"\tТовары:");
                    SqlCommand cmd = new SqlCommand("SELECT productID,productName,price FROM Orders WHERE userID=@id AND state = 1", conn);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = loginForm.user.id;
                    SqlDataReader dataReader = null;
                    try
                    {
                        dataReader = cmd.ExecuteReader();
                        int counter = 1;
                        double sum = 0;
                        while (dataReader.Read())
                        {
                            sum += Convert.ToDouble(dataReader[2]);
                            writer.WriteLine($"{counter}. {Convert.ToString(dataReader[1])} (id: {Convert.ToString(dataReader[0])}) - Цена: {Convert.ToString(dataReader[2])}");
                            counter++;
                        }
                        writer.WriteLine("==============================");
                        writer.WriteLine($"Сумма заказа = {sum}");
                        MessageBox.Show("Файл сохранён","success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        writer.Close();
                        if (dataReader != null && !dataReader.IsClosed)
                        {
                            dataReader.Close();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Вы не выбрали файл","info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }   
            }
            else
            {
                MessageBox.Show("Выполните вход","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            

            
        }

        private void button1_Click(object sender, EventArgs e) //mail to 
        {
            if (loginForm.user.id != -1)
            {
                string body = "";
                
                body += $"<b>Пользователь: {loginForm.user.userName}</b><br/>";
                body += $"<b>Номер телефона: {loginForm.user.phoneNumber}</b><br/>";
                body += $"<b>Адрес: {loginForm.user.address}</b><br/><br/>";
                body += $"<h1>\tТовары:</h1></br><ol>";
                SqlCommand cmd = new SqlCommand("SELECT productID,productName,price FROM Orders WHERE userID=@id AND state = 1", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = loginForm.user.id;
                SqlDataReader dataReader = null;
                try
                {
                    dataReader = cmd.ExecuteReader();
                    double sum = 0;
                    while (dataReader.Read())
                    {
                        sum += Convert.ToDouble(dataReader[2]);
                        body += $"<li>{Convert.ToString(dataReader[1])} (id: {Convert.ToString(dataReader[0])}) - Цена: {Convert.ToString(dataReader[2])}</li>";
                    }
                    body += "</ol><br/><hr/>";
                    body += $"<b><i>Сумма заказа = {sum}</b></i>";
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
                
                try
                {
                    MailAddress from = new MailAddress("volkov.electronics@gmail.com", "admin");
                    MailAddress to = new MailAddress(loginForm.user.email);
                    MailMessage mail = new MailMessage(from, to);
                    mail.Subject = "Чек заказа volkov.electronics";
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential("volkov.electronics@gmail.com", "volkov.electronics12345678");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    //send to administrator
                    from = new MailAddress("volkov.electronics@gmail.com", "admin");
                    to = new MailAddress("volkov.electronics@gmail.com");
                    mail = new MailMessage(from, to);
                    mail.Subject = "Чек заказа volkov.electronics";
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential("volkov.electronics@gmail.com", "volkov.electronics12345678");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    MessageBox.Show($"Чек успешно оправлен на почту {loginForm.user.email}", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выполните вход", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        string pageBody = "";// var for print function
        private void button3_Click(object sender, EventArgs e) //PRINT
        {
            if (loginForm.user.id != -1)
            {
                pageBody += $"Пользователь: {loginForm.user.userName}\n";
                pageBody += $"Номер телефона: {loginForm.user.phoneNumber}\n";
                pageBody += $"Адрес: {loginForm.user.address}\n";
                pageBody += $"Эл. почта: {loginForm.user.email}\n\n";
                pageBody += $"\tТовары:\n";
                SqlCommand cmd = new SqlCommand("SELECT productID,productName,price FROM Orders WHERE userID=@id AND state = 1", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = loginForm.user.id;
                SqlDataReader dataReader = null;
                try
                {
                    dataReader = cmd.ExecuteReader();
                    double sum = 0;
                    int counter = 0;
                    while (dataReader.Read())
                    {
                        counter++;
                        sum += Convert.ToDouble(dataReader[2]);
                        pageBody += $"\t{counter}. {Convert.ToString(dataReader[1])} (id: {Convert.ToString(dataReader[0])}) - Цена: {Convert.ToString(dataReader[2])}\n";
                    }
                    pageBody += "==============================\n";
                    pageBody += $"Сумма заказа = {sum}";
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

                try
                {
                    PrintDocument doc = new PrintDocument();
                    doc.PrintPage += PrintPageHandler;
                    printPreviewDialog1.Document = doc;
                    printPreviewDialog1.ShowDialog();
                    printDialog1.Document = doc;
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        printDialog1.Document.Print();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выполните вход", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            pageBody = string.Empty;
        }
        void PrintPageHandler(object sender, PrintPageEventArgs e) // for printing handler
        {
            e.Graphics.DrawString(pageBody, new Font("Arial", 14), Brushes.Black, 0, 0);
        }
    }
}
