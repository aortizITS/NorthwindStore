using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Store.Data;
using Northwind.Store.Web.Internet.Models;
using Northwind.Store.Web.Internet.Settings;

namespace Northwind.Store.Web.Internet.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SessionSettings _ss;
        private readonly NWContext _db;

        public ProductController(ILogger<HomeController> logger, SessionSettings ss, NWContext db)
        {
            _logger = logger;
            _ss = ss;
            _db = db;
        }

        public IActionResult Index(string searchString, int pg = 1)
        {
            var starTime = HttpContext.Items["StartTime"];

            if (!String.IsNullOrEmpty(searchString))
            {
                return View(_db.Products.Where(p => p.ProductName.Contains(searchString)).ToList());
            }

            const int pageSize = 10;
            if (pg < 1) {
                pg = 1;
            }

            int recsCount = _db.Products.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _db.Products.Skip(recSkip).Take(pager.PageSize).Include(p => p.Category).ToList();
            this.ViewBag.Pager = pager;

            //return View(_db.Products.Include(p => p.Category).ToList());
            return View(data); 
        }
    }
}
