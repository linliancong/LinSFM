
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinSFMSys
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "lincong";
            textBox2.Text = "1234";
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text.ToString();
            string pwd = textBox2.Text.ToString();
            if (Logins(user, pwd) ==1)
            {
                Gloab.username = user;
                //MessageBox.Show("登陆成功","提示");             
                this.DialogResult = DialogResult.OK;
                this.Dispose();
                this.Close();

            }
            else {
                 MessageBox.Show("用户名或密码错误，请重新登陆！","提示");
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

            this.Close();//只是关闭当前窗口，若不是主窗体的话，是无法退出程序的，另外若有托管线程（非主线程），也无法干净地退出；
            //Application.Exit();//强制所有消息中止，退出所有的窗体，但是若有托管线程（非主线程），也无法干净地退出；
            //Application.ExitThread();//强制中止调用线程上的所有消息，同样面临其它线程无法正确退出的问题； 
           //System.Environment.Exit(0);//这是最彻底的退出方式，不管什么线程都被强制退出，把程序结束的很干净。
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageBox.Show("真的要退出程序么？", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                //e.Cancel = true;
           // }
        }

        #region "查询联系人信息"

        public List<string> GetLinkMan(String FranchiserID)
        {
            DataSet ds = null;
            List<string> list = new List<string>();
            try
            {
                string strSql = string.Format("exec APP_GetLinkManInfo '{0}'", FranchiserID);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //LinkMan a = new LinkMan();
                        //a.setLinkManName(dt.Rows[i]["LinkManName"].ToString());
                        //a.setLinkManMobile(dt.Rows[i]["LinkManMobile"].ToString());
                        //a.setEmail(dt.Rows[i]["Email"].ToString());

                        //list.Add(a);
                    }
                }

                //return MySqlDbFunc.ExecSql(strSql);
                return list;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region "登陆验证"

        public int Logins(String Username,String PWD)
        {
            DataSet ds = null;
            int tag = 0;
            try
            {
                string strSql = string.Format("select 1 tag from TUser where UserName= '{0}' and PWD='{1}'", Username,PWD);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //tag =Convert.ToInt32(dt.Rows[0]["tag"]);
                        tag = (int)dt.Rows[0]["tag"];        
                    }
                }                
                return tag;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

    }
}
