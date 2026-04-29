using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class HoaDonDAL : DBConnect
    {
        // Lấy danh sách hóa đơn(đã sửa)
        public List<HoaDon> GetAllHoaDon()
        {
            string query = @"SELECT HD.MaHoaDon, HD.MaBan, HD.MaNhanVien, HD.ThoiGianLap, HD.TongTien, HD.MaKhachHang, HD.GiamGia, HD.ThanhToan
                            FROM HoaDon HD
                            WHERE HD.ThanhToan > 0 AND HD.TongTien > 0
                           ";

            List<HoaDon> list = new List<HoaDon>();
            using (SqlCommand command = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HoaDon item = new HoaDon()
                        {
                            MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                            MaKhachHang = Convert.ToInt32(reader["MaKhachHang"]),
                            MaBan = Convert.ToInt32(reader["MaBan"]),
                            MaNhanVien = Convert.ToInt32(reader["MaNhanVien"]),
                            ThoiGianLap = Convert.ToDateTime(reader["ThoiGianLap"].ToString()),
                            TongTien = Convert.ToDecimal(reader["TongTien"]),
                            GiamGia = Convert.ToInt32(reader["GiamGia"]),
                            ThanhToan = Convert.ToDecimal(reader["ThanhToan"])


                        };
                        list.Add(item);
                    }
                }

            }
            return list;
        }


       
        public bool InsertHoaDon(HoaDon hd)
        {
            string sql = "INSERT INTO HoaDon (MaBan, MaNhanVien, MaKhachHang, ThoiGianLap, TongTien, GiamGia, ThanhToan) " +
                         "VALUES (@MaBan, @MaNhanVien, @MaKhachHang, @ThoiGianLap, @TongTien, @GiamGia, @ThanhToan)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaBan", hd.MaBan);
                cmd.Parameters.AddWithValue("@MaNhanVien", hd.MaNhanVien);
                cmd.Parameters.AddWithValue("@MaKhachHang", hd.MaKhachHang);
                cmd.Parameters.AddWithValue("@ThoiGianLap", hd.ThoiGianLap);
                cmd.Parameters.AddWithValue("@TongTien", hd.TongTien);
                cmd.Parameters.AddWithValue("@GiamGia", hd.GiamGia);
                cmd.Parameters.AddWithValue("@ThanhToan", hd.ThanhToan);



                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //conn.Close();

                return rows > 0;
            }
        }

       
        // lấy hóa đơn chưa thanh toán(tt)
        public List<DSHoaDonCanThanhToan> GetHoaDonChuaThanhToan()
        {

            string query = @"SELECT hd.MaHoaDon,kh.MaKhachHang, ba.MaBan, ba.SoBan,ba.Tang, ba.TrangThai FROM HoaDon hd JOIN BanAn ba ON hd.MaBan = ba.MaBan JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang WHERE hd.TongTien = 0 AND ba.TrangThai = N'Đang dùng'";

            List<DSHoaDonCanThanhToan> ds = new List<DSHoaDonCanThanhToan>();

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DSHoaDonCanThanhToan item = new DSHoaDonCanThanhToan
                        {
                            MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                            MaKhachHang = Convert.ToInt32(reader["MaKhachHang"]),
                            MaBan = Convert.ToInt32(reader["MaBan"]),
                            SoBan = Convert.ToInt32(reader["SoBan"]),
                            Tang = Convert.ToInt32(reader["Tang"]),
                            TrangThai = reader["TrangThai"].ToString()
                        };
                        ds.Add(item);
                    }
                }
                conn.Close();
            }

            return ds;
        }

        public decimal TinhTongTienHoaDon(int maHoaDon)
        {
            decimal tongTien = 0;

            using (SqlCommand cmd = new SqlCommand("sp_TinhTongTienHoaDon", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                SqlParameter output = new SqlParameter("@TongTien", SqlDbType.Decimal);
                output.Direction = ParameterDirection.Output;
                output.Precision = 18;
                output.Scale = 2;
                cmd.Parameters.Add(output);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                tongTien = (decimal)output.Value;
            }

            return tongTien;
        }

        public bool CapNhatHoaDon(int maHoaDon, decimal tongTien, int giamGia, decimal thanhtoan)
        {
            bool thanhCong = false;

            string sql = @"UPDATE HoaDon SET TongTien = @TongTien, GiamGia = @GiamGia, ThanhToan = @ThanhToan WHERE MaHoaDon = @MaHoaDon";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                cmd.Parameters.AddWithValue("@TongTien", tongTien);
                cmd.Parameters.AddWithValue("@GiamGia", giamGia);
                cmd.Parameters.AddWithValue("@ThanhToan", thanhtoan);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                thanhCong = rows > 0;
            }

            return thanhCong;
        }
    }
}
