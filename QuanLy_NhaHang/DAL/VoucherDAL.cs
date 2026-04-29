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
    public class VoucherDAL : DBConnect
    {
        public bool Them(Voucher km)
        {
            string sql = @"INSERT INTO KhuyenMai (TenKM, MoTa, PhanTramGiam, NgayBatDau, NgayKetThuc)
                       VALUES (@TenKM, @MoTa, @PhanTramGiam, @NgayBatDau, @NgayKetThuc)";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TenKM", km.TenKM);
                cmd.Parameters.AddWithValue("@MoTa", km.MoTa);
                cmd.Parameters.AddWithValue("@PhanTramGiam", km.PhanTramGiam);
                cmd.Parameters.AddWithValue("@NgayBatDau", km.NgayBatDau);
                cmd.Parameters.AddWithValue("@NgayKetThuc", km.NgayKetThuc);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows > 0;
            }
        }

        public List<Voucher> LayDanhSach()
        {
            List<Voucher> ds = new List<Voucher>();
            string sql = "SELECT * FROM KhuyenMai";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ds.Add(new Voucher
                        {
                            MaKM = Convert.ToInt32(reader["MaKM"]),
                            TenKM = reader["TenKM"].ToString(),
                            MoTa = reader["MoTa"].ToString(),
                            PhanTramGiam = Convert.ToInt32(reader["PhanTramGiam"]),
                            NgayBatDau = Convert.ToDateTime(reader["NgayBatDau"]),
                            NgayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"])
                        });
                    }
                }
                conn.Close();
            }

            return ds;
        }

        public bool Sua(Voucher km)
        {
            string sql = @"UPDATE KhuyenMai SET MoTa = @MoTa, PhanTramGiam = @PhanTramGiam,
                       NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc WHERE MaKM = @MaKM";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaKM", km.MaKM);
                cmd.Parameters.AddWithValue("@TenKM", km.TenKM);
                cmd.Parameters.AddWithValue("@MoTa", km.MoTa);
                cmd.Parameters.AddWithValue("@PhanTramGiam", km.PhanTramGiam);
                cmd.Parameters.AddWithValue("@NgayBatDau", km.NgayBatDau);
                cmd.Parameters.AddWithValue("@NgayKetThuc", km.NgayKetThuc);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows > 0;
            }
        }

        public bool Xoa(int maKM)
        {
            string sql = "DELETE FROM KhuyenMai WHERE MaKM = @MaKM";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaKM", maKM);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows > 0;
            }
        }

        public bool KiemTraTrungTen(string tenKM)
        {
            string sql = "SELECT COUNT(*) FROM KhuyenMai WHERE TenKM = @TenKM";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TenKM", tenKM);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();
                return count > 0;
            }
        }

        public Voucher LayKhuyenMaiTheoTen(string tenKM)
        {
            Voucher km = null;

            string sql = "SELECT * FROM KhuyenMai WHERE TenKM = @TenKM";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TenKM", tenKM);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        km = new Voucher
                        {
                            MaKM = Convert.ToInt32(reader["MaKM"]),
                            TenKM = reader["TenKM"].ToString(),
                            MoTa = reader["MoTa"].ToString(),
                            PhanTramGiam = Convert.ToInt32(reader["PhanTramGiam"]),
                            NgayBatDau = Convert.ToDateTime(reader["NgayBatDau"]),
                            NgayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"])
                        };
                    }
                }

                conn.Close();
            }

            return km;
        }
    }
}