using NCIASTaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class PettyCashLines_Reimbursement_ : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter adapter;
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

               // LoadStaffDetails();
                LoadResponsibilityCenter();
                LoadAccountNos();
                LoadPostedPettyCashLines();
                LoadDepartments();
                LoadUnits();
                LoadAdvanceTypes();


                string query = Request.QueryString["query"];
                string approvalStatus = Request.QueryString["status"].Replace("%", " ");
                if (query == "new")
                {
                    MultiView1.SetActiveView(vwHeader);

                }
                else if (query == "old")
                {
                    string pettyCashReNo = Request.QueryString["PettyCashReNo"];
                    MultiView1.SetActiveView(vwLines);
                    lblLNo.Text = pettyCashReNo;
                    lblPettyCashReNo.Text = pettyCashReNo;
                    LoadAdvanceTypes();
                    BindGridViewData(pettyCashReNo);
                    BindAttachedDocuments(pettyCashReNo);
                    // LoadAccountNos();
                }

                if (approvalStatus == "Open" || approvalStatus == "Pending" || approvalStatus == "New")
                {
                    btnApproval.Visible = true;
                    btnCancellApproval.Visible = false;
                    attachments.Visible = true;
                    //lbtnAddLine.Visible = true;
                    //lbtnClose.Visible = true;
                }
                else if (approvalStatus == "Pending Approval")
                {
                    btnApproval.Visible = false;
                    btnCancellApproval.Visible = true;
                    lbtnAddLine.Visible = false;
                    lbtnClose.Visible = false;
                    attachments.Visible = false;
                }
                else
                {
                    btnApproval.Visible = false;
                    btnCancellApproval.Visible = false;
                    lbtnAddLine.Visible = false;
                    lbtnClose.Visible = false;
                    attachments.Visible = false;
                }
            }
        }
        private void LoadDepartments()
        {

            try
            {
                ddlDepartment.Items.Clear();
                string code = "DEPARTMENT";
                string departments = webportals.GetDimensions(code);
                if (!string.IsNullOrEmpty(departments))
                {
                    string[] departmentsArr = departments.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string departmant in departmentsArr)
                    {
                        string[] responseArr = departmant.Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length >= 2)
                        {
                            string displayText = $"{responseArr[0]} - {responseArr[1]}"; 
                            string value = responseArr[0];
                            ListItem li = new ListItem(displayText, value);
                            ddlDepartment.Items.Add(li);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

        }
        private void LoadUnits()
        {

            try
            {
                ddlUnit.Items.Clear();
                string code = "UNITS";
                string units = webportals.GetDimensions(code);
                if (!string.IsNullOrEmpty(units))
                {
                    string[] unitsArr = units.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string unit in unitsArr)
                    {
                        string[] responseArr = unit.Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length >= 2)
                        {
                            string displayText = $"{responseArr[0]} - {responseArr[1]}";
                            string value = responseArr[0];
                            ListItem li = new ListItem(displayText, value);
                            ddlUnit.Items.Add(li);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

        }
        private void LoadPostedPettyCashLines()
        {
            try
            {
                ddlPostedPettyCash.Items.Clear();
                string staffNo = Session["username"].ToString();
                lblStaffNo.Text = staffNo;
                string staffName = Session["StaffName"].ToString();
                //string AccountNo = lblAccountNo.ToString();
                string staffAccountNo = webportals.GetStaffPettyCashNo(staffNo);
                if (!string.IsNullOrEmpty(staffAccountNo))
                {
                    lblAccountNo.Text = staffAccountNo;
                }
               
                string PostedPettyCash = webportals.GetPostedPettyCash(staffAccountNo);
                if (!string.IsNullOrEmpty(PostedPettyCash))
                {
                    string[] PostedPettyCashArr = PostedPettyCash.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string pettyCash in PostedPettyCashArr)
                    {
                        ddlPostedPettyCash.Items.Add(new ListItem(pettyCash));
                    }
                }
                else
                {
                    ddlPostedPettyCash.Items.Add(new ListItem("No posted petty cash available"));
                }

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

                string grouping = "REIMBURSE";
                string resCenters = webportals.GetResponsibilityCentres(grouping);
                if (!string.IsNullOrEmpty(resCenters))
                {
                    string[] resCenterArr = resCenters.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string rescenter in resCenterArr)
                    {
                        ddlResponsibilityCenter.Items.Add(new ListItem(rescenter));
                    }
                }
                else
                {
                    ddlResponsibilityCenter.Items.Add(new ListItem("No responsibility centers available"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ddlResponsibilityCenter.Items.Add(new ListItem("Error loading responsibility centers"));
            }
        }
        private void LoadAccountNos()
        {

            try
            {
                ddlVote.Items.Clear();
                string AccountNo = webportals.GetGLAccounts();
                if (!string.IsNullOrEmpty(AccountNo))
                {
                    string[] AccountNoArr = AccountNo.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string account in AccountNoArr)
                    {
                        string[] responseArr = account.Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length >= 2)
                        {
                            //ListItem li = new ListItem(responseArr[1], responseArr[0]);
                            //ddlAccountNo.Items.Add(li);
                            string displayText = $"{responseArr[0]} - {responseArr[1]}"; // Display as "No - Name"
                            string value = responseArr[0]; // Only the account number is stored
                            ListItem li = new ListItem(displayText, value);
                            ddlVote.Items.Add(li);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

        }
        private void LoadAdvanceTypes()
        {
            try
            {
                ddlAdvancType.Items.Clear();

                string advanceTypes = webportals.GetAdvancetype(6);
                if (!string.IsNullOrEmpty(advanceTypes))
                {
                    string[] advanceTypesArr = advanceTypes.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string advanceType in advanceTypesArr)
                    {
                        //ddlAdvancType.Items.Add(new ListItem(advanceType));
                        string[] responseArr = advanceType.Split(strLimiters, StringSplitOptions.None);
                        ListItem li = new ListItem(responseArr[1], responseArr[0]);
                        ddlAdvancType.Items.Add(li);
                    }
                }
                else
                {
                    ddlAdvancType.Items.Add(new ListItem("No Advance type available"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ddlAdvancType.Items.Add(new ListItem("Error loading advcance types centers"));
            }
        }
       

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["username"].ToString();
                string pettyCashNo = ddlPostedPettyCash.SelectedValue;
                string responsibilityCenter = ddlResponsibilityCenter.SelectedValue;
                string AccountNo = lblAccountNo.Text;
               

                if (string.IsNullOrEmpty(AccountNo))
                {
                    Message("AccountNo cannot be null. Contact Administrator to set it");
                    return;
                }
                
                if (string.IsNullOrEmpty(responsibilityCenter))
                {
                    Message("Responsibility center cannot be null!");
                    return;
                }
               

                string response = webportals.CreatePettyCashReimbursementHeader(username, pettyCashNo, responsibilityCenter);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string pettyCashReNo = responseArr[1];
                        Message($"Petty cash (Reimburement) number {pettyCashReNo} has been created successfully!");
                        Session["PettyCashReNo"] = pettyCashReNo;
                        BindGridViewData(pettyCashReNo);
                        NewView();
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
                string pettyCashReNo = Session["PettyCashReNo"].ToString();
                newLines.Visible = true;
                lbtnAddLine.Visible = false;
                lblLNo.Text = pettyCashReNo;
                lblPettyCashReNo.Text = pettyCashReNo;
                LoadAdvanceTypes();
                BindGridViewData(pettyCashReNo);
                BindAttachedDocuments(pettyCashReNo);

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        private void BindAttachedDocuments(string documentNo)
        {
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spDocumentLines",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@DocNo", "'" + documentNo + "'");
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvAttachments.DataSource = dt;
                gvAttachments.DataBind();
                connection.Close();

                foreach (GridViewRow row in gvAttachments.Rows)
                {
                    row.Cells[3].Text = row.Cells[3].Text.Split(' ')[0];
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }


        private void BindGridViewData(string pettyCashReNo)
        {
            string pettyCashLines = webportals.GetPettyCashReimbursementLines(pettyCashReNo);

            if (!string.IsNullOrEmpty(pettyCashLines))
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("Document No_");
                dt.Columns.Add("Advance Type");
                dt.Columns.Add("Vote");
                dt.Columns.Add("Department");
                dt.Columns.Add("Unit");
                dt.Columns.Add("Purpose");
                dt.Columns.Add("Amount");


                string[] lines = pettyCashLines.Split(new[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);


                foreach (string line in lines)
                {
                    string[] fields = line.Split(new[] { "::" }, StringSplitOptions.None);
                    if (fields.Length == 7)
                    {
                        DataRow row = dt.NewRow();
                        row["Document No_"] = fields[0];
                        row["Advance Type"] = fields[1];
                        row["Vote"] = fields[2];
                        row["Department"] = fields[3];
                        row["Unit"] = fields[4];
                        row["Amount"] = fields[5];
                        row["Purpose"] = fields[6];
                        dt.Rows.Add(row);
                    }
                }

                gvLines.DataSource = dt;
                gvLines.DataBind();


            }

        }


        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        private void SuccessMessage(string message)
        {
            string page = "PettyCashListing(Reimbursement).aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "'</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        protected void btnLine_Click(object sender, EventArgs e)
        {
            try
            {
                string pettyCashReNo = lblPettyCashReNo.Text;
                string username = Session["username"].ToString();
                string advanceType = ddlAdvancType.SelectedValue;
                string amount = txtAmnt.Text;
                string purpose =txtPurpose.Text;
                string voteNo = ddlVote.SelectedValue;
                string department = ddlDepartment.SelectedValue;
                string unit = ddlUnit.SelectedValue;
                if (string.IsNullOrEmpty(purpose))

                {
                    Message("Purpose cannot be null!");
                    return;
                }
                if (string.IsNullOrEmpty(advanceType))
                {
                    Message("Advance type cannot be null!");
                    return;
                }
                if (string.IsNullOrEmpty(voteNo))
                {
                    Message("Account Number cannot be null!");
                    return;
                }
                if (string.IsNullOrEmpty(amount))
                {
                    Message("Amount cannot be empty!");
                    txtAmnt.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(department))
                {
                    Message("Department cannot be empty!");
                    txtAmnt.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(unit))
                {
                    Message("Unit cannot be empty!");
                    txtAmnt.Focus();
                    return;
                }

                string response = webportals.InsertPettyCashReimbursementLine(pettyCashReNo, voteNo, advanceType,Convert.ToDecimal(amount), purpose, unit, department);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] strLimiters = new string[] { "::" };
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        Message("Line added successfully!");
                        txtAmnt.Text = string.Empty;
                        BindGridViewData(pettyCashReNo);
                    }
                    if (returnMsg == "FAILED")
                    {
                        Message($"Error! failed to add lines! ,record with vote:{voteNo} already exists ");
                        
                        BindGridViewData(pettyCashReNo);
                    }
                }
               
            }
            catch (Exception ex)
            {
                Message("An error occurred: " + ex.Message);
               // ex.Data.Clear();
            }
        }

        protected void lbtnAddLine_Click(object sender, EventArgs e)
        {
            string pettyCashReNo = string.Empty;
            if (Request.QueryString["PettyCashReNo"] == null)
            {
                pettyCashReNo = Session["PettyCashReNo"].ToString();
            }
            else
            {
                pettyCashReNo = Request.QueryString["PettyCashReNo"].ToString();
            }
            lblLNo.Text = pettyCashReNo;
            LoadAdvanceTypes();
            newLines.Visible = true;
            lbtnAddLine.Visible = false;
        }

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string status = Request.QueryString["status"].ToString().Replace("%", " ");
                if (status == "Open" || status == "Pending" || status == "New")
                {
                    string[] args = new string[2];
                    string pettyCashReNo = lblPettyCashReNo.Text;
                    args = (sender as LinkButton).CommandArgument.ToString().Split(';');
                    ///string advanceType = args[0];
                    string voteNo = args[0];
                    string response = webportals.RemovePettyCashReimbursementLine(pettyCashReNo, voteNo);
                    if (!string.IsNullOrEmpty(response))
                    {
                        if (response == "SUCCESS")
                        {
                            Message("Line deleted successfully!");
                            BindGridViewData(pettyCashReNo);
                        }
                        else
                        {
                            Message("An error occured while removing line. Please try again later");
                            return;
                        }
                    }
                }
                else
                {
                    Message("You can only edit an open document!");
                    return;
                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            try
            {
                string pettyCashReNo = lblPettyCashReNo.Text;
                if (gvLines.Rows.Count < 1)
                {
                    Message("Please add lines before sending for approval!");
                    return;
                }
                if (gvAttachments.Rows.Count < 1)
                {
                    Message("Please attach documents before sending for approval!");
                    return;
                }
                string msg = webportals.OnSendPettyCashReimbursementForApproval1(pettyCashReNo);
                if (msg == "SUCCESS")
                {
                    
                    SuccessMessage($"PettyCash (Reimbursement) number {pettyCashReNo} has been sent for approval successfuly!");
                }
                else
                {
                    Message("ERROR: " + msg);
                }
            }
            catch (Exception ex)
            {
                Message("An error occurred: " + ex.Message);
               // ex.Data.Clear();
            }
        }

        protected void btnCancellApproval_Click(object sender, EventArgs e)
        {
            try
            {
                string pettyCashReNo = lblPettyCashReNo.Text;
                webportals.OnCancelPettyCashReimbursement(pettyCashReNo);
                SuccessMessage($"Petty cash (Reimbursement) {pettyCashReNo} has been cancelled successfuly!");
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
                ex.Data.Clear();
            }
        }

        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            newLines.Visible = false;
            lbtnAddLine.Visible = true;
        }

        protected void lbtnAttach_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuClaimDocs.PostedFile != null)
                {
                    string DocumentNo = lblLNo.Text;
                    string username = Session["username"].ToString();
                    string filePath = fuClaimDocs.PostedFile.FileName.Replace(" ", "-");
                    string fileName = fuClaimDocs.FileName.Replace(" ", "-");
                    string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    if (fileExtension == "pdf" || fileExtension == "jpg" || fileExtension == "png" || fileExtension == "jpeg")
                    {
                        string strPath = Server.MapPath("~/Uploads");
                        if (!Directory.Exists(strPath))
                        {
                            Directory.CreateDirectory(strPath);
                        }

                        string pathToUpload = Path.Combine(strPath, DocumentNo + fileName.ToUpper());
                        if (File.Exists(pathToUpload))
                        {
                            File.Delete(pathToUpload);
                        }
                        fuClaimDocs.SaveAs(pathToUpload);
                        webportals.SaveMemoAttchmnts(DocumentNo, pathToUpload, fileName.ToUpper(), username);
                        Stream fs = fuClaimDocs.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((int)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        webportals.RegFileUploadAtt(DocumentNo, fileName.ToUpper(), base64String, 52178447, "Petty Cash Reimbursement");
                        BindAttachedDocuments(DocumentNo);
                        Message("Document uploaded successfully!");
                    }
                    else
                    {
                        Message("Please upload files with .pdf, .png, .jpg and .jpeg extensions only!");
                        return;
                    }
                }
                else
                {
                    Message("Please upload a file!");
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnRemoveAttach_Click(object sender, EventArgs e)
        {
            try
            {
                //string documentNo = Session["PettyCashNo"].ToString();
                string documentNo = Request.QueryString["pettyCashReNo"];
                // string documentNo = lblLNo.Text;
                string[] args = new string[2];
                args = (sender as LinkButton).CommandArgument.ToString().Split(';');
                string systemId = args[0];
                if (Components.ObjNav.DeleteDocumentAttachments(systemId))
                {
                    Message("Document deleted successfully!");
                    BindAttachedDocuments(documentNo);

                }
                else
                {
                    Message("An error occured while deleting document. Please try again later!");
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

    }
}