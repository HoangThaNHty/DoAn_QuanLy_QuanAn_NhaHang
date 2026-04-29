using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using BLL;
using System.Text.RegularExpressions;


namespace QUANLY_NHAHANG
{
    public partial class DangKy_Account : Form
    {
        public int manv;
        public Account_BLL acc_bll = new Account_BLL();
        private Form dangki_info;

        public DangKy_Account(int manv, Form dangki)
        {
            InitializeComponent();
            this.manv = manv;
            this.Load += DangKy_Account_Load;
            this.FormClosing += DangKy_Account_FormClosing;
            btn_DangKy.Click += Btn_DangKy_Click;
            txt_TenDangNhap.TextChanged += Txt_TenDangNhap_TextChanged;
            dangki_info = dangki;

        }

        private void Txt_TenDangNhap_TextChanged(object sender, EventArgs e)
        {
            string tendangnhap = txt_TenDangNhap.Text.ToString();
            if (acc_bll.KiemTraKhongTrung(tendangnhap))
            {
                lb_error_TenDangNhap.Visible = false;
            }
            else
            {
                lb_error_TenDangNhap.Visible = true;

            }
        }

        private void DangKy_Account_FormClosing(object sender, FormClosingEventArgs e)
        {
            dangki_info.Close();
   
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

        private void Btn_DangKy_Click(object sender, EventArgs e)
        {
            if((!String.IsNullOrWhiteSpace(txt_MatKhau.Text)) && (!String.IsNullOrWhiteSpace(txt_NhapLaiMatKhau.Text)) && (!String.IsNullOrWhiteSpace(txt_TenDangNhap.Text)))
            {

                //check trùng tên đăng nhập
                if (lb_error_TenDangNhap.Visible == true)
                {
                    return;
                }                

                //check độ dài pass khong hợp lệ
                if (LaMatKhauHopLe(txt_MatKhau.Text.ToString()) == false)
                {
                    lb_Error_MatKhau.Visible = true;
                    return;
                }

                //check chưa trùng pass
                if (txt_MatKhau.Text.ToString() != txt_NhapLaiMatKhau.Text.ToString())
                {
                    lb_Error_NhapLaiMatKhau.Visible = true;
                    return;
                }

                lb_Error_MatKhau.Visible = false;
                lb_Error_NhapLaiMatKhau.Visible = false;
                lb_error_TenDangNhap.Visible = false;
                lb_Error_DienDauDu.Visible = false;


                TaiKhoan tk = new TaiKhoan
                {
                    TenDangNhap = txt_TenDangNhap.Text.Trim(),
                    MatKhau = txt_MatKhau.Text.Trim().ToString(),
                    MaNhanVien = manv
                };

                bool taoTK = acc_bll.ThemTaiKhoan(tk);

                if (taoTK)
                {
                    txt_MatKhau.Clear();
                    txt_NhapLaiMatKhau.Clear();
                    txt_TenDangNhap.Clear();
                    MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tạo tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                lb_Error_DienDauDu.Visible = true;
            }
        }
        
        private void DangKy_Account_Load(object sender, EventArgs e)
        {
            lb_Error_MatKhau.Visible = false;
            lb_Error_NhapLaiMatKhau.Visible = false;
            lb_error_TenDangNhap.Visible = false;   
            lb_Error_DienDauDu.Visible = false;
        }

        //public string BamSHA256(string matKhau)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] bytes = Encoding.UTF8.GetBytes(matKhau);
        //        byte[] hash = sha256.ComputeHash(bytes);

        //        StringBuilder sb = new StringBuilder();
        //        foreach (byte b in hash)
        //        {
        //            sb.Append(b.ToString("x2")); 
        //        }
        //        return sb.ToString();
        //    }
        //}



    }
}
