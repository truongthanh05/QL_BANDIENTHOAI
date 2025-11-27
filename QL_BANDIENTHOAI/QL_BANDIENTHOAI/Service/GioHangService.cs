using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using QL_BANDIENTHOAI.Models;

namespace QL_BANDIENTHOAI.Services
{
    public class GioHangService
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["SqlDbContext"].ConnectionString;

        // Lấy danh sách giỏ hàng
        public List<CartItem> GetCart(string matk)
        {
            var list = new List<CartItem>();

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                string sql = @"
                    SELECT gh.MaSP, sp.TenSP, sp.AnhSanPham, gh.DonGia, gh.SoLuong
                    FROM CHITIETGH gh
                    JOIN SANPHAM sp ON gh.MaSP = sp.MaSP
                    WHERE gh.MaTK = @tk";

                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tk", matk);

                var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    list.Add(new CartItem
                    {
                        MaSp = r.GetString(0),
                        TenSp = r.GetString(1),
                        AnhSanPham = r.GetString(2),
                        GiaBan = Convert.ToDouble(r.GetValue(3)),
                        SoLuong = r.GetInt32(4)
                    });
                }
            }

            return list;
        }

        // Lấy tổng số lượng SP trong giỏ
        public int GetCartCount(string matk)
        {
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                string sql = @"SELECT SUM(SoLuong) FROM CHITIETGH WHERE MaTK = @tk";

                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tk", matk);

                var result = cmd.ExecuteScalar();

                return (result == null || result == DBNull.Value)
                    ? 0
                    : Convert.ToInt32(result);
            }
        }

        // Thêm hoặc tăng số lượng sản phẩm
        public void AddToCart(string matk, string masp)
        {
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                // KIỂM TRA SP TỒN TẠI TRONG GIỎ CHƯA — SỬA LỖI TenTK → MaTK
                string check = @"SELECT SoLuong FROM CHITIETGH WHERE MaTK = @tk AND MaSP = @sp";

                var cmd = new SqlCommand(check, conn);
                cmd.Parameters.AddWithValue("@tk", matk);
                cmd.Parameters.AddWithValue("@sp", masp);

                var rs = cmd.ExecuteScalar();

                if (rs != null && rs != DBNull.Value)
                {
                    // UPDATE +1
                    string update = @"UPDATE CHITIETGH 
                                      SET SoLuong = SoLuong + 1 
                                      WHERE MaTK = @tk AND MaSP = @sp";

                    var cmdU = new SqlCommand(update, conn);
                    cmdU.Parameters.AddWithValue("@tk", matk);
                    cmdU.Parameters.AddWithValue("@sp", masp);
                    cmdU.ExecuteNonQuery();
                }
                else
                {
                    // Lấy giá sản phẩm
                    string getPrice = @"SELECT GiaBan FROM SANPHAM WHERE MaSP = @id";
                    var cmdP = new SqlCommand(getPrice, conn);
                    cmdP.Parameters.AddWithValue("@id", masp);

                    double gia = Convert.ToDouble(cmdP.ExecuteScalar());

                    // Insert mới
                    string insert = @"
                        INSERT INTO CHITIETGH (MaTK, MaSP, SoLuong, DonGia)
                        VALUES (@tk, @sp, 1, @gia)";

                    var cmdI = new SqlCommand(insert, conn);
                    cmdI.Parameters.AddWithValue("@tk", matk);
                    cmdI.Parameters.AddWithValue("@sp", masp);
                    cmdI.Parameters.AddWithValue("@gia", gia);
                    cmdI.ExecuteNonQuery();
                }
            }
        }

        // Giảm số lượng
        public void Minus(string matk, string masp)
        {
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                string sql = @"SELECT SoLuong FROM CHITIETGH WHERE MaTK = @tk AND MaSP = @sp";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tk", matk);
                cmd.Parameters.AddWithValue("@sp", masp);

                int sl = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                if (sl <= 1)
                {
                    string del = @"DELETE FROM CHITIETGH WHERE MaTK = @tk AND MaSP = @sp";
                    var cmdD = new SqlCommand(del, conn);
                    cmdD.Parameters.AddWithValue("@tk", matk);
                    cmdD.Parameters.AddWithValue("@sp", masp);
                    cmdD.ExecuteNonQuery();
                }
                else
                {
                    string update = @"UPDATE CHITIETGH SET SoLuong = SoLuong - 1 
                                      WHERE MaTK = @tk AND MaSP = @sp";

                    var cmdU = new SqlCommand(update, conn);
                    cmdU.Parameters.AddWithValue("@tk", matk);
                    cmdU.Parameters.AddWithValue("@sp", masp);
                    cmdU.ExecuteNonQuery();
                }
            }
        }

        // Xóa sản phẩm
        public void Remove(string matk, string masp)
        {
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                string sql = @"DELETE FROM CHITIETGH WHERE MaTK = @tk AND MaSP = @sp";

                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tk", matk);
                cmd.Parameters.AddWithValue("@sp", masp);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
