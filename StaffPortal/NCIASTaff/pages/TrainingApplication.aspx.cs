using NCIASTaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                LoadIndividualCourses();
                LoadCountries();
                LoadSupervisor();
                LoadTrainer();
            }
        }

        private void LoadTrainer()
        {
            try
            {
                ddlTrainer.Items.Clear();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetVendors",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["Name"].ToString() == "") continue;
                        ListItem li = new ListItem(reader["Name"].ToString().ToUpper(), reader["No_"].ToString());
                        ddlTrainer.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadSupervisor()
        {
            try
            {
                ddlSupervisor.Items.Clear();
                ddlParticipants.Items.Clear();
                string username = Session["username"].ToString();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetRelievers",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //if (reader["No_"].ToString() == username) continue;
                        ListItem li = new ListItem(reader["Name"].ToString().ToUpper(), reader["No_"].ToString());
                        ddlSupervisor.Items.Add(li);
                        ddlParticipants.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadCountries()
        {
           /* try
            {
                ddlCountry.Items.Clear();
                var serviceRoot = Components.ServiceRoot;
                var context = new BC.NAV(new Uri(serviceRoot));
                context.BuildingRequest += Components.Context_BuildingRequest;
                var data = context.CountriesRegions.Execute();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        ListItem li = new ListItem(item.Name, item.Code);
                        ddlCountry.Items.Add(li);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }*/
        }

        private void LoadIndividualCourses()
        {
            /*try
            {
                ddlIndividualCourse.Items.Clear();
                var serviceRoot = Components.ServiceRoot;
                var context = new BC.NAV(new Uri(serviceRoot));
                context.BuildingRequest += Components.Context_BuildingRequest;
                var data = context.HRMCourseList.Execute();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        ListItem li = new ListItem(item.Course_Tittle.ToUpper(), item.Course_Code);
                        ddlIndividualCourse.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }*/
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

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string staffNo = Session["username"].ToString();
                string directorate = lblDirectorate.Text;
                string department = lblDepartment.Text;
                string location = ddlLocation.SelectedValue;
                string individualCourse = ddlIndividualCourse.SelectedValue;
                string country = ddlCountry.SelectedValue;
                string supervisor = ddlSupervisor.SelectedValue;
                string trainingCategory = ddlTrainingCategory.SelectedValue;
                string trainer = ddlTrainer.SelectedValue;
                string sponsor = ddlSponsor.SelectedValue;
                string county = ddlCounty.SelectedValue;
                string purpose = txtPurpose.Text;

                //string response = webportals.HRMTrainingApplication(staffNo, supervisor, Convert.ToInt32(trainingCategory), individualCourse, purpose, Convert.ToInt32(sponsor), Convert.ToInt32(location), country, county, trainer, directorate,department);
                //if (!string.IsNullOrEmpty(response))
                //{
                //    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                //    string returnMsg = responseArr[0];
                //    if (returnMsg == "SUCCESS")
                //    {
                //        string trainingNo = responseArr[1];
                //        MultiView1.SetActiveView(vwLines);
                //        Session["TrainingNo"] = trainingNo;
                //        lblTrainingNo.Text = trainingNo;
                //        BindGridViewData(trainingNo);
                //    }
                //}
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
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spTrainingApplicants",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@TrainingNo", "'" + trainingNo + "'");
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvLines.DataSource= dt;
                gvLines.DataBind();
                connection.Close();
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnAddParticipant_Click(object sender, EventArgs e)
        {
            try
            {
                string trainingNo = lblTrainingNo.Text;
                string participant = ddlParticipants.SelectedValue;
                string objective = txtObjective.Text;

                if (string.IsNullOrEmpty(objective))
                {
                    Message("Objective cannot be null or empty!");
                    txtObjective.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(participant))
                {
                    Message("Training participant cannot be null or empty!");
                    txtObjective.Focus();
                    return;
                }

                //string response = webportals.InsertHRMTrainingParticipants(trainingNo, participant, objective);
                //if(!string.IsNullOrEmpty(response))
                //{
                //    if (response == "SUCCESS")
                //    {
                //        Message("Training participant has been added successfully");
                //        txtObjective.Text = string.Empty;
                //        BindGridViewData(trainingNo);
                //    }
                //    else
                //    {
                //        Message(response);
                //    }
                //}
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
                string employeeCode = args[0];
                string trainingNo = lblTrainingNo.Text;
                //string response = webportals.RemoveHRMTrainingParticipant(trainingNo, employeeCode);
                //if(!string.IsNullOrEmpty(response))
                //{
                //    if(response == "SUCCESS")
                //    {
                //        Message("Training participant removed successfully");
                //        BindGridViewData(trainingNo);
                //    }
                //    else
                //    {
                //        Message("An error occured while removing the participant");
                //        return;
                //    }
                //}
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void Message(string message)
        {
            string strScript = "<script>alert('"+message+"');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
    }
}