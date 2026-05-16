-- ============================================================================
-- FILE 02: TẠO CÁC KHUNG NHÌN (VIEWS)
-- Dự án: Quản Lý Phòng Khám
-- Thao tác: Tạo Views không dùng SELECT *
-- ============================================================================

USE QLPK;
GO

-- 1. View Danh sách tài khoản (Không hiển thị password)
CREATE VIEW v_DanhSachTaiKhoan AS
SELECT 
    tk.maTaiKhoan,
    tk.userName,
    tk.name,
    tk.maRole,
    r.tenRole
FROM TaiKhoan tk
LEFT JOIN Roles r ON tk.maRole = r.maRole;
GO

-- 2. View Danh sách bệnh nhân (Chỉ lấy người chưa bị xóa)
CREATE VIEW v_DanhSachBenhNhan AS
SELECT 
    maBenhNhan,
    tenBenhNhan,
    ngaySinh,
    diaChi,
    CCCD,
    gioiTinh,
    email
FROM BenhNhan
WHERE isDeleted = 0;
GO

-- 3. View Danh sách bệnh
CREATE VIEW v_DanhSachBenh AS
SELECT 
    maBenh,
    tenBenh
FROM Benh;
GO

-- 4. View Danh sách thuốc (JOIN tên đơn vị và cách dùng)
CREATE VIEW v_DanhSachThuoc AS
SELECT 
    t.maThuoc,
    t.tenThuoc,
    t.donGia,
    t.soLuong,
    t.maDonVi,
    t.maCachDung,
    dv.tenDonVi,
    cd.tenCachDung
FROM Thuoc t
JOIN DonVi dv ON t.maDonVi = dv.maDonVi
JOIN CachDung cd ON t.maCachDung = cd.maCachDung;
GO

-- 5. View Danh sách dịch vụ
CREATE VIEW v_DanhSachDichVu AS
SELECT 
    maDichVu,
    tenDichVu,
    tienDichVu
FROM DichVu;
GO

-- 6. View Danh sách lịch hẹn
CREATE VIEW v_DanhSachLichHen AS
SELECT 
    lh.maLichHen,
    lh.ngayHen,
    lh.maBenhNhan,
    lh.maTaiKhoan,
    lh.maDieuDuong,
    bn.tenBenhNhan,
    tk1.name AS tenBacSi,
    tk2.name AS tenDieuDuong,
    lh.trangThai
FROM LichHen lh
JOIN BenhNhan bn ON lh.maBenhNhan = bn.maBenhNhan
LEFT JOIN TaiKhoan tk1 ON lh.maTaiKhoan = tk1.maTaiKhoan
LEFT JOIN TaiKhoan tk2 ON lh.maDieuDuong = tk2.maTaiKhoan;
GO

-- 7. View Chi tiết phiếu khám
CREATE VIEW v_ChiTietPhieuKham AS
SELECT 
    pkb.maPKB,
    pkb.ngayKham,
    pkb.trieuChung,
    pkb.maBenhNhan,
    pkb.maTaiKhoan,
    bn.tenBenhNhan,
    tk.name AS tenBacSi,
    pkb.ngayTaiKham
FROM PhieuKhamBenh pkb
JOIN BenhNhan bn ON pkb.maBenhNhan = bn.maBenhNhan
JOIN TaiKhoan tk ON pkb.maTaiKhoan = tk.maTaiKhoan;
GO

-- 8. View Danh sách hóa đơn
CREATE VIEW v_DanhSachHoaDon AS
SELECT 
    hd.maHoaDon,
    hd.ngayLapHoaDon,
    hd.tienThuoc,
    hd.tienKham,
    hd.tongTien,
    bn.tenBenhNhan,
    tk.name AS tenNhanVienThuNgan
FROM HoaDon hd
JOIN PhieuKhamBenh pkb ON hd.maPKB = pkb.maPKB
JOIN BenhNhan bn ON pkb.maBenhNhan = bn.maBenhNhan
JOIN TaiKhoan tk ON hd.maTaiKhoan = tk.maTaiKhoan;
GO

-- 9. View Danh mục phụ
CREATE VIEW v_DanhSachDonVi AS
SELECT maDonVi, tenDonVi FROM DonVi;
GO

CREATE VIEW v_DanhSachCachDung AS
SELECT maCachDung, tenCachDung FROM CachDung;
GO

CREATE VIEW v_DanhSachRoles AS
SELECT maRole, tenRole FROM Roles;
GO

-- 10. View Chi tiết đơn thuốc (để C# truy vấn)
CREATE VIEW v_ChiTietDonThuoc AS
SELECT 
    tt.maPKB,
    ct.maToaThuoc,
    ct.maThuoc,
    ct.soLuong,
    tt.ngayKeToa
FROM ToaThuoc tt
JOIN ChiTietDonThuoc ct ON tt.maToaThuoc = ct.maToaThuoc;
GO

-- 11. View Danh sách chuẩn đoán
CREATE VIEW v_DanhSachChuanDoan AS
SELECT maBenh, maPKB, tenChuanDoan FROM ChuanDoan;
GO
