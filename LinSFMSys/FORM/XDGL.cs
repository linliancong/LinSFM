using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinSFMSys
{
    public partial class XDGL : Form
    {
        private Boolean isAuto=false;
        private int CID = 0;
        private string FID = "";
        string filePath = "";
        public XDGL()
        {
            InitializeComponent();
            btn_up.Visible = false;
            btn_place.Enabled = false;
            progressBar1.Visible = false;
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            query();
            
        }

        private void btn_place_Click(object sender, EventArgs e)
        {                       
            string str = "XLS";     
            DateTime currentTime = DateTime.Now;
            str+= currentTime.ToString("yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo);
            Random rd = new Random();
            str += rd.Next(100, 999).ToString();

            DataSet ds = null;
            int result = -1;
            try
            {
                string strSql = string.Format("exec UP_TForm_ADD '{0}','{1}','{2}','{3}','{4}'",
                     str,CID,lab_num.Text, lab_price.Text, 0);
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
                MessageBox.Show("下单成功，请尽快上销售合同！", "提示");
                query();
            }
            else
            {
                MessageBox.Show("下单失败！", "提示");
            }
        }

        private void btn_up_Click(object sender, EventArgs e)
        {

            //创建文件弹出选择窗口（包括文件名）对象
            OpenFileDialog ofd = new OpenFileDialog();
            //判断选择的路径
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                progressBar1.Visible = true;
                //this.txtSoundPath.Text = ofd.FileName.ToString();
                filePath = ofd.FileName.ToString();
                try
                {
                    //上传后文件保存的名称
                    string saveName = FID+".doc";
                    int count = UpFile.UpSound_Request(filePath, saveName, this.progressBar1);
                    if (count > 0)
                    {
                        MessageBox.Show("上传文件成功！");
                        progressBar1.Visible = false;
                        
                        DataSet ds = null;
                        int result = -1;
                        try
                        {
                            string strSql = string.Format("exec UP_TForm_ADD '{0}','{1}','{2}','{3}','{4}'",
                                 FID, CID, lab_num.Text, lab_price.Text, 1);
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
                        
                        query();
                    }
                    else
                    {
                        MessageBox.Show("上传文件失败！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex.GetBaseException());
                }
            }
                   
        }

        private void query()
        {
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT b.FID,a.SName,a.Norms,a.Texture,b.Num,b.Price,b.Tag FROM  dbo.TSale a,dbo.TCusForm b,dbo.TCustomer c WHERE a.ID=b.SID and b.CID=c.ID and b.Tag<>2 AND  c.CName='{0}'", txt_name.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    FID = dt.Rows[0]["FID"].ToString();
                    dt.Columns["FID"].ColumnName = "订单ID";
                    dt.Columns["SName"].ColumnName = "产品名称";
                    dt.Columns["Norms"].ColumnName = "规格";
                    dt.Columns["Texture"].ColumnName = "材质";
                    dt.Columns["Num"].ColumnName = "数量";
                    dt.Columns["Price"].ColumnName = "总价格";
                    dt.Columns["Tag"].ColumnName = "订单状态";
                    this.dataGridView1.DataSource = dt.DefaultView;
                    queryCus();
                    queryCount();
                    //btn_place.Enabled = true;

                    if (!isAuto)
                    {
                        AutoSizeColumn(dataGridView1);
                        isAuto = true;
                    }

                }
                else {
                    if (dataGridView1.DataSource != null) {
                        dataGridView1.DataSource = null;
                        lab_num.Text = "";
                        lab_price.Text = "";
                        lab_tap.Text = "";
                        btn_place.Enabled = false;
                    }
                }

            }
            catch
            {
                if (dataGridView1.DataSource != null)
                {
                    dataGridView1.DataSource = null;
                    lab_num.Text = "";
                    lab_price.Text = "";
                    lab_tap.Text = "";
                    btn_place.Enabled = false;
                }
            }
        }

        private void queryCount()
        {
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT SUM(b.Num) num,SUM(b.Price) price,ISNULL(b.FID,0) tag FROM  dbo.TSale a,dbo.TCusForm b,dbo.TCustomer c WHERE a.ID=b.SID and b.CID=c.ID AND b.Tag<>2 AND c.CName='{0}' GROUP BY b.FID", txt_name.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    lab_num.Text = dt.Rows[0]["num"].ToString();
                    lab_price.Text = dt.Rows[0]["price"].ToString();
                    if (dt.Rows[0]["tag"].ToString().Equals("0"))
                    {
                        btn_place.Enabled = true;
                        lab_tap.Text = "未下单(不可上传)";

                    }
                    else {
                        //已下单情况下查询合同上传状态
                        btn_place.Enabled = false;
                        queryTag();
                        
                    }
                }
                else
                {
                    lab_num.Text = "";
                    lab_price.Text = "";
                    lab_tap.Text = "";
                }

            }
            catch
            {
                lab_num.Text = "";
                lab_price.Text = "";
                lab_tap.Text = "";
            }
        }

        private void queryTag()
        {
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT a.tag FROM  dbo.TForm a,dbo.TCusForm b,dbo.TCustomer c WHERE a.FID=b.FID AND b.CID=c.ID AND b.Tag=1 AND c.CName='{0}'", txt_name.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    
                    if (dt.Rows[0]["tag"].ToString().Equals("0"))
                    {
                        lab_tap.Text = "未上传";
                        btn_up.Visible = true;
                       

                    }

                }
                else
                {
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

        private void queryCus()
        {
            
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT ID FROM dbo.TCustomer WHERE CName='{0}'", txt_name.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    CID = (int)dt.Rows[0]["ID"];

                }               

            }
            catch
            {

            }
        }

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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 6)//找到类型列
            {
                e.Value = Convert.ToInt32(e.Value) == 0 ? "未下单" : "已下单";
            }
            

        }

         
    }
}
