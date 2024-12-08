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
                LoadSupervisor();
                LoadStaffDetails();
                LoadPeriod();
                string query = Request.QueryString["query"];

                if (query == "new")
                {
                    MultiView1.SetActiveView(vwHeader);

                }
                else
                {
                    string appraisalNo = Request.QueryString["appraisalNo"].ToString();
                    if (!string.IsNullOrEmpty(appraisalNo))
                    {
                        Session["appraisalNo"] = appraisalNo;
                        MultiView1.SetActiveView(vwLines);
                        BindGridViewData();
                    }

                }
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
                //lblPayee.Text = staffName;
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
                    BindGridViewData();
                  

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
            string unit = lblDirectorate.Text;
            string appraisalLines = webportals.CpActivityLines(staffNo);
            
            if (!string.IsNullOrEmpty(appraisalLines))
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("Sub-Activity Code");
                dt.Columns.Add("Sub-Activity Description");
                dt.Columns.Add("Perfomance Target");



                string[] lines = appraisalLines.Split(new[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);


                foreach (string line in lines)
                {
                    string[] fields = line.Split(new[] { "::" }, StringSplitOptions.None);
                    if (fields.Length >= 4 && fields[0] == "SUCCESS")
                    {
                        DataRow row = dt.NewRow();
                        row["Sub-Activity Code"] = fields[1];
                        row["Sub-Activity Description"] = fields[2];
                        row["Perfomance Target"] = fields[3];


                        dt.Rows.Add(row);
                    }
                }

                gvLines.DataSource = dt;
                gvLines.DataBind();


            }
            else
            {
                Message($"No approved workplan for {department}, {unit} ");
            }
            foreach (GridViewRow row in gvLines.Rows)
            {
                string appraisalNo = Session["appraisalNo"].ToString();
                TextBox txtCriteria = row.FindControl("txtCriteria") as TextBox;
               // TextBox txtAnnualTarget = row.FindControl("txtAnnualTarget") as TextBox;
                TextBox txtRemarks = row.FindControl("txtRemarks") as TextBox;

                string response = webportals.GetAppraisalLines(appraisalNo);

                if (!string.IsNullOrEmpty(response) && response.StartsWith("SUCCESS"))
                {
                    string[] records = response.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < records.Length; i++)
                    {
                        string[] data = records[i].Split(new string[] { "::" }, StringSplitOptions.None);

                        if (data.Length >= 4)
                        {
                            string performanceCriteria = data[1];
                            //string annualTarget = data[2];
                            string remarks = data[3];

                            if (i == row.RowIndex) 
                            {
                                txtCriteria.Text = performanceCriteria;
                              //  txtAnnualTarget.Text = annualTarget;
                                txtRemarks.Text = remarks;
                            }
                        }
                    }
                }
                else
                {
                    txtCriteria.Text = "";
                   // txtAnnualTarget.Text = "";
                    txtRemarks.Text = "";
                }
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
                        string criteria = ((TextBox)row.FindControl("txtCriteria")).Text.Trim();
                        string annualTargetText = ((TextBox)row.FindControl("txtAnnualTarget")).Text.Trim();
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

                                string approvalResponse = webportals.OnSendPettyCashSurrenderForApproval(appraisalNo);
                                if (approvalResponse == "SUCCESS")
                                {
                                    SuccessMessage("PAppraisal has been submitted successfully!");
                                }
                                //else
                                //{
                                //    Message(approvalResponse);
                                //    return;
                                //}
                                else
                                {
                                    Message("ERROR:Approval Workflow not set");
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
                        Message($"Error Submitting Appraisal lines   { ex.Message}");
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