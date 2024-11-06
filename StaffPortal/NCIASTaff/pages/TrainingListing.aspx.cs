using NCIASTaff.NAVWS;
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
    public partial class TrainingListing : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlDataReader reader;
        SqlCommand command;
        Staffportall webportals = Components.ObjNav;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
            }
        }

        public string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetTrainingApps",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@EmpNo", "'" + username + "'");
                reader = command.ExecuteReader();
                int counter = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        counter++;
                        var statusCls = "default";
                        string status = reader["MyStatus"].ToString();
                        switch (status)
                        {
                            case "New":
                                statusCls = "warning"; break;
                            case "Pending Approval":
                                statusCls = "primary"; break;
                            case "Approved":
                                statusCls = "success"; break;
                            case "Posted":
                                statusCls = "success"; break;
                            case "Cancelled":
                                statusCls = "danger"; break;
                            case "":
                                statusCls = "info"; break;
                        }

                        htmlStr += String.Format(@"
                        <tr  class='text-primary small'>
                            <td>{0}</td>
                            <td>{1}</td>
                            <td>{2}</td>
                            <td>{3}</td>
                            <td>{4}</td>
                            <td>{5}</td>
                            <td>{6}</td>
                            <td><span class='label label-{8}'>{7}</span></td>
                            <td>
                                <div class='options btn-group' >
					                <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                <ul class='dropdown-menu'>
                                        <li><a href='TrainingApplication.aspx?TrainingNo={0}&query=old'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                        <li><a href='ApprovalTracking.aspx?DocNum={0}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                    </ul>	
                                </div>
                            </td>
                        </tr>
                        ",
                        reader["Application No"].ToString(),
                        reader["Course Title"].ToString(),
                        Convert.ToDateTime(reader["From Date"]).ToShortDateString(),
                        Convert.ToDateTime(reader["To Date"]).ToShortDateString(),
                        reader["Purpose of Training"].ToString(),
                         Convert.ToInt32(Convert.ToDouble(reader["Duration"])),
                        reader["TrainingStatus"].ToString(),
                        status,
                        statusCls
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return htmlStr;
        }
    }
}