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
    public class DefaultController : Controller
    {
        Supplier webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult Index()
        {
            var tenders = new List<Tenders>();
            try
            {
                string openTenders = webportals.GetOpenTenders();
                if(!string.IsNullOrEmpty(openTenders) )
                {
                    string[] openTendersArr = openTenders.Split(strLimiters2,StringSplitOptions.RemoveEmptyEntries);
                    foreach(string openTender in  openTendersArr)
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
            catch(Exception ex)
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
                    foreach(var openTenderLine in openTenderLinesArr)
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
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return View(tenderLines);
        }
    }
}