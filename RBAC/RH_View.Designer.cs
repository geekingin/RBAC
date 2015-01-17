namespace RBAC
{
    partial class RH_View
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RH_View));
            this.panel7 = new System.Windows.Forms.Panel();
            this.listBox_RoleName = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Btn_Refresh = new System.Windows.Forms.Button();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel7.Controls.Add(this.listBox_RoleName);
            this.panel7.Location = new System.Drawing.Point(23, 25);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(157, 97);
            this.panel7.TabIndex = 32;
            // 
            // listBox_RoleName
            // 
            this.listBox_RoleName.BackColor = System.Drawing.Color.Moccasin;
            this.listBox_RoleName.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox_RoleName.FormattingEnabled = true;
            this.listBox_RoleName.ItemHeight = 20;
            this.listBox_RoleName.Location = new System.Drawing.Point(3, 3);
            this.listBox_RoleName.Name = "listBox_RoleName";
            this.listBox_RoleName.Size = new System.Drawing.Size(145, 84);
            this.listBox_RoleName.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 20);
            this.label5.TabIndex = 33;
            this.label5.Text = "单独角色:";
            // 
            // Btn_Refresh
            // 
            this.Btn_Refresh.BackColor = System.Drawing.Color.Lime;
            this.Btn_Refresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Btn_Refresh.FlatAppearance.BorderSize = 0;
            this.Btn_Refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Refresh.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Refresh.Location = new System.Drawing.Point(330, 2);
            this.Btn_Refresh.Name = "Btn_Refresh";
            this.Btn_Refresh.Size = new System.Drawing.Size(118, 28);
            this.Btn_Refresh.TabIndex = 34;
            this.Btn_Refresh.TabStop = false;
            this.Btn_Refresh.Text = "角色层次图";
            this.Btn_Refresh.UseVisualStyleBackColor = false;
            this.Btn_Refresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // RH_View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(754, 429);
            this.Controls.Add(this.Btn_Refresh);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel7);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RH_View";
            this.Text = "RH_View";
            this.Load += new System.EventHandler(this.RH_View_Load);
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.ListBox listBox_RoleName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button Btn_Refresh;
    }
}