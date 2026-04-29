using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class ThongKe_BLL
    {
        ThongKeDAL tk = new ThongKeDAL();
        public DataTable ThongKeDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return tk.LayDoanhThuTheoNgay(tuNgay, denNgay);
        }

        public DataTable ThongKeTopMonAn(DateTime tuNgay, DateTime denNgay)
        {
            return tk.LayTopMonAnBanChay(tuNgay, denNgay);
        }

        public string ThongKeMonAnBanChayNhat(DateTime tuNgay, DateTime denNgay)
        {
            return tk.LayTenMonAnBanChayNhat(tuNgay, denNgay);
        }
        public DataTable LayDoanhThuTheoKhoangThang(DateTime tuNgay, DateTime denNgay)
        {
            return tk.LayDoanhThuTheoKhoangThang(tuNgay, denNgay);
        }
        public DataTable ThongKeTrangThaiBanAn()
        {
            return tk.LayTrangThaiBanAn();
        }

        public DataTable LayHoaDonTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return tk.LayHoaDonTheoNgay(tuNgay, denNgay);
        }

    }
}
