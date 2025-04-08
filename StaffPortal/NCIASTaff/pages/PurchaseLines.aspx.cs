using NCIASTaff.NAVWS;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class PurchaseLines : System.Web.UI.Page
    {
        Staffportall webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter adapter;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
                LoadResponsibilityCenter();
                LoadStaffDetails();

                string query = Request.QueryString["query"].ToString();
                if (query == "old")
                {
                    string approvalStatus = Request.QueryString["status"].Replace("%", " ");
                    string requisitionNo = Request.QueryString["PRNNo"].ToString();
                    lblLPurchaseNo.Text = requisitionNo;
                    lblPurchaseNo.Text = requisitionNo;
                    MultiView1.SetActiveView(vwLines);
                    BindGridViewData(requisitionNo);
                    LoadAccounts();
                    if (approvalStatus == "Open" || approvalStatus == "Pending")
                    {
                        newLines.Visible = true;
                        lbtnCloseLines.Visible = true;
                        lbtnAddLines.Visible = false;
                        lbtnApproval.Visible = true;
                        lbtnCancel.Visible = false;
                    }
                    else if (approvalStatus == "Pending Approval")
                    {
                        newLines.Visible = false;
                        lbtnCloseLines.Visible = false;
                        lbtnAddLines.Visible = false;
                        lbtnApproval.Visible = false;
                        lbtnCancel.Visible = true;
                    }
                    else
                    {
                        newLines.Visible = false;
                        lbtnCloseLines.Visible = false;
                        lbtnAddLines.Visible = false;
                        lbtnApproval.Visible = false;
                        lbtnCancel.Visible = false;
                    }
                }
                else
                {
                    MultiView1.SetActiveView(vwHeader);
                }
            }
        }

        private void LoadStaffDetails()
        {
            try
            {
                string username = Session["username"].ToString();
                string staffName = Session["staffName"].ToString();
                string departmentDetails = webportals.GetStaffDepartmentDetails(username);
                if (!string.IsNullOrEmpty(departmentDetails))
                {
                    string[] departmentDetailsArr = departmentDetails.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = departmentDetailsArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        lblDirectorate.Text = departmentDetailsArr[1];
                        lblDepartment.Text = departmentDetailsArr[2];
                    }
                }
                lblUserId.Text = username;
                lblStaffName.Text = staffName;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadResponsibilityCenter()
        {
            try
            {
                ddlResponsibilityCenter.Items.Clear();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetResponsilityCenter",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListItem li = new ListItem(reader["Name"].ToString().ToUpper(), reader["Code"].ToString());
                        ddlResponsibilityCenter.Items.Add(li);

                    }
                }
                connection.Close();

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
                string username = Session["username"].ToString();
                string directorate = lblDirectorate.Text;
                string department = lblDepartment.Text;
                string requiredDate = txtRequiredDate.Text;
                string responsibilityCenter = ddlResponsibilityCenter.SelectedValue.ToString();
                string description = txtDescription.Text;
                /*
                                if (string.IsNullOrEmpty(directorate) || string.IsNullOrEmpty(department))
                                {
                                    Message("Please ensure that you have the directorate and department defined.");
                                    return;
                                }*/
                if (string.IsNullOrEmpty(requiredDate))
                {
                    Message("Required date cannot be empty.");
                    return;
                }
                if (string.IsNullOrEmpty(responsibilityCenter))
                {
                    Message("Responsibility center cannot be empty.");
                    return;
                }
                if (string.IsNullOrEmpty(description))
                {
                    Message("Request description cannot be empty.");
                    return;
                }
                if (description.Length > 200)
                {
                    Message("Request description cannot only be 200 characters and below.");
                    return;
                }

                string response = webportals.CreatePurchaseRequisitionHeader(username, description, Convert.ToDateTime(requiredDate), responsibilityCenter);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string purchaseNo = responseArr[1];
                        Session["RequisitionNo"] = purchaseNo;
                        Message($"Purchase requisition with number {purchaseNo} has been created successfully.");
                        NewView();
                    }
                    else
                    {
                        Message("An error occured while creating the purchase header. Please try again");
                        return;
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
            string requisitionNo = Session["RequisitionNo"].ToString();
            lblLPurchaseNo.Text = requisitionNo;
            lblPurchaseNo.Text = requisitionNo;
            newLines.Visible = true;
            lbtnCloseLines.Visible = true;
            lbtnAddLines.Visible = false;
            lbtnApproval.Visible = true;
            lbtnCancel.Visible = false;
            LoadAccounts();
            BindGridViewData(requisitionNo);
            MultiView1.SetActiveView(vwLines);
        }

        protected void lbtnAddLines_Click(object sender, EventArgs e)
        {
            newLines.Visible = true;
            lbtnAddLines.Visible = false;
            lbtnCloseLines.Visible = true;
        }

        protected void lbtnCloseLines_Click(object sender, EventArgs e)
        {
            newLines.Visible = false;
            lbtnAddLines.Visible = true;
            lbtnCloseLines.Visible = false;
        }

        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        private void SuccessMessage(string message)
        {
            string page = "PurchaseListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "';</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string type = ddlType.SelectedValue;
                if (type == "0")
                {
                    LoadAccounts();
                }
                else if (type == "1")
                {
                    LoadItems();
                }
                else
                {
                    LoadFixedAssets();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadFixedAssets()
        {
            try
            {
                ddlItem.Items.Clear();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetFixedAssets",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListItem li = new ListItem(reader["Description"].ToString().ToUpper(), reader["No_"].ToString());
                        ddlItem.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadItems()
        {
            try
            {
                ddlItem.Items.Clear();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetStoreItems",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListItem li = new ListItem(reader["Description"].ToString().ToUpper(), reader["Code"].ToString());
                        ddlItem.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadAccounts()
        {
            try
            {
                ddlItem.Items.Clear();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetGlAccounts",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListItem li = new ListItem(reader["Name"].ToString().ToUpper(), reader["No_"].ToString());
                        ddlItem.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string type = ddlType.SelectedValue;
                string item = ddlItem.SelectedValue;
                string quantity = txtQuantity.Text;
                string requisitionNo = lblPurchaseNo.Text;

                if (string.IsNullOrEmpty(quantity))
                {
                    Message("Quantity cannot be empty");
                    txtQuantity.Focus();
                    return;
                }

                string response = webportals.CreatePurchaseRequisitionLine(Convert.ToInt32(type), item, requisitionNo, "", Convert.ToDecimal(quantity));
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        Message("Line added successfully");
                        BindGridViewData(requisitionNo);
                        ddlType.SelectedValue = "0";
                        LoadAccounts();
                        return;
                    }
                    else if (returnMsg == "FAILED")
                    {
                        Message("An error occured while inserting the line. Please try again later.");
                        return;
                    }
                    else
                    {
                        Message(returnMsg);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
                return;
            }
        }

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string documentNo = lblLPurchaseNo.Text;
                string[] args = new string[2];
                args = (sender as LinkButton).CommandArgument.ToString().Split(';');
                string lineNo = args[0];
                if (webportals.RemovePurchaseLine(documentNo, Convert.ToInt32(lineNo)))
                {
                    Message("Line deleted successfully");
                    BindGridViewData(documentNo);
                    return;
                }
                else
                {
                    Message("An error occured while deleting the line");
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnApproval_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvLines.Rows.Count < 1)
                {
                    Message("Please add lines before sending for approval");
                    return;
                }
                string requisitionNo = lblLPurchaseNo.Text;
                string response = webportals.OnSendPurchaseRequisitionForApproval(requisitionNo);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        SuccessMessage("Purchase requisition has been sent for approval successfuly");
                    }
                    else
                    {
                        Message(response);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Message("ERROR: An error has occured while sending for approval");
                ex.Data.Clear();
            }
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                string requisitionNo = lblLPurchaseNo.Text;
                webportals.OnCancelPurchaseRequsiition(requisitionNo);
                SuccessMessage("Purchase requisition has been cancelled successfuly");
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void BindGridViewData(string requisitionNo)
        {
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetPurchaseLines",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@ReqNo", "'" + requisitionNo + "'");
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvLines.DataSource = dt;
                gvLines.DataBind();
                connection.Close();
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}