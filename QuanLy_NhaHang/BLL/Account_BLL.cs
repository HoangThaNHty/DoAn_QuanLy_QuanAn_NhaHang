using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using static BLL.Account_BLL;

namespace BLL
{
    public class Account_BLL
    {
        public Account dalAccount = new Account();


        public string BamSHA256(string matKhau)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(matKhau);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public NhanVien DangNhap(TaiKhoan tk)
        {
            NhanVien nhanVien;
            TaiKhoan acc = tk;
            acc.MatKhau = BamSHA256(acc.MatKhau);
            bool ketQua = dalAccount.KiemTraDangNhap(acc, out nhanVien);

            if (ketQua)
            {
                return nhanVien;
            }
            else
            {
                return null;
            }
        }
        public int ThemNhanVien(NhanVien nv)
        {
            return dalAccount.InsertNhanVien(nv); 
        }

        public bool ThemTaiKhoan(TaiKhoan tk)
        {
            TaiKhoan acc = tk;
            acc.MatKhau = BamSHA256(acc.MatKhau);
            return dalAccount.InserTaiKhoan(acc);
        }

        public bool KiemTraKhongTrung(string tendangnhap)
        {
            return dalAccount.KiemTraKhongTrungTenDangNhap(tendangnhap);
        }


        public List<NhanVien_TaiKhoan> Get_NhanVien_TaiKhoan()
        {
            return dalAccount.GetNhanVien_TaiKhoan();
        }

        public bool KiemTraNhanVienCoHoaDon(int manv)
        {
            return dalAccount.KiemTraNhanVienCoHoaDon(manv);
        }
        public bool Delete_NhanVienTaiKhoan(string tentk, int manv)
        {
            return dalAccount.Delete_NhanVien_TaiKhoan(tentk, manv);
        }


        public bool Sua_NhanVien(NhanVien nv)
        {
            return dalAccount.SuaNhanVien(nv);
        }

        public NhanVien GetNhanVienById(int maNV)
        {
            return dalAccount.GetNhanVienById(maNV);
        }


    }


}
