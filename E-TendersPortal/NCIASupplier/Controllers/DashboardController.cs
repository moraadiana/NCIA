using NCIASupplier;
using NCIASupplier.Models;
using NCIASupplier.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NCIASupplier.Controllers
{
    public class DashboardController : Controller
    {
        Supplier webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
        {
            if (Session["BidderNo"] == null) return RedirectToAction("index", "login");
            BidderDetails bidder = new BidderDetails();
            try
            {
                string username = Session["BidderNo"].ToString();
                string response = webportals.GetBidderDetails(username);
                if (response != null)
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    bidder.BidderNo = responseArr[0];
                    bidder.CompanyName = responseArr[1];
                    bidder.Address = responseArr[2];
                    bidder.City = responseArr[3];
                    bidder.PhoneNo = responseArr[4];
                    bidder.Email = responseArr[5];
                    bidder.ContactPerson = responseArr[6];
                    bidder.CompanyContact = responseArr[7];
                    bidder.ContactPersonEmail = responseArr[8];
                    bidder.CompanyEmail = responseArr[9];
                    bidder.VatRegistrationNo = responseArr[10];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("index");
            }
            return View(bidder);
        }

        public ActionResult CompanyProfile()
        {
            if (Session["BidderNo"] == null) return RedirectToAction("index", "login");
            BidderDetails bidder = new BidderDetails();
            try
            {
                string username = Session["BidderNo"].ToString();
                string response = webportals.GetBidderDetails(username);
                if (response != null)
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    bidder.BidderNo = responseArr[0];
                    bidder.CompanyName = responseArr[1];
                    bidder.Address = responseArr[2];
                    bidder.City = responseArr[3];
                    bidder.PhoneNo = responseArr[4];
                    bidder.Email = responseArr[5];
                    bidder.ContactPerson = responseArr[6];
                    bidder.CompanyContact = responseArr[7];
                    bidder.ContactPersonEmail = responseArr[8];
                    bidder.CompanyEmail = responseArr[9];
                    bidder.VatRegistrationNo = responseArr[10];
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("index");
            }
            return View(bidder);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("index","default");
        }
    }
}