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
    public partial class DDCX : Form
    {
        private Boolean isAuto = false;
        public DDCX()
        {
            InitializeComponent();
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            query();
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

        private void query()
        {
            string str = "";
            if (radioButton1.Checked)
            {
                
            }
            if (radioButton2.Checked)
            {
                str = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddMonths(-1).ToShortDateString();
            }
            if (radioButton3.Checked)
            {
                str = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddMonths(-3).ToShortDateString();
            }
            if (radioButton4.Checked)
            {
                str = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddMonths(-6).ToShortDateString();
            }
            if (radioButton5.Checked)
            {
                str = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddMonths(-12).ToShortDateString();
            }
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT b.FID,c.CName,a.SName,a.Norms,a.Texture,b.Num,b.Price,b.Tag,b.Time FROM  dbo.TSale a,dbo.TCusForm b,dbo.TCustomer c WHERE a.ID=b.SID and b.CID=c.ID and b.Tag<>0 AND c.CName like '%{0}%' and b.Time>'{1}' and b.FID like '%{2}%'", txt_name.Text, str,textBox1.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns["FID"].ColumnName = "订单ID";
                    dt.Columns["CName"].ColumnName = "客户姓名";
                    dt.Columns["SName"].ColumnName = "产品名称";
                    dt.Columns["Norms"].ColumnName = "规格";
                    dt.Columns["Texture"].ColumnName = "材质";
                    dt.Columns["Num"].ColumnName = "数量";
                    dt.Columns["Price"].ColumnName = "总价格";
                    dt.Columns["Tag"].ColumnName = "订单状态";
                    dt.Columns["Time"].ColumnName = "下单时间";
                    this.dataGridView1.DataSource = dt.DefaultView;
                    queryCus();
                    queryCount();
                    if (!isAuto)
                    {
                        AutoSizeColumn(dataGridView1);
                        isAuto = true;
                    }

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

        private void queryCount()
        {
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT SUM(b.Num) num,SUM(b.Price) price FROM  dbo.TSale a,dbo.TCusForm b,dbo.TCustomer c WHERE a.ID=b.SID and b.CID=c.ID AND b.Tag<>0 AND c.CName  like '%{0}%' and b.FID like '%{1}%'", txt_name.Text,textBox1.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    lab_num.Text = dt.Rows[0]["num"].ToString();
                    lab_price.Text = dt.Rows[0]["price"].ToString();
                    
                }
                else
                {
                    lab_num.Text = "";
                    lab_price.Text = "";
       
                }

            }
            catch
            {
                lab_num.Text = "";
                lab_price.Text = "";
                
            }
        }

        private void queryCus()
        {

            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT ID FROM dbo.TCustomer WHERE CName like '%{0}%' and FID like '%{1}%'", txt_name.Text,textBox1.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    //CID = (int)dt.Rows[0]["ID"];

                }

            }
            catch
            {

            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 6)//找到类型列
            {
                e.Value = Convert.ToInt32(e.Value) == 0 ? "未下单" : "已下单";
            }


        }

    }
}
