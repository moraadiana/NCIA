using NCIASTaff.NAVWS;
using System;

namespace NCIASTaff.pages
{
    public partial class TransportRequisitionListing : System.Web.UI.Page
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

                string result = webportals.GetMyTransportRequisitions(empNo);

                if (!string.IsNullOrEmpty(result))
                {

                    string[] transportRequests = result.Split('|');

                    for (int i = 0; i < transportRequests.Length; i++)
                    {

                        string[] transportDetails = transportRequests[i].Split(new string[] { "::" }, StringSplitOptions.None);

                        if (transportDetails.Length == 8)
                        {
                            string requestNumber = transportDetails[0];
                            string requestDate = transportDetails[1];
                            string requestDescription = transportDetails[2];
                            string requestTravelDate = transportDetails[3];
                            string requestNoOfDays = transportDetails[4];
                            string requestDestination = transportDetails[5];
                            string requestReturnDate = transportDetails[6];
                            string requestStatus = transportDetails[7];



                            // Generate HTML table rows
                            htmlStr += "<tr class='text-primary small'>";
                            htmlStr += $"<td>{i + 1}</td>"; // Row number
                            htmlStr += $"<td>{requestNumber}</td>";
                            htmlStr += $"<td>{requestDate}</td>";
                            htmlStr += $"<td>{requestDescription}</td>";
                            htmlStr += $"<td>{requestTravelDate}</td>";
                            htmlStr += $"<td>{requestNoOfDays}</td>";
                            htmlStr += $"<td>{requestDestination}</td>";
                            htmlStr += $"<td>{requestReturnDate}</td>";
                            htmlStr += $"<td>{requestStatus}</td>";
                            
                            htmlStr += $"<td><a href='TransportRequisition.aspx?requestNo={requestNumber} & query= old&status={requestStatus}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></td>";


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