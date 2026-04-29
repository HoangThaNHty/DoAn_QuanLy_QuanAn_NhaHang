using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DTO;

namespace QUANLY_NHAHANG
{
    public partial class UC_KhuyenMai : UserControl
    {
        private bool isThemMoi = false;
        private int maKhuyenMaiDangChon = -1;
        public UC_KhuyenMai()
        {
            InitializeComponent();
            this.Load += UC_KhuyenMai_Load;

            btn_ThemVoucher.Click += Btn_ThemVoucher_Click;
            btn_XoaVoucher.Click += Btn_XoaVoucher_Click;
            btn_SuaVoucher.Click += Btn_SuaVoucher_Click;

            btn_HuyVoucher.Click += Btn_HuyVoucher_Click;
            btn_LuuVoucher.Click += Btn_LuuVoucher_Click;

            txt_PhanTramGiam.KeyPress += Txt_PhanTramGiam_KeyPress;

            dgv_KhuyenMai.SelectionChanged += Dgv_KhuyenMai_SelectionChanged;
        }

        private void Dgv_KhuyenMai_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_KhuyenMai.SelectedRows.Count == 0)
                return;

            if (dgv_KhuyenMai.SelectedRows.Count > 0 && dgv_KhuyenMai.SelectedRows[0].DataBoundItem is Voucher km)
            {
                txt_TenKhuyenMai.Text = km.TenKM;
                txt_PhanTramGiam.Text = km.PhanTramGiam.ToString();
                txt_MoTa.Text = km.MoTa;
                timPC_NgayBD.Value = km.NgayBatDau;
                timePC_NgayKT.Value = km.NgayKetThuc;

                maKhuyenMaiDangChon = km.MaKM;
            }

            btn_HuyVoucher.Enabled = false;
            btn_LuuVoucher.Enabled = false;

            btn_ThemVoucher.Enabled = true;
            btn_XoaVoucher.Enabled = true;
            btn_SuaVoucher.Enabled = true;

