using NCIASTaff.NAVWS;
using System;
using System.Data.SqlClient;

namespace NCIASTaff.pages
{
    public partial class MidYearReviewListing : System.Web.UI.Page
    {
        Staffportall webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };

        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                if (Request.QueryString["appraisalNo"] != null)
                {
                    string appraisalNo = Request.QueryString["appraisalNo"].ToString();
                    string approvalStatus = Request.QueryString["status"].Replace("%", " ");

                    Components.ObjNav.OnCancelAppraisalRequest(appraisalNo);

                }
            }
        }


        protected string Jobs()
        {
            string htmlStr = string.Empty;

            try
            {

                string empNo = Session["username"].ToString();

                string username = Session["StaffName"].ToString().ToUpper().Replace(" ", ".");
                //type mid year
                int type = 2;

                string appraisalList = Components.ObjNav.GetMyAppraisals(empNo, type);

                if (!string.IsNullOrEmpty(appraisalList))
                {

                    string[] appraisalListArr = appraisalList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < appraisalListArr.Length; i++)
                    {

                        string[] appraisalDetails = appraisalListArr[i].Split(new string[] { "::" }, StringSplitOptions.None);

                        if (appraisalDetails.Length == 4)
                        {
                            string appraisalNo = appraisalDetails[0];
                            string Date = appraisalDetails[1];
                            string period = appraisalDetails[2];
                            string status = appraisalDetails[3];




                            // Generate HTML table rows
                            htmlStr += "<tr class='text-primary small'>";
                            htmlStr += $"<td>{i + 1}</td>"; // Row number
                            htmlStr += $"<td>{appraisalNo}</td>";
                            //htmlStr += $"<td>{Date}</td>";
                            //htmlStr += $"<td>{period}</td>";
                            htmlStr += $"<td>{status}</td>";

                            htmlStr += $"<td><a href='MidYearReview.aspx?appraisalNo={appraisalNo}&status={status}&query=old'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></td>";


                            htmlStr += "</tr>";
                        }
                    }
                }
                else
                {
                    htmlStr = "<tr><td colspan='6'>No records found.</td></tr>";
                }
            }
            catch (Exception exception)
            {
                exception.Data.Clear();
                htmlStr = "<tr><td colspan='6'>Error fetching list.</td></tr>";
            }

            return htmlStr;
        }
        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
    }
}