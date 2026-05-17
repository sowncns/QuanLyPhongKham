using System;
using System.Collections.Generic;
using QLPKDTO; // Gọi tầng DTO
using QLPKDAL; // Gọi tầng DAL

namespace QLPKBUS
{
    public class BenhBUS
    {
        private BenhDAL beDAL;

        public BenhBUS()
        {
            beDAL = new BenhDAL();
        }

        public bool ThemBenh(benhDTO be)
        {
            if (string.IsNullOrEmpty(be.TenBenh))
            {
                throw new Exception("Tên bệnh không được phép để trống!");
            }
            return beDAL.them(be);
        }

        public bool SuaBenh(benhDTO be)
        {
            if (string.IsNullOrWhiteSpace(be.TenBenh))
            {
                throw new Exception("Tên bệnh không được để trống!");
            }

            if (be.MaBenh <= 0)
            {
                throw new Exception("Mã bệnh không hợp lệ!");
            }

            return beDAL.sua(be);
        }

        public bool XoaBenh(benhDTO be)
        {
            // Kiểm tra an toàn: Nếu mã bệnh <= 0 tức là chưa chọn bệnh hợp lệ để xóa
            if (be.MaBenh <= 0)
            {
                throw new Exception("Mã bệnh cần xóa không hợp lệ!");
            }
            return beDAL.xoa(be);
        }

        public List<benhDTO> select()
        {
            return beDAL.select();
        }

        public List<benhDTO> selectByKeyWord(string sKeyword)
        {
            if (sKeyword == null) sKeyword = string.Empty;
            return beDAL.selectByKeyWord(sKeyword);
        }
    }
}