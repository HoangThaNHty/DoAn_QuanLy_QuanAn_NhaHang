using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DTO;


namespace QUANLY_NHAHANG
{
    public partial class UC_DatBan : UserControl
    {
        public NhanVien NhanVienDangNhap { get; set; }

        public void RefreshData()
        {
            LoadBanAnTrong();

            LoadDSKhachHangDaDat();

            Btn_HuyDatBan_Click(null, null);
        }
        public UC_DatBan()
        {
            InitializeComponent();
            this.Load += UC_DatBan_Load;
            btn_ThemDatBan.Click += Btn_ThemDatBan_Click;
            btn_HuyDatBan.Click += Btn_HuyDatBan_Click;
            btn_LuuDatBan.Click += Btn_LuuDatBan_Click;
            dgvKhachHang.SelectionChanged += DgvKhachHang_SelectionChanged;
            cbo_BanAn.SelectedIndexChanged += Cbo_BanAn_SelectedIndexChanged;
            btn_SuaDatBan.Click += Btn_SuaDatBan_Click;
            btn_DoiBan.Click += Btn_DoiBan_Click;
            btn_XoaDatBan.Click += Btn_XoaDatBan_Click;
            txt_SoDienThoai.KeyPress += Txt_SoDienThoai_KeyPress;
            txt_SoLuongKhach.KeyPress += Txt_SoLuongKhach_KeyPress;
        }

        private void Txt_SoLuongKhach_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void Txt_SoDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void Btn_XoaDatBan_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa đặt bàn và hóa đơn liên quan?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            int maDatBan = Convert.ToInt32(dgvKhachHang.SelectedRows[0].Cells["MaDatBan"].Value);

            BanAnBLL banan = new BanAnBLL();
            bool ok = banan.XoaDatBanVaHoaDon(maDatBan);