            txt_TenKhuyenMai.Enabled = false;
            txt_PhanTramGiam.Enabled = false;
            txt_MoTa.Enabled = false;
            timePC_NgayKT.Enabled = false;
            timPC_NgayBD.Enabled = false;

        }

        private void Txt_PhanTramGiam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            } 
                
        }

        private void Btn_SuaVoucher_Click(object sender, EventArgs e)
        {
            if (dgv_KhuyenMai.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa.");
                return;
            }

            isThemMoi = false;

            Voucher km = (Voucher)dgv_KhuyenMai.SelectedRows[0].DataBoundItem;
            maKhuyenMaiDangChon = km.MaKM;

            txt_TenKhuyenMai.Text = km.TenKM;
            txt_PhanTramGiam.Text = km.PhanTramGiam.ToString();
            txt_MoTa.Text = km.MoTa;
            timPC_NgayBD.Value = km.NgayBatDau;
            timePC_NgayKT.Value = km.NgayKetThuc;

            txt_TenKhuyenMai.Enabled = false;
            txt_PhanTramGiam.Enabled = true;
            txt_MoTa.Enabled = true;
            timePC_NgayKT.Enabled = true;
            timPC_NgayBD.Enabled = true;

            btn_HuyVoucher.Enabled = true;
            btn_LuuVoucher.Enabled = true;
            btn_ThemVoucher.Enabled = false;
            btn_XoaVoucher.Enabled = false;
            btn_SuaVoucher.Enabled = false;

        }

        private void Btn_LuuVoucher_Click(object sender, EventArgs e)
        {
            string tenKM = txt_TenKhuyenMai.Text.Trim();
            if (string.IsNullOrEmpty(tenKM))
            {
                MessageBox.Show("Vui lòng nhập tên khuyến mãi.");
                return;
            }


            if (!int.TryParse(txt_PhanTramGiam.Text, out int phanTram) || phanTram < 0 || phanTram > 100)
            {
                MessageBox.Show("Phần trăm giảm phải từ 0 đến 100.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (timPC_NgayBD.Value > timePC_NgayKT.Value)
            {
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Voucher km = new Voucher
            {
                TenKM = txt_TenKhuyenMai.Text,
                MoTa = txt_MoTa.Text,
                PhanTramGiam = phanTram,
                NgayBatDau = timPC_NgayBD.Value,
                NgayKetThuc = timePC_NgayKT.Value
            };

            VoucherBLL vc = new VoucherBLL();
            bool result = false;
           

            if (isThemMoi)
            {

                if (vc.KiemTraTrungTen(tenKM))
                {
                    MessageBox.Show("Tên khuyến mãi đã tồn tại. Vui lòng chọn tên khác.", "Trùng tên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if(timPC_NgayBD.Value < DateTime.Today)
                {
                    MessageBox.Show("Ngày bắt đầu phải từ ngày hôm nay trở về sau");
                    return;

                }   
                
                result = vc.Them(km);
                if (result)
                    MessageBox.Show("Thêm khuyến mãi thành công!");
            }
            else
            {
                km.MaKM = maKhuyenMaiDangChon;
                result = vc.Sua(km);
                if (result)
                    MessageBox.Show("Cập nhật khuyến mãi thành công!");
            }

            if (result)
            {
                LoadVaoBan();
                Btn_HuyVoucher_Click(null, null);
            }
            else
            {
                MessageBox.Show("Thao tác thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Btn_HuyVoucher_Click(object sender, EventArgs e)
        {
            txt_TenKhuyenMai.Clear();
            txt_PhanTramGiam.Clear();
            txt_MoTa.Clear();
            timPC_NgayBD.Value = DateTime.Now;
            timePC_NgayKT.Value = DateTime.Now;

            btn_HuyVoucher.Enabled = false;
            btn_LuuVoucher.Enabled = false;

            btn_ThemVoucher.Enabled = true;
            btn_XoaVoucher.Enabled = true;
            btn_SuaVoucher.Enabled = true;

            txt_TenKhuyenMai.Enabled = false;
            txt_PhanTramGiam.Enabled = false;
            txt_MoTa.Enabled = false;
            timePC_NgayKT.Enabled = false;
            timPC_NgayBD.Enabled = false;

            LoadVaoBan();
        }

        private void Btn_XoaVoucher_Click(object sender, EventArgs e)
        {
           

            if (dgv_KhuyenMai.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.");
                return;
            }

            Voucher km = (Voucher)dgv_KhuyenMai.SelectedRows[0].DataBoundItem;

            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa khuyến mãi này?", "Xác nhận", MessageBoxButtons.YesNo);
            VoucherBLL vc = new VoucherBLL();
            if (result == DialogResult.Yes)
            {
                bool ok = vc.Xoa(km.MaKM);
                if (ok)
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadVaoBan();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.");
                }
            }


        }

        private void Btn_ThemVoucher_Click(object sender, EventArgs e)
        {
            isThemMoi = true;

            btn_HuyVoucher.Enabled = true;
            btn_LuuVoucher.Enabled = true;

            btn_ThemVoucher.Enabled = false;
            btn_XoaVoucher.Enabled = false;
            btn_SuaVoucher.Enabled = false;

            txt_TenKhuyenMai.Enabled = true;
            txt_PhanTramGiam.Enabled = true;
            txt_MoTa.Enabled = true;
            timePC_NgayKT.Enabled = true;
            timPC_NgayBD.Enabled = true;

            txt_TenKhuyenMai.Clear();
            txt_PhanTramGiam.Clear();
            txt_MoTa.Clear();
            timPC_NgayBD.Value = DateTime.Now;
            timePC_NgayKT.Value = DateTime.Now;


        }

        public void LoadVaoBan()
        {
            VoucherBLL voucher = new VoucherBLL();
            List<Voucher> listVC = voucher.LayDanhSach();

            dgv_KhuyenMai.DataSource = listVC;
            //dgv_KhuyenMai.ClearSelection();


        }
        private void UC_KhuyenMai_Load(object sender, EventArgs e)
        {
            btn_HuyVoucher.Enabled = false;
            btn_LuuVoucher.Enabled = false;

            btn_ThemVoucher.Enabled = true;
            btn_XoaVoucher.Enabled = true;
            btn_SuaVoucher.Enabled = true;

            txt_TenKhuyenMai.Enabled = false;
            txt_PhanTramGiam.Enabled = false;
            txt_MoTa.Enabled = false;
            timePC_NgayKT.Enabled = false;
            timPC_NgayBD.Enabled = false;

            LoadVaoBan();

            dgv_KhuyenMai.ReadOnly = true;

        }
        public void Reload()
        {
            btn_HuyVoucher.Enabled = false;
            btn_LuuVoucher.Enabled = false;

            btn_ThemVoucher.Enabled = true;
            btn_XoaVoucher.Enabled = true;
            btn_SuaVoucher.Enabled = true;

            txt_TenKhuyenMai.Enabled = false;
            txt_PhanTramGiam.Enabled = false;
            txt_MoTa.Enabled = false;
            timePC_NgayKT.Enabled = false;
            timPC_NgayBD.Enabled = false;

            txt_TenKhuyenMai.Clear();
            txt_PhanTramGiam.Clear();
            txt_MoTa.Clear();
            timPC_NgayBD.Value = DateTime.Now;
            timePC_NgayKT.Value = DateTime.Now;

            LoadVaoBan();

            dgv_KhuyenMai.ClearSelection();
        }




    }
}
