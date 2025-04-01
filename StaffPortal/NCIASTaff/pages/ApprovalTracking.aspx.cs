using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class ApprovalTracking : System.Web.UI.Page
    {
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
            }
        }
        protected string ApprovalTracks()
        {
            string htmlStr = string.Empty;

            try
            {

                string documentNo = Request.QueryString["DocNum"].ToString();

                string ApprovalList =Components.ObjNav.GetDocumentApprovalEntries(documentNo);

                if (!string.IsNullOrEmpty(ApprovalList))
                {

                    string[] ApprovalListArr = ApprovalList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < ApprovalListArr.Length; i++)
                    {

                        string[] Approvallist = ApprovalListArr[i].Split(new string[] { "::" }, StringSplitOptions.None);

                        if (Approvallist.Length >= 6)
                        {
                            string EntryNo = Approvallist[0];
                            string SeqNo = Approvallist[1];
                            string date = Approvallist[2];
                            string Sender = Approvallist[3];
                            string Approver = Approvallist[4];
                            string Status = Approvallist[5];





                            // Generate HTML table rows
                            htmlStr += "<tr class='text-primary small'>";
                            htmlStr += $"<td>{i + 1}</td>"; // Row number
                            htmlStr += $"<td>{EntryNo}</td>";
                            htmlStr += $"<td>{SeqNo}</td>";
                            htmlStr += $"<td>{date}</td>";
                            htmlStr += $"<td>{Sender}</td>";
                            htmlStr += $"<td>{Approver}</td>";
                            htmlStr += $"<td>{Status}</td>";

                            // htmlStr += $"<td><a href='TransportRequisition.aspx?requestNo={requestNumber}' ><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</a></td>";



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
        protected string ApprovalTracks1()
        {
            var htmlStr = string.Empty;
            try
            {
                using (var conn = Components.GetconnToNAV())
                {
                    var cmd = new SqlCommand();
                    cmd.CommandText = "spGetMyApprovalTracks";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                    cmd.Parameters.AddWithValue("@DocumentNo", "'" + Request.QueryString["DocNum"].ToString() + "'");

                    using (var reader = cmd.ExecuteReader())
                    {
                        int counter = 0;
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                counter++;
                                var statusCls = "default";
                                string status = reader["MyStatus"].ToString();
                                switch (status)
                                {
                                    case "Created":

                                    case "Open":
                                        statusCls = "warning"; break;
                                    case "Cancelled":
                                        statusCls = "primary"; break;
                                    case "Reject":
                                    case "Approved":
                                        statusCls = "success"; break;
                                    case "":
                                        statusCls = "info"; break;
                                }

                                htmlStr += string.Format(
                                    @"<tr class='text-info small'>
                            <td>{0}</td>
                            <td>{1}</td>
                            <td>{2}</td>
                            <td>{3}</td>
                            <td>{4}</td>
                            <td>{5}</td>
                            <td>{6}</td>
                            </tr>",
                                    counter,
                                    reader["Entry No_"],
                                    reader["Sequence No_"],
                                    Convert.ToDateTime(reader["Date-Time Sent for Approval"]),
                                    reader["Sender ID"],
                                    reader["Approver ID"],
                                   status,
                                   statusCls
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                htmlStr = $"Error: {ex.Message}";
            }

            return htmlStr;
        }

    
   
    }
}