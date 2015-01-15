namespace RBAC
{
    partial class SubjectRegister
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
            this.btn_Enter = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pwd = new System.Windows.Forms.TextBox();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.UserName = new System.Windows.Forms.TextBox();
            this.rpwd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pwdWarning = new System.Windows.Forms.Label();
            this.userWaring = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Enter
            // 
            this.btn_Enter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_Enter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Enter.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Enter.Location = new System.Drawing.Point(39, 156);
            this.btn_Enter.Name = "btn_Enter";
            this.btn_Enter.Size = new System.Drawing.Size(87, 28);
            this.btn_Enter.TabIndex = 4;
            this.btn_Enter.TabStop = false;
            this.btn_Enter.Text = "注册(&Y)";
            this.btn_Enter.UseVisualStyleBackColor = true;
            this.btn_Enter.Click += new System.EventHandler(this.btn_Enter_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(26, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "账号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(26, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "密码";
            // 
            // pwd
            // 
            this.pwd.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pwd.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pwd.Location = new System.Drawing.Point(73, 69);
            this.pwd.Multiline = true;
            this.pwd.Name = "pwd";
            this.pwd.PasswordChar = '*';
            this.pwd.Size = new System.Drawing.Size(166, 22);
            this.pwd.TabIndex = 2;
            this.pwd.TextChanged += new System.EventHandler(this.pwd_TextChanged);
            // 
            // btn_Exit
            // 
            this.btn_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Exit.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Exit.Location = new System.Drawing.Point(188, 156);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(87, 28);
            this.btn_Exit.TabIndex = 5;
            this.btn_Exit.TabStop = false;
            this.btn_Exit.Text = "退 出 (&E)";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // UserName
            // 
            this.UserName.BackColor = System.Drawing.SystemColors.ControlLight;
            this.UserName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserName.Location = new System.Drawing.Point(73, 27);
            this.UserName.Multiline = true;
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(166, 22);
            this.UserName.TabIndex = 1;
            this.UserName.TextChanged += new System.EventHandler(this.UserName_TextChanged);
            // 
            // rpwd
            // 
            this.rpwd.BackColor = System.Drawing.SystemColors.ControlLight;
            this.rpwd.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rpwd.Location = new System.Drawing.Point(73, 118);
            this.rpwd.Multiline = true;
            this.rpwd.Name = "rpwd";
            this.rpwd.PasswordChar = '*';
            this.rpwd.Size = new System.Drawing.Size(166, 22);
            this.rpwd.TabIndex = 3;
            this.rpwd.TextChanged += new System.EventHandler(this.rpwd_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "确认密码";
            // 
            // pwdWarning
            // 
            this.pwdWarning.AutoSize = true;
            this.pwdWarning.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pwdWarning.ForeColor = System.Drawing.Color.Red;
            this.pwdWarning.Location = new System.Drawing.Point(245, 117);
            this.pwdWarning.Name = "pwdWarning";
            this.pwdWarning.Size = new System.Drawing.Size(107, 20);
            this.pwdWarning.TabIndex = 10;
            this.pwdWarning.Text = "两次密码不一致";
            this.pwdWarning.Visible = false;
            // 
            // userWaring
            // 
            this.userWaring.AutoSize = true;
            this.userWaring.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userWaring.ForeColor = System.Drawing.Color.Red;
            this.userWaring.Location = new System.Drawing.Point(245, 27);
            this.userWaring.Name = "userWaring";
            this.userWaring.Size = new System.Drawing.Size(0, 20);
            this.userWaring.TabIndex = 11;
            this.userWaring.Visible = false;
            // 
            // SubjectRegister
            // 
            this.AcceptButton = this.btn_Enter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.CancelButton = this.btn_Exit;
            this.ClientSize = new System.Drawing.Size(351, 212);
            this.Controls.Add(this.userWaring);
            this.Controls.Add(this.pwdWarning);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rpwd);
            this.Controls.Add(this.UserName);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_Enter);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "SubjectRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "注册账号";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SubjectRegister_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Enter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pwd;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.TextBox UserName;
        private System.Windows.Forms.TextBox rpwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label pwdWarning;
        private System.Windows.Forms.Label userWaring;
    }
}