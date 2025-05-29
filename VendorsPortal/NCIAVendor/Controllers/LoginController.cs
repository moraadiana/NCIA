using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NCIAVendor.Models;
using NCIAVendor.NAVWS;

namespace NCIAVendor.Controllers
{
    public class LoginController : Controller
    {

        VendorsPortal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        /*  public ActionResult Index()
          {
              return View();
          }

          [HttpPost]
          public ActionResult Login(Account account)
           {
              try
              {
                  string VATno = account.VATno;
                  string emailAddress = account.EmailAddress;
                  string password = account.Password.Trim();
                  if (webportals.CheckValidVendorNo(VATno))
                  {
                      string response = webportals.CheckVendorLogin(VATno, password);
                      if (!string.IsNullOrEmpty(response))
                      {
                          string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                          string returnMsg = responseArr[0];
                          if (returnMsg == "SUCCESS")
                          {
                              string vendorNo = responseArr[1];
                              string vendorName = responseArr[2];
                              string vendorEmail = responseArr[3];
                              string vendorVat = responseArr[4];
                              Session["VendorNo"] = vendorNo;
                              Session["VendorName"] = vendorName;
                              Session["VendorEmail"] = vendorEmail;
                              Session["VendorVat"] = vendorVat;

                              string otp = GenerateOtp(6);
                              Session["otp"] = otp;

                              string subject = "NCIA Vendors Portal OTP";
                              string body = $"{otp} is your OTP Code for NCIA Vendors portal.";
                              Components.SendEmailAlerts(vendorEmail, subject, body);
                              return RedirectToAction("verifyotp");
                              //return RedirectToAction("index", "dashboard");
                          }
                          else
                          {
                              TempData["error"] = returnMsg;
                              return View("index");
                          }
                      }
                  }
                  else
                  {
                      TempData["error"] = "Invalid Vendor No. ";
                      return View("index");
                  }
              }
              catch (Exception ex)
              {
                  TempData["error"] = ex.Message;
                  return View("index");
              }
              return View();
          }

          public ActionResult VerifyOTP()
          {
              if (Session["VendorNo"] == null) return View("index");
              return View();
          }

          [HttpPost]
          public ActionResult VerifyOTP(OTP otp)
          {
              try
              {
                  string generatedOtp = Session["otp"].ToString();
                  string otpFromUser = otp.OTPCode.Trim();

                  if (generatedOtp.ToLower() == otpFromUser.ToLower())
                  {
                      return RedirectToAction("index", "dashboard");
                  }
                  else
                  {
                      TempData["error"] = "Invalid OTP. Please try again later";
                      return View("verifyotp");
                  }
              }
              catch (Exception ex)
              {
                  TempData["error"] = ex.Message;
                  return View("verifyotp");
              }
          }

          public static string GenerateOtp(int length)
          {
              var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
              var random = new Random();
              var result = new string(
                  Enumerable.Repeat(chars, length)
                            .Select(s => s[random.Next(s.Length)])
                            .ToArray());

              return result;
          }

          [ChildActionOnly]
          public PartialViewResult Notification()
          {
              return PartialView("_Notification");
          }
          public ActionResult ForgotPassword()
          {
              return View();
          }

          [HttpPost]
          public ActionResult ForgotPassword(Account account)
          {
              try
              {
                  string vatNo = account.VATno;
                  return RedirectToAction("loginforunchangedpassword", "login", new { vatNo });
              }
              catch (Exception ex)
              {
                  TempData["Error"] = ex.Message;
                  return RedirectToAction("forgotpassword", "login");
              }
          }*/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            try
            {
                string vatNo = account.VATNo;
                string password = account.Password;

                if (!webportals.CheckValidVatNo(vatNo))
                {
                    TempData["Error"] = "Invalid VAT Registration No";
                    return RedirectToAction("index", "login");
                }

                if (webportals.CheckVendorPasswordChanged(vatNo))
                {
                    return RedirectToAction("loginforchangedpassword", "login", new { vatNo, password });
                }
                else
                {
                    return RedirectToAction("loginforunchangedpassword", "login", new { vatNo });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "login");
            }
        }

        public ActionResult LoginForChangedPassword(string vatNo, string password)
        {
            try
            {
                string response = webportals.LoginForChangedPassword(vatNo, password);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string vendorNo = responseArr[0];
                    string vendorName = responseArr[1];
                    string email = responseArr[3];
                    Session["VendorNo"] = vendorNo;
                    Session["VATNo"] = vatNo;
                    Session["VendorName"] = vendorName;
                    Session["VendorEmail"] = email;
                    return RedirectToAction("index", "dashboard");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "login");
            }
            return View();
        }

        public ActionResult LoginForUnchangedPassword(string vatNo)
        {
            try
            {
                string response = webportals.LoginForUnchangedPassword(vatNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string vendorNo = responseArr[0];
                    Session["vendorNo"] = vendorNo;
                    return RedirectToAction("resetpassword", "login");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "login");
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(Account account)
        {
            try
            {
                string vatNo = account.VATNo;
                return RedirectToAction("loginforunchangedpassword", "login", new { vatNo });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("forgotpassword", "login");
            }
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Account account)
        {
            try
            {
                string vendorNo = Session["vendorNo"].ToString();
                string genpass = account.NewPassword;
                webportals.UpdateVendorPassword(vendorNo, genpass);
                TempData["Success"] = "Password has been updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("index", "login");
        }
    }
}