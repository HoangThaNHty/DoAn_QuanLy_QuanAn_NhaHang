using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NhanVien
    {
        public int MaNhanVien { get; set; }
        public string HoTen { get; set; }
	    public string GioiTinh { get; set; }
        public string ChucVu { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime  NgayVaoLam { get; set; }
    }
}
