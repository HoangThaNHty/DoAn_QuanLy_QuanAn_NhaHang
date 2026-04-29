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
    public partial class DangKy_Info : Form
    {
        private Form formDangNhap;
        public DangKy_Info(Form dangnhap)
        {
            InitializeComponent();
            formDangNhap = dangnhap;
            btn_TLDangNhap.Click += Btn_TLDangNhap_Click;
            this.FormClosing += DangKy_Info_FormClosing;
            btn_TiepTuc.Click += Btn_TiepTuc_Click;
            this.Load += DangKy_Info_Load;
            txt_SoDienThoai.KeyPress += Txt_SoDienThoai_KeyPress;
        }

        private void Txt_SoDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void DangKy_Info_Load(object sender, EventArgs e)
        {
            rdb_Nam.Checked = true;
            lb_Error_DangKyInfo.Visible = false;
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
        private void Btn_TiepTuc_Click(object sender, EventArgs e)
        {
            if (txt_HoTen.Text.Length > 0 && txt_SoDienThoai.Text.Length > 0)
            {
                if(CheckNumberPhone(txt_SoDienThoai.Text.ToString()))
                {
                    NhanVien nv = new NhanVien
                    {
                        HoTen = txt_HoTen.Text.Trim().ToString(),
                        GioiTinh = rdb_Nam.Checked ? "Nam" : "Nữ",
                        ChucVu = "Nhân viên",
                        SoDienThoai = txt_SoDienThoai.Text.Trim().ToString(),
                        NgayVaoLam = DateTime.Now,
                    };

                    Account_BLL nv_bll = new Account_BLL();
                    int maNV = nv_bll.ThemNhanVien(nv);

                    if (maNV > 0)
                    {

                        DangKy_Account dk_ac = new DangKy_Account(maNV, this);
                        dk_ac.Show();

                        this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }    
                else
                {
                    lb_Error_DangKyInfo.Text = "Số điện thoại không hợp lệ ";
                    lb_Error_DangKyInfo.Visible = true;
                }    
                
            }
            else
            {
                lb_Error_DangKyInfo.Text = "Vui lòng điền đầy đủ thông tin";
                lb_Error_DangKyInfo.Visible = true;
            }
            

        }

        private void DangKy_Info_FormClosing(object sender, FormClosingEventArgs e)
        {
            formDangNhap.Show();
        }

       
        private void Btn_TLDangNhap_Click(object sender, EventArgs e)
        {
            formDangNhap.Show();
            this.Close();
        }
    }
}
