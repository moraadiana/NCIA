using NCIASTaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class PettyCashListing_Reimbursement_ : System.Web.UI.Page
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
                    Components.ObjNav.OnCancelAppraisalRequest(appraisalNo);
                    Response.Redirect("AppraisalListing.aspx");
                }
            }
        }

        protected string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                
                string appraisalList = webportals.GetMyPettyCashReimbursement(username);
                if (!string.IsNullOrEmpty(appraisalList))
                {
                    int counter = 0;
                    string[] appraisalListArr = appraisalList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string appraisallist in appraisalListArr)
                    {
                        counter++;
                        string[] responseArr = appraisallist.Split(strLimiters, StringSplitOptions.None);
                        var statusCls = "default";
                        string status = responseArr[3];
                        switch (status)
                        {
                            case "Open":
                                statusCls = "warning";
                                break;
                            case "Released":
                                statusCls = "success";
                                break;
                            case "Posted":
                                statusCls = "primary";
                                break;
                            case "Pending Approval":
                                statusCls = "success";
                                break;
                            case "Cancelled":
                                statusCls = "danger";
                                break;
                            case "Approved":
                                statusCls = "success";
                                break;
                        }
                        htmlStr += String.Format(@"
                            <tr>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td>{3}</td>
                                <td><span class='label label-{5}'>{4}</span></td>
                                <td class='small'>
                                    <div class='options btn-group' >
					                    <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                    <ul class='dropdown-menu'>
                                            <li><a href='PettyCashLines(Reimbursement).aspx?pettyCashReNo={1}&query=old&status={4}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                            <li><a href=PettyCashListing(Reimbursement).aspx?pettyCashReNo={1}&status={4}'><i class='fa fa-trash text-danger'></i><span class='text-danger'>Cancel</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={1}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                            </tr>
                            "
                        ,
                          counter,
                          responseArr[0],
                          responseArr[1],
                          responseArr[2],
                          responseArr[3],


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
    }
}