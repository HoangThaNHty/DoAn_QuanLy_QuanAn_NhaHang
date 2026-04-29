using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using DTO;

namespace BLL
{
    public class HoaDonBLL
    {
        private HoaDonDAL hoaDonDAL = new HoaDonDAL();

        //?ã s?a
        public List<HoaDon> GetAllHoaDon()
        {
            return hoaDonDAL.GetAllHoaDon();
        }

        //public DataTable GetHoaDonByBan(int maBan)
        //{
        //    return hoaDonDAL.GetHoaDonByBan(maBan);
        //}

        //?ã s?a 
        public bool Insert_HoaDon(HoaDon hd)
        {
            return hoaDonDAL.InsertHoaDon(hd);
        }

        //public bool CapNhatTongTien(int maHoaDon, decimal tongTien)
        //{
        //    return hoaDonDAL.CapNhatTongTien(maHoaDon, tongTien);
        //}

        //public DataTable GetChiTietHoaDon(int maHoaDon)
        //{
        //    return hoaDonDAL.GetChiTietHoaDon(maHoaDon);
        //}

        //public bool ThemMonVaoHoaDon(int maHoaDon, int maMon, int soLuong, decimal thanhTien)
        //{
        //    return hoaDonDAL.ThemMonVaoHoaDon(maHoaDon, maMon, soLuong, thanhTien);
        //}

        //public bool XoaMonKhoiHoaDon(int maChiTiet)
        //{
        //    return hoaDonDAL.XoaMonKhoiHoaDon(maChiTiet);
        //}

        //public bool ThanhToanHoaDon(int maHoaDon, decimal tongTien)
        //{
        //    return hoaDonDAL.ThanhToanHoaDon(maHoaDon, tongTien);
        //}

        //public DataTable GetHoaDonChuaThanhToan()
        //{
        //    return hoaDonDAL.GetHoaDonChuaThanhToan();
        //}

        //public DataTable ThongKeDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        //{
        //    return hoaDonDAL.ThongKeDoanhThuTheoNgay(tuNgay, denNgay);
        //}

        //public DataTable ThongKeMonAnBanChay(DateTime tuNgay, DateTime denNgay)
        //{
        //    return hoaDonDAL.ThongKeMonAnBanChay(tuNgay, denNgay);
        //}

        public List<DSHoaDonCanThanhToan> GetHoaDonChuaThanhToan()
        {
            return hoaDonDAL.GetHoaDonChuaThanhToan(); 
        }

        public decimal TinhTongTienHoaDon(int maHoaDon)
        {
            return hoaDonDAL.TinhTongTienHoaDon(maHoaDon);

        }

        public bool CapNhatHoaDon(int maHoaDon, decimal tongTien, int giamGia, decimal thanhtoan)
        {
            return hoaDonDAL.CapNhatHoaDon(maHoaDon, tongTien, giamGia, thanhtoan);
        }



}
}
