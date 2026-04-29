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
    public class BanAnDAL : DBConnect
    {
        public List<BanAn> GetBanAn()
        {
            List<BanAn> list = new List<BanAn>();
            string sql = "SELECT MaBan, SoBan, SoChoNgoi, TrangThai, Tang FROM BanAn";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    BanAn ban = new BanAn
                    {
                        MaBan = (int)dr["MaBan"],
                        SoBan = int.Parse(dr["SoBan"].ToString()),
                        SoChoNgoi = int.Parse(dr["SoChoNgoi"].ToString()),
                        TrangThai = dr["TrangThai"].ToString(),
                        Tang = int.Parse(dr["Tang"].ToString()),
                    };
                    list.Add(ban);
                }

                conn.Close();
            }

            return list;
        }

        public List<BanAn> GetBanAnTheoTang(int tang)
        {
            List<BanAn> list = new List<BanAn>();
            string sql = "SELECT MaBan, SoBan, SoChoNgoi, TrangThai, Tang FROM BanAn WHERE Tang = @Tang";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Tang", tang);

                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BanAn ban = new BanAn
                        {
                            MaBan = Convert.ToInt32(dr["MaBan"]),
                            SoBan = Convert.ToInt32(dr["SoBan"]),
                            SoChoNgoi = Convert.ToInt32(dr["SoChoNgoi"]),
                            TrangThai = dr["TrangThai"].ToString(),
                            Tang = Convert.ToInt32(dr["Tang"]),
                        };
                        list.Add(ban);
                    }
                }
            }

            return list;
        }


        public bool InsertBanAn(BanAn ban)
        {
            string sql = "INSERT INTO BanAn (SoBan, SoChoNgoi, TrangThai, Tang) VALUES (@SoBan, @SoChoNgoi, @TrangThai, @Tang)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@SoBan", ban.SoBan);
            cmd.Parameters.AddWithValue("@SoChoNgoi", ban.SoChoNgoi);
            cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);
            cmd.Parameters.AddWithValue("@Tang", ban.Tang);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdateBanAn(BanAn ban)
        {
            string sql = "UPDATE BanAn SET SoBan=@SoBan, SoChoNgoi=@SoChoNgoi, TrangThai=@TrangThai, Tang = @Tang WHERE MaBan=@MaBan";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@SoBan", ban.SoBan);
            cmd.Parameters.AddWithValue("@SoChoNgoi", ban.SoChoNgoi);
            cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);
            cmd.Parameters.AddWithValue("@MaBan", ban.MaBan);
            cmd.Parameters.AddWithValue("@Tang", ban.Tang);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteBanAn(int maBan)
        {
            string sql = "DELETE FROM BanAn WHERE MaBan=@MaBan";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaBan", maBan);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }


        public List<BanAn> LayBanAnTrong()
        {
            List<BanAn> dsBan = new List<BanAn>();
            string query = "SELECT MaBan, SoBan, SoChoNgoi, Tang, TrangThai  FROM BanAn WHERE TrangThai = N'Trống'";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BanAn ban = new BanAn
                    {
                        MaBan = reader.GetInt32(0),
                        SoBan = reader.GetInt32(1),
                        SoChoNgoi = reader.GetInt32(2),
                        Tang = reader.GetInt32(3),
                        TrangThai = reader.GetString(4)

                    };
                    dsBan.Add(ban);
                }
                conn.Close();
            }

            return dsBan;
        }

        //trang Đặt bàn ăn ----------------------------------------------



        public bool InsertDatBan(DatBan db)
        {
            string sql = "INSERT INTO DatBan (MaKhachHang, MaBan, ThoiGianDat, GhiChu) " +
                         "VALUES (@MaKhachHang, @MaBan, @ThoiGianDat, @GhiChu)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaKhachHang", db.MaKhachHang);
                cmd.Parameters.AddWithValue("@MaBan", db.MaBan);
                cmd.Parameters.AddWithValue("@ThoiGianDat", db.ThoiGianDat);
                cmd.Parameters.AddWithValue("@GhiChu", db.GhiChu);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows > 0;
            }
        }



        public List<KhachHangDaDatBanDTO> LayDanhSachKhachHangDaDat()
        {
            List<KhachHangDaDatBanDTO> ds = new List<KhachHangDaDatBanDTO>();
            string sql = @"SELECT kh.MaKhachHang, ba.MaBan, db.MaDatBan, kh.HoTen, kh.SoLuongKhach, kh.SoDienThoai,ba.SoBan, ba.Tang, db.ThoiGianDat, db.GhiChu
                   FROM DatBan db
                   JOIN KhachHang kh ON db.MaKhachHang = kh.MaKhachHang
                   JOIN BanAn ba ON db.MaBan = ba.MaBan
                   JOIN HoaDon hd ON db.MaKhachHang = hd.MaKhachHang
                   WHERE ba.TrangThai = N'Đã đặt' AND hd.TongTien <=0 ";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    KhachHangDaDatBanDTO item = new KhachHangDaDatBanDTO
                    {
                        MaKhachHang = Convert.ToInt32(dr["MaKhachHang"]),
                        MaBan = Convert.ToInt32(dr["MaBan"]),
                        MaDatBan = Convert.ToInt32(dr["MaDatBan"]),
                        HoTen = dr["HoTen"].ToString(),
                        SoLuongKhach = Convert.ToInt32(dr["SoLuongKhach"]),
                        SoDienThoai = dr["SoDienThoai"].ToString(),
                        SoBan = Convert.ToInt32(dr["SoBan"]),
                        Tang = Convert.ToInt32(dr["Tang"]),
                        ThoiGianDat = Convert.ToDateTime(dr["ThoiGianDat"]),
                        GhiChu = dr["GhiChu"].ToString()
                    };
                    ds.Add(item);
                }
                conn.Close();
            }

            return ds;
        }

        public bool CapNhatTrangThaiBan(BanAn ban)
        {
            string sql = "UPDATE BanAn SET TrangThai = @TrangThai WHERE MaBan = @MaBan";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);
                cmd.Parameters.AddWithValue("@MaBan", ban.MaBan);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows > 0;
            }
        }


        public bool CapNhatGhiChu(int maDatBan, string ghiChuMoi)
        {
            string sql = "UPDATE DatBan SET GhiChu = @GhiChu WHERE MaDatBan = @MaDatBan";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@GhiChu", ghiChuMoi);
                cmd.Parameters.AddWithValue("@MaDatBan", maDatBan);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows > 0;
            }
        }



        // đổi bàn cho khách
        public bool DoiBan(int maDatBan, int maBanMoi)
        {
            //int maBanCu = -1;

            //string sqlGetOld = "SELECT MaBan FROM DatBan WHERE MaDatBan = @MaDatBan";
            //using (SqlCommand cmd = new SqlCommand(sqlGetOld, conn))
            //{
            //    cmd.Parameters.AddWithValue("@MaDatBan", maDatBan);
            //    conn.Open();
            //    object result = cmd.ExecuteScalar();
            //    conn.Close();

            //    if (result == null) return false;
            //    maBanCu = Convert.ToInt32(result);
            //}

            string sqlUpdate = "UPDATE DatBan SET MaBan = @MaBanMoi WHERE MaDatBan = @MaDatBan";
            using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn))
            {
                cmd.Parameters.AddWithValue("@MaBanMoi", maBanMoi);
                cmd.Parameters.AddWithValue("@MaDatBan", maDatBan);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();
                if (rows <= 0) return false;
            }

            //string sqlBanCu = "UPDATE BanAn SET TrangThai = N'Trống' WHERE MaBan = @MaBanCu";
            //using (SqlCommand cmd = new SqlCommand(sqlBanCu, conn))
            //{
            //    cmd.Parameters.AddWithValue("@MaBanCu", maBanCu);
            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //    conn.Close();
            //}

            string sqlBanMoi = "UPDATE BanAn SET TrangThai = N'Đã đặt' WHERE MaBan = @MaBanMoi";
            using (SqlCommand cmd = new SqlCommand(sqlBanMoi, conn))
            {
                cmd.Parameters.AddWithValue("@MaBanMoi", maBanMoi);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return true;
        }




        public bool XoaDatBanVaHoaDon(int maDatBan)
        {
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                int maBan = -1;
                int maKhachHang = -1;

                string sqlInfo = "SELECT MaBan, MaKhachHang FROM DatBan WHERE MaDatBan = @MaDatBan";
                using (SqlCommand cmd = new SqlCommand(sqlInfo, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@MaDatBan", maDatBan);
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            maBan = Convert.ToInt32(rd["MaBan"]);
                            maKhachHang = Convert.ToInt32(rd["MaKhachHang"]);
                        }
                        else
                        {
                            tran.Rollback(); conn.Close(); return false;
                        }
                    }
                }

                List<int> danhSachHD = new List<int>();
                string sqlGetHD = "SELECT MaHoaDon FROM HoaDon WHERE MaKhachHang = @MaKhachHang";
                using (SqlCommand cmd = new SqlCommand(sqlGetHD, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            danhSachHD.Add(Convert.ToInt32(rd["MaHoaDon"]));
                        }
                    }
                }

                foreach (int maHD in danhSachHD)
                {
                    string sqlXoaCT = "DELETE FROM ChiTietHoaDon WHERE MaHoaDon = @MaHD";
                    using (SqlCommand cmd = new SqlCommand(sqlXoaCT, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", maHD);
                        cmd.ExecuteNonQuery();
                    }
                }

                string sqlXoaHD = "DELETE FROM HoaDon WHERE MaKhachHang = @MaKhachHang";
                using (SqlCommand cmd = new SqlCommand(sqlXoaHD, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                    cmd.ExecuteNonQuery();
                }

                string sqlXoaDB = "DELETE FROM DatBan WHERE MaDatBan = @MaDatBan";
                using (SqlCommand cmd = new SqlCommand(sqlXoaDB, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@MaDatBan", maDatBan);
                    cmd.ExecuteNonQuery();
                }

                string sqlUpdateBan = "UPDATE BanAn SET TrangThai = N'Trống' WHERE MaBan = @MaBan";
                using (SqlCommand cmd = new SqlCommand(sqlUpdateBan, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@MaBan", maBan);
                    cmd.ExecuteNonQuery();
                }

                string sqlXoaKH = "DELETE FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
                using (SqlCommand cmd = new SqlCommand(sqlXoaKH, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                conn.Close();
                return true;
            }
            catch
            {
                tran.Rollback();
                conn.Close();
                return false;
            }
        }


        public List<BanAn> LayBanAn_DaDatHoacDangDung()
        {
            List<BanAn> ds = new List<BanAn>();
            string sql = "SELECT * FROM BanAn WHERE TrangThai <> N'Trống'";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BanAn ban = new BanAn
                        {
                            MaBan = Convert.ToInt32(reader["MaBan"]),
                            SoBan = Convert.ToInt32(reader["SoBan"]),
                            Tang = Convert.ToInt32(reader["Tang"]),
                            SoChoNgoi = Convert.ToInt32(reader["SoChoNgoi"]),
                            TrangThai = reader["TrangThai"].ToString(),
                        };
                        ds.Add(ban);
                    }
                }
                conn.Close();
            }

            return ds;
        }

        public BanAn GetBanAnById(int maBan)
        {
            BanAn ban = null;
            string query = "SELECT * FROM BanAn WHERE MaBan = @MaBan";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ban = new BanAn
                    {
                        MaBan = Convert.ToInt32(reader["MaBan"]),
                        SoBan = Convert.ToInt32(reader["SoBan"]),
                        SoChoNgoi = Convert.ToInt32(reader["SoChoNgoi"]),
                        TrangThai = reader["TrangThai"].ToString(),
                        Tang = Convert.ToInt32(reader["Tang"])

                    };
                }

                conn.Close();
            }

            return ban;
        }

        public string GetTrangThaiBanAn(int maBan)
        {
            string trangthai = "";
            string query = "SELECT TrangThai FROM BanAn WHERE MaBan = @MaBan";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    trangthai = reader["TrangThai"].ToString();

                }

                conn.Close();
            }

            return trangthai;
        }


        public bool capNhatBan_ThanhTrong(int maban)
        {
            try
            {
                string query = "UPDATE BanAn SET TrangThai = N'Trống' WHERE MaBan = @MaBan";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBan", maban);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }

                return true;
            }
            catch 
            {
                return false;
            }

        }
        public bool capNhatBan_ThanhDaDat(int maban)
        {
            try
            {
                string query = "UPDATE BanAn SET TrangThai = N'Đã đặt' WHERE MaBan = @MaBan";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBan", maban);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool capNhatBan_ThanhDangDung(int maban)
        {
            try
            {
                string query = "UPDATE BanAn SET TrangThai = N'Đang dùng' WHERE MaBan = @MaBan";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBan", maban);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
