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
    public class KhachHangDAL:DBConnect
    {

        public int InsertKhachHang(KhachHang kh)
        {
            int maKH = -1;
            string sql = "INSERT INTO KhachHang (HoTen, SoDienThoai, SoLuongKhach) " +
                         "OUTPUT INSERTED.MaKhachHang " +
                         "VALUES (@HoTen, @SoDienThoai, @SoLuongKhach)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@HoTen", kh.HoTen);
                cmd.Parameters.AddWithValue("@SoDienThoai", kh.SoDienThoai);
                cmd.Parameters.AddWithValue("@SoLuongKhach", kh.SoLuongKhach);

                conn.Open();
                maKH = (int)cmd.ExecuteScalar();
                conn.Close();
            }

            return maKH;
        }
        //cập nhật lại số luongj khách
        public bool CapNhatKhachHang(int maKhachHang, int soLuongMoi, string sdt)
        {
            string sql = "UPDATE KhachHang SET SoLuongKhach = @SoLuong, SoDienThoai = @SoDienThoai WHERE MaKhachHang = @MaKhachHang";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@SoLuong", soLuongMoi);
                cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                cmd.Parameters.AddWithValue("@SoDienThoai", sdt);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows > 0;
            }
        }

        //lấy khách từ mã hóa đoen
        public string GetTenKhachHangTuHoaDon(int maHoaDon)
        {
            string tenKhachHang = "";

            string sql = @" SELECT kh.HoTen FROM HoaDon hd JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang WHERE hd.MaHoaDon = @MaHoaDon";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    conn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        tenKhachHang = result.ToString();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return tenKhachHang;
        }

        public KhachHang GetKhachHangById(int maKH)
        {
            KhachHang kh = null;
            string query = "SELECT * FROM KhachHang WHERE MaKhachHang = @MaKH";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaKH", maKH);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    kh = new KhachHang
                    {
                        MaKhachHang = Convert.ToInt32(reader["MaKhachHang"]),
                        HoTen = reader["HoTen"].ToString(),
                        SoDienThoai = reader["SoDienThoai"].ToString(),
                        SoLuongKhach = Convert.ToInt32(reader["SoLuongKhach"])
                    };
                }

                conn.Close();
            }

            return kh;
        }
    }
}
