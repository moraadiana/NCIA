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
    public class OpenTendersController : Controller
    {
        Supplier webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult Index()
        {
            if (Session["BidderNo"]==null)return RedirectToAction("index","default");
            var tenders = new List<Tenders>();
            try
            {
                string openTenders = webportals.GetOpenTenders2();
                if (!string.IsNullOrEmpty(openTenders))
                {
                    string[] openTendersArr = openTenders.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string openTender in openTendersArr)
                    {
                        string[] tenderArr = openTender.Split(strLimiters, StringSplitOptions.None);
                        Tenders tender = new Tenders()
                        {
                            No = tenderArr[0].Trim(),
                            ReqNo = tenderArr[1].Trim(),
                            Description = tenderArr[2].Trim(),
                            OpeningDate = tenderArr[3].Trim(),
                            ClosingDate = tenderArr[4].Trim()
                        };
                        tenders.Add(tender);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(tenders);
        }

        public ActionResult TenderLines(string tenderNo)
        {
            var tenderLines = new List<Tenders>();
            try
            {
                string openTenderLines = webportals.GetOpenTenderLines(tenderNo);
                if (!string.IsNullOrEmpty(openTenderLines))
                {
                    string[] openTenderLinesArr = openTenderLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var openTenderLine in openTenderLinesArr)
                    {
                        string[] tenderLineArr = openTenderLine.Split(strLimiters, StringSplitOptions.None);
                        Tenders tenderLine = new Tenders()
                        {
                            No = tenderLineArr[0].Trim(),
                            Description = tenderLineArr[1].Trim(),
                            UnitofMeasure = tenderLineArr[2].Trim(),
                            Quantity = tenderLineArr[3].Trim()
                        };
                        tenderLines.Add(tenderLine);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(tenderLines);
        }

        public ActionResult ApplyTender(string tenderNo)
        {
            if (Session["BidderNo"] == null) return RedirectToAction("index", "login");
            Session["TenderNo"] = tenderNo;
            var tenderLines = new List<Tenders>();
            try
            {
                string bidderNo = Session["BidderNo"].ToString();
                if (webportals.TenderApplied(bidderNo, tenderNo))
                {
                    TempData["Error"] = $"You have already submitted your bid for tender number {tenderNo}!";
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
                return View("applytender", new { tenderNo = tenderNo });
            }
            return View(tenderLines);
        }

    /*   public ActionResult SubmitTenderApplication()
        {
            return View();
        }*/

        [HttpPost]
        public ActionResult SubmitTenderApplication(Tenders tender)
        {
            if (Session["TenderNo"] == null) return RedirectToAction("index", "opentenders");
            string tenderNo = Session["TenderNo"].ToString();
            try
            {
                string bidderNo = Session["BidderNo"].ToString();                
                string categories = tender.SelectedCategories;

                string path = Server.MapPath("~/Attachments");                

                string bidNo = webportals.CreateTenderSubmissionHeader(bidderNo, tenderNo);

                if(!string.IsNullOrEmpty(bidNo))
                {
                    if (tender.RFQForm != null && tender.RFQForm.ContentLength > 0)
                    {
                        string filename = tender.RFQForm + " RFQ Form.pdf";
                       
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string pathToUpload = Path.Combine(path, filename);

                        if (System.IO.File.Exists(pathToUpload))
                        {
                            System.IO.File.Delete(pathToUpload);
                        }

                        tender.RFQForm.SaveAs(pathToUpload);
                        Stream fs = tender.RFQForm.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        string name = "RFQ Form.pdf";
                        webportals.DocumentAttachment(bidNo, name, base64String, 52178793);
                    }

                    string[] categoriesArr = categories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string category in categoriesArr)
                    {
                        string[] productArr = category.Split(strLimiters, StringSplitOptions.None);
                        string itemNo = productArr[0].Trim();
                        decimal quotedAmnt = Convert.ToDecimal(productArr[1].Trim());
                        
                        //update line no here this is just constant
                        webportals.InsertTenderSubmissionLine(bidderNo, tenderNo, bidNo,itemNo,quotedAmnt);
                    }
                    if (webportals.SubmitBid(bidderNo, bidNo))
                    {
                        TempData["Success"] = "Bid has been submitted successfuly";
                        return RedirectToAction("appliedtenders", "opentenders");
                    }
                }
               
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("applytender", "opentenders", new { tenderNo = tenderNo });
            }
            // return View(tender);
            return RedirectToAction("appliedtenders", "opentenders");
        }

        public ActionResult AppliedTenders()
        {
            if (Session["BidderNo"] == null) return RedirectToAction("index", "login");
            var bids = new List<Tenders>();
            try
            {
                string bidderNo = Session["BidderNo"].ToString();
                string myBids = webportals.GetMyBids(bidderNo);
                if(!string.IsNullOrEmpty(myBids))
                {
                    string[] myBidsArr = myBids.Split(strLimiters2, StringSplitOptions.None);
                    foreach(var myBid in myBidsArr)
                    {
                        string[] products = myBid.Split(strLimiters, StringSplitOptions.None);
                        Tenders bid = new Tenders()
                        {
                            No = products[0].Trim(),
                            TenderNo = products[1].Trim(),
                            Description = products[2].Trim(),
                            ApplicationDate = products[3].Trim(),
                            OpeningDate = products[4].Trim(),
                            ClosingDate = products[5].Trim(),
                            Status = products[6].Trim()
                        };
                        bids.Add(bid);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return View(bids);
        }

        public ActionResult BidLines(string bidNo)
        {
            var bidLines = new List<Tenders>();
            try
            {
                string bidderNo = Session["BidderNo"].ToString();
                string myBidLines = webportals.GetMyBidLines(bidderNo,bidNo);
                if (!string.IsNullOrEmpty(myBidLines))
                {
                    string[] myBidLinesArr = myBidLines.Split(strLimiters2, StringSplitOptions.None);
                    foreach (var myBid in myBidLinesArr)
                    {
                        string[] products = myBid.Split(strLimiters, StringSplitOptions.None);
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
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return View(bidLines);
        }
    }
}