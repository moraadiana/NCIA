using NCIASTaff.NAVWS;
using System;
using System.Data.SqlClient;

namespace NCIASTaff.pages
{
    public partial class PettyCashListing : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlDataReader reader;
        SqlCommand command;
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
                string pettycashlist = webportals.GetMyPettyCashApplications(username);
                if (!string.IsNullOrEmpty(pettycashlist))
                {
                    // int counter = 0;
                    string[] pettyCashListArr = pettycashlist.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string pettyCashlist in pettyCashListArr)
                    {
                        //counter++;
                        string[] responseArr = pettyCashlist.Split(strLimiters, StringSplitOptions.None);
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
                            <tr  class='text-primary small'>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td><span class='label label-{4}'>{3}</span></td>
                                <td class='small'>
                                    <div class='options btn-group' >
                         <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
                         <ul class='dropdown-menu'>
                                            <li><a href='PettyCashLines.aspx?PettyCashNo={0}&query=old&status={3}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={0}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                            </tr>"
                        ,
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
        private void Message(string message)
        {
            string strScript = " < script>alert('" + message + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
    }
}