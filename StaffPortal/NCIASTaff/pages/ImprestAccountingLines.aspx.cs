﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCIASTaff.NAVWS;

namespace NCIASTaff.pages
{
    public partial class ImprestAccountingLines : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter adapter;
        private Staffportall webportals = Components.ObjNav;
        readonly string[] strLimiters = new string[] { "::" };
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
                //GetPostedReceipts();
                //BindGridView();
                string query = Request.QueryString["query"].ToString();
                string documentNo = string.Empty;
                if(query == "new")
                {
                    documentNo = webportals.GetNextImprestSurrenderNo();
                    LoadPostedImprests();
                }
                else
                {
                    documentNo = Request.QueryString["SurrenderNo"].ToString();
                    string imprestNo = Request.QueryString["ImprestNo"].ToString();
                    string response = webportals.GetImprestDetails(imprestNo);
                    if (!string.IsNullOrEmpty(response))
                    {
                        ddlPostedImprest.Items.Clear();
                        string[] responseArr = response.Split(strLimiters,StringSplitOptions.None);
                        string imprestNumber = responseArr[0];
                        string imprestDescription = responseArr[1];
                        string responsibilityCenter = responseArr[2];
                        ListItem li = new ListItem(imprestNumber + " => " + imprestDescription, imprestNumber);
                        ddlPostedImprest.Items.Add(li);
                        ddlResponsibilityCenter.SelectedValue = responsibilityCenter;
                    }                    
                }

