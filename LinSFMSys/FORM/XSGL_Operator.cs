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
    public partial class XSGL_Operator : Form
    {
        private string ID = "0";
        private string tag = "";
        public XSGL_Operator()
        {
            InitializeComponent();
            this.Text = "新增产品";
            tag = "新增";
        }
        public XSGL_Operator(string ID, string SName, string Norms, string Texture, string Price, string Num)
        {
            InitializeComponent();
            tag = "修改";
            this.ID = ID;
            textBox1.Text = SName;
            textBox2.Text = Norms;
            textBox3.Text = Texture;
            textBox4.Text = Price;
            textBox5.Text = Num;
            this.Text = "修改产品";
        }

        public int ReturnValue { get; protected set; } //用这个公开属性传值

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            int result = -1;
            try
            {
                string strSql = string.Format("exec UP_TSale '{0}','{1}','{2}','{3}','{4}','{5}'",
                    ID, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    result=(int)dt.Rows[0]["Result"];                    
                }

            }
            catch
            {

            }

            ReturnValue = result;
            if (result == -1)
            {
                MessageBox.Show(tag+"失败，请稍后重试！", "提示");
            }
            else {
                MessageBox.Show(tag+"成功！", "提示");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }   
            
        }
    }
}
