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
               reader=command.ExecuteReader();
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
                string year = ddlYear.SelectedValue;
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetPayslipMonths",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@current", "'" + year + "'");
                reader=command.ExecuteReader();
                if(reader.HasRows)
                {
                    ddlMonth.DataSource = reader;
                    ddlMonth.DataTextField = "Period Name";
                    ddlMonth.DataValueField = "Period Month";
                    ddlMonth.DataBind();
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

                /*if (webportals.CanViewPaySlip(username))
                {*/
                    webportals.GeneratePaySlipReport3(username, period, String.Format(@"PAYSLIP-{0}.pdf", filename));

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