using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NCIAVendor.NAVWS;
using NCIAVendor.Models;

namespace NCIAVendor.Controllers
{
    public class OpenTendersController : Controller
    {
        VendorsPortal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult Index()
        {
            if (Session["VendorNo"] == null) return RedirectToAction("index", "login");
            var tenders = new List<Tenders>();
            try
            {
                string username = Session["VendorNo"].ToString();
                string openVendorTenders = webportals.GetOpenVendorTenders(username);
                if (!string.IsNullOrEmpty(openVendorTenders))
                {
                    string[] openVendorTendersArr = openVendorTenders.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var openTender in openVendorTendersArr)
                    {
                        string[] responseArr = openTender.Split(strLimiters, StringSplitOptions.None);
                        Tenders tender = new Tenders()
                        {
                            No = responseArr[0],
                            ReqNo = responseArr[1],
                            Description = responseArr[2],
                            OpeningDate = responseArr[3],
                            ClosingDate = responseArr[4]
                        };
                        tenders.Add(tender);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
            return View(tenders);
        }

        public ActionResult TenderLines(string tenderNo)
        {
            if (Session["VendorNo"] == null) return RedirectToAction("index", "login");
            var tenderLines = new List<Tenders>();
            try
            {
                string openTenderLines = webportals.GetOpenTenderLines(tenderNo);
                if (!string.IsNullOrEmpty(openTenderLines))
                {
                    string[] openTenderLinesArr = openTenderLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string openTenderLine in openTenderLinesArr)
                    {
                        string[] responseArr = openTenderLine.Split(strLimiters, StringSplitOptions.None);
                        Tenders tender = new Tenders()
                        {
                            No = responseArr[0],
                            Description = responseArr[1],
                            UnitofMeasure = responseArr[2],
                            Quantity = responseArr[3]
                        };
                        tenderLines.Add(tender);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index");
            }
            return View(tenderLines);
        }

        public ActionResult ApplyTender(string tenderNo)
        {
            if (Session["VendorNo"] == null) return RedirectToAction("index", "login");
            Session["TenderNo"] = tenderNo;
            var tenderLines = new List<Tenders>();
            try
            {
                string vendorNo = Session["VendorNo"].ToString();
                if (webportals.RFQApplied(vendorNo, tenderNo))
                {
                    TempData["Error"] = "You have already submitted your quote for this RFQ!";
                    return RedirectToAction("appliedtenders", "opentenders");
                }

                string openTenderLines = webportals.GetOpenTenderLines(tenderNo);
                if (!string.IsNullOrEmpty(openTenderLines))
                {
                    string[] openTenderLinesArr = openTenderLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string openTenderLine in openTenderLinesArr)
                    {
                        string[] responseArr = openTenderLine.Split(strLimiters, StringSplitOptions.None);
                        Tenders tender = new Tenders()
                        {
                            No = responseArr[0],
                            Description = responseArr[1],
                            UnitofMeasure = responseArr[2],
                            Quantity = responseArr[3]
                        };
                        tenderLines.Add(tender);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("applytender");
            }
            return View(tenderLines);
        }

        public ActionResult SubmitTenderApplication(Tenders tenders)
        {
            if (Session["TenderNo"] == null) return RedirectToAction("index", "opentenders");
            string username = Session["VendorNo"].ToString();
            string tenderNo = Session["TenderNo"].ToString();
            try
            {
                string categories = tenders.SelectedCategories;
                // Create RFQ header
                string bidNo = webportals.CreateRFQHeader(username, tenderNo);
                if (!string.IsNullOrEmpty(bidNo))
                {
                    string[] categoriesArr = categories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string category in categoriesArr)
                    {
                        string[] productsArr = category.Split(strLimiters, StringSplitOptions.None);
                        // Create RFQ line
                        
                       int lineNo = 0;
                        
                        webportals.InsertRFQLines(username, tenderNo, bidNo, productsArr[0].Trim(), Convert.ToDecimal(productsArr[1].Trim()), lineNo);
                    }
                    if (webportals.SubmitRFQ(username, bidNo))
                    {
                        TempData["Success"] = "Quotation submitted successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("applytender", "opentenders", new { tenderNo = tenderNo });
            }
            return RedirectToAction("appliedtenders", "opentenders");
        }

        public ActionResult AppliedTenders()
        {
            if (Session["VendorNo"] == null) return RedirectToAction("index", "login");
            var quotes = new List<Tenders>();
            try
            {
                string username = Session["VendorNo"].ToString();
                string myQuotes = webportals.GetMyQuotes(username);
                if (!string.IsNullOrEmpty(myQuotes))
                {
                    string[] myQuotesArr = myQuotes.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string myQuote in myQuotesArr)
                    {
                        string[] products = myQuote.Split(strLimiters, StringSplitOptions.None);
                        Tenders quote = new Tenders()
                        {
                            No = products[0].Trim(),
                            TenderNo = products[1].Trim(),
                            Description = products[2].Trim(),
                            ApplicationDate = products[3].Trim(),
                            OpeningDate = products[4].Trim(),
                            ClosingDate = products[5].Trim(),
                            Status = products[6].Trim()
                        };
                        quotes.Add(quote);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("appliedtenders", "opentenders");
            }
            return View(quotes);
        }

        public ActionResult BidLines(string bidNo)
        {
            if (Session["VendorNo"] == null) return RedirectToAction("index", "login");
            var bidLines = new List<Tenders>();
            try
            {
                string username = Session["VendorNo"].ToString();
                string myQuoteLines = webportals.GetMyQuoteLines(username, bidNo);
                if (!string.IsNullOrEmpty(myQuoteLines))
                {
                    string[] myQuoteLinesArr = myQuoteLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string myQuoteLine in myQuoteLinesArr)
                    {
                        string[] products = myQuoteLine.Split(strLimiters, StringSplitOptions.None);
                        Tenders bidLine = new Tenders()
                        {
                            No = products[0].Trim(),
                            Description = products[1].Trim(),
                            UnitofMeasure = products[2].Trim(),
                            UnitCost = products[3].Trim(),
                            Quantity = products[4].Trim(),
                            Amount = products[5].Trim()
                        };
                        bidLines.Add(bidLine);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("appliedtenders", "opentenders");
            }
            return View(bidLines);
        }

        public FileResult Download(string tenderNo)
        {
            string filename = tenderNo + ".pdf";
            string filepath = Server.MapPath("~/Downloads/") + filename;
            string bigText = "";
            webportals.GenerateRFQReport(tenderNo, filename, ref bigText);
            byte[] array = Convert.FromBase64String(bigText);
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            FileStream output = new FileStream(filepath, FileMode.CreateNew);
            BinaryWriter binaryWriter = new BinaryWriter(output);
            binaryWriter.Write(array, 0, array.Length);
            binaryWriter.Close();
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        [ChildActionOnly]
        public PartialViewResult Notification()
        {
            return PartialView("_Notification");
        }
    }
}