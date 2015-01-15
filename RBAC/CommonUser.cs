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
    public partial class CommonUser : Form
    {
        public string UserName = "";
        public CommonUser()
        {

            InitializeComponent();
        }

        private void CommonUser_Load(object sender, EventArgs e)
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
        public string GetUserName
        {
            set
            {
                this.UserName = value;
            }
            get
            {

                return UserName;
            }

        }



        //以下是角色表选项卡的代码
        /// <summary>
        /// 更新角色表中的信息
        /// </summary>
        private void Refresh_RoleTable()
        {


            this.View_RolesData.Rows.Clear();

            LinkedList<string> DirectRole = Security_Officer.Get_User_Direct_Roles(GetUserName);//获取用户拥有 的所有角色
            while (DirectRole.Count != 0)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(this.View_RolesData);
                r.Cells[0].Value = DirectRole.First.Value;

                LinkedList<string> DirectFatherRoles = Security_Officer.GetDirectFatherRoles(DirectRole.First.Value);
                LinkedList<string> FatherRoles = Security_Officer.GetFatherRoles(DirectRole.First.Value);

                LinkedList<string> DirectChildRoles = Security_Officer.GetDirectChildRoles(DirectRole.First.Value);
                LinkedList<string> ChildRoles = Security_Officer.GetChildRoles(DirectRole.First.Value);
           
                while (FatherRoles.Count != 0)
                {
                    if ( DirectFatherRoles.Find(FatherRoles.First.Value)== null)
                    {
                        r.Cells[3].Value += FatherRoles.First.Value + " ";
                     
                    }
                    FatherRoles.RemoveFirst();
                }

                while (ChildRoles.Count != 0)
                {
                    if (  DirectChildRoles.Find(ChildRoles.First.Value) == null)
                    {
                        r.Cells[4].Value += ChildRoles.First.Value + " ";
                       
                    }
                    ChildRoles.RemoveFirst();
                }
                while (DirectFatherRoles.Count != 0)
                {
                    r.Cells[1].Value += DirectFatherRoles.First.Value + " ";
                    DirectFatherRoles.RemoveFirst();

                }
                while (DirectChildRoles.Count != 0)
                {
                    r.Cells[2].Value += DirectChildRoles.First.Value + " ";
                    DirectChildRoles.RemoveFirst();
                }

                DirectRole.RemoveFirst();

                this.View_RolesData.Rows.Add(r);
            }

        }
        private void Refresh_Permission()
        {
            this.ListBox_Permissions.Items.Clear();
            LinkedList<string> Direct_Roles = Security_Officer.Get_User_Direct_Roles(GetUserName);//获取用户的指派角色
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
                   
                    if (Roles.Find(ChildRole.First.Value) == null)
                    {
                        Roles.AddLast(ChildRole.First.Value);
                    }
                    ChildRole.RemoveFirst();
                }
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
        private void Table_Selected(object sender, TabControlEventArgs e)
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
                        Refresh_Permission();
                        


                        break;
                    }




                default:
                    break;


            }       //当选项卡选中项改变时，重新刷新该项中的信息
        }
    }
}