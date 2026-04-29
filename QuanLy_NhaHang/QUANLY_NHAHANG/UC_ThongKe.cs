using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BLL;
using DTO;

namespace QUANLY_NHAHANG
{
    public partial class UC_ThongKe : UserControl
    {
        public UC_ThongKe()
        {
            InitializeComponent();
            this.Load += UC_ThongKe_Load;
            btn_ThongKe.Click += Btn_ThongKe_Click;
 
            
        }

        private void Btn_ThongKe_Click(object sender, EventArgs e)
        {
            string selected = cbo_LoaiThongKe.SelectedItem.ToString();
            DateTime tuNgay = dtp_TuNgay.Value;
            DateTime denNgay = dtp_DenNgay.Value;
            ThongKe_BLL tk = new ThongKe_BLL();

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selected == "Doanh thu theo ngày")
            {
                // thống kê hóa đơn
                DataTable dt = tk.ThongKeDoanhThuTheoNgay(tuNgay, denNgay);
                VeBieuDoDoanhThuTheoNgay(dt);
                decimal tong = 0;
                int sl = 0;
                foreach (DataRow row in dt.Rows)
                {
                    tong += Convert.ToDecimal(row["TongDoanhThu"]);
                    sl += Convert.ToInt32(row["SoHoaDon"]);
                }
                lb_TongDoanhThu.Text = $"{tong:#,##0} VNĐ";
                lb_SoHoaDon.Text = sl.ToString();
                if (sl > 0)
                {
                    decimal trungBinh = tong / sl;
                    lb_DoanhThuTrungBinh.Text = $"{trungBinh:#,##0} VNĐ";
                }
                else
                {
                   
                    lb_DoanhThuTrungBinh.Text = "0";
                }

                


            }
            else if (selected == "Doanh thu theo tháng")
            {
                DataTable dt = tk.LayDoanhThuTheoKhoangThang(tuNgay, denNgay);
                VeBieuDoDoanhThuTheoThang(dt);

                decimal tong = 0;
                int sl = 0;
                foreach (DataRow row in dt.Rows)
                {
                    tong += Convert.ToDecimal(row["TongDoanhThu"]);
                    sl += Convert.ToInt32(row["SoHoaDon"]);
                }
                lb_TongDoanhThu.Text = $"{tong:#,##0} VNĐ";
                lb_SoHoaDon.Text = sl.ToString();
                if (sl > 0)
                {
                    decimal trungBinh = tong / sl;
                    lb_DoanhThuTrungBinh.Text = $"{trungBinh:#,##0} VNĐ";
                }
                else
                {

                    lb_DoanhThuTrungBinh.Text = "0";
                }
            }

            //top 5 món bán chạy

            DataTable banchay = tk.ThongKeTopMonAn(tuNgay, denNgay);
            VeBieuDoTopMonAn(banchay);

            //top 1 món bán hchayj
            string tenMon = tk.ThongKeMonAnBanChayNhat(dtp_TuNgay.Value, dtp_DenNgay.Value);
            lb_MonBanChay.Text = $"{tenMon}";
        }
        private void VeBieuDoDoanhThuTheoThang(DataTable dt)
        {
            chart_DoanhThu.Series.Clear();
            chart_DoanhThu.Titles.Clear();

            Series series = new Series("Doanh thu");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.DarkOrange;
            series.IsValueShownAsLabel = true;

            foreach (DataRow row in dt.Rows)
            {
                string thang = row["ThangNam"].ToString();
                decimal doanhThu = Convert.ToDecimal(row["TongDoanhThu"]);
                series.Points.AddXY(thang, doanhThu);
            }

            chart_DoanhThu.Series.Add(series);
            chart_DoanhThu.ChartAreas[0].AxisX.Title = "Tháng";
            chart_DoanhThu.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
            chart_DoanhThu.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0";
            chart_DoanhThu.Titles.Add("Biểu đồ doanh thu theo tháng");
        }



        private void UC_ThongKe_Load(object sender, EventArgs e)
        {
            cbo_LoaiThongKe.Items.Clear();
            cbo_LoaiThongKe.Items.Add("Doanh thu theo ngày");
            cbo_LoaiThongKe.Items.Add("Doanh thu theo tháng");
            dtp_TuNgay.Value = DateTime.Now;
            dtp_DenNgay.Value = DateTime.Now;
            cbo_LoaiThongKe.SelectedIndex = 0;

            btn_ThongKe.PerformClick();
        }
        private void VeBieuDoDoanhThuTheoNgay(DataTable dt)
        {
            chart_DoanhThu.Series.Clear();
            chart_DoanhThu.Titles.Clear();

            Series series = new Series("Doanh thu");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.SteelBlue;
            series.IsValueShownAsLabel = true;

            foreach (DataRow row in dt.Rows)
            {
                DateTime ngay = Convert.ToDateTime(row["Ngay"]);
                decimal doanhThu = Convert.ToDecimal(row["TongDoanhThu"]);
                series.Points.AddXY(ngay.ToString("dd/MM"), doanhThu);
            }

            chart_DoanhThu.Series.Add(series);
            chart_DoanhThu.ChartAreas[0].AxisX.Title = "Ngày";
            chart_DoanhThu.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
            chart_DoanhThu.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0";
            chart_DoanhThu.Titles.Add("Biểu đồ doanh thu theo ngày");
        }
        private void VeBieuDoTopMonAn(DataTable dt)
        {
            chart_MonAn.Series.Clear();
            chart_MonAn.Titles.Clear();

            Series series = new Series("Top món ăn");
            series.ChartType = SeriesChartType.Pie;
            series.IsValueShownAsLabel = true;

            foreach (DataRow row in dt.Rows)
            {
                string tenMon = row["TenMon"].ToString();
                int soLuong = Convert.ToInt32(row["TongSoLuong"]);
                series.Points.AddXY(tenMon, soLuong);
            }

            chart_MonAn.Series.Add(series);
            chart_MonAn.Titles.Add("Top món ăn bán chạy");
        }

