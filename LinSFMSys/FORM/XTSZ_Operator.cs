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
    public partial class XTSZ_Operator : Form
    {
        private string ID = "0";
        private string tag = "";
        private string[] RMenuID;
        public XTSZ_Operator()
        {
            InitializeComponent();
            this.Text = "新增角色";
            tag = "新增";
        }
        public XTSZ_Operator(string ID, string RName, string RMenu)
        {
            InitializeComponent();
            this.Text = "修改角色";
            tag = "修改";
            this.ID = ID;
            textBox1.Text = ID;
            textBox2.Text = RName;
            RMenuID=RMenu.Split(',');
            showCheckBox();
            
           
        }

        public int ReturnValue { get; protected set; } //用这个公开属性传值

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            string Rn = "";
            string Ri = "";
            if (cb1.Checked) {
                Rn += cb1.Text+",";
                Ri += "1,";
            } 
            if (cb2.Checked)
            {
                Rn += cb2.Text + ",";
                Ri += "2,";
            }
            if (cb3.Checked)
            {
                Rn += cb3.Text + ",";
                Ri += "3,";
            }
            if (cb4.Checked)
            {
                Rn += cb4.Text + ",";
                Ri += "4,";
            }
            if (cb5.Checked)
            {
                Rn += cb5.Text + ",";
                Ri += "5,";
            }
            if (cb6.Checked)
            {
                Rn += cb6.Text + ",";
                Ri += "6,";
            }
            if (cb7.Checked)
            {
                Rn += cb7.Text + ",";
                Ri += "7,";
            }
            if (cb8.Checked)
            {
                Rn += cb8.Text + ",";
                Ri += "8,";
            }
            if (cb9.Checked)
            {
                Rn += cb9.Text + ",";
                Ri += "9,";
            }
            if (cb10.Checked)
            {
                Rn += cb10.Text + ",";
                Ri += "10,";
            }
            if (cb11.Checked)
            {
                Rn += cb11.Text + ",";
                Ri += "11,";
            }


            DataSet ds = null;
            int result = -1;
            try
            {
                string strSql = string.Format("exec UP_TRoleMenu '{0}','{1}','{2}','{3}'",
                    ID, textBox2.Text,Ri,Rn);
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

        public void showCheckBox() {
            for (int i = 0; i < RMenuID.Length; i++)
            {
                switch (RMenuID[i]) { 
                    case "1":
                        cb1.Checked = true;
                        break;
                    case "2":
                        cb2.Checked = true;
                        break;
                    case "3":
                        cb3.Checked = true;
                        break;
                    case "4":
                        cb4.Checked = true;
                        break;
                    case "5":
                        cb5.Checked = true;
                        break;
                    case "6":
                        cb6.Checked = true;
                        break;
                    case "7":
                        cb7.Checked = true;
                        break;
                    case "8":
                        cb8.Checked = true;
                        break;
                    case "9":
                        cb9.Checked = true;
                        break;
                    case "10":
                        cb10.Checked = true;
                        break;
                    case "11":
                        cb11.Checked = true;
                        break;
                    
                }
            }
        }
    }
}
