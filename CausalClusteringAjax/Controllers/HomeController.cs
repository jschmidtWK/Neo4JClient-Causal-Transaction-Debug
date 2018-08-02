using System;
using System.Web.Mvc;
using Library.Data;
using Library.Neo4J;

namespace CausalClusteringAjax.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(Guid? Id)
        {
            Driver driver = new Driver();
            if(Id.HasValue  && Id != Guid.Empty)
            {
                driver = NeoStore.Get<Driver>(Id.Value);
            }
            return View(driver);
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
    }
}