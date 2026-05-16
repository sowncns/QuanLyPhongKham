-- ============================================================================
-- FILE 03: TẠO CÁC HÀM (FUNCTIONS)
-- Dự án: Quản Lý Phòng Khám
-- Thao tác: Tạo Scalar và Table-Valued Functions
-- ============================================================================

USE QLPK;
GO

-- 1. Hàm tính tổng tiền thuốc của một Phiếu khám bệnh (Scalar Function)
CREATE FUNCTION f_TinhTienThuoc (@maPKB INT)
RETURNS FLOAT
AS
BEGIN
    DECLARE @TongTien FLOAT;
    
    SELECT @TongTien = SUM(th.donGia * ctdt.soLuong)
    FROM ToaThuoc tt
    JOIN ChiTietDonThuoc ctdt ON tt.maToaThuoc = ctdt.maToaThuoc
    JOIN Thuoc th ON ctdt.maThuoc = th.maThuoc
    WHERE tt.maPKB = @maPKB;
    
    RETURN ISNULL(@TongTien, 0);
END;
GO

-- 2. Hàm lấy lịch sử khám bệnh (Table-Valued Function)
-- Trả về: Lịch sử khám, tên thuốc đã kê, tổng chi phí
CREATE FUNCTION f_LichSuKhamBenh (@maBenhNhan INT)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        pkb.maPKB,
        pkb.ngayKham,
        pkb.trieuChung,
        STRING_AGG(th.tenThuoc, ', ') AS ToaThuoc,
        (dbo.f_TinhTienThuoc(pkb.maPKB) + ISNULL(hd.tienKham, 0)) AS TongChiPhi
    FROM PhieuKhamBenh pkb
    LEFT JOIN ToaThuoc tt ON pkb.maPKB = tt.maPKB
    LEFT JOIN ChiTietDonThuoc ctdt ON tt.maToaThuoc = ctdt.maToaThuoc
    LEFT JOIN Thuoc th ON ctdt.maThuoc = th.maThuoc
    LEFT JOIN HoaDon hd ON pkb.maPKB = hd.maPKB
    WHERE pkb.maBenhNhan = @maBenhNhan
    GROUP BY pkb.maPKB, pkb.ngayKham, pkb.trieuChung, hd.tienKham
);
GO
-- Lưu ý: Hàm STRING_AGG yêu cầu SQL Server 2017 trở lên.
-- Nếu dùng bản cũ hơn, cần dùng FOR XML PATH hoặc trả về nhiều dòng.
