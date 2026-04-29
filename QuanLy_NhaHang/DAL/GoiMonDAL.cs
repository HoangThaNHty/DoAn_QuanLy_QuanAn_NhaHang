using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class GoiMonDAL : DBConnect
    {
        public List<BanAnChuaThanhToan> LayHoaDonBanTrong_TongTien0()
        {
            List<BanAnChuaThanhToan> ds = new List<BanAnChuaThanhToan>();
            string sql = @" SELECT hd.MaHoaDon, ba.MaBan, ba.Tang, ba.SoBan FROM HoaDon hd JOIN BanAn ba ON hd.MaBan = ba.MaBan WHERE hd.TongTien = 0 AND ba.TrangThai <> N'Trống'";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BanAnChuaThanhToan item = new BanAnChuaThanhToan
                        {
                            MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                            MaBan = Convert.ToInt32(reader["MaBan"]),
                            Tang = Convert.ToInt32(reader["Tang"]),
                            SoBan = Convert.ToInt32(reader["SoBan"])
                        };
                        ds.Add(item);
                    }
                }
                conn.Close();
            }

            return ds;
        }

    }
}
