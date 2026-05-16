-- ============================================================================
-- FILE 01: TẠO CẤU TRÚC BẢNG (TABLES)
-- Dự án: Quản Lý Phòng Khám
-- Thao tác: Tạo Database, Tables, Constraints, Indexes
-- ============================================================================

CREATE DATABASE QLPK;
GO
USE QLPK;
GO

-- 1. BẢNG QUYỀN (Roles)
CREATE TABLE Roles(
    maRole INT IDENTITY(1,1),
    tenRole NVARCHAR(50) NOT NULL,
    CONSTRAINT pk_Roles PRIMARY KEY CLUSTERED (maRole ASC)
);
GO

-- 2. BẢNG TÀI KHOẢN (TaiKhoan)
CREATE TABLE TaiKhoan(
    maTaiKhoan INT IDENTITY(1,1),
    userName NVARCHAR(50) NOT NULL,
    passWord NVARCHAR(255) NOT NULL, -- Độ dài 255 hỗ trợ hash password
    name NVARCHAR(50) NOT NULL,
    maRole INT,
    CONSTRAINT pk_TaiKhoan PRIMARY KEY CLUSTERED (maTaiKhoan ASC),
    CONSTRAINT uq_Username UNIQUE (userName),
    CONSTRAINT fk_Roles_TaiKhoan FOREIGN KEY (maRole) REFERENCES Roles (maRole)
);
GO

-- 3. BẢNG BỆNH NHÂN (BenhNhan)
CREATE TABLE BenhNhan(
    maBenhNhan INT IDENTITY(1,1),
    tenBenhNhan NVARCHAR(50) NOT NULL,
    ngaySinh DATE NOT NULL,
    diaChi NVARCHAR(50) NOT NULL,
    CCCD NVARCHAR(12) NOT NULL,
    gioiTinh NVARCHAR(50) NOT NULL,
    email NVARCHAR(100) NULL,
    isDeleted BIT DEFAULT 0, -- Soft delete
    CONSTRAINT pk_BenhNhan PRIMARY KEY CLUSTERED (maBenhNhan ASC),
    CONSTRAINT uq_CCCD UNIQUE (CCCD)
);
GO

-- 4. BẢNG LOẠI BỆNH (Benh)
CREATE TABLE Benh(
    maBenh INT IDENTITY(1,1),
    tenBenh NVARCHAR(50) NOT NULL,
    CONSTRAINT pk_Benh PRIMARY KEY CLUSTERED (maBenh ASC),
    CONSTRAINT uq_TenBenh UNIQUE (tenBenh)
);
GO

-- 5. BẢNG PHIẾU KHÁM BỆNH (PhieuKhamBenh)
CREATE TABLE PhieuKhamBenh(
    maPKB INT IDENTITY(1,1),
    ngayKham DATE DEFAULT GETDATE(),
    trieuChung NVARCHAR(255),
    maBenhNhan INT,
    maTaiKhoan INT, -- Bác sĩ khám
    ngayTaiKham DATE,
    DaGuiMail BIT NOT NULL DEFAULT 0,
    CONSTRAINT pk_PhieuKhamBenh PRIMARY KEY CLUSTERED (maPKB ASC),
    CONSTRAINT fk_BenhNhan_PKB FOREIGN KEY (maBenhNhan) REFERENCES BenhNhan(maBenhNhan),
    CONSTRAINT fk_TaiKhoan_PKB FOREIGN KEY (maTaiKhoan) REFERENCES TaiKhoan(maTaiKhoan)
);
GO

-- 6. BẢNG CHUẨN ĐOÁN (ChuanDoan)
CREATE TABLE ChuanDoan(
    maBenh INT,
    maPKB INT,
    tenChuanDoan NVARCHAR(255),
    CONSTRAINT pk_ChuanDoan PRIMARY KEY CLUSTERED (maBenh, maPKB ASC),
    CONSTRAINT fk_Benh_ChuanDoan FOREIGN KEY (maBenh) REFERENCES Benh (maBenh),
    CONSTRAINT fk_PKB_ChuanDoan FOREIGN KEY (maPKB) REFERENCES PhieuKhamBenh (maPKB)
);
GO

-- 7. BẢNG ĐƠN VỊ TÍNH (DonVi)
CREATE TABLE DonVi(
    maDonVi INT IDENTITY(1,1),
    tenDonVi NVARCHAR(20) NOT NULL,
    CONSTRAINT pk_DonVi PRIMARY KEY CLUSTERED (maDonVi ASC),
    CONSTRAINT uq_TenDonVi UNIQUE (tenDonVi)
);
GO

-- 8. BẢNG CÁCH DÙNG (CachDung)
CREATE TABLE CachDung(
    maCachDung INT IDENTITY(1,1),
    tenCachDung NVARCHAR(50) NOT NULL,
    CONSTRAINT pk_CachDung PRIMARY KEY CLUSTERED (maCachDung ASC),
    CONSTRAINT uq_TenCachDung UNIQUE (tenCachDung)
);
GO

