using Library.Data;
using Library.Neo4J;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CausalClusteringAjax.Controllers
{
    public class DriverController : Controller
    {
        // GET: Driver
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Edit(Guid id, string companyIdentifier, string lastname, string firstname, string neoUrl, string neoPort, string neoUser, string neoPassword)
        {
            try
            {
                Driver driver = new Driver();
                if (!string.IsNullOrEmpty(neoUrl))
                {
                    NeoStore.neo4jIP = neoUrl;
                    NeoStore.neo4jLogin = neoUser;
                    NeoStore.neo4jPassword = neoPassword;
                    NeoStore.neo4jPort = neoPort;
                }

                if (NeoStore.Connect())
                {
                    using (var tx = NeoStore.BeginTransaction())
                    {
                        if (id != Guid.Empty)
                        {
                            driver = NeoStore.Get<Driver>(id);
                        }

                        driver.CompanyIdentifier = companyIdentifier;
                        driver.Firstname = firstname;
                        driver.Lastname = lastname;

                        driver.Update();
                        tx.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

            return Json(new { success = true, responseText = "OK", driverId = "", isNew = false });
        }
    }
}