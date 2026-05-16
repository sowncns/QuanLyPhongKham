-- ============================================================================
-- FILE 05: TẠO CÁC THỦ TỤC LƯU TRỮ (STORED PROCEDURES)
-- Dự án: Quản Lý Phòng Khám
-- Thao tác: Tạo Procedures cho tất cả các bảng
-- ============================================================================

USE QLPK;
GO

-------------------------------------------------------------------------------
-- 1. ĐĂNG NHẬP & TÀI KHOẢN
-------------------------------------------------------------------------------

-- 1.1 Kiểm tra đăng nhập
CREATE PROCEDURE sp_KiemTraDangNhap
    @userName NVARCHAR(50),
    @passWord NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT maTaiKhoan, name, maRole
    FROM TaiKhoan
    WHERE userName = @userName AND passWord = @passWord;
END;
GO

-- 1.2 Thêm tài khoản
CREATE PROCEDURE sp_ThemTaiKhoan
    @userName NVARCHAR(50),
    @passWord NVARCHAR(255),
    @name NVARCHAR(50),
    @maRole INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM TaiKhoan WHERE userName = @userName)
    BEGIN
        RAISERROR (N'Tên đăng nhập đã tồn tại.', 16, 1);
        RETURN;
    END
    INSERT INTO TaiKhoan (userName, passWord, name, maRole)
    VALUES (@userName, @passWord, @name, @maRole);
END;
GO

-- 1.3 Sửa tài khoản
CREATE PROCEDURE sp_SuaTaiKhoan
    @maTaiKhoan INT,
    @passWord NVARCHAR(255),
    @name NVARCHAR(50),
    @maRole INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TaiKhoan
    SET passWord = @passWord,
        name = @name,
        maRole = @maRole
    WHERE maTaiKhoan = @maTaiKhoan;
END;
GO

-- 1.4 Xóa tài khoản (Trigger trg_ChanXoaTaiKhoan sẽ kiểm tra ràng buộc)
CREATE PROCEDURE sp_XoaTaiKhoan
    @maTaiKhoan INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM TaiKhoan WHERE maTaiKhoan = @maTaiKhoan;
END;
GO


-------------------------------------------------------------------------------
-- 2. QUẢN LÝ BỆNH NHÂN
-------------------------------------------------------------------------------

-- 2.1 Thêm bệnh nhân
CREATE PROCEDURE sp_ThemBenhNhan
    @tenBenhNhan NVARCHAR(50),
    @ngaySinh DATE,
    @diaChi NVARCHAR(50),
    @gioiTinh NVARCHAR(50),
    @CCCD NVARCHAR(12),
    @email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM BenhNhan WHERE CCCD = @CCCD)
    BEGIN
        RAISERROR (N'Số CCCD đã tồn tại.', 16, 1);
        RETURN;
    END
    INSERT INTO BenhNhan (tenBenhNhan, ngaySinh, diaChi, gioiTinh, CCCD, email)
    VALUES (@tenBenhNhan, @ngaySinh, @diaChi, @gioiTinh, @CCCD, @email);
END;
GO

-- 2.2 Sửa bệnh nhân
CREATE PROCEDURE sp_SuaBenhNhan
    @maBenhNhan INT,
    @tenBenhNhan NVARCHAR(50),
    @ngaySinh DATE,
    @diaChi NVARCHAR(50),
    @gioiTinh NVARCHAR(50),
    @CCCD NVARCHAR(12),
    @email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE BenhNhan
    SET tenBenhNhan = @tenBenhNhan,
        ngaySinh = @ngaySinh,
        diaChi = @diaChi,
        gioiTinh = @gioiTinh,
        CCCD = @CCCD,
        email = @email
    WHERE maBenhNhan = @maBenhNhan AND isDeleted = 0;
END;
GO

-- 2.3 Xóa bệnh nhân (Soft Delete)
CREATE PROCEDURE sp_XoaBenhNhan
    @maBenhNhan INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE BenhNhan SET isDeleted = 1 WHERE maBenhNhan = @maBenhNhan;
END;
GO

-- 2.4 Tìm kiếm bệnh nhân
CREATE PROCEDURE sp_TimKiemBenhNhan
    @tuKhoa NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT maBenhNhan, tenBenhNhan, ngaySinh, diaChi, CCCD, gioiTinh, email
    FROM BenhNhan
    WHERE isDeleted = 0 AND (tenBenhNhan LIKE '%' + @tuKhoa + '%' OR CCCD LIKE '%' + @tuKhoa + '%');
END;
GO


-------------------------------------------------------------------------------
-- 3. QUẢN LÝ BỆNH
-------------------------------------------------------------------------------

-- 3.1 Thêm bệnh
CREATE PROCEDURE sp_ThemBenh
    @tenBenh NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Benh (tenBenh) VALUES (@tenBenh);
