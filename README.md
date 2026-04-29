# Restaurant Management - Phần mềm quản lý nhà hàng

Ứng dụng desktop quản lý nhà hàng với đầy đủ tính năng: đặt bàn, gọi món, tính tiền, in hóa đơn, thống kê doanh thu.

## Tính năng

- Quản lý đặt bàn trước theo giờ, ngày
- Gọi món theo bàn, theo khách hàng
- Tính tiền, in hóa đơn (Crystal Reports)
- Quản lý khuyến mãi, voucher giảm giá
- Thống kê doanh thu theo ngày/tháng/năm
- Quản lý khách hàng, nhân viên
- Quản lý món ăn, danh mục món

## Yêu cầu hệ thống

- Windows 7 trở lên
- .NET Framework 4.7.2
- SQL Server 2012 trở lên
- Visual Studio 2019 trở lên

## Cài đặt

### 1. Cấu hình Database

Chạy file SQL script trong thư mục gốc để tạo database:

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

Nhấn F5 để chạy.

## Kiến trúc

Ứng dụng sử dụng kiến trúc 3 lớp:

```
├── DAL (Data Access Layer)     - Truy xuất database
├── BLL (Business Logic Layer) - Xử lý nghiệp vụ  
├── DTO (Data Transfer Object) - Đối tượng dữ liệu
└── GUI                    - Giao diện WinForms
```

## Các module chính

| Module | Chức năng |
|--------|----------|
| Đặt bàn | Đặt trước, chọn bàn, giờ |
| Gọi món | Chọn món, thêm vào bàn |
| Thanh toán | Tính tiền, in hóa đơn |
| Khuyến mãi | Tạo voucher, giảm giá |
| Thống kê | Báo cáo doanh thu |
| Danh sách hóa đơn | Xem lịch sử thanh toán |

## Công nghệ sử dụng

- WinForms (.NET Framework 4.7.2)
- SQL Server
- Crystal Reports (In hóa đơn)
- Guna UI 2 (UI Library)
- ADO.NET

## Hướng dẫn sử dụng

### Đăng nhập

Sử dụng tài khoản mặc định:
- Quản lý: admin / admin
- Nhân viên: nv01 / 123

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

MIT License