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
    public partial class Security_Officer : Form
    {
        public Security_Officer()
        {
            InitializeComponent();
        }
        private void Main_Load(object sender, System.EventArgs e)
        {
           

            //显示系统时间
            this.timer.Start();
            this.timer.Interval = 100;
            this.timer.Tick += timer_Tick;

            Refresh_RoleTable();
           
        }
        //更新时间
        void timer_Tick(object sender, System.EventArgs e)
        {
            this.timeLabel.Text = DateTime.Now.ToString();

        }
    
        //以下是角色表选项卡的代码
        /// <summary>
        /// 更新角色表中的信息
        /// </summary>
        private void Refresh_RoleTable()
        {
            
            
             this.View_RolesData.Rows.Clear();
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [角色信息表]";
            OleDbDataReader dr = cmd.ExecuteReader();

            LinkedList<string> RoleName = new LinkedList<string>();//用于记录在角色信息表中出现的角色名称
            if (dr.HasRows == true)

                while (dr.Read())
                {
                    DataGridViewRow r = new DataGridViewRow();
                    r.CreateCells(this.View_RolesData);
                    
                    
                    r.Cells[0].Value = dr["角色名称"];
                    LinkedList<string> childRoles = GetDirectChildRoles(dr["角色名称"].ToString());//获取直接下级
                    while (childRoles.Count != 0)
                    {
                        r.Cells[1].Value += childRoles.First.Value + " ";
                        childRoles.RemoveFirst();
                    }
                    LinkedList<string> permission = Get_Role_Direct_Permissions(dr["角色名称"].ToString());
                    while (permission.Count != 0)
                    {
                        r.Cells[2].Value += permission.First.Value + " ";
                        permission.RemoveFirst();
                    }
                    LinkedList<string> User = Get_Roles_Direct_User(dr["角色名称"].ToString());
                    while (User.Count != 0)
                    {
                        r.Cells[3].Value += User.First.Value + " ";
                        User.RemoveFirst();
                    }

                    r.Cells[4].Value = dr["创建时间"];
                 
                  this.View_RolesData.Rows.Add(r);
                }
         
            dr.Close();
 
            oleDB.Close();
      
        
        }
        private  string  GetRoleCreatTime(string RoleName)
        {
            string time = string.Empty;
           
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色信息表] where 角色名称=" + "'" + RoleName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    time = dr["创建时间"].ToString();
                }
            }

            dr.Close();
            oleDB.Close();
          
            return time;
        }

      

        //以下是增加角色的选项卡代码
        //刷新该选项卡
        private void Refresh_RoleAdd()
        {
            this.CheckedList_ChildRole.Items.Clear();
            this.CheckedList_FatherRole.Items.Clear();
            this.Warning_RoleUsed.Visible = false;
            this.Txt_RoleName.Text = "";
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [角色信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)

                while (dr.Read())
                {
                    this.CheckedList_FatherRole.Items.Add(dr["角色名称"]);
                    this.CheckedList_ChildRole.Items.Add(dr["角色名称"]);
                }
            
            dr.Close();
            oleDB.Close();
      
        }
        //当角色名称改变时
        private void Txt_RoleName_TextChanged(object sender, EventArgs e)
        {


            if (IsUserNameUsed(Txt_RoleName.Text) == true )//角色名称已经被占用
            {
                this.Warning_RoleUsed.Visible = true;
               
               
            }
            else
            {
                this.Warning_RoleUsed.Visible = false;
               
            }

        }
        //rolename是否已经被占用
        public static bool IsUserNameUsed(string RoleName)
        {
            bool isUsed = false;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [角色信息表] where 角色名称=" + "'" + RoleName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = conn.ExecuteReader();
            isUsed = dr.HasRows;
            dr.Close();
            oleDB.Close();
            return isUsed;

        }
     

        //确定增加角色
        private void Btn_RegisterRole_Click(object sender, EventArgs e)
        {

            if (Txt_RoleName.Text.Length == 0)//角色名称已经被占用
            {
                MessageBox.Show(this, "角色名称不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (IsUserNameUsed(Txt_RoleName.Text) == true)//角色名称已经被占用
            {
                MessageBox.Show(this, "角色名称已经被占用，请重新输入", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (this.CheckedList_ChildRole.Items.Count==0 && this.CheckedList_FatherRole.Items.Count==0)//角色表暂无其他角色，不用添加上级角色或者下级角色
            {
                InsertRole(Txt_RoleName.Text.ToLower());
                MessageBox.Show(this, "添加成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Refresh_RoleAdd();
            }
            else
            {
                if (this.CheckedList_FatherRole.CheckedItems.Count == 0 && this.CheckedList_ChildRole.CheckedItems.Count == 0)
                {
                    if (MessageBox.Show(this, "上级角色或下级角色中未选中任何项，是否保持(按’是‘添加，’否‘返回)", "提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        InsertRole(Txt_RoleName.Text.ToLower());
                        MessageBox.Show(this, "添加成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Refresh_RoleAdd();
                        return;
                    }
                    else
                    {
                        return;
                    }

                }
                //检查上级角色及下级角色是否有相同的项，以及角色的添加是否会导致其他角色的关系错误
                for (int i = 0; i < this.CheckedList_FatherRole.CheckedItems.Count; i++)
                {
                    for (int j = 0; j < this.CheckedList_ChildRole.CheckedItems.Count; j++)
                    {
                        if (string.Equals(this.CheckedList_FatherRole.CheckedItems[i], this.CheckedList_ChildRole.CheckedItems[j]) == true)
                        {
                            MessageBox.Show(this, "上级角色及下级角色中有相同的项，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {

                            if (RoleRelationShipError(this.CheckedList_FatherRole.CheckedItems[i].ToString(),
                                this.CheckedList_ChildRole.CheckedItems[j].ToString()) == false)
                            {
                                MessageBox.Show(this, "此角色的添加将导致其他角色的上下级关系错误，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                
                                return;
                            }
                            if (IsExcursionSafety(this.CheckedList_FatherRole.CheckedItems[i].ToString(), this.CheckedList_ChildRole.CheckedItems[j].ToString()) == true)
                               {
                                   MessageBox.Show(this, "此角色的添加将导致互斥角色产生上下级关系，请重新选择或者删除互斥角色", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                  
                                   return;
                            }
                        }
                    }
                }
               
                InsertRole(Txt_RoleName.Text.ToLower());//添加到角色信息表中
                if (this.CheckedList_FatherRole.CheckedItems.Count == 0)//新创建 的角色无上级角色
                {
                    for (int i = 0; i < this.CheckedList_ChildRole.CheckedItems.Count; i++)
                    {

                       InsertRole_Relation(Txt_RoleName.Text.ToLower(),CheckedList_ChildRole.CheckedItems[i].ToString());
                    }
                }
                else
                    if (this.CheckedList_ChildRole.CheckedItems.Count == 0)//新创建 的角色无下级角色
                    {
                        for (int i = 0; i < this.CheckedList_FatherRole.CheckedItems.Count; i++)
                        {

                            InsertRole_Relation(CheckedList_FatherRole.CheckedItems[i].ToString(), Txt_RoleName.Text.ToLower());
                        }
                    }
                    else//新创建的角色上下级角色均有
                    {
                        for (int i = 0; i < this.CheckedList_ChildRole.CheckedItems.Count; i++)
                        {

                            InsertRole_Relation(Txt_RoleName.Text.ToLower(), CheckedList_ChildRole.CheckedItems[i].ToString());
                        }
                        for (int i = 0; i < this.CheckedList_FatherRole.CheckedItems.Count; i++)
                        {

                            InsertRole_Relation(CheckedList_FatherRole.CheckedItems[i].ToString(), Txt_RoleName.Text.ToLower());
                        }
                    
                    }
                MessageBox.Show(this, "添加成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Refresh_RoleAdd();
            }
            
        }

        /// <summary>
        /// 检测角色 fatherName ,childName之间若发生上下层次关系对于系统中互斥角色是否有影响
        /// </summary>
        /// <returns></returns>返回为真时 ，表示有影响，应禁止 

        private  bool IsExcursionSafety(string fatherName, string childName)
        {
            bool b = false;
            LinkedList<string> LinkFatherName = GetFatherRoles(fatherName);//找到fatherName的所有父级角色
            LinkedList<string> LinkChildName = GetChildRoles(childName);//找到childName的所有下级角色
            LinkedList<string> LinkChild_FatherName = GetFatherRoles(childName);//找到childName的所有上级角色,用于LinkFatherName中排除共同的上级
            if (LinkChildName.Find(fatherName) != null || LinkChild_FatherName.Find(fatherName) != null)//如果以前fatherName和childName存在上下级关系，则不会导致不安全
            {
                return b;
            }

            int i = 0;

            for (LinkedListNode<string> L = LinkChild_FatherName.First; i < LinkChild_FatherName.Count; i++, L = L.Next)
            {
                LinkFatherName.Remove(L.Value);
            }
            i = 0;
            //将fatherName 及childName也加入到检测中
            LinkFatherName.AddLast(fatherName);
            LinkChildName.AddLast(childName);

            for (LinkedListNode<string> F = LinkFatherName.First; i < LinkFatherName.Count; i++, F = F.Next)
            {
                int j = 0;
                for (LinkedListNode<string> C = LinkChildName.First; j < LinkChildName.Count; j++, C = C.Next)
                {
                    if (IsRoleExcursionExist(F.Value, C.Value) == true)//找到互斥的角色，说明导致系统不安全，应禁止
                    {
                        return true;
                    }
                    
                }
            }
           
             

            return b;
        }
        /// <summary>
        /// 查询互斥角色表中是否存在参数参数name1 name2的信息
        /// </summary>
        /// <param name="Name1"></param>
        /// <param name="Name2"></param>
        /// <returns></returns>
        private  bool IsRoleExcursionExist(string Name1, string Name2)
        {
            bool b = false;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [互斥角色信息表] where 角色1名称=" + "'" + Name1.ToLower() + "'" + "and 角色2名称=" + "'" + Name2.ToLower() + "'";  //根据角色名称查找

            OleDbDataReader dr = cmd.ExecuteReader();
            b = dr.HasRows;
            dr.Close();
            if (b == false)//未找到，更换查询条件
            {
                cmd.CommandText = "select *  from  [互斥角色信息表] where 角色1名称=" + "'" + Name2.ToLower() + "'" + "and 角色2名称=" + "'" + Name1.ToLower() + "'";  //根据角色名称查找

                dr = cmd.ExecuteReader();
                b = dr.HasRows;
                dr.Close();

            }
            oleDB.Close();
            return b;
        }
        /// <summary>
        /// 插入新角色到数据库的角色信息表中
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="ChildRoleName"></param>
        private void InsertRole(string RoleName)
        {
            //将注册信息插入到注册表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //插入到角色信息中
            cmd.CommandText = "insert into  [角色信息表](角色名称,创建时间) values('" +  RoleName. ToLower() + "','" +  DateTime.Now.ToString() + "')";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }
        /// <summary>
        /// 插入新角色到数据库的角色关系表中
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="ChildRoleName"></param>
        private void InsertRole_Relation(string FatherName,string ChildName)
        {
            //将注册信息插入到注册表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //插入到角色关系信息表中
            cmd.CommandText = "insert into  [角色关系信息表](父角色名称,子角色名称) values('" + FatherName.ToLower() + "','" + ChildName + "')";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }
        
        /// <summary>
        /// 检查选中的上级及下级角色是否导致系统不安全
        /// </summary>
        /// <param name="fatherRole"></param>
        /// <param name="childRole"></param>
        /// <returns></returns>返回为假表示不安全状态，真表示安全状态>

        private bool RoleRelationShipError(string  fatherRole,string childRole)
        {
            bool b = true;

            LinkedList<string> List_Father = GetFatherRoles(fatherRole);//获取父角色的所有父角色
            if (List_Father.Find(childRole) != null)//若在父角色中找到了子角色名称，则表示不安全，返回假
            {
                b = false;
                return b;
            }
            LinkedList<string> List_Child = GetChildRoles(childRole);//获取子角色的所有子角色
            if (List_Child.Find(fatherRole) != null)//若在子角色中找到了父角色名称，则表示不安全，返回假
            {
                b = false;
                return b;
            }
            return b;  
        }
        /// <summary>
        /// 获取某个角色的所有上级角色,形成链表返回
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static LinkedList<string> GetFatherRoles(string roleName)
        {
            LinkedList<string> LinkRole = new LinkedList<string>();


            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色关系信息表] where 子角色名称=" + "'" + roleName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (string.Equals(dr["父角色名称"], "(null)") == false)//存在父级角色
                    {
                        if (LinkRole.Find(dr["父角色名称"].ToString()) == null)//当前链表总不存在该名称时才查找
                        {
                            LinkedListNode<string> L = new LinkedListNode<string>(dr["父角色名称"].ToString());//将父级角色添加到链表的第一个位置
                            LinkRole.AddLast(L);

                            LinkedList<string> returnList = GetFatherRoles(dr["父角色名称"].ToString());//递归调用，查找下一个可能的父角色名称
                            int i = 0;
                            for (LinkedListNode<string> position = returnList.First; i < returnList.Count; i++, position = position.Next)//连接两个双向链表的代码
                            {
                                 
                                LinkedListNode<string> l = new LinkedListNode<string>(position.Value);
                                
                                LinkRole.AddLast(l);
                            }
                        }
                    }

                }
            }
         
            dr.Close();
            oleDB.Close();
            return LinkRole;
        }
        /// <summary>
        /// 获取某个角色的所有下级角色，形成链表返回
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static LinkedList<string> GetChildRoles(string roleName)
        {
            LinkedList<string> LinkRole = new LinkedList<string>();

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色关系信息表] where 父角色名称=" + "'" + roleName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (string.Equals(dr["子角色名称"], "(null)") == false)//存在父级角色
                    {
                        if (LinkRole.Find(dr["子角色名称"].ToString()) == null)//当前链表总不存在该名称时才查找
                        {
                            LinkedListNode<string> L = new LinkedListNode<string>(dr["子角色名称"].ToString());//将父级角色添加到链表的第一个位置
                            LinkRole.AddLast(L);

                            LinkedList<string> returnList = GetChildRoles(dr["子角色名称"].ToString());//递归调用，查找下一个可能的父角色名称
                            int i = 0;
                            for (LinkedListNode<string> position = returnList.First; i < returnList.Count; i++, position = position.Next)//连接两个双向链表的代码
                            {
                                LinkedListNode<string> l = new LinkedListNode<string>(position.Value);
                                LinkRole.AddLast(l);
                            }
                        }
                    }

                }
            }

            dr.Close();
            oleDB.Close();
            return LinkRole;
        }
        //以下是选项卡3 角色查询的代码

        //更新查询列表
        private void Refresh_RolesInquire()
        {
           
            this.Combox_RoleName.Items.Clear();
            this.Label_Time.Visible = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [角色信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)

                while (dr.Read())
                 
                {
                    this.Combox_RoleName.Items.Add(dr["角色名称"]);
                     
                }
            if (this.Combox_RoleName.Items.Count != 0)
            {
                this.Combox_RoleName.SelectedIndex = 0;
            }

            dr.Close();
            oleDB.Close();
        }
        private void Combox_RoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBox_ChildRoleName.Items.Clear();
            this.listBox_FatherRoleName.Items.Clear();
            this.ListBox_Roles_Permission.Items.Clear();
            this.ListBox_Role_User.Items.Clear();

            this.Label_Time.Text = GetRoleCreatTime(this.Combox_RoleName.Text);
            this.Label_Time.Visible = true;
            LinkedList<string> FatherList = GetFatherRoles(this.Combox_RoleName.Text);
            LinkedList<string> ChildList = GetChildRoles(this.Combox_RoleName.Text);
            int i = 0 ;
 
            for (LinkedListNode<string> f = FatherList.First; i < FatherList.Count; i++, f = f.Next)
            {
                if (this.listBox_FatherRoleName.Items.IndexOf(f.Value) == -1)
                {
                    this.listBox_FatherRoleName.Items.Add(f.Value);
                }
               
              
              
            }
            FatherList.AddLast(this.Combox_RoleName.Text);
            i = 0;
          
            
            for (LinkedListNode<string> c = ChildList.First; i < ChildList.Count; i++, c = c.Next)
            {
                if (this.listBox_ChildRoleName.Items.IndexOf(c.Value)==-1)
                {

                    this.listBox_ChildRoleName.Items.Add(c.Value);
                }
                
               
            }
            ChildList.AddLast(this.Combox_RoleName.Text);
            while (ChildList.Count != 0)
            {
                LinkedList<string> permission = Get_Role_Direct_Permissions( ChildList.First.Value);
                while (permission.Count != 0)
                {

                    if (this.ListBox_Roles_Permission.Items.IndexOf((permission.First.Value)) == -1)
                    {
                        this.ListBox_Roles_Permission.Items.Add(permission.First.Value);

                    }
                    permission.RemoveFirst();

                }
                ChildList.RemoveFirst();
            }


            while (FatherList.Count != 0)
            {

                LinkedList<string> Users = Get_Roles_Direct_User(FatherList.First.Value);
                while (Users.Count != 0)
                {

                    if (this.ListBox_Role_User.Items.IndexOf(Users.First.Value) == -1)
                    {
                        this.ListBox_Role_User.Items.Add(Users.First.Value);
                    }
                    Users.RemoveFirst();

                }
                FatherList.RemoveFirst();
            }
           
        }
      //以下是选项卡4-角色管理的代码

        private void Refresh_RoleMangement(int index)
        {
        
            this.ComBox_MangementRoleName.Items.Clear();
            //添加所有角色到角色下拉列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [角色信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)

                while (dr.Read())
                {
                    this.ComBox_MangementRoleName.Items.Add(dr["角色名称"]);
               
                }
            if (this.ComBox_MangementRoleName.Items.Count != 0)
            {
                this.ComBox_MangementRoleName.SelectedIndex = index;
            }

            dr.Close();
            oleDB.Close();
        }
        private void ComBox_MangementRoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CheckedListBox_DirectChildRole.Items.Clear();
            this.CheckedListBox_DirectFatherRole.Items.Clear();
            this.ListBox_DirectChildName.Items.Clear();
            this.ListBox_DirectFatherName.Items.Clear();

            LinkedList<string> DirectFatherList = GetDirectFatherRoles(this.ComBox_MangementRoleName.Text);
            LinkedList<string> DirectChildList = GetDirectChildRoles(this.ComBox_MangementRoleName.Text);
            int i = 0;

            for (LinkedListNode<string> f = DirectFatherList.First; i < DirectFatherList.Count; i++, f = f.Next)
            {
                this.ListBox_DirectFatherName.Items.Add(f.Value);

            }
            i = 0;
            for (LinkedListNode<string> c = DirectChildList.First; i < DirectChildList.Count; i++, c = c.Next)
            {
                this.ListBox_DirectChildName.Items.Add(c.Value);
            }

            //添加所有的角色到可选的上级 、下级角色框中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [角色信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)

                while (dr.Read())
                {
                    this.CheckedListBox_DirectChildRole.Items.Add(dr["角色名称"]);
                    this.CheckedListBox_DirectFatherRole.Items.Add(dr["角色名称"]);
                                      
                }
            dr.Close();
            oleDB.Close();

            //将已经有的上级角色的选项部分置为True
            foreach (var I in this.ListBox_DirectChildName.Items)
            {
               this.CheckedListBox_DirectChildRole.SetItemChecked(
                   this.CheckedListBox_DirectChildRole.Items.IndexOf(I),true);
            }
            foreach (var I in this.ListBox_DirectFatherName.Items)
            {
                this.CheckedListBox_DirectFatherRole.SetItemChecked(
                  this.CheckedListBox_DirectFatherRole.Items.IndexOf(I), true);
            }
            this.CheckedListBox_DirectChildRole.Items.Remove(this.ComBox_MangementRoleName.Text);//移除自身
            this.CheckedListBox_DirectFatherRole.Items.Remove(this.ComBox_MangementRoleName.Text);
           
        }
        /// <summary>
        /// 获取某个角色的所有直接上级级角色,形成链表返回
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static LinkedList<string> GetDirectFatherRoles(string roleName)
        {
            LinkedList<string> LinkRole = new LinkedList<string>();


            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色关系信息表] where 子角色名称=" + "'" + roleName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (string.Equals(dr["父角色名称"], "(null)") == false)//存在父级角色
                    {
                        if (LinkRole.Find(dr["父角色名称"].ToString()) == null)//当前链表总不存在该名称时才添加
                        {
                            LinkedListNode<string> L = new LinkedListNode<string>(dr["父角色名称"].ToString());//将父级角色添加到链表的第一个位置
                            LinkRole.AddLast(L);
                        }
                    }

                }
            }

            dr.Close();
            oleDB.Close();
            return LinkRole;
        }
        /// <summary>
        /// 获取某个角色的所有直接下级角色，形成链表返回
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static LinkedList<string> GetDirectChildRoles(string roleName)
        {
            LinkedList<string> LinkRole = new LinkedList<string>();


            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色关系信息表] where 父角色名称=" + "'" + roleName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (string.Equals(dr["子角色名称"], "(null)") == false)//存在子级角色
                    {
                        if (LinkRole.Find(dr["子角色名称"].ToString()) == null)//当前链表总不存在该名称时才查找
                        {
                            LinkedListNode<string> L = new LinkedListNode<string>(dr["子角色名称"].ToString());//将父级角色添加到链表的第一个位置
                            LinkRole.AddLast(L);
                        }

                    }

                }
            }

            dr.Close();
            oleDB.Close();
            return LinkRole;
        }
        //删除角色
        private void Btn_DeleteRole_Click(object sender, EventArgs e)
        {
            //删除角色时，需要将其所有的直接上级角色与直接下级角色重新对应，同时，指派到角色的用户应该指派到该角色的所有下级角色，指派到该角色的权限应该指派到该角色的所有上级角色
            LinkedList<string> DirectFatherList = GetDirectFatherRoles(this.ComBox_MangementRoleName.Text);//获取直接上下级角色
            LinkedList<string> DirectChildList = GetDirectChildRoles(this.ComBox_MangementRoleName.Text);
            LinkedList<string> DirectUsersList = Get_Roles_Direct_User(this.ComBox_MangementRoleName.Text);
            LinkedList<string> DirectPermissionList =  Get_Role_Direct_Permissions (this.ComBox_MangementRoleName.Text);
            
            int i = 0;
            if (DirectFatherList.Count != 0 && DirectChildList.Count != 0)//上级或者下级角色为空，则不需要将角色关系写进数据库的角色关系表中
            {
              
                for (LinkedListNode<string> f = DirectFatherList.First; i < DirectFatherList.Count; i++, f = f.Next)
                {
                    int j = 0;
                    for (LinkedListNode<string> c = DirectChildList.First; j < DirectChildList.Count; j++, c = c.Next)
                    {
                        if (IsRoleRelationExist(f.Value, c.Value) == false)//如果数据库中不存在该关系
                        {
                            InsertRole_Relation(f.Value, c.Value);//插入到角色关系表中
                        }
                    }

                }               
            }
            i = 0;
            //删除直接上级角色和要删除的角色之间的信息，以及下级角色和要删除的角色之间的信息
            for (LinkedListNode<string> f = DirectFatherList.First; i < DirectFatherList.Count; i++, f = f.Next)
            {
                DeleteRole_Relation(f.Value, this.ComBox_MangementRoleName.Text);
                int j = 0;
                for (LinkedListNode<string> p = DirectPermissionList.First; j < DirectPermissionList.Count; j++, p = p.Next)
                {
                    if (IsRolePermissionRelationExist(f.Value, p.Value) == false)
                    {
                        Insert_Permission_Roles(p.Value, f.Value);

                    }

                }
            }
            i = 0;
            for (LinkedListNode<string> c = DirectChildList.First; i < DirectChildList.Count; i++, c = c.Next)
            {
                DeleteRole_Relation(this.ComBox_MangementRoleName.Text,c.Value);
                int j = 0;
                for (LinkedListNode<string> u = DirectUsersList.First; j < DirectUsersList.Count; j++, u = u.Next)
                {
                    if (IsRoleUserRelationExist(c.Value,u.Value) == false)
                    {
                        Insert_User_Roles(c.Value, u.Value);
                    }
                }       
                    
            }
            //最后删除角色的具体信息,以及在互斥角色中的信息
            DeleteRole(this.ComBox_MangementRoleName.Text);
            DeleteRole_Excursion(this.ComBox_MangementRoleName.Text);

            MessageBox.Show(this, "删除角色成功，相应的角色关系已调整", "删除成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_RoleMangement(this.ComBox_MangementRoleName.SelectedIndex);
           
        }
        /// <summary>
        /// 删除数据库的互斥角色表中的一个角色
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="ChildRoleName"></param>
        private void DeleteRole_Excursion(string Name)
        {

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色关系信息表中删除
            cmd.CommandText = "delete from  [互斥角色信息表]   where  角色1名称=" + "'" + Name.ToLower() + "'" + "or 角色2名称=" + "'" + Name.ToLower() + "'";
            cmd.ExecuteNonQuery();

            oleDB.Close();
        }
       
        /// <summary>
        /// 查看角色关系表中已经存在该信息，为True表示有
        /// </summary>
        /// <param name="FatherName"></param>
        /// <param name="ChildName"></param>
        private bool IsRoleRelationExist(string FatherName, string ChildName)
        {
            bool b = false;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色关系信息表中删除
            cmd.CommandText = "select * from  [角色关系信息表]   where  父角色名称=" + "'" + FatherName.ToLower() + "'" + " and 子角色名称=" + "'" + ChildName.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();
            b = dr.HasRows;
            dr.Close();            
            oleDB.Close();
            return b;
        }
        /// <summary>
        /// 查看用户-角色指派信息表中已经存在该信息，为True表示有
        /// </summary>
        /// <param name="RoleName"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        private bool IsRoleUserRelationExist(string RoleName, string UserName)
        {
            bool b = false;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色关系信息表中删除
            cmd.CommandText = "select * from  [用户-角色指派信息表]   where  角色名称=" + "'" + RoleName.ToLower() + "'" + " and 用户名称=" + "'" + UserName.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();
            b = dr.HasRows;
            dr.Close();
            oleDB.Close();
            return b;
        }
        private bool IsRolePermissionRelationExist(string RoleName, string PermissionName)
        {
            bool b = false;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色关系信息表中删除
            cmd.CommandText = "select * from  [角色-权限指派信息表]   where  角色名称=" + "'" + RoleName.ToLower() + "'" + " and 权限名称=" + "'" + PermissionName.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();
            b = dr.HasRows;
            dr.Close();
            oleDB.Close();
            return b;
        }

        /// <summary>
        /// 删除数据库的角色关系表中的一条记录
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="ChildRoleName"></param>
        private void DeleteRole_Relation(string FatherName, string ChildName)
        {
            
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色关系信息表中删除
            cmd.CommandText = "delete from  [角色关系信息表]   where  父角色名称=" + "'" + FatherName.ToLower() + "'" + " and 子角色名称=" + "'" + ChildName.ToLower() + "'";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }
        /// <summary>
        /// 根据提供的角色名称删除数据库的角色关系表中的记录，
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="ChildRoleName"></param>
        private void DeleteRole_Relation(string Name )
        {

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色关系信息表中删除
            cmd.CommandText = "delete from  [角色关系信息表]   where  父角色名称=" + "'" + Name.ToLower()+"'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from  [角色关系信息表]   where  子角色名称=" + "'" + Name.ToLower()+"'";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }
        /// <summary>
        /// 删除数据库 中的一个角色
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="ChildRoleName"></param>
        private void DeleteRole(string RoleName)
        {
            
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            
            cmd.CommandText = "delete from  [角色信息表]   where  角色名称=" + "'" + RoleName.ToLower() + "'";
             cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from  [角色-权限指派信息表]   where  角色名称=" + "'" + RoleName.ToLower() + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from  [用户-角色指派信息表]   where  角色名称=" + "'" + RoleName.ToLower() + "'";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }

        //确认更改角色之间的关系
        private void Btn_ChangeRole_Click(object sender, EventArgs e)
        {
            LinkedList<StoreRoles> Link = GetRole_Relation(this.ComBox_MangementRoleName.Text);//首先存储所有包含ComBox_MangementRoleName.Text的角色关系表的信息

            DeleteRole_Relation(this.ComBox_MangementRoleName.Text); //首先删除所有包含ComBox_MangementRoleName.Text的角色关系表的信息

            //检查上级角色及下级角色是否有相同的项，以及角色的添加是否会导致其他角色的关系错误
            for (int i = 0; i < this.CheckedListBox_DirectFatherRole.CheckedItems.Count; i++)
            {
                for (int j = 0; j < this.CheckedListBox_DirectChildRole.CheckedItems.Count; j++)
                {
                    if (string.Equals(this.CheckedListBox_DirectFatherRole.CheckedItems[i], this.CheckedListBox_DirectChildRole.CheckedItems[j]) == true)
                    {
                        MessageBox.Show(this, "上级角色及下级角色中有相同的项，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        InsertRole_Relation(Link);//重新插入
                        Refresh_RoleMangement(this.ComBox_MangementRoleName.SelectedIndex);
                        return;
                    }
                    else
                    {

                        if (RoleRelationShipError(this.CheckedListBox_DirectFatherRole.CheckedItems[i].ToString(),
                            this.CheckedListBox_DirectChildRole.CheckedItems[j].ToString()) == false)
                        {
                            MessageBox.Show(this, "此角色的更改将导致其他角色的上下级关系错误，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Refresh_RoleMangement(this.ComBox_MangementRoleName.SelectedIndex);
                            InsertRole_Relation(Link);//重新插入
                            return;
                        }
                        if (IsExcursionSafety(this.CheckedListBox_DirectFatherRole.CheckedItems[i].ToString(), this.CheckedListBox_DirectChildRole.CheckedItems[j].ToString()) == true)
                        {
                            MessageBox.Show(this, "此角色的添加将导致互斥角色产生上下级关系，请重新选择或者删除互斥角色", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Refresh_RoleMangement(this.ComBox_MangementRoleName.SelectedIndex);
                            InsertRole_Relation(Link);//重新插入
                            return;
                        }
                    }
                }
            }
           
            if (this.CheckedListBox_DirectFatherRole.CheckedItems.Count == 0)//更改的角色无上级角色
            {
                for (int i = 0; i < this.CheckedListBox_DirectChildRole.CheckedItems.Count; i++)
                {

                    if (IsRoleRelationExist(this.ComBox_MangementRoleName.Text.ToLower(), this.CheckedListBox_DirectChildRole.CheckedItems[i].ToString()) == false)//如果数据库中不存在该关系
                    {
                        InsertRole_Relation(this.ComBox_MangementRoleName.Text.ToLower(), this.CheckedListBox_DirectChildRole.CheckedItems[i].ToString()); //插入到角色关系表中
                    }

                }
            }
            else
                if (this.CheckedListBox_DirectChildRole.CheckedItems.Count == 0)//更改的角色无下级角色
                {
                    for (int i = 0; i < this.CheckedListBox_DirectFatherRole.CheckedItems.Count; i++)
                    {
                        if (IsRoleRelationExist(this.CheckedListBox_DirectFatherRole.CheckedItems[i].ToString(), this.ComBox_MangementRoleName.Text.ToLower()) == false)//如果数据库中不存在该关系
                        {
                            InsertRole_Relation(this.CheckedListBox_DirectFatherRole.CheckedItems[i].ToString(), this.ComBox_MangementRoleName.Text.ToLower());
                        }
                    }
                }
                else//更改的角色上下级角色均有
                {
                    for (int i = 0; i < this.CheckedListBox_DirectChildRole.CheckedItems.Count; i++)
                    {
                        if (IsRoleRelationExist(this.ComBox_MangementRoleName.Text.ToLower(), this.CheckedListBox_DirectChildRole.CheckedItems[i].ToString()) == false)//如果数据库中不存在该关系
                        {
                            InsertRole_Relation(this.ComBox_MangementRoleName.Text.ToLower(), this.CheckedListBox_DirectChildRole.CheckedItems[i].ToString());
                        }
                    }
                    for (int i = 0; i < this.CheckedListBox_DirectFatherRole.CheckedItems.Count; i++)
                    {
                        if (IsRoleRelationExist(this.CheckedListBox_DirectFatherRole.CheckedItems[i].ToString(), this.ComBox_MangementRoleName.Text.ToLower()) == false)//如果数据库中不存在该关系
                        {

                            InsertRole_Relation(this.CheckedListBox_DirectFatherRole.CheckedItems[i].ToString(), this.ComBox_MangementRoleName.Text.ToLower());
                        }
                    }

                }
            MessageBox.Show(this, "更改成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private  LinkedList<StoreRoles> GetRole_Relation(string Name)
        {
            LinkedList<StoreRoles> S = new LinkedList<StoreRoles>();
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色关系信息表中删除
            cmd.CommandText = "select * from  [角色关系信息表]   where  父角色名称=" + "'" + Name.ToLower() + "'" + "or  子角色名称=" + "'" + Name.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LinkedListNode<StoreRoles> T = new LinkedListNode<StoreRoles>(new StoreRoles(dr["父角色名称"].ToString(), dr["子角色名称"].ToString()));
                    S.AddLast(T);
                }
            }
           
            dr.Close();
            oleDB.Close();
            return S;
        }
        private  void InsertRole_Relation(LinkedList<StoreRoles> S)
        {

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色关系信息表中删除
            int i = 0;
            for (LinkedListNode<StoreRoles> L = S.Last; i < S.Count; i++, L = L.Previous)
            {
                //插入到角色关系信息表中
                cmd.CommandText = "insert into  [角色关系信息表](父角色名称,子角色名称) values('" + L.Value.FatherName + "','" + L.Value.ChildName + "')";
                cmd.ExecuteNonQuery();
            }

            oleDB.Close();

        }
      
        //以下是选项卡5-互斥角色管理及查询的代码

        void Refresh_ExcursionRole(int index)
        {
            this.CheckedListBox_ExcursionAdd.Items.Clear();
            this.CheckedListBox_ExcursionCancel.Items.Clear();
            this.ComboBox_ExcursionRole.Items.Clear();
            //首先绑定数据源到DataGridView_ExcursionRole中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbDataAdapter da = new OleDbDataAdapter("select * from [互斥角色信息表]",oleDB);
            DataSet ds = new DataSet();
            da.Fill(ds,"互斥角色信息表");
            DataGridView_ExcursionRole.DataSource = ds.Tables[0];
           

            //将所有的互斥角色名称添加到ComboBox_ExcursionRole中
          

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [角色信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)

                while (dr.Read())
                {
                    this.ComboBox_ExcursionRole.Items.Add(dr["角色名称"]);

                }
            if (this.ComboBox_ExcursionRole.Items.Count != 0)
            {
                this.ComboBox_ExcursionRole.SelectedIndex = index;
            }

            dr.Close();
            oleDB.Close();
            
        
        }
        //
        private void ComboBox_ExcursionRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CheckedListBox_ExcursionAdd.Items.Clear();
            this.CheckedListBox_ExcursionCancel.Items.Clear();
            LinkedList<string> L = GetExcursionRoles(this.ComboBox_ExcursionRole.Text);
          
            int i = 0;
            for(LinkedListNode<string> l = L.Last;i<L.Count;i++,l=l.Previous)
            
            {

                this.CheckedListBox_ExcursionCancel.Items.Add(l.Value);//添加与其互斥的角色
            }
            LinkedList<string> LinkExcursionRole = GetRolesCanExcursion(this.ComboBox_ExcursionRole.Text);
             i = 0;
            for (LinkedListNode<string> l = LinkExcursionRole.Last; i < LinkExcursionRole.Count; i++, l = l.Previous)
            {

                this.CheckedListBox_ExcursionAdd.Items.Add(l.Value);//添加与其互斥的角色
            }
        }
       
        //返回与RoleName互斥的所有角色,不包括自身
        private LinkedList <string> GetExcursionRoles(string RoleName)
        {
            LinkedList<string> LinkRole = new LinkedList<string>();
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [互斥角色信息表] where 角色1名称=" + "'" + RoleName.ToLower() + "'"+"or 角色2名称=" + "'" + RoleName.ToLower() + "'";//根据角色名称查找
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (string.Equals(dr["角色1名称"], RoleName.ToLower()) == true)//角色1名称为RoleName，添加角色2到链表中
                    {
                        if (LinkRole.Find(dr["角色2名称"].ToString()) == null)//不添加重复的角色名称
                        {
                            LinkedListNode<string> L = new LinkedListNode<string>(dr["角色2名称"].ToString());//将角色2添加到链表的第一个位置
                            LinkRole.AddLast(L);
                        }
                       
                    }
                    if (string.Equals(dr["角色2名称"], RoleName.ToLower()) == true)//角色1名称为RoleName，添加角色2到链表中
                    {
                        if (LinkRole.Find(dr["角色1名称"].ToString()) == null)//不添加重复的角色名称
                        {
                            LinkedListNode<string> L = new LinkedListNode<string>(dr["角色1名称"].ToString());//将角色1添加到链表的第一个位置
                            LinkRole.AddLast(L);

                        }
                    }

                }
            }

            dr.Close();
            oleDB.Close();
            return LinkRole;
        }

        //返回能设为与RoleName互斥的所有角色,不包括自身
        private LinkedList<string> GetRolesCanExcursion(string RoleName)
        {
            LinkedList<string> LinkRole = new LinkedList<string>();
            //返回RoleName的父角色 子角色  及与其互斥的角色
            LinkedList<string> LinkFatherRole = GetFatherRoles(RoleName);//返回其父级角色
            LinkedList<string> LinkChildRole = GetChildRoles(RoleName);//返回其子级角色
            LinkedList<string> LinkExcursionRole =GetExcursionRoles(RoleName);//返回已经与其互斥的角色
            //找到以上三种角色后，其他角色可与其设为互斥角色
              //从 this.ComboBox_ExcursionRole中排除以上三种角色
            foreach(var v in this.ComboBox_ExcursionRole.Items)
            {
                if (string.Equals(v.ToString(),RoleName)==false && LinkFatherRole.Find(v.ToString()) == null && LinkChildRole.Find(v.ToString()) == null && LinkExcursionRole.Find(v.ToString()) == null)
                {
                    LinkRole.AddLast(v.ToString());
                }
           }
            　
            return LinkRole;
        }
        private void Btn_ExcursionCancel_Click(object sender, EventArgs e)
        {
           
            //未选中任何一项
            if (this.CheckedListBox_ExcursionCancel.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "还未选中任何一项，请重新选择后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var v in this.CheckedListBox_ExcursionCancel.CheckedItems)
            {
                DeleteRole_Excursion(this.ComboBox_ExcursionRole.Text, v.ToString());
 
            }
                  
          
            MessageBox.Show(this, "删除互斥角色成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_ExcursionRole(this.ComboBox_ExcursionRole.SelectedIndex);
        }
        /// <summary>
        /// 删除数据库的互斥角色表中的一条记录
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="ChildRoleName"></param>
        private void DeleteRole_Excursion(string Name1, string Name2)
        {

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色关系信息表中删除
            cmd.CommandText = "delete from  [互斥角色信息表]   where  角色1名称=" + "'" + Name1.ToLower() + "'" + " and 角色2名称=" + "'" + Name2.ToLower() + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from  [互斥角色信息表]   where  角色1名称=" + "'" + Name2.ToLower() + "'" + " and 角色2名称=" + "'" + Name1.ToLower() + "'";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }
        //添加互斥
        private void Btn_ExcursionAdd_Click(object sender, EventArgs e)
        {
       
            //未选中任何一项
            if (this.CheckedListBox_ExcursionAdd.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "还未选中任何一项，请重新选择后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (var v in this.CheckedListBox_ExcursionAdd.CheckedItems)
            {
                InsertRole_Excursion(this.ComboBox_ExcursionRole.Text, v.ToString());
            }
          

            
            MessageBox.Show(this, "增加互斥角色对成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_ExcursionRole(this.ComboBox_ExcursionRole.SelectedIndex);
        }

        /// <summary>
        /// 向数据库的互斥角色表中添加一条记录
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="ChildRoleName"></param>
        private void InsertRole_Excursion(string Name1, string Name2)
        {
              //互斥信息插入到注册表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //插入到角色互斥信息表中
            cmd.CommandText = "insert into  [互斥角色信息表](角色1名称,角色2名称) values('" + Name1.ToLower() + "','" + Name2.ToLower() + "')";
            cmd.ExecuteNonQuery();
        
          
            oleDB.Close(); 
        }





        // 选项卡-用户表的代码
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
                        while (Author_Roles.Count != 0)
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
                            r.Cells[3].Value += Permission.First.Value + " ";
                            Permission.RemoveFirst();
                        }
                        this.View_UsersData.Rows.Add(r);
                    }
                }
            dr.Close();
            oleDB.Close();


        }
  
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
        //选择不同的用户进行查询
        private void ComboBox_UserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CheckedListBox_AssignRole.Items.Clear();
            this.CheckedListBox_AssignRoleAdd.Items.Clear();
            this.ListBox_AuthorizeRole.Items.Clear();
            this.ListBox_Permissions.Items.Clear();
           

         
            //添加所有的角色到可选的指派角色框中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [角色信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)

                while (dr.Read())
                {
                    this.CheckedListBox_AssignRoleAdd.Items.Add(dr["角色名称"]);
                     

                }
            dr.Close();
            oleDB.Close();

            LinkedList<string> Direct_Roles = Get_User_Direct_Roles(ComboBox_UserName.Text);//获取用户的指派角色
            LinkedList<string> Roles = new LinkedList<string>();//获取用户拥有的所有角色(包括继承的);

            int i = 0;
            for (LinkedListNode<string> L = Direct_Roles.First; i < Direct_Roles.Count; i++, L = L.Next)
            {
                LinkedList<string> ChildRole = GetChildRoles(L.Value);//获取指定角色的所有下级角色，添加到授权列表中

               
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
                this.CheckedListBox_AssignRole.Items.Add(Direct_Roles.First.Value);
                //从可指派的角色列表中删除对应已指派的角色
                this.CheckedListBox_AssignRoleAdd.Items.Remove(Direct_Roles.First.Value);
                this.ListBox_AuthorizeRole.Items.Remove(Direct_Roles.First.Value);

                Direct_Roles.RemoveFirst();

            }
            while (Roles.Count != 0)
            {

                LinkedList<string> permisssion = Get_Role_Direct_Permissions(Roles.First.Value);
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
        //获取角色拥有的直接权限，形成链表返回
        public static LinkedList<string> Get_Role_Direct_Permissions(string roleName)
        {
            LinkedList<string> LinkPermission= new LinkedList<string>();


            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色-权限指派信息表] where 角色名称=" + "'" + roleName.ToLower() + "'"; 
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LinkPermission.AddLast(dr["权限名称"].ToString());

                }
            }

            dr.Close();
            oleDB.Close();
            return LinkPermission;
        }

        //获取用户拥有的直接角色，形成链表返回
        public static LinkedList<string> Get_User_Direct_Roles(string userName)
        {
            LinkedList<string> LinkRole = new LinkedList<string>();

        
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [用户-角色指派信息表] where 用户名称=" + "'" + userName + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LinkRole.AddLast(dr["角色名称"].ToString());                   

                }
            }

            dr.Close();
            oleDB.Close();
            return LinkRole;
        }
        //取消用户 - 角色指派时
        private void Btn_User_Roles_Delete_Click(object sender, EventArgs e)
        {
            if (this.CheckedListBox_AssignRole.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "未选中任何一项，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            for (int i = 0; i < this.CheckedListBox_AssignRole.CheckedItems.Count; i++)
            {
                Delete_User_Roles(this.CheckedListBox_AssignRole.CheckedItems[i].ToString(), this.ComboBox_UserName.Text);
            }
            
            MessageBox.Show(this, "撤销指派成功", "成功撤销指派", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_Users_Roles(this.ComboBox_UserName.SelectedIndex);
          
        }
        //增加用户角色指派
        private void Btn_User_Roles_Add_Click(object sender, EventArgs e)
        {
            if (this.CheckedListBox_AssignRoleAdd.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "未选中任何一项，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保证已有的角色和即将添加的角色不是互斥角色再添加到数据库中
            for (int i = 0; i < this.CheckedListBox_AssignRoleAdd.CheckedItems.Count; i++)
            {
                for (int j = 0; j <this.CheckedListBox_AssignRole.Items.Count; j++)
                {
                    //两者互斥
                    if (IsRoleExcursionExist(this.CheckedListBox_AssignRoleAdd.CheckedItems[i].ToString(), this.CheckedListBox_AssignRole.Items[j].ToString()))
                    {
                        MessageBox.Show(this, "互斥角色 -" + this.CheckedListBox_AssignRoleAdd.CheckedItems[i].ToString()+"-和-"
                            + this.CheckedListBox_AssignRole.Items[j].ToString()+"-不能同时授予同一用户,请更改互斥角色或者重新选择其他角色", "用户不能同时拥有互斥角色", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }   
                }
               // Insert_User_Roles(this.CheckedListBox_AssignRoleAdd.CheckedItems[i].ToString(), this.ComboBox_UserName.Text);
            }
            //即将添加的角色内部之间不能有互斥角色
            for (int i = 0; i < this.CheckedListBox_AssignRoleAdd.CheckedItems.Count; i++)
            {
                for (int j = i+1; j < this.CheckedListBox_AssignRoleAdd.CheckedItems.Count; j++)
                {
                    if (IsRoleExcursionExist(this.CheckedListBox_AssignRoleAdd.CheckedItems[i].ToString(), this.CheckedListBox_AssignRoleAdd.CheckedItems[j].ToString()))
                    {
                        MessageBox.Show(this, "互斥角色 -" + this.CheckedListBox_AssignRoleAdd.CheckedItems[i].ToString() + "-和-"
                            + this.CheckedListBox_AssignRoleAdd.CheckedItems[j].ToString() + "-不能同时授予同一用户,请更改互斥角色或者重新选择其他角色", "用户不能同时拥有互斥角色", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }   
                }
            }
            for (int i = 0; i < this.CheckedListBox_AssignRoleAdd.CheckedItems.Count; i++)
            {
                Insert_User_Roles(this.CheckedListBox_AssignRoleAdd.CheckedItems[i].ToString(), this.ComboBox_UserName.Text);
            }
            MessageBox.Show(this, "增加指派成功", "成功增加指派", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_Users_Roles(this.ComboBox_UserName.SelectedIndex);

        }
        /// <summary>
        /// 删除角色用户指派
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="userName"></param>
        private void Delete_User_Roles(string roleName,string userName)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色用户指派关系信息表中删除
            cmd.CommandText = "delete from  [用户-角色指派信息表]   where  角色名称=" + "'" + roleName.ToLower() + "'" + "and 用户名称=" + "'" + userName.ToLower() + "'";
            cmd.ExecuteNonQuery();

            oleDB.Close();
        }
        /// <summary>
        /// 增加角色用户指派
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="userName"></param>
        private void Insert_User_Roles(string roleName, string userName)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //增加用户角色指派
            cmd.CommandText = "insert into  [用户-角色指派信息表](角色名称,用户名称) values('" + roleName.ToLower() + "','" + userName.ToLower() + "')";
            cmd.ExecuteNonQuery();

            oleDB.Close();
        }
        //以下是权限表的  选项卡代码

        // 选项卡-用户表的代码
        private void Refresh_PermissionTable()
        {


            this.View_Permission.Rows.Clear();
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [权限信息表]";
            OleDbDataReader dr = cmd.ExecuteReader();

            LinkedList<string> RoleName = new LinkedList<string>();//用于记录在角色信息表中出现的角色名称
            if (dr.HasRows == true)

                while (dr.Read())
                {

                    DataGridViewRow r = new DataGridViewRow();
                    r.CreateCells(this.View_Permission);

                    r.Cells[0].Value = dr["权限名称"];
                    r.Cells[4].Value = dr["创建时间"];

                    LinkedList<string> LinkRoles = Get_Permission_Direct_Roles(dr["权限名称"].ToString());//获取拥有此权限的直接角色
                    LinkedList<string> AuthorLinkRoles = new LinkedList<string>();//保存继承该权限的角色名
                    int i = 0;
                    for (LinkedListNode<string> L = LinkRoles.First; i < LinkRoles.Count; i++)
                    {
                        LinkedList<string> FatherRole = GetFatherRoles(L.Value);
                        while (FatherRole.Count != 0)
                        {
                            if (LinkRoles.Find(FatherRole.First.Value) == null)
                            {
                                AuthorLinkRoles.AddLast(FatherRole.First.Value);
                            }

                            FatherRole.RemoveFirst();
                        }
                    }
                    LinkedList<string> Users = new LinkedList<string>();//保存拥有此权限的所有用户
                    while (LinkRoles.Count != 0)
                    {
                        r.Cells[1].Value += LinkRoles.First.Value + " ";
                        LinkedList<string> U = Get_Roles_Direct_User(LinkRoles.First.Value);
                        while (U.Count != 0)
                        {
                            if (Users.Find(U.First.Value) == null)
                            {
                                Users.AddLast(U.First.Value);
                            }
                            U.RemoveFirst();
                        }
                        LinkRoles.RemoveFirst();
                    }
                    while (AuthorLinkRoles.Count != 0)
                    {
                        r.Cells[2].Value += AuthorLinkRoles.First.Value + " ";
                        LinkedList<string> U = Get_Roles_Direct_User(AuthorLinkRoles.First.Value);
                        while (U.Count != 0)
                        {
                            if (Users.Find(U.First.Value) == null)
                            {
                                Users.AddLast(U.First.Value);
                            }
                            U.RemoveFirst();
                        }

                        AuthorLinkRoles.RemoveFirst();
                    }
                    while (Users.Count != 0)
                    {
                        r.Cells[3].Value += Users.First.Value + " ";
                        Users.RemoveFirst();
                    }
                    //while (Permission.Count != 0)
                    //{
                    //    r.Cells[3].Value += Permission.First.Value + " ";
                    //    Permission.RemoveFirst();
                    //}
                    this.View_Permission.Rows.Add(r);
                }

            dr.Close();
            oleDB.Close();


        }

        //以下是权限-角色 的选项卡代码

        void Refresh_Permission_Roles(int index)
        {
            this.Combobox_PermissionName.Items.Clear();
          //this.ComboBox_UserName.Items.Clear();

            //首先绑定数据源到DataGridView_ExcursionRole中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            //将所有的用户名称添加到ComboBox_ExcursionRole中
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [权限信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {

                while (dr.Read())
                {
                   
                        this.Combobox_PermissionName.Items.Add(dr["权限名称"]);

                     
                }
            }
            if (this.Combobox_PermissionName.Items.Count != 0)
            {
                this.Combobox_PermissionName.SelectedIndex = index;
            }

            dr.Close();
            oleDB.Close();


        }
        //选择不同的权限进行查询
        private void Combobox_PermissionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CheckedListBox_Permission_Roles.Items.Clear();
            this.CheckedListBox_Permission_RolesAdd.Items.Clear();
            this.ListBox_AuthorizePermission_Roles.Items.Clear();
            this.ListBox_Permission_Users.Items.Clear();

            this.Label_Perrmission_Time.Text = "创建时间：" + GetPermissionCreatTime(this.Combobox_PermissionName.Text);
            this.Label_Perrmission_Time.Visible = true;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [角色信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)

                while (dr.Read())
                {
                    this.CheckedListBox_Permission_RolesAdd.Items.Add(dr["角色名称"]);
                }
            dr.Close();
            oleDB.Close();

            LinkedList<string> Direct_Roles = Get_Permission_Direct_Roles(Combobox_PermissionName.Text);//获取权限的直接指派角色
            LinkedList<string> Roles = new LinkedList<string>();//用于所有拥有该权限的角色
            ;

            int i = 0;
            for (LinkedListNode<string> L = Direct_Roles.First; i < Direct_Roles.Count; i++, L = L.Next)
            {
                if (Roles.Find(L.Value) == null)
                {
                    Roles.AddLast(L.Value);
                }
                LinkedList<string> FatherRole = GetFatherRoles(L.Value);//获取指定角色的所有上级角色，添加到拥有权限的角色列表中
                while (FatherRole.Count != 0)
                {
                    if (ListBox_AuthorizePermission_Roles.Items.IndexOf(FatherRole.First.Value) == -1)//角色列表中没有该角色时，再添加
                    {
                        ListBox_AuthorizePermission_Roles.Items.Add(FatherRole.First.Value);
                    }
                    if (Roles.Find(FatherRole.First.Value) == null)
                    {
                        Roles.AddLast(FatherRole.First.Value);
                    }
                    FatherRole.RemoveFirst();
                }
            }

            while (Direct_Roles.Count != 0)
            {

                this.CheckedListBox_Permission_RolesAdd.Items.Remove(Direct_Roles.First.Value);
                this.ListBox_AuthorizePermission_Roles.Items.Remove(Direct_Roles.First.Value);
                this.CheckedListBox_Permission_Roles.Items.Add(Direct_Roles.First.Value);
                Direct_Roles.RemoveFirst();
            }
            while (Roles.Count != 0)
            {
                LinkedList<string> Users = Get_Roles_Direct_User(Roles.First.Value);

                while (Users.Count != 0)
                {
                    if (this.ListBox_Permission_Users.Items.IndexOf(Users.First.Value) == -1)
                    {
                        this.ListBox_Permission_Users.Items.Add(Users.First.Value);
                    }
                    Users.RemoveFirst();

                }
                Roles.RemoveFirst();
            }
        }
        //获取角色对应的用户，形成链表返回
        private static LinkedList<string> Get_Roles_Direct_User(string roleName)
        {
            LinkedList<string> LinkUsers = new LinkedList<string>();


            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [用户-角色指派信息表] where 角色名称=" + "'" + roleName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LinkUsers.AddLast(dr["用户名称"].ToString());

                }
            }

            dr.Close();
            oleDB.Close();
            return LinkUsers;
        }
        ///获取权限的创建时间
        private string GetPermissionCreatTime(string PermissionName)
        {
            string time = string.Empty;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [权限信息表] where 权限名称=" + "'" + PermissionName.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    time = dr["创建时间"].ToString();
                }
            }

            dr.Close();
            oleDB.Close();

            return time;
        }
        //获取权限指派的直接角色，形成链表返回
        private static LinkedList<string> Get_Permission_Direct_Roles(string PermissionName)
        {
            LinkedList<string> LinkRoles = new LinkedList<string>();


            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色-权限指派信息表] where 权限名称=" + "'" + PermissionName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LinkRoles.AddLast(dr["角色名称"].ToString());

                }
            }

            dr.Close();
            oleDB.Close();
            return LinkRoles;
        }
        //删除权限到角色的指派
        private void Btn_Permission_Delete_Click(object sender, EventArgs e)
        {
            if (this.CheckedListBox_Permission_Roles.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "未选中任何一项，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            for (int i = 0; i < this.CheckedListBox_Permission_Roles.CheckedItems.Count; i++)
            {
                Delete_Permission_Roles(this.Combobox_PermissionName.Text, this.CheckedListBox_Permission_Roles.CheckedItems[i].ToString());
               // Delete_User_Roles(this.CheckedListBox_Permission_Roles.CheckedItems[i].ToString(), this.Combobox_PermissionName.Text);
            }

            MessageBox.Show(this, "撤销指派成功", "成功撤销指派", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_Permission_Roles(Combobox_PermissionName.SelectedIndex);
        }
        /// <summary>
        /// 删除权限-角色指派
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="userName"></param>
        private void Delete_Permission_Roles(string permissionName,string roleName)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //从角色用户指派关系信息表中删除
            cmd.CommandText = "delete from  [角色-权限指派信息表]   where  角色名称=" + "'" + roleName.ToLower() + "'" + "and 权限名称=" + "'" + permissionName.ToLower() + "'";
            cmd.ExecuteNonQuery();

            oleDB.Close();
        }
        //增加权限到角色的指派
        private void Btn_Permission_Add_Click(object sender, EventArgs e)
        {
            if (CheckedListBox_Permission_RolesAdd.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "未选中任何一项，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < this.CheckedListBox_Permission_RolesAdd.CheckedItems.Count; i++)
            {
                Insert_Permission_Roles(this.Combobox_PermissionName.Text, this.CheckedListBox_Permission_RolesAdd.CheckedItems[i].ToString());

            }
            MessageBox.Show(this, "增加指派成功", "成功增加指派", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_Permission_Roles(Combobox_PermissionName.SelectedIndex);
        }
        /// <summary>
        /// 增加权限角色指派
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="userName"></param>
        private void Insert_Permission_Roles(string permissionName, string roleName)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //增加用户角色指派
            cmd.CommandText = "insert into  [角色-权限指派信息表](角色名称,权限名称) values('" + roleName.ToLower() + "','" + permissionName.ToLower() + "')";
            cmd.ExecuteNonQuery();

            oleDB.Close();
        }
        //删除权限
        private void Btn_PermissionDelete_Click(object sender, EventArgs e)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "delete  from  [权限信息表] where 权限名称=" + "'" +this.Combobox_PermissionName.Text.ToLower() + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete  from  [角色-权限指派信息表] where 权限名称=" + "'" + this.Combobox_PermissionName.Text.ToLower() + "'";
            cmd.ExecuteNonQuery();
            oleDB.Close();
            MessageBox.Show(this, "删除权限成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_Permission_Roles(0);
            
        }

        //以下是选项卡，更新  增加权限的代码
        void Refresh_Permission_Add( )
        {
       
            this.CheckedListBox_Permission_Add.Items.Clear();
            this.TextBox_PermissionName.Text = "";

            //首先绑定数据源到DataGridView_ExcursionRole中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            //将所有的用户名称添加到ComboBox_ExcursionRole中
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [角色信息表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {

                while (dr.Read())
                {


                    this.CheckedListBox_Permission_Add.Items.Add(dr["角色名称"]);


                }
            }
            

            dr.Close();
            oleDB.Close();
        }

        private void Btn_Perrmission_NewAdd_Click(object sender, EventArgs e)
        {
            if (TextBox_PermissionName.Text.Length == 0)//角色名称已经被占用
            {
                MessageBox.Show(this, "权限名称不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (IsPerrmissionNameUsed(TextBox_PermissionName.Text) == true)//角色名称已经被占用
            {
                MessageBox.Show(this, "权限名称已经被占用，请重新输入", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (this.CheckedListBox_Permission_Add.CheckedItems.Count == 0)//角色表暂无其他角色，不用添加上级角色或者下级角色
            {
                if (MessageBox.Show(this, "是否不指派到任何角色", "提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    InsertPermission(TextBox_PermissionName.Text.ToLower());
                    MessageBox.Show(this, "增加权限成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;


                }
                else
                {
                    return;
                }
            }
            else
            {
                //MessageBox.Show(this.CheckedListBox_Permission_Add.CheckedItems.Count.ToString());
                InsertPermission(TextBox_PermissionName.Text.ToLower());
                for (int i = 0; i < this.CheckedListBox_Permission_Add.CheckedItems.Count; i++)
                {
                    Insert_Permission_Roles(TextBox_PermissionName.Text.ToLower(),this.CheckedListBox_Permission_Add.CheckedItems[i].ToString().ToLower());
                }
                MessageBox.Show(this, "增加权限成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        //PerrmissionName是否已经被占用
        public static bool IsPerrmissionNameUsed(string PerrmissionName)
        {
            bool isUsed = false;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [权限信息表] where 权限名称=" + "'" + PerrmissionName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = conn.ExecuteReader();
            isUsed = dr.HasRows;
            dr.Close();
            oleDB.Close();
            return isUsed;

        }
        /// <summary>
        /// 插入新权限到数据库的角色信息表中
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="ChildRoleName"></param>
        private void InsertPermission(string PermissionName)
        {
            //将注册信息插入到注册表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //插入到角色信息中
            cmd.CommandText = "insert into  [权限信息表](权限名称,创建时间) values('" + PermissionName.ToLower() + "','" + DateTime.Now.ToString() + "')";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }
 
        //当选项卡选中项改变时，重新刷新该项中的信息
        private void AbilityTable_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0:
                    {

                        Refresh_RoleTable();
                        break;
                    }

                case 1:
                    {
                        Refresh_RoleAdd();


                        break;
                    }


                case 2:
                    {

                        Refresh_RolesInquire();
                        break;
                    }
                case 3:
                    {
                        Refresh_RoleMangement(0);
                        break;
                    }
                case 4:
                    {


                        Refresh_ExcursionRole(0);

                        break;
                    }
                case 5:
                    {
                         Refresh_UserTable();
                     
                        break;
                    }

                case 6:
                    {

                          Refresh_Users_Roles(0);
                        break;
                    }
                case 7:
                    {


                        Refresh_PermissionTable();
                        break;
                    }
                case 8:
                    {

                       
                        Refresh_Permission_Roles(0);
                        break;
                    }
                      case 9:
                    {

                        Refresh_Permission_Add();
                        break;
                    }

                default:
                    break;


            }
        }

        private void TextBox_PermissionName_TextChanged(object sender, EventArgs e)
        {
            if (IsPerrmissionNameUsed(TextBox_PermissionName.Text) == true)//角色名称已经被占用
            {
                this.Label_Permission_Warning.Visible = true;
               

            }
            else
            {
                this.Label_Permission_Warning.Visible = false;
               

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
            this.Close();
        }

        private void 角色表RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 0;
        }

        private void 权限表PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 7;
        }

        private void 用户表UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 5;
        }

        private void 互斥角色表EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 4;
        }

        private void 角色RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 1;
        }

        private void 权限PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 9;
        }

        private void 角色RToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 3;
        }

        private void 权限PToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 9;
        }

        private void 查看VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 4;
        }

        private void 增加互斥角色AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 4;
        }

        private void 删除互斥角色DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 4;
        }

        private void 查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 6;
        }

        private void 增加AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 6;
        }

        private void 删除DToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 6;
        }

        private void 更改CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 6;
        }

        private void 查看VToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 8;
        }

        private void 增加AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 9;
        }

        private void 删除DToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 9;
        }

        private void 更改CToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Table.SelectedIndex = 9;
        }

        private void 角色图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RH_View r = new RH_View();
           
            r.ShowDialog();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void tabPage9_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }


      

       
      

       

       

   

       
 
    

        

     
    }
    class StoreRoles
    {
        public string FatherName = string.Empty;
        public string ChildName = string.Empty;
     
        public StoreRoles(string FatherName, string ChildName)
        {
            this.ChildName = ChildName;
            this.FatherName = FatherName;
            
        }
       
    }
}
