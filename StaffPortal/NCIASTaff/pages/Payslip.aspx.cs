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
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetPayslipYears",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    ddlYear.DataSource = reader;
                    ddlYear.DataTextField = "Period Year";
                    ddlYear.DataValueField = "Period Year";
                    ddlYear.DataBind();
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
                ddlMonth.Items.Clear(); 

                string payslipMonths = webportals.GetPayslipMonths(); 
                if (!string.IsNullOrEmpty(payslipMonths))
                {
                    string[] monthsArr = payslipMonths.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string months in monthsArr)
                    {
                        
                        string[] responseArr = months.Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length == 2)
                        {
                            string monthNumber = responseArr[0]; 
                            string monthName = responseArr[1];  

                            
                            ListItem li = new ListItem(monthName, monthNumber);
                            ddlMonth.Items.Add(li); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear(); 
            }
        }
        private void ViewPayslip()
        {
            try
            {
                string username = Session["username"].ToString();
                //string sanitizedUsername = username.Replace("/", "");
              //  string filename = $"PAYSLIP-{sanitizedUsername}.pdf";
                var filename = Session["username"].ToString().Replace(@"/", @"");
                string month = ddlMonth.SelectedValue.PadLeft(2, '0');
                string periodString = $"{month}/01/{ddlYear.SelectedValue}";
                DateTime period = DateTime.ParseExact(periodString, "M/d/yyyy", CultureInfo.InvariantCulture);
           //     string downloadsPath = Server.MapPath("~/Downloads/");
         //       string filePath = Path.Combine(downloadsPath, filename);

                try
                {
                    string returnstring = "";
                   // Components.ObjNav.Generatep9Report(period, employee, String.Format("p9Form{0}.pdf", filename), ref returnstring);
                    webportals.GeneratePayslipReport2(username, period, String.Format(@"PAYSLIP-{0}.pdf", filename), ref returnstring);
                    myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format("PAYSLIP-{0}.pdf", filename)));
                    //WSConfig.ObjNavWS.FnFosaStatement(accno, ref returnstring, filter);
                    byte[] bytes = Convert.FromBase64String(returnstring);
                    string path = HostingEnvironment.MapPath("~/Downloads/" + $"PAYSLIP-{filename}.pdf");
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    FileStream stream = new FileStream(path, FileMode.CreateNew);
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(bytes, 0, bytes.Length);
                    writer.Close();
                    myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format("PAYSLIP-{0}.pdf", filename)));
                }
                catch (Exception exception)
                {
                    exception.Data.Clear();
                    //     HttpContext.Current.Response.Write(exception);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
            private void ViewPayslip1()
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
                //var filePath = Server.MapPath("~/Downloads/") + pdfFileName;
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

                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error generating payslip: " + ex.ToString());
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