﻿namespace RBAC
{
    partial class CommonUser
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonUser));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新登录NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.注册ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel21 = new System.Windows.Forms.Panel();
            this.ListBox_Permissions = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.View_RolesData = new System.Windows.Forms.DataGridView();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Table = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button_Activate_Roles = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.listBox_Activating_Permissions = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkedListBox_Roles = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel21.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.View_RolesData)).BeginInit();
            this.Table.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(626, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重新登录NToolStripMenuItem,
            this.注册ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 重新登录NToolStripMenuItem
            // 
            this.重新登录NToolStripMenuItem.Name = "重新登录NToolStripMenuItem";
            this.重新登录NToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.重新登录NToolStripMenuItem.Text = "重新登录(&R)";
            // 
            // 注册ToolStripMenuItem
            // 
            this.注册ToolStripMenuItem.Name = "注册ToolStripMenuItem";
            this.注册ToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.注册ToolStripMenuItem.Text = "注册账号(&S)";
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.退出ToolStripMenuItem.Text = "退出(&E)";
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Location = new System.Drawing.Point(0, 386);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(629, 23);
            this.panel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.timeLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(629, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(82, 17);
            this.toolStripStatusLabel1.Text = "当前系统时间:";
            // 
            // timeLabel
            // 
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage2.Controls.Add(this.panel21);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(621, 323);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "权限";
            // 
            // panel21
            // 
            this.panel21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel21.Controls.Add(this.ListBox_Permissions);
            this.panel21.Location = new System.Drawing.Point(175, 28);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(210, 269);
            this.panel21.TabIndex = 51;
            // 
            // ListBox_Permissions
            // 
            this.ListBox_Permissions.BackColor = System.Drawing.SystemColors.Window;
            this.ListBox_Permissions.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ListBox_Permissions.FormattingEnabled = true;
            this.ListBox_Permissions.ItemHeight = 20;
            this.ListBox_Permissions.Location = new System.Drawing.Point(3, 2);
            this.ListBox_Permissions.Name = "ListBox_Permissions";
            this.ListBox_Permissions.Size = new System.Drawing.Size(200, 264);
            this.ListBox_Permissions.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(87, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "拥有的权限:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.View_RolesData);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(621, 323);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "角色表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // View_RolesData
            // 
            this.View_RolesData.AllowUserToAddRows = false;
            this.View_RolesData.AllowUserToDeleteRows = false;
            this.View_RolesData.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.View_RolesData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.View_RolesData.BackgroundColor = System.Drawing.SystemColors.Window;
            this.View_RolesData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.View_RolesData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column8,
            this.dataGridViewTextBoxColumn1,
            this.Column1,
            this.Column3,
            this.dataGridViewTextBoxColumn7});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 12F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.View_RolesData.DefaultCellStyle = dataGridViewCellStyle4;
            this.View_RolesData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.View_RolesData.Location = new System.Drawing.Point(3, 3);
            this.View_RolesData.MultiSelect = false;
            this.View_RolesData.Name = "View_RolesData";
            this.View_RolesData.ReadOnly = true;
            this.View_RolesData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.View_RolesData.RowHeadersWidth = 5;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.View_RolesData.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.View_RolesData.RowTemplate.Height = 23;
            this.View_RolesData.Size = new System.Drawing.Size(615, 317);
            this.View_RolesData.TabIndex = 2;
            // 
            // Column8
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Column8.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column8.HeaderText = " 角色名称";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 120;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn1.HeaderText = "直接上级";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 3;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "直接下级";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 120;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "其他上级";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 150;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn7.HeaderText = "其他下级";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 150;
            // 
            // Table
            // 
            this.Table.Controls.Add(this.tabPage1);
            this.Table.Controls.Add(this.tabPage2);
            this.Table.Controls.Add(this.tabPage3);
            this.Table.Font = new System.Drawing.Font("SimSun", 12F);
            this.Table.Location = new System.Drawing.Point(0, 31);
            this.Table.Name = "Table";
            this.Table.SelectedIndex = 0;
            this.Table.Size = new System.Drawing.Size(629, 353);
            this.Table.TabIndex = 3;
            this.Table.Selected += new System.Windows.Forms.TabControlEventHandler(this.Table_Selected);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button_Activate_Roles);
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Controls.Add(this.panel2);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(621, 323);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button_Activate_Roles
            // 
            this.button_Activate_Roles.BackColor = System.Drawing.Color.Lime;
            this.button_Activate_Roles.FlatAppearance.BorderSize = 0;
            this.button_Activate_Roles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Activate_Roles.Location = new System.Drawing.Point(264, 111);
            this.button_Activate_Roles.Name = "button_Activate_Roles";
            this.button_Activate_Roles.Size = new System.Drawing.Size(94, 41);
            this.button_Activate_Roles.TabIndex = 4;
            this.button_Activate_Roles.Text = "激活角色";
            this.button_Activate_Roles.UseVisualStyleBackColor = false;
            this.button_Activate_Roles.Click += new System.EventHandler(this.button_Activate_Roles_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.listBox_Activating_Permissions);
            this.panel3.Location = new System.Drawing.Point(373, 71);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 246);
            this.panel3.TabIndex = 3;
            // 
            // listBox_Activating_Permissions
            // 
            this.listBox_Activating_Permissions.FormattingEnabled = true;
            this.listBox_Activating_Permissions.ItemHeight = 16;
            this.listBox_Activating_Permissions.Location = new System.Drawing.Point(3, 3);
            this.listBox_Activating_Permissions.Name = "listBox_Activating_Permissions";
            this.listBox_Activating_Permissions.Size = new System.Drawing.Size(197, 228);
            this.listBox_Activating_Permissions.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkedListBox_Roles);
            this.panel2.Location = new System.Drawing.Point(28, 71);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 246);
            this.panel2.TabIndex = 2;
            // 
            // checkedListBox_Roles
            // 
            this.checkedListBox_Roles.FormattingEnabled = true;
            this.checkedListBox_Roles.Location = new System.Drawing.Point(3, 3);
            this.checkedListBox_Roles.Name = "checkedListBox_Roles";
            this.checkedListBox_Roles.Size = new System.Drawing.Size(194, 235);
            this.checkedListBox_Roles.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(411, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "激活的权限";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "拥有的角色";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // CommonUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(626, 409);
            this.Controls.Add(this.Table);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CommonUser";
            this.Text = "普通用户";
            this.Load += new System.EventHandler(this.CommonUser_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel21.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.View_RolesData)).EndInit();
            this.Table.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel timeLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem 重新登录NToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 注册ToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl Table;
        private System.Windows.Forms.DataGridView View_RolesData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.ListBox ListBox_Permissions;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_Activate_Roles;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListBox listBox_Activating_Permissions;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckedListBox checkedListBox_Roles;
    }
}