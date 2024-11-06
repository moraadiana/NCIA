using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class PurchaseListing : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
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

        protected string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetMyPurchaseReq",
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@UserId", "'" + username + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var statusCls = "default";
                        string status = reader["MyStatus"].ToString();
                        switch (status)
                        {
                            case "Pending":
                                statusCls = "warning"; break;
                            case "Pending Approval":
                                statusCls = "primary"; break;
                            case "Approved":
                                statusCls = "success"; break;
                            case "Posted":
                                statusCls = "success"; break;
                            case "Cancelled":
                                statusCls = "danger"; break;
                            case "Open":
                                statusCls = "warning"; break;
                        }

                        htmlStr += String.Format(@"
                        <tr>
                            <td>{0}</td>
                            <td>{1}</td>
                            <td><span class='label label-{3}'>{2}</span></td>
                                <td class='small'>
                                    <div class='options btn-group' >
					                    <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                    <ul class='dropdown-menu'>
                                            <li><a href='PurchaseLines.aspx?PRNNo={0}&query=old&status={2}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={0}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                        </tr>",
                        reader["No_"].ToString(),
                        reader["Request Narration"].ToString(),
                        status,
                        statusCls
                        );
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return htmlStr;
        }
    }
}