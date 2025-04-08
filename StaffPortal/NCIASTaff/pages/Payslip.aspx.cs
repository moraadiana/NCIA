using NCIASTaff.NAVWS;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.Hosting;
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
                string year = ddlYear.SelectedValue;
                Console.WriteLine($"Selected Year: {year}");
                int CurrentYear = Convert.ToInt32(year);

                string payslipMonths = webportals.GetPayslipMonths(CurrentYear);
                System.Diagnostics.Debug.WriteLine($"Payslip Months Response: {payslipMonths}");
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

                var filename = Session["username"].ToString().Replace(@"/", @"");
                // DateTime period = DateTime.ParseExact($"{ddlMonth.SelectedValue.PadLeft(2, '0')}/01/{ddlYear.SelectedValue}", "MM/dd/yyyy", CultureInfo.InvariantCulture);
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
                //string s = "07/01/2024";
                //DateTime period2 = Convert.ToDateTime(s);
                try
                {
                    string returnstring = "";
                    Components.ObjNav.GeneratePaySlipReport(username, period, String.Format("PAYSLIP{0}.pdf", filename), ref returnstring);
                    myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("PAYSLIP{0}.pdf", filename)));

                    byte[] bytes = Convert.FromBase64String(returnstring);
                    string path = HostingEnvironment.MapPath("~/Download/" + $"PAYSLIP{filename}.pdf");
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    FileStream stream = new FileStream(path, FileMode.CreateNew);
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(bytes, 0, bytes.Length);
                    writer.Close();
                    myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("PAYSLIP{0}.pdf", filename)));
                }
                catch (Exception exception)
                {
                    exception.Data.Clear();

                }

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
            catch (Exception ex)
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