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
    public partial class YGXX_Operator : Form
    {
        private string ID = "0";
        private string Resume = "";
        private string tag = "";
        public YGXX_Operator()
        {
            InitializeComponent();
            this.Text = "新增员工";
            tag = "新增";
                    
            ////控制日期或时间的显示格式
            //this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            ////使用自定义格式
            //this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            ////时间控件的启用
            //this.dateTimePicker1.ShowUpDown = true;
           
        }
        public YGXX_Operator(string ID, string Name, int Sex, string Phone, string Address, string FamilyPhone, string FamilyAddress, string Birthday, string EntryTime, string Resume, int State)
        {
            InitializeComponent();
            tag = "修改";
            this.ID = ID;
            textBox1.Text = Name;
            comboBox1.SelectedIndex = Sex;
            textBox3.Text = Phone;
            textBox4.Text = Address;
            textBox5.Text = FamilyPhone;
            textBox7.Text = FamilyAddress;
            dateTimePicker1.Text = Birthday;
            dateTimePicker2.Text = EntryTime;
            this.Resume = Resume;
            comboBox2.SelectedIndex = State;
            this.Text = "修改员工";
            
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
            string a = comboBox1.SelectedIndex.ToString();
            string b = comboBox2.SelectedIndex.ToString();
            try
            {
                string strSql = string.Format("exec UP_TStaff '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'",
                    ID, 0, textBox1.Text, comboBox1.SelectedIndex, textBox3.Text, textBox4.Text, textBox5.Text, textBox7.Text, dateTimePicker1.Text, dateTimePicker2.Text, Resume, comboBox2.SelectedIndex);
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
                MessageBox.Show(tag+"失败，请稍后重试！", "提示");
            }
            else {
                MessageBox.Show(tag + "成功！", "提示");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }   
            
        }
    }
}
