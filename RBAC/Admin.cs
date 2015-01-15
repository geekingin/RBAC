using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;
using ADOX;
using System.IO;

namespace RBAC
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }
        private void Admin_Load(object sender, EventArgs e)
        {

            //显示系统时间
            this.timer.Start();
            this.timer.Interval = 100;
            this.timer.Tick += timer_Tick;
            Refresh_UserTable();
           // Refresh_RoleTable();
            if (UserRegister() == true)
            {
                if (MessageBox.Show(this, "有新的账号需要检查通过，是否现在查看", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Table.SelectedIndex = 2;
                }

            }
        }
        //检查是否有新账号注册需要通过
        private bool UserRegister()
        {
            bool b = false;
            //添加注册管理中的待拉用户到列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [注册管理表] ";
            OleDbDataReader dr = cmd.ExecuteReader();

            b = dr.HasRows;

            dr.Close();
            oleDB.Close();
            return b;
        }
        //更新时间
        void timer_Tick(object sender, System.EventArgs e)
        {
            this.timeLabel.Text = DateTime.Now.ToString();

        }
        //更新用户表的代码
        private void Refresh_UserTable()
        {


            this.View_UsersData.Rows.Clear();
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [主体信息表]";
            OleDbDataReader dr = cmd.ExecuteReader();

            LinkedList<string> RoleName = new LinkedList<string>();//用于记录在角色信息表中出现的角色名称
            if (dr.HasRows == true)

                while (dr.Read())
                {
                    if (string.Equals(dr["主体名称"].ToString().ToLower(), "admin") == false &&
                       string.Equals(dr["主体名称"].ToString().ToLower(), "security_officer") == false)//管理员和安全员不必添加
                    {
                        DataGridViewRow r = new DataGridViewRow();
                        r.CreateCells(this.View_UsersData);

                        r.Cells[0].Value = dr["主体名称"];
                        r.Cells[4].Value = dr["注册时间"];

                        LinkedList<string> Direct_Roles = Security_Officer.Get_User_Direct_Roles(dr["主体名称"].ToString());//获取用户的指派角色
                        int i = 0;
                        LinkedList<string> Author_Roles = new LinkedList<string>();//获取用户的授权角色

                          

                        for (LinkedListNode<string> L = Direct_Roles.First; i < Direct_Roles.Count; i++, L = L.Next)
                        {
                            LinkedList<string> ChildRole = Security_Officer.GetChildRoles(L.Value);
                            while (ChildRole.Count != 0)
                            {
                                if (Direct_Roles.Find(ChildRole.First.Value) == null)
                                {
                                    Author_Roles.AddLast(ChildRole.First.Value);
                                  }
                                ChildRole.RemoveFirst();
                            }
                        }
                        LinkedList<string> Permission = new LinkedList<string>();//获取用户的授权角色
                        while (Direct_Roles.Count != 0)
                        {
                            r.Cells[1].Value += Direct_Roles.First.Value + " ";
                            LinkedList<string> P = Security_Officer.Get_Role_Direct_Permissions(Direct_Roles.First.Value);
                            while (P.Count != 0)
                            {
                                if (Permission.Find(P.First.Value) == null)
                                {
                                    Permission.AddLast(P.First.Value);
                                }
                                P.RemoveFirst();
                            }
                            Direct_Roles.RemoveFirst();
                        }
                        while (Author_Roles.Count != 0 )
                        {
                            r.Cells[2].Value += Author_Roles.First.Value + " ";
                            LinkedList<string> P = Security_Officer.Get_Role_Direct_Permissions(Author_Roles.First.Value);
                            while (P.Count != 0)
                            {
                                if (Permission.Find(P.First.Value) == null)
                                {
                                    Permission.AddLast(P.First.Value);
                                }
                                P.RemoveFirst();
                            }
                            Author_Roles.RemoveFirst();
                        }
                        while (Permission.Count != 0)
                        {
                            r.Cells[3].Value += Permission.First.Value+" ";
                            Permission.RemoveFirst();
                        }
                        this.View_UsersData.Rows.Add(r);
                    }
                }
            dr.Close();         
            oleDB.Close();


        }
       //以下是选项卡三 注册管理的代码

        private void Refresh_RolesMangement()
        {
            //添加注册管理中的待拉用户到列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [注册管理表] ";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {

                    this.Combox_SubjectRegister.Items.Add(dr["主体名称"]);//    增加到下拉框中            

                }


            }

            if (this.Combox_SubjectRegister.Items.Count != 0)
            {
                this.Combox_SubjectRegister.SelectedIndex = 0;
            }
            dr.Close();
            oleDB.Close();
        }
        private void Combox_SubjectRegister_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [注册管理表] where 主体名称=" + "'" + this.Combox_SubjectRegister.Text.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                if (dr.Read())
                {

                    this.RegisterTime.Text = dr["注册时间"].ToString();

                }


            }
            dr.Close();
            oleDB.Close();
        }

        private void btn_Register_Sure_Click(object sender, EventArgs e)
        {
            if (this.Combox_SubjectRegister.Text.Length == 0)
            {

                MessageBox.Show(this, "还未选中任何主体", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InsertSubject(this.Combox_SubjectRegister.Text, this.RegisterTime.Text);//插入到主体信息表中
            Refresh_SubjectRegister();
            MessageBox.Show("增加该用户成功！，按确定返回");
        }

        private void btn_Register_Clear_Click(object sender, EventArgs e)
        {
            if (this.Combox_SubjectRegister.Text.Length == 0)
            {

                MessageBox.Show(this, "还未选中任何注册主体", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "delete from [注册管理表] where 主体名称=" + "'" + this.Combox_SubjectRegister.Text.ToLower() + "'";
            if (cmd.ExecuteNonQuery() != 0)
            {
                MessageBox.Show("已清除该条注册信息");
            }
            oleDB.Close();
            Refresh_SubjectRegister();

        }
        /// <summary>
        /// 增加主体信息到主体信息表中
        /// </summary>
        private void InsertSubject(string UserName, string time)
        {

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [注册管理表] where 主体名称=" + "'" + UserName.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            string pwd = string.Empty;
            if (dr.HasRows == true)
            {
                if (dr.Read())
                {
                    pwd = dr["密码"].ToString();

                }


            }
            dr.Close();
            //删除在注册管理表中的信息
            cmd.CommandText = "delete from [注册管理表] where 主体名称=" + "'" + UserName.ToLower() + "'";
            cmd.ExecuteNonQuery();
            // 增加到主体信息表中
            cmd.CommandText = "insert into  [主体信息表](主体名称,密码,注册时间) values('" + UserName.ToLower() + "','" + pwd + "','" + time + "')";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }
        private void Refresh_SubjectRegister()
        {

            this.Combox_SubjectRegister.Items.Clear();
            this.RegisterTime.Text = "";

            //添加注册管理中的待拉用户到列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [注册管理表] ";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {

                    this.Combox_SubjectRegister.Items.Add(dr["主体名称"]);//    增加到下拉框中            

                }


            }

            if (this.Combox_SubjectRegister.Items.Count != 0)
            {
                this.Combox_SubjectRegister.SelectedIndex = 0;
            }
            dr.Close();
            oleDB.Close();

        }
        //选项卡2 的代码

        //以下是选项卡7-用户-角色指派的代码
        void Refresh_Users_Roles(int index)
        {
            this.ComboBox_UserName.Items.Clear();

            //首先绑定数据源到DataGridView_ExcursionRole中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            //将所有的用户名称添加到ComboBox_ExcursionRole中
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [主体信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {

                while (dr.Read())
                {
                    if (string.Equals(dr["主体名称"].ToString().ToLower(), "admin") == false &&
                        string.Equals(dr["主体名称"].ToString().ToLower(), "security_officer") == false)//管理员和安全员不必添加
                    {
                        this.ComboBox_UserName.Items.Add(dr["主体名称"]);

                    }
                }
            }
            if (this.ComboBox_UserName.Items.Count != 0)
            {
                this.ComboBox_UserName.SelectedIndex = index;
            }

            dr.Close();
            oleDB.Close();


        }
        private void ComboBox_UserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ListBox_AssignRole.Items.Clear();
        //    this.ListBox_AssignRoleAdd.Items.Clear();
            this.ListBox_AuthorizeRole.Items.Clear();
            this.ListBox_Permissions.Items.Clear();
            this.Label_Time.Text = GetUserCreatTime(this.ComboBox_UserName.Text) ;



 
       
            LinkedList<string> Direct_Roles = Security_Officer.Get_User_Direct_Roles(this.ComboBox_UserName.Text);//获取用户的指派角色
            LinkedList<string> Roles = new LinkedList<string>();//获取用户拥有的所有角色(包括继承的);

            int i = 0;
            for (LinkedListNode<string> L = Direct_Roles.First; i < Direct_Roles.Count; i++, L = L.Next)
            {
                LinkedList<string> ChildRole = Security_Officer.GetChildRoles(L.Value);//获取指定角色的所有下级角色，添加到授权列表中


                if (Roles.Find(L.Value) == null)
                {
                    Roles.AddLast(L.Value);
                }
                while (ChildRole.Count != 0)
                {
                    if (this.ListBox_AuthorizeRole.Items.IndexOf(ChildRole.First.Value) == -1)//授权列表中没有该角色时，再添加
                    {
                        this.ListBox_AuthorizeRole.Items.Add(ChildRole.First.Value);
                    }
                    if (Roles.Find(ChildRole.First.Value) == null)
                    {
                        Roles.AddLast(ChildRole.First.Value);
                    }
                    ChildRole.RemoveFirst();
                }
            }

            while (Direct_Roles.Count != 0)
            {
                this.ListBox_AssignRole.Items.Add(Direct_Roles.First.Value);
                //从可指派的角色列表中删除对应已指派的角色
               // this.CheckedListBox_AssignRoleAdd.Items.Remove(Direct_Roles.First.Value);
                this.ListBox_AuthorizeRole.Items.Remove(Direct_Roles.First.Value);

                Direct_Roles.RemoveFirst();

            }
            while (Roles.Count != 0)
            {

                LinkedList<string> permisssion = Security_Officer.Get_Role_Direct_Permissions(Roles.First.Value);
                while (permisssion.Count != 0)
                {
                    if (this.ListBox_Permissions.Items.IndexOf((permisssion.First.Value)) == -1)
                    {
                        this.ListBox_Permissions.Items.Add(permisssion.First.Value);

                    }
                    permisssion.RemoveFirst();

                }
                Roles.RemoveFirst();
            }

        }

        /// <summary>
        /// 获取用户的创建时间
        /// </summary>
        /// <param name="PermissionName"></param>
        /// <returns></returns>

        private string GetUserCreatTime(string Name)
        {
            string time = string.Empty;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [主体信息表] where 主体名称=" + "'" + Name.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    time = dr["注册时间"].ToString();
                }
            }

            dr.Close();
            oleDB.Close();

            return time;
        }
        //清除用户
        private void Btn_Delete_User_Click(object sender, EventArgs e)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "delete  from  [主体信息表] where 主体名称=" + "'" + this.ComboBox_UserName.Text.ToLower() + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete  from  [用户-角色指派信息表] where 用户名称=" + "'" + this.ComboBox_UserName.Text.ToLower() + "'";
            cmd.ExecuteNonQuery();
            oleDB.Close();
            MessageBox.Show(this, "删除用户成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
               Refresh_Users_Roles(0);
            
        }
        private void Table_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0:
                    {

                        Refresh_UserTable();
                        break;
                    }

                case 1:
                    {
                        Refresh_Users_Roles(0);


                        break;
                    }


                case 2:
                    {

                        Refresh_RolesMangement();
                        break;
                    }
              
                default:
                    break;


            }
        }

        private void 重新登录NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SubjectRegister.IsUserNameUsed("admin") == false || SubjectRegister.IsUserNameUsed("security_officer") == false)//若数据库文件不存在或则管理员账号不存在，则创建新的文件或者管理员帐号并退出
            {
                if (SubjectRegister.IsUserNameUsed("admin") == false)
                {
                    MessageBox.Show(this, "未设置管理员账号，按确定设置管理员账号及密码", "设置管理员账号及密码", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SubjectRegister register = new SubjectRegister();
                    register.Text = "设置管理员账号及密码";
                    register.getIsAdmin = true;
                    register.ShowDialog();
                    
                    return;

                }
                if (SubjectRegister.IsUserNameUsed("security_officer") == false)
                {
                    MessageBox.Show(this, "未设置安全员账号，按确定设置安全员员账号及密码", "设置安全员账号及密码", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SubjectRegister register = new SubjectRegister();
                    register.Text = "设置安全员账号及密码";
                    register.GetIsSecurityOfficer = true;
                    register.ShowDialog();
                 
                    return;

                }



            }
            else
            {

                SubjectRegister register = new SubjectRegister();
                register.Text = "设置普通用户账号及密码";
                register.isAdmin = false;
                register.ShowDialog();    


            }

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 用户表UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 0;
        }

        private void 查询用户RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 1;
        }

        private void 注册管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 2;
        }

        private void 删除用户DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 1;
        }


        
     
      

        
    }
}
