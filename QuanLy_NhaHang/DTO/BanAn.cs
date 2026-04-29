using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BanAn
    {
        public int MaBan { get; set; }
        public int SoBan { get; set; }
        public int SoChoNgoi { get; set; }
        public int Tang { get; set; }
        public string TrangThai { get; set; }

        public string TenHienThi
        {
            get { return $"Bàn:{SoBan} - Tầng:{Tang} - {SoChoNgoi} chỗ "; }
        }

    }

}
