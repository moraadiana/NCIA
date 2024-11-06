
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCIASTaff.NAVWS;

namespace NCIASTaff.pages
{
    public partial class MemoImprestLines : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlDataReader reader;
        SqlCommand command;
        Staffportall webportals = Components.ObjNav;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                if (Request.QueryString["ImprestNo"] != null)
                {
                    string imprestNo = Request.QueryString["ImprestNo"].ToString();
                    string response = ""; webportals.OnCancelImprestRequisition(imprestNo);
                    if (response == "SUCCESS")
                    {
                        Message($"Imprest number {imprestNo} has been successfully cancelled!");
                        return;
                    }
                    else
                    {
                        Message("An error occured. Please try again later");
                        return;
                    }
                }
            }
        }


        public string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                //string userId = Components.UserID;
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetMyImprests",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@userID", "'" + username + "'");
                reader = command.ExecuteReader();
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
                            case "":
                                statusCls = "info"; break;
                        }

                        htmlStr += String.Format(@"
                            <tr  class='text-primary small'>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td>
                                    <a href='MemoReport.aspx?memoNo={3}'><i class='fa fa-download'></i>&nbsp;{3}</a>
                                </td>
                                <td><span class='label label-{5}'>{4}</span></td>
                                <td class='small'>
                                    <div class='options btn-group' >
					                    <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                    <ul class='dropdown-menu'>
                                            <li><a href='MemoImprestListing.aspx?ImprestNo={0}'><i class='fa fa-plus-circle text-danger'></i><span class='text-danger'>Cancell</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={0}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                            </tr>
                        ",
                        reader["No_"].ToString(),
                        reader["Payee"].ToString(),
                        reader["Purpose"].ToString(),
                        reader["Project Memo No"].ToString(),
                        status,
                        statusCls
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return htmlStr;
        }

        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
    }
}