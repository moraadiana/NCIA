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

                GenerateLeaveStatement();
            }
        }
        private void GenerateLeaveStatement1()
        {
            string username = Session["username"].ToString();
            string fileName = username.ToString().Replace(@"/", @"");
            string pdfFilename = String.Format(@"Leave-Statement-{0}.pdf", fileName);
            var filePath = Server.MapPath("~/Downloads/") + String.Format("Leave-Statement-{0}.pdf", fileName);
            if (!Directory.Exists(Server.MapPath("~/Downloads/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Downloads/"));
            }

            Components.ObjNav.GenerateStaffLeaveStatement(username, String.Format(@"Leave-Statement-{0}.pdf", fileName));
            if (File.Exists(filePath))
            {
                System.Diagnostics.Debug.WriteLine("Statement generated successfully.");
                myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format("Leave-Statement-{0}.pdf", fileName)));
            }
            else
            {
                //throw new FileNotFoundException("Statement PDF was not found after generation.");
                System.Diagnostics.Debug.WriteLine("Statement PDF was not found after generation");
            }

        }
        protected void GenerateLeaveStatement()
        {
            try
            {
                var filename = Session["username"].ToString().Replace(@"/", @"");
                try
                {
                    Components.ObjNav.GenerateLeaveStatement(Session["username"].ToString(), String.Format("LvSttmnts{0}.pdf", filename));
                    myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format("LvSttmnts{0}.pdf", filename)));
                }
                catch (Exception exception)
                {
                    exception.Data.Clear();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

    }
}