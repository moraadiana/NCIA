using NCIASTaff.NAVWS;
using System;
using System.Data.SqlClient;

namespace NCIASTaff.pages
{
    public partial class TrainingListing : System.Web.UI.Page
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
                if (Request.QueryString["trainingNo"] != null)
                {
                    string trainingNo = Request.QueryString["trainingNo"].ToString();
                    // Components.ObjNav.OnCancelLeaveApplication(leaveNo);
                    //Response.Redirect("LeaveListing.aspx");
                }
            }
        }

        public string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                string trainingList = webportals.GetMyTrainingApplications(username);
                if (!string.IsNullOrEmpty(trainingList))
                {
                    int counter = 0;
                    string[] leaveListArr = trainingList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string leavelist in leaveListArr)
                    {
                        counter++;
                        string[] responseArr = leavelist.Split(strLimiters, StringSplitOptions.None);
                        var statusCls = "default";
                        string status = responseArr[7];
                        switch (status)
                        {
                            case "New":
                                statusCls = "warning";
                                break;
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
                                <td>{4}</td>
                                <td>{5}</td>
                                <td>{6}</td>
                                <td><span class='label label-{8}'>{7}</span></td>
                                <td class='small'>
                                    <div class='options btn-group' >
					                    <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                    <ul class='dropdown-menu'>
                                            <li><a href='TrainingApplication.aspx?trainingNo={1}&query=old&status={7}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                            <li><a href='TrainingListing.aspx?trainingNo={1}&status={7}'><i class='fa fa-trash text-danger'></i><span class='text-danger'>Cancel</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={0}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
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
                          responseArr[4],
                          responseArr[5],
                          responseArr[6],
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