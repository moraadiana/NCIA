using NCIASTaff.NAVWS;
using System;

namespace NCIASTaff.pages
{
    public partial class StaffExit : System.Web.UI.Page
    {
        Staffportall webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }
            LoadStaffDepartmentDetails();



        }



        private void LoadStaffDepartmentDetails()
        {
            try
            {
                string username = Session["username"].ToString();

                // string staffNo = Session["username"].ToString();
                string staffName = Session["StaffName"].ToString();
                string response = webportals.GetStaffDepartmentDetails(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        //lblDirectorate.Text = responseArr[2];
                        lblDepartment.Text = responseArr[1];
                    }
                    else
                    {
                        Message("An error occured while loading details. Please try again later.");
                        return;
                    }
                }
                lblEmpNo.Text = username;
                lblEmpName.Text = staffName;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

        }


        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string Empno = Session["username"].ToString();
                string Empname = lblEmpName.Text;
                string department = lblDepartment.Text;
                string designation = lblDesignation.Text;
                string natureofLeaving = ddlNatureofLeaving.SelectedValue;
                //string leavingDate = txtleavingDate.Text;
                string reason = txtReason.Text;
                DateTime leavingDate = Convert.ToDateTime(txtleavingDate.Text);
                string response = webportals.CreateClearanceHeader(Empno, Empname, department, designation, Convert.ToInt32(natureofLeaving), Convert.ToDateTime(leavingDate), reason);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        SuccessMessage("Your application has been submitted.");
                    }
                    if (returnMsg == "FAILED")
                    {
                        SuccessMessage("The record for " + Empno + " already exists ");
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
        private void SuccessMessage(string message)
        {
            string page = "staffexitlisting.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "';</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
    }
}