END;
GO

-- 3.2 Sửa bệnh
CREATE PROCEDURE sp_SuaBenh
    @maBenh INT,
    @tenBenh NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Benh SET tenBenh = @tenBenh WHERE maBenh = @maBenh;
END;
GO

-- 3.3 Xóa bệnh
CREATE PROCEDURE sp_XoaBenh
    @maBenh INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Benh WHERE maBenh = @maBenh;
END;
GO


-------------------------------------------------------------------------------
-- 4. QUẢN LÝ THUỐC
-------------------------------------------------------------------------------

-- 4.1 Thêm thuốc
CREATE PROCEDURE sp_ThemThuoc
    @tenThuoc NVARCHAR(50),
    @donGia FLOAT,
    @maCachDung INT,
    @maDonVi INT,
    @soLuong INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Thuoc (tenThuoc, donGia, maCachDung, maDonVi, soLuong)
    VALUES (@tenThuoc, @donGia, @maCachDung, @maDonVi, @soLuong);
END;
GO

-- 4.2 Sửa thuốc
CREATE PROCEDURE sp_SuaThuoc
    @maThuoc INT,
    @tenThuoc NVARCHAR(50),
    @donGia FLOAT,
    @maCachDung INT,
    @maDonVi INT,
    @soLuong INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Thuoc
    SET tenThuoc = @tenThuoc,
        donGia = @donGia,
        maCachDung = @maCachDung,
        maDonVi = @maDonVi,
        soLuong = @soLuong
    WHERE maThuoc = @maThuoc;
END;
GO

-- 4.3 Xóa thuốc
CREATE PROCEDURE sp_XoaThuoc
    @maThuoc INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Thuoc WHERE maThuoc = @maThuoc;
END;
GO


-------------------------------------------------------------------------------
-- 5. QUẢN LÝ LỊCH HẸN
-------------------------------------------------------------------------------

-- 5.1 Thêm lịch hẹn
CREATE PROCEDURE sp_ThemLichHen
    @ngayHen DATETIME2,
    @maTaiKhoan INT,
    @maBenhNhan INT,
    @maDieuDuong INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO LichHen (ngayHen, maTaiKhoan, maBenhNhan, maDieuDuong)
    VALUES (@ngayHen, @maTaiKhoan, @maBenhNhan, @maDieuDuong);
END;
GO

-- 5.2 Cập nhật trạng thái lịch hẹn
CREATE PROCEDURE sp_CapNhatTrangThaiLichHen
    @maLichHen INT,
    @trangThaiMoi NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE LichHen SET trangThai = @trangThaiMoi WHERE maLichHen = @maLichHen;
END;
GO


-------------------------------------------------------------------------------
-- 6. PHIẾU KHÁM & TOA THUỐC (Sử dụng TRANSACTION)
-------------------------------------------------------------------------------

-- 6.1 Tạo phiếu khám và toa thuốc rỗng
CREATE PROCEDURE sp_TaoPhieuKhamVaToaThuoc
    @trieuChung NVARCHAR(255),
    @maBenhNhan INT,
    @maTaiKhoan INT, -- Bác sĩ
    @ngayTaiKham DATE
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Tạo phiếu khám
        INSERT INTO PhieuKhamBenh (ngayKham, trieuChung, maBenhNhan, maTaiKhoan, ngayTaiKham)
        VALUES (GETDATE(), @trieuChung, @maBenhNhan, @maTaiKhoan, @ngayTaiKham);
        
        DECLARE @newPKB INT = SCOPE_IDENTITY();
        
        -- 2. Tạo toa thuốc rỗng
        INSERT INTO ToaThuoc (ngayKeToa, maPKB)
        VALUES (GETDATE(), @newPKB);
        
        COMMIT TRANSACTION;
        SELECT @newPKB AS NewPKBID;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;
GO

-- 6.2 Thêm chuẩn đoán
CREATE PROCEDURE sp_ThemChuanDoan
    @maBenh INT,
    @maPKB INT,
    @tenChuanDoan NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ChuanDoan (maBenh, maPKB, tenChuanDoan)
    VALUES (@maBenh, @maPKB, @tenChuanDoan);
END;
GO

-- 6.3 Kê thuốc vào đơn
CREATE PROCEDURE sp_KeThuocVaoDon
    @maThuoc INT,
    @maToaThuoc INT,
    @soLuong INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Trigger trg_CapNhatSoLuongThuocTrongKho sẽ xử lý trừ kho và kiểm tra âm kho
    INSERT INTO ChiTietDonThuoc (maThuoc, maToaThuoc, soLuong)
    VALUES (@maThuoc, @maToaThuoc, @soLuong);
END;
GO

