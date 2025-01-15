using NCIASTaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class StaffExit1 : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                LoadStaffDepartmentDetails();
               
                string query = Request.QueryString["query"];
                string approvalStatus = Request.QueryString["status"].Replace("%", " ");
                if (query == "new")
                {
                    MultiView1.SetActiveView(vwHeader);

                }
                else
                {
                    string requestNo = Request.QueryString["requestNo"].ToString();
                    if (!string.IsNullOrEmpty(requestNo))
                    {
                        Session["requestNo"] = requestNo;
                        MultiView1.SetActiveView(vwLines);
                        BindGridViewData();
                    }
                    if (approvalStatus == "Open" || approvalStatus == "Pending")
                    {
                        //btnSendForApproval.Visible = true;
                        //lbtnSubmit.Visible = true;
                    }
                    else
                    {
                       // btnSendForApproval.Visible = false;
                    }

                }
            }
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
                       // SuccessMessage("Your application has been submitted.");
                        Message($"Clearance request has been created successfully.");
                        MultiView1.SetActiveView(vwLines);
                        BindGridViewData();
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
        private void NewView()
        {
            try
            {
                MultiView1.SetActiveView(vwLines);
                string requestNo = Session["requestNo"].ToString();
                string staffNo = Session["username"].ToString();
                BindGridViewData();

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        private void BindGridViewData()
        {
            string staffNo = Session["username"].ToString();
            string department = lblDepartment.Text;

            // Fetch the clearance setup (this assumes it calls the relevant AL procedure exposed as a web service)
            string clearanceSetupResponse = webportals.ClearanceSetup(); // Call ClearanceSetup

            // Process ClearanceSetup response and prepare to fetch clearance lines
            if (!string.IsNullOrEmpty(clearanceSetupResponse))
            {
                string[] setupLines = clearanceSetupResponse.Split(new[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);

                // Create or retrieve the existing DataTable
                DataTable dt = ViewState["GridViewData"] as DataTable;
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("Cleared By");
                    dt.Columns.Add("Clearer Name");
                    dt.Columns.Add("Department Code");
                    dt.Columns.Add("Designation");
                    dt.Columns.Add("Cleared");
                    dt.Columns.Add("Comments");
                    dt.Columns.Add("Date Cleared");

                    ViewState["GridViewData"] = dt;
                }

                foreach (string line in setupLines)
                {
                    string[] fields = line.Split(new[] { "::" }, StringSplitOptions.None);
                    if (fields.Length >= 5 && fields[0] == "SUCCESS")
                    {
                        DataRow row = dt.NewRow();
                        row["Cleared By"] = fields[1];
                        row["Clearer Name"] = fields[2];
                        row["Department Code"] = fields[3];
                        row["Designation"] = fields[4];
                        row["Cleared"] = string.Empty;  // Placeholder for future updates
                        row["Comments"] = string.Empty; // Placeholder for future updates
                        row["Date Cleared"] = string.Empty; // Placeholder for future updates

                        dt.Rows.Add(row);

                        // Fetch and process clearance lines for each "Cleared By"
                        // Modify the call to 'webportals.GetClearanceLines' to use your AL procedure via the web service
                        string clearanceLinesResponse = webportals.GetClearanceLines(staffNo, fields[1]); // Assuming fields[1] is "Cleared By"

                        if (!string.IsNullOrEmpty(clearanceLinesResponse))
                        {
                            string[] records = clearanceLinesResponse.Split(new[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string record in records)
                            {
                                string[] data = record.Split(new[] { "::" }, StringSplitOptions.None);
                                if (data.Length >= 4 && data[0] == "SUCCESS")
                                {
                                    //DataRow row = dt.NewRow();
                                    row["Cleared"] = data[1];
                                    row["Comments"] = data[2];
                                    row["Date Cleared"] = data[3];
                                }
                            }
                        }
                    }
                }

                // Bind the data to GridView
                gvLines.DataSource = dt;
                gvLines.DataBind();
            }
        }

       

       
        protected void btnSendForApproval_Click(object sender, EventArgs e)
        {

            List<string> results = new List<string>();

            foreach (GridViewRow row in gvLines.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    try
                    {
                        string appraisalNo = Session["appraisalNo"]?.ToString();
                        string subCode = row.Cells[1].Text.Trim();
                        string activityDescription = row.Cells[2].Text.Trim();
                        string annualTargetText = row.Cells[3].Text.Trim();
                        string criteria = ((TextBox)row.FindControl("txtCriteria")).Text.Trim();
                        //string annualTargetText = ((TextBox)row.FindControl("txtAnnualTarget")).Text.Trim();
                        string remarks = ((TextBox)row.FindControl("txtRemarks")).Text.Trim();

                        // Convert annualTarget to decimal
                        decimal annualTarget = 0;
                        if (!decimal.TryParse(annualTargetText, out annualTarget))
                        {
                            results.Add($"Error in row {row.RowIndex + 1}: Invalid Annual Target value.");
                            continue; // Skip to next row
                        }

                        string response = webportals.InsertAppraisalLines(appraisalNo, subCode, activityDescription, criteria, annualTarget, remarks);

                        if (!string.IsNullOrEmpty(response))
                        {
                            if (response.StartsWith("SUCCESS"))
                            {
                                string msg = webportals.OnSendAppraisalForApproval(appraisalNo);
                                if (msg == "SUCCESS")
                                {
                                    SuccessMessage($"Appraisal number {appraisalNo} has been sent for approval successfuly!");
                                }
                                else
                                {
                                    Message("ERROR:Approval Workflow not set");
                                }
                                //string approvalResponse = webportals.OnSendAppraisalForApproval(appraisalNo);
                                //if (approvalResponse == "SUCCESS")
                                //{
                                //    SuccessMessage("PAppraisal has been submitted successfully!");
                                //}
                                ////else
                                ////{
                                ////    Message(approvalResponse);
                                ////    return;
                                ////}
                                //else
                                //{
                                //    Message("ERROR:Approval Workflow not set");
                                //}
                            }
                            else
                            {
                                Message($"Error Submitting Appraisal lines .");
                                return;
                            }
                        }
                        else
                        {
                            Message($"Error ");
                        }

                    }
                    catch (Exception ex)
                    {
                        Message($"Error Submitting Appraisal lines   {ex.Message}");
                    }
                }
            }
        }

        private void SuccessMessage(string message)
        {
            string page = "AppraisalListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "'</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }


    }
}