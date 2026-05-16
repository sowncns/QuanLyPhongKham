using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLPKDTO
{
    public class ChiTietToaThuocDTO
    {
        private int maThuoc;
        private int maToa;
        private int soLuong;

        public int MaThuoc { get => maThuoc; set => maThuoc = value; }
        public int MaToa { get => maToa; set => maToa = value; }
        public int SoLuong { get => soLuong; set => soLuong = value; }
    }
}
