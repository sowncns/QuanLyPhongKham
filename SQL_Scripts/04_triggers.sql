-- ============================================================================
-- FILE 04: TẠO CÁC BẪY KÍCH HOẠT (TRIGGERS)
-- Dự án: Quản Lý Phòng Khám
-- Thao tác: Tạo Triggers đảm bảo toàn vẹn dữ liệu
-- ============================================================================

USE QLPK;
GO

-- 1. Trigger Không cho xóa tài khoản nếu đã phát sinh lịch sử khám/hóa đơn
CREATE TRIGGER trg_ChanXoaTaiKhoan
ON TaiKhoan
INSTEAD OF DELETE
AS
BEGIN
    -- Kiểm tra xem tài khoản có nằm trong PhieuKhamBenh hoặc HoaDon không
    IF EXISTS (
        SELECT 1 FROM PhieuKhamBenh WHERE maTaiKhoan IN (SELECT maTaiKhoan FROM deleted)
    ) OR EXISTS (
        SELECT 1 FROM HoaDon WHERE maTaiKhoan IN (SELECT maTaiKhoan FROM deleted)
    )
    BEGIN
        RAISERROR (N'Không thể xóa tài khoản này vì đã có lịch sử khám hoặc hóa đơn.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Nếu không có ràng buộc, tiến hành xóa
    DELETE FROM TaiKhoan WHERE maTaiKhoan IN (SELECT maTaiKhoan FROM deleted);
END;
GO

-- 2. Trigger Không cho đặt lịch ngày quá khứ
CREATE TRIGGER trg_KiemTraNgayDatLich
ON LichHen
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM inserted WHERE ngayHen < GETDATE()
    )
    BEGIN
        RAISERROR (N'Không thể đặt lịch hẹn trong quá khứ.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO

-- 3. Trigger Cập nhật số lượng thuốc trong kho khi kê đơn
CREATE TRIGGER trg_CapNhatSoLuongThuocTrongKho
ON ChiTietDonThuoc
AFTER INSERT
AS
BEGIN
    -- Kiểm tra xem có đủ thuốc không
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN Thuoc t ON i.maThuoc = t.maThuoc
        WHERE t.soLuong < i.soLuong
    )
    BEGIN
        RAISERROR (N'Không đủ số lượng thuốc trong kho!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Trừ kho
    UPDATE t
    SET t.soLuong = t.soLuong - i.soLuong
    FROM Thuoc t
    JOIN inserted i ON t.maThuoc = i.maThuoc;
END;
GO

-- 4. Trigger Hoàn trả số lượng thuốc khi xóa thuốc khỏi đơn
CREATE TRIGGER trg_HoanTraSoLuongThuoc
ON ChiTietDonThuoc
AFTER DELETE
AS
BEGIN
    -- Cộng lại kho
    UPDATE t
    SET t.soLuong = t.soLuong + d.soLuong
    FROM Thuoc t
    JOIN deleted d ON t.maThuoc = d.maThuoc;
END;
GO
