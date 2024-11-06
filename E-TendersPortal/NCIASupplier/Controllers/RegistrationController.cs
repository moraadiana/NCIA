using NCIASupplier.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NCIASupplier.Controllers
{
    public class RegistrationController : Controller
    {
        Supplier webportals = Components.ObjNav;
        public ActionResult Activation()
        {
            if (Request.QueryString["username"] != null && Request.QueryString["sessionkey"] != null)
            {
                if (!Activated())
                {
                    TempData["Error"] = "Account activation failed, contact the admin";

                    return RedirectToAction("index", "login");
                }
                else
                {
                    TempData["Success"] = "Account activation successful, set your account password";

                    return RedirectToAction("resetpassword", "login", new { username = Request.QueryString["username"], email = Request.QueryString["email"] });
                }
            }
            TempData["Error"] = "Invalid account activation details, contact the admin";
            return RedirectToAction("index", "login");
        }

        protected Boolean Activated()
        {
            bool b = false;
            try
            {
                b = webportals.ActivateBidderAccount(Request.QueryString["username"], Request.QueryString["sessionkey"]);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return b;
        }
    }
}