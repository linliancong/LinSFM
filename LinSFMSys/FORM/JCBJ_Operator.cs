﻿using System;
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
    public partial class JCBJ_Operator : Form
    {
        private string ID = "0";
        public JCBJ_Operator()
        {
            InitializeComponent();
            
        }
        public JCBJ_Operator(string ID, string SName, string Price, string BasePrice)
        {
            InitializeComponent();
            this.ID = ID;
            textBox1.Text = SName;
            
            textBox4.Text = Price;
            textBox5.Text = BasePrice;
            this.Text = "修改产品基础报价";
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
                string strSql = string.Format("exec UP_TSale2 '{0}','{1}','{2}','{3}'",
                    ID, textBox1.Text,  textBox4.Text, textBox5.Text);
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
                MessageBox.Show("修改失败，请稍后重试！", "提示");
            }
            else {
                MessageBox.Show("修改成功！", "提示");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }   
            
        }
    }
}
