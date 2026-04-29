using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class MonAn_BLL
    {
        public MonAnDAL monandal = new MonAnDAL();
        public bool Insert_MonAn(MonAn mon)
        {
            return monandal.Insert(mon);
        }

        public List<MonAn> Get_MonAn()
        {
            return monandal.GetAllMonAn();
        }

        public bool KiemTraMaMonAnTrongChiTietHoaDon(int mamon)
        {
            return monandal.KiemTraMaMonAnTrongChiTietHoaDon(mamon);
        }
        public bool Delete_MonAn(int mamon)
        {
            return monandal.DeleteMonAn(mamon);
        }
        public string LayDuongDanAnh(int mamon)
        {
            return monandal.LayDuongDanAnh(mamon);
        }

        public bool Update_MonAn(MonAn monan)
        {
            return monandal.UpdateMonAn(monan);
        }
        public List<MonAn> GetAllMonAnCon()
        {
            return monandal.GetAllMonAnCon();
        }
    }
}
