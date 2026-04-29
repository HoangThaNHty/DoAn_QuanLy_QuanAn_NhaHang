using System;
using System.Data;
using System.Windows.Forms;
using BLL;
using DTO;

namespace QUANLY_NHAHANG
{
    public partial class UC_ThanhToan : UserControl
    {
        private HoaDonBLL hoaDonBLL = new HoaDonBLL();
        private BanAnBLL banAnBLL = new BanAnBLL();
        private MonAn_BLL monAnBLL = new MonAn_BLL();
        
        private int maBanSelected = 0;
        private int maHoaDonSelected = 0;
        private decimal tongTienGoc = 0;

        public UC_ThanhToan()
        {
            InitializeComponent();
            this.Load += UC_ThanhToan_Load;
            
            dgv_DanhSachBan.SelectionChanged += Dgv_DanhSachBan_SelectionChanged;
            txt_TienKhachDua.TextChanged += Txt_TienKhachDua_TextChanged;
            txt_TienKhachDua.KeyPress += Txt_TienKhachDua_KeyPress;
            btn_ApDung.Click += Btn_ApDung_Click;
            btn_ThanhToan.Click += Btn_ThanhToan_Click;
        }

        private void Btn_ThanhToan_Click(object sender, EventArgs e)
        {
            if(txt_MaHoaDon.Text.Length <=0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn");
                return;
            }    

            if(txt_TongTien.Text.Length <=0 )
            {
                MessageBox.Show("Chưa có tổng tiền để thanh toán");
                return;
            }    

            int mahd = int.Parse(txt_MaHoaDon.Text.ToString());
            decimal tongtien = decimal.Parse(txt_TongTien.Text.ToString());
            int giamgia = 0;
            decimal thanhtoan = tongtien;
            int maban = 0;
            

            // lấy mã bàn cho cập nhật trang thái bàn ăn thành tronngd
            if (dgv_DanhSachBan.CurrentRow != null)
            {
               maban = int.Parse(dgv_DanhSachBan.CurrentRow.Cells["MaBan"].Value.ToString());
            }
            else
            {
                MessageBox.Show("Không thể lấy bàn ăn");
                return;
            }

            if(txt_TienKhachDua.Text.Length <=0)
            {
                MessageBox.Show("Vui lòng nhập tiền khách hàng");
                txt_TienKhachDua.Focus();
                return;
            }    


            if (txt_GiamGia.Text.Length > 0)
            {
                VoucherBLL vc = new VoucherBLL();
                Voucher v = vc.LayKhuyenMaiTheoTen(txt_GiamGia.Text.ToString());

                if (v != null)
                {
                    DateTime today = DateTime.Now;

                    if (today >= v.NgayBatDau && today <= v.NgayKetThuc)
                    {
                        giamgia = v.PhanTramGiam;
                        decimal giam = tongtien * giamgia / 100;
                        thanhtoan = tongtien - giam;
                    }
                }      
            }


            if (int.Parse(txt_TienKhachDua.Text.ToString()) < thanhtoan)
            {
                MessageBox.Show("Khách hàng chưa đưa đủ");
                return;
            }

            HoaDonBLL hd = new HoaDonBLL();
            bool result = hd.CapNhatHoaDon(mahd, tongtien, giamgia, thanhtoan);
            if (result)
            {
                BanAnBLL ba = new BanAnBLL();
                MessageBox.Show("Thanh toán thành công");

                dgv_ChiTietHoaDon.DataSource = null;

                LoadDanhSachBan();


                bool re = ba.CapNhatBan_ThanhTrong(maban);
                if (!re)
                {
                    MessageBox.Show("Không thể cập nhật trạng thái bàn ăn");
                    return ;
                }

                //Reload();

               
            }
            else
            {
                MessageBox.Show("Thanh toán thất bại");
            }

        }

        private void Btn_ApDung_Click(object sender, EventArgs e)
        {
            if (txt_GiamGia.Text.Length > 0)
            {
                VoucherBLL vc = new VoucherBLL();
                Voucher v = vc.LayKhuyenMaiTheoTen(txt_GiamGia.Text.ToString());

                if (v != null)
                {
                    DateTime today = DateTime.Now;

                    if (today >= v.NgayBatDau && today <= v.NgayKetThuc)
                    {
                        int phantram = v.PhanTramGiam;

                        decimal tongTien = decimal.Parse(txt_TongTien.Text);
                        decimal tienGiam = tongTien * phantram / 100;
                        decimal tienSauGiam = tongTien - tienGiam;

                        txt_ConLai.Text = tienSauGiam.ToString("N0");
                        return;
                       
                    }
                    else
                    {
                        MessageBox.Show("Mã giảm giá đã hết hạn hoặc chưa bắt đầu.");
                    }
                }
                else
                {
                    MessageBox.Show("Không tồn tại mã giảm giá này.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã giảm giá.");
                txt_GiamGia.Focus();
            }

            txt_ConLai.Text = txt_TongTien.Text;
        }

        private void Txt_TienKhachDua_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            
        }

        private void Txt_TienKhachDua_TextChanged(object sender, EventArgs e)
        {
            decimal tienkhachdua, tongtien;

            if (decimal.TryParse(txt_TienKhachDua.Text, out tienkhachdua) &&
                decimal.TryParse(txt_ConLai.Text, out tongtien))
            {
                decimal tienthua = tienkhachdua - tongtien;
                txt_TienThua.Text = tienthua.ToString("N0");
            }
            else
            {
                txt_TienThua.Text = "0";
            }
        }

        private void UC_ThanhToan_Load(object sender, EventArgs e)
        {
            LoadDanhSachBan();

        }

        private void LoadDanhSachBan()
        {
           HoaDonBLL hd = new HoaDonBLL();
            dgv_DanhSachBan.DataSource = hd.GetHoaDonChuaThanhToan();
        }


       
        private void Dgv_DanhSachBan_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_DanhSachBan.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgv_DanhSachBan.SelectedRows[0];
                int mahd = Convert.ToInt32(row.Cells["MaHoaDon"].Value);

                //fill vào chi tiết
                CTHD_BLL cthd = new CTHD_BLL();
                dgv_ChiTietHoaDon.DataSource = cthd.GetChiTietHoaDon(mahd);
           
                //Fill vào thanh toán
                KhachHangBLL kh = new KhachHangBLL();

                string Tenkhachhang = kh.GetTenKhachHangTuHoaDon(mahd);
                txt_TenKhachHang.Text = Tenkhachhang;
                txt_MaHoaDon.Text = mahd.ToString();
                
                HoaDonBLL hd = new HoaDonBLL();
                decimal tongtien = hd.TinhTongTienHoaDon(mahd);
                txt_TongTien.Text = tongtien.ToString("N0");
                txt_ConLai.Text = tongtien.ToString("N0");


            }
        }


        public void Reload()
        {


            //dgv_ChiTietHoaDon.DataSource = null;

            DataTable dt = dgv_ChiTietHoaDon.DataSource as DataTable;
            if (dt != null)
            {
                dt.Clear();

            }

            LoadDanhSachBan();

            //dgv_ChiTietHoaDon.Rows.Clear();

            txt_TenKhachHang.Clear();
            txt_MaHoaDon.Clear();
            txt_TongTien.Clear();
            txt_ConLai.Clear();
            txt_TienKhachDua.Clear();
            txt_TienThua.Clear();
            txt_GiamGia.Clear();

            maBanSelected = 0;
            maHoaDonSelected = 0;
            tongTienGoc = 0;

            dgv_DanhSachBan.ClearSelection();
        }



    }
}
