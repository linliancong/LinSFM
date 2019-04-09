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
    public partial class XTSZ : Form
    {
        private Boolean isAuto = false;
        public XTSZ()
        {
            InitializeComponent();
            query();
        }     

        private void btn_add_Click(object sender, EventArgs e)
        {
            XTSZ_Operator xo = new XTSZ_Operator();
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
                XTSZ_Operator xo = new XTSZ_Operator(dataGridView1.Rows[a].Cells[0].Value.ToString(), dataGridView1.Rows[a].Cells[1].Value.ToString(), dataGridView1.Rows[a].Cells[2].Value.ToString());
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
                    string strSql = string.Format("exec UP_TRole_Delete '{0}'", id);
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

        private void query()
        {
            DataSet ds = null;
            try
            {
                string strSql = string.Format("SELECT a.RoleID,a.RoleName,b.RMenuID,b.RMenuName FROM dbo.TRole a LEFT JOIN dbo.TRole_Menu b ON a.RoleID=b.RID");
                ds = DbFunc.GetTableNoName(strSql);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt.Columns["RoleID"].ColumnName = "角色ID";
                    dt.Columns["RoleName"].ColumnName = "角色名称";
                    dt.Columns["RMenuID"].ColumnName = "对应权限ID";
                    dt.Columns["RMenuName"].ColumnName = "对应权限";
                    
                    
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
