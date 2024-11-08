﻿using NCIASTaff;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
                LoadApprovedMemos();
                LoadResponsibilityCenters();

                if (Request.QueryString["ImprestNo"] != null)
                {
                    string imprestNo = Request.QueryString["ImprestNo"].ToString();
                    ddlApprovedMemos.SelectedValue = imprestNo;
                }
                LoadImprestDetails();
                BindGridViewData();
                BindAttachedDocuments();

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
            try
            {
                string username = Session["username"].ToString();
                string imprestNo = ddlApprovedMemos.SelectedValue.ToString();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spMemoImprestLines",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@ImprestNo", "'" + imprestNo + "'");
                command.Parameters.AddWithValue("@username", "'" + username + "'");
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvLines.DataSource = dt;
                gvLines.DataBind();
                connection.Close();
                decimal totalAmount = 0;

                foreach (GridViewRow row in gvLines.Rows)
                {
                    totalAmount += Convert.ToDecimal(row.Cells[5].Text);
                    lblTotalNetAmount.Text = String.Format("{0:#,##0.00}", totalAmount);
                    row.Cells[5].Text = String.Format("{0:#,##0.00}", Convert.ToDecimal(row.Cells[5].Text));
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void BindAttachedDocuments()
        {
            try
            {
                string imprestNo = ddlApprovedMemos.SelectedValue.ToString();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spDocumentLines",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@ReqNo", "'" + imprestNo + "'");
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

        private void LoadApprovedMemos()
        {
            try
            {
                string username = Session["username"].ToString();
                ddlApprovedMemos.Items.Clear();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetMyPendingImprests",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@userID", username );
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListItem li = new ListItem(reader["No_"].ToString() + " => " + reader["Purpose"].ToString().ToUpper(), reader["No_"].ToString());
                        ddlApprovedMemos.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadResponsibilityCenters()
        {
            try
            {
                ddlResponsibilityCenter.Items.Clear();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetImprestReponsibilityCentre",
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
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadImprestDetails()
        {
            try
            {
                imprestDetails.Visible = true;
                string imprestNo = ddlApprovedMemos.Text.Trim();
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
                //string userId = MyComponents.UserID;
                string responsibilityCenter = ddlResponsibilityCenter.SelectedValue.ToString();
                string imprestNo = ddlApprovedMemos.SelectedValue.ToString();
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
                if (fuImprestDocs.PostedFile != null)
                {
                    string DocumentNo = ddlApprovedMemos.SelectedValue.ToString();
                    string username = Session["username"].ToString();
                    string filePath = fuImprestDocs.PostedFile.FileName.Replace(" ", "-");
                    string fileName = fuImprestDocs.FileName.Replace(" ", "-");
                    string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    if (fileExtension == "pdf" || fileExtension == "jpg" || fileExtension == "png" || fileExtension == "jpeg")
                    {
                        string strPath = Server.MapPath("~/Uploads");
                        if (!Directory.Exists(strPath))
                        {
                            Directory.CreateDirectory(strPath);
                        }

                        string pathToUpload = Path.Combine(strPath, DocumentNo.Replace("/", "-") + fileName.ToUpper());
                        if (File.Exists(pathToUpload))
                        {
                            File.Delete(pathToUpload);
                        }
                        fuImprestDocs.SaveAs(pathToUpload);
                        Components.ObjNav.SaveMemoAttchmnts(DocumentNo, pathToUpload, fileName.ToUpper(), username);
                        Stream fs = fuImprestDocs.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((int)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        Components.ObjNav.RegFileUploadAtt(DocumentNo, fileName.ToUpper(), base64String, 52178708, "Imprest Requisition");
                        BindAttachedDocuments();
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
                    string documentNo = ddlApprovedMemos.SelectedValue.ToString();
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