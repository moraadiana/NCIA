
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
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
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


        protected string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
               
                string imprestList = webportals.GetMyImprests(username);
                if (!string.IsNullOrEmpty(imprestList))
                {
                    int counter = 0;
                    string[] ImprestListArr = imprestList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string ImprestList in ImprestListArr)
                    {
                        counter++;
                        string[] responseArr = ImprestList.Split(strLimiters, StringSplitOptions.None);
                        var statusCls = "default";
                        string status = responseArr[4];
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
                                            <li><a href='MemoImprestLines.aspx?ImprestNo={1}'><i class='fa fa-plus-circle text-success'></i><span class='text-danger'>Details</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={1}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                            </tr>",
                          counter,
                          responseArr[0],
                          responseArr[1],
                          responseArr[2],
                          responseArr[3],
                          responseArr[4],    
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