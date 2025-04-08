using NCIASTaff.NAVWS;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class TransportRequisition : System.Web.UI.Page
    {
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
                LoadStaffDepartmentDetails();

                string requestNo = Request.QueryString["RequestNo"];


                if (requestNo != null)
                {
                    MultiView1.ActiveViewIndex = 1;
                    BindGridviewData(requestNo);
                }
                else
                {
                    MultiView1.ActiveViewIndex = 0;
                    //LoadApprovedMemos();
                    //LoadStations();
                    //LoadBudgetLines();
                    //LoadProjects();
                }
            }
        }
        private void LoadStaffDepartmentDetails()
        {
            try
            {
                string username = Session["username"].ToString();
                string response = webportals.GetStaffDepartmentDetails(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        lblUnitCode.Text = responseArr[2];
                        lblDepartment.Text = responseArr[1];
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

        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return; // Validate fields before proceeding

            try
            {
                string empNo = Session["username"].ToString();
                string username = Session["StaffName"].ToString().ToUpper().Replace(" ", ".");
                string reqNo = Session["RequestNo"]?.ToString() ?? string.Empty;
                //string memoNo = ddlMemoNo.SelectedValue;
                string description = txtDescription.Text.Trim();
                DateTime dateOfTravel;
                if (!DateTime.TryParse(txtDateOfTravel.Text, out dateOfTravel))
                {
                    Message("Invalid date of travel. Please enter a valid date.");
                    return;
                }

                decimal noOfDays;
                if (!decimal.TryParse(txtNoOfDays.Text, out noOfDays) || noOfDays <= 0)
                {
                    Message("Number of days must be a positive number.");
                    return;
                }

                DateTime expectedReturnDate = dateOfTravel.AddDays((double)noOfDays);
                string from = txtFrom.Text.Trim();
                string destination = txtDestination.Text.Trim();
                string unitCode = lblUnitCode.Text;
                string department = lblDepartment.Text;
                //string budgetLineCode = ddlBudgetLine.SelectedValue;
                // string projectCode = ddlProjectCode.SelectedValue;
                string createdBy = empNo;

                string details = Components.ObjNav.CreateTransportRequest(reqNo, description, dateOfTravel, noOfDays, expectedReturnDate, from, destination, unitCode, department, createdBy);
                Message(details);

                // Extract Request No from the details message
                if (details.StartsWith("SUCCESS"))
                {
                    string[] detailsParts = details.Split(new[] { "Request No: " }, StringSplitOptions.None);
                    if (detailsParts.Length > 1)
                    {
                        string requestNo = detailsParts[1].Trim();
                        Session["RequestNo"] = requestNo;
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle exception if needed
                ex.Data.Clear();
            }

            // Switch to the second view
            MultiView1.ActiveViewIndex = 1;
        }

        private bool ValidateFields()
        {
            // Validate each field here
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                Message("Description is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDateOfTravel.Text))
            {
                Message("Date of Travel is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNoOfDays.Text))
            {
                Message("Number of Days is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFrom.Text))
            {
                Message("From location is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                Message("Destination is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(lblUnitCode.Text))
            {
                Message("Unit Code is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(lblDepartment.Text))
            {
                Message("Department is required.");
                return false;
            }



            return true;
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransportRequisitionListing.aspx"); // Go back to the first view
        }

        protected void lbnClose_Click(object sender, EventArgs e)
        {
            newLines.Visible = false;
            lbnAddLine.Visible = true;
        }

        protected void btnLine_Click(object sender, EventArgs e)
        {
            try
            {
                string passengerNo = ddlPassengerNo.SelectedValue;
                string passengerDetails = ddlPassengerNo.SelectedItem.Text;

                string[] details = passengerDetails.Split(new string[] { " - " }, StringSplitOptions.None);
                string passengerName = details.Length > 1 ? details[1] : string.Empty;
                string passengerPhoneNo = details.Length > 2 ? details[2] : string.Empty;


                //string reqNo = Session["requestNo"]?.ToString();
                //string reqNo = Request.QueryString["RequestNo"].ToString();
                string reqNo = Session["requestNo"]?.ToString() ?? Request.QueryString["RequestNo"];

                if (string.IsNullOrEmpty(reqNo))
                {
                    Message("Error: No requisition selected.");
                    return;
                }

                string result = Components.ObjNav.InsertTransportPassengers(reqNo, passengerNo, passengerName, passengerPhoneNo);


                if (!String.IsNullOrEmpty(result))
                {
                    string returnMsg = "";
                    string[] strdelimiters = new string[] { "::" };
                    string[] result_arr = result.Split(strdelimiters, StringSplitOptions.None);

                    returnMsg = result_arr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        Message("Passenger added successfully.");
                        ddlPassengerNo.SelectedIndex = 0;
                        txtName.Text = null;
                        txtPhoneNo.Text = null;

                        BindGridviewData(reqNo);
                    }
                }
            }
            catch (Exception ex)
            {
                Message("An error occurred while adding the passenger. Please try again.");

            }
        }

        private void HandlePassengerInsertionResult(string result, string reqNo)
        {
            if (!string.IsNullOrEmpty(result))
            {
                string[] resultParts = result.Split(new[] { "::" }, StringSplitOptions.None);
                string returnMsg = resultParts[0];

                if (returnMsg == "SUCCESS")
                {
                    Message("Passenger added successfully.");
                    //ddlPassengerNo.SelectedIndex = 0;
                    ddlPassengerNo.Items.Clear();
                    txtName.Text = null;
                    txtPhoneNo.Text = null;
                    BindGridviewData(reqNo);
                }
            }
        }

        private void BindGridviewData(string requestNo)
        {
            string passengerData = Components.ObjNav.GetTransportReqPassengers(requestNo);
            if (!string.IsNullOrEmpty(passengerData) && passengerData != "No passengers found for the given requisition.")
            {
                var passengerRows = passengerData.Split('|');
                var passengerList = new DataTable();
                passengerList.Columns.Add("PassengerNo");
                passengerList.Columns.Add("Name");
                passengerList.Columns.Add("PhoneNo");

                foreach (var row in passengerRows)
                {
                    var columns = row.Split(new[] { "::" }, StringSplitOptions.None);
                    if (columns.Length == 3)
                    {
                        passengerList.Rows.Add(columns[0], columns[1], columns[2]);
                    }
                }

                gvLines.DataSource = passengerList;
                gvLines.DataBind();
            }
            else
            {
                gvLines.DataSource = null;
                gvLines.DataBind();
            }
        }

        private void Message(string message)
        {
            string strScript = $"<script>alert('{message}');</script>";
            Page.RegisterStartupScript("ClientScript", strScript);
        }

        protected void lbnAddLine_Click(Object sender, EventArgs e)
        {
            LoadPassengers();
            newLines.Visible = true;
            lbnAddLine.Visible = false;
        }

        protected void lbtnBack1_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransportRequisitionListing.aspx");
        }


        protected void cancel(object sender, EventArgs e)
        {
            string message = "Are you sure you want to delete?";
            ClientScript.RegisterOnSubmitStatement(this.GetType(), "confirm", "return confirm('" + message + "');");
            string[] arg = new string[2];
            arg = (sender as LinkButton).CommandArgument.ToString().Split(';');
            string passengerNo = arg[0];
            string reqNo = Session["requestNo"]?.ToString() ?? Request.QueryString["RequestNo"];
            try
            {
                Components.ObjNav.RemoveTransportPassengers(passengerNo);
                Message("Deleted successfully");
                BindGridviewData(reqNo);

            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message.ToString());
                ex.Data.Clear();
            }
        }
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {

            string reqNo = Session["requestNo"]?.ToString() ?? Request.QueryString["RequestNo"];


            if (!CheckPassengersAdded(reqNo))
            {
                Message("Please add at least one passenger before submitting for approval.");
                return;
            }

            try
            {
                string resultMessage = Components.ObjNav.OnSentTransportRequisitionForApproval(reqNo);
                if (resultMessage == "SUCCESS")
                {
                    ShowMessageAndRedirect("Your request has been sent for approval.", "TransportRequisitionListing.aspx");
                }
                else
                {
                    ShowMessageAndRedirect(resultMessage, "TransportRequisitionListing.aspx");
                }

            }
            catch (Exception ex)
            {
                ShowMessageAndRedirect("ERROR: " + ex.Message, "TransportRequisitionListing.aspx");

            }

        }
        private void ShowMessageAndRedirect(string message, string redirectUrl)
        {
            string script = $"alert('{message}'); window.location='{redirectUrl}';";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
        }
        private bool CheckPassengersAdded(string requestNo)
        {
            // Check if the GridView has any rows
            return gvLines.Rows.Count > 0;
        }

        //private void LoadApprovedMemos()
        //{
        //    string approvedMemos = Components.ObjNav.GetApprovedMemos();
        //    if (!string.IsNullOrEmpty(approvedMemos))
        //    {
        //        string[] memos = approvedMemos.Split('|');
        //        ddlMemoNo.Items.Clear();

        //        foreach (string memo in memos)
        //        {
        //            string[] details = memo.Split(new[] { "::" }, StringSplitOptions.None);
        //            if (details.Length == 1)
        //            {
        //                string memoNoValue = details[0]; // Memo No
        //                ddlMemoNo.Items.Add(new System.Web.UI.WebControls.ListItem(memoNoValue, memoNoValue));
        //            }
        //        }
        //    }
        //}
        /*
                private void LoadStations()
                {
                    string stationCodes = Components.ObjNav.GetStations();

                    if (!string.IsNullOrEmpty(stationCodes))
                    {

                        string[] stations = stationCodes.Split('|');


                        ddlStationCode.Items.Clear();

                        foreach (string station in stations)
                        {

                            string[] details = station.Split(new string[] { "::" }, StringSplitOptions.None);
                            if (details.Length == 1)
                            {
                                string stationName = details[0]; // Memo No
                                                                 //   string createdBy = details[1];   // Created By

                                // Add new item to the dropdown list
                                System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem($"{stationName}", stationName);
                                ddlStationCode.Items.Add(listItem);
                            }
                        }
                    }

                }

                private void LoadBudgetLines()
                {
                    string budgetLineList = Components.ObjNav.GetBudgetLines();

                    if (!string.IsNullOrEmpty(budgetLineList))
                    {

                        string[] budgetLines = budgetLineList.Split('|');


                        ddlBudgetLine.Items.Clear();

                        foreach (string budgetLine in budgetLines)
                        {

                            string[] details = budgetLine.Split(new string[] { "::" }, StringSplitOptions.None);
                            if (details.Length == 2)
                            {
                                string budgetLineCode = details[0];
                                string budgetLineName = details[1];

                                // Add new item to the dropdown list
                                System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem($"{budgetLineCode} - {budgetLineName}", budgetLineCode);
                                ddlBudgetLine.Items.Add(listItem);
                            }
                        }
                    }

                }
                private void LoadProjects()
                {
                    string projectList = Components.ObjNav.GetProjects();

                    if (!string.IsNullOrEmpty(projectList))
                    {

                        string[] projectsList = projectList.Split('|');


                        ddlProjectCode.Items.Clear();

                        foreach (string projectlist in projectsList)
                        {

                            string[] details = projectlist.Split(new string[] { "::" }, StringSplitOptions.None);
                            if (details.Length == 2)
                            {
                                string projectCode = details[0];
                                string projectName = details[1];

                                // Add new item to the dropdown list
                                System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem($"{projectCode} - {projectName}", projectCode);
                                ddlProjectCode.Items.Add(listItem);
                            }
                        }
                    }

                }*/
        private void LoadPassengers()
        {
            string passengerList = Components.ObjNav.GetPassengers();

            if (!string.IsNullOrEmpty(passengerList))
            {

                string[] passengersList = passengerList.Split('|');


                ddlPassengerNo.Items.Clear();
                ddlPassengerNo.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Passenger--", ""));
                foreach (string passengerlist in passengersList)
                {

                    string[] details = passengerlist.Split(new string[] { "::" }, StringSplitOptions.None);
                    //if (details.Length == 3)
                    //{
                    string passengerNo = details[0];
                    string passengerName = details[1];
                    string passengerPhoneNo = details[2];

                    // Add new item to the dropdown list
                    System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem($"{passengerNo} - {passengerName} - {passengerPhoneNo}", passengerNo);
                    ddlPassengerNo.Items.Add(listItem);
                    ////txtName.Text = passengerName;
                    //txtPhoneNo.Text = passengerPhoneNo;
                    // }
                }
            }
        }

    }
}