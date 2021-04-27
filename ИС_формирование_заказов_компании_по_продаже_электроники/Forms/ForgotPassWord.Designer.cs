namespace ИС_формирование_заказов_компании_по_продаже_электроники
{
    partial class ForgotPassWord
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForgotPassWord));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelCheckNewPassword = new System.Windows.Forms.Label();
            this.textBoxCheckNewPassword = new System.Windows.Forms.TextBox();
            this.textBoxNewPassword = new System.Windows.Forms.TextBox();
            this.labelNewPassword = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxConfirm = new System.Windows.Forms.TextBox();
            this.labelConfirm = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.maskedTextBoxPhoneNum = new System.Windows.Forms.MaskedTextBox();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.labelErr = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelErr);
            this.groupBox1.Controls.Add(this.buttonApply);
            this.groupBox1.Controls.Add(this.buttonConfirm);
            this.groupBox1.Controls.Add(this.labelCheckNewPassword);
            this.groupBox1.Controls.Add(this.textBoxCheckNewPassword);
            this.groupBox1.Controls.Add(this.textBoxNewPassword);
            this.groupBox1.Controls.Add(this.labelNewPassword);
            this.groupBox1.Controls.Add(this.buttonSend);
            this.groupBox1.Controls.Add(this.textBoxConfirm);
            this.groupBox1.Controls.Add(this.labelConfirm);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxEmail);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.maskedTextBoxPhoneNum);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 332);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Введите данные для восстоновления";
            // 
            // labelCheckNewPassword
            // 
            this.labelCheckNewPassword.AutoSize = true;
            this.labelCheckNewPassword.Location = new System.Drawing.Point(19, 245);
            this.labelCheckNewPassword.Name = "labelCheckNewPassword";
            this.labelCheckNewPassword.Size = new System.Drawing.Size(73, 13);
            this.labelCheckNewPassword.TabIndex = 13;
            this.labelCheckNewPassword.Text = "Подтвердите";
            this.labelCheckNewPassword.Visible = false;
            // 
            // textBoxCheckNewPassword
            // 
            this.textBoxCheckNewPassword.Location = new System.Drawing.Point(128, 242);
            this.textBoxCheckNewPassword.Name = "textBoxCheckNewPassword";
            this.textBoxCheckNewPassword.Size = new System.Drawing.Size(147, 20);
            this.textBoxCheckNewPassword.TabIndex = 4;
            this.textBoxCheckNewPassword.UseSystemPasswordChar = true;
            this.textBoxCheckNewPassword.Visible = false;
            this.textBoxCheckNewPassword.TextChanged += new System.EventHandler(this.textBoxCheckNewPassword_TextChanged);
            // 
            // textBoxNewPassword
            // 
            this.textBoxNewPassword.Location = new System.Drawing.Point(128, 216);
            this.textBoxNewPassword.Name = "textBoxNewPassword";
            this.textBoxNewPassword.Size = new System.Drawing.Size(147, 20);
            this.textBoxNewPassword.TabIndex = 3;
            this.textBoxNewPassword.UseSystemPasswordChar = true;
            this.textBoxNewPassword.Visible = false;
            this.textBoxNewPassword.TextChanged += new System.EventHandler(this.textBoxNewPassword_TextChanged);
            // 
            // labelNewPassword
            // 
            this.labelNewPassword.AutoSize = true;
            this.labelNewPassword.Location = new System.Drawing.Point(19, 219);
            this.labelNewPassword.Name = "labelNewPassword";
            this.labelNewPassword.Size = new System.Drawing.Size(80, 13);
            this.labelNewPassword.TabIndex = 10;
            this.labelNewPassword.Text = "Новый пароль";
            this.labelNewPassword.Visible = false;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(200, 81);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 9;
            this.buttonSend.Text = "Отправить";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxConfirm
            // 
            this.textBoxConfirm.Location = new System.Drawing.Point(22, 148);
            this.textBoxConfirm.Name = "textBoxConfirm";
            this.textBoxConfirm.Size = new System.Drawing.Size(253, 20);
            this.textBoxConfirm.TabIndex = 2;
            this.textBoxConfirm.Visible = false;
            // 
            // labelConfirm
            // 
            this.labelConfirm.AutoSize = true;
            this.labelConfirm.Location = new System.Drawing.Point(19, 119);
            this.labelConfirm.MaximumSize = new System.Drawing.Size(200, 0);
            this.labelConfirm.Name = "labelConfirm";
            this.labelConfirm.Size = new System.Drawing.Size(182, 26);
            this.labelConfirm.TabIndex = 7;
            this.labelConfirm.Text = "Вам отправлено письмо на почту, введите код из письма ";
            this.labelConfirm.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "email";
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(88, 55);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(187, 20);
            this.textBoxEmail.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ваш номер";
            // 
            // maskedTextBoxPhoneNum
            // 
            this.maskedTextBoxPhoneNum.Location = new System.Drawing.Point(88, 29);
            this.maskedTextBoxPhoneNum.Mask = "+375(00)000-00-00";
            this.maskedTextBoxPhoneNum.Name = "maskedTextBoxPhoneNum";
            this.maskedTextBoxPhoneNum.Size = new System.Drawing.Size(187, 20);
            this.maskedTextBoxPhoneNum.TabIndex = 0;
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Location = new System.Drawing.Point(200, 174);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(75, 23);
            this.buttonConfirm.TabIndex = 14;
            this.buttonConfirm.Text = "Проверить";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Visible = false;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(22, 293);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(253, 23);
            this.buttonApply.TabIndex = 15;
            this.buttonApply.Text = "Изменить пароль";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Visible = false;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // labelErr
            // 
            this.labelErr.AutoSize = true;
            this.labelErr.ForeColor = System.Drawing.Color.Red;
            this.labelErr.Location = new System.Drawing.Point(125, 265);
            this.labelErr.Name = "labelErr";
            this.labelErr.Size = new System.Drawing.Size(19, 13);
            this.labelErr.TabIndex = 16;
            this.labelErr.Text = "err";
            this.labelErr.Visible = false;
            // 
            // ForgotPassWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 356);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ForgotPassWord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Восстанавление пароля";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelConfirm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPhoneNum;
        private System.Windows.Forms.TextBox textBoxConfirm;
        private System.Windows.Forms.Label labelCheckNewPassword;
        private System.Windows.Forms.TextBox textBoxCheckNewPassword;
        private System.Windows.Forms.TextBox textBoxNewPassword;
        private System.Windows.Forms.Label labelNewPassword;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Label labelErr;
    }
}