using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLPKDTO
{
    public class toathuocDTO
    {
        private int maToa;
        private DateTime ngayKetoa;
        private int maPkb;
        public int MaToa { get => maToa; set => maToa = value; }
        public int MaPkb { get => maPkb; set => maPkb = value; }
        public DateTime NgayKetoa { get => ngayKetoa; set => ngayKetoa = value; }
    }
}
