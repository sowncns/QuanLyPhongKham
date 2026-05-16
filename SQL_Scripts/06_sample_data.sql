-- ============================================================================
-- FILE 06: TẠO DỮ LIỆU MẪU (SAMPLE DATA)
-- Dự án: Quản Lý Phòng Khám
-- Thao tác: Chèn dữ liệu mẫu để test hệ thống
-- ============================================================================

USE QLPK;
GO

-- 1. Chèn Roles
INSERT INTO Roles (tenRole) VALUES 
(N'Bác sĩ'),
(N'Thu ngân'),
(N'Điều dưỡng'),
(N'Quản trị viên');
GO

-- 2. Chèn Tài Khoản (Mật khẩu giả định đã hash hoặc chưa tùy ý, ở đây để text cho dễ test)
INSERT INTO TaiKhoan (userName, passWord, name, maRole) VALUES 
('bacsi1', '123', N'Bác sĩ Nguyễn Văn A', 1),
('thungan1', '123', N'Thu Ngân Lê Thị B', 2),
('dieuduong1', '123', N'Điều Dưỡng Trần Văn C', 3),
('admin', '123', N'Quản Trị Viên', 4);
GO

-- 3. Chèn Bệnh Nhân
INSERT INTO BenhNhan (tenBenhNhan, ngaySinh, diaChi, CCCD, gioiTinh, email) VALUES 
(N'Nguyễn Văn Một', '1990-01-01', N'Hà Nội', '001122334455', N'Nam', 'mot@gmail.com'),
(N'Trần Thị Hai', '1995-05-05', N'TP HCM', '001122334456', N'Nữ', 'hai@gmail.com');
GO

-- 4. Chèn Bệnh
INSERT INTO Benh (tenBenh) VALUES 
(N'Cảm cúm'),
(N'Sốt xuất huyết'),
(N'Viêm họng');
GO

-- 5. Chèn Đơn Vị
INSERT INTO DonVi (tenDonVi) VALUES 
(N'Viên'),
(N'Chai'),
(N'Vỉ');
GO

-- 6. Chèn Cách Dùng
INSERT INTO CachDung (tenCachDung) VALUES 
(N'Uống sau ăn'),
(N'Uống trước ăn'),
(N'Ngậm');
GO

-- 7. Chèn Thuốc
INSERT INTO Thuoc (tenThuoc, donGia, maCachDung, maDonVi, soLuong) VALUES 
(N'Paracetamol', 5000, 1, 1, 1000),
(N'Panadol', 6000, 1, 1, 500),
(N'Siro Ho', 50000, 1, 2, 100);
GO

-- 8. Chèn Dịch Vụ
INSERT INTO DichVu (tenDichVu, tienDichVu) VALUES 
(N'Khám bệnh', 50000),
(N'Siêu âm', 150000),
(N'Xét nghiệm máu', 200000);
GO

PRINT N'Đã chèn dữ liệu mẫu thành công!';
GO
