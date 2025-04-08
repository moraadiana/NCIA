using NCIASTaff.NAVWS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class AppraisalLines : System.Web.UI.Page
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
                
                string query = Request.QueryString["query"];
                ViewState["SubCode"] = 1;
                string approvalStatus = Request.QueryString["status"].Replace("%", " ");
               // string appraisalNo = Request.QueryString["appraisalNo"].ToString();
              //  BindGridViewData(appraisalNo);
                if (query == "new")
                {
                    MultiView1.SetActiveView(vwHeader);

                }
                else if (query == "old")
                {
                    string appraisalNo = Request.QueryString["appraisalNo"].ToString();
                    Session["appraisalNo"] = appraisalNo;
                    MultiView1.SetActiveView(vwLines);
                    BindGridViewData(appraisalNo);
                    // }


                }
                if (approvalStatus == "Open" || approvalStatus == "Pending")
                {
                    btnSendForApproval.Visible = true;
                    lbtnSubmit.Visible = true;
                    lbnAddLine.Visible = true;
                    btnMidYearReport.Visible = false;
                }
                else if (approvalStatus == "Approved")
                {
                    btnSendForApproval.Visible = false;
                    lbtnSubmit.Visible = false;
                    lbnAddLine.Visible = false;
                    btnMidYearReport.Visible = true;
                }
                else
                {
                    btnSendForApproval.Visible = false;
                    lbtnSubmit.Visible = false;
                    lbnAddLine.Visible = false;
                    btnMidYearReport.Visible = false;
                }
                LoadSupervisor();
                LoadStaffDetails();
                LoadPeriod();

            }
        }
        private void LoadSupervisor()
        {
            try
            {
                ddlSupervisor.Items.Clear();
                string Relievers = webportals.GetRelievers();
                if (!string.IsNullOrEmpty(Relievers))
                {
                    string[] relieverArr = Relievers.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string reliever in relieverArr)
                    {
                        string[] responseArr = reliever.Split(strLimiters, StringSplitOptions.None);
                        ListItem li = new ListItem(responseArr[1], responseArr[0]);
                        ddlSupervisor.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadPeriod()
        {
           // lblPeriod
           string period = webportals.GetAppraisalPeriod();
            if (!string.IsNullOrEmpty(period))
            {
                string[] periodArr = period.Split(strLimiters2,StringSplitOptions.RemoveEmptyEntries);
                foreach (string Period in periodArr)
                {
                    string[] responseArr = Period.Split(strLimiters, StringSplitOptions.None);
                    lblPeriod.Text = responseArr[1];
                }
               

            }
        }
        private void LoadStaffDetails()
        {
            try
            {
                string staffNo = Session["username"].ToString();
                string staffName = Session["StaffName"].ToString();
                string response = webportals.GetStaffDepartmentDetails(staffNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {

                        lblDepartment.Text = responseArr[1];
                        lblDirectorate.Text = responseArr[2];
                    }
                }

                lblStaffNo.Text = staffNo;
                
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            string username = Session["username"].ToString();
            string department = lblDepartment.Text;
            string unit = lblDirectorate.Text;
            string period = lblPeriod.Text;
            string supervisor = ddlSupervisor.SelectedValue;
            string documentNo = string.Empty;


            if (string.IsNullOrEmpty(department))
            {
                Message("Department cannot be null");
                return;
            }
            if (string.IsNullOrEmpty(unit))
            {
                Message("Unit cannot be null");
                return;
            }
            if (string.IsNullOrEmpty(period))
            {
                Message("Period cannot be null");
                return;
            }
            if (string.IsNullOrEmpty(supervisor)) 
            {
                Message("Reliever cannot be null");
                return;
            }
            string response = webportals.CreateAppraisalHeader(username, supervisor, period);
            if (!string.IsNullOrEmpty(response))
            {
                string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                string returnMsg = responseArr[0];
                if (returnMsg == "SUCCESS")
                {
                    documentNo = responseArr[1];
                    Session["AppraisalNo"] = documentNo;
                    Message($"Appraisal with number {documentNo} has been created successfully.");
                    MultiView1.SetActiveView(vwLines);
                    BindGridViewData(documentNo);
                  

                }
                else
                {
                    Message("An error occured while surendering imprest. Please try again later");
                    return;
                }
            }
            
        }
        private void NewView()
        {
            try
            {
                MultiView1.SetActiveView(vwLines);
                string appraisalNo = Session["appraisalNo"].ToString();
                string staffNo = Session["username"].ToString();
                BindGridViewData(appraisalNo);
              
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void BindGridViewData(string appraisalNo)
        {
            try
            {
                string appraisalLines = webportals.GetAppraisalLines(appraisalNo);

                if (!string.IsNullOrEmpty(appraisalLines))
                {
                    string[] lineItems = appraisalLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    Console.WriteLine("Number of line items: " + lineItems.Length);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Sub Code");
                    dt.Columns.Add("Sub Activity");
                    dt.Columns.Add("Performance Criteria");
                    dt.Columns.Add("Annual Target");
                    dt.Columns.Add("Remarks");

                    foreach (string item in lineItems)
                    {

                        string[] fields = item.Split(strLimiters, StringSplitOptions.None);

                        if (fields.Length >= 5)
                        {
                            DataRow row = dt.NewRow();
                            row["Sub Code"] = fields[0];
                            row["Sub Activity"] = fields[1];
                            row["Performance Criteria"] = fields[2];
                            row["Annual Target"] = fields[3];
                            row["Remarks"] = fields[4];

                            dt.Rows.Add(row);
                        }
                        else
                        {
                            Console.WriteLine("Skipping invalid record: " + item);
                        }
                    }

                    gvLines.DataSource = dt;
                    gvLines.DataBind();
                }
                else
                {
                    gvLines.DataSource = null;
                    gvLines.DataBind();
                    Console.WriteLine("No lines returned.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
     
        protected void btnLine_Click(object sender, EventArgs e)
        {
            try
            {

                //string subCode = txtSubCode.Text;
                int currentSubCode = 1;
                if (ViewState["SubCode"] != null)
                {
                    currentSubCode = (int)ViewState["SubCode"];
                }
                // Use currentSubCode as your subCode
                string subCode = currentSubCode.ToString("D2");

                // Then update the hidden field for the next insert


                string subActivity = txtSubActivity.Text;
                string performanceCriteria = txtPerformanceCriteria.Text;
                string annualTarget = txtAnnualTarget.Text;
                string remarks = txtRemarks.Text;
                string selfAssessment = "0.00";
                string appraisalNo = Session["appraisalNo"]?.ToString();



                if (string.IsNullOrEmpty(subActivity))
                {
                    Message("Sub Activity cannot be null or empty!");
                    txtSubActivity.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(performanceCriteria))
                {
                    Message("Performance Criteria cannot be null or empty!");
                    txtPerformanceCriteria.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(annualTarget))
                {
                    Message("Annual Target cannot be null or empty!");
                    txtAnnualTarget.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(remarks))
                {
                    Message("Remarks Criteria cannot be null or empty!");
                    txtRemarks.Focus();
                    return;
                }

                string response = webportals.InsertOrUpdateAppraisalLines(appraisalNo, subCode, subActivity, performanceCriteria,Convert.ToDecimal(annualTarget),remarks,Convert.ToDecimal(selfAssessment));
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        Message("Appraisal line has been added successfully");
                        // Increment subCode for next entry and update hidden field
                        currentSubCode++;
                        ViewState["SubCode"] = currentSubCode;


                        BindGridViewData(appraisalNo);
                    }
                    else if (response == "FAILED")
                    {
                        Message($"Error adding line{response} ");
                    }

                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();

            }
        }
        protected void gvLines_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string appraisalNo = Session["appraisalNo"]?.ToString() ?? Request.QueryString["appraisalNo"];
            gvLines.PageIndex = e.NewPageIndex;
            BindGridViewData(appraisalNo);  // Rebind your data here
        }
        protected void lbnAddLine_Click(Object sender, EventArgs e)
        {
            newLines.Visible = true;
            lbnAddLine.Visible = false;
        }
        protected void lbnClose_Click(object sender, EventArgs e)
        {
            newLines.Visible = false;
            lbnAddLine.Visible = true;
        }
        protected void btnSendForApproval_Click(object sender, EventArgs e)
        {
            try
            {
                
                string appraisalNo = Session["appraisalNo"]?.ToString() ?? Request.QueryString["appraisalNo"];
                if (gvLines.Rows.Count < 1)
                {
                    Message("Please add lines before sending for approval!");
                    return;
                }
               

                string msg = webportals.OnSendAppraisalForApproval(appraisalNo);
                if (msg == "SUCCESS")
                {
                    SuccessMessage($"Appraisal number {appraisalNo} has been sent for approval successfuly!");
                }
                else
                {
                    Message("ERROR:Approval Workflow not set");
                }
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
                ex.Data.Clear();
            }
        }
        protected void btnSendForApproval_Click1(object sender, EventArgs e)
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
                        Message($"Error Submitting Appraisal lines   { ex.Message}");
                    }
                }
            }
        }
        protected void lbtnMidYearReport_Click(object sender, EventArgs e)
        {
            string appraisalNo = Session["AppraisalNo"].ToString();
            string response = webportals.InitiateMidYearNew(appraisalNo);
            if (!string.IsNullOrEmpty(response))
            {
                string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                string returnMsg = responseArr[0];
                if (returnMsg == "SUCCESS")
                {
                    string documentNo = responseArr[1];
                    Session["AppraisalNo"] = documentNo;
                    SuccessMessage($"Mid Year Appraisal with number {documentNo} has been created successfully.Open it to add lines");


                }
                else if (returnMsg == "EXIST")
                {
                    string documentNo = responseArr[1];
                    Session["AppraisalNo"] = documentNo;
                    SuccessMessage($"Mid Year Appraisal with number {documentNo} already exist. Open it to add lines");

                }
                else if (returnMsg == "ERROR")
                {
                    string startDate = responseArr[1];
                    string endDate = responseArr[2];
                    SuccessMessage($"Mid Year reporting runs from {startDate} to {endDate}");
                }

                else if (returnMsg == "FAILED")
                {
                    Message($"Error initiating Mid Year Review ");
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