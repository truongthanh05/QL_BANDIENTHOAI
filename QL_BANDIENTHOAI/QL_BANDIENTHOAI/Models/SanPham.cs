using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_BANDIENTHOAI.Models
{
    public class SanPham
    {
        public string MaLoai { get; set; }
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public double? GiaBan { get; set; }
        public string MoTa { get; set; }

        public LoaiSp LoaiSp { get; set; }
        public ICollection<ChiTietHD> ChiTietHds { get; set; }
        public ICollection<ChiTietPN> ChiTietPns { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }

        public SanPham()
        {
            ChiTietHds = new List<ChiTietHD>();
            ChiTietPns = new List<ChiTietPN>();
            ChiTietGioHangs = new List<ChiTietGioHang>();
        }
    }
}