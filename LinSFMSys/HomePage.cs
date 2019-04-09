using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinSFMSys
{
    public partial class HomePage : Form
    {
        Point mp = new Point();
        public HomePage()
        {
            InitializeComponent();
            //SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.Width = 900;
            this.Height = 700;
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false; //禁用"最大化"按钮
            //menuStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;  
            SetDouble(this);
            SetDouble(tabControl1);

                     
        }
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}

        //启用双缓存
        public static void SetDouble(Control cc)
        {

            cc.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic).SetValue(cc, true, null);
        }

         protected override void WndProc(ref Message m)
        {
 
            if (m.Msg == 0x0014) // 禁掉清除背景消息
 
                return;
 
            base.WndProc(ref m);
 
        }

        private void HomePage_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        //动态加载菜单
        private void HomePage_Load(object sender, EventArgs e)
        {

            //this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT * FROM dbo.TMenu f,(SELECT b.RMenuName FROM dbo.TUser a,dbo.TRole_Menu b WHERE a.UserName='{0}' AND a.RoleID=b.RID) AS t WHERE t.RMenuName LIKE '%'+f.Name+'%' AND f.FID=0",Gloab.username);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ToolStripMenuItem tool = new ToolStripMenuItem();
                        tool.Tag = dt.Rows[i]["EName"].ToString();
                        tool.Name = dt.Rows[i]["EName"].ToString();
                        tool.Text = dt.Rows[i]["Name"].ToString();
                        
                        
                        tool.Font = new Font("Microsoft YaHei UI", 10f, FontStyle.Bold );              
                        
                        //二级菜单
                        string strSql2 = string.Format("SELECT * FROM dbo.TMenu WHERE FID='{0}'", dt.Rows[i]["ID"].ToString());
                        DataSet ds2 = null;
                        ds2 = DbFunc.GetTableNoName(strSql2);
                        if (ds2 != null && ds2.Tables.Count != 0 && ds2.Tables[0].Rows.Count != 0)
                        {
                            DataTable dt2 = ds2.Tables[0];

                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                ToolStripMenuItem tool2 = new ToolStripMenuItem();
                                tool2.Tag = dt2.Rows[j]["EName"].ToString();
                                tool2.Name = dt2.Rows[j]["EName"].ToString();
                                tool2.Text = dt2.Rows[j]["Name"].ToString();
                                tool2.Font = new Font("Microsoft YaHei UI", 10f, FontStyle.Bold);
                                tool2.Click += new EventHandler(subMenu_Click);
                                tool.DropDownItems.Add(tool2);
                            }
                        }
                        tool.Click += new EventHandler((subMenu_Click));
                        this.menuStrip1.Items.Add(tool);
                    }
                } 
               
            }
            catch
            {
                
            }
        }
        //响应右键按钮的操作
        private void toolMenu_Click(object sender, EventArgs e) {
            string acName = ((ToolStripMenuItem)sender).Name.ToString();
            switch (acName)
            {
                case "toolStripMenuItem1":
                    for (int i = 0; i < this.tabControl1.TabPages.Count; i++)
                    {
                        Rectangle r = tabControl1.GetTabRect(i);

                        if (r.Contains(mp))
                        {
                            this.tabControl1.TabPages.RemoveAt(i);
                            break;
                        }
                    }
                    break;
                case "toolStripMenuItem2":
                    for (int i = 0; i < this.tabControl1.TabPages.Count; i++)
                    {
                        Rectangle r = tabControl1.GetTabRect(i);
                        if (r.Contains(mp))
                        {
                            
                            int j = i;
                            for (int n=j+1; n<this.tabControl1.TabPages.Count; )
                                this.tabControl1.TabPages.RemoveAt(n);
                            for (int m = j; m >0; m--)
                                this.tabControl1.TabPages.RemoveAt(0);                          
                            break;                           
                        }
                    }
                    break;
                case "toolStripMenuItem3":
                    tabControl1.TabPages.Clear();
                    break;
            }
        }

        //响应主菜单的操作
        private void subMenu_Click(object sender, EventArgs e)
        {

            try
            {
                //tag属性在这里有用到。
                string acName = ((ToolStripMenuItem)sender).Tag.ToString();
                switch(acName){
                    //销售管理
                    case "xsgl":
                        //tabControl1.TabPages.Add("销售管理");
                        this.Add_TabPage("销售管理", new XSGL());
                        
                        break;
                    //客户订单
                    case "khdd":
                        //tabControl1.TabPages.Add("客户订单");
                        this.Add_TabPage("客户订单", new KHDD());
                       
                        break;
                    //下单管理
                    case "xdgl":
                        //tabControl1.TabPages.Add("下单管理");
                        this.Add_TabPage("下单管理", new XDGL());
                        
                        break;
                    //物流配送
                    case "wlps":
                        //tabControl1.TabPages.Add("物流配送");
                        this.Add_TabPage("物流配送", new WLPS());
                        
                        break;
                    //完工验收
                    case "wgys":
                        //tabControl1.TabPages.Add("完工验收");
                        this.Add_TabPage("完工验收", new WGYS());
                        
                        break;
                    //余款跟踪
                    case "ykgz":
                        //tabControl1.TabPages.Add("余款跟踪");
                        this.Add_TabPage("余款跟踪", new YKGZ());
                        
                        break;
                    //财务管理
                    case "cwgl":
                        //tabControl1.TabPages.Add("财务管理");
                        this.Add_TabPage("财务管理", new CWGL());
                        
                        break;
                    //订单查询
                    case "ddcx":
                        //tabControl1.TabPages.Add("订单查询");
                        this.Add_TabPage("订单查询", new DDCX());
                        
                        break;
                    //员工信息
                    case "ygxx":
                        //tabControl1.TabPages.Add("员工信息");
                        this.Add_TabPage("员工信息", new YGXX());
                       
                        break;
                    //基础报价
                    case "jcbj":
                        //tabControl1.TabPages.Add("基础报价");
                        this.Add_TabPage("基础报价", new JCBJ());
                        
                        break;
                    //系统设置
                    case "xtsz":
                        //tabControl1.TabPages.Add("系统设置");
                        this.Add_TabPage("系统设置", new XTSZ());
                        
                        break;
                    default: 
                        break;
                }               
                
            }
            catch 
            {

            }
        }

        //绘制关闭按钮
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            //e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 3);
            // e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);           
            //e.DrawFocusRectangle();


            //获取TabControl主控件的工作区域

            Rectangle rec = tabControl1.ClientRectangle;



            //获取背景图片，我的背景图片在项目资源文件中。

            //Image backImage = Resources.枫叶;



            //新建一个StringFormat对象，用于对标签文字的布局设置

            StringFormat StrFormat = new StringFormat();

            StrFormat.LineAlignment = StringAlignment.Center;// 设置文字垂直方向居中

            StrFormat.Alignment = StringAlignment.Center;// 设置文字水平方向居中  


            // 标签背景填充颜色，也可以是图片
            //rgb(39, 174, 96)

            SolidBrush bru = new SolidBrush(Color.FromArgb(39, 174, 96));

            SolidBrush bru2 = new SolidBrush(Color.FromArgb(255, 255, 255));

            SolidBrush bruFont = new SolidBrush(Color.FromArgb(0, 0, 0));// 标签字体颜色

            Font font = new System.Drawing.Font("微软雅黑", 10F);//设置标签字体样式



            //绘制主控件的背景

            //e.Graphics.DrawImage(backImage, 0, 0, tabControl1.Width, tabControl1.Height);


            //绘制标签样式



            //获取标签头的工作区域
            Rectangle recChild = tabControl1.GetTabRect(e.Index);

            //绘制标签头背景颜色
            e.Graphics.FillRectangle(bru, recChild);
            //绘制标签头的文字
            e.Graphics.DrawString("x", font, bruFont, e.Bounds.Right - 15, e.Bounds.Top);
            e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, font, bruFont, recChild, StrFormat);

            if (e.Index != 0)
            {
                recChild = tabControl1.GetTabRect(e.Index - 1);
                e.Graphics.FillRectangle(bru2, recChild);
                //绘制标签头的文字
                e.Graphics.DrawString("x", font, bruFont, e.Bounds.Right - 15, e.Bounds.Top);
                e.Graphics.DrawString(tabControl1.TabPages[e.Index - 1].Text, font, bruFont, recChild, StrFormat);
            }
            if (e.Index != tabControl1.TabPages.Count - 1)
            {
                recChild = tabControl1.GetTabRect(tabControl1.TabPages.Count - 1);
                e.Graphics.FillRectangle(bru2, recChild);
                //绘制标签头的文字
                e.Graphics.DrawString("x", font, bruFont, e.Bounds.Right - 15, e.Bounds.Top);
                e.Graphics.DrawString(tabControl1.TabPages[tabControl1.TabPages.Count - 1].Text, font, bruFont, recChild, StrFormat);
            }
            e.Graphics.Dispose();
           

        }
        //TabControl鼠标点击关闭按钮操作
        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < this.tabControl1.TabPages.Count; i++)
            {
                Rectangle r = tabControl1.GetTabRect(i);
                //Getting the position of the "x" mark.
                Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 3, 11, 9);
                if (closeButton.Contains(e.Location))
                {
                    this.tabControl1.TabPages.RemoveAt(i);
                    
                    
                }
            }
            //点击右键时切换到对应界面
            if (e.Button == MouseButtons.Right) {
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    if (tabControl1.GetTabRect(i).Contains(new Point(e.X, e.Y)))
                    {
                        mp = new Point(e.X, e.Y);
                        tabControl1.SelectedTab = tabControl1.TabPages[i];
                    }
                }
            }
            
        }

        //鼠标点击操作
        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                contextMenuStrip1.Show(MousePosition.X,MousePosition.Y);
            }
        }

        //以下两步为为菜单添加界面
        //1、将标题添加进tabpage中
        public void Add_TabPage(string str, Form myForm) //将标题添加进tabpage中
       {
            if (!this.tabControlCheckHave(this.tabControl1, str))
            {
                this.tabControl1.TabPages.Add(str);
                this.tabControl1.SelectTab((int)(this.tabControl1.TabPages.Count - 1));              
                myForm.FormBorderStyle = FormBorderStyle.None;
                //myForm.Dock = DockStyle.Fill;
                //myForm.WindowState = FormWindowState.Normal;
                //myForm.WindowState = FormWindowState.Maximized; 

                myForm.Height = tabControl1.Height;
                myForm.Width = tabControl1.Width;
                myForm.TopLevel = false;
                myForm.Show();
                myForm.Parent = this.tabControl1.SelectedTab;
            }
        }
        //2、看tabpage中是否已有窗体
        public bool tabControlCheckHave(TabControl tab, string tabName) //看tabpage中是否已有窗体
        {
            for (int i = 0; i < tab.TabCount; i++)
            {
                if (tab.TabPages[i].Text == tabName)
                {
                    tab.SelectedIndex = i;
                    return true;
                }
            }
            return false;
        }

       

    }
}
