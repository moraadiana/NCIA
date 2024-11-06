using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class BackToOfficeListsing : System.Web.UI.Page
    {
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
        protected string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                using (var conn = Components.GetconnToNAV())
                {
                    //,,,,,Posted
                    string L_ = null;
                    var cmd = new SqlCommand();
                    L_ = "spGetMyBackOfficeApps";
                    cmd.CommandText = L_;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                    cmd.Parameters.AddWithValue("@Employee_No", "'"+Session["username"].ToString()+"'");
                    int counter = 0;
                    using (SqlDataReader drL = cmd.ExecuteReader())
                    {
                        if (drL.HasRows)
                        {                            
                            while (drL.Read())
                            {
                                //Open,Pending Approval,Released,Pending Prepayment,Cancelled,Posted
                                counter++;
                                var statusCls = "default";
                                string status = drL["Status Description"].ToString();
                                Session["ReqStatus"] = status;
                                switch (status)
                                {
                                    case "Open":
                                        statusCls = "warning";
                                        break;
                                    case "Released":
                                        statusCls = "success";
                                        break;
                                    case "Pending Approval":
                                        statusCls = "primary";
                                        break;
                                    case "Pending Prepayment":
                                        statusCls = "success";
                                        break;
                                    case "Cancelled":
                                        statusCls = "danger";
                                        break;
                                    case "Approved":
                                        statusCls = "success";
                                        break;
                                }
                                htmlStr += string.Format(@"<tr  class='text-info small'>
                                                            <td>{0}</td>
                                                            <td><a href='#'>{1}</a></td>
                                                            <td>{2}</td>
                                                            <td>{3}</td>
                                                            <td>{4}</td>
                                                            <td>{5}</td>
                                                            <td>{6}</td>
                                                            <td>{7}</td>
                                                            <td><span class='label label-{9}'>{8}</span></td>
                                                             <td class='small'>
                                                                    <a href='#?appNo={1}&status={1}'><i class='fa fa-times  text-danger'></i><span class='text-danger'>  Cancel</span></a>
                                                            </td>
                                                     </tr>",
                                    counter,
                                    drL["No_"],
                                    drL["Leave Type"],
                                    Convert.ToInt32(Convert.ToDouble(drL["Applied Days"]).ToString(CultureInfo.InvariantCulture)),
                                    Convert.ToDateTime(drL["Date"]).ToShortDateString(),
                                    Convert.ToDateTime(drL["Starting Date"]).ToShortDateString(),
                                    Convert.ToDateTime(drL["End Date"]).ToShortDateString(),
                                    Convert.ToDateTime(drL["Return Date"]).ToShortDateString(),
                                    drL["Status Description"],
                                    statusCls,
                                    drL["Status"]
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                exception.Data.Clear();
            }
            return htmlStr;
        }
    }
}