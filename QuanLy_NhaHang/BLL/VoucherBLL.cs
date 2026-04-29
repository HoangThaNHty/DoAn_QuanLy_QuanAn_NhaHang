using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using DAL;
using DTO;

namespace BLL
{
    public class VoucherBLL
    {
        public VoucherDAL vc = new VoucherDAL();
        public List<Voucher> LayDanhSach()
        {
            return vc.LayDanhSach();
        }

        public bool Them(Voucher km)
        {
            return vc.Them(km);
        }

        public bool Sua(Voucher km)
        {
            return vc.Sua(km);
        }

        public bool Xoa(int maKM)
        {
            return vc.Xoa(maKM);
        }
        public bool KiemTraTrungTen(string tenKM)
        {
            return vc.KiemTraTrungTen(tenKM);
        }
        public Voucher LayKhuyenMaiTheoTen(string tenKM)
        {
            return vc.LayKhuyenMaiTheoTen(tenKM);
        }

    }
}
