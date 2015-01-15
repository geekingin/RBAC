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
    public partial class RH_View : Form
    {
        LinkedList<Roles> Roles = new LinkedList<Roles>();

        LinkedList<int > RoleX = new LinkedList<int> ();
        LinkedList<int> RoleY= new LinkedList<int>();
       LinkedList<  string> RoleFatherName = new LinkedList<string>();
       LinkedList<string>  RoleChildName= new LinkedList<string>();

        Point startPoint = new Point(250, 120);
        int  LabelLength = 50;//用于存储标签的长度和宽度
        int  LabelWidth =20;
        
        
        float LineLength = 100;
       // int NumberOfHaveDraw = 0;//已经画了的角色

        public RH_View()
        {
            InitializeComponent();     
            
           
        }
        public void Refresh_Rh_View()
        {
            this.listBox_RoleName.Items.Clear();
          
            //找到所有不在角色关系表中的角色名称添加到列表中
            LinkedList<string> single_Roles = Get_Single_Roles();
            if (single_Roles.Count == 0)
            {
                this.listBox_RoleName.Visible = false;
                this.label5.Visible = false;
                this.panel7.Visible = false;
            }
            while (single_Roles.Count != 0)
            {
                this.listBox_RoleName.Items.Add(single_Roles.First.Value);
           
            
                single_Roles.RemoveFirst();
            }
         

        }

        /// <summary>
        /// 开始作图按钮回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            start_drawing();
        }

        /// <summary>
        /// 绘图
        /// </summary>
        private void start_drawing()
        {
            // Refresh_Rh_View();//刷新列表
            Get_All_Realation();

            //找到所有处于最上级的角色
            LinkedList<string> S = Get_Top_Roles();
            int n = S.Count + 1;
            startPoint.X = (this.Size.Width) / n;
            while (S.Count != 0)
            {
                DrawLabel(startPoint, S.First.Value);//画第一个结点

                //NumberOfHaveDraw++;
                RoleX.AddLast(startPoint.X);//记录当前已经画了的角色的坐标
                RoleY.AddLast(startPoint.Y);

                Draw_Child_Role(S.First.Value, startPoint.X, startPoint.Y);

                startPoint.X += (this.Size.Width) / n;

                S.RemoveFirst();
            }
        }

        /// <summary>
        /// 画儿子结点
        /// </summary>
        /// <param name="FatherRoleName"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void Draw_Child_Role(string FatherRoleName, int X, int Y)
        {
            LinkedList<string> childName = Get_Direct_Child_Roles(FatherRoleName);

            if (childName.Count == 0)
            {
                return;
            }

            int n = childName.Count;
            double degress =180 / (n+1);//求出角度
            
            double NowDegress = 0; 
            int [,]XY = new int [n,2];//用于保存子结点的位置

            int i = 0;
            for (LinkedListNode<string> c = childName.First; i < n; i++, c = c.Next)
            {
                
                NowDegress += degress;
                XY[i, 0] =X- (int)(Math.Cos(NowDegress * Math.PI / 180)*LineLength);
                XY[i, 1] = Y + (int)(Math.Sin(NowDegress * Math.PI / 180) * LineLength) + LabelWidth;
                
                 

                DrawLine(X, Y, LineLength, NowDegress,FatherRoleName,c.Value,n);
                RoleFatherName.AddLast(FatherRoleName);
                RoleChildName.AddLast(c.Value.ToString());
            }

          

              i = 0;
             while (childName.Count != 0)
             {

                 Draw_Child_Role(childName.First.Value, XY[i, 0], XY[i, 1]);
                 childName.RemoveFirst();
                 i++;
            }
        }
        /// <summary>
        /// 根据角度和线的长度划线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="degress"></param>
        /// <param name="labelText"></param>
        private void DrawLine(int X,int Y, double length,double degress, string fatherName, string labelText,int n )//n用来表明当前有几个同级角色

        {
            Point start = new Point(X, Y);
            Point end=new Point();
            int i = 0;
            LinkedListNode<string> f = RoleFatherName.First;
            LinkedListNode<string> c = RoleChildName.First;


            for (; i < RoleFatherName.Count; i++, f = f.Next, c = c.Next)
            {
                if (string.Equals(c.Value,labelText) && string.Equals(fatherName, f.Value))
                {
                    return;
                }
            }
            i = 0;
            for (LinkedListNode<Roles> r = Roles.First; i < Roles.Count; i++,r=r.Next)
            {
                if (string.Equals( r.Value.RoleName,labelText) && r.Value.IsDraw )
                {
                    if (X > r.Value.X)
                    {
                        end.X = r.Value.X + 5;
                    }
                    else
                    {
                        end.X = r.Value.X - 5;
                    }
                   end.Y = r.Value.Y;
                   Graphics g1 = this.CreateGraphics();
                   g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                   Pen p1 = new Pen(Color.Black, 2);
                   p1.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 4, true);
                   g1.DrawLine(p1, start, end);
                   return;
                }
            }
            
            double x = Math.Cos(degress*Math.PI   / 180) * length;
            double y = Math.Sin(degress*Math.PI   / 180) * length;
              end = new Point(start.X-(int)x , start.Y +(int)y);
            if (TestDrawLabel(end) == true)
            {
                double d = degress;
                while (d < 175 && TestDrawLabel(end))
                {
                    x = Math.Cos(d * Math.PI / 180) * length;
                    y = Math.Sin(d * Math.PI / 180) * length;
                    end = new Point(start.X - (int)x, start.Y + (int)y);
                    d += 10;
                }
                
                int j = 0;
                while (TestDrawLabel(end))
                {

                    x = Math.Cos(degress * Math.PI / 180) * length;
                    y = Math.Sin(degress * Math.PI / 180) * length+j*LabelWidth;
                    j++;
                    end = new Point(start.X - (int)x, start.Y + (int)y);
                }
            }
            //Point end = ()
            
            Graphics g = this.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen p = new Pen(Color.Black, 2);
            p.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 4, true);
            g.DrawLine(p, start, end); 
            DrawLabel(end,labelText);
            
        }
        //检查当前已经画的和即将画的是否会产生覆盖
        private bool TestDrawLabel(Point point)
        {
            bool b = false;
          
            LinkedListNode<int > x= this.RoleX.First;
            LinkedListNode<int > y= this.RoleY.First;
            for(  int i =0 ;i<this.RoleX.Count;i++)
            {
                if (Math.Abs(x.Value - point.X) <= LabelLength && Math.Abs(y.Value - point.Y) <= LabelWidth)
                {
                    b = true;
                }
                x = x.Next;
                y = y.Next;
            }

            return b;
        }
   
        private void DrawLabel( Point start, string labelText)
        {
           
            Label l = new Label();
            l.AutoSize = true;
            l.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            l.Location = new System.Drawing.Point(start.X - 10, start.Y);
            l.Size = new System.Drawing.Size(50, 20);
            l.BackColor = TransparencyKey;
            l.Text = labelText;
            this.Controls.Add(l);

            RoleX.AddLast(start.X);
            RoleY.AddLast(start.Y);

            int i = 0;
            for (LinkedListNode<Roles> r = Roles.First; i < Roles.Count; i++,r=r.Next)
            {
                if (string.Equals(r.Value.RoleName,labelText) && r.Value.IsDraw==false)
                {
                    r.Value.IsDraw = true;

                    r.Value.X = start.X;
                    r.Value.Y = start.Y;
                    break;
                }
            }
        }
        private void Get_All_Realation()
        {
            LinkedList<string> l = Get_All_Roles_Relation();
            while (l.Count != 0)
            {
                Roles r = new RBAC.Roles(l.First.Value.ToString());
                Roles.AddLast(r);
                l.RemoveFirst();
            }
        }
     

         
        /// 找到最上级的角色
        private LinkedList<string> Get_Top_Roles()
        {
            LinkedList<string> Link = new LinkedList<string>();
            LinkedList<string> Roles = Get_All_Roles_Relation();//获取所有在角色关系信息表中的角色
           while(Roles.Count != 0)
            {
                if (Is_Has_Father_Roles (Roles.First.Value ) == false)//如果一个角色无上级角色，表明该角色为最上级的角色
                {
                    if (Link.Find(Roles.First.Value) == null)
                    {

                        Link.AddLast(Roles.First.Value);
                    }
                }
             Roles.RemoveFirst();
            }
            return Link;

        }
           
        /// <summary>
        /// 获取某个角色的所有直接上级级角色,形成链表返回
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static LinkedList<string> Get_Direct_Father_Roles(string roleName)
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
        public static  bool Is_Has_Father_Roles(string roleName)
        {

            bool b = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色关系信息表] where 子角色名称=" + "'" + roleName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            b = dr.HasRows;
            

            dr.Close();
            oleDB.Close();
            return b;
           
        }
        /// <summary>
        /// 获取某个角色的所有直接下级角色，形成链表返回
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static LinkedList<string> Get_Direct_Child_Roles(string roleName)
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
 
        /// <summary>
        ///  获取角色信息表中无上下级关系的角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public  LinkedList<string> Get_Single_Roles()
        {
            LinkedList<string> LinkRole = new LinkedList<string>();

            LinkedList<string> All_Roles = Get_All_Roles_Info();
            LinkedList<string> All_Roles_Rea = Get_All_Roles_Relation();
            while (All_Roles.Count != 0)
            {
                if (All_Roles_Rea.Find(All_Roles.First.Value) == null)
                {
                    LinkRole.AddLast(All_Roles.First.Value);
                }
                All_Roles.RemoveFirst();
            }
            return LinkRole;
        }

    
            /// <summary>
        ///  获取角色信息表中所有的角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public   LinkedList<string> Get_All_Roles_Info()
        {
            LinkedList<string> LinkRole = new LinkedList<string>();


            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色信息表] ";//查找账号是否已经被注册
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
        /// <summary>
        ///  获取角色关系表中所有的角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public  LinkedList<string> Get_All_Roles_Relation()
        {
            LinkedList<string> LinkRole = new LinkedList<string>();

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [角色关系信息表] ";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (LinkRole.Find(dr["父角色名称"].ToString()) == null)
                    {
                        LinkRole.AddLast(dr["父角色名称"].ToString());
                        
                    }
                //    RoleFatherName.AddLast(dr["父角色名称"].ToString());
                    if (LinkRole.Find(dr["子角色名称"].ToString()) == null)
                    {
                        LinkRole.AddLast(dr["子角色名称"].ToString());
                    }
                   // RoleChildName.AddLast(dr["子角色名称"].ToString());


                }
            }

            dr.Close();
            oleDB.Close();
            return LinkRole;
        }

        private void RH_View_Load(object sender, EventArgs e)
        {
            Refresh_Rh_View();
            start_drawing();
        }
    } 
    class Roles
    {

        public string RoleName = string.Empty;
       // public string RoleName = string.Empty;
        public bool IsDraw = false;
       // public int FatherX = 0;
       // public int FatherY = 0;
        public int X = 0;
        public int Y = 0;

        public  Roles(string Name)
        {
            this.RoleName =Name;
           
        
        }


    }
}
