 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class pnineform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                LoadYears();
                LoadP9();
            }
        }
        protected void LoadYears()
        {
            string username = Session["username"].ToString();
            //string nameu = "'"+username+"'";
            try
            {
                using (SqlConnection connToNAV = Components.GetconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetP9Years";
                    SqlCommand cmdProgStage = new SqlCommand();
                    cmdProgStage.CommandText = sqlStmt;
                    cmdProgStage.Connection = connToNAV;
                    cmdProgStage.CommandType = CommandType.StoredProcedure;
                    cmdProgStage.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                    cmdProgStage.Parameters.AddWithValue("@username", "'"+username+"'");
                    using (SqlDataReader sqlReaderStages = cmdProgStage.ExecuteReader())
                    {
                        if (sqlReaderStages.HasRows)
                        {
                            ddlYear.DataSource = sqlReaderStages;
                            ddlYear.DataTextField = "Period Year";
                            ddlYear.DataValueField = "Period Year";
                            ddlYear.DataBind();
                        }
                    }
                    connToNAV.Close();
                }
            }
            catch (Exception ex)
            {

                ex.Data.Clear();
            }
        }
        protected void LoadP9()
        {
           try
            {
                var filename = Session["username"].ToString().Replace(@"/", @"");
                var employee =Session["username"].ToString();
                int period = Convert.ToInt32(ddlYear.SelectedValue);
                //var s =Convert.ToDateTime(period.ToString("M/dd/yyyy", CultureInfo.InvariantCulture));
                try
                { 
                    string returnstring = "";
                    Components.ObjNav.Generatep9Report(period, employee, String.Format("p9Form{0}.pdf", filename), ref returnstring);
                    myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("p9Form{0}.pdf", filename)));
                    //WSConfig.ObjNavWS.FnFosaStatement(accno, ref returnstring, filter);
                    byte[] bytes = Convert.FromBase64String(returnstring);
                    string path = HostingEnvironment.MapPath("~/Download/" + $"p9Form{filename}.pdf");
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    FileStream stream = new FileStream(path, FileMode.CreateNew);
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(bytes, 0, bytes.Length);
                    writer.Close();
                    myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("p9Form{0}.pdf", filename))); 
                }
            catch (Exception exception)
            {
                exception.Data.Clear();
               //     HttpContext.Current.Response.Write(exception);
            }
        }
           catch (Exception ex)
           {
                ex.Data.Clear();
                //HttpContext.Current.Response.Write(ex);
            }
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadP9();
        }
    }
}