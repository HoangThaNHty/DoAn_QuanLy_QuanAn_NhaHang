using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class BanAnBLL
    {
        BanAnDAL dal = new BanAnDAL();

        public List<BanAn> GetBanAn()
        {
            return dal.GetBanAn();
        }

        public List<BanAn> GetBanAn_TheoTang(int tang)
        {
            return dal.GetBanAnTheoTang(tang);
        }

        public bool ThemBan(BanAn ban)
        {
            return dal.InsertBanAn(ban);
        }

        public bool SuaBan(BanAn ban)
        {
            return dal.UpdateBanAn(ban);
        }

        public bool XoaBan(int maBan)
        {
            return dal.DeleteBanAn(maBan);
        }

        public List<BanAn> LayBanAn_Trong()
        {
            return dal.LayBanAnTrong();
        }


        /// trang đặt bàn
        
        public bool Insert_DatBan(DatBan db)
        {
            return dal.InsertDatBan(db);
        }

        public List<KhachHangDaDatBanDTO> LayDanhSach_KhachHangDaDat()
        {
            return dal.LayDanhSachKhachHangDaDat();
        }

        public bool CapNhatTrangThaiBan(BanAn ban)
        {
            return dal.CapNhatTrangThaiBan(ban);
        }

        public bool CapNhatGhiChu(int maDatBan, string ghiChuMoi)
        {
            return dal.CapNhatGhiChu(maDatBan, ghiChuMoi);
        }

        //
        public bool DoiBan(int maDatBan, int maBanMoi)
        {
            return dal.DoiBan(maDatBan, maBanMoi);
        }

        public bool XoaDatBanVaHoaDon(int maDatBan)
        {
            return dal.XoaDatBanVaHoaDon(maDatBan);
        }

        //public List<BanAn> LayBanAn_DaDatHoacDangDung()
        //{
        //    return dal.LayBanAn_DaDatHoacDangDung();
        //}

        // lây bàn theo mã bàn
        public BanAn GetBanAnById(int maBan)
        {
            return dal.GetBanAnById(maBan);
        }
        //public List<BanAn> GetBanAnTrong()
        //{
        //    return dal.GetBanAnTrong();
        //}
        public string GetTrangThaiBanAn(int maBan)
        {
            return dal.GetTrangThaiBanAn(maBan);
        }


        public bool CapNhatBan_ThanhTrong(int maBan)
        {
            return dal.capNhatBan_ThanhTrong(maBan);
        }
        public bool capNhatBan_ThanhDaDat(int maBan)
        {
            return dal.capNhatBan_ThanhDaDat(maBan);
        }
        public bool capNhatBan_ThanhDangDung(int maBan)
        {
            return dal.capNhatBan_ThanhDangDung(maBan);
        }


    }
}
