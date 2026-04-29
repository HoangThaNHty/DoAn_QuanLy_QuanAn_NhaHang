using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BanAnChuaThanhToan
    {
        public int MaHoaDon { get; set; }
        public int MaBan {get; set; }
        public int Tang { get; set;  }
        public int SoBan { get; set; }

        public string TenHienThi
        {
            get { return $"Bàn:{SoBan} - Tầng:{Tang}"; }
        }

    }
}
