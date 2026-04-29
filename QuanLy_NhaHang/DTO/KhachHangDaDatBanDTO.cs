using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class KhachHangDaDatBanDTO
    {
        public int MaKhachHang { get; set; }
        public int MaBan { get; set; }
        public int MaDatBan { get; set; }
        public string HoTen { get; set; }
        public int SoLuongKhach { get; set; }
        public string SoDienThoai { get; set; }
        public int SoBan { get; set; }
        public int Tang { get; set; }
        public DateTime ThoiGianDat { get; set; }
        public string GhiChu { get; set; }

    }
}
