using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class CTHD_BLL
    {
        ChiTietHoaDonDAL cthd = new ChiTietHoaDonDAL();
        public List<CTHD_HienThi> GetChiTietHoaDon(int mahd)
        {
            return cthd.GetChiTietHoaDon(mahd);
        }

        public bool ThemChiTietHoaDon(int maHoaDon, int maMon)
        {
            return cthd.ThemMonVaoHoaDon(maHoaDon, maMon);
        }

        public bool GiamSoLuongChiTiet(int maChiTiet)
        {
            return cthd.GiamSoLuongChiTiet(maChiTiet);
        }
        public bool TangSoLuongChiTiet(int maChiTiet, out string message)
        {
            return cthd.TangSoLuongChiTiet(maChiTiet, out message);
        }

    }
}
