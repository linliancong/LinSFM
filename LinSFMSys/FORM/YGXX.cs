using LinSFMSys.FORM;
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
    public partial class YGXX : Form
    {
        //private string FID = "";
        private Boolean isAuto = false;
        public YGXX()
        {
            InitializeComponent();
            progressBar1.Visible = false;
            btn_useradd.Visible = false;
            btn_userdel.Visible = false;
            button2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                MessageBox.Show("没有选中任何产品，请选中后重试！", "提示");
            }
            else
            {
                string filePath = "";

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
                        int a = dataGridView1.CurrentRow.Index;
                        int id = (int)dataGridView1.Rows[a].Cells[0].Value;
                        string name = dataGridView1.Rows[a].Cells[1].Value.ToString();
                        string saveName = name + ".doc";
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
                                string time = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
                                string strSql = string.Format("exec UP_TStaff_Resume '{0}','{1}'", id, saveName);
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
        }

        private void query()
        {

            DataSet ds = null;
            try
            {
                string strSql = string.Format("select ID,Name,Sex,Phone,Address,FamilyPhone,FamilyAddress,Birthday,EntryTime,Resume,State from  TStaff where Name like '%{0}%' ", txt_name.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns["ID"].ColumnName = "用户ID";
                    dt.Columns["Name"].ColumnName = "姓名";
                    dt.Columns["Sex"].ColumnName = "性别";
                    dt.Columns["Phone"].ColumnName = "联系电话";
                    dt.Columns["Address"].ColumnName = "现居住地";
                    dt.Columns["FamilyPhone"].ColumnName = "家庭电话";
                    dt.Columns["FamilyAddress"].ColumnName = "家庭地址";
                    dt.Columns["Birthday"].ColumnName = "出生日期";
                    dt.Columns["EntryTime"].ColumnName = "入职时间";
                    dt.Columns["Resume"].ColumnName = "简历名称";
                    dt.Columns["State"].ColumnName = "员工状态";

                    this.dataGridView1.DataSource = dt.DefaultView;

                    btn_useradd.Visible = true;
                    btn_userdel.Visible = true;
                    button2.Visible = true;
                    
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
                        btn_useradd.Visible = false;
                        btn_userdel.Visible = false;
                        button2.Visible = false;
                    }
                   
                }

            }
            catch
            {
                btn_useradd.Visible = false;
                btn_userdel.Visible = false;
                button2.Visible = false;
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

        private void btn_add_Click(object sender, EventArgs e)
        {
            YGXX_Operator xo = new YGXX_Operator();
            xo.ShowDialog();

            if (xo.DialogResult == DialogResult.OK && xo.ReturnValue == 0)
            {
                query();
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                MessageBox.Show("没有选中任何产品，请选中后重试！", "提示");
            }
            else
            {
                int a = dataGridView1.CurrentRow.Index;
                YGXX_Operator xo = new YGXX_Operator(dataGridView1.Rows[a].Cells[0].Value.ToString(), dataGridView1.Rows[a].Cells[1].Value.ToString(), 
                    (int)dataGridView1.Rows[a].Cells[2].Value, dataGridView1.Rows[a].Cells[3].Value.ToString(), dataGridView1.Rows[a].Cells[4].Value.ToString(), 
                    dataGridView1.Rows[a].Cells[5].Value.ToString(),dataGridView1.Rows[a].Cells[6].Value.ToString(),
                    dataGridView1.Rows[a].Cells[7].Value.ToString(), dataGridView1.Rows[a].Cells[8].Value.ToString(),
                    dataGridView1.Rows[a].Cells[9].Value.ToString(), (int)dataGridView1.Rows[a].Cells[10].Value
                    );
                xo.ShowDialog();

                if (xo.DialogResult == DialogResult.OK && xo.ReturnValue == 0)
                {
                    query();
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                MessageBox.Show("没有选中任何产品，请选中后重试！", "提示");
            }
            else
            {
                int a = dataGridView1.CurrentRow.Index;
                int id = (int)dataGridView1.Rows[a].Cells[0].Value;


                DataSet ds = null;
                int result = -1;
                try
                {
                    string strSql = string.Format("exec UP_TStaff_Delete '{0}'", id);
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
                    query();
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2)//找到类型列
            {
                e.Value = Convert.ToInt32(e.Value) == 0 ? "男" : "女";
            }
            if (e.ColumnIndex == 10)//找到类型列
            {
                e.Value = Convert.ToInt32(e.Value) == 0 ? "在职" : "离职";
            }
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            query();
        }

        private void btn_useradd_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                MessageBox.Show("没有选中任何产品，请选中后重试！", "提示");
            }
            else
            {
                int a = dataGridView1.CurrentRow.Index;
                USER_Operator xo = new USER_Operator((int)dataGridView1.Rows[a].Cells[0].Value, dataGridView1.Rows[a].Cells[1].Value.ToString());
                xo.ShowDialog();

                if (xo.DialogResult == DialogResult.OK && xo.ReturnValue == 0)
                {
                    query();
                }
            }
        }

        private void btn_userdel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                MessageBox.Show("没有选中任何产品，请选中后重试！", "提示");
            }
            else
            {
                int a = dataGridView1.CurrentRow.Index;
                int id = (int)dataGridView1.Rows[a].Cells[0].Value;


                DataSet ds = null;
                int result = -1;
                try
                {
                    string strSql = string.Format("exec UP_TStaff_Delete {0},{1}", id,1);
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
                    query();
                }
            }
        }

        

    }
}
