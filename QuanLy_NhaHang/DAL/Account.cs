using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class Account : DBConnect
    {
        //đăng nhập
        public bool KiemTraDangNhap(TaiKhoan nd, out NhanVien nhanVien)
        {
            nhanVien = null;

            try
            {
                conn.Open();

                string query = "SELECT nv.MaNhanVien, nv.HoTen, nv.GioiTinh, nv.ChucVu, nv.SoDienThoai, nv.NgayVaoLam " +
               "FROM TaiKhoan tk " +
               "JOIN NhanVien nv ON tk.MaNhanVien = nv.MaNhanVien " +
               "WHERE tk.TenDangNhap COLLATE SQL_Latin1_General_CP1_CS_AS = @tk " +
               "AND tk.MatKhau COLLATE SQL_Latin1_General_CP1_CS_AS = @mk";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tk", nd.TenDangNhap);
                cmd.Parameters.AddWithValue("@mk", nd.MatKhau);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    nhanVien = new NhanVien
                    {
                        MaNhanVien = int.Parse(reader["MaNhanVien"].ToString()),
                        HoTen = reader["HoTen"].ToString(),
                        GioiTinh = reader["GioiTinh"].ToString(),
                        ChucVu = reader["ChucVu"].ToString(),
                        SoDienThoai = reader["SoDienThoai"].ToString(),
                        NgayVaoLam = DateTime.Parse(reader["NgayVaoLam"].ToString())
                    };
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        //thêm nhân viên 
        public int InsertNhanVien(NhanVien nv)
        {
            int maNV = 0;
            conn.Open();

            string query = "INSERT INTO NhanVien (HoTen, GioiTinh, ChucVu, SoDienThoai, NgayVaoLam) " +
                               "OUTPUT INSERTED.MaNhanVien " +
                               "VALUES (@HoTen, @GioiTinh, @ChucVu, @SoDienThoai, @NgayVaoLam)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
            cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
            cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
            cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);
            cmd.Parameters.AddWithValue("@NgayVaoLam", nv.NgayVaoLam);

            maNV = (int)cmd.ExecuteScalar();
            conn.Close();

            return maNV;

        }



        public bool KiemTraKhongTrungTenDangNhap(string tendangnhap)
        {
            try
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap COLLATE SQL_Latin1_General_CP1_CS_AS = @TenDangNhap";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenDangNhap", tendangnhap);

                int result = (int)cmd.ExecuteScalar();
                conn.Close();

                if (result == 0)
                {
                    return true;
                }

                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }
        //thêm  tài khoản
        public bool InserTaiKhoan(TaiKhoan tk)
        {
            try
            {
                conn.Open();

                string query = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, MaNhanVien) VALUES (@TenDangNhap, @MatKhau, @MaNhanVien)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenDangNhap", tk.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", tk.MatKhau);
                cmd.Parameters.AddWithValue("@MaNhanVien", tk.MaNhanVien);

                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public List<NhanVien_TaiKhoan> GetNhanVien_TaiKhoan()
        {
            List<NhanVien_TaiKhoan> ds = new List<NhanVien_TaiKhoan>();
            string query = @"SELECT nv.MaNhanVien, nv.HoTen, nv.GioiTinh, nv.ChucVu,nv.SoDienThoai,nv.NgayVaoLam, tk.TenDangNhap
                   FROM NhanVien nv
                   JOIN TaiKhoan tk ON nv.MaNhanVien = tk.MaNhanVien";


            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) 
                    {
                        conn.Open();
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NhanVien_TaiKhoan nv = new NhanVien_TaiKhoan();
                            nv.MaNhanVien = Convert.ToInt32(reader["MaNhanVien"]);
                            nv.HoTen = reader["HoTen"].ToString();
                            nv.GioiTinh = reader["GioiTinh"].ToString();
                            nv.ChucVu = reader["ChucVu"].ToString();
                            nv.SoDienThoai = reader["SoDienThoai"].ToString();
                            nv.NgayVaoLam = DateTime.Parse(reader["NgayVaoLam"].ToString());
                            nv.TenDangNhap = reader["TenDangNhap"].ToString();
                            ds.Add(nv);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi truy xuất dữ liệu nhân viên: " + ex.Message, ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            return ds;
        }
        public bool KiemTraNhanVienCoHoaDon(int manv)
        {
            string query = "select count(*) from HoaDon where MaNhanVien = @MaNhanVien";

            using(SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("MaNhanVien", manv);
                conn.Open();

                var result = cmd.ExecuteScalar();

                if(int.Parse(result.ToString()) <= 0)
                {
                    return true;
                }    
                return false;
            }    
        }
        public bool Delete_NhanVien_TaiKhoan(string tenkh, int manv)
        {
            try
            {
                conn.Open();
                string query1 = @"DELETE TaiKhoan WHERE TenDangNhap = @TenDangNhap ";
                SqlCommand cmd1 = new SqlCommand(query1, conn);
                cmd1.Parameters.AddWithValue("@TenDangNhap", tenkh);

                string query2 = @"DELETE NhanVien WHERE MaNhanVien = @MaNhanVien ";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@MaNhanVien", manv);

                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //sửa nhân viên
        public bool SuaNhanVien(NhanVien nv)
        {
            string query = @"
                UPDATE NhanVien
                SET HoTen = @HoTen,
                    GioiTinh = @GioiTinh,
                    ChucVu = @ChucVu,
                    SoDienThoai = @SoDienThoai
                WHERE MaNhanVien = @MaNhanVien";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
                    cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
                    cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                    cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);
                    cmd.Parameters.AddWithValue("@MaNhanVien", nv.MaNhanVien);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public NhanVien GetNhanVienById(int maNV)
        {
            NhanVien nv = null;
            string query = "SELECT * FROM NhanVien WHERE MaNhanVien = @MaNV";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    nv = new NhanVien
                    {
                        MaNhanVien = Convert.ToInt32(reader["MaNhanVien"]),
                        HoTen = reader["HoTen"].ToString(),
                        SoDienThoai = reader["SoDienThoai"].ToString(),
                        GioiTinh  = reader["GioiTinh"].ToString(),
                        ChucVu = reader["ChucVu"].ToString(),
                        NgayVaoLam = Convert.ToDateTime(reader["NgayVaoLam"].ToString()),
                        
                    };
                }

                conn.Close();
            }

            return nv;
        }



    }

}

