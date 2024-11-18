using NCIASupplier;
using NCIASupplier.Models;
using NCIASupplier.NAVWS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NCIASupplier.Controllers
{
    public class LoginController : Controller
    {
        Supplier webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
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
                string password = account.Password;
                if (webportals.ValidVATno(VATno))
                {
                    //if (webportals.AccountActivated(emailAddress))
                    //{
                        string bidderLogin = webportals.CheckBidderLogin(VATno, password);
                        if (!string.IsNullOrEmpty(bidderLogin))
                        {
                            string[] bidderLoginArr = bidderLogin.Split(strLimiters, StringSplitOptions.None);
                            string returnMsg = bidderLoginArr[0];
                            if (returnMsg == "SUCCESS")
                            {
                                string bidderNo = bidderLoginArr[1];
                                string bidderName = bidderLoginArr[2];
                                string bidderEmail = bidderLoginArr[3];
                                string bidderVat = bidderLoginArr[4];

                                Session["BidderNo"] = bidderNo;
                                Session["BidderName"] = bidderName;
                                Session["BidderEmail"] = bidderEmail;
                                Session["BidderVat"]=bidderVat;
                                //return RedirectToAction("index", "dashboard");

                                string otp = GenerateOtp(6);
                                Session["otp"] = otp;
                                string subject = "NCIA E-Tender Portal OTP";
                                string body = $"{otp} is your OTP Code for NCIA eTendering Portal. Use it to verify login.";
                                Components.SentEmailAlerts(bidderEmail, subject, body);
                            //  return RedirectToAction("verifyotp");
                            return RedirectToAction("index", "dashboard");
                        }
                            else
                            {
                                TempData["Error"] = returnMsg;
                            }
                        }
                    //}
                    //else
                    //{
                    //    TempData["Error"] = "Your account has not been activated.";
                    //    //SendPasswordResetLink(emailAddress);
                    //}
                }
                else
                {
                    TempData["Error"] = "Invalid VAT No.";
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View("index");
        }

        public ActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyOtp(OtpVerification verification)
        {
            string generatedOtp = Session["otp"].ToString();
            string userOtp = verification.OTPCode;
            if (generatedOtp.ToLower() == userOtp.ToLower())
            {
                return RedirectToAction("index", "dashboard");
            }
            else
            {
                //TempData["Error"] = "OTP do not match. Please try again later"; TODO : TO BE CHANGED
                return RedirectToAction("index", "dashboard");
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ResetPassword reset)
        {
            try
            {
                string username = reset.UserName;
                SendPasswordResetLink(username);
                TempData["Success"] = "Please Follow the link sent to your email to set a password for your account.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("forgotpassword");
            }
            return View("forgotpassword");
        }

        public ActionResult ResetPassword(string email)
        {
            Session["EmailAddress"] = email;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPassword reset)
        {
            try
            {
                string newPassword = reset.Password;
                string confirmPassword = reset.PasswordConfirmation;
                string email = Session["EmailAddress"].ToString();

                    string response = webportals.ChangeBidderPassword(email, newPassword);
                    if (!string.IsNullOrEmpty(response))
                    {
                        if (response == "SUCCESS")
                        {
                            TempData["Success"] = "Password has been updated successfully";
                            return RedirectToAction("index", "login");
                        }
                        else
                        {
                            TempData["Error"] = "An error occured while updating your password. Please try again later.";
                            return RedirectToAction("resetpassword", "login");
                        }
                    }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View();
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(Registration registration)
        {
            try
            {
                string path = Server.MapPath("~/Attachments");
                if(registration.KRACert !=null && registration.KRACert.ContentLength > 0)
                {
                    string filename = $"{registration.KRAPin} KRA Certificate.pdf";
                    
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string pathToUpload = Path.Combine(path,filename);

                    if (System.IO.File.Exists(pathToUpload))
                    {
                        System.IO.File.Delete(pathToUpload);
                    }
                    
                    registration.KRACert.SaveAs(pathToUpload);
                    Stream fs = registration.KRACert.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string name = "KRA Certificate.pdf";
                    var response = webportals.DocumentAttachment(registration.KRAPin, name, base64String, 52178793);
                }
                if (registration.IncopCert != null && registration.IncopCert.ContentLength > 0)
                {
                    string filename = registration.KRAPin + " Certificate of Incorporation.pdf";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string pathToUpload = Path.Combine(path, filename);

                    if (System.IO.File.Exists(pathToUpload))
                    {
                        System.IO.File.Delete(pathToUpload);
                    }
                   
                    registration.IncopCert.SaveAs(pathToUpload);
                    Stream fs = registration.IncopCert.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string name = "Certificate of Incorporation.pdf";
                    var response2 = webportals.DocumentAttachment(registration.KRAPin, name, base64String, 52178793);
                }
                if (registration.CompCert != null && registration.CompCert.ContentLength > 0)
                {
                    string filename = registration.KRAPin + " KRA Compliance Certificate.pdf";
                    
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string pathToUpload = Path.Combine(path, filename);

                    if (System.IO.File.Exists(pathToUpload))
                    {
                        System.IO.File.Delete(pathToUpload);
                    }
                    
                    registration.CompCert.SaveAs(pathToUpload);
                    Stream fs = registration.CompCert.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string name = "KRA Compliance Certificate.pdf";
                    var response3 = webportals.DocumentAttachment(registration.KRAPin, name, base64String, 52178793);
                }
                if(registration.Password != registration.ConfirmPassword || registration.Password == null || registration.ConfirmPassword==null)
                {
                    TempData["Error"] = "Please check your passwords and try again";
                    return View("createaccount");

                }
                string sessionKey = GenerateOtp(10);
                if (webportals.CreateBidderAccount(registration.KRAPin.ToUpper(), registration.CompanyName,
                    registration.Address, registration.CompanyPhone, registration.CompanyEmail,
                    registration.ContactPerson, registration.ContactPhone, registration.ContactEmail, sessionKey,registration.Password))
                {
                    SentAccountActivationLink(registration.CompanyName, sessionKey, registration.CompanyEmail, registration.KRAPin);
                    TempData["Success"] = "Please check your email to set a password for your account";
                    return RedirectToAction("index");
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("createaccount");
            }
            return View();
        }

        private void SentAccountActivationLink(string companyName, string sessionKey, string companyEmail, string kraPin)
        {
            try
            {
                string subject = "NCIA Tenders Portal Account";
                string body = $"Dear {companyName}," +
                    $"<br/><br/>" +
                    $"Please follow the link below to confirm Your NCIA Tenders Portal Account Creation." +
                    $"<br/><br/>" +
                    $"<a href ='" + string.Format("{0}://{1}/registration/activation?username={2}&sessionkey={3}&email={4}", Request.Url.Scheme, Request.Url.Authority, kraPin, sessionKey, companyEmail) + "'>Click here</a>";
                Components.SentEmailAlerts(companyEmail, subject, body);
            }
            catch( Exception ex )
            {
                ex.Data.Clear();
            }
        }

        public void SendPasswordResetLink(string email)
        {
            try
            {
                string bidderName = webportals.GetBidderName(email);
                string subject = "NCIA Tenders Account Password Reset";
                string body = $"Hello {bidderName};" +
                    $"<br/><br/>" +
                    $"Please follow the link below to reset your NCIA Tenders Account Password." +
                    $"<br/><br/>" +
                    $"<a href='{String.Format(@"{0}://{1}/login/resetpassword?username={2}", Request.Url.Scheme, Request.Url.Authority, email)}'>Click here.</a>" +
                    $"<br/><br/>" +
                    $"Regards, Administrator";
                Components.SentEmailAlerts(email, subject, body);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
        }

        private static string GenerateOtp(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }

        public JsonResult ValidatePin(string email)
        {
            var exists = new object();
            try
            {
                if (webportals.ValidateEmail(email))
                {
                    exists = "True";
                }
                else
                {
                    exists = "False";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
            }
            return Json(exists, JsonRequestBehavior.AllowGet);
        }
    }
}