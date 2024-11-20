using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NCIAVendor.NAVWS;

namespace NCIAVendor.Controllers
{
    public class DashboardController : Controller
    {
        VendorsPortal webportals = Components.ObjNav;
            string[] strLimiters = new string[] { "::" };
            public ActionResult Index()
            {
                if (Session["VendorNo"] == null) return RedirectToAction("index", "login");
                string username = Session["VendorNo"].ToString();
                GetVendorData(username);
                return View();
            }

            public ActionResult Logout()
            {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                return RedirectToAction("index", "login");
            }

            private void GetVendorData(string username)
            {
                try
                {
                    string response = webportals.GetVendorDetails(username);
                    if (response != null)
                    {
                        string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                        Session["CompanyName"] = responseArr[0];
                        Session["Address"] = responseArr[1];
                        Session["ContactPerson"] = responseArr[2];
                        Session["Email"] = responseArr[3];
                        Session["vat"] = responseArr[4];
                        Session["PhoneNo"] = responseArr[5];
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
            }
        }
    }