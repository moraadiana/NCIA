using System;
using NCIASTaff.NAVWS;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCIASTaff;
namespace NCIASTaff.pages
{
    public partial class PettyCashAccounting : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        Staffportall webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                string pCashList = webportals.GetMyPettyCashSurrender(username);
                if (!string.IsNullOrEmpty(pCashList))
                {
                    //int counter = 0;
                    string[] pCashListArr = pCashList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string petyCashlist in pCashListArr)
                    {
                       // counter++;
                        string[] responseArr = petyCashlist.Split(strLimiters, StringSplitOptions.None);
                        var statusCls = "default";
                        string status = responseArr[3];
                        switch (status)
                        {
                            case "Pending":
                                statusCls = "warning"; break;
                            case "Pending Approval":
                                statusCls = "primary"; break;                   
                            case "Approved":
                                statusCls = "success"; break;
                            case "Posted":
                                statusCls = "info"; break;
                        }
                        htmlStr += string.Format(@"
                            <tr  class='text-primary small'>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td><span class='label label-{4}'>{3}</span></td>
                                <td>
                                    <div class='options btn-group' >
			                            <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
			                            <ul class='dropdown-menu'>
                                            <li><a href='PettyCashAccountingLines.aspx?SurrenderNo={0}&PettyCashNo={1}&query=old&status={3}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={0}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                            </tr>",
                          // counter,
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