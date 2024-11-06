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
    public partial class RelieverListsing : System.Web.UI.Page
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
                    string n = null;
                    var cmd = new SqlCommand();
                    L_ = "spGetMyRelieverLeaveApps";
                    cmd.CommandText = L_;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                    cmd.Parameters.AddWithValue("@Employee_No", "'" + Session["username"].ToString() + "'");
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
                                /*
                                n = "spGetMyRelievers";
                                var RelieverAcc = "default";
                                string accepted = drL["Reliever Accepted"].ToString();
                                Session["RelAccepted"] = accepted;*/
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
                                var RelieverAcc = "default";
                                string accepted = drL["Reliever Accepted"].ToString();
                                Session["RelAccepted"] = accepted;
                                if (accepted == "1")
                                {
                                    RelieverAcc = "No action";
                                }
                                if (accepted == "2")
                                {
                                    RelieverAcc = "Accepted";
                                }
                                if (accepted == "3")
                                {
                                    RelieverAcc = "Declined";
                                }/*
                                switch(accepted)
                                { 
                                    case "No action":
                                        RelieverAcc = "warning";
                                        break;
                                    case "Accepted":
                                        RelieverAcc = "success";
                                        break;
                                    case "Declined":
                                        RelieverAcc = "danger";
                                        break;
                                }*/
                                htmlStr += string.Format(@"<tr  class='text-info small'>
                                                            <td>{0}</td>
                                                            <td><a href='#'>{1}</a></td>
                                                            <td>{2}</td>
                                                            <td>{3}</td>
                                                            <td>{4}</td>
                                                            <td>{5}</td>
                                                            <td>{6}</td>
                                                            <td>{7}</td>
                                                            <td>{8}</td>
                                                            <td>{9}</td>
                                                            <td><span class='label label-{11}'>{10}</span></td>
                                                             <td class='small'>
                                                               <div class='options btn-group' >
                                                                   <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'>Options</a>
					                                                 <ul class='dropdown-menu'>
                                                                   <li><a href='Revieweraccept.aspx?appNo={1}'><i class='fa fa-send-o'>Accept</i></a></li>
                                                                   <li><a href='Reviewerdecline.aspx?appNo={1}'><i class='fa fa-send-o'>Decline</i></a></li>
                                                                  </ul>
                                                               </div>
                                                            </td>
                                                     </tr>",
                                    counter,
                                    drL["No_"],
                                    drL["Leave Type"],
                                    drL["Employee Name"],
                                    Convert.ToInt32(Convert.ToDouble(drL["Applied Days"]).ToString(CultureInfo.InvariantCulture)),
                                    Convert.ToDateTime(drL["Date"]).ToShortDateString(),
                                    Convert.ToDateTime(drL["Starting Date"]).ToShortDateString(),
                                    Convert.ToDateTime(drL["End Date"]).ToShortDateString(),
                                    Convert.ToDateTime(drL["Return Date"]).ToShortDateString(),
                                    RelieverAcc,//drL["Reliever Accepted"],
                                    drL["Status Description"],
                                    statusCls,
                                    drL["Status"]
                                    //RelieverAcc
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
        protected void lbtnAccept_Click(object sender, EventArgs e)
        {
            
            try
            {
                string RequiNo = Request.QueryString["An"].ToString();
                Session["ReqNo"] = RequiNo;
                Components.ObjNav.updateleave(RequiNo, 1);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

        }

        protected void lbtnDecline_Click(object sender, EventArgs e)
        {
            
            try
            {
                string RequiNo = Request.QueryString["An"].ToString();
                Session["ReqNo"] = RequiNo;
                Components.ObjNav.updateleave(RequiNo,2);
            }
            catch (Exception ex)
            {
                ex.Data.Clear()
;            }

        }
    }
}