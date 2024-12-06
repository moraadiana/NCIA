using NCIASTaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff
{
    public partial class TrainingApplication : System.Web.UI.Page
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

                string query = Request.QueryString["query"].ToString();
                if (query == "new")
                {
                    MultiView1.SetActiveView(vwHeader);
                }
                else
                {
                    MultiView1.SetActiveView(vwLines);
                    string trainingNo = Request.QueryString["TrainingNo"].ToString();
                    Session["TrainingNo"] = trainingNo;
                    lblTrainingNo.Text = trainingNo;
                    BindGridViewData(trainingNo);
                }

                LoadStaffDetails();
                LoadBudgetVotes();
                LoadCourses();
                Loadparticipants();
                //LoadSupervisor();
                // LoadTrainer();
            }
        }
        private void Loadparticipants()
        {
            ddlParticipants.Items.Clear();
            try
            {
                ddlParticipants.Items.Clear();
                string Relievers = webportals.GetRelievers();
                if (!string.IsNullOrEmpty(Relievers))
                {
                    string[] relieverArr = Relievers.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string reliever in relieverArr)
                    {
                        string[] responseArr = reliever.Split(strLimiters, StringSplitOptions.None);
                        ListItem li = new ListItem(responseArr[1], responseArr[0]);
                        ddlParticipants.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

        }
        private void LoadBudgetVotes()
        {
            try
            {
                ddlDsaVote.Items.Clear();
                ddlTrainingVote.Items.Clear();
                ddlTransportVote.Items.Clear();

                //  int type = 1;
                string vote = webportals.GetTrainingVotes(5);
                if (!string.IsNullOrEmpty(vote))
                {
                    string[] voteArr = vote.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string votes in voteArr)
                    {
                        string[] responseArr = votes.Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length == 2)
                        {
                            string code = responseArr[0];
                            string description = responseArr[1];

                            // Create a ListItem with the month name as Text and the month number as Value
                            ListItem li = new ListItem(description, code);
                            ddlDsaVote.Items.Add(li); // Add the item to the dropdown
                            ddlTrainingVote.Items.Add(li);
                            ddlTransportVote.Items.Add(li);
                        }
                    }
                }
                else
                {
                    ddlDsaVote.Items.Clear();
                    ddlTrainingVote.Items.Clear();
                    ddlTransportVote.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ddlDsaVote.Items.Add(new ListItem("Error loading budget votes"));
            }
        }


   

        private void LoadCourses()
        {
            
            try
            {
                ddlCourse.Items.Clear();
               

                //  int type = 1;
                string course = webportals.GetTrainingVotes(3);
                if (!string.IsNullOrEmpty(course))
                {
                    string[] courseArr = course.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string courses in courseArr)
                    {
                        string[] responseArr = courses.Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length == 2)
                        {
                            string code = responseArr[0];
                            string description = responseArr[1];

                            // Create a ListItem with the month name as Text and the month number as Value
                            ListItem li = new ListItem(description, code);
                            ddlCourse.Items.Add(li); // Add the item to the dropdown
                            
                        }
                    }
                }
                else
                {
                    ddlCourse.Items.Clear();
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ddlCourse.Items.Add(new ListItem("Error loading courses"));
            }
        }

        private void LoadStaffDetails()
        {
            try
            {
                string username = Session["username"].ToString();
                string staffName = Session["staffName"].ToString();
                lblStaffNo.Text = username;
                lblStaffName.Text = staffName;
                string response = webportals.GetStaffDepartmentDetails(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        lblDirectorate.Text = responseArr[1];
                        lblDepartment.Text = responseArr[2];
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void txtStartDate_TextChanged(object sender, EventArgs e)
        {
            string startDate = txtStartDate.Text;

        }
        protected void txtEndDate_TextChanged(object sender, EventArgs e)
        {

        }
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string staffNo = Session["username"].ToString();
                string directorate = lblDirectorate.Text;
                string department = lblDepartment.Text;
                string Course = ddlCourse.SelectedValue;
                string type = ddlType.SelectedValue;
                string applicationType = ddlApplicationType.SelectedValue;
                string trainingClassification = ddlTrainingClassification.SelectedValue;
               
                string trainingNeed = txtTrainingNeed.Text;
                string trainingObjective = txtTrainingObjective.Text;
                string trainingMode = ddlModeofTraining.SelectedValue;
                string dsaVote = ddlDsaVote.SelectedValue;
                string trainingVote = ddlTrainingVote.SelectedValue;
                string transportVote = ddlTransportVote.SelectedValue;
                string startDate = txtStartDate.Text;
                string endDate = txtEndDate.Text;



                string response = webportals.HRMTrainingApplication(staffNo, Convert.ToInt32(type), Course, Convert.ToInt32(applicationType), Convert.ToInt32(trainingClassification), trainingNeed, trainingObjective, Convert.ToInt32(trainingMode), dsaVote, trainingVote, transportVote, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string trainingNo = responseArr[1];
                        MultiView1.SetActiveView(vwLines);
                        Session["TrainingNo"] = trainingNo;
                        lblTrainingNo.Text = trainingNo;
                        BindGridViewData(trainingNo);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        private void BindGridViewData(string trainingNo)
        {
            try
            {
                // Call the AL web service method
                string transportLines = webportals.GetTrainingLines(trainingNo);

                // Check if the response is not empty or null
                if (!string.IsNullOrEmpty(transportLines))
                {
                    // Split the response by '[]' to separate each line
                    string[] lineItems = transportLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    Console.WriteLine("Number of line items: " + lineItems.Length);

                    // Create a DataTable to hold the parsed data for binding
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Document No");
                    dt.Columns.Add("Staff No");
                    dt.Columns.Add("Staff Name");
                    dt.Columns.Add("Course Fee");
                    dt.Columns.Add("DSA Amount");
                    dt.Columns.Add("Transport Cost");

                    // Loop through each line item
                    foreach (string item in lineItems)
                    {
                        // Split each line by '::' to get individual fields
                        string[] fields = item.Split(strLimiters, StringSplitOptions.None);

                        // Check if we have the correct number of fields
                        if (fields.Length == 6)
                        {
                            DataRow row = dt.NewRow();
                            row["Document No"] = fields[0];
                            row["Staff No"] = fields[1];
                            row["Staff Name"] = fields[2];
                            row["Course Fee"] = fields[3];
                            row["DSA Amount"] = fields[4];
                            row["Transport Cost"] = fields[5];
                            dt.Rows.Add(row);
                        }
                        else
                        {
                            Console.WriteLine("Skipping invalid record: " + item);
                        }
                    }

                    // Bind the DataTable to the GridView
                    gvLines.DataSource = dt;
                    gvLines.DataBind();
                }
                else
                {
                    // Handle the case where there are no lines
                    gvLines.DataSource = null;
                    gvLines.DataBind();
                    Console.WriteLine("No transport lines returned.");
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log or show an error message as needed)
                Console.WriteLine("Error: " + ex.Message);
            }
        }

      

        protected void lbtnAddParticipant_Click(object sender, EventArgs e)
        {
            try
            {
                string trainingNo = lblTrainingNo.Text;
                string participant = ddlParticipants.SelectedValue;
               // string staffNo = Session["username"].ToString();
                string courseFee = txtCourseFee.Text;
                string dsaAmount = txtDsaAmount.Text;
                string transportCost = txtTransportCost.Text;



                  if (string.IsNullOrEmpty(participant))
                  {
                      Message("participant cannot be null or empty!");
                      ddlParticipants.Focus();
                      return;
                  }

                  if (string.IsNullOrEmpty(courseFee))
                  {
                      Message("Course Fee cannot be null or empty!");
                      txtCourseFee.Focus();
                      return;
                  }
                if (string.IsNullOrEmpty(dsaAmount))
                {
                    Message("DSA Amount cannot be null or empty!");
                    txtDsaAmount.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(transportCost))
                {
                    Message("Transport Cost cannot be null or empty!");
                    txtTransportCost.Focus();
                    return;
                }

                string response = webportals.InsertHRMTrainingParticipants(trainingNo, participant, Convert.ToDecimal(courseFee), Convert.ToDecimal(dsaAmount), Convert.ToDecimal(transportCost));
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        Message("Training participant has been added successfully");
                        txtCourseFee.Text = string.Empty;
                        txtDsaAmount.Text = string.Empty;
                        txtTransportCost.Text = string.Empty;
                        BindGridViewData(trainingNo);
                    }
                    else
                    {
                        Message(response);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string[] args = new string[2];
                args = (sender as LinkButton).CommandArgument.ToString().Split(';');
                string staffNo = args[0];
                string trainingNo = lblTrainingNo.Text;
                string response = webportals.RemoveHRMTrainingParticipant(trainingNo, staffNo);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        Message("Training participant removed successfully");
                        BindGridViewData(trainingNo);
                    }
                    else
                    {
                        Message("An error occured while removing the participant");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void btnSendForApproval_Click(object sender, EventArgs e)
        {
            try
            {
                string trainingNo = lblTrainingNo.Text;
                if (gvLines.Rows.Count < 1)
                {
                    Message("Please add lines before sending for approval!");
                    return;
                }
                //if (gvAttachments.Rows.Count < 1)
                //{
                //    Message("Please attach documents before sending for approval!");
                //    return;
                //}
                string msg = webportals.OnSendClaimRequisitionForApproval(trainingNo);//ClaimRequisitionApprovalRequest(claimNo);//
                if (msg == "SUCCESS")
                {
                    SuccessMessage($"Training number {trainingNo} has been sent for approval successfuly!");
                }
                else
                {
                    Message("ERROR:Approval Workflow not set"  );
                }
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
                ex.Data.Clear();
            }
        }
        private void SuccessMessage(string message)
        {
            string page = "TrainingListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "'</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        private void Message(string message)
        {
            string strScript = "<script>alert('"+message+"');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
    }
}