-- 9. BẢNG THUỐC (Thuoc)
CREATE TABLE Thuoc(
    maThuoc INT IDENTITY(1,1),
    tenThuoc NVARCHAR(50) NOT NULL,
    donGia FLOAT NOT NULL,
    maCachDung INT NOT NULL,
    maDonVi INT NOT NULL,
    soLuong INT DEFAULT 0,
    CONSTRAINT pk_Thuoc PRIMARY KEY CLUSTERED (maThuoc ASC),
    CONSTRAINT uq_TenThuoc UNIQUE (tenThuoc),
    CONSTRAINT chk_SoLuongThuoc CHECK (soLuong >= 0), -- Không cho âm kho
    CONSTRAINT fk_DonVi_Thuoc FOREIGN KEY (maDonVi) REFERENCES DonVi (maDonVi),
    CONSTRAINT fk_CachDung_Thuoc FOREIGN KEY (maCachDung) REFERENCES CachDung (maCachDung)
);
GO

-- 10. BẢNG TOA THUỐC (ToaThuoc)
CREATE TABLE ToaThuoc(
    maToaThuoc INT IDENTITY(1,1),
    ngayKeToa DATE DEFAULT GETDATE(),
    maPKB INT,
    CONSTRAINT pk_ToaThuoc PRIMARY KEY CLUSTERED (maToaThuoc ASC),
    CONSTRAINT fk_PKB_ToaThuoc FOREIGN KEY (maPKB) REFERENCES PhieuKhamBenh (maPKB)
);
GO

-- 11. BẢNG CHI TIẾT ĐƠN THUỐC (ChiTietDonThuoc)
CREATE TABLE ChiTietDonThuoc(
    maThuoc INT,
    maToaThuoc INT,
    soLuong INT NOT NULL,
    CONSTRAINT pk_CTDT PRIMARY KEY CLUSTERED (maThuoc, maToaThuoc ASC),
    CONSTRAINT chk_SoLuongKe CHECK (soLuong > 0),
    CONSTRAINT fk_Thuoc_CTDT FOREIGN KEY (maThuoc) REFERENCES Thuoc (maThuoc),
    CONSTRAINT fk_ToaThuoc_CTDT FOREIGN KEY (maToaThuoc) REFERENCES ToaThuoc (maToaThuoc)
);
GO

-- 12. BẢNG DỊCH VỤ (DichVu)
CREATE TABLE DichVu(
    maDichVu INT IDENTITY(1,1),
    tenDichVu NVARCHAR(50) NOT NULL,
    tienDichVu FLOAT NOT NULL,
    CONSTRAINT pk_DichVu PRIMARY KEY CLUSTERED (maDichVu ASC),
    CONSTRAINT uq_TenDichVu UNIQUE (tenDichVu),
    CONSTRAINT chk_TienDichVu CHECK (tienDichVu >= 0)
);
GO

-- 13. BẢNG HÓA ĐƠN (HoaDon)
CREATE TABLE HoaDon(
    maHoaDon INT IDENTITY(1,1),
    ngayLapHoaDon DATE DEFAULT GETDATE(),
    tienThuoc FLOAT DEFAULT 0,
    tienKham FLOAT DEFAULT 0,
    tongTien FLOAT DEFAULT 0,
    ngayTaiKham DATE,
    maPKB INT,
    maTaiKhoan INT, -- Nhân viên thu ngân
    CONSTRAINT pk_HoaDon PRIMARY KEY CLUSTERED (maHoaDon ASC),
    CONSTRAINT fk_PKB_HoaDon FOREIGN KEY (maPKB) REFERENCES PhieuKhamBenh (maPKB),
    CONSTRAINT fk_TaiKhoan_HoaDon FOREIGN KEY (maTaiKhoan) REFERENCES TaiKhoan(maTaiKhoan)
);
GO

-- 14. BẢNG LỊCH HẸN (LichHen)
CREATE TABLE LichHen(
    maLichHen INT IDENTITY(1,1),
    ngayHen DATETIME2 NOT NULL,
    maTaiKhoan INT, -- Bác sĩ được hẹn
    maBenhNhan INT,
    maDieuDuong INT, -- Người tạo lịch
    trangThai NVARCHAR(30) DEFAULT N'Chờ khám',
    CONSTRAINT pk_LichHen PRIMARY KEY CLUSTERED (maLichHen ASC),
    CONSTRAINT fk_LichHen_BacSi FOREIGN KEY(maTaiKhoan) REFERENCES TaiKhoan(maTaiKhoan),
    CONSTRAINT fk_LichHen_DieuDuong FOREIGN KEY(maDieuDuong) REFERENCES TaiKhoan(maTaiKhoan),
    CONSTRAINT fk_LichHen_BenhNhan FOREIGN KEY(maBenhNhan) REFERENCES BenhNhan(maBenhNhan)
);
GO

-- 15. BẢNG AUDIT LOG (Lịch sử thao tác)
CREATE TABLE AuditLog(
    logID INT IDENTITY(1,1),
    ngayThaoTac DATETIME DEFAULT GETDATE(),
    maTaiKhoan INT,
    hanhDong NVARCHAR(50),
    chiTiet NVARCHAR(255),
    CONSTRAINT pk_AuditLog PRIMARY KEY CLUSTERED (logID ASC),
    CONSTRAINT fk_TaiKhoan_Audit FOREIGN KEY (maTaiKhoan) REFERENCES TaiKhoan(maTaiKhoan)
);
GO

-- TẠO CÁC INDEX ĐƯỢC YÊU CẦU
CREATE INDEX idx_CCCD ON BenhNhan(CCCD);
CREATE INDEX idx_Username ON TaiKhoan(userName);
CREATE INDEX idx_LichHen ON LichHen(maLichHen);
CREATE INDEX idx_PKB ON PhieuKhamBenh(maPKB);
GO
