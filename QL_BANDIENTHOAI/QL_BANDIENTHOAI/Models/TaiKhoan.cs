using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_BANDIENTHOAI.Models
{
    public class TaiKhoan
    {
        public string Id { get; set; }
        public string Matk { get; set; }
        public string TenTk { get; set; }
        public string MatKhau { get; set; }

        // Navs
        public NguoiDung NguoiDung { get; set; }
        public ICollection<GioHang> GioHangs { get; set; }

        public TaiKhoan()
        {
            GioHangs = new List<GioHang>();
        }
    }
}