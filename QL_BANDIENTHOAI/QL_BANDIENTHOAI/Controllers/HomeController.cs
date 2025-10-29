using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QL_BANDIENTHOAI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }
        public ActionResult DbTest()
        {
            var cs = ConfigurationManager.ConnectionStrings["SqlServerDbContext"].ConnectionString;

            try
            {
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open(); // Test kết nối

                    // Truy vấn 1 lệnh: lấy 1 dòng từ LOAISP
                    var sql = "SELECT TOP (1) MALOAI, TENLOAI FROM LOAISP ORDER BY MALOAI";

                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("OK ✅ Connected");
                        if (reader.Read())
                        {
                            var maLoai = reader["MALOAI"] as string;
                            var tenLoai = reader["TENLOAI"] as string;
                            sb.AppendLine("Sample row from LOAISP:");
                            sb.AppendLine($"- MALOAI = {maLoai}");
                            sb.AppendLine($"- TENLOAI = {tenLoai}");
                        }
                        else
                        {
                            sb.AppendLine("LOAISP has no rows.");
                        }
                        return Content(sb.ToString(), "text/plain", Encoding.UTF8);
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("FAIL ❌ " + ex.Message, "text/plain", Encoding.UTF8);
            }
        }
    }
}