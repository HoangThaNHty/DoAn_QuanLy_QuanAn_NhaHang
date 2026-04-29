using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class KhachHangBLL
    {
        public KhachHangDAL khDAL = new KhachHangDAL();
        public int Insert_KhachHang(KhachHang kh)
        {
            return khDAL.InsertKhachHang(kh);
        }

        public bool CapNhatKhachHang(int makh, int soluongkhach, string sdt)
        {
            return khDAL.CapNhatKhachHang(makh, soluongkhach, sdt);
        }

        public string GetTenKhachHangTuHoaDon(int maHoaDon)
        {
            return khDAL.GetTenKhachHangTuHoaDon(maHoaDon);
        }


        public KhachHang GetKhachHangById(int maKH)
        {
            return khDAL.GetKhachHangById(maKH);
        }

    }
}
