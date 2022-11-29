using Microsoft.AspNetCore.Mvc;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.Web.Internet.Settings;

namespace Northwind.Store.Web.Internet.Controllers
{
    public class CartController : Controller
    {
        private readonly NWContext _db;
        private readonly SessionSettings _ss;
        private readonly RequestSettings _rs;
        public CartController(NWContext db, SessionSettings ss)
        {
            _db = db;
            _ss = ss;
            _rs = new RequestSettings(this);

        }

        public IActionResult Index()
        {
            var productId = TempData[nameof(Product.ProductId)];
            ViewBag.productAdded = _rs.ProductAdded;

            return View(_ss.Cart);
        }

        public ActionResult Add(int? id)
        {
            if (id.HasValue)
            {
                #region Session
                var cart = _ss.Cart;

                if (!cart.Items.Any(i => i.ProductId == id))
                {
                    var p = _db.Products.Find(id);
                    cart.Items.Add(p);
                    _ss.Cart = cart;

                    #region TempData
                    TempData[nameof(Product.ProductId)] = p.ProductId;
                    TempData[nameof(Product.ProductName)] = p.ProductName;
                    TempData["success"] = "Product added successfully";

                    _rs.ProductAdded = p;
                    #endregion
                }
                #endregion
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult Checkout() {
            if (_ss.Cart != null && _ss.Cart.Count > 0) {
                foreach (var item in _ss.Cart.Items) {
                    var order = new OrderDetail();
                    order.OrderId = 11075;
                    order.ProductId = item.ProductId;
                    order.UnitPrice = ((decimal?)item.UnitPrice > 0) ? (decimal)item.UnitPrice : 0;
                    order.Quantity = 1;

                    _db.OrderDetails.Add(order);
                }
                _db.SaveChanges();
                _ss.CustomClearSession();
                TempData["success"] = "Checkout process done";
            }
            return RedirectToAction("Index", "Product");
        }
    }
}
