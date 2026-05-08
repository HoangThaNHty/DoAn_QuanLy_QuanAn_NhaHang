# 🍔 Restaurant Management - Phần mềm Quản lý Nhà hàng

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue?style=flat-square&logo=.net)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-8.0-purple?style=flat-square&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2012+-orange?style=flat-square&logo=sql)](https://www.microsoft.com/en-us/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)](LICENSE)

Ứng dụng desktop quản lý nhà hàng toàn diện: đặt bàn, gọi món, tính tiền, in hóa đơn, thống kê doanh thu.

## Tính năng

- Quản lý đặt bàn trước theo giờ, ngày
- Gọi món theo bàn, theo khách hàng
- Tính tiền, in hóa đơn (Crystal Reports)
- Quản lý khuyến mãi, voucher giảm giá
- Thống kê doanh thu theo ngày/tháng/năm
- Quản lý khách hàng, nhân viên
- Quản lý món ăn, danh mục món

## Kiến trúc hệ thống

Ứng dụng sử dụng kiến trúc **3-Layer Architecture**:

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer (GUI)                  │
│                      WinForms + Guna UI 2                    │
└─────────────────────────┬───────────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────────┐
│                  Business Logic Layer (BLL)                  │
│  ├── QuanLyBanBLL     │  Thống kê doanh thu                │
│  ├── QuanLyMonBLL     │  Xử lý voucher, khuyến mãi         │
│  ├── QuanLyHoaDonBLL  │  Quản lý nhân viên, khách hàng     │
│  └── QuanLyTaiKhoanBLL                                          │
└─────────────────────────┬───────────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────────┐
│                    Data Access Layer (DAL)                   │
│  ├── DatabaseHelper    │  SQL Operations (ADO.NET)         │
│  ├── ConnectDB          │  SQL Server Connection            │
│  └── DataProvider                                                │
└─────────────────────────┬───────────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────────┐
│                      SQL Server Database                      │
│        QL_NHAHANG (16 bảng: Ban, Mon, HoaDon,...)            │
└─────────────────────────────────────────────────────────────┘
```

## Cấu trúc dự án

```
QuanLy_NhaHang/
├── DAL/                     # Data Access Layer
│   ├── DatabaseHelper.cs    # SQL Server operations
│   ├── ConnectDB.cs         # Connection management
│   └── DataProvider.cs      # Data provider utilities
│
├── BLL/                     # Business Logic Layer
│   ├── QuanLyBanBLL.cs
│   ├── QuanLyMonBLL.cs
│   ├── QuanLyHoaDonBLL.cs
│   ├── QuanLyTaiKhoanBLL.cs
│   ├── QuanLyKhuyenMaiBLL.cs
│   ├── QuanLyThongKeBLL.cs
│   └── QuanLyKhachHangBLL.cs
│
├── DTO/                     # Data Transfer Objects
│   ├── BanDTO.cs
│   ├── MonDTO.cs
│   ├── HoaDonDTO.cs
│   ├── KhachHangDTO.cs
│   ├── TaiKhoanDTO.cs
│   └── KhuyenMaiDTO.cs
│
├── QUANLY_NHAHANG/          # GUI Layer (WinForms)
│   ├── frmMain.cs           # Main form
│   ├── frmDatBan.cs        # Đặt bàn
│   ├── frmGoiMon.cs        # Gọi món
│   ├── frmThanhToan.cs     # Thanh toán
│   ├── frmQuanLyMon.cs     # Quản lý món ăn
│   ├── frmBaoCao.cs        # Báo cáo thống kê
│   └── Reports/            # Crystal Reports
│
└── CSDL_QUANLY_NHAHANG.sql  # Database script
```

## Công nghệ sử dụng

| Component | Technology |
|---|---|
| **Framework** | .NET Framework 4.7.2 |
| **Language** | C# 8.0 |
| **UI** | WinForms + Guna UI 2 |
| **Database** | SQL Server 2012+ |
| **Reporting** | Crystal Reports |
| **Data Access** | ADO.NET |

## Yêu cầu hệ thống

- Windows 7 trở lên
- .NET Framework 4.7.2
- SQL Server 2012 trở lên
- Visual Studio 2019 trở lên

## Cài đặt

### 1. Cấu hình Database

Chạy file SQL script để tạo database:

- `CSDL_QUANLY_NHAHANG.sql` - Tạo cơ sở dữ liệu
- Hoặc sử dụng SQL Server Management Studio

### 2. Cấu hình kết nối

Mở file `App.config` trong `QuanLy_NhaHang/QUANLY_NHAHANG/` và sửa connection string:

```xml
<connectionStrings>
    <add name="QL_NHAHANG"
         connectionString="Data Source=TEN_MAY;Initial Catalog=QL_NHAHANG;User ID=sa;Password=your_password"
         providerName="System.Data.SqlClient"/>
</connectionStrings>
```

### 3. Chạy ứng dụng

Mở solution `QUANLY_NHAHANG.sln` trong Visual Studio.

Nhấn **F5** để chạy.

## Các module chính

| Module | Chức năng |
|---|---|
| Đặt bàn | Đặt trước, chọn bàn, giờ |
| Gọi món | Chọn món, thêm vào bàn |
| Thanh toán | Tính tiền, in hóa đơn |
| Khuyến mãi | Tạo voucher, giảm giá |
| Thống kê | Báo cáo doanh thu |
| Danh sách hóa đơn | Xem lịch sử thanh toán |

## Hướng dẫn sử dụng

### Đăng nhập

Sử dụng tài khoản mặc định:

| Role | Username | Password |
|---|---|---|
| Quản lý | admin | admin |
| Nhân viên | nv01 | 123 |

### Quy trình đặt bàn

1. Chọn tab "Đặt bàn"
2. Chọn bàn trống
3. Nhập thông tin khách hàng
4. Xác nhận đặt bàn

### Quy trình gọi món

1. Chọn bàn đã đặt
2. Chọn món từ danh sách
3. Thêm vào đơn
4. Xác nhận gọi món

### Quy trình thanh toán

1. Chọn bàn cần thanh toán
2. Kiểm tra đơn hàng
3. Áp dụng voucher (nếu có)
4. In hóa đơn
5. Hoàn tất thanh toán

## License

[MIT License](LICENSE)