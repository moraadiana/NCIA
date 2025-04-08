using System;
using System.Web.UI;

namespace NCIASTaff.pages
{
    public partial class Revieweraccept : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["status"] == "5")
            {
                Message("This Leave has already been approved and therefore cannot be cancelled.");
                return;
            }
            Accept(Request.QueryString["appNo"], 1);
        }
        protected void Accept(string LeaveNo, int status)
        {
            try
            {
                //string RequiNo = Request.QueryString["An"].ToString();
                //Session["ReqNo"] = RequiNo;
                Components.ObjNav.updateleave(LeaveNo, 2);
                {
                    //string mymail = Components.emailcomp.ToString();
                    //if (!string.IsNullOrEmpty(reliever))
                    {
                        string emailBody = "<center><img src='/images/logo.png' alt=NDMA Reliever Request' Height='100px' Width='100px'/ >" +
                                       "<br/><b style='font-family:monotype-corsiva; font-size:15px'>NDMA</b></center>" +
                                       "<hr style='height: 5px; border - width:0; color: #0c8c01; background - color:#0c8c01'>" +
                                       "Hello, " +
                                          "<br/><br/>Please Note that your reliever has accepted the your reliever request for leave no " + LeaveNo + " <br>" +
                                          "Please login to your portal and send the leave for approval<br/><br/> " +
                                           "To access your self-service portal, please use the following link: ";//link to be provided [URL link: http://portal.ndma.go.ke/] //+ mymail +

                        //Components.SendMyEmail(mymail, "NDMA Reliever Request", emailBody);
                        {
                            Message("Email Sent to requestor");
                        }
                        /*
                        else
                        {
                            Message("Failed to send email to reliever");// +
                            //"but fail to send activation mail, Please contact your system administrator.");
                        }*/
                    }
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