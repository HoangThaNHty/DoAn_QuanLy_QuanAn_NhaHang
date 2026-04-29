using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using CrystalDecisions.CrystalReports.Engine;
using DTO;

namespace QUANLY_NHAHANG
{
    public partial class UC_DanhSachHoaDon : UserControl
    {
        public UC_DanhSachHoaDon()
        {
            InitializeComponent();
            this.Load += UC_DanhSachHoaDon_Load;
            dgv_HoaDon.SelectionChanged += Dgv_HoaDon_SelectionChanged;
        }

        private void Dgv_HoaDon_SelectionChanged(object sender, EventArgs e)
        {
            int mahd = 0;
            int maban = 0;
            int mannv = 0;
            DateTime ngaytao = DateTime.Now;
            decimal tongtien = 0;
            int makh = 0;
            int phantram = 0;
            decimal thanhtien = 0;

            if (dgv_HoaDon.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgv_HoaDon.SelectedRows[0];

                mahd = Convert.ToInt32(row.Cells["MaHoaDon"].Value);
                maban = Convert.ToInt32(row.Cells["MaBan"].Value);
                mannv = Convert.ToInt32(row.Cells["MaNhanVien"].Value);
                ngaytao = Convert.ToDateTime(row.Cells["ThoiGianlap"].Value);
                tongtien = Convert.ToDecimal(row.Cells["TongTien"].Value);
                makh = Convert.ToInt32(row.Cells["MaKhachHang"].Value);
                phantram = Convert.ToInt32(row.Cells["GiamGia"].Value);
                thanhtien = Convert.ToDecimal(row.Cells["ThanhToan"].Value);

                // Lấy chi tiết hóa đơn
                CTHD_BLL ct = new CTHD_BLL();
                List<CTHD_HienThi> dsCTHD = ct.GetChiTietHoaDon(mahd);
                DataTable dt = ConvertToDataTable(dsCTHD);

                // Tạo report
                ReportDocument rpt = new ReportDocument();
                string path = Path.Combine(Application.StartupPath, "Report_HoaDon.rpt");
                rpt.Load(path);
                rpt.SetDataSource(dt);

                //lấy tông tin bàn
                BanAnBLL ba = new BanAnBLL();
                BanAn BA = ba.GetBanAnById(maban);

                KhachHangBLL kh = new KhachHangBLL();
                KhachHang KH = kh.GetKhachHangById(makh);

                Account_BLL ac = new Account_BLL();
                NhanVien NV = ac.GetNhanVienById(mannv);

                // Gán thông tin vào biến trong report
                rpt.SetParameterValue("MaHoaDon", mahd.ToString());
                rpt.SetParameterValue("ThoiGianLap", ngaytao);
                rpt.SetParameterValue("SoBan", BA.SoBan);
                rpt.SetParameterValue("Tang", BA.Tang);
                rpt.SetParameterValue("HoTenNV", NV.HoTen);
                rpt.SetParameterValue("MaKhachHang", makh);
                rpt.SetParameterValue("HoTenKH", KH.HoTen);
                rpt.SetParameterValue("TongTien", tongtien);
                rpt.SetParameterValue("GiamGia", phantram);
                rpt.SetParameterValue("TongThanhToan", thanhtien);


                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();



            }
        }






        //private void Dgv_HoaDon_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (dgv_HoaDon.SelectedRows.Count == 0) return;

        //    DataGridViewRow row = dgv_HoaDon.SelectedRows[0];
        //    if (row.Cells["MaHoaDon"].Value == null) return;

        //    int mahd = Convert.ToInt32(row.Cells["MaHoaDon"].Value);
        //    int maban = Convert.ToInt32(row.Cells["MaBan"].Value);
        //    int mannv = Convert.ToInt32(row.Cells["MaNhanVien"].Value);
        //    DateTime ngaytao = Convert.ToDateTime(row.Cells["ThoiGianlap"].Value);
        //    decimal tongtien = Convert.ToDecimal(row.Cells["TongTien"].Value);
        //    int makh = Convert.ToInt32(row.Cells["MaKhachHang"].Value);
        //    int phantram = Convert.ToInt32(row.Cells["GiamGia"].Value);
        //    decimal thanhtien = Convert.ToDecimal(row.Cells["ThanhToan"].Value);

