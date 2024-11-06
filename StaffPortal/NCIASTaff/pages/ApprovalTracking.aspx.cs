using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class ApprovalTracking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
            }
        }

        protected string ApprovalTracks()
        {
            var htmlStr = string.Empty;
            try
            {
                using (var conn = Components.GetconnToNAV())
                {
                    var cmd = new SqlCommand();
                    cmd.CommandText = "spGetMyApprovalTracks";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                    cmd.Parameters.AddWithValue("@DocumentNo", "'" + Request.QueryString["DocNum"].ToString() + "'");
                    int counter = 0;
                    using (SqlDataReader drL = cmd.ExecuteReader())
                    {
                        if (drL.HasRows)
                        {
                            while (drL.Read())
                            {
                                counter++;

                                htmlStr += string.Format(
                                    @"<tr  class='text-info small'>
                                            <td>{0}</td>
                                            <td>{1}</td>
                                            <td>{2}</td>
                                            <td>{3}</td>
                                            <td>{4}</td>
                                            <td>{5}</td>
                                            <td>{6}</td>
                                        </tr>",
                                    counter,
                                    drL["Entry No_"],
                                    drL["Sequence No_"],
                                    Convert.ToDateTime(drL["Date-Time Sent for Approval"]),
                                    drL["Sender ID"],
                                    drL["Approver ID"],
                                    drL["Status"]
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                exception.Data.Clear();
            }
            return htmlStr;
        }
    }
}