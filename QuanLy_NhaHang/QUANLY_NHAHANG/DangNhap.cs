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
using BLL;
using DTO;

namespace QUANLY_NHAHANG
{
    public partial class DangNhap : Form
    {
        public NhanVien nv = new NhanVien();
        public DangNhap()
        {
            InitializeComponent();
            btn_DangNhap.Click += Btn_DangNhap_Click;
            this.Load += DangNhap_Load;
            btn_ChuaCoTaiKhoan.Click += Btn_ChuaCoTaiKhoan_Click;
            txt_MatKhau.KeyDown += Txt_MatKhau_KeyDown;
        }

        private void Txt_MatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_DangNhap.PerformClick(); 
            }


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


        private void Btn_ChuaCoTaiKhoan_Click(object sender, EventArgs e)
        {
            DangKy_Info dk_info = new DangKy_Info(this);
            dk_info.Show();
            this.Hide();
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {
            lb_Error_DangNhap.Visible = false;
        }

        private void Btn_DangNhap_Click(object sender, EventArgs e)
        {
            Account_BLL accountBLL = new Account_BLL();
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = txt_TenDangNhap.Text.ToString();
            tk.MatKhau = txt_MatKhau.Text.Trim().ToString();
            tk.MaNhanVien = 0;

            nv = accountBLL.DangNhap(tk);

            if (nv != null)
            {
                lb_Error_DangNhap.Visible = false;
                txt_MatKhau.Clear();
                txt_TenDangNhap.Clear();
                Dashboard db = new Dashboard(nv, this);
                db.Show();
                this.Hide();
                //MessageBox.Show($"Xin chào {nv.HoTen} - {nv.ChucVu}", "Đăng nhập thành công");
            }
            else
            {
                //MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu", "Lỗi đăng nhập");
                lb_Error_DangNhap.Visible = true;

            }
        }

     
    }
}
