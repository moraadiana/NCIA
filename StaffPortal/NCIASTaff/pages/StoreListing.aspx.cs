﻿using NCIASTaff.NAVWS;
using System;
using System.Data;
using System.Data.SqlClient;

namespace NCIASTaff.pages
{
    public partial class StoreListing : System.Web.UI.Page
    {
        Staffportall webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
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

        protected string Jobs1()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetMyStoresReq",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@userID", "'" + username + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int counter = 0;
                    while (reader.Read())
                    {
                        counter++;
                        var statusCls = "default";
                        string status = reader["MyStatus"].ToString();
                        switch (status)
                        {
                            case "Open":
                                statusCls = "warning"; break;
                            case "Released":
                                statusCls = "default"; break;
                            case "Pending Approval":
                                statusCls = "primary"; break;
                            case "Pending Prepayment":
                                statusCls = "danger"; break;
                            case "Canceled":
                                statusCls = "info"; break;
                            case "Posted":
                                statusCls = "success"; break;
                        }

                        htmlStr += String.Format(@"
                            <tr>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td>{3}</td>                                
                                <td><span class='label label-{5}'>{4}</span></td>
                                <td class='small'>
                                    <div class='options btn-group' >
					                    <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                    <ul class='dropdown-menu'>
                                            <li><a href='StoreLines.aspx?query=old&ReqNo={0}&status={4}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={1}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                            </tr>
                            ",
                             reader["No_"].ToString(),
                            reader["ReqStatus"].ToString(),
                            Convert.ToDateTime(reader["Request date"]).ToShortDateString(),
                            Convert.ToDateTime(reader["Required Date"]).ToShortDateString(),
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
        protected string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                string storereqList = webportals.GetMyStoreRequisitions(username);
                if (!string.IsNullOrEmpty(storereqList))
                {
                    int counter = 0;
                    string[] storereqListArr = storereqList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string storelist in storereqListArr)
                    {
                        counter++;
                        string[] responseArr = storelist.Split(strLimiters, StringSplitOptions.None);
                        var statusCls = "default";
                        string status = responseArr[3];
                        switch (status)
                        {
                            case "Open":
                                statusCls = "warning"; break;
                            case "Released":
                                statusCls = "default"; break;
                            case "Pending Approval":
                                statusCls = "primary"; break;
                            case "Pending Prepayment":
                                statusCls = "danger"; break;
                            case "Canceled":
                                statusCls = "info"; break;
                            case "Posted":
                                statusCls = "success"; break;
                        }
                        htmlStr += String.Format(@"
                            <tr>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td>{3}</td>    
                        
                                <td><span class='label label-{5}'>{4}</span></td>
                                <td class='small'>
                                    <div class='options btn-group' >
					                    <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                    <ul class='dropdown-menu'>
                                            <li><a href='StoreLines.aspx?query=old&ReqNo={1}&status={4}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={1}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                            </tr>
                            ",
                          counter,
                          responseArr[0],
                          responseArr[1],
                          responseArr[2],
                          responseArr[3],
                          // responseArr[4],


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