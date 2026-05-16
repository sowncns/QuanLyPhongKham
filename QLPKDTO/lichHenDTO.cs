using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLPKDTO
{
    public class lichHenDTO
    {
        private int maLichHen;
        private int maBenhNhan;
        private int maTaiKhoan; //cua benh nhan
        private DateTime ngayHen;
        private string trangThai;
        private int maDieuDuong;
        public int MaLichHen { get => maLichHen; set => maLichHen = value; }
        public int MaBenhNhan { get => maBenhNhan; set => maBenhNhan = value; }
        public int MaTaiKhoan { get => maTaiKhoan; set => maTaiKhoan = value; }
        public DateTime NgayHen { get => ngayHen; set => ngayHen = value; }
        public string TrangThai { get => trangThai; set => trangThai = value; }
        public int MaDieuDuong { get => maDieuDuong; set => maDieuDuong = value; }
    }
}