        //        private void LoadData()
        //        {
        //            try
        //            {
        //                // Khởi tạo date range mặc định
        //                dtp_TuNgay.Value = DateTime.Now.AddMonths(-1);
        //                dtp_DenNgay.Value = DateTime.Now;

        //                // Load dữ liệu ban đầu
        //                LoadDoanhThuTheoNgay();
        //                LoadDoanhThuTheoNhanVien();
        //                LoadMonAnBanChay();
        //                LoadBieuDoDoanhThu();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", 
        //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }

        //        private void LoadDoanhThuTheoNgay()
        //        {
        //            try
        //            {
        //                DateTime tuNgay = dtp_TuNgay.Value.Date;
        //                DateTime denNgay = dtp_DenNgay.Value.Date.AddDays(1).AddSeconds(-1);

        //                // Giả lập dữ liệu - trong thực tế sẽ gọi từ BLL
        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("Ngày", typeof(DateTime));
        //                dt.Columns.Add("SoHoaDon", typeof(int));
        //                dt.Columns.Add("DoanhThu", typeof(decimal));

        //                // Thêm dữ liệu mẫu
        //                Random rnd = new Random();
        //                for (DateTime date = tuNgay; date <= denNgay; date = date.AddDays(1))
        //                {
        //                    dt.Rows.Add(date, rnd.Next(10, 50), rnd.Next(1000000, 5000000));
        //                }

        //                dgv_ChiTiet.DataSource = dt;

        //                // Format columns
        //                if (dgv_ChiTiet.Columns["Ngày"] != null)
        //                    dgv_ChiTiet.Columns["Ngày"].DefaultCellStyle.Format = "dd/MM/yyyy";
        //                if (dgv_ChiTiet.Columns["DoanhThu"] != null)
        //                {
        //                    dgv_ChiTiet.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
        //                    dgv_ChiTiet.Columns["DoanhThu"].HeaderText = "Doanh Thu (VNĐ)";
        //                }
        //                if (dgv_ChiTiet.Columns["SoHoaDon"] != null)
        //                    dgv_ChiTiet.Columns["SoHoaDon"].HeaderText = "Số Hóa Đơn";
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Lỗi khi tải doanh thu theo ngày: " + ex.Message);
        //            }
        //        }

        //        private void LoadDoanhThuTheoNhanVien()
        //        {
        //            try
        //            {
        //                // Giả lập dữ liệu - trong thực tế sẽ gọi từ BLL
        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("MaNhanVien", typeof(int));
        //                dt.Columns.Add("TenNhanVien", typeof(string));
        //                dt.Columns.Add("SoHoaDon", typeof(int));
        //                dt.Columns.Add("DoanhThu", typeof(decimal));

        //                // Thêm dữ liệu mẫu
        //                Random rnd = new Random();
        //                string[] tenNV = { "Nguyễn Văn A", "Trần Thị B", "Lê Văn C", "Phạm Thị D" };
        //                for (int i = 0; i < tenNV.Length; i++)
        //                {
        //                    dt.Rows.Add(i + 1, tenNV[i], rnd.Next(20, 100), rnd.Next(5000000, 20000000));
        //                }

        //                dgv_ChiTiet.DataSource = dt;

        //                // Format columns
        //                if (dgv_ChiTiet.Columns["DoanhThu"] != null)
        //                {
        //                    dgv_ChiTiet.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
        //                    dgv_ChiTiet.Columns["DoanhThu"].HeaderText = "Doanh Thu (VNĐ)";
        //                }
        //                if (dgv_ChiTiet.Columns["TenNhanVien"] != null)
        //                    dgv_ChiTiet.Columns["TenNhanVien"].HeaderText = "Tên Nhân Viên";
        //                if (dgv_ChiTiet.Columns["SoHoaDon"] != null)
        //                    dgv_ChiTiet.Columns["SoHoaDon"].HeaderText = "Số Hóa Đơn";
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Lỗi khi tải doanh thu theo nhân viên: " + ex.Message);
        //            }
        //        }

