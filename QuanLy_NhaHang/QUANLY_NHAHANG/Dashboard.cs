using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BLL;
using DTO;
using Guna.UI2.WinForms;

namespace QUANLY_NHAHANG
{
    public partial class Dashboard : Form
    {
        public NhanVien nv;
        public Form dangnhap;

        public Account_BLL acbll = new Account_BLL();

        private List<NhanVien_TaiKhoan> danhSachNhanVien;

        private bool isFormLoading = true;

        public string imagePath = "";



        public Dashboard(NhanVien _nv, Form _dangnhap)
        {



            InitializeComponent();

            guna2TabControl1.SelectedIndexChanged += Guna2TabControl1_SelectedIndexChanged;
            //home

            this.Load += Dashboard_Load;
            this.FormClosing += Dashboard_FormClosing;
            nv = _nv;
            dangnhap = _dangnhap;
            btn_DangXuat.Click += Btn_DangXuat_Click;
            dgv_DSNhanVien.SelectionChanged += Dgv_DSNhanVien_SelectionChanged;
            cbo_SapXep.SelectedIndexChanged += Cbo_SapXep_SelectedIndexChanged;
            cbo_loc.SelectedIndexChanged += Cbo_loc_SelectedIndexChanged;
            btn_Them.Click += Btn_Them_Click;
            btn_luu.Click += Btn_luu_Click;
            btn_huy.Click += Btn_huy_Click;
            txt_TenDangNhap.TextChanged += Txt_TenDangNhap_TextChanged;
            btn_Xoa.Click += Btn_Xoa_Click;
            btn_Sua.Click += Btn_Sua_Click;
            txt_SoDienThoai.KeyPress += Txt_SoDienThoai_KeyPress;


            //trang món ăn 
            btn_ChonAnh.Click += Btn_ChonAnh_Click;
            btn_ThemMonAn.Click += Btn_ThemMonAn_Click;
            btn_LuuMonAn.Click += Btn_LuuMonAn_Click;
            dgv_MonAn.SelectionChanged += Dgv_MonAn_SelectionChanged;
            btn_XoaMonAn.Click += Btn_XoaMonAn_Click;
            btn_SuaMonAn.Click += Btn_SuaMonAn_Click;
            btn_HuyMonAn.Click += Btn_HuyMonAn_Click;
            txt_DonGia.KeyPress += Txt_DonGia_KeyPress;
            txt_SoLuong.KeyPress += Txt_SoLuong_KeyPress;


            //trang bàn ăn

            //tab_BanAn.Click += Tab_BanAn_Click;
            btn_ThemBan.Click += Btn_ThemBan_Click;
            btn_LuuBan.Click += Btn_LuuBan_Click;
            cbo_Tang.SelectedIndexChanged += Cbo_Tang_SelectedIndexChanged;
            btn_XoaBan.Click += Btn_XoaBan_Click;
            btn_SuaBan.Click += Btn_SuaBan_Click;
            btn_HuyBan.Click += Btn_HuyBan_Click;
            txt_SoChoNgoi.KeyPress += Txt_SoChoNgoi_KeyPress;

            //trang order
            guna2TabControl3.SelectedIndexChanged += Guna2TabControl3_SelectedIndexChanged;

            //trang nhân viên



            //trang khuyến mãi


            //thonngs ke
            guna2TabControl4.SelectedIndexChanged += Guna2TabControl4_SelectedIndexChanged;

            //thanh toán

        }

        private void Guna2TabControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;

            if (tabControl != null)
            {
                if (tabControl.SelectedTab == tab_ThongKe_DanhSachHoaDon)
                {
                    this.uC_DanhSachHoaDon1.Reload();
                }
                else if (tabControl.SelectedTab == tab_ThongKe_DoanhThu)
                {
                    this.uC_ThongKe2.Reload();

                }
            }
        }

        private void Guna2TabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;

