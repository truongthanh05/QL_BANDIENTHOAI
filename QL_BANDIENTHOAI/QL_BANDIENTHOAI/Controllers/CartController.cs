using System.Web.Mvc;
using QL_BANDIENTHOAI.Services;

namespace QL_BANDIENTHOAI.Controllers
{
    public class CartController : Controller
    {
        private readonly GioHangService _cart = new GioHangService();

        // =======================
        // GIỎ HÀNG
        // =======================
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Account");

            string matk = Session["User"].ToString();

            var list = _cart.GetCart(matk);

            return View(list);
        }

        // =======================
        // THÊM +1 (điều hướng)
        // =======================
        public ActionResult Add(string masp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Account");

            string matk = Session["User"].ToString();
            _cart.AddToCart(matk, masp);

            return RedirectToAction("Index");
        }

        // =======================
        // TRỪ -1
        // =======================
        public ActionResult Minus(string masp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Account");

            string matk = Session["User"].ToString();
            _cart.Minus(matk, masp);

            return RedirectToAction("Index");
        }

        // =======================
        // XOÁ SP
        // =======================
        public ActionResult Remove(string masp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Account");

            string matk = Session["User"].ToString();
            _cart.Remove(matk, masp);

            return RedirectToAction("Index");
        }

        // =======================
        // ADD AJAX
        // =======================
        [HttpPost]
        public JsonResult AddAjax(string masp)
        {
            if (Session["User"] == null)
                return Json(new { success = false, message = "not_login" });

            string matk = Session["User"].ToString();

            _cart.AddToCart(matk, masp);

            int count = _cart.GetCartCount(matk);

            return Json(new { success = true, cart = count });
        }
    }
}