        //        private void LoadMonAnBanChay()
        //        {
        //            try
        //            {
        //                // Giả lập dữ liệu - trong thực tế sẽ gọi từ BLL
        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("MaMon", typeof(int));
        //                dt.Columns.Add("TenMon", typeof(string));
        //                dt.Columns.Add("SoLuongBan", typeof(int));
        //                dt.Columns.Add("DoanhThu", typeof(decimal));




        //                //string[] tenMon = { "Cá", "Gà", "Bò", "Rau", "Đồ chay", "Thức uống" };
        //                //for (int i = 0; i < tenMon.Length; i++)
        //                //{
        //                //    dt.Rows.Add(i + 1, tenMon[i], rnd.Next(50, 200), rnd.Next(1000000, 5000000));
        //                //}

        //                dgv_ChiTiet.DataSource = dt;

        //                // Format columns
        //                if (dgv_ChiTiet.Columns["DoanhThu"] != null)
        //                {
        //                    dgv_ChiTiet.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
        //                    dgv_ChiTiet.Columns["DoanhThu"].HeaderText = "Doanh Thu (VNĐ)";
        //                }
        //                if (dgv_ChiTiet.Columns["TenMon"] != null)
        //                    dgv_ChiTiet.Columns["TenMon"].HeaderText = "Tên Món Ăn";
        //                if (dgv_ChiTiet.Columns["SoLuongBan"] != null)
        //                    dgv_ChiTiet.Columns["SoLuongBan"].HeaderText = "Số Lượng Bán";
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Lỗi khi tải món ăn bán chạy: " + ex.Message);
        //            }
        //        }

        //        private void LoadBieuDoDoanhThu()
        //        {
        //            try
        //            {
        //                LoadBieuDoCot();
        //                LoadBieuDoTron();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Lỗi khi tải biểu đồ: " + ex.Message);
        //            }
        //        }

        //        private void LoadBieuDoCot()
        //        {
        //            chart_DoanhThu.Series.Clear();
        //            chart_DoanhThu.ChartAreas.Clear();

        //            ChartArea chartArea = new ChartArea("MainArea");
        //            chartArea.AxisX.Title = "Ngày";
        //            chartArea.AxisY.Title = "Doanh Thu (VNĐ)";
        //            chartArea.AxisY.LabelStyle.Format = "N0";
        //            chart_DoanhThu.ChartAreas.Add(chartArea);

        //            Series series = new Series("Doanh Thu");
        //            series.ChartType = SeriesChartType.Column;
        //            series.XValueType = ChartValueType.Date;

        //            // Thêm dữ liệu mẫu
        //            DateTime tuNgay = dtp_TuNgay.Value.Date;
        //            DateTime denNgay = dtp_DenNgay.Value.Date;
        //            Random rnd = new Random();

        //            for (DateTime date = tuNgay; date <= denNgay && series.Points.Count < 30; date = date.AddDays(1))
        //            {
        //                series.Points.AddXY(date, rnd.Next(1000000, 5000000));
        //            }

        //            chart_DoanhThu.Series.Add(series);
        //        }

        //        private void LoadBieuDoTron()
        //        {
        //            chart_MonAn.Series.Clear();
        //            chart_MonAn.ChartAreas.Clear();

        //            ChartArea chartArea = new ChartArea("MainArea");
        //            chart_MonAn.ChartAreas.Add(chartArea);

        //            Series series = new Series("Doanh Thu Theo Danh Mục");
        //            series.ChartType = SeriesChartType.Pie;

        //            // Thêm dữ liệu mẫu
        //            Random rnd = new Random();
        //            string[] danhMuc = { "Món Chính", "Đồ Uống", "Tráng Miệng", "Khai Vị" };
        //            foreach (string dm in danhMuc)
        //            {
        //                DataPoint point = new DataPoint();
        //                point.SetValueY(rnd.Next(1000000, 5000000));
        //                point.Label = dm + "\n#PERCENT{P1}";
        //                point.LegendText = dm;
        //                series.Points.Add(point);
        //            }

        //            chart_MonAn.Series.Add(series);
        //            if (chart_MonAn.Legends.IndexOf("Legend1") < 0)
        //            {
        //                chart_MonAn.Legends.Add(new Legend("Legend1"));
        //            }
        //        }

        //        private void btn_TimKiem_Click(object sender, EventArgs e)
        //        {
        //            LoadData();
        //        }

        //        private void btn_XuatExcel_Click(object sender, EventArgs e)
        //        {
        //            try
        //            {
        //                MessageBox.Show("Chức năng xuất Excel sẽ được hoàn thiện bởi team phát triển", 
        //                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message);
        //            }
        //        }

        //        private void btn_ThongKe_Click(object sender, EventArgs e)
        //        {
        //            LoadData();
        //        }


        public void Reload()
        {
            UC_ThongKe_Load(this, EventArgs.Empty);
        }

    }
}