-- 6.4 Xóa thuốc khỏi đơn
CREATE PROCEDURE sp_XoaThuocKhoiDon
    @maThuoc INT,
    @maToaThuoc INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Trigger trg_HoanTraSoLuongThuoc sẽ tự động cộng lại kho
    DELETE FROM ChiTietDonThuoc WHERE maThuoc = @maThuoc AND maToaThuoc = @maToaThuoc;
END;
GO


-------------------------------------------------------------------------------
-- 7. HÓA ĐƠN (Sử dụng TRANSACTION & FUNCTION)
-------------------------------------------------------------------------------

-- 7.1 Lập hóa đơn thanh toán
CREATE PROCEDURE sp_LapHoaDonThanhToan
    @maPKB INT,
    @maTaiKhoan INT -- Thu ngân
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Tính tiền thuốc bằng Function
        DECLARE @tienThuoc FLOAT = dbo.f_TinhTienThuoc(@maPKB);
        
        -- 2. Lấy tiền khám từ bảng DichVu
        DECLARE @tienKham FLOAT;
        SELECT @tienKham = tienDichVu FROM DichVu WHERE tenDichVu = N'Khám bệnh';
        IF @tienKham IS NULL SET @tienKham = 50000;
        
        DECLARE @tongTien FLOAT = @tienThuoc + @tienKham;
        
        DECLARE @ngayTaiKham DATE;
        SELECT @ngayTaiKham = ngayTaiKham FROM PhieuKhamBenh WHERE maPKB = @maPKB;
        
        -- 3. Tạo hóa đơn
        INSERT INTO HoaDon (ngayLapHoaDon, tienThuoc, tienKham, tongTien, ngayTaiKham, maPKB, maTaiKhoan)
        VALUES (GETDATE(), @tienThuoc, @tienKham, @tongTien, @ngayTaiKham, @maPKB, @maTaiKhoan);
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;
GO

-------------------------------------------------------------------------------
-- 8. QUẢN LÝ DỊCH VỤ
-------------------------------------------------------------------------------

-- 8.1 Thêm dịch vụ
CREATE PROCEDURE sp_ThemDichVu
    @tenDichVu NVARCHAR(50),
    @tienDichVu FLOAT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO DichVu (tenDichVu, tienDichVu) VALUES (@tenDichVu, @tienDichVu);
END;
GO

-- 8.2 Sửa dịch vụ
CREATE PROCEDURE sp_SuaDichVu
    @maDichVu INT,
    @tenDichVu NVARCHAR(50),
    @tienDichVu FLOAT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DichVu SET tenDichVu = @tenDichVu, tienDichVu = @tienDichVu WHERE maDichVu = @maDichVu;
END;
GO

-- 8.3 Xóa dịch vụ
CREATE PROCEDURE sp_XoaDichVu
    @maDichVu INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM DichVu WHERE maDichVu = @maDichVu;
END;
GO

-------------------------------------------------------------------------------
-- 9. KHÁC
-------------------------------------------------------------------------------

-- 9.1 Cập nhật trạng thái gửi mail
CREATE PROCEDURE sp_CapNhatDaGuiMail
    @maPKB INT,
    @daGui BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE PhieuKhamBenh SET DaGuiMail = @daGui WHERE maPKB = @maPKB;
END;
GO

-- 9.2 Thêm toa thuốc
CREATE PROCEDURE sp_ThemToaThuoc
    @ngayKeToa DATE,
    @maPKB INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ToaThuoc (ngayKeToa, maPKB) VALUES (@ngayKeToa, @maPKB);
    SELECT SCOPE_IDENTITY() AS NewToaThuocID;
END;
GO

-- 9.3 Báo cáo sử dụng thuốc theo tháng
CREATE PROCEDURE sp_BaoCaoSuDungThuoc
    @month INT,
    @year INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TH.maThuoc, TH.tenThuoc, SUM(KT.soLuong) AS soLuong 
    FROM ToaThuoc T 
    JOIN ChiTietDonThuoc KT ON T.maToaThuoc = KT.maToaThuoc 
    JOIN Thuoc TH ON KT.maThuoc = TH.maThuoc 
    WHERE MONTH(T.ngayKeToa) = @month AND YEAR(T.ngayKeToa) = @year 
    GROUP BY TH.maThuoc, TH.tenThuoc;
END;
GO

-- 9.4 Đếm số lần kê thuốc trong tháng
CREATE PROCEDURE sp_DemSoLanKeThuoc
    @mathuoc INT,
    @month INT,
    @year INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(KT.maToaThuoc) AS SLD 
    FROM ToaThuoc T 
    JOIN ChiTietDonThuoc KT ON T.maToaThuoc = KT.maToaThuoc 
    WHERE MONTH(T.ngayKeToa) = @month AND YEAR(T.ngayKeToa) = @year AND KT.maThuoc = @mathuoc;
END;
GO
