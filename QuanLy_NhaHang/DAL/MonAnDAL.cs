using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using DTO;

namespace DAL
{
    public class MonAnDAL : DBConnect
    {
        public bool Insert(MonAn mon)
        {
            
            try
            {
                conn.Open();
                string query = @"INSERT INTO MonAn (TenMon,MoTa,DonGia, HinhAnh, LoaiMon,SoLuong ) 
                             VALUES (@TenMon, @MoTa, @DonGia, @HinhAnh, @LoaiMon, @SoLuong)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenMon", mon.TenMon);
                cmd.Parameters.AddWithValue("@Mota", mon.MoTa);
                cmd.Parameters.AddWithValue("@DonGia", mon.DonGia);
                cmd.Parameters.AddWithValue("@HinhAnh", mon.HinhAnh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LoaiMon", mon.LoaiMon);
                cmd.Parameters.AddWithValue("@SoLuong", mon.SoLuong);

                int result = cmd.ExecuteNonQuery();
                conn.Close();
                return result > 0;
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
           
        }

        public List<MonAn> GetAllMonAn()
        {
            List<MonAn> dsMonAn = new List<MonAn>();

            string sql = "SELECT MaMon, TenMon,MoTa, DonGia, HinhAnh, LoaiMon, SoLuong FROM MonAn";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MonAn ma = new MonAn
                    {
                        MaMon = (int)dr["MaMon"],
                        TenMon = dr["TenMon"].ToString(),
                        MoTa = dr["MoTa"].ToString(),
                        DonGia = decimal.Parse(dr["DonGia"].ToString()),
                        HinhAnh = dr["HinhAnh"].ToString(),
                        LoaiMon = dr["LoaiMon"].ToString(),
                        SoLuong = int.Parse(dr["SoLuong"].ToString())
                    };
                    dsMonAn.Add(ma);
                }

                conn.Close();
            }

            return dsMonAn;
        }

        public bool KiemTraMaMonAnTrongChiTietHoaDon(int mamon)
        {
            string query = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaMon = @MaMon";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaMon", mamon);
                conn.Open();
                var count = cmd.ExecuteScalar();

                if(int.Parse(count.ToString()) <=0)
                {
                    return false;
                }    
                else
                {
                    return true;
                }    
                
            }    
        }


        public bool DeleteMonAn(int maMon)
        {
            string query = "DELETE FROM MonAn WHERE MaMon = @MaMon";
            //kiểm tra nếu món ăn tồn tại trong chi tiết hóa đơn thì không xóa
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaMon", maMon);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }


        public string LayDuongDanAnh(int mamon)
        {

            string hinh = "";
            string query = "select HinhAnh from MonAn where MaMon = @MaMon";
            using (SqlCommand cmd  = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaMon", mamon);
                conn.Open();
                var hinhanh = cmd.ExecuteScalar();


                if(hinhanh != null)
                {
                    hinh = hinhanh.ToString();
                }    


            }    
            return hinh;
        }

        public bool UpdateMonAn(MonAn mon)
        {
            string query = @"UPDATE MonAn
                     SET TenMon = @TenMon,
                         MoTa = @MoTa,
                         DonGia = @DonGia,
                         LoaiMon = @LoaiMon,
                         SoLuong = @SoLuong,
                         HinhAnh = @HinhAnh
                     WHERE MaMon = @MaMon";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaMon", mon.MaMon);
                cmd.Parameters.AddWithValue("@TenMon", mon.TenMon);
                cmd.Parameters.AddWithValue("@MoTa", mon.MoTa);
                cmd.Parameters.AddWithValue("@DonGia", mon.DonGia);
                cmd.Parameters.AddWithValue("@LoaiMon", mon.LoaiMon);
                cmd.Parameters.AddWithValue("@SoLuong", mon.SoLuong);
                cmd.Parameters.AddWithValue("@HinhAnh", mon.HinhAnh);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
        public List<MonAn> GetAllMonAnCon()
        {
            List<MonAn> dsMonAn = new List<MonAn>();

            string sql = "SELECT MaMon, TenMon,MoTa, DonGia, HinhAnh, LoaiMon, SoLuong FROM MonAn WHERE SoLuong > 0";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MonAn ma = new MonAn
                    {
                        MaMon = (int)dr["MaMon"],
                        TenMon = dr["TenMon"].ToString(),
                        MoTa = dr["MoTa"].ToString(),
                        DonGia = decimal.Parse(dr["DonGia"].ToString()),
                        HinhAnh = dr["HinhAnh"].ToString(),
                        LoaiMon = dr["LoaiMon"].ToString(),
                        SoLuong = int.Parse(dr["SoLuong"].ToString())
                    };
                    dsMonAn.Add(ma);
                }

                conn.Close();
            }

            return dsMonAn;
        }
    }
}
