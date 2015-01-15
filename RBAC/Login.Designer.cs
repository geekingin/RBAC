namespace RBAC
{
    partial class Login
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
            this.btn_enter = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pwd = new System.Windows.Forms.TextBox();
            this.btn_exit = new System.Windows.Forms.Button();
            this.userCombox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.roleText = new System.Windows.Forms.TextBox();
            this.btn_register = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_enter
            // 
            this.btn_enter.BackColor = System.Drawing.Color.Lime;
            this.btn_enter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_enter.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.btn_enter.FlatAppearance.BorderSize = 0;
            this.btn_enter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_enter.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_enter.Location = new System.Drawing.Point(306, 29);
            this.btn_enter.Name = "btn_enter";
            this.btn_enter.Size = new System.Drawing.Size(87, 28);
            this.btn_enter.TabIndex = 3;
            this.btn_enter.TabStop = false;
            this.btn_enter.Text = "登录(&Y)";
            this.btn_enter.UseVisualStyleBackColor = false;
            this.btn_enter.Click += new System.EventHandler(this.btn_enter_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label2.Location = new System.Drawing.Point(26, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "账号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label3.Location = new System.Drawing.Point(26, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "密码";
            // 
            // pwd
            // 
            this.pwd.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pwd.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pwd.Location = new System.Drawing.Point(73, 109);
            this.pwd.Multiline = true;
            this.pwd.Name = "pwd";
            this.pwd.PasswordChar = '*';
            this.pwd.Size = new System.Drawing.Size(202, 22);
            this.pwd.TabIndex = 2;
            // 
            // btn_exit
            // 
            this.btn_exit.BackColor = System.Drawing.Color.Red;
            this.btn_exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_exit.FlatAppearance.BorderSize = 0;
            this.btn_exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_exit.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_exit.Location = new System.Drawing.Point(306, 67);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(87, 28);
            this.btn_exit.TabIndex = 4;
            this.btn_exit.TabStop = false;
            this.btn_exit.Text = "退出(&E)";
            this.btn_exit.UseVisualStyleBackColor = false;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // userCombox
            // 
            this.userCombox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.userCombox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.userCombox.BackColor = System.Drawing.Color.White;
            this.userCombox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userCombox.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userCombox.FormattingEnabled = true;
            this.userCombox.Location = new System.Drawing.Point(73, 27);
            this.userCombox.MaxLength = 100;
            this.userCombox.Name = "userCombox";
            this.userCombox.Size = new System.Drawing.Size(202, 22);
            this.userCombox.TabIndex = 1;
            this.userCombox.SelectedIndexChanged += new System.EventHandler(this.userCombox_SelectedIndexChanged);
            this.userCombox.TextChanged += new System.EventHandler(this.userCombox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label1.Location = new System.Drawing.Point(26, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "角色";
            // 
            // roleText
            // 
            this.roleText.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.roleText.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.roleText.Location = new System.Drawing.Point(73, 67);
            this.roleText.Multiline = true;
            this.roleText.Name = "roleText";
            this.roleText.ReadOnly = true;
            this.roleText.Size = new System.Drawing.Size(202, 22);
            this.roleText.TabIndex = 8;
            // 
            // btn_register
            // 
            this.btn_register.BackColor = System.Drawing.Color.Aqua;
            this.btn_register.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_register.FlatAppearance.BorderSize = 0;
            this.btn_register.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_register.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_register.Location = new System.Drawing.Point(306, 105);
            this.btn_register.Name = "btn_register";
            this.btn_register.Size = new System.Drawing.Size(87, 28);
            this.btn_register.TabIndex = 9;
            this.btn_register.TabStop = false;
            this.btn_register.Text = "注册";
            this.btn_register.UseVisualStyleBackColor = false;
            this.btn_register.Click += new System.EventHandler(this.btn_register_Click_1);
            // 
            // Login
            // 
            this.AcceptButton = this.btn_enter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CancelButton = this.btn_exit;
            this.ClientSize = new System.Drawing.Size(425, 159);
            this.Controls.Add(this.btn_register);
            this.Controls.Add(this.roleText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userCombox);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_enter);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录账号";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_enter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pwd;
        private System.Windows.Forms.Button btn_exit;
        public System.Windows.Forms.ComboBox userCombox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox roleText;
        private System.Windows.Forms.Button btn_register;
    }
}