using NCIASTaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class Payslip : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter adapter;
        string[] strLimiters2 = new string[] { "[]" };
        Staffportall webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
                LoadYears();
                LoadMonths();
                ViewPayslip();
            }
        }

        private void LoadYears()
        {
            try
            {
                
                ddlYear.Items.Clear();
                string payslipYears = webportals.GetPayslipYears();
                if (!string.IsNullOrEmpty(payslipYears))
                {
                    string[] yearsArr = payslipYears.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string years in yearsArr)
                    {
                        string[] responseArr = years.Split(strLimiters, StringSplitOptions.None);
                        ListItem li = new ListItem(responseArr[0]);
                        ddlYear.Items.Add(li);
                    }
                }
               
                
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadMonths1()
        {
            try
            {
                ddlMonth.Items.Clear();
                string payslipMonths = webportals.GetPayslipMonths();
                if (!string.IsNullOrEmpty(payslipMonths))
                {
                    string[] monthsArr = payslipMonths.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string months in monthsArr)
                    {
                        string[] responseArr = months.Split(strLimiters, StringSplitOptions.None);
                        ListItem li = new ListItem(responseArr[0]);
                        ddlMonth.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        private void LoadMonths()
        {
            try
            {
                ddlMonth.Items.Clear(); // Clear the dropdown list

                string payslipMonths = webportals.GetPayslipMonths(); // Get the payslip months
                if (!string.IsNullOrEmpty(payslipMonths))
                {
                    string[] monthsArr = payslipMonths.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string months in monthsArr)
                    {
                        // Split the response into month number and month name
                        string[] responseArr = months.Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length == 2)
                        {
                            string monthNumber = responseArr[0]; // Month number
                            string monthName = responseArr[1];   // Month name

                            // Create a ListItem with the month name as Text and the month number as Value
                            ListItem li = new ListItem(monthName, monthNumber);
                            ddlMonth.Items.Add(li); // Add the item to the dropdown
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear(); // Clear exception data for error handling
            }
        }

        private void ViewPayslip()
        {
            try
            {
                string username = Session["username"].ToString();
                var filename = Session["username"].ToString().Replace(@"/", @"");
                string pdfFileName = String.Format(@"PAYSLIP-{0}.pdf", filename);
                var month = ddlMonth.SelectedValue;

                if (month == "12")
                {
                    month = "12";

                }
                else if (month == "11")
                {
                    month = "11";
                }
                else if (month == "10")
                {
                    month = "10";
                }
                else if (month == "")
                {
                    month = "01";
                }
                else
                {
                    month = "0" + month;
                }
                var myDate = month + "/01/" + ddlYear.SelectedValue;
                var period = DateTime.ParseExact(myDate, "M/dd/yyyy", CultureInfo.InvariantCulture);



                var filePath = Server.MapPath("~/Downloads/") + String.Format("PAYSLIP-{0}.pdf", filename);

                // Check if directory exists, if not create it
                if (!Directory.Exists(Server.MapPath("~/Downloads/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Downloads/"));
                }
                webportals.GeneratePaySlipReport3(username, period, String.Format(@"PAYSLIP-{0}.pdf", filename));

                // myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format(@"PAYSLIP-{0}.pdf", filename)));
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine("Payslip generated successfully.");
                    myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format("PAYSLIP-{0}.pdf", filename)));
                }
                else
                {
                    throw new FileNotFoundException("Payslip PDF was not found after generation.");
                }

                /*if (webportals.CanViewPaySlip(username))
                {*/
                /*    webportals.GeneratePaySlipReport3(username, period, String.Format(@"PAYSLIP-{0}.pdf", filename));

                    string networkPath = @"\\10.107.8.40\Downloads\" + pdfFileName;


                    string localFolderPath = Server.MapPath("~/Downloads/");
                    string localFilePath = Path.Combine(localFolderPath, pdfFileName);

                    if (!Directory.Exists(localFolderPath))
                    {
                        Directory.CreateDirectory(localFolderPath);
                    }


                    System.IO.File.Copy(networkPath, localFilePath, true);


                    string fileUrl = ResolveUrl("~/Downloads/" + pdfFileName);


                    myPDF.Attributes.Add("src", fileUrl);
                /*}
                else
                {
                    Message("This period has not been enabled for viewing");
                }
               */

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadMonths();
                ViewPayslip();
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewPayslip();
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
    }
}