using NCIASTaff.NAVWS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class AppraisalLines : System.Web.UI.Page
    {
        Staffportall webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }
            LoadStaffDetails();
            string query = Request.QueryString["query"];

            if (query == "new")
            {
                MultiView1.SetActiveView(vwHeader);
            }

        }
        private void LoadStaffDetails()
        {
            try
            {
                string staffNo = Session["username"].ToString();
                string staffName = Session["StaffName"].ToString();
                string response = webportals.GetStaffDepartmentDetails(staffNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {

                        lblDepartment.Text = responseArr[1];
                        lblDirectorate.Text = responseArr[2];
                    }
                }

                lblStaffNo.Text = staffNo;
                //lblPayee.Text = staffName;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(vwLines);
        }
        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            newLines.Visible = false;
            lbtnAddLine.Visible = true;
        }
        protected void btnLine_Click(object sender, EventArgs e)
        {
            try
            {
                //string pettyCashNo = lblPettyCashNo.Text;
                //string employeeNo = Session["username"].ToString();
                //string advanceType = ddlAdvancType.SelectedValue;
                //string amount = txtAmnt.Text;
                //if (advanceType == "0")
                //{
                //    Message("Advance type cannot be null!");
                //    return;
                //}
                //if (amount == "")
                //{
                //    Message("Amount cannot be empty!");
                //    txtAmnt.Focus();
                //    return;
                //}

                //string response = webportals.InsertPettyCashRequisitionLine(pettyCashNo, advanceType, Convert.ToDecimal(amount));
                //if (!string.IsNullOrEmpty(response))
                //{
                //    string[] strLimiters = new string[] { "::" };
                //    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                //    string returnMsg = responseArr[0];
                //    if (returnMsg == "SUCCESS")
                //    {
                //        Message("Line added successfully!");
                //        txtAmnt.Text = string.Empty;
                //        BindGridViewData(pettyCashNo);
                //    }
                //}
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnAddLine_Click(object sender, EventArgs e)
        {
            //string pettyCashNo = string.Empty;
            //if (Request.QueryString["PettyCashNo"] == null)
            //{
            //    pettyCashNo = Session["PettyCashNo"].ToString();
            //}
            //else
            //{
            //    pettyCashNo = Request.QueryString["PettyCashNo"].ToString();
            //}
           // lblLNo.Text = pettyCashNo;
            //LoadAdvanceTypes();
            newLines.Visible = true;
            lbtnAddLine.Visible = false;
        }

        private void BindGridViewData(string pettyCashNo)
        {
            string pettyCashLines = webportals.GetPettyCashLines(pettyCashNo);

            if (!string.IsNullOrEmpty(pettyCashLines))
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("Document No_");
                dt.Columns.Add("Advance Type");
                dt.Columns.Add("Account No_");
                dt.Columns.Add("Account Name");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Line No_");


                string[] lines = pettyCashLines.Split(new[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);


                foreach (string line in lines)
                {
                    string[] fields = line.Split(new[] { "::" }, StringSplitOptions.None);
                    if (fields.Length == 6)
                    {
                        DataRow row = dt.NewRow();
                        row["Document No_"] = fields[0];
                        row["Advance Type"] = fields[1];
                        row["Account No_"] = fields[2];
                        row["Account Name"] = fields[3];
                        row["Amount"] = fields[4];
                        row["Line No_"] = fields[5];

                        dt.Rows.Add(row);
                    }
                }

                gvLines.DataSource = dt;
                gvLines.DataBind();


            }

        }

    }
}