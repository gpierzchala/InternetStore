using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    public class ManageOrdersController : Controller
    {
        // GET: Admin/ManageOrders
        public ActionResult Index()
        {
            return View();
        }
    }
}