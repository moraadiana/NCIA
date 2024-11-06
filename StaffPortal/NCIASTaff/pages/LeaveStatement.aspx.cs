using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIASTaff.pages
{
    public partial class LeaveStatement : System.Web.UI.Page
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

                GeneratetLeaveStatement();
            }
        }

        private void GeneratetLeaveStatement()
        {
            try
            {
                string username = Session["username"].ToString();
                string fileName = username.Replace("/", "");
                string pdfFilename = $"Leave-Statement-{fileName}.pdf";
                Components.ObjNav.GenerateStaffLeaveStatement(username, String.Format(@"Leave-Statement-{0}.pdf", fileName));
               


                string networkPath = @"\\10.107.8.40\Downloads\" + pdfFilename;


                string localFolderPath = Server.MapPath("~/Downloads/");
                string localFilePath = Path.Combine(localFolderPath, pdfFilename);


                if (!Directory.Exists(localFolderPath))
                {
                    Directory.CreateDirectory(localFolderPath);
                }


                System.IO.File.Copy(networkPath, localFilePath, true);


                string fileUrl = ResolveUrl("~/Downloads/" + pdfFilename);


                myPDF.Attributes.Add("src", fileUrl);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}