            if(tabControl != null)
            {
                if(tabControl.SelectedTab == tab_DatBan )
                {
                    this.uC_DatBan1.Reload();
                }
                else if(tabControl.SelectedTab == tab_GoiMon)
                {
                    this.uC_GoiMon1.Reload();

                }
            }    
        }

       

        private void Guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;

            if (tabControl != null)
            {
                if (tabControl.SelectedTab == tab_Home)
                {
                    ThongKe_BLL tk = new ThongKe_BLL();
                    VeBieuDoTrangThaiBanAn(tk.ThongKeTrangThaiBanAn());
                    VeBieuDoDoanhThuTheoNgay(tk.LayHoaDonTheoNgay(DateTime.Now, DateTime.Now));
                    LoadBanAnTrong();
                    LoadMonAnCon();

                }
               
                else if (tabControl.SelectedTab == tab_BanAn)
                {
                    tableLayout_tabtang1.Controls.Clear();
                    tableLayout_TabTang2.Controls.Clear();
                    Load_Ban_Vao_Vi_Tri();
                }
                else if (tabControl.SelectedTab == tab_MonAn)
                {
                    LoadDataMonAn();

                }
                else if (tabControl.SelectedTab == tab_Order)
                {
                    this.uC_DatBan1.Reload();
                    this.uC_GoiMon1.Reload();


                }
                else if (tabControl.SelectedTab == tab_NhanVien)
                {
                    cbo_ChucVu.Items.Clear();
                    cbo_ChucVu.Items.Add("Nhân viên");
                    cbo_ChucVu.Items.Add("Quản lý");
                    cbo_ChucVu.SelectedIndex = 0;

                    danhSachNhanVien = acbll.Get_NhanVien_TaiKhoan();
                    dgv_DSNhanVien.DataSource = danhSachNhanVien;

                    dgv_DSNhanVien.AutoGenerateColumns = false;

                    dgv_DSNhanVien.DataBindingComplete += (s, e2) =>
                    {
                        dgv_DSNhanVien.Columns["Column25"].DataPropertyName = null;
                        GanSoThuTu(dgv_DSNhanVien);
                    };

                    dgv_DSNhanVien.ReadOnly = true;

                    cbo_SapXep.Items.Clear();
                    cbo_SapXep.Items.Add("Sắp xếp");
                    cbo_SapXep.Items.Add("Tăng dần theo tên");
                    cbo_SapXep.Items.Add("Giảm dần theo tên");
                    cbo_SapXep.SelectedIndex = 0;

                    cbo_loc.Items.Clear();
                    cbo_loc.Items.Add("Tất cả");
                    cbo_loc.Items.Add("Nhân viên");
                    cbo_loc.Items.Add("Quản lý");
                    cbo_loc.Items.Add("Nam");
                    cbo_loc.Items.Add("Nữ");

                    cbo_loc.SelectedIndex = 0;

                }
                else if (tabControl.SelectedTab == tab_KhuyenMai)
                {
                    this.uC_KhuyenMai1.Reload();
                }
                else if (tabControl.SelectedTab == tab_ThongKe)
                {
                    this.uC_DanhSachHoaDon1.Reload();
                    this.uC_ThongKe2.Reload();
                }
                else if (tabControl.SelectedTab == tab_ThanhToan)
                {
                    this.uC_ThanhToan2.Reload();
                }
            }
        }

       
      
        private void Tab_NhanVien_Click(object sender, EventArgs e)
        {
            danhSachNhanVien = acbll.Get_NhanVien_TaiKhoan();
            dgv_DSNhanVien.DataSource = danhSachNhanVien;

            dgv_DSNhanVien.AutoGenerateColumns = false;

            dgv_DSNhanVien.DataBindingComplete += (s, e2) =>
            {
                dgv_DSNhanVien.Columns["Column25"].DataPropertyName = null;
                GanSoThuTu(dgv_DSNhanVien);
            };
        }






        //trang nhân viên
        private void Txt_SoDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }





        //------------------------------------------------------------------------------------------------trang bàn ăn
        //private void Tab_BanAn_Click(object sender, EventArgs e)
        //{
        //    Load_Ban_Vao_Vi_Tri();

        //}
        private void Btn_HuyBan_Click(object sender, EventArgs e)
        {
            tableLayout_tabtang1.Controls.Clear();
            tableLayout_TabTang2.Controls.Clear();
            Load_Ban_Vao_Vi_Tri();
        }

        private void Btn_SuaBan_Click(object sender, EventArgs e)
        {
            

            btn_ThemBan.Enabled = false;
            btn_XoaBan.Enabled = false;

            btn_LuuBan.Enabled = true;
            btn_HuyBan.Enabled = true;

            txt_SoBan.Enabled = false;

            cbo_Tang.Enabled = true;
            txt_SoChoNgoi.Enabled = true;
            cbo_TrangThai.Enabled = true;

        }

        private void Btn_XoaBan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_MaBan_Temp.Text))
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi xóa!");
                return;
            }

            DialogResult r;
            r = MessageBox.Show("Bạn muốn xóa ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (r == DialogResult.Yes)
            {
                BanAnBLL ban = new BanAnBLL();
                int maban = int.Parse(txt_MaBan_Temp.Text.ToString());
                
                string trangthai = ban.GetTrangThaiBanAn(maban);
                if(trangthai != "Trống")
                {
                    MessageBox.Show("Chỉ có thể xóa bàn ăn lúc 'Trống'");
                    return;
                }    
                if (ban.XoaBan(maban))
                {
                    MessageBox.Show("Xóa bàn ăn thành công!");
                    tableLayout_tabtang1.Controls.Clear();
                    tableLayout_TabTang2.Controls.Clear();
                    Load_Ban_Vao_Vi_Tri();
                }
                else
                {
                    MessageBox.Show("Xóa bàn ăn thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void fillSoBan(int tang)
        {
            BanAnBLL ba = new BanAnBLL();
            var list = ba.GetBanAn_TheoTang(tang);

            int soluong = list.Count();

            for (int i = 1; i < soluong + 2; i++)
            {
                bool check = false;
                foreach (BanAn b in list)
                {
                    if (i == b.SoBan)
                    {
                        check = true;
                    }
                }

                if (check == false)
                {
                    txt_SoBan.Text = i.ToString();
                    return;
                }
            }
        }

        private void Txt_SoChoNgoi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            int tang = int.Parse(cbo_Tang.SelectedItem.ToString());
            fillSoBan(tang);
        }

        private void Cbo_Tang_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (int.TryParse(cbo_Tang.SelectedItem.ToString(), out int tang))
            {
                fillSoBan(tang);
            }
        }
        
        private void Btn_LuuBan_Click(object sender, EventArgs e)
        {
            if (btn_ThemBan.Enabled == true)
            {
                BanAnBLL ba = new BanAnBLL();
                BanAn banan = new BanAn();
                //banan.SoBan = int.Parse(txt_SoBan.ToString());
                if(txt_SoChoNgoi.Text.Length <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số chổ ngồi");
                    txt_SoChoNgoi.Focus();
                    return;
                }    
                banan.SoBan = Convert.ToInt32(txt_SoBan.Text.ToString());
                banan.SoChoNgoi = Convert.ToInt32(txt_SoChoNgoi.Text.ToString());
                banan.Tang = Convert.ToInt32(cbo_Tang.SelectedItem.ToString());
                banan.TrangThai = cbo_TrangThai.SelectedItem.ToString();


                if (ba.ThemBan(banan))
                {
                    tableLayout_tabtang1.Controls.Clear();
                    tableLayout_TabTang2.Controls.Clear();
                    MessageBox.Show("Thêm thành công", "Thông báo");
                    Load_Ban_Vao_Vi_Tri();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại", "Thông báo");
                }

            }
            else if (btn_SuaBan.Enabled == true)
            {
                if (txt_SoChoNgoi.Text.Length <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số chổ ngồi");
                    txt_SoChoNgoi.Focus();
                    return;
                }

                BanAnBLL ba = new BanAnBLL();

                BanAn banan = new BanAn();
                banan.MaBan = int.Parse(txt_MaBan_Temp.Text.ToString());
                banan.SoBan = Convert.ToInt32(txt_SoBan.Text.ToString());
                banan.SoChoNgoi = Convert.ToInt32(txt_SoChoNgoi.Text.ToString());
                banan.Tang = Convert.ToInt32(cbo_Tang.SelectedItem.ToString());
                banan.TrangThai = cbo_TrangThai.SelectedItem.ToString();

                if (ba.GetTrangThaiBanAn(banan.MaBan) != "Trống")
                {
                    MessageBox.Show("Chỉ có thể cập nhật lúc bàn ăn trống");
                    return;
                }


                if (ba.SuaBan(banan))
                {
                    tableLayout_tabtang1.Controls.Clear();
                    tableLayout_TabTang2.Controls.Clear();
                    MessageBox.Show("Cập nhật thành công", "Thông báo");
                    Load_Ban_Vao_Vi_Tri();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại", "Thông báo");

                }
            }
            else
            {
                MessageBox.Show("Lỗi không xác đinh", "Thông báo");
                return;
            }
        }

        private void Btn_ThemBan_Click(object sender, EventArgs e)
        {
            btn_SuaBan.Enabled = false;
            btn_XoaBan.Enabled = false;

            btn_LuuBan.Enabled = true;
            btn_HuyBan.Enabled = true;

            txt_SoBan.Clear();
            txt_SoChoNgoi.Clear();
            cbo_Tang.SelectedIndex = 0;
            cbo_TrangThai.SelectedIndex = 0;

            txt_SoBan.Enabled = false;
            txt_SoChoNgoi.Enabled = true;
            cbo_Tang.Enabled = true;
            cbo_TrangThai.Enabled = false;


        }

        public void Load_BanAn()
        {
            txt_SoBan.Clear();
            txt_SoChoNgoi.Clear();


            cbo_Tang.Items.Clear();
            cbo_Tang.Items.Add("1");
            cbo_Tang.Items.Add("2");
            cbo_Tang.SelectedIndex = 0;

            cbo_TrangThai.Items.Clear();
            cbo_TrangThai.Items.Add("Trống");
            cbo_TrangThai.Items.Add("Đã đặt");
            cbo_TrangThai.Items.Add("Đang dùng");
            cbo_TrangThai.SelectedIndex = 0;

            txt_SoBan.Enabled = false;
            txt_SoChoNgoi.Enabled = false;
            cbo_Tang.Enabled = false;
            cbo_TrangThai.Enabled = false;

            btn_ThemBan.Enabled = true;
            btn_SuaBan.Enabled = false;
            btn_XoaBan.Enabled = false;

            btn_LuuBan.Enabled = false;
            btn_HuyBan.Enabled = false;

            txt_MaBan_Temp.Visible = false;

        }
        public void Load_Ban_Vao_Vi_Tri()
        {

            Load_BanAn();

            BanAnBLL bananbll = new BanAnBLL();
            List<BanAn> list = bananbll.GetBanAn();

            int col1 = 1;
            int row1 = 1;

            int col2 = 1;
            int row2 = 1;
            foreach (BanAn an in list)
            {


                if (an.Tang == 1)
                {
                    Button btnBan = new Button();
                    btnBan.Text = "Bàn " + an.SoBan.ToString() + " - " + an.SoChoNgoi.ToString() + "c";
                    btnBan.Dock = DockStyle.Fill;

                    if (an.TrangThai == "Trống")
                    {
                        btnBan.BackColor = Color.Green;

                    }
                    else if (an.TrangThai == "Đã đặt")
                    {
                        btnBan.BackColor = Color.Yellow;

                    }
                    else
                    {
                        btnBan.BackColor = Color.Red;

                    }

                    btnBan.Tag = an;

                    btnBan.Click += BtnBan_Click;


                    tableLayout_tabtang1.Controls.Add(btnBan, col1, row1);
                    if (col1 >= 9)
                    {
                        col1 = 1;
                        row1 = row1 + 2;
                    }
                    else
                    {
                        col1 = col1 + 2;
                    }

                    if (row1 > 9)
                    {
                        return;
                    }
                }
                if (an.Tang == 2)
                {
                    Button btnBan = new Button();
                    btnBan.Text = "Bàn " + an.SoBan.ToString() + " - " + an.SoChoNgoi.ToString() + "c";
                    btnBan.Dock = DockStyle.Fill;

                    if (an.TrangThai == "Trống")
                    {
                        btnBan.BackColor = Color.Green;

                    }
                    else if (an.TrangThai == "Đã đặt")
                    {
                        btnBan.BackColor = Color.Yellow;

                    }
                    else
                    {
                        btnBan.BackColor = Color.Red;

                    }

                    btnBan.Tag = an;

                    btnBan.Click += BtnBan_Click;


                    tableLayout_TabTang2.Controls.Add(btnBan, col2, row2);
                    if (col2 >= 9)
                    {
                        col2 = 1;
                        row2 = row2 + 2;
                    }
                    else
                    {
                        col2 = col2 + 2;
                    }

                    if (row2 > 9)
                    {
                        return;
                    }
                }
            }
        }

        private void BtnBan_Click(object sender, EventArgs e)
        {

            btn_SuaBan.Enabled = true;
            btn_XoaBan.Enabled = true;
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                BanAn ban = (BanAn)btn.Tag;

                txt_MaBan_Temp.Text = ban.MaBan.ToString();
                txt_SoBan.Text = ban.SoBan.ToString();
                txt_SoChoNgoi.Text = ban.SoChoNgoi.ToString();
                cbo_Tang.SelectedItem = ban.Tang.ToString();
                cbo_TrangThai.SelectedItem = ban.TrangThai.ToString();

                txt_SoBan.Enabled = false;
                txt_SoChoNgoi.Enabled = false;
                cbo_Tang.Enabled = false;
                cbo_TrangThai.Enabled = false;

            }
        }


        //------------------------------------------------------------------------------------------------trang món ăn




        private void Txt_DonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private void Txt_SoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private void Btn_HuyMonAn_Click(object sender, EventArgs e)
        {
            LoadDataMonAn();
        }

        private void Btn_SuaMonAn_Click(object sender, EventArgs e)
        {
            btn_XoaMonAn.Enabled = false;
            btn_ThemMonAn.Enabled = false;

            btn_LuuMonAn.Enabled = true;
            btn_HuyMonAn.Enabled = true;

            input_text_enable(true);


        }

        private void Btn_XoaMonAn_Click(object sender, EventArgs e)
        {
            MonAn_BLL monanBLL = new MonAn_BLL();

            if (dgv_MonAn.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn món ăn cần xóa!");
                return;
            }
            int maMon = Convert.ToInt32(dgv_MonAn.CurrentRow.Cells["MaMon"].Value);

            if (monanBLL.KiemTraMaMonAnTrongChiTietHoaDon(maMon))
            {
                MessageBox.Show("Món ăn này được mở bán KHÔNG THỂ XÓA");
                return;
            }    

            DialogResult r;
            r = MessageBox.Show("Bạn muốn xóa ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                bool kq = monanBLL.Delete_MonAn(maMon);
                if (kq)
                {
                    MessageBox.Show("Xóa món ăn thành công!");
                    LoadDataMonAn();
                }
                else
                {
                    MessageBox.Show("Xóa món ăn thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void Dgv_MonAn_SelectionChanged(object sender, EventArgs e)
        {
            input_text_enable(false);
            if (dgv_MonAn.CurrentRow != null && dgv_MonAn.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dgv_MonAn.CurrentRow;

                txt_TenMon.Text = row.Cells["TenMon"].Value?.ToString();
                txt_MoTa.Text = row.Cells["MoTa"].Value?.ToString();
                txt_DonGia.Text = row.Cells["DonGia"].Value?.ToString();
                int mamon = int.Parse(row.Cells["MaMon"].Value?.ToString());
                var hinhAnhValue = row.Cells["HinhAnh"].Value as Image;
                if (hinhAnhValue != null)
                {
                    if (pic_MonAn.Image != null)
                    {
                        pic_MonAn.Image.Dispose();
                        pic_MonAn.Image = null;
                    }

                    pic_MonAn.Image = (Image)hinhAnhValue.Clone();

                    MonAn_BLL ma = new MonAn_BLL();
                    string maanh = ma.LayDuongDanAnh(mamon);

                    imagePath = maanh;
                }
                else
                {
                    pic_MonAn.Image = null;
                    imagePath = "";
                }


                cbo_LoaiMon.SelectedItem = row.Cells["LoaiMon"].Value?.ToString();
                txt_SoLuong.Text = row.Cells["SoLuong"].Value?.ToString();

            }

        }

        private void Btn_LuuMonAn_Click(object sender, EventArgs e)
        {
            if (btn_ThemMonAn.Enabled == true)
            {
                MonAn_BLL monanbll = new MonAn_BLL();
                if (string.IsNullOrWhiteSpace(txt_TenMon.Text) || string.IsNullOrWhiteSpace(txt_MoTa.Text) || string.IsNullOrWhiteSpace(txt_DonGia.Text) || pic_MonAn.Image == null || string.IsNullOrWhiteSpace(txt_SoLuong.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin món ăn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txt_DonGia.Text, out decimal donGia))
                {
                    MessageBox.Show("Đơn giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txt_SoLuong.Text, out int soLuong))
                {
                    MessageBox.Show("Số lượng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var loaimon = cbo_LoaiMon.SelectedItem.ToString();
                MonAn mon = new MonAn
                {
                    TenMon = txt_TenMon.Text.Trim(),
                    MoTa = txt_MoTa.Text.Trim(),
                    DonGia = donGia,
                    SoLuong = soLuong,
                    HinhAnh = imagePath,
                    LoaiMon = loaimon
                };

                MonAn_BLL monanBLL = new MonAn_BLL();
                bool kq = monanBLL.Insert_MonAn(mon);

                if (kq)
                {
                    LoadDataMonAn();
                    MessageBox.Show("Thêm món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_TenMon.Clear();
                    txt_MoTa.Clear();
                    txt_DonGia.Clear();
                    txt_SoLuong.Clear();
                    pic_MonAn.Image = null;
                    imagePath = "";
                }
                else
                {
                    MessageBox.Show("Lỗi khi thêm món ăn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (btn_SuaMonAn.Enabled == true)
            {
                if (dgv_MonAn.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn món ăn để sửa!");
                    return;
                }
                int maMon = Convert.ToInt32(dgv_MonAn.CurrentRow.Cells["MaMon"].Value);

                string newImagePath = imagePath;



                MonAn mon = new MonAn
                {
                    MaMon = maMon,
                    TenMon = txt_TenMon.Text.Trim(),
                    MoTa = txt_MoTa.Text.Trim(),
                    DonGia = decimal.TryParse(txt_DonGia.Text, out decimal dg) ? dg : 0,
                    LoaiMon = cbo_LoaiMon.SelectedItem?.ToString(),
                    SoLuong = int.TryParse(txt_SoLuong.Text, out int sl) ? sl : 0,
                    HinhAnh = imagePath
                };

                MonAn_BLL monanBLL = new MonAn_BLL();
                if (monanBLL.Update_MonAn(mon))
                {
                    MessageBox.Show("Sửa món ăn thành công!");
                    LoadDataMonAn();
                }
                else
                {
                    MessageBox.Show("Sửa món ăn thất bại!");
                }
            }
            else
            {
                MessageBox.Show("lỗi thao tác");
                return;
            }
        }

        private void Btn_ThemMonAn_Click(object sender, EventArgs e)
        {
            dgv_MonAn.ClearSelection();

            input_text_enable(true);
            txt_TenMon.Clear();
            txt_MoTa.Clear();
            txt_DonGia.Clear();
            pic_MonAn.Image = null;
            cbo_LoaiMon.SelectedIndex = 0;
            txt_SoLuong.Clear();


            btn_XoaMonAn.Enabled = false;
            btn_SuaMonAn.Enabled = false;

            btn_LuuMonAn.Enabled = true;
            btn_HuyMonAn.Enabled = true;


        }



        private void Btn_ChonAnh_Click(object sender, EventArgs e)
        {
            

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string sourceFile = ofd.FileName;
                string folderPath = Path.Combine(Application.StartupPath, "Images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(folderPath, fileName);
                if (!File.Exists(destFile))
                    File.Copy(sourceFile, destFile, true);

                if (pic_MonAn.Image != null)
                {
                    pic_MonAn.Image.Dispose();
                    pic_MonAn.Image = null;
                }

                pic_MonAn.Image = Image.FromFile(destFile);
                pic_MonAn.Tag = fileName;

                // Chỉ cập nhật đường dẫn khi chọn ảnh mới
                imagePath = "Images/" + fileName;
            }
        }


        private void LoadDataMonAn()
        {
            MonAn_BLL monanbll = new MonAn_BLL();
            List<MonAn> ds = monanbll.Get_MonAn();

            DataTable dt = new DataTable();
            dt.Columns.Add("MaMon", typeof(int));
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("HinhAnh", typeof(Image));
            dt.Columns.Add("TenMon", typeof(string));
            dt.Columns.Add("MoTa", typeof(string));
            dt.Columns.Add("DonGia", typeof(decimal));
            dt.Columns.Add("LoaiMon", typeof(string));
            dt.Columns.Add("SoLuong", typeof(int));

            int stt = 1;
            foreach (var item in ds)
            {
                Image img = null;
                if (!string.IsNullOrEmpty(item.HinhAnh))
                {
                    string path = Path.Combine(Application.StartupPath, item.HinhAnh);
                    if (File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            img = Image.FromStream(fs);
                        }
                    }
                }

                dt.Rows.Add(item.MaMon, stt++, img, item.TenMon, item.MoTa, item.DonGia, item.LoaiMon, item.SoLuong);
            }

            dgv_MonAn.DataSource = dt;
         


            dgv_MonAn.RowTemplate.Height = 80;
            DataGridViewImageColumn imgCol = (DataGridViewImageColumn)dgv_MonAn.Columns["HinhAnh"];
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;

            cbo_LoaiMon.Items.Clear();
            cbo_LoaiMon.Items.Add("Gà");
            cbo_LoaiMon.Items.Add("Cá");
            cbo_LoaiMon.Items.Add("Bò");
            cbo_LoaiMon.Items.Add("Rau");
            cbo_LoaiMon.Items.Add("Đồ chay");
            cbo_LoaiMon.Items.Add("Thức uống");
            cbo_LoaiMon.SelectedIndex = 0;


            dgv_MonAn.ReadOnly = true;
            dgv_MonAn.AllowUserToAddRows = false;


            btn_ThemMonAn.Enabled = true;
            btn_XoaMonAn.Enabled = true;
            btn_SuaMonAn.Enabled = true;

            btn_LuuMonAn.Enabled = false;
            btn_HuyMonAn.Enabled = false;

        }


        private void input_text_enable(bool val)
        {
            txt_TenMon.Enabled = val;
            txt_MoTa.Enabled = val;
            txt_DonGia.Enabled = val;
            txt_SoLuong.Enabled = val;
            cbo_LoaiMon.Enabled = val;
            btn_ChonAnh.Enabled = val;

        }






        //----------------------------------------------------------------trang nhân viên
        private void Btn_Sua_Click(object sender, EventArgs e)
        {
            btn_Them.Enabled = false;
            btn_Xoa.Enabled = false;
            btn_luu.Enabled = true;
            btn_huy.Enabled = true;

            txt_HoTen.Enabled = true;
            rdb_Nam.Enabled = true;
            rdb_Nu.Enabled = true;
            cbo_ChucVu.Enabled = true;
            txt_SoDienThoai.Enabled = true;

            txt_TenDangNhap.Enabled = false;

            lb_Error.Visible = false;

            txt_HoTen.Tag = "edit";
        }

        private void Btn_Xoa_Click(object sender, EventArgs e)
        {
            Account_BLL acbll_delete = new Account_BLL();

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên không?", "Xác nhận xóa",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }
            if (dgv_DSNhanVien.CurrentRow == null && dgv_DSNhanVien.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa");
                return;
            }

            string tentk = "";
            int manv = 0;

            btn_luu.Enabled = false;
            btn_huy.Enabled = false;
            btn_Them.Enabled = false;
            btn_Sua.Enabled = false;

            txt_HoTen.Enabled = false;
            txt_MatKhau.Enabled = false;
            txt_NhapLaiMatKhau.Enabled = false;
            txt_SoDienThoai.Enabled = false;
            txt_TenDangNhap.Enabled = false;
            cbo_ChucVu.Enabled = false;
            rdb_Nam.Enabled = false;
            rdb_Nu.Enabled = false;

            if (dgv_DSNhanVien.CurrentRow != null && dgv_DSNhanVien.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dgv_DSNhanVien.CurrentRow;

                manv = int.Parse(row.Cells["MaNhanVien"].Value?.ToString());
                tentk = row.Cells["TenDangNhap"].Value?.ToString();

                if(acbll.KiemTraNhanVienCoHoaDon(manv))
                {
                    MessageBox.Show("Không thể xóa nhân viên vì tài khoản này có lưu hóa đơn của khách hàng");
                    return;
                }    


            }

            if (acbll_delete.Delete_NhanVienTaiKhoan(tentk, manv))
            {
                dgv_DSNhanVien.DataSource = acbll_delete.Get_NhanVien_TaiKhoan();
                dgv_DSNhanVien.DataBindingComplete += (s, e2) =>
                {
                    dgv_DSNhanVien.Columns["Column25"].DataPropertyName = null;
                    GanSoThuTu(dgv_DSNhanVien);
                };
            }
            else
            {
                MessageBox.Show("Lỗi: không thể xóa", "Thông báo");
            }


        }

        private void Txt_TenDangNhap_TextChanged(object sender, EventArgs e)
        {
            lb_Error.Visible = false;
            Account_BLL acbll_check = new Account_BLL();


            string tendangnhap = txt_TenDangNhap.Text.ToString();
            if (acbll_check.KiemTraKhongTrung(tendangnhap))
            {
                lb_Error.Visible = false;
            }
            else
            {
                lb_Error.Text = "Tên đăng nhập đã tồn tại";
                lb_Error.Visible = true;

            }
        }

        private void Btn_huy_Click(object sender, EventArgs e)
        {
            dgv_DSNhanVien.ClearSelection();
            dgv_DSNhanVien.Rows[0].Selected = true;
            dgv_DSNhanVien.CurrentCell = dgv_DSNhanVien.Rows[0].Cells[0];

            /// ẩn các text
            txt_HoTen.Enabled = false;
            txt_MatKhau.Enabled = false;
            txt_NhapLaiMatKhau.Enabled = false;
            txt_SoDienThoai.Enabled = false;
            txt_TenDangNhap.Enabled = false;
            cbo_ChucVu.Enabled = false;
            rdb_Nam.Enabled = false;
            rdb_Nu.Enabled = false;
            ///show nut
            btn_Them.Enabled = true;
            btn_Xoa.Enabled = true;
            btn_Sua.Enabled = true;
            /// ẩn nút
            btn_luu.Enabled = false;
            btn_huy.Enabled = false;


        }
        private bool CheckNumberPhone(string sdt)
        {
            if (!String.IsNullOrWhiteSpace(sdt) && sdt.Length == 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LaMatKhauHopLe(string matKhau)
        {
            if (matKhau.Length < 9) return false;

            bool coChuThuong = Regex.IsMatch(matKhau, "[a-z]");
            bool coChuHoa = Regex.IsMatch(matKhau, "[A-Z]");
            bool coSo = Regex.IsMatch(matKhau, "[0-9]");
            bool coKyTuDacBiet = Regex.IsMatch(matKhau, "[^a-zA-Z0-9]");

            return coChuThuong && coChuHoa && coSo && coKyTuDacBiet;
        }

        private void Btn_luu_Click(object sender, EventArgs e)
        {

            if (btn_Them.Enabled == true)
            {
                Account_BLL acbll_insert = new Account_BLL();

                if (!string.IsNullOrWhiteSpace(txt_HoTen.Text.ToString()) && txt_SoDienThoai.Text.Length > 0 && !string.IsNullOrWhiteSpace(txt_TenDangNhap.Text.ToString()) && !string.IsNullOrWhiteSpace(txt_MatKhau.Text.ToString()) && !string.IsNullOrWhiteSpace(txt_NhapLaiMatKhau.Text.ToString()))
                {
                    if (CheckNumberPhone(txt_SoDienThoai.Text.ToString()))
                    {
                        NhanVien nv = new NhanVien
                        {
                            HoTen = txt_HoTen.Text.Trim().ToString(),
                            GioiTinh = rdb_Nam.Checked ? "Nam" : "Nữ",
                            ChucVu = cbo_ChucVu.SelectedItem.ToString(),
                            SoDienThoai = txt_SoDienThoai.Text.Trim().ToString(),
                            NgayVaoLam = DateTime.Now,
                        };

                        Account_BLL nv_bll = new Account_BLL();
                        int maNV = nv_bll.ThemNhanVien(nv);

                        if (maNV > 0)
                        {
                            //check trùng tên đăng nhập
                            if (lb_Error.Visible == true)
                            {
                                return;
                            }

                            //check độ dài pass khong hợp lệ
                            if (LaMatKhauHopLe(txt_MatKhau.Text.ToString()) == false)
                            {
                                lb_Error.Text = "Độ dai > 8 ký tự bao gồm chữ hoa, thường,  số, ký tư đặc biệt";
                                lb_Error.Visible = true;
                                return;
                            }
                            else
                            {
                                lb_Error.Visible = false;
                            }

                            //check chưa trùng pass
                            if (txt_MatKhau.Text.ToString() != txt_NhapLaiMatKhau.Text.ToString())
                            {
                                lb_Error.Text = "Nhập lại mật khẩu không khớp";
                                txt_NhapLaiMatKhau.Focus();
                                lb_Error.Visible = true;
                                return;
                            }

                            lb_Error.Visible = false;

                            TaiKhoan tk = new TaiKhoan
                            {
                                TenDangNhap = txt_TenDangNhap.Text.Trim(),
                                //MatKhau = BamSHA256(txt_MatKhau.Text.Trim().ToString()),
                                MatKhau = txt_MatKhau.Text.Trim().ToString(),
                                MaNhanVien = maNV
                            };

                            bool taoTK = acbll_insert.ThemTaiKhoan(tk);

                            if (taoTK)
                            {
                                txt_MatKhau.Clear();
                                txt_NhapLaiMatKhau.Clear();
                                txt_TenDangNhap.Clear();
                                MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
           

                                dgv_DSNhanVien.DataSource = acbll_insert.Get_NhanVien_TaiKhoan();
                            }
                            else
                            {
                                MessageBox.Show("Tạo tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            lb_Error.Text = "Lỗi thông tin nhân viên";
                            lb_Error.Visible = true;
                        }
                    }
                    else
                    {
                        lb_Error.Text = "Số điện thoại không hợp lệ ";
                        lb_Error.Visible = true;
                    }

                }
                else
                {
                    lb_Error.Text = "Vui lòng điền đầy đủ thông tin";
                    lb_Error.Visible = true;
                }
            }
            // === CHẾ ĐỘ SỬA ===

            else if (btn_Sua.Enabled == true)
            {
                Account_BLL acbll_edit = new Account_BLL();

                if (txt_HoTen.Tag != null && txt_HoTen.Tag.ToString() == "edit")
                {
                    if (string.IsNullOrWhiteSpace(txt_HoTen.Text) || string.IsNullOrWhiteSpace(txt_SoDienThoai.Text))
                    {
                        lb_Error.Text = "Vui lòng nhập đầy đủ thông tin.";
                        lb_Error.Visible = true;
                        return;
                    }

                    if (!CheckNumberPhone(txt_SoDienThoai.Text))
                    {
                        lb_Error.Text = "Số điện thoại không hợp lệ.";
                        lb_Error.Visible = true;
                        return;
                    }

                    DataGridViewRow row = dgv_DSNhanVien.CurrentRow;
                    if (row == null)
                    {
                        MessageBox.Show("Vui lòng chọn nhân viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int maNV = Convert.ToInt32(row.Cells["MaNhanVien"].Value);

                    NhanVien nvSua = new NhanVien
                    {
                        MaNhanVien = maNV,
                        HoTen = txt_HoTen.Text.Trim(),
                        GioiTinh = rdb_Nam.Checked ? "Nam" : "Nữ",
                        ChucVu = cbo_ChucVu.SelectedItem.ToString(),
                        SoDienThoai = txt_SoDienThoai.Text.Trim()
                    };

                    bool kq = acbll_edit.Sua_NhanVien(nvSua);
                    Account_BLL acbll_update = new Account_BLL();

                    if (kq)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Reset form
                        txt_HoTen.Enabled = false;
                        txt_SoDienThoai.Enabled = false;
                        cbo_ChucVu.Enabled = false;
                        rdb_Nam.Enabled = false;
                        rdb_Nu.Enabled = false;
                        txt_HoTen.Tag = null;


                        // Load lại danh sách
                        danhSachNhanVien = acbll_update.Get_NhanVien_TaiKhoan();
                        dgv_DSNhanVien.DataSource = null;
                        dgv_DSNhanVien.DataSource = danhSachNhanVien;
                        GanSoThuTu(dgv_DSNhanVien);
                        
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
            }
            else
            {
                MessageBox.Show("Lỗi: không xác định thao tác");
            }

        }

        private void Btn_Them_Click(object sender, EventArgs e)
        {
            lb_Error.Visible = false;

            dgv_DSNhanVien.ClearSelection();

            btn_luu.Enabled = true;
            btn_huy.Enabled = true;

            btn_Xoa.Enabled = false;
            btn_Sua.Enabled = false;

            txt_HoTen.Clear();
            txt_MatKhau.Clear();
            txt_NhapLaiMatKhau.Clear();
            txt_SoDienThoai.Clear();
            txt_TenDangNhap.Clear();
            rdb_Nam.Checked = true;
            cbo_ChucVu.SelectedIndex = 0;

            txt_HoTen.Enabled = true;
            txt_MatKhau.Enabled = true;
            txt_NhapLaiMatKhau.Enabled = true;
            txt_SoDienThoai.Enabled = true;
            txt_TenDangNhap.Enabled = true;
            cbo_ChucVu.Enabled = true;
            rdb_Nam.Enabled = true;
            rdb_Nu.Enabled = true;



        }

        private void Cbo_loc_SelectedIndexChanged(object sender, EventArgs e)
        {
          

            lb_Error.Visible = false;

            if (isFormLoading) return;

            if (cbo_loc.SelectedItem == null) return;
            cbo_SapXep.SelectedIndex = 0;


            string selected = cbo_loc.SelectedItem.ToString().Trim();
            List<NhanVien_TaiKhoan> dsLoc;

            switch (selected)
            {
                case "Nhân viên":
                    dsLoc = danhSachNhanVien.Where(nv => nv.ChucVu == "Nhân viên").ToList();
                    break;
                case "Quản lý":
                    dsLoc = danhSachNhanVien.Where(nv => nv.ChucVu == "Quản lý").ToList();
                    break;
                case "Nam":
                    dsLoc = danhSachNhanVien.Where(nv => nv.GioiTinh == "Nam").ToList();
                    break;
                case "Nữ":
                    dsLoc = danhSachNhanVien.Where(nv => nv.GioiTinh == "Nữ").ToList();
                    break;
                default:
                    dsLoc = new List<NhanVien_TaiKhoan>(danhSachNhanVien);
                    break;
            }

            dgv_DSNhanVien.DataSource = null;
            dgv_DSNhanVien.DataSource = dsLoc;
            GanSoThuTu(dgv_DSNhanVien);
        }

        private void Cbo_SapXep_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (isFormLoading) return;

            if (cbo_SapXep.SelectedItem == null) return;
            //cbo_loc.SelectedIndex = 0;


            string selected = cbo_SapXep.SelectedItem.ToString().Trim();
            List<NhanVien_TaiKhoan> dsSapXep;

            if (selected.Equals("Tăng dần theo tên", StringComparison.OrdinalIgnoreCase))
            {
                dsSapXep = danhSachNhanVien.OrderBy(nv => nv.HoTen).ToList();
            }
            else if (selected.Equals("Giảm dần theo tên", StringComparison.OrdinalIgnoreCase))
            {
                dsSapXep = danhSachNhanVien.OrderByDescending(nv => nv.HoTen).ToList();
            }
            else
            {
                dsSapXep = new List<NhanVien_TaiKhoan>(danhSachNhanVien);
            }

            dgv_DSNhanVien.DataSource = null;
            dgv_DSNhanVien.DataSource = dsSapXep;
            GanSoThuTu(dgv_DSNhanVien);
        }


        private void Dgv_DSNhanVien_SelectionChanged(object sender, EventArgs e)
        {

            btn_luu.Enabled = false;
            btn_huy.Enabled = false;

            btn_Them.Enabled = true;
            btn_Xoa.Enabled = true;
            btn_Sua.Enabled = true;

            if (dgv_DSNhanVien.CurrentRow != null && dgv_DSNhanVien.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dgv_DSNhanVien.CurrentRow;

                txt_HoTen.Text = row.Cells["HoTen"].Value?.ToString();
                if (row.Cells["GioiTinh"].Value?.ToString() == "Nam")
                {
                    rdb_Nam.Checked = true;
                }
                else
                {
                    rdb_Nu.Checked = true;
                }
                cbo_ChucVu.SelectedItem = row.Cells["ChucVu"].Value?.ToString();
                txt_SoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txt_TenDangNhap.Text = row.Cells["TenDangNhap"].Value?.ToString();
                txt_MatKhau.Text = "00000000";
                txt_NhapLaiMatKhau.Text = "00000000";

            }
        }

        private void Btn_DangXuat_Click(object sender, EventArgs e)
        {

            DialogResult r;
            r = MessageBox.Show("Bạn muốn đăng xuất?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                this.Hide();
                dangnhap.Show();
            }
                
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            dangnhap.Close();
        }
        public void GanSoThuTu(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells["Column25"].Value = (i + 1).ToString();
            }
        }

        //-----------------------------------------------------------------Trang Dashboard

       
        private void VeBieuDoTrangThaiBanAn(DataTable dt)
        {
            chart_BanAn.Series.Clear();
            chart_BanAn.Titles.Clear();

            Series series = new Series("Trạng thái bàn");
            series.ChartType = SeriesChartType.Pie;
            series.IsValueShownAsLabel = true;

            foreach (DataRow row in dt.Rows)
            {
                string trangThai = row["TrangThai"].ToString();
                int soLuong = Convert.ToInt32(row["SoLuong"]);
                series.Points.AddXY(trangThai, soLuong);
            }

            chart_BanAn.Series.Add(series);
            chart_BanAn.Titles.Add("Biểu đồ trạng thái bàn ăn");
        }

        private void VeBieuDoDoanhThuTheoNgay(DataTable dt)
        {
            decimal tongDoanhThu = 0;

            chart_DoanhThuTrongNgay.Series.Clear();
            chart_DoanhThuTrongNgay.Titles.Clear();

            Series series = new Series("Doanh thu");
            series.ChartType = SeriesChartType.Line;  // Chuyển sang biểu đồ đường
            series.Color = Color.SteelBlue;
            series.BorderWidth = 2;                   // Độ dày đường
            series.IsValueShownAsLabel = true;        // Hiển thị giá trị trên điểm

            foreach (DataRow row in dt.Rows)
            {
                DateTime ngay = Convert.ToDateTime(row["ThoiGianLap"]);
                decimal doanhThu = Convert.ToDecimal(row["ThanhToan"]);
                tongDoanhThu += doanhThu;

                // Hiển thị nhãn theo giờ: "HH:00"
                series.Points.AddXY(ngay.Hour.ToString("00") + ":00", doanhThu);
            }

            chart_DoanhThuTrongNgay.Series.Add(series);
            chart_DoanhThuTrongNgay.ChartAreas[0].AxisX.Title = "Giờ trong ngày";
            chart_DoanhThuTrongNgay.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
            chart_DoanhThuTrongNgay.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0";
            chart_DoanhThuTrongNgay.ChartAreas[0].AxisX.Interval = 1; // 1 giờ 1 nhãn

            lb_TongDoanhThu.Text = $"{tongDoanhThu:#,##0} VNĐ";
        }





        private void LoadBanAnTrong()
        {
            BanAnBLL ba = new BanAnBLL();
            dvg_BanAnTrong.DataSource = ba.LayBanAn_Trong().OrderBy(x => x.Tang).ToList();
            dvg_BanAnTrong.ReadOnly = true;

        }
        

        private void LoadMonAnCon()
        {
           
            MonAn_BLL monanbll = new MonAn_BLL();
            List<MonAn> ds = monanbll.GetAllMonAnCon();

            DataTable dt = new DataTable();
            dt.Columns.Add("MaMon1", typeof(int));
            dt.Columns.Add("HinhAnh1", typeof(Image));
            dt.Columns.Add("TenMon1", typeof(string));
            dt.Columns.Add("MoTa1", typeof(string));
            dt.Columns.Add("DonGia1", typeof(decimal));
            dt.Columns.Add("LoaiMon1", typeof(string));
            dt.Columns.Add("SoLuong1", typeof(int));

            foreach (var item in ds)
            {
                Image img = null;
                if (!string.IsNullOrEmpty(item.HinhAnh))
                {
                    string path = Path.Combine(Application.StartupPath, item.HinhAnh);
                    if (File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            img = Image.FromStream(fs);
                        }
                    }
                }

                dt.Rows.Add(item.MaMon, img, item.TenMon, item.MoTa, item.DonGia, item.LoaiMon, item.SoLuong);
            }

            dgv_MonAnCon.DataSource = dt;

            dgv_MonAnCon.RowTemplate.Height = 80;
            DataGridViewImageColumn imgCol = (DataGridViewImageColumn)dgv_MonAnCon.Columns["HinhAnh1"];
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imgCol.Width = 100;
        }

        ///---------------------------------------------------------------Form Load
        private void Dashboard_Load(object sender, EventArgs e)
        {
            isFormLoading = true;
         


            ///dashboard
            string lb = "Xin chào! " + nv.ChucVu + " - " + nv.HoTen;
            lb_Name_role.Text = lb;
            uC_DatBan1.NhanVienDangNhap = this.nv;


            ThongKe_BLL tk = new ThongKe_BLL();
            VeBieuDoTrangThaiBanAn(tk.ThongKeTrangThaiBanAn());


            VeBieuDoDoanhThuTheoNgay(tk.LayHoaDonTheoNgay(DateTime.Now, DateTime.Now));
            LoadBanAnTrong();
            LoadMonAnCon();

            dgv_MonAnCon.ReadOnly = true;





            //Phaan quyền

            if (nv.ChucVu.ToString() == "Nhân viên")
            {
                guna2TabControl1.TabPages.Remove(tab_NhanVien);
                guna2TabControl1.TabPages.Remove(tab_ThongKe);

            }


            isFormLoading = false;

            /// ẩn các text
            txt_HoTen.Enabled = false;
            txt_MatKhau.Enabled = false;
            txt_NhapLaiMatKhau.Enabled = false;
            txt_SoDienThoai.Enabled = false;
            txt_TenDangNhap.Enabled = false;
            cbo_ChucVu.Enabled = false;
            rdb_Nam.Enabled = false;
            rdb_Nu.Enabled = false;
            ///show nut
            btn_Them.Enabled = true;
            btn_Xoa.Enabled = true;
            btn_Sua.Enabled = true;
            /// ẩn nút
            btn_luu.Enabled = false;
            btn_huy.Enabled = false;

            lb_Error.Visible = false;



            //// trang Món ăn
            ///

            input_text_enable(false);

        }

        private void uC_KhuyenMai1_Load(object sender, EventArgs e)
        {

        }

        private void guna2TabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void tab_ThongKe_DanhSachHoaDon_Click(object sender, EventArgs e)
        {

        }
    }
}