                Session["DocumentNo"] = documentNo;
                BindAttachedDocuments(documentNo);
                BindGridViewData();
                LoadResponsibilityCenter();
                //GetPostedReceipts();
            }
        }

        private void LoadPostedImprests()
        {
            try
            {
                //GetPostedReceipts();
                string username = Session["username"].ToString();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spImprestToSurrender",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@userID", "'" + username + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListItem li = new ListItem(reader["No_"].ToString() + " => " + reader["Purpose"].ToString(), reader["No_"].ToString());
                        ddlPostedImprest.Items.Add(li);
                    }
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
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetImprestSurrReponsibilityCentre",
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
      
        private void BindGridViewData()
        {
            try
            {
                string imprestNo = ddlPostedImprest.SelectedValue.ToString();
                connection =    Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spImprestLines",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@ImpNo", "'" + imprestNo + "'");
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvLines.DataSource = dt;
                gvLines.DataBind();
                connection.Close();

                foreach (GridViewRow row in gvLines.Rows)
                {
                    string account = row.Cells[2].Text;
                    string surrenderNo = Session["DocumentNo"].ToString();
                    DropDownList ddlReceipts = row.FindControl("ddlReceipts") as DropDownList;

                    string receipts = webportals.GetReceipts();
                    if (!string.IsNullOrEmpty(receipts))
                    {
                        ListItem li = new ListItem("--Select Receipts--","");
                        ddlReceipts.Items.Add(li);
                        string[] receiptsArr = receipts.Split(new string[] {"[]"}, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string receipt in receiptsArr)
                        {
                            string[] responseArr = receipt.Split(strLimiters, StringSplitOptions.None);
                            li = new ListItem(responseArr[0], responseArr[0]);
                            ddlReceipts.Items.Add(li);
                        }
                    }

                    //if (txtActualAmount.Text == "") txtActualAmount.Text = "0";
                    //if (txtAmountReturned.Text == "") txtAmountReturned.Text = "0";

                    string response = webportals.LoadImprestSurrenderLineDetails(surrenderNo, account);
                    if (!string.IsNullOrEmpty(response))
                    {
                        string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                        string returnMsg = responseArr[0];
                        if (returnMsg == "SUCCESS")
                        {
                            ListItem li = new ListItem(responseArr[0]);
                            //ddlReceipts.Items.Add(li);
                        }
                        else
                        {
                            ListItem li = new ListItem("--Select receipts--","");
                            ddlReceipts.Items.Add(li);
                        }
                    }
                }
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
                command.Parameters.AddWithValue("@ReqNo", "'" + documentNo + "'");
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

        protected void ddlPostedImprest_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindGridViewData();
        }

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string imprestNo = ddlPostedImprest.SelectedValue.ToString();
                string responsibilityCenter = ddlResponsibilityCenter.SelectedValue.ToString();
                string username = Session["username"].ToString();
                string documentNo = string.Empty;

                if (imprestNo == "")
                {
                    Message("Posted imprest no cannot be empty!");
                    return;
                }
                if (responsibilityCenter == "")
                {
                    Message("Responsibility center cannot be empty!");
                    return;
                }

                if (gvAttachments.Rows.Count < 1)
                {
                    Message("Please upload supporting documents.");
                    return;
                }

                string imprestSurrenderNo = Session["DocumentNo"].ToString();

                string response = webportals.CreateImprestSurrenderHeader(imprestSurrenderNo, imprestNo, responsibilityCenter);
                if (response != null)
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if(returnMsg == "SUCCESS")
                    {
                        documentNo = responseArr[1];
                    }
                    else
                    {
                        Message("An error occured while surendering imprest. Please try again later");
                        return;
                    }
                }

                foreach (GridViewRow row in gvLines.Rows)
                {
                   DropDownList ddlReceipts = row.FindControl("ddlReceipts") as DropDownList;
                    string receiptNo = ddlReceipts.SelectedValue;
                    decimal amount = Convert.ToDecimal(row.Cells[5].Text);
                    string accountNo = row.Cells[2].Text;
                    webportals.InsertImprestSurrenderLines(documentNo, imprestNo, accountNo, receiptNo);
                }

                string approvalResponse = webportals.OnSendImprestSurrenderForApproval(documentNo);
                if (approvalResponse == "SUCCESS")
                {
                    SuccessMessage("Imprest Surrender has been submitted successfully!");
                }
                else
                {
                    Message(approvalResponse);
                    return;
                }
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
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
            string page = "ImprestAccountingListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "';</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        protected void lbtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuImprestDocs.PostedFile != null)
                {
                    string username = Session["username"].ToString();
                    string filePath = fuImprestDocs.PostedFile.FileName.Replace(" ", "-");
                    string fileName = fuImprestDocs.FileName.Replace(" ", "-");
                    string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    string DocumentNo = Session["DocumentNo"].ToString();
                    if (fileExtension == "pdf" || fileExtension == "jpg" || fileExtension == "png" || fileExtension == "jpeg" || fileExtension == "docx" || fileExtension == "doc")
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
                        fuImprestDocs.SaveAs(pathToUpload);
                        Components.ObjNav.SaveMemoAttchmnts(DocumentNo, pathToUpload, fileName.ToUpper(), username);
                        Stream fs = fuImprestDocs.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((int)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        Components.ObjNav.RegFileUploadAtt(DocumentNo, fileName.ToUpper(), base64String, 52178705, "Imprest Surrender");
                        BindAttachedDocuments(DocumentNo);
                        Message("Document uploaded successfully");
                    }
                    else
                    {
                        Message("Please upload files with .pdf, .png, .jpg and .jpeg extensions only!");
                    }
                }
                else
                {
                    Message("Please upload a file!");
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
                string[] args = new string[2];
                args = (sender as LinkButton).CommandArgument.ToString().Split(';');
                string systemId = args[0];
                string documentNo = Session["DocumentNo"].ToString();
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
                        BindAttachedDocuments(documentNo);
                    }
                    else
                    {
                        Message("An error has occured. Please try again later.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void ddlReceipts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow row = (GridViewRow)(sender as DropDownList).NamingContainer;
            //string rNo = ((DropDownList)row.FindControl("ddlReceipts")).SelectedItem.Value;
            //if (rNo != "Please select")
            //{
            //    DataSet ds = GetMyData(rNo);
            //    //string country = (e.Row.FindControl("ddlReceipts") as Label).Text;
            //    //ddlRecpts.Items.FindByValue(country).Selected = true;
            //    row.Cells[7].Text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            //}
        }
        private DataSet GetMyData(string Receipts)
        {
            using (SqlConnection con = Components.GetconnToNAV())
            {
                string stst = "spGetMyReceiptsAmount";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = stst;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                cmd.Parameters.AddWithValue("@ReceiptNo", "'" + Receipts + "'");

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }

      
        private DataTable GetGridViewData()
        {
            
            DataTable dt = new DataTable();
       
            try
            {
                

                string username = Session["username"].ToString();
               
                using (SqlConnection connection = Components.GetconnToNAV())
                {
                    
                    
                    using (SqlCommand command = new SqlCommand("spGetMyReceipts", connection))

                    {
                        
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                        command.Parameters.AddWithValue("@Username", "'" + username + "'");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                           
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors (optional logging)
                Console.WriteLine("Error: " + ex.Message);
            }
            return dt;
        }

        protected void gvLines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                DropDownList ddlRecpts = (e.Row.FindControl("ddlReceipts") as DropDownList);
                ddlRecpts.DataSource = GetData();
                ddlRecpts.DataTextField = "No";
                ddlRecpts.DataValueField = "No";
                ddlRecpts.DataBind();

                
                ddlRecpts.Items.Insert(0, new ListItem("Please select"));

                //Select the Country of Customer in DropDownList.
                //string country = (e.Row.FindControl("ddlReceipts") as Label).Text;
                //ddlRecpts.Items.FindByValue(country).Selected = true;
            }
        }
        private DataSet GetData()
        {
            string username = Session["username"].ToString();
            
            using (SqlConnection con = Components.GetconnToNAV())
            {
                string stst = "spGetMyReceipts";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = stst;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                cmd.Parameters.AddWithValue("@Username", "'" + username + "'");

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }

    }
}