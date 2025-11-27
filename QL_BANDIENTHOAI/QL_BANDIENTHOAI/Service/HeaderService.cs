using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using QL_BANDIENTHOAI.Models;

namespace QL_BANDIENTHOAI.Services
{
    public class HeaderService
    {
        private readonly string cs = ConfigurationManager
            .ConnectionStrings["SqlDbContext"].ConnectionString;

        // ========================
        // 1. LẤY DANH SÁCH CỬA HÀNG (KHO)
        // ========================
        public List<Kho> GetStores()
        {
            var list = new List<Kho>();

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                string sql = "SELECT MAKHO, TENKHO, DIACHI FROM KHO";

                var cmd = new SqlCommand(sql, conn);
                var rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new Kho
                    {
                        MaKho = rd.GetString(0),
                        TenKho = rd.IsDBNull(1) ? "" : rd.GetString(1),
                        DiaChi = rd.IsDBNull(2) ? "" : rd.GetString(2)
                    });
                }
            }

            return list;
        }

        // ========================
        // 2. LẤY ĐƠN HÀNG THEO MATK (JOIN TAIKHOAN → NGUOIDUNG.ID → HOADON.ID)
        // ========================
        public List<HoaDon> GetOrders(string matk)
        {
            var list = new List<HoaDon>();

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                string sql = @"
                    SELECT hd.MAHD, hd.NGAYLAP, hd.THANHTIEN
                    FROM HOADON hd
                    JOIN TAIKHOAN tk ON tk.ID = hd.ID
                    WHERE tk.MATK = @m";

                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@m", matk);

                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(new HoaDon
                    {
                        MaHd = rd.GetString(0),
                        NgayLap = rd.IsDBNull(1) ? DateTime.Now : rd.GetDateTime(1),
                        ThanhTien = rd.IsDBNull(2) ? 0 : Convert.ToDouble(rd.GetValue(2))
                    });
                }
            }

            return list;
        }

        // ========================
        // 3. LẤY KHUYẾN MÃI ĐANG Active
        // ========================
        public List<PhieuKhuyenMai> GetVouchers()
        {
            var list = new List<PhieuKhuyenMai>();

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                string sql = @"
                    SELECT MAPKM, LOAIPHIEU, GIATRI, NGAYHETHAN
                    FROM PHIEUKHUYENMAI
                    WHERE TRANGTHAI = 'Active'";

                var cmd = new SqlCommand(sql, conn);
                var rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new PhieuKhuyenMai
                    {
                        MaPKM = rd.GetString(0),
                        LoaiPhieu = rd.IsDBNull(1) ? "" : rd.GetString(1),
                        GiaTri = rd.IsDBNull(2) ? 0 : Convert.ToInt32(rd.GetValue(2)),
                        NgayHetHan = rd.IsDBNull(3) ? DateTime.Now : rd.GetDateTime(3)
                    });
                }
            }

            return list;
        }
    }
}
