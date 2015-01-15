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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, System.EventArgs e)
        {
            if (File.Exists("dac.accdb") == false || SubjectRegister.IsUserNameUsed("admin") == false||  SubjectRegister.IsUserNameUsed("security_officer") == false)//若数据库文件不存在或则管理员账号不存在，则创建新的文件或者管理员帐号并退出
            {
                if (File.Exists("dac.accdb") == false)
                {
                    CreatDb();
                }
                if (SubjectRegister.IsUserNameUsed("admin") == false)
                {
                    //设置管理员账号及密码
                    MessageBox.Show(this, "未设置管理员账号，按确定设置管理员账号及密码", "设置管理员账号及密码", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SubjectRegister register = new SubjectRegister();
                    register.Text = "设置管理员账号及密码";
                    register.getIsAdmin = true;
                    register.ShowDialog();
                    Refresh_UserInfo();//将数据库中用户信息添加到用户列表中
                }
                if (SubjectRegister.IsUserNameUsed("security_officer") == false)
                {
                    //设置安全员账号及密码
                    MessageBox.Show(this, "未设置安全员账号，按确定设置安全员员账号及密码", "设置安全员账号及密码", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SubjectRegister register = new SubjectRegister();
                    register.Text = "设置安全员账号及密码";
                    register.GetIsSecurityOfficer = true;
                    register.ShowDialog();
                    Refresh_UserInfo();//将数据库中用户信息添加到用户列表中
                }   
            }
            Refresh_UserInfo();//将数据库中用户信息添加到用户列表中
        }
        //将数据库的用户更新到用户列表中
        private void Refresh_UserInfo()
        {
            //将数据库中用户信息添加到用户列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");//连接数据库并打开
            oleDB.Open();

            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [主体信息表]";//SQL命令

            OleDbDataReader dr = conn.ExecuteReader();//执行读操作
            if (dr.HasRows == true)
            {
                userCombox.Items.Clear();//清除当前的下拉窗口中的值
                while (dr.Read())
                {
                    userCombox.Items.Add(dr[0].ToString());//显示所有注册用户到下拉窗口
                }
            }
            if (userCombox.Items.Count != 0)
            {
                userCombox.SelectedIndex = 0;
            }

            dr.Close();
            oleDB.Close();
        }
     

        /// <summary>
        /// 创建新的信息数据库
        /// </summary>
        private void CreatDb()
        {
            string con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb";

            ADOX.Catalog catalog = new Catalog();
            catalog.Create(con);//创建新的数据库

            catalog.ActiveConnection();
            ADODB.Connection connection = new ADODB.Connection();
            connection.Open(con, null, null, -1);

            //新建主体信息数据表
            ADOX.Table table = new Table();
            table.Name = "主体信息表";

            ADOX.Column subjectName = new Column();//主体名称
            subjectName.Name = "主体名称";
            ADOX.Column pwd = new Column();//主体登录密码
            pwd.Name = "密码";
            ADOX.Column SubjectRegisterTime = new Column();//主体登录密码
            SubjectRegisterTime.Name = "注册时间";

            table.Columns.Append(subjectName);//将列添加到表中
            table.Columns.Append(pwd);
            table.Columns.Append(SubjectRegisterTime);
            catalog.Tables.Append(table);

            //新建角色信息数据表
             table = new Table();
            table.Name = "角色信息表";

            ADOX.Column RoleName = new Column();//角色名称
            RoleName.Name = "角色名称";

            ADOX.Column RoleRegisterTime= new Column();//角色注册时间
            RoleRegisterTime.Name = "创建时间";

            table.Columns.Append(RoleName);//将列添加到表中
            table.Columns.Append(RoleRegisterTime);            
            catalog.Tables.Append(table);
            //新建角互斥色信息数据表
            table = new Table();
            table.Name = "互斥角色信息表";

            ADOX.Column ExclusionRoleName = new Column();//角色名称
            ExclusionRoleName.Name = "角色1名称";
            ADOX.Column ExclusionRoleName1 = new Column();//角色名称
            ExclusionRoleName1.Name = "角色2名称";

            table.Columns.Append(ExclusionRoleName);//将列添加到表中
            table.Columns.Append(ExclusionRoleName1);
            catalog.Tables.Append(table);

            //新建角色关系信息数据表
            table = new Table();
            table.Name = "角色关系信息表";
            ADOX.Column FatherRoleName = new Column();//角色名称
            FatherRoleName.Name = "父角色名称";
            ADOX.Column ChildRoleName = new Column();//角色名称
            ChildRoleName.Name = "子角色名称";
            table.Columns.Append(FatherRoleName);//将列添加到表中
            table.Columns.Append(ChildRoleName);
            catalog.Tables.Append(table);


            //新建权限信息表
            table = new Table();
            table.Name = "权限信息表";
            ADOX.Column PermissionName = new Column();//权限名称
            PermissionName.Name = "权限名称";
            ADOX.Column PermissionRegisterTime = new Column();//权限创建注册时间
            PermissionRegisterTime.Name = "创建时间";

            table.Columns.Append(PermissionName);//将列添加到表中
            table.Columns.Append(PermissionRegisterTime);
            catalog.Tables.Append(table);

            //新建用户角色指派信息表
            table = new Table();
            table.Name = "用户-角色指派信息表";
            ADOX.Column UserName = new Column();//用户名称
            UserName.Name = "用户名称";
              RoleName = new Column();//角色名称
            RoleName.Name = "角色名称";

            table.Columns.Append(UserName);//将列添加到表中
            table.Columns.Append(RoleName);
            catalog.Tables.Append(table);

            //新建角色到权限的指派信息表

            table = new Table();
            table.Name = "角色-权限指派信息表";
            RoleName = new Column();//角色名称
            RoleName.Name = "角色名称";
            PermissionName = new Column();//权限名称
            PermissionName.Name = "权限名称";

            table.Columns.Append(RoleName);
            table.Columns.Append(PermissionName);
            catalog.Tables.Append(table);


            //新建注册管理表
            table = new Table();
            table.Name = "注册管理表";
            ADOX.Column subject_Name1 = new Column();//客体名称
            subject_Name1.Name = "主体名称";
            ADOX.Column time = new Column();//客体名称
            time.Name = "注册时间";
            ADOX.Column pwd1 = new Column();//客体名称
            pwd1.Name = "密码";

            table.Columns.Append(subject_Name1);
            table.Columns.Append(time);
            table.Columns.Append(pwd1);
            catalog.Tables.Append(table);           
            
            connection.Close();//关闭连接
        }

        private void userCombox_TextChanged(object sender, System.EventArgs e)
        {
            this.pwd.Text = "";
            if (userCombox.Text.ToLower().CompareTo("admin") == 0)
            {
                roleText.Text = "管理员";
            }
            else
                if (userCombox.Text.ToLower().CompareTo("security_officer") == 0)
                {
                    roleText.Text = "安全员";
                }
                else
                {
                    roleText.Text = "普通用户";
                }
        }

       
        //登录
        private void btn_enter_Click(object sender,  System.EventArgs e)
        {
            bool isLogin = false;

            if (SubjectRegister.IsUserNameUsed(userCombox.Text) == false)
            {
                MessageBox.Show(this, "数据库中找不到这样的账号", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (userCombox.Text.Length == 0 || pwd.Text.Length == 0)
            {
                MessageBox.Show(this, "账户名称及密码不能为空", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [主体信息表] where 主体名称=" + "'" + userCombox.Text.ToLower() + "'";
            OleDbDataReader dr = conn.ExecuteReader();
            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    if (dr[1].ToString().CompareTo(pwd.Text.GetHashCode().ToString()) == 0)//找到相应的记录，置登录标志为真
                    {
                        isLogin = true;
                    }
                }
            }
            dr.Close();
            oleDB.Close();
            if (isLogin == false)
            {
                MessageBox.Show(this, "密码错误", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show(this, "登录成功,按确定进入", "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Visible = false;

            switch (this.userCombox.Text.ToLower())//选择不同的窗口
            {
                case "admin":
                    {
                        Admin a = new Admin();
                        a.ShowDialog();
                        break;
                    }
                case "security_officer":
                    {
                        Security_Officer s= new Security_Officer();                    
                        s.ShowDialog();
                        break;
                    }
                default:
                    {
                        CommonUser c = new CommonUser();
                        c.GetUserName = this.userCombox.Text.ToLower();
                        c.ShowDialog();
                        break;
                    }
            }
            this.Visible = true;
        }

        private void btn_exit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btn_register_Click_1(object sender, System.EventArgs e)
        {
            if (SubjectRegister.IsUserNameUsed("admin") == false || SubjectRegister.IsUserNameUsed("security_officer")==false)//若数据库文件不存在或则管理员账号不存在，则创建新的文件或者管理员帐号并退出
            {
                if (SubjectRegister.IsUserNameUsed("admin") == false)
                {
                    MessageBox.Show(this, "未设置管理员账号，按确定设置管理员账号及密码", "设置管理员账号及密码", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SubjectRegister register = new SubjectRegister();
                    register.Text = "设置管理员账号及密码";
                    register.getIsAdmin = true;
                    register.ShowDialog();
                    Refresh_UserInfo();//将数据库中用户信息添加到用户列表中
                    return;

                }
                if (SubjectRegister.IsUserNameUsed("security_officer") == false)
                {
                    MessageBox.Show(this, "未设置安全员账号，按确定设置安全员员账号及密码", "设置安全员账号及密码", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SubjectRegister register = new SubjectRegister();
                    register.Text = "设置安全员账号及密码";
                    register.GetIsSecurityOfficer = true;
                    register.ShowDialog();
                    Refresh_UserInfo();//将数据库中用户信息添加到用户列表中
                    return;
                }
            }
            else
            {
                SubjectRegister register = new SubjectRegister();
                register.Text = "设置普通用户账号及密码";
                register.isAdmin = false;
                register.ShowDialog();
                Refresh_UserInfo();//将数据库中用户信息添加到用户列表中
            }
        }

        private void userCombox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }
    }
}
