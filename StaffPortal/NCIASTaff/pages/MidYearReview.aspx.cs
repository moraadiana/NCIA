using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCIASTaff.NAVWS;
using System.Xml.Linq;

namespace NCIASTaff.pages
{
    public partial class MidYearReview : System.Web.UI.Page
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
                string approvalStatus = Request.QueryString["status"].Replace("%", " ");
              

                string appraisalNo = Request.QueryString["appraisalNo"].ToString();
                Session["appraisalNo"] = appraisalNo;
                if (approvalStatus == "Open" || approvalStatus == "Pending")
                {
                    //lbtnSubmit.Visible = true;
                    lbtnSubmit.Visible = true;
                    btnEndYearReport.Visible = false;

                }
                else if (approvalStatus == "Approved")
                {
                    lbtnSubmit.Visible = false;
                    // lbnAddLine.Visible = false;
                    btnEndYearReport.Visible = true;
                }
                else
                {
                    lbtnSubmit.Visible = false;
                    // lbnAddLine.Visible = false;
                    btnEndYearReport.Visible = false;
                }
                MultiView1.SetActiveView(vwLines);
                BindGridViewData(appraisalNo);
                LoadAppraisalDetails();

            }
        }
        private void LoadAppraisalDetails()
        {
            try
            {
                string username = Session["username"].ToString();
                string appraisalNo = Session["appraisalNo"].ToString();
                string response = webportals.GetAppraisalHeaderDetails(appraisalNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        txtComments.Text = responseArr[1];
                        txtMitigation.Text = responseArr[2];
                        txtAssignments.Text = responseArr[3];
                    }
                    else
                    {
                        Message("An error occured while loading details. Please try again later.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        private bool CheckLinesAdded(string requestNo)
        {
            // Check if the GridView has any rows
            return gvLines.Rows.Count > 0;
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

                    // Create a DataTable to hold the parsed data for binding
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Sub Code");
                    dt.Columns.Add("Sub Activity");
                    dt.Columns.Add("Performance Criteria");
                    dt.Columns.Add("Annual Target");
                    dt.Columns.Add("Remarks");
                    dt.Columns.Add("Self Assessment");

                    foreach (string item in lineItems)
                    {
                        string[] fields = item.Split(strLimiters, StringSplitOptions.None);

                        // Check if we have the correct number of fields
                        if (fields.Length >= 6)
                        {
                            DataRow row = dt.NewRow();
                            row["Sub Code"] = fields[0];
                            row["Sub Activity"] = fields[1];
                            row["Performance Criteria"] = fields[2];
                            row["Annual Target"] = fields[3];
                            row["Remarks"] = fields[4];
                            row["Self Assessment"] = fields[5];

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


        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("MidYearReviewListing.aspx"); // Go back to the first view
        }
        protected void cancel(object sender, EventArgs e)
        {
            string message = "Are you sure you want to delete?";
            ClientScript.RegisterOnSubmitStatement(this.GetType(), "confirm", "return confirm('" + message + "');");
            string[] arg = new string[2];
            arg = (sender as LinkButton).CommandArgument.ToString().Split(';');
            string lineNo = arg[0];
            int intLineNo = Convert.ToInt32(lineNo);
            string appraisalNo = Session["appraisalNo"]?.ToString() ?? Request.QueryString["appraisalNo"];
            try
            {
                //string response = webportals.RemoveAppraisalLines(appraisalNo, intLineNo);
                //if (!string.IsNullOrEmpty(response))
                //{
                //    if (response == "SUCCESS")
                //    {
                //        Message("Appraisal Line removed successfully");
                //        BindGridViewData(appraisalNo);
                //    }
                //    else
                //    {
                //        Message("An error occured while removing the appraisal line");
                //        return;
                //    }
                //}

            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
        }
        
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {

            List<string> results = new List<string>();

            foreach (GridViewRow row in gvLines.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    try
                    {
                        string comments = txtComments.Text;
                        string mitigation = txtMitigation.Text;
                        string assignments = txtAssignments.Text;
                        string appraisalNo = Session["appraisalNo"]?.ToString();
                        string subCode = row.Cells[0].Text.Trim();
                        string subActivity = row.Cells[1].Text.Trim();
                        string performanceCriteria = row.Cells[2].Text.Trim();
                        string annualTarget = row.Cells[3].Text.Trim();
                        //string subCode = " ";
                        string remarks = " ";
                        string selfAssessment = ((TextBox)row.FindControl("txtSelfAssessment")).Text.Trim();

                        if (string.IsNullOrEmpty(selfAssessment) || selfAssessment.Trim() == "0")
                        {
                            Message("Self Assessment cannot be empty, or zero!");
                            return;
                        }


                        
                        
                         string response = webportals.InsertOrUpdateAppraisalLines(appraisalNo, subCode, subActivity, performanceCriteria, Convert.ToDecimal(annualTarget), remarks, Convert.ToDecimal(selfAssessment));

                        if (!string.IsNullOrEmpty(response))
                        {
                            if (response.StartsWith("SUCCESS"))
                            {
                                string headerResponse = webportals.UpdateAppraisalHeader(appraisalNo, comments, mitigation, assignments);
                                if (headerResponse == "SUCCESS")
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
                                }
                                else
                                {
                                    Message($"Error {headerResponse}");
                                }
                                

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
        protected void lbtnEndYearReport_Click(object sender, EventArgs e)
        {
            string appraisalNo = Session["AppraisalNo"].ToString();
            string trimmedAppraisalNo = appraisalNo.Remove(appraisalNo.Length - 1);
            //string response = webportals.InitiateQ2Appraisal(trimmedAppraisalNo);
            string response = webportals.InitiateEndYearNew(trimmedAppraisalNo);
            if (!string.IsNullOrEmpty(response))
            {
                string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                string returnMsg = responseArr[0];
                if (returnMsg == "SUCCESS")
                {
                    string documentNo = responseArr[1];
                    Session["AppraisalNo"] = documentNo;
                    SuccessMessage($"End Year Appraisal with number {documentNo} has been created successfully.Open it to add lines");


                }
                else if (returnMsg == "EXIST")
                {
                    string documentNo = responseArr[1];
                    Session["AppraisalNo"] = documentNo;
                    SuccessMessage($"End Year Review with number {documentNo} already exist. Open it to add lines");

                }
                else if (returnMsg == "ERROR")
                {
                    string startDate = responseArr[1];
                    string endDate = responseArr[2];
                    SuccessMessage($"End Year reporting runs from {startDate} to {endDate}");
                }

                else if (returnMsg == "FAILED")
                {
                    Message($"Error initiating End Year Review ");
                }
            }
        }
        
        private void SuccessMessage(string message)
        {
            string page = "MidYearReviewListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "'</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
        private void ErrMessage(string message)
        {
            string page = "MidYearReviewListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "'</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
        protected void gvLines_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string appraisalNo = Session["appraisalNo"]?.ToString() ?? Request.QueryString["appraisalNo"];
            gvLines.PageIndex = e.NewPageIndex;
            BindGridViewData(appraisalNo);  // Rebind your data here
        }
    }
}