using LinSFMSys.FORM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinSFMSys
{
    public partial class KHDD : Form
    {
        private int ID;
        private Boolean isUser=false;

        public KHDD()
        {
            InitializeComponent();
            btn_zhen.Enabled = false;
        }

        private void btn_cha_Click(object sender, EventArgs e)
        {
            int result = 0;
            DataSet ds = null;
            try
            {
                string strSql = string.Format("select * from TCustomer where CName = '{0}'", txt_username.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    ID = (int)dt.Rows[0]["ID"];
                    txt_username.Text = dt.Rows[0]["CName"].ToString();
                    txt_phone.Text = dt.Rows[0]["Phone"].ToString();
                    txt_email.Text = dt.Rows[0]["Email"].ToString();
                    txt_address.Text = dt.Rows[0]["Address"].ToString();
                    result = 1;
                }
                
            }
            catch
            {
               
            }
            if (result == 0)
            {
                ID = 0;
                MessageBox.Show("没有该联系人，请新增后再试！", "提示");
                btn_zhen.Enabled = true;
                isUser = false;
            }
            else {
                query2();
                btn_zhen.Enabled = false;
                isUser = true;
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            ID = 0;
            txt_username.Text = "";
            txt_phone.Text = "";
            txt_email.Text = "";
            txt_address.Text = "";
            
        }

        private void btn_zhen_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            int result = -1;
            try
            {
                string strSql = string.Format("exec UP_TCustomer '{0}','{1}','{2}','{3}','{4}'",
                    ID, txt_username.Text, txt_phone.Text, txt_email.Text, txt_address.Text);
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

            if (result == -1)
            {
                MessageBox.Show("新增失败，请稍后重试！", "提示");
                isUser = false;
            }
            else
            {
                MessageBox.Show("新增成功！", "提示");
                ID = result;
                btn_zhen.Enabled = false;
                isUser = true;
                
            }   
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            
            query();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (isUser)
            {   
                DateTime currentTime=DateTime.Now;
                string str = currentTime.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
                int a = dataGridView1.CurrentRow.Index;
                DataSet ds = null;
                int result = -1;
                try
                {
                    string strSql = string.Format("exec UP_TCusForm_ADD '{0}','{1}','{2}','{3}','{4}','{5}'",
                        ID, dataGridView1.Rows[a].Cells[0].Value.ToString(), 1, dataGridView1.Rows[a].Cells[4].Value.ToString(), str, 0);
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
                if (result == 0)
                {
                    query2();
                }
                else
                {
                    MessageBox.Show("添加产品失败！", "提示");
                }
            }
            else {
                MessageBox.Show("客户信息不能为空！", "提示");
            }
        }

        private void btn_upd_Click(object sender, EventArgs e)
        {
            if (dataGridView2.RowCount == 0)
            {
                MessageBox.Show("没有选中任何产品，请选中后重试！", "提示");
            }
            else
            {
                int a = dataGridView2.CurrentRow.Index;
                KHDD_Operator xo = new KHDD_Operator(dataGridView2.Rows[a].Cells[0].Value.ToString(), dataGridView2.Rows[a].Cells[1].Value.ToString(), dataGridView2.Rows[a].Cells[2].Value.ToString(), dataGridView2.Rows[a].Cells[3].Value.ToString(), dataGridView2.Rows[a].Cells[4].Value.ToString(), dataGridView2.Rows[a].Cells[5].Value.ToString());
                xo.ShowDialog();

                if (xo.DialogResult == DialogResult.OK && xo.ReturnValue == 0)
                {
                    query2();
                }
            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (dataGridView2.RowCount == 0)
            {
                MessageBox.Show("没有选中任何订单，请选中后重试！", "提示");
            }
            else
            {
                int a = dataGridView2.CurrentRow.Index;
                int id = (int)dataGridView2.Rows[a].Cells[0].Value;


                DataSet ds = null;
                int result = -1;
                try
                {
                    string strSql = string.Format("exec UP_TCusForm_Delete '{0}'", id);
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

                if (result == 0)
                {
                    MessageBox.Show("删除成功！", "提示");
                    query2();
                }
            }
        }

        private void query2()
        {
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT b.ID,a.SName,a.Norms,a.Texture,b.Num,b.Price FROM  dbo.TSale a,dbo.TCusForm b WHERE a.ID=b.SID and b.CID='{0}' and b.tag=0", ID);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns["ID"].ColumnName = "订单ID";
                    dt.Columns["SName"].ColumnName = "产品名称";
                    dt.Columns["Norms"].ColumnName = "规格";
                    dt.Columns["Texture"].ColumnName = "材质";
                    dt.Columns["Num"].ColumnName = "数量";
                    dt.Columns["Price"].ColumnName = "总价格";
                    this.dataGridView2.DataSource = dt.DefaultView;

                    //AutoSizeColumn(dataGridView2);

                }
                else
                {
                    if (dataGridView2.DataSource != null)
                    {
                        dataGridView2.DataSource = null;
                    }
                }

            }
            catch
            {

            }
        }

        private void query()
        {
            DataSet ds = null;
            try
            {
                string strSql = string.Format("select ID,SName,Norms,Texture,Price,Num from TSale where SName like '%{0}%'", txt_name.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns["ID"].ColumnName = "产品ID";
                    dt.Columns["SName"].ColumnName = "产品名称";
                    dt.Columns["Norms"].ColumnName = "规格";
                    dt.Columns["Texture"].ColumnName = "材质";
                    dt.Columns["Price"].ColumnName = "价格";
                    dt.Columns["Num"].ColumnName = "库存数量";
                    this.dataGridView1.DataSource = dt.DefaultView;

                    //AutoSizeColumn(dataGridView1);

                }
                else {
                    if (dataGridView1.DataSource != null)
                    {
                        dataGridView1.DataSource = null;
                    }
                }

            }
            catch
            {

            }
        }

        /// <summary>
        /// 使DataGridView的列自适应宽度
        /// </summary>
        /// <param name="dataGridView1"></param>
        private void AutoSizeColumn(DataGridView dgViewFiles)
        {
            int width = 0;
            //使列自使用宽度
            //对于DataGridView的每一个列都调整
            for (int i = 0; i < dgViewFiles.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                dgViewFiles.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += dgViewFiles.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，
            //如果是小于原来设定的宽度，将模式改为填充。
            if (width > dgViewFiles.Size.Width)
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            //冻结某列 从左开始 0，1，2
            //dgViewFiles.Columns[1].Frozen = true;
        }

        
      


    }
}
