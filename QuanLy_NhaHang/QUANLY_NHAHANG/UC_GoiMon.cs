using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using CrystalDecisions.CrystalReports.Engine;
using DTO;


namespace QUANLY_NHAHANG
{
    public partial class UC_GoiMon : UserControl
    {
        public UC_GoiMon()
        {
            InitializeComponent();
            this.Load += GoiMon_Load;
            cbo_Ban.SelectedIndexChanged += Cbo_Ban_SelectedIndexChanged;
            dgv_MonAn.SelectionChanged += Dgv_MonAn_SelectionChanged;
            btn_ThemMon.Click += Btn_ThemMon_Click;
            dgv_CTHD.CellClick += Dgv_CTHD_CellClick;
            btn_HuyThemMon.Click += Btn_HuyThemMon_Click;
            cbo_LoaiMonAn.SelectedValueChanged += Cbo_LoaiMonAn_SelectedValueChanged;

        }

        private void Btn_HuyThemMon_Click(object sender, EventArgs e)
        {
            dgv_MonAn.ClearSelection();
        }

        private void Cbo_LoaiMonAn_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cbo_LoaiMonAn.SelectedIndex == 0)
            {
                MonAn_BLL monanbll = new MonAn_BLL();
                List<MonAn> ds = monanbll.Get_MonAn().Where(x => x.SoLuong > 0).ToList();

                var list = ds.Select(mon => new
                {
                    mon.MaMon,
                    mon.TenMon,
                    mon.MoTa,
                    mon.DonGia,
                    HinhAnh = File.Exists(mon.HinhAnh) ? Image.FromFile(mon.HinhAnh) : null,
                    mon.LoaiMon,
                    mon.SoLuong
                }).ToList();

                dgv_MonAn.DataSource = list;
            }    
            else
            {
                var loai = cbo_LoaiMonAn.SelectedItem.ToString();
                MonAn_BLL monanbll = new MonAn_BLL();
                List<MonAn> ds = monanbll.Get_MonAn().Where(x => (x.SoLuong > 0 && x.LoaiMon == loai)).ToList();

                var list = ds.Select(mon => new
                {
                    mon.MaMon,
                    mon.TenMon,
                    mon.MoTa,
                    mon.DonGia,
                    HinhAnh = File.Exists(mon.HinhAnh) ? Image.FromFile(mon.HinhAnh) : null,
                    mon.LoaiMon,
                    mon.SoLuong
                }).ToList();

                dgv_MonAn.DataSource = list;
            }    
        }

        private void Dgv_CTHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgv_CTHD.Columns[e.ColumnIndex].Name == "btn_TangSL")
            {
                CTHD_BLL cthd = new CTHD_BLL();

                int maChiTiet = Convert.ToInt32(dgv_CTHD.Rows[e.RowIndex].Cells["MaChiTiet"].Value);
                string message;
                bool ok = cthd.TangSoLuongChiTiet(maChiTiet, out message);
                int madh = int.Parse(cbo_Ban.SelectedValue.ToString());


                if (ok)
                {
                    LoadDSMon();
                    LoadChiTietHoaDon(madh);
                }
                else
                {
                    MessageBox.Show(string.IsNullOrEmpty(message) ? "Không thể tăng số lượng" : message);
                }
            }
            if (e.RowIndex >= 0 && dgv_CTHD.Columns[e.ColumnIndex].Name == "btn_GiamSL")
            {
                CTHD_BLL cthd = new CTHD_BLL();

                int maChiTiet = Convert.ToInt32(dgv_CTHD.Rows[e.RowIndex].Cells["MaChiTiet"].Value);
                bool ok = cthd.GiamSoLuongChiTiet(maChiTiet);
                int madh = int.Parse(cbo_Ban.SelectedValue.ToString());


                if (ok)
                {
                    LoadDSMon();
                    LoadChiTietHoaDon(madh);
                }
                else
                {
                    MessageBox.Show("Không thể giảm số lượng", "Lỗi");
                }
            }
        }

        private void Btn_ThemMon_Click(object sender, EventArgs e)
        {
            if(dgv_MonAn.SelectedRows.Count <=0 )
            {
                MessageBox.Show("Vui lòng chọn món ăn");
                return;
            }    
            string mamon = "";
            if (dgv_MonAn.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgv_MonAn.SelectedRows[0];

                mamon = row.Cells["MaMon"].Value?.ToString();         
            }
            ChiTietHoaDon cthd = new ChiTietHoaDon();

            int madh = int.Parse(cbo_Ban.SelectedValue.ToString());


            int mon = int.Parse(mamon.ToString());

            CTHD_BLL ct = new CTHD_BLL();
            bool result = ct.ThemChiTietHoaDon(madh, mon);
            if(result)
            {
                MessageBox.Show("Thêm món thành công");


                LoadChiTietHoaDon(madh);
                LoadDSMon();
            }    
            else
            {
                MessageBox.Show("Thêm món thất bại");

            }

        }

        private void Dgv_MonAn_SelectionChanged(object sender, EventArgs e)
        {
            btn_HuyThemMon.Enabled = true;
            btn_ThemMon.Enabled = true;

            
        }

        private void Cbo_Ban_SelectedIndexChanged(object sender, EventArgs e)
        {
            int madh = int.Parse(cbo_Ban.SelectedValue.ToString());
            KhachHangBLL kh = new KhachHangBLL();
            string tenkh = kh.GetTenKhachHangTuHoaDon(madh);
            txt_TenKhachHang.Text = tenkh;
            LoadChiTietHoaDon(madh);
        }

        public void LoadChiTietHoaDon(int mahd)
        {
            CTHD_BLL ct = new CTHD_BLL();

            dgv_CTHD.DataSource = ct.GetChiTietHoaDon(mahd);
        }

        public void LoadDSMon()
        {

            MonAn_BLL monanbll = new MonAn_BLL();
            List<MonAn> ds = monanbll.Get_MonAn().Where(x => x.SoLuong > 0).ToList();

            var list = ds.Select(mon => new
            {
                mon.MaMon,
                mon.TenMon,
                mon.MoTa,
                mon.DonGia,
                HinhAnh = File.Exists(mon.HinhAnh) ? Image.FromFile(mon.HinhAnh) : null,
                mon.LoaiMon,
                mon.SoLuong
            }).ToList();

            dgv_MonAn.DataSource = list;

        }
       

        private void GoiMon_Load(object sender, EventArgs e)
        {
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


            cbo_LoaiMonAn.Items.Add("----Tất cả----");
            cbo_LoaiMonAn.Items.Add("Gà");
            cbo_LoaiMonAn.Items.Add("Cá");
            cbo_LoaiMonAn.Items.Add("Bò");
            cbo_LoaiMonAn.Items.Add("Rau");
            cbo_LoaiMonAn.Items.Add("Đồ chay");
            cbo_LoaiMonAn.Items.Add("Thức uống");
            cbo_LoaiMonAn.SelectedIndex = 0;


            GoiMonBLL banan = new GoiMonBLL();
            cbo_Ban.DisplayMember = "TenHienThi";
            cbo_Ban.ValueMember = "MaHoaDon";
            cbo_Ban.DataSource = banan.LayHoaDonBanTrong_TongTien0();

            LoadDSMon();
            dgv_MonAn.ClearSelection();

            btn_HuyThemMon.Enabled = false;
            btn_HuyThemMon.Enabled = false;


           

        }


        public void Reload()
        {
            LoadDSMon();

            GoiMonBLL banan = new GoiMonBLL();
            cbo_Ban.DisplayMember = "TenHienThi";
            cbo_Ban.ValueMember = "MaHoaDon";
            cbo_Ban.DataSource = banan.LayHoaDonBanTrong_TongTien0();

            if (cbo_Ban.Items.Count > 0)
            {
                int mahd = int.Parse(cbo_Ban.SelectedValue.ToString());
                LoadChiTietHoaDon(mahd);
            }
            else
            {
                dgv_CTHD.DataSource = null;
                txt_TenKhachHang.Clear();
            }

            // Clear selection ở danh sách món
            dgv_MonAn.ClearSelection();

            // Reset nút
            btn_ThemMon.Enabled = false;
            btn_HuyThemMon.Enabled = false;
        }



    }
}
