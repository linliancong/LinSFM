using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinSFMSys
{
    public class DbFunc
    {

        public static string strDbWSConn = "Data Source=127.0.0.1;Initial Catalog=LinSFM;User ID=sa;Password=lin1234";

        //public static string strDbWSConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public DbFunc()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static long ExecSql(System.String v_szSql)
        {

            //string msg = ""; 
            //int i=-1;
            //验证是否有权访问 
            //if(!myHeader.IsValid(out msg)) 
            //{
            //	return i;
            //}
            //else
            //{
            SqlConnection conn = null;
            try
            {
                conn = new System.Data.SqlClient.SqlConnection(strDbWSConn);
                conn.Open();

                SqlCommand Command = new SqlCommand(v_szSql, conn);
                object result = Command.ExecuteScalar();

                if (result != null)
                    return Convert.ToInt64(result);
                else
                    return Int32.MinValue;


                //System.Int32	iRet = Command.ExecuteNonQuery();


                //return iRet;
            }
            catch
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }
            //}
        }


        public static DataSet GetTable(System.String v_szSql, System.String v_szTableName)
        {
            //DataSet ds=null;
            //if(!myHeader.IsValid(out msg)) 
            //	{
            //		return ds;
            //	}
            //	else
            //	{
            try
            {
                SqlConnection conn = new System.Data.SqlClient.SqlConnection(strDbWSConn);
                conn.Open();

                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(v_szSql, conn);
                DataSet mDataSet = new System.Data.DataSet();
                da.Fill(mDataSet, v_szTableName);

                conn.Close();
                return mDataSet;
            }
            catch
            {
                return null;
            }
            //}
        }


        public static DataSet GetTableNoName(System.String v_szSql)
        {
            //DataSet ds=null;
            //if(!myHeader.IsValid(out msg)) 
            //{
            //	return ds;
            //}
            //else
            //{
            try
            {
                SqlConnection conn = new System.Data.SqlClient.SqlConnection(strDbWSConn);
                conn.Open();

                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(v_szSql, conn);
                DataSet mDataSet = new System.Data.DataSet();
                da.Fill(mDataSet);

                conn.Close();
                return mDataSet;
            }
            catch
            {
                return null;
            }
            //}
        }


        public static System.Object ExecuteScalar(System.String v_szSql)
        {
            //System.Object o = null;
            try
            {
                SqlConnection conn = new System.Data.SqlClient.SqlConnection(strDbWSConn);
                conn.Open();

                SqlCommand Command = new SqlCommand(v_szSql, conn);
                System.Object obj = Command.ExecuteScalar();
                Command.Dispose();
                conn.Close();
                conn.Dispose();


                return obj;
            }
            catch
            {
                return null;
            }
        }

    }
}
