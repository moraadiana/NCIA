using NCIASTaff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
using NCIASTaff.NAVWS;

namespace NCIASTaff.pages
{
    public partial class MemoImprestLines1 : System.Web.UI.Page
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
              ///  LoadApprovedMemos();
              //  LoadResponsibilityCenters();

                if (Request.QueryString["ImprestNo"] != null)
                {
                    string imprestNo = Request.QueryString["ImprestNo"].ToString();
                    //ddlApprovedMemos.SelectedValue = imprestNo;
                }
                LoadImprestDetails();
                BindGridViewData();
                BindAttachedDocuments();
                LoadResponsibilityCenter();

                string status = lblStatus.Text;
                if (status == "Pending" || status == string.Empty)
                {
                    attachments.Visible = true;
                }
                else if (status == "Pending Approval")
                {
                    attachments.Visible = false;
                }
                else
                {
                    attachments.Visible = false;
                }
            }
        }

        
 
        private void BindGridViewData()
        {
            string imprestNo = Request.QueryString["ImprestNo"].ToString();
            string imprestReqLines = webportals.GetImprestLines(imprestNo);

            if (!string.IsNullOrEmpty(imprestReqLines))
            {
                // Assuming the data is separated by '[]' for each line and '::' for each field within a line.
                DataTable dt = new DataTable();
                dt.Columns.Add("No");
                dt.Columns.Add("Advance Type");
                dt.Columns.Add("Account No:");
                dt.Columns.Add("Account Name");
                dt.Columns.Add("Amount", typeof(decimal));

                string[] lines = imprestReqLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    string[] fields = line.Split(strLimiters, StringSplitOptions.None);
                    if (fields.Length == 5)  // Ensure all fields are present
                    {
                        DataRow row = dt.NewRow();
                        row["No"] = fields[0];
                        row["Advance Type"] = fields[1];
                        row["Account No:"] = fields[2];
                        row["Account Name"] = fields[3];
                        decimal amount;
                        if (decimal.TryParse(fields[4], out amount))
                        {
                            row["Amount"] = amount;
                        }
                        else
                        {
                            row["Amount"] = 0;  // Set to 0 if parsing fails, or handle as needed
                        }
                        dt.Rows.Add(row);
                    }
                }

                gvLines.DataSource = dt;
                gvLines.DataBind();

                // Calculate total amount and format each row
                decimal totalAmount = 0;
                
                foreach (DataRow row in dt.Rows)
                {
                    totalAmount += Convert.ToDecimal(row["Amount"]);
                }

                // Display total amount in lblTotalNetAmount
                lblTotalNetAmount.Text = String.Format("{0:#,##0.00}", totalAmount);
            }
            else
            {
                lblTotalNetAmount.Text = "0.00";
            }
        }

        private void BindAttachedDocuments()
        {
            try
            {
                string imprestNo = Request.QueryString["ImprestNo"];
                if (string.IsNullOrEmpty(imprestNo))
                {
                    Message("Imprest number is missing.");
                    return;
                }

                using (connection = Components.GetconnToNAV())
                {
                    using (command = new SqlCommand("spDocumentLines", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                        command.Parameters.AddWithValue("@DocNo", "'" + imprestNo + "'");

                        DataTable dt = new DataTable();
                        using (adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }

                        gvAttachments.DataSource = dt;
                        gvAttachments.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Message($"Error: {ex.Message}");
            }
        }


        private void BindAttachedDocuments1()
        {
            try
            {
                // string imprestNo = ddlApprovedMemos.SelectedValue.ToString();
                //string DocNo = Session["ImprestNo"].ToString();
                string imprestNo = Request.QueryString["ImprestNo"].ToString();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spDocumentLines",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@DocNo", "'" + imprestNo + "'");
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvAttachments.DataSource = dt;
                gvAttachments.DataBind();
                connection.Close();

                foreach (GridViewRow row in gvAttachments.Rows)
                {
                    row.Cells[3].Text = row.Cells[5].Text.Split(' ')[0];
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        //private void LoadApprovedMemos()
        //{
        //    try
        //    {
        //        string username = Session["username"].ToString();
        //        ddlApprovedMemos.Items.Clear();
        //        connection = Components.GetconnToNAV();
        //        command = new SqlCommand()
        //        {
        //            CommandText = "spGetMyPendingImprests",
        //            CommandType = CommandType.StoredProcedure,
        //            Connection = connection
        //        };
        //        command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
        //        command.Parameters.AddWithValue("@userID", username );
        //        reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                ListItem li = new ListItem(reader["No_"].ToString() + " => " + reader["Purpose"].ToString().ToUpper(), reader["No_"].ToString());
        //                ddlApprovedMemos.Items.Add(li);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Clear();
        //    }
        //}

        
        private void LoadResponsibilityCenter()
        {
            try
            {
                ddlResponsibilityCenter.Items.Clear();

                string grouping = "IMPREST";
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
        private void LoadImprestDetails()
        {
            try
            {
                imprestDetails.Visible = true;
                string imprestNo = Request.QueryString["ImprestNo"].ToString();
               // string imprestNo = ddlApprovedMemos.Text.Trim();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetImprestHeader",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@ReqNo", "'" + imprestNo + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string date = Convert.ToDateTime(reader["Date"]).ToShortDateString();
                    string payee = reader["Payee"].ToString();
                    string payingBankAccount = reader["Paying Bank Account"].ToString();
                    string directorate = reader["Global Dimension 1 Code"].ToString();
                    string department = reader["Shortcut Dimension 2 Code"].ToString();
                    string status = reader["MyStatus"].ToString();
                    string bankName = reader["Bank Name"].ToString();
                    string payMode = reader["PayMode"].ToString();
                    string purpose = reader["Purpose"].ToString();
                    string region = reader["Region"].ToString();
                    string accountNo = reader["Account No_"].ToString();
                    string mobileNo = reader["Mobile No"].ToString();
                    string jobTitle = reader["Job Title"].ToString();
                    string payeeBankCode = reader["payees bank code"].ToString();
                    string payeeBankName = reader["payees bank name"].ToString();
                    string payeeBankBranchCode = reader["payees Branch code"].ToString();
                    string payeeBankBranchName = reader["payees  branch name"].ToString();
                    string payeeBankaccount = reader["payees bank account"].ToString();
                    string paymentReleaseDate = Convert.ToDateTime(reader["Payment Release Date"]).ToShortDateString();
                    string chequeNo = reader["Cheque No_"].ToString();
                    string resCenter = reader["Responsibility Center"].ToString();

                    lblDate.Text = date;
                    lblDirectorate.Text = directorate;
                    lblDepartment.Text = department;
                    lblRegion.Text = region;
                    lblAccountNo.Text = accountNo;
                    lblPayee.Text = payee;
                    lblMobileNo.Text = mobileNo;
                    lblJobTitle.Text = jobTitle;
                    txtPurpose.Text = purpose;
                    lblStatus.Text = status;
                    lblPayeesBankCode.Text = payeeBankCode;
                    lblPayeesBankName.Text = payeeBankName;
                    lblPayeesBranchCode.Text = payeeBankBranchCode;
                    lblPayeeBranchName.Text = payeeBankBranchName;
                    lblPayeesBankAccount.Text = payeeBankaccount;
                    lblPaymentReleaseDate.Text = paymentReleaseDate;
                    lblPayMode.Text = payMode == "" ? "None" : payMode;
                    lblChequeNo.Text = chequeNo;
                    ddlResponsibilityCenter.Text = resCenter;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void ddlApprovedMemos_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadImprestDetails();
            BindGridViewData();
            BindAttachedDocuments();
        }

        protected void lbtnApprovalRequest_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvLines.Rows.Count < 1)
                {
                    Message("Please add imprest lines before sending for approval.");
                    return;
                }

                string username = Session["username"].ToString();
                string responsibilityCenter = ddlResponsibilityCenter.SelectedValue;
                string imprestNo = Request.QueryString["ImprestNo"].ToString();
                decimal totalAmount = Convert.ToDecimal(lblTotalNetAmount.Text);

                if (responsibilityCenter == "")
                {
                    Message("Responsibility center cannot be empty!");
                    return;
                }

                if (imprestNo == "")
                {
                    Message("Please select a memo!");
                    return;
                }

                string response = webportals.UpdateImprestHeader(imprestNo, responsibilityCenter, username);
                if (response != null)
                {
                    string returnMsg = response;
                    if (returnMsg == "SUCCESS")
                    {
                        string approvalRequest = webportals.ImprestRequisitionApprovalRequest(imprestNo, totalAmount);
                        if (approvalRequest != null)
                        {
                            if (approvalRequest == "SUCCESS")
                            {
                                SuccessMessage("Imprest requisition has been sent for approval successfully.");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
                ex.Data.Clear();
            }
        }
        protected void lbtnImprestAttach_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuImprestDocs.HasFile)
                {
                    string documentNo = Request.QueryString["ImprestNo"];
                    string username = Session["username"]?.ToString();

                    if (string.IsNullOrEmpty(documentNo) || string.IsNullOrEmpty(username))
                    {
                        Message("Imprest number or username is missing.");
                        return;
                    }

                    string fileName = Path.GetFileName(fuImprestDocs.FileName).Replace(" ", "-").ToUpper();
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    if (!new[] { ".pdf", ".jpg", ".png", ".jpeg" }.Contains(fileExtension))
                    {
                        Message("Please upload files with .pdf, .png, .jpg, or .jpeg extensions only!");
                        return;
                    }

                    string uploadPath = Server.MapPath("~/Uploads");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string filePath = Path.Combine(uploadPath, documentNo.Replace("/", "-") + fileName);

                    // Save the file locally
                    fuImprestDocs.SaveAs(filePath);

                    // Register the file in the system (base64 is optional based on implementation)
                    using (Stream fs = fuImprestDocs.PostedFile.InputStream)
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] fileBytes = br.ReadBytes((int)fs.Length);
                            string base64String = Convert.ToBase64String(fileBytes);

                            // Update the file registration in the backend
                            Components.ObjNav.RegFileUploadAtt(documentNo, fileName, base64String, 52178708, "Imprest Requisition");
                        }
                    }

                    // Refresh the grid view
                    BindAttachedDocuments();
                    Message("Document uploaded successfully!");
                }
                else
                {
                    Message("Please select a file to upload!");
                }
            }
            catch (Exception ex)
            {
                Message($"Error: {ex.Message}");
            }
        }

        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
        private void SuccessMessage(string message)
        {
            string page = "MemoImprestListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "';</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        protected void lbtnRemoveAttach_Click(object sender, EventArgs e)
        {
            try
            {
                string status = lblStatus.Text;
                if (status == "Open" || status == "Pending")
                {
                    string[] args = new string[2];
                    args = (sender as LinkButton).CommandArgument.ToString().Split(';');
                    string systemId = args[0];
                    if (Components.ObjNav.DeleteDocumentAttachments(systemId))
                    {
                        Message("Document deleted successfully!");
                        BindAttachedDocuments();

                    }
                    else
                    {
                        Message("An error occured while deleting document. Please try again later!");
                        return;
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

        protected void lbtnRemoveAttach_Click1(object sender, EventArgs e)
        {
            try
            {
                string status = lblStatus.Text;
                if (status == "Open" || status == "Pending")
                {
                    string[] args = new string[2];
                    args = (sender as LinkButton).CommandArgument.ToString().Split(';');
                    string systemId = args[0];
                    string documentNo = Request.QueryString["ImprestNo"].ToString();
                    string fileName = string.Empty;
                    string documentDetails = webportals.GetAttachmentDetails(systemId);
                    if (documentDetails != null)
                    {
                        string[] documentsDetailsArr = documentDetails.Split(strLimiters, StringSplitOptions.None);
                        fileName = documentsDetailsArr[1].Split('.')[0];
                    }

                    string response = webportals.DeleteDocumentAttachment(systemId, fileName, documentNo);
                    if (response != null)
                    {
                        string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                        string returnMsg1 = responseArr[0];
                        string returnMsg2 = responseArr[1];
                        if (returnMsg1 == "SUCCESS" && returnMsg2 == "SUCCESS")
                        {
                            Message("Document deleted successfully.");
                            BindAttachedDocuments();
                        }
                        else
                        {
                            Message("An error has occured. Please try again later.");
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
    }
}