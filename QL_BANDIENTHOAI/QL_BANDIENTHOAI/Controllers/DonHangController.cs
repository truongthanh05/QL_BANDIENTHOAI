using System.Web.Mvc;
using QL_BANDIENTHOAI.Services;

namespace QL_BANDIENTHOAI.Controllers
{
    public class DonHangController : Controller
    {
        private readonly HeaderService service = new HeaderService();

        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Account");

            string matk = Session["User"].ToString();
            var orders = service.GetOrders(matk);

            return View(orders);
        }
    }
}
