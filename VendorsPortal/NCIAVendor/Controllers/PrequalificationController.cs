using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NCIAVendor.NAVWS;
using NCIAVendor.Models;

namespace NCIAVendor.Controllers
{
    public class PrequalificationController : Controller
    {
        VendorsPortal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult PrequalificationApplication()
        {
            if (Session["VendorVat"] == null) return RedirectToAction("index", "login");
            Categories categories = new Categories();
            try
            {
                string vendorVat = Session["VendorVat"].ToString();
                var list = new List<Categories>();
                var list1 = new List<Categories>();

                string unappliedCategories = webportals.GetUnappliedPrequalificationCategories(vendorVat);
                if (!string.IsNullOrEmpty(unappliedCategories))
                {
                    string[] unappliedCategoriesArr = unappliedCategories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string unappliedCategory in unappliedCategoriesArr)
                    {
                        string[] responseArr = unappliedCategory.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Categories()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
                        });
                    }
                }

                string appliedCategories = webportals.GetAppliedPrequalificationCategories(vendorVat);
                if (!string.IsNullOrEmpty(appliedCategories))
                {
                    string[] appliedCategoriesArr = appliedCategories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string appliedCategory in appliedCategoriesArr)
                    {
                        string[] responseArr = appliedCategory.Split(strLimiters, StringSplitOptions.None);
                        list1.Add(new Categories()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
                        });
                    }
                }

                categories.AllCategories = list;
                categories.AppliedCategories = list1;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(categories);
        }

        public ActionResult AddCategory(string category)
        {
            Categories categories = new Categories();
            try
            {
                string vendorVat = Session["VendorVat"].ToString();
                ViewBag.Category = category;
                var list = new List<Categories>();
                string categoryRequirments = webportals.GetCategoryRequirements(category, vendorVat);
                if (!string.IsNullOrEmpty(categoryRequirments))
                {
                    string[] categoryRequirementsArr = categoryRequirments.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string categoryRequirement in categoryRequirementsArr)
                    {
                        string[] responseArr = categoryRequirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Categories()
                        {
                            Description = responseArr[0],
                            Mandatory = responseArr[1],
                            Attached = responseArr[2]
                        });
                    }
                    categories.AllCategories = list;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(categories);
        }

        [HttpPost]
        public ActionResult SubmitCategoryDocument(Categories categories)
        {
            string vendorVat = Session["VendorVat"].ToString();
            string vendorEmail = Session["VendorEmail"].ToString();
            string vendorNo = Session["VendorNo"].ToString();
            string path = Server.MapPath("~/Attachments");
            try
            {
                if (!webportals.PrequalificationApplied(vendorVat, vendorEmail))
                {
                    webportals.CreatePrequalificationHeader(vendorNo);
                }
                if (categories.DocFile != null && categories.DocFile.ContentLength > 0)
                {
                    string filename = vendorVat + categories.DocName;
                    string filepath = path + filename;
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    categories.DocFile.SaveAs(filepath);
                    System.IO.Stream fs = categories.DocFile.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string name = categories.DocName + ".pdf";
                    bool submitted = webportals.DocumentAttachment(vendorVat, name, base64String, 52178541);
                }
                TempData["Success"] = categories.DocName + " attached succesfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("addcategory", new { category = categories.Code });
            }
            return RedirectToAction("addcategory", new { category = categories.Code });
        }

        [HttpPost]
        public ActionResult SubmitCategory(Categories categories)
        {
            try
            {
                string vendorVat = Session["VendorVat"].ToString();
                string vendorEmail = Session["VendorEmail"].ToString();
                string vendorNo = Session["VendorNo"].ToString();
                if (webportals.NotAllMandatoryDocumentsAttached(vendorVat, categories.Code))
                {
                    TempData["Error"] = "Attach all the mandatory documents before submitting!";
                    return RedirectToAction("addcategory", "prequalification", new { category = categories.Code });
                }

                if (!webportals.PrequalificationApplied(vendorVat, vendorEmail))
                {
                    webportals.CreatePrequalificationHeader(vendorNo);
                }

                webportals.CreatePrequalificationLine(vendorEmail, vendorVat, categories.Code);
                TempData["Success"] = "Category added successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("addcategory", new { category = categories.Code });
            }
            return RedirectToAction("prequalificationapplication", "prequalification", new { category = categories.Code });
        }

        public ActionResult SubmitApplication()
        {
            try
            {
                string vendorVat = Session["VendorVat"].ToString();
                string vendorEmail = Session["VendorEmail"].ToString();
                if (!webportals.AddedCategory(vendorVat))
                {
                    TempData["Error"] = "You have not added any prequalification category to your application";
                    return RedirectToAction("prequalificationapplication", "prequalification");
                }
                if (webportals.SubmitPrequalificationApplication(vendorVat, vendorEmail))
                {
                    TempData["Success"] = "Prequalification application submitted successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("prequalificationapplication", "prequalification");
            }
            return RedirectToAction("prequalificationapplications", "prequalification");
        }

        public ActionResult ApplyPrequalification()
        {
            if (Session["VendorNo"] == null) return RedirectToAction("index", "login");
            var categories = new List<Categories>();
            try
            {
                string vatNo = Session["VendorVat"].ToString();
                string vendorEmail = Session["VendorEmail"].ToString();
                if (webportals.PrequalificationApplied(vatNo, vendorEmail))
                {
                    TempData["Error"] = "You have already submitted your prequalification application for the current period!";
                    return RedirectToAction("prequalificationapplications", "prequalification");
                }

                string preqCategories = webportals.GetPrequalificationCategories();
                if (!string.IsNullOrEmpty(preqCategories))
                {
                    string[] preqCategoriesArr = preqCategories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var preqCategory in preqCategoriesArr)
                    {
                        string[] responseArr = preqCategory.Split(strLimiters, StringSplitOptions.None);
                        Categories category = new Categories()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
                        };
                        categories.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(categories);
        }

        [HttpPost]
        public ActionResult SubmitPrequalificationApplication(Registration registration)
        {
            try
            {
                string vendorNo = Session["VendorNo"].ToString();
                string vendorEmail = Session["VendorEmail"].ToString();
                string vatNo = Session["VendorVat"].ToString(); // Attachments

                if (webportals.CreatePrequalificationHeader(vendorNo))
                {
                    string categories = registration.SelectedCategories;
                    string[] categoriesArr = categories.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var category in categoriesArr)
                    {
                        webportals.CreatePrequalificationLine(vendorEmail, vatNo, category);
                    }
                    if (webportals.SubmitPrequalificationApplication(vatNo, vendorEmail))
                    {
                        TempData["Success"] = "Prequalification application submitted successfully.";
                        return RedirectToAction("prequalificationapplications", "prequalification");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("applyprequalification");
            }
            return View();
        }

        public ActionResult PrequalificationApplications()
        {
            if (Session["VendorNo"] == null) return RedirectToAction("index", "login");
            var applications = new List<Registration>();
            try
            {
                string vendorVat = Session["VendorVat"].ToString();
                string vendorEmail = Session["VendorEmail"].ToString();
                string prequalificationApplications = webportals.GetPrequalificationApplications(vendorVat, vendorEmail);
                if (!string.IsNullOrEmpty(prequalificationApplications))
                {
                    string[] prequalificationApplicationsArr = prequalificationApplications.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    int counter = 0;
                    foreach (var prequalifcationApplication in prequalificationApplicationsArr)
                    {
                        counter++;
                        string[] responseArr = prequalifcationApplication.Split(strLimiters, StringSplitOptions.None);
                        var application = new Registration()
                        {
                            Period = responseArr[0],
                            Status = responseArr[1],
                            Counting = counter.ToString()
                        };
                        applications.Add(application);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("prequalificationapplications");
            }
            return View(applications);
        }

        public ActionResult PeriodCategories(string period)
        {
            var categories = new List<Registration>();
            try
            {
                string vendorVat = Session["VendorVat"].ToString();
                string periodCategories = webportals.GetPrequalificationApplicationCategories(vendorVat, period);
                if (!string.IsNullOrEmpty(periodCategories))
                {
                    string[] periodCategoriesArr = periodCategories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    int counter = 0;
                    foreach (var periodCategory in periodCategoriesArr)
                    {
                        counter++;
                        string[] responseArr = periodCategory.Split(strLimiters, StringSplitOptions.None);
                        Registration category = new Registration()
                        {
                            Counting = counter.ToString(),
                            Period = responseArr[0],
                            Status = responseArr[1],
                            CategoryName = responseArr[2]
                        };
                        categories.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("periodcategories", new { period = period });
            }
            return View(categories);
        }
    }
}