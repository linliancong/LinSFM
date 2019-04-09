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
    public partial class JCBJ : Form
    {
        private Boolean isAuto = false;
        public JCBJ()
        {
            InitializeComponent();
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            query();
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
                JCBJ_Operator xo = new JCBJ_Operator(dataGridView1.Rows[a].Cells[0].Value.ToString(), dataGridView1.Rows[a].Cells[1].Value.ToString(), dataGridView1.Rows[a].Cells[2].Value.ToString(), dataGridView1.Rows[a].Cells[3].Value.ToString());
                xo.ShowDialog();

                if (xo.DialogResult == DialogResult.OK && xo.ReturnValue == 0)
                {
                    query();
                }
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

        private void query()
        {
            DataSet ds = null;
            try
            {
                string strSql = string.Format("select ID,SName,Price,BasePrice from TSale where SName like '%{0}%'", txt_name.Text);
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns["ID"].ColumnName = "产品ID";
                    dt.Columns["SName"].ColumnName = "产品名称";              
                    dt.Columns["Price"].ColumnName = "销售价格";
                    dt.Columns["BasePrice"].ColumnName = "基础报价";
                    this.dataGridView1.DataSource = dt.DefaultView;

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
                }

            }
            catch
            {

            }
        }

    }
}
