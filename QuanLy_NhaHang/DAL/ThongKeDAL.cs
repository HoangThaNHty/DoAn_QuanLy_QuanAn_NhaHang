using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace DAL
{
    public class ThongKeDAL: DBConnect
    {
        public DataTable LayDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            DataTable dt = new DataTable();
            //string query = @"SELECT CAST(ThoiGianLap AS DATE) AS Ngay,SUM(ThanhToan) AS TongDoanhThu FROM HoaDon WHERE ThanhToan > 0 AND CAST(ThoiGianLap AS DATE) BETWEEN @TuNgay AND @DenNgay GROUP BY CAST(ThoiGianLap AS DATE) ORDER BY Ngay";
            string query = @"SELECT CAST(ThoiGianLap AS DATE) AS Ngay,SUM(ThanhToan) AS TongDoanhThu,COUNT(*) AS SoHoaDon FROM HoaDon WHERE ThanhToan > 0 AND CAST(ThoiGianLap AS DATE) BETWEEN @TuNgay AND @DenNgay GROUP BY CAST(ThoiGianLap AS DATE) ORDER BY Ngay";



            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Date);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable LayTopMonAnBanChay(DateTime tuNgay, DateTime denNgay)
        {
            DataTable dt = new DataTable();
            string query = @" SELECT TOP 5 M.TenMon, SUM(CT.SoLuong) AS TongSoLuong, SUM(CT.SoLuong * M.DonGia) AS TongTien 
                                FROM ChiTietHoaDon CT 
                                JOIN HoaDon HD ON CT.MaHoaDon = HD.MaHoaDon
                                JOIN MonAn M ON CT.MaMon = M.MaMon
                                WHERE CAST(HD.ThoiGianLap AS DATE) BETWEEN @TuNgay AND @DenNgay
                                AND HD.ThanhToan > 0
                                GROUP BY M.TenMon
                                ORDER BY TongSoLuong DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Date);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public string LayTenMonAnBanChayNhat(DateTime tuNgay, DateTime denNgay)
        {
            string tenMon = string.Empty;
            string query = @"SELECT TOP 1 M.TenMon
            FROM ChiTietHoaDon CT
            JOIN HoaDon HD ON CT.MaHoaDon = HD.MaHoaDon
            JOIN MonAn M ON CT.MaMon = M.MaMon
            WHERE CAST(HD.ThoiGianLap AS DATE) BETWEEN @TuNgay AND @DenNgay
            AND HD.ThanhToan > 0
            GROUP BY M.TenMon
            ORDER BY SUM(CT.SoLuong) DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Date);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay.Date);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    tenMon = result.ToString();
                }
                conn.Close();
            }

            return tenMon;
        }

        public DataTable LayDoanhThuTheoKhoangThang(DateTime tuNgay, DateTime denNgay)
        {
            DataTable dt = new DataTable();
            //string query = @"SELECT FORMAT(ThoiGianLap, 'MM/yyyy') AS ThangNam, SUM(ThanhToan) AS TongDoanhThu
            //    FROM HoaDon
            //    WHERE ThanhToan > 0
            //    AND ThoiGianLap >= @TuNgay
            //    AND ThoiGianLap < DATEADD(MONTH, 1, @DenNgay)
            //    GROUP BY FORMAT(ThoiGianLap, 'MM/yyyy')
            //    ORDER BY MIN(ThoiGianLap)";

            string query = @"SELECT 
FORMAT(ThoiGianLap, 'MM/yyyy') AS ThangNam,
    SUM(ThanhToan) AS TongDoanhThu,
    COUNT(*) AS SoHoaDon
FROM HoaDon
WHERE ThanhToan > 0
  AND ThoiGianLap >= @TuNgay
  AND ThoiGianLap < DATEADD(MONTH, 1, @DenNgay)
GROUP BY FORMAT(ThoiGianLap, 'MM/yyyy')
ORDER BY MIN(ThoiGianLap);
";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Date);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable LayHoaDonTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM HoaDon 
            WHERE ThanhToan > 0 
            AND CAST(ThoiGianLap AS DATE) 
            BETWEEN @TuNgay AND @DenNgay 
            ";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Date);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public DataTable LayTrangThaiBanAn()
        {
            DataTable dt = new DataTable();
            string query = @" SELECT TrangThai, COUNT(*) AS SoLuong FROM BanAn GROUP BY TrangThai";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
    }
}
