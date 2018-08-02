using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CausalClusteringAjax.Controllers
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


        [HttpPost]
        public async Task<JsonResult> Edit(string id, string companyIdentifier, string lastname, string firstName)
        {
            //Driver driver = (!string.IsNullOrWhiteSpace(id)) ? Finder.GetDriver(new Guid(id)) : new Driver();
            
            //bool isNewDriver = false;
            //driver.CompanyIdentifier = companyIdentifier;
            //driver.Firstname = firstName;
            //driver.Lastname = lastname;

            try
            {
                //Account masterAccount = Finder.GetMasterAccountFromDriver(driver.Id);
                //using (var tx = Finder.BeginNeo4JTransaction())
                //{
                //    driver.Update();

                //    string[] separators = { ";" };
                //    string[] mtags = tags.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                //    List<string> tagsList = new List<string>();
                //    if (mtags != null)
                //    {
                //        tagsList = mtags.ToList();
                //    }

                //    driver.UpdateTags(this.CurrentAccount, tagsList);
                //    if (string.IsNullOrWhiteSpace(id))
                //    {
                //        isNewDriver = true;

                //        if (masterAccount != null)
                //        {
                //            masterAccount.Hires(driver);
                //        }
                //        this.CurrentAccount.Hires(driver);
                //    }
                //    tx.Commit();
                //}
                //if (isNewDriver)
                //{
                //    // If it is a new driver, we send him an email with the password for the mobile app.
                //    HttpResponseMessage response = await Post($"api/driversapp/generatepassword/", new { @Email = driver.Email }, ApiAccessToken);

                //}
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Une erreur est survenue lors de la création du conducteur" });
            }

            return Json(new { success = true, responseText = "OK", driverId = driver.Id.ToString(), isNew = isNewDriver });
        }
    }
}