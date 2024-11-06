using NCIASTaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Services.Description;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class StoreLines : System.Web.UI.Page
    {
        private readonly Staffportall webportals = Components.ObjNav;
        readonly string[] strLimiters = new string[] { "::" };
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
                LoadIssuingStore();

                string query = Request.QueryString["query"].ToString();
                if (query == "old")
                {
                    string approvalStatus = Request.QueryString["status"].Replace("%", " ");
                    string requisitionNo = Request.QueryString["ReqNo"].ToString();
                    lblLStoreNo.Text = requisitionNo;
                    lblStoreNo.Text = requisitionNo;
                    MultiView1.SetActiveView(vwLines);
                    BindGridViewData(requisitionNo);
                    LoadItems();
                    GetItemDetails();
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

        private void LoadIssuingStore()
        {
            try
            {
                ddlissuingStore.Items.Clear();
                ddlIssuingStoreLines.Items.Clear();
                using (SqlConnection conn = Components.GetconnToNAV())
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "spGetStores",
                        CommandType = CommandType.StoredProcedure,
                        Connection = conn
                    };
                    cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ListItem li = new ListItem(reader["Name"].ToString(), reader["Code"].ToString());
                                ddlissuingStore.Items.Add(li);
                                ddlIssuingStoreLines.Items.Add(li);
                            }
                        }
                    }

                }
            }
            catch(Exception ex) { 
                ex.Data.Clear();
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
                        lblTitle.Text = departmentDetailsArr[3];
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
                    CommandText = "spGetStoresResponsilityCenter",
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
     

        private void LoadFixedassets()
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

        private void GetItemDetails()
        {
            try
            {
                string item = ddlItem.SelectedValue;
                string response = webportals.GetStoreItemQuantity(item);
                if (!string.IsNullOrEmpty(response))
                {
                    txtQtyInStore.Text = response;
                }
                else
                {
                    txtQtyInStore.Text = "0";
                }
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
                    CommandText = "spStoresReqLines",
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

        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        private void SuccessMessage(string message)
        {
            string page = "StoreListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "';</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        private void NewView()
        {
            string requisitionNo = Session["RequisitionNo"].ToString();
            lblLStoreNo.Text = requisitionNo;
            lblStoreNo.Text = requisitionNo;
            newLines.Visible = true;
            lbtnCloseLines.Visible = true;
            lbtnAddLines.Visible = false;
            lbtnApproval.Visible = true;
            lbtnCancel.Visible = false;
            LoadItems();
            GetItemDetails();
            BindGridViewData(requisitionNo);
            MultiView1.SetActiveView(vwLines);
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
                string requisitionType = ddlRequisitionType.SelectedValue.ToString();
                string issuingStore = ddlissuingStore.SelectedValue.ToString();

               /* if (string.IsNullOrEmpty(directorate) || string.IsNullOrEmpty(department))
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
                if (string.IsNullOrEmpty(issuingStore))
                {
                    Message("Issuing store cannot be empty.");
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

                //string response = webportals.CreateStoreRequisitionHeader(username, Convert.ToInt32(requisitionType), Convert.ToDateTime(requiredDate),directorate, department, responsibilityCenter, issuingStore, description);
                var response = webportals.CreateStoreRequisitionHeader(username, Convert.ToDateTime(requiredDate), description, department, directorate, Convert.ToInt32(requisitionType), issuingStore, responsibilityCenter);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string storeNo = responseArr[1];
                        Session["RequisitionNo"] = storeNo;
                        Message($"Store requisition with number {storeNo} has been created successfully.");
                        NewView();
                    }
                    else
                    {
                        Message("An error occured while creating the store requisition. Please try again later.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string type = ddlType.SelectedValue.ToString();
                if (type == "1") LoadItems();
                if (type == "2") LoadFixedassets();
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlType.SelectedValue == "2") return;
                GetItemDetails();
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
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

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string storeNo = lblStoreNo.Text;
                string type = ddlType.SelectedValue;
                string item = ddlItem.SelectedValue;
                string qtyInStore = txtQtyInStore.Text;
                string quantity = txtQuantity.Text;
                string issuingStore = ddlIssuingStoreLines.SelectedValue;

                if (type == "1")
                {
                    if(string.IsNullOrEmpty(quantity))
                    {
                        Message("Quantity requested cannot be null or empty");
                        txtQuantity.Focus();
                        return;
                    }
                    if(Convert.ToDouble(qtyInStore) < 1)
                    {
                        Message("Item is out of stock");
                        return;
                    }
                    if(Convert.ToDouble(quantity) > Convert.ToDouble(qtyInStore))
                    {
                        Message("Quantity requested cannot be more than the quantity in store");
                        return;
                    }
                }

                string response = webportals.InsertStoreLines(storeNo, Convert.ToInt32(type), item, Convert.ToDecimal(quantity), issuingStore);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        Message("Line added successfully");
                        BindGridViewData(storeNo);
                    }
                    else
                    {
                        Message("An error occured while adding the line. Please try again later.");
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnApproval_Click(object sender, EventArgs e)
        {
            try
            {
                if(gvLines.Rows.Count < 1)
                {
                    Message("Please add line before senting for approval");
                    return;
                }
                string storeNo = lblLStoreNo.Text;
                string response = webportals.OnSendStoreRequisitionForApproval(storeNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        SuccessMessage("Store requisition has been sent for approval successfully.");
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
                ex.Data.Clear();
            }
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                string storeNo = lblLStoreNo.Text;
                webportals.OnCancelStoreRequisitionApproval(storeNo);
                SuccessMessage("Store requisition has been cancelled successfully.");
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}