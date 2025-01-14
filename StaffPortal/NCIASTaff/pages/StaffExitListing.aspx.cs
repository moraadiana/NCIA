using NCIASTaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class StaffExitListing : System.Web.UI.Page
    {
        Staffportall webportals = Components.ObjNav;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }
        public string Jobs()
        {
            string htmlStr = string.Empty;

            try
            {

                string empNo = Session["username"].ToString();

                string username = Session["StaffName"].ToString().ToUpper().Replace(" ", ".");

                string result = webportals.GetMyExitRequisitions(empNo);

                if (!string.IsNullOrEmpty(result))
                {

                    string[] exitRequests = result.Split('|');

                    for (int i = 0; i < exitRequests.Length; i++)
                    {

                        string[] exitDetails = exitRequests[i].Split(new string[] { "::" }, StringSplitOptions.None);

                        if (exitDetails.Length == 7)
                        {
                            string EmpNo = exitDetails[0];
                            string EmpName = exitDetails[1];
                            string Date = exitDetails[2];
                            string Designation = exitDetails[3];
                            string Reason = exitDetails[4];
                            string leavingDate = exitDetails[5];
                            string status = exitDetails[6];
                           



                            // Generate HTML table rows
                            htmlStr += "<tr class='text-primary small'>";
                            htmlStr += $"<td>{i + 1}</td>"; // Row number
                            htmlStr += $"<td>{EmpNo}</td>";
                            htmlStr += $"<td>{EmpName}</td>";
                            htmlStr += $"<td>{Date}</td>";
                            htmlStr += $"<td>{Designation}</td>";
                            htmlStr += $"<td>{Reason}</td>";
                            htmlStr += $"<td>{leavingDate}</td>";
                            htmlStr += $"<td>{status}</td>";
                           
                            // htmlStr += $"<td><a href='TransportRequisition.aspx?requestNo={requestNumber}' ><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</a></td>";
                           //htmlStr += $"<td><a href='TransportRequisition.aspx?requestNo={EmpNo}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></td>";


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
    }
}