        //    // Lấy chi tiết hóa đơn
        //    CTHD_BLL ct = new CTHD_BLL();
        //    List<CTHD_HienThi> dsCTHD = ct.GetChiTietHoaDon(mahd);
        //    DataTable dt = ConvertToDataTable(dsCTHD);

        //    // Load report
        //    ReportDocument rpt = new ReportDocument();
        //    string path = Path.Combine(Application.StartupPath, "Report_HoaDon.rpt");
        //    rpt.Load(path);
        //    rpt.SetDataSource(dt);

        //    // thông tin liên quan
        //    BanAnBLL ba = new BanAnBLL();
        //    BanAn BA = ba.GetBanAnById(maban);

        //    KhachHangBLL kh = new KhachHangBLL();
        //    KhachHang KH = kh.GetKhachHangById(makh);

        //    Account_BLL ac = new Account_BLL();
        //    NhanVien NV = ac.GetNhanVienById(mannv);

        //    // Set parameter
        //    rpt.SetParameterValue("MaHoaDon", mahd);
        //    rpt.SetParameterValue("ThoiGianLap", ngaytao);

        //    rpt.SetParameterValue("SoBan", BA?.SoBan ?? 0);
        //    rpt.SetParameterValue("Tang", BA?.Tang ?? 0);

        //    rpt.SetParameterValue("HoTenNV", NV?.HoTen ?? "Không rõ");
        //    rpt.SetParameterValue("HoTenKH", KH?.HoTen ?? "Khách lẻ");

        //    rpt.SetParameterValue("TongTien", tongtien);
        //    rpt.SetParameterValue("GiamGia", phantram);
        //    rpt.SetParameterValue("TongThanhToan", thanhtien);

        //    crystalReportViewer1.ReportSource = rpt;
        //    crystalReportViewer1.Refresh();
        //}





        public static DataTable ConvertToDataTable(List<CTHD_HienThi> list)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("MaChiTiet", typeof(int));
            dt.Columns.Add("MaHoaDon", typeof(int));
            dt.Columns.Add("MaMon", typeof(int));
            dt.Columns.Add("TenMon", typeof(string));
            dt.Columns.Add("DonGia", typeof(decimal));
            dt.Columns.Add("SoLuong", typeof(int));
            dt.Columns.Add("ThanhTien", typeof(decimal));

            foreach (var item in list)
            {
                dt.Rows.Add(item.MaChiTiet,  item.MaHoaDon, item.MaMon, item.TenMon, item.DonGia,
                            item.SoLuong, item.ThanhTien);
            }

            return dt;

            
        }

        private void LoadDanhSachHoaDon()
        {
            HoaDonBLL hd = new HoaDonBLL();
            dgv_HoaDon.DataSource = hd.GetAllHoaDon();
        }
        private void UC_DanhSachHoaDon_Load(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();

            //ReportDocument rpt = new ReportDocument();
            //string path = Path.Combine(Application.StartupPath, "ReportCTHDTemp.rpt");
            //rpt.Load(path);


            //int maHoaDon = 1111111;
            //string maHoaDonStr = maHoaDon.ToString();
            //rpt.SetParameterValue("MaHoaDon", maHoaDonStr);
            //rpt.SetParameterValue("ThoiGianLap", DateTime.Now);
            //rpt.SetParameterValue("SoBan", 2);
            //rpt.SetParameterValue("Tang", 1);
            //rpt.SetParameterValue("HoTenKH", "Trọng Thanh");
            //rpt.SetParameterValue("TongTien", 500000);



            //crystalReportViewer1.ReportSource = rpt;
            //crystalReportViewer1.Refresh();
        }
        


        public void Reload()
        {
            LoadDanhSachHoaDon(); 
            crystalReportViewer1.ReportSource = null; 
            crystalReportViewer1.Refresh(); 
        }

    }
}
