using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLPKDTO
{
    public class chandoanDTO
    {
        private int maPkb;
        private int maBenh;
        private string tenChuanDoan;
        private string trieuChung;
        public int MaPkb { get => maPkb; set => maPkb = value; }
        public int MaBenh { get => maBenh; set => maBenh = value; }
        public string TenChuanDoan { get => tenChuanDoan; set => tenChuanDoan = value; }
        public string TrieuChung { get => trieuChung; set => trieuChung = value; }
    }
}