            if (ok)
            {
                MessageBox.Show("Xóa đặt bàn và hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDSKhachHangDaDat();
                Btn_HuyDatBan_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Xóa thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Btn_DoiBan_Click(object sender, EventArgs e)
        {
            btn_ThemDatBan.Enabled = false;
            btn_XoaDatBan.Enabled = false;
            btn_SuaDatBan.Enabled = false;
            btn_DoiBan.Enabled = true;

            btn_LuuDatBan.Enabled = true;
            btn_HuyDatBan.Enabled = true;

            txt_GhiChu.Enabled = false;
            txt_HoTenKhach.Enabled = false;
            txt_SoDienThoai.Enabled = false;
            txt_SoLuongKhach.Enabled = false;
            cbo_BanAn.Enabled = true;

            BanAnBLL banan = new BanAnBLL();
            List<BanAn> listBanAn = banan.LayBanAn_Trong().OrderBy(x => x.Tang).ToList();
            cbo_BanAn.DisplayMember = "TenHienThi";
            cbo_BanAn.ValueMember = "MaBan";
            cbo_BanAn.DataSource = listBanAn;
        }

        private void Cbo_BanAn_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Btn_SuaDatBan_Click(object sender, EventArgs e)
        {
            btn_ThemDatBan.Enabled = false;
            btn_XoaDatBan.Enabled = false;
            //btn_SuaDatBan.Enabled = true;
            btn_DoiBan.Enabled = false;

            btn_LuuDatBan.Enabled = true;
            btn_HuyDatBan.Enabled = true;

            txt_GhiChu.Enabled = true;
            txt_HoTenKhach.Enabled = false;
            txt_SoDienThoai.Enabled = true;
            txt_SoLuongKhach.Enabled = true;
            cbo_BanAn.Enabled = false;

            


            //LoadBanAnTrong();
        }


        

        private void DgvKhachHang_SelectionChanged(object sender, EventArgs e)
        {


            btn_SuaDatBan.Enabled = true;
            btn_ThemDatBan.Enabled = true;
            btn_DoiBan.Enabled = true;
            btn_XoaDatBan.Enabled = true;



            if (dgvKhachHang.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvKhachHang.SelectedRows[0];

                txt_HoTenKhach.Text = row.Cells["HoTen"].Value?.ToString();
                txt_SoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txt_SoLuongKhach.Text = row.Cells["SoLuongKhach"].Value?.ToString();
                txt_GhiChu.Text = row.Cells["GhiChu"].Value?.ToString();

                int maBan = Convert.ToInt32(row.Cells["MaBan"].Value);
                cbo_BanAn.SelectedValue = maBan;

            }



        }

        private void LoadDSKhachHangDaDat()
        {
            BanAnBLL bananbll = new BanAnBLL();
            dgvKhachHang.DataSource = bananbll.LayDanhSach_KhachHangDaDat();
        }
        private void Btn_LuuDatBan_Click(object sender, EventArgs e)
        {
            if (btn_ThemDatBan.Enabled == true)
            {
                if (string.IsNullOrWhiteSpace(txt_HoTenKhach.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_HoTenKhach.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_SoDienThoai.Text) || txt_SoDienThoai.Text.Length != 10)
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại có đúng 10 chữ số .", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_SoDienThoai.Focus();
                    return;
                }

                if (!int.TryParse(txt_SoLuongKhach.Text, out int soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng khách phải là số nguyên dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_SoLuongKhach.Focus();
                    return;
                }

                if (cbo_BanAn.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn bàn ăn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbo_BanAn.Focus();
                    return;
                }
                ///kiểm tra bàn ăn có trống hay không
                ///                
                BanAn ban = (BanAn)cbo_BanAn.SelectedItem;


                BanAnBLL ba = new BanAnBLL();
                string trangthai = ba.GetTrangThaiBanAn(ban.MaBan);
                if(trangthai != "Trống")
                {
                    MessageBox.Show("Bàn ăn này đã được đặt");
                    return;
                }    


                KhachHang kh = new KhachHang
                {
                    HoTen = txt_HoTenKhach.Text.Trim(),
                    SoDienThoai = txt_SoDienThoai.Text.Trim(),
                    SoLuongKhach = soLuong
                };

                KhachHangBLL khbll = new KhachHangBLL();

                int maKH = khbll.Insert_KhachHang(kh);
                if (maKH <= 0)
                {
                    MessageBox.Show("Thêm khách hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //BanAn ban = (BanAn)cbo_BanAn.SelectedItem;

                DatBan db = new DatBan
                {
                    MaKhachHang = maKH,
                    MaBan = ban.MaBan,
                    ThoiGianDat = DateTime.Now,
                    GhiChu = txt_GhiChu.Text.Trim()
                };

                BanAnBLL bananbll = new BanAnBLL();
                bool datBanOK = bananbll.Insert_DatBan(db);
                if (!datBanOK)
                {
                    MessageBox.Show("Đặt bàn thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                HoaDon hd = new HoaDon
                {
                    MaBan = ban.MaBan,
                    MaNhanVien = NhanVienDangNhap.MaNhanVien,
                    MaKhachHang = maKH,
                    ThoiGianLap = DateTime.Now,
                    TongTien = 0,
                    GiamGia = 0,
                    ThanhToan= 0,
                
                };

                HoaDonBLL hdbll = new HoaDonBLL();
                bool hoaDonOK = hdbll.Insert_HoaDon(hd);
                if (!hoaDonOK)
                {
                    MessageBox.Show("Tạo hóa đơn thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                BanAnBLL baanbll1 = new BanAnBLL();
                ban.TrangThai = "Đã đặt";
                bananbll.CapNhatTrangThaiBan(ban);

                MessageBox.Show("Đặt bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            //ày là nút sủa đặt bàn
            if(btn_SuaDatBan.Enabled == true)
            {


                //cập nhật khách hàng
                if (!int.TryParse(txt_SoLuongKhach.Text, out int soLuongMoi) || soLuongMoi <= 0)
                {
                    MessageBox.Show("Số lượng khách phải là số nguyên dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txt_SoDienThoai.Text) || txt_SoDienThoai.Text.Length != 10)
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại có đúng 10 chữ số .", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_SoDienThoai.Focus();
                    return;
                }
                int maKhachHang = Convert.ToInt32(dgvKhachHang.SelectedRows[0].Cells["MaKhachHang"].Value);
                KhachHangBLL khbll = new KhachHangBLL();
                bool ok = khbll.CapNhatKhachHang(maKhachHang, soLuongMoi, txt_SoDienThoai.Text.ToString());

                if (!ok)
                {
                    MessageBox.Show("Cập nhật khách hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //cập nhật ghi chú
                BanAnBLL banan = new BanAnBLL();

                int madatban = Convert.ToInt32(dgvKhachHang.SelectedRows[0].Cells["MaDatBan"].Value);

                bool check = banan.CapNhatGhiChu(madatban, txt_GhiChu.Text.ToString());

                if (!check)
                {
                    MessageBox.Show("Cập nhật ghi chú thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            //ày là nút đổi bàn
            if(btn_DoiBan.Enabled == true)
            {

                BanAnBLL ba = new BanAnBLL();

                int maban = Convert.ToInt32(dgvKhachHang.SelectedRows[0].Cells["MaBan"].Value);

                bool result = ba.CapNhatBan_ThanhTrong(maban);

                if(result)
                {
                    BanAn ban = (BanAn)cbo_BanAn.SelectedItem;
                    string trangthai = ba.GetTrangThaiBanAn(ban.MaBan);
                    if (trangthai != "Trống")
                    {
                        MessageBox.Show("Bàn ăn này đã được đặt");
                        return;
                    }


                    int madatban = Convert.ToInt32(dgvKhachHang.SelectedRows[0].Cells["MaDatBan"].Value);
                    int mabanmoi = int.Parse(cbo_BanAn.SelectedValue.ToString());

                    BanAnBLL banan = new BanAnBLL();

                    bool ok = banan.DoiBan(madatban, mabanmoi);

                    if (ok)
                    {
                        MessageBox.Show("Đổi bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("Đổi bàn thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }    
   
            }

            LoadDSKhachHangDaDat();
            Btn_HuyDatBan_Click(sender, e);

        }

        private void Btn_HuyDatBan_Click(object sender, EventArgs e)
        {
            btn_ThemDatBan.Enabled = true;
            btn_XoaDatBan.Enabled = true;
            btn_SuaDatBan.Enabled = true;
            btn_DoiBan.Enabled = true;


            btn_LuuDatBan.Enabled = false;
            btn_HuyDatBan.Enabled = false;

            txt_GhiChu.Clear();
            txt_HoTenKhach.Clear();
            txt_SoDienThoai.Clear();
            txt_SoLuongKhach.Clear();
            cbo_BanAn.SelectedIndex = 0;

            txt_GhiChu.Enabled = false;
            txt_HoTenKhach.Enabled = false;
            txt_SoDienThoai.Enabled = false;
            txt_SoLuongKhach.Enabled = false;
            cbo_BanAn.Enabled = false;

            BanAnBLL banan = new BanAnBLL();
            List<BanAn> listBanAn = banan.GetBanAn().OrderBy(x => x.Tang).ToList();
            cbo_BanAn.DisplayMember = "TenHienThi";
            cbo_BanAn.ValueMember = "MaBan";
            cbo_BanAn.DataSource = listBanAn;

        }

        private void Btn_ThemDatBan_Click(object sender, EventArgs e)
        {
            //btn_ThemDatBan.Enabled = true;
            btn_XoaDatBan.Enabled = false;
            btn_SuaDatBan.Enabled = false;
            btn_DoiBan.Enabled = false;


            btn_LuuDatBan.Enabled = true;
            btn_HuyDatBan.Enabled = true;

            txt_GhiChu.Enabled = true;
            txt_HoTenKhach.Enabled = true;
            txt_SoDienThoai.Enabled = true;
            txt_SoLuongKhach.Enabled = true;
            cbo_BanAn.Enabled = true;

            txt_GhiChu.Clear();
            txt_HoTenKhach.Clear();
            txt_SoDienThoai.Clear();
            txt_SoLuongKhach.Clear();
            cbo_BanAn.SelectedIndex = 0;




        }

        private void LoadBanAnTrong()
        {
            BanAnBLL banan = new BanAnBLL();
            List<BanAn> listBanAn = banan.GetBanAn().OrderBy(x => x.Tang).ToList();

            cbo_BanAn.DisplayMember = "TenHienThi";
            cbo_BanAn.ValueMember = "MaBan";
            cbo_BanAn.DataSource = listBanAn;

            //cbo_BanAn.Visible = false;

        }
        private void UC_DatBan_Load(object sender, EventArgs e)
        {
            btn_ThemDatBan.Enabled = true;
            btn_XoaDatBan.Enabled = false;
            btn_SuaDatBan.Enabled = false;
            btn_DoiBan.Enabled = false;


            btn_LuuDatBan.Enabled = false;
            btn_HuyDatBan.Enabled = false;

            txt_GhiChu.Enabled = false;
            txt_HoTenKhach.Enabled = false;
            txt_SoDienThoai.Enabled = false;
            txt_SoLuongKhach.Enabled = false;
            cbo_BanAn.Enabled = false;

            LoadBanAnTrong();
            LoadDSKhachHangDaDat();
            dgvKhachHang.ReadOnly = true;



        }


        public void Reload()
        {
            LoadBanAnTrong();

            LoadDSKhachHangDaDat();

            Btn_HuyDatBan_Click(null, null);

            if (dgvKhachHang.Rows.Count > 0)
                dgvKhachHang.ClearSelection();
        }

    }
}
