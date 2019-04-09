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
    public partial class WLPS : Form
    {
        private Boolean isAuto = false;
        private string FID = "";
        public WLPS()
        {
            InitializeComponent();
            progressBar1.Visible = false;
            btn_up1.Enabled = false;
            btn_up2.Enabled = false;
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            query();
        }

        //上传配送凭证
        private void btn_up1_Click(object sender, EventArgs e)
        {
            string filePath = "";
            FID = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();

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
                    string saveName = FID + "_PSPZ"+".jpg";
                    int count = UpFile.UpSound_Request( filePath, saveName, this.progressBar1);
                    if (count > 0)
                    {
                        MessageBox.Show("上传文件成功！");
                        progressBar1.Visible = false;

                        DataSet ds = null;
                        int result = -1;
                        try
                        {
                            DateTime currentTime = DateTime.Now;
                            string time = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
                            string strSql = string.Format("exec UP_TLogistics_ADD '{0}','{1}','{2}','{3}','{4}','{5}'",
                                 FID, 1, saveName, dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[9].Value.ToString(), time, dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[10].Value.ToString());
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

        //上传收货凭证
        private void btn_up2_Click(object sender, EventArgs e)
        {
            string filePath = "";
            FID = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();

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
                    string saveName = FID + "_SHPZ"+".jpg";
                    int count = UpFile.UpSound_Request(filePath, saveName, this.progressBar1);
                    if (count > 0)
                    {
                        MessageBox.Show("上传文件成功！");
                        progressBar1.Visible = false;

                        DataSet ds = null;
                        int result = -1;
                        try
                        {
                            DateTime currentTime = DateTime.Now;
                            string time= currentTime.ToString("yyyy-MM-dd HH:mm:ss");
                            string strSql = string.Format("exec UP_TLogistics_ADD '{0}','{1}','{2}','{3}','{4}','{5}'",
                                 FID, 2, dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[7].Value.ToString(), saveName, dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[8].Value.ToString(), time);
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
            int state=0;
            if (radioButton1.Checked)
            {
                state = 0;
            }
            if (radioButton2.Checked)
            {
                state = 1;
            }
            if (radioButton3.Checked)
            {
                state=2;
            }
            
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT * FROM (SELECT a.FID,a.Num,a.Time,b.CName,b.Phone,b.Address,ISNULL(c.State,0) State,ISNULL(c.Certificate1,0) Certificate1,c.Time1 Time1,ISNULL(c.Certificate2,0) Certificate2,c.Time2 Time2 FROM dbo.TForm a INNER JOIN dbo.TCustomer b on a.CID=b.ID AND b.CName like '%{0}%' LEFT JOIN TLogistics c ON a.FID=c.FID ) AS Z WHERE Z.State={1} ", txt_name.Text, state);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns["FID"].ColumnName = "订单ID";
                    dt.Columns["Num"].ColumnName = "数量";
                    dt.Columns["CName"].ColumnName = "客户名字";
                    dt.Columns["Phone"].ColumnName = "客户手机";
                    dt.Columns["Address"].ColumnName = "客户地址";
                    dt.Columns["Time"].ColumnName = "下单时间";
                    dt.Columns["State"].ColumnName = "配送状态";
                    dt.Columns["Certificate1"].ColumnName = "送货凭证";
                    dt.Columns["Time1"].ColumnName = "送货时间";
                    dt.Columns["Certificate2"].ColumnName = "收货凭证";                   
                    dt.Columns["Time2"].ColumnName = "收货时间";

                    this.dataGridView1.DataSource = dt.DefaultView;
                    btn_up1.Enabled = true;
                    btn_up2.Enabled = true;

                    if (!isAuto)
                    {
                        AutoSizeColumn(dataGridView1);
                        isAuto = true;
                    }

                }
                else
                {
                    if (dataGridView1.DataSource != null)
                    {
                        dataGridView1.DataSource = null;
                    }
                    btn_up1.Enabled = false;
                    btn_up2.Enabled = false;
                }

            }
            catch
            {
                btn_up1.Enabled = false;
                btn_up2.Enabled = false;
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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 6)//找到类型列
            {
                e.Value = Convert.ToInt32(e.Value) == 0 ? "未配送" : Convert.ToInt32(e.Value) == 1 ?"配送中":"已完成";
            }
            //if (e.ColumnIndex == 7)//找到类型列
            //{
            //    e.Value = Convert.ToInt32(e.Value) == 0 ? "未上传" : "已上传";
            //}
            //if (e.ColumnIndex == 9)//找到类型列
            //{
            //    e.Value = Convert.ToInt32(e.Value) == 0 ? "未上传" : "已上传";
            //}
        }

        



    }
}
