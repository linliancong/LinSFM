using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinSFMSys.FORM
{
    public partial class USER_Operator : Form
    {
        private int ID = 0;
        private int UID=0;
        private int RoleID = 0;
        private string UName = "";
        private string tag = "";
        public USER_Operator()
        {
            InitializeComponent();
        }
        public USER_Operator(int ID,string Name)
        {
            InitializeComponent();
            tag = "注册";
            this.Text = "注册用户";
            textBox1.Text = Name;
            this.ID = ID;
            query();
        }

        public int ReturnValue { get; protected set; } //用这个公开属性传值

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            int st = 0;
         
            if (UName.Equals(""))
            {
                DataSet ds1 = null;
                string strSql1 = string.Format("select 1 from tuser where username='{0}'", textBox2.Text);
                ds1 = DbFunc.GetTableNoName(strSql1);
                if (ds1 != null && ds1.Tables.Count != 0 && ds1.Tables[0].Rows.Count != 0)
                {
                    st = 1;
                    
                }
                else
                {
                    st = 0;
                }
            }else{
                st=0;
            }

            if (st == 1) {
                MessageBox.Show("该用户名已存在，请重新输入！", "提示");
            }
            else
            {
                if (textBox3.Text.Equals(textBox4.Text))
                {
                    DataSet ds = null;
                    int result = -1;
                    try
                    {
                        string strSql = string.Format("exec UP_TUser '{0}','{1}','{2}','{3}','{4}'",
                            UID, ID, textBox2.Text, textBox4.Text, comboBox1.SelectedValue);
                        ds = DbFunc.GetTableNoName(strSql);
                        if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                        {
                            DataTable dt = ds.Tables[0];
                            result = (int)dt.Rows[0]["Result"];
                        }

                    }
                    catch
                    {

                    }
                    ReturnValue = result;
                    if (result == -1)
                    {
                        MessageBox.Show(tag + "失败，请稍后重试！", "提示");
                    }
                    else
                    {
                        MessageBox.Show(tag + "成功！", "提示");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }

                }
                else
                {
                    MessageBox.Show("两次输入的密码不同，请重新输入！", "提示");
                }
            }
                 
        }

        private void query()
        {

            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT ISNULL(b.UserID,0) UserID,b.UserName,ISNULL(b.RoleID,2) RoleID FROM dbo.TStaff a LEFT JOIN dbo.TUser b on a.UID=b.UserID AND a.ID={0}", ID);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    UID=(int)dt.Rows[0]["UserID"];
                    RoleID = (int)dt.Rows[0]["RoleID"];
                    UName = dt.Rows[0]["UserName"].ToString();
                  
                    //ComboBox控件实现  (与ListBox的实现类似)
                    comboBox1.DataSource = query2();
                    comboBox1.DisplayMember = "RoleName";
                    comboBox1.ValueMember = "RoleID";
                    comboBox1.SelectedValue = RoleID;
                    textBox2.Text = UName;
                    
                }
                

            }
            catch
            {
                
            }
        }

        private DataTable query2()
        {
            DataSet ds = null;
            DataTable dt = null;
            try
            {
                string strSql = string.Format("SELECT * FROM dbo.TRole");
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    dt = ds.Tables[0];                                      

                }
                return dt;

            }
            catch
            {
                return null;
            }
        }

    }
}
