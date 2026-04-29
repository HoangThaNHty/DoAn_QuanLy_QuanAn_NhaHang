using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace DAL
{
    public class DBConnect
    {
        //protected SqlConnection conn = new SqlConnection(
        //    "Data Source=LAPTOP-FQ575CP6\\SQLEXPRESS;Initial Catalog=QuanLy_NhaHang_Test;Integrated Security=True;"
        //);

        protected SqlConnection conn = new SqlConnection("Data Source=LAPTOP-6A1M9L9L\\SQLEXPRESS;Initial Catalog=QL_NHAHANG;User ID=sa;Password=123;");


        // Method để GwtData -- trả về DataTable
        public DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thực thi GetData: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }

        // Method để ExecuteNonQuery -- trả về bool
        public bool ExecuteNonQuery(string sql)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thực thi ExecuteNonQuery: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // Method để ExecuteNonQuery với parameters
        //public bool ExecuteNonQuery(string sql, SqlParameter[] parameters)
        //{
        //    try
        //    {
        //        if (conn.State == ConnectionState.Closed)
        //            conn.Open();

        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddRange(parameters);
        //        int result = cmd.ExecuteNonQuery();
        //        return result > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Lỗi khi thực thi ExecuteNonQuery: " + ex.Message);
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //            conn.Close();
        //    }
        //}




    }



}
