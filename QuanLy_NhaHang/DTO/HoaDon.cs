using System;

namespace DTO
{
    public class HoaDon
    {
        public int MaHoaDon { get; set; }
        public int MaBan { get; set; }
        public int MaNhanVien { get; set; }
        public int MaKhachHang { get; set; }

        public DateTime ThoiGianLap { get; set; }
        public decimal TongTien { get; set; }

        public int GiamGia { get; set; }
        public decimal ThanhToan { get;set; }
    }
}
