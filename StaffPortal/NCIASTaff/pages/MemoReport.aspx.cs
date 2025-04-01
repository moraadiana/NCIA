using NDMAstaff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class MemoReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                GenerateMemoReport();
            }
        }
        private void GenerateMemoReport()
        {
            try
            {
                string memoNo = Request.QueryString["memoNo"].ToString();
                string username = Session["username"].ToString();
                string safeUsername = username.Replace("/", "@");
                string fileName = String.Format("MemoReport-{0}.pdf", safeUsername);

                // Physical file path
                var filePath = Server.MapPath("~/Downloads/") + fileName;

                // Ensure directory exists
                if (!Directory.Exists(Server.MapPath("~/Downloads/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Downloads/"));
                }

                // Call the AL procedure
                Components.ObjNav.GenerateMemoReport(memoNo, fileName);

                // Verify the file exists
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Memo PDF file '{filePath}' was not found after generation.");
                }

                // Update iframe src
                string fileUrl = ResolveUrl("~/Downloads/" + fileName);
                myPDF.Attributes.Add("src", fileUrl);
                System.Diagnostics.Debug.WriteLine($"Memo generated successfully at: {fileUrl}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error generating memo: {ex.Message}");
                ex.Data.Clear();
            }
        }

       

      
    }
}