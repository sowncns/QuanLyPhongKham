USE QLPK;
GO

-- XÓA TOÀN BỘ DỮ LIỆU CŨ
DELETE FROM HoaDon;
DELETE FROM ChiTietDonThuoc;
DELETE FROM ToaThuoc;
DELETE FROM ChuanDoan;
DELETE FROM PhieuKhamBenh;
DELETE FROM LichHen;
DELETE FROM DichVu;
DELETE FROM Thuoc;
DELETE FROM CachDung;
DELETE FROM DonVi;
DELETE FROM Benh;
DELETE FROM BenhNhan;
DELETE FROM TaiKhoan;
DELETE FROM Roles;
GO

-- RESET IDENTITY
DBCC CHECKIDENT ('HoaDon', RESEED, 0);

DBCC CHECKIDENT ('ToaThuoc', RESEED, 0);

DBCC CHECKIDENT ('PhieuKhamBenh', RESEED, 0);
DBCC CHECKIDENT ('LichHen', RESEED, 0);
DBCC CHECKIDENT ('DichVu', RESEED, 0);
DBCC CHECKIDENT ('Thuoc', RESEED, 0);
DBCC CHECKIDENT ('CachDung', RESEED, 0);
DBCC CHECKIDENT ('DonVi', RESEED, 0);
DBCC CHECKIDENT ('Benh', RESEED, 0);
DBCC CHECKIDENT ('BenhNhan', RESEED, 0);
DBCC CHECKIDENT ('TaiKhoan', RESEED, 0);
DBCC CHECKIDENT ('Roles', RESEED, 0);
GO

-- 1. ROLES
INSERT INTO Roles (tenRole)
VALUES
(N'Bác sĩ'),
(N'Thu ngân'),
(N'Điều dưỡng'),
(N'Quản trị viên');
GO

-- 2. TÀI KHOẢN
INSERT INTO TaiKhoan
(userName, passWord, name, maRole)
VALUES
('bacsi1', '123', N'Bác sĩ Nguyễn Văn A', 1),
('bacsi2', '123', N'Bác sĩ Trần Minh B', 1),
('thungan1', '123', N'Thu ngân Lê Thị C', 2),
('dieuduong1', '123', N'Điều dưỡng Phạm Văn D', 3),
('admin', '123', N'Quản trị viên', 4);
GO

-- 3. BỆNH NHÂN
INSERT INTO BenhNhan
(tenBenhNhan, ngaySinh, diaChi, CCCD, gioiTinh, email)
VALUES
(N'Nguyễn Văn Một', '1990-01-01', N'Hà Nội', '001122334455', N'Nam', 'mot@gmail.com'),
(N'Trần Thị Hai', '1995-05-05', N'TP HCM', '001122334456', N'Nữ', 'hai@gmail.com'),
(N'Lê Văn Ba', '1988-03-12', N'Đà Nẵng', '001122334457', N'Nam', 'ba@gmail.com'),
(N'Phạm Thị Bốn', '2000-07-21', N'Cần Thơ', '001122334458', N'Nữ', 'bon@gmail.com'),
(N'Hoàng Văn Năm', '1975-11-15', N'Hải Phòng', '001122334459', N'Nam', 'nam@gmail.com');
GO

-- 4. BỆNH
INSERT INTO Benh (tenBenh)
VALUES
(N'Cảm cúm'),
(N'Sốt xuất huyết'),
(N'Viêm họng'),
(N'Đau lưng'),
(N'Mất ngủ'),
(N'Đau dạ dày'),
(N'Viêm da'),
(N'Đau khớp'),
(N'Rối loạn tiêu hóa');
GO

-- 5. ĐƠN VỊ
INSERT INTO DonVi (tenDonVi)
VALUES
(N'Viên'),
(N'Chai'),
(N'Vỉ'),
(N'Gói'),
(N'Ống');
GO

-- 6. CÁCH DÙNG
INSERT INTO CachDung (tenCachDung)
VALUES
(N'Uống sau ăn'),
(N'Uống trước ăn'),
(N'Ngậm'),
(N'Tiêm'),
(N'Bôi ngoài da');
GO

-- 7. THUỐC
INSERT INTO Thuoc
(tenThuoc, donGia, maCachDung, maDonVi, soLuong)
VALUES
(N'Paracetamol', 5000, 1, 1, 1000),
(N'Panadol', 6000, 1, 1, 500),
(N'Siro ho Prospan', 50000, 1, 2, 100),
(N'Amoxicillin', 12000, 1, 1, 300),
(N'Vitamin C', 3000, 1, 1, 800),
(N'Thuốc đau dạ dày Yumangel', 15000, 1, 4, 200),
(N'Diclofenac', 10000, 1, 1, 400),
(N'Kem bôi da Gentrisone', 25000, 5, 5, 100);
GO

-- 8. DỊCH VỤ
INSERT INTO DichVu
(tenDichVu, tienDichVu)
VALUES
(N'Khám bệnh', 50000),
(N'Siêu âm', 150000),
(N'Xét nghiệm máu', 200000),
(N'Chụp X-Quang', 300000),
(N'Nội soi', 500000);
GO

PRINT N'Đã reset và tạo lại dữ liệu mẫu thành công!';
GO