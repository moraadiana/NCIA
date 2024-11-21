
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace NCIASTaff.pages

{
    public partial class BackToOffice : System.Web.UI.Page
    {
        private readonly object dvMdlContentFail;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["username"] == null)
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    LoadLeaves();
                    BindGridviewData();
                }
            }
            catch (Exception Ex)
            {

                Ex.Data.Clear();
            }
        }
        private void BindGridviewData()
        {
            string number = ddlLeaves.SelectedValue.ToString();
            try
            {
                using (SqlConnection con = Components.GetconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spbackto_officedata";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                    cmd.Parameters.AddWithValue("@LvNo", "'" + Components.ValidateEntry(number) + "'");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        //cmd.Parameters.AddWithValue("",);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvLines.DataSource = dt;
                            gvLines.DataBind();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        protected void LoadLeaves()
        {
            string userID = Session["username"].ToString();
            try
            {
                this.ddlLeaves.Items.Clear();

                using (SqlConnection connToNav = Components.GetconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spGetMyLeavesPosted";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = connToNav;
                    cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                    cmd.Parameters.AddWithValue("@userID", "'" + userID + "'");
                    ddlLeaves.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Leave--", String.Empty));
                    System.Web.UI.WebControls.ListItem li = null;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                            while (dr.Read())
                            {
                                li = new System.Web.UI.WebControls.ListItem(
                                    dr["No_"].ToString(),
                                    dr["No_"].ToString()
                                );

                                this.ddlLeaves.Items.Add(li);
                            }
                    }
                    connToNav.Close();
                }
            }
            catch (Exception ex)
            {
                //cSite.SendErrorToDeveloper(ex);
                ex.Data.Clear();
            }
        }
        protected void ddlLeaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridviewData();
        }
        public void Message1(string strMsg)
        {
            string strScript = null;
            strScript = "<script>";
            strScript = strScript + "alert('" + strMsg + "');";
            strScript = strScript + "</script>";
            Page.RegisterStartupScript("ClientScript", strScript.ToString());
        }
        public void Message(string strMsg)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myDetails", "$('#eventModal').modal();", true);
            //dvMdlContentFail.Visible = true;
            //dvMdlContentPass.Visible = false;
        }

        public void ExceptionMsg(string Msg)
        {
            lbtnApply.Visible = false;
            Message(Msg);
        }

        protected void lbtnApply_Click(object sender, EventArgs e)
        {
            string LeaveNo = ddlLeaves.SelectedValue.ToString();
            try
            {
                // Get Leave Details
                string getleave = Components.ObjNav.GetLeaveDetails(LeaveNo);
                if (!String.IsNullOrEmpty(getleave))
                {
                    string[] strdelimiters = new string[] { ":" };
                    string[] leavedata_arr = getleave.Split(strdelimiters, StringSplitOptions.None);
                    string empno = leavedata_arr[0];
                    string empname = leavedata_arr[1];
                    string leavedate = leavedata_arr[2];
                    string applieddays = leavedata_arr[3];
                    string startingdate = leavedata_arr[4];
                    string enddate = leavedata_arr[5];
                    string purpose = leavedata_arr[6];
                    string leavetype = leavedata_arr[7];
                    string returndate = leavedata_arr[8];
                    string userid = leavedata_arr[9];
                    string relieverno = leavedata_arr[10];
                    string relievername = leavedata_arr[11];
                    string shortcutdim3 = leavedata_arr[12];

                    try
                    {
                        // Safely parse the dates using DateTime.TryParseExact
                        DateTime parsedStartingDate;
                        if (!DateTime.TryParseExact(startingdate, "MM/dd/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartingDate))
                        {
                            Message("Invalid starting date format.");
                            return;
                        }

                        DateTime parsedEndDate;
                        if (!DateTime.TryParseExact(enddate, "MM/dd/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                        {
                            Message("Invalid end date format.");
                            return;
                        }

                        DateTime parsedReturnDate;
                        if (!DateTime.TryParseExact(returndate, "MM/dd/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedReturnDate))
                        {
                            Message("Invalid return date format.");
                            return;
                        }

                        // Submit back to office details
                        string submitBacktoOffice = Components.ObjNav.SubmitbacktoOffice(LeaveNo, empno, empname, Convert.ToInt32(applieddays), parsedStartingDate, parsedEndDate, purpose, leavetype, parsedReturnDate, userid, relieverno, relievername, shortcutdim3);
                        if (!String.IsNullOrEmpty(submitBacktoOffice))
                        {
                            string[] strdelimiterss = new string[] { "::" };
                            string[] staffLoginInfo_arr = submitBacktoOffice.Split(strdelimiterss, StringSplitOptions.None);
                            string DocumentNo = staffLoginInfo_arr[0];
                        }

                        // Process GridView rows
                        foreach (GridViewRow gvr in this.gvLines.Rows)
                        {
                            if (gvr.RowType != DataControlRowType.DataRow)
                                continue;

                            DateTime Actual, returndt;
                            TextBox txtActual = gvr.FindControl("txtActual") as TextBox;
                            TextBox txtreturndt = gvr.FindControl("txtreturndt") as TextBox;

                            if (!string.IsNullOrEmpty(txtActual.Text.ToString()))
                            {
                                if (DateTime.TryParseExact(txtActual.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out Actual) &&
                                    DateTime.TryParseExact(txtreturndt.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out returndt))
                                {
                                    if (!Components.IsNumeric(Actual.ToString()))
                                    {
                                        Message("Invalid Actual Amount!");
                                        txtActual.Focus();
                                        return;
                                    }

                                    if (!Components.IsNumeric(returndt.ToString()))
                                    {
                                        Message("Invalid Return Date!");
                                        txtreturndt.Focus();
                                        return;
                                    }

                                    // Process Back2Office details
                                    string DocumentNo = "";
                                    Components.ObjNav.Back2officedetails(DocumentNo, Actual, returndt, LeaveNo);
                                }
                                else
                                {
                                    Message("Invalid date format in Actual or Return Date fields.");
                                    return;
                                }
                            }
                        }

                        Message("Back to Office Applied Successfully");
                    }
                    catch (Exception exception)
                    {
                        Message("ERROR: " + exception.Message.ToString());
                        exception.Data.Clear();
                    }
                }
            }
            catch (Exception exception)
            {
                Message("ERROR: " + exception.Message.ToString());
                exception.Data.Clear();
            }
        }


        protected void lbtnApply_Click1(object sender, EventArgs e)
        {
            string LeaveNo = ddlLeaves.SelectedValue.ToString();
            try
            {
                //GetLeaveDetails
                string getleave = Components.ObjNav.GetLeaveDetails(LeaveNo);
                if (!String.IsNullOrEmpty(getleave))
                {
                    string[] strdelimiters = new string[] { ":" };
                    string[] leavedata_arr = getleave.Split(strdelimiters, StringSplitOptions.None);
                    string empno = leavedata_arr[0];
                    string empname = leavedata_arr[1];
                    string leavedate = leavedata_arr[2];
                    string applieddays = leavedata_arr[3];
                    string startingdate = leavedata_arr[4];
                    string enddate = leavedata_arr[5];
                    string purpose = leavedata_arr[6];
                    string leavetype = leavedata_arr[7];
                    string returndate = leavedata_arr[8];
                    string userid = leavedata_arr[9];
                    string relieverno = leavedata_arr[10];
                    string relievername = leavedata_arr[11];
                    string shortcutdim3 = leavedata_arr[12];
                    try
                    {
                        string submitBacktoOffice = Components.ObjNav.SubmitbacktoOffice(LeaveNo, empno, empname, Convert.ToInt32(applieddays), Convert.ToDateTime(startingdate), Convert.ToDateTime(enddate), purpose, leavetype, Convert.ToDateTime(returndate), userid, relieverno, relievername, shortcutdim3);
                       if (!String.IsNullOrEmpty(submitBacktoOffice))
                        {
                            string[] strdelimiterss = new string[] { "::" };
                            string[] staffLoginInfo_arr = submitBacktoOffice.Split(strdelimiters, StringSplitOptions.None);

                            string DocumentNo = staffLoginInfo_arr[0];
                        }

                        foreach (GridViewRow gvr in this.gvLines.Rows)
                        {
                            if (gvr.RowType != DataControlRowType.DataRow)
                                continue;

                            DateTime Actual, returndt;
                            TextBox txtActual = gvr.FindControl("txtActual") as TextBox;
                            TextBox txtreturndt = gvr.FindControl("txtreturndt") as TextBox;
                            if (!string.IsNullOrEmpty(txtActual.Text.ToString()))
                            {
                                Actual = DateTime.ParseExact(txtActual.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                //cashd = Convert.ToDecimal(txtCash.Text.ToString());
                                returndt = DateTime.ParseExact(txtreturndt.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                if (!string.IsNullOrEmpty(txtreturndt.ToString()))
                                {
                                    if (!Components.IsNumeric(Actual.ToString()))
                                    {
                                        Message("Invalid Actual Amount!");
                                        txtActual.Focus();
                                        return;
                                    }
                                    if (!Components.IsNumeric(returndt.ToString()))
                                    {
                                        Message("Invalid Cash Amount!");
                                        txtreturndt.Focus();
                                        return;
                                    }
                                    string DocumentNo = "";
                                    Components.ObjNav.Back2officedetails(DocumentNo, Actual, returndt, LeaveNo);
                                }
                            }
                            Message("Back to Office Applied Successfully");
                        }
                    }
                    catch (Exception exception)
                    {
                        Message("ERROR: " + exception.Message.ToString());
                        exception.Data.Clear();
                    }
                    
                   
                }
            }
            catch (Exception exception)
            {
                Message("ERROR: " + exception.Message.ToString());
                exception.Data.Clear();
            }

        }


        protected void gvLines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the DropDownList in the Row.
                DropDownList ddlRecpts = (e.Row.FindControl("ddlReceipts") as DropDownList);
                ddlRecpts.DataSource = GetData();
                ddlRecpts.DataTextField = "No";
                ddlRecpts.DataValueField = "No";
                ddlRecpts.DataBind();

                //Add Default Item in the DropDownList.
                ddlRecpts.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please select"));

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

        protected void ddlReceipts_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(sender as DropDownList).NamingContainer;
            string rNo = ((DropDownList)row.FindControl("ddlReceipts")).SelectedItem.Value;
            DataSet ds = GetMyData(rNo);
            //string country = (e.Row.FindControl("ddlReceipts") as Label).Text;
            //ddlRecpts.Items.FindByValue(country).Selected = true;
            row.Cells[0].Text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
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
    }
}
//#endregion