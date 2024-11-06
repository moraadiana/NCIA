using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace NCIASTaff.pages
{
    public partial class Reviewerdecline : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["status"] == "5")
            {
                Message("This Leave has already been approved and therefore cannot be cancelled.");
                return;
            }
            decline(Request.QueryString["appNo"], 1);
        }
        protected void decline(string LeaveNo, int status)
        {
            try
            {
                Components.ObjNav.updateleave(LeaveNo, 3);
                {
                    Response.Redirect("RelieverListsing.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("RelieverListsing.aspx");
            }
        }
        public void Message(string strMsg)
        {
            string strScript = null;
            strScript = "<script>";
            strScript = strScript + "alert('" + strMsg + "');";
            strScript = strScript + "</script>";
            Page.RegisterStartupScript("ClientScript", strScript.ToString());
        }
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("LeaveListsing.aspx");
        }
    }
}