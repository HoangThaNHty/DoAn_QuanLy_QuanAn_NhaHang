using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class ChiTietHoaDonDAL : DBConnect
    {

        public List<CTHD_HienThi> GetChiTietHoaDon(int mahd)
        {
            List<CTHD_HienThi> ds = new List<CTHD_HienThi>();
            string sql = @"SELECT ct.MaChiTiet, ct.MaHoaDon, ct.MaMon, m.TenMon,m.DonGia, ct.SoLuong, ct.ThanhTien FROM ChiTietHoaDon ct JOIN MonAn m ON ct.MaMon = m.MaMon WHERE ct.MaHoaDon = @MaHoaDon";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaHoaDon", mahd);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ct = new CTHD_HienThi
                        {
                            MaChiTiet = Convert.ToInt32(reader["MaChiTiet"]),
                            MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                            MaMon = Convert.ToInt32(reader["MaMon"]),
                            TenMon = reader["TenMon"].ToString(),
                            DonGia = Convert.ToDecimal(reader["DonGia"]),
                            SoLuong = Convert.ToInt32(reader["SoLuong"]),
                            ThanhTien = Convert.ToDecimal(reader["ThanhTien"])
                        };
                        ds.Add(ct);
                    }
                }
                conn.Close();
            }

            return ds;

        }


        public bool ThemMonVaoHoaDon(int maHoaDon, int maMon)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ThemMonVaoHoaDon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MaHoaDon", SqlDbType.Int).Value = maHoaDon;
                    cmd.Parameters.Add("@MaMon", SqlDbType.Int).Value = maMon;

                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    int rows = cmd.ExecuteNonQuery();

                    return rows > 0 || rows == -1;
                }
            }
            catch (Exception ex)
            {
                conn.Close();

                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public bool GiamSoLuongChiTiet(int maChiTiet)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_GiamSoLuongChiTiet", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaChiTiet", maChiTiet);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();

                    return rows > 0 || rows == -1;
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }
        }
        public bool TangSoLuongChiTiet(int maChiTiet, out string message)
        {
            message = "";

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_TangSoLuongChiTiet", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaChiTiet", maChiTiet);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();

                    return rows > 0 || rows == -1;

                }
            }
            catch (SqlException ex)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                message = ex.Message; 
                return false;
            }
        }
    }

}


