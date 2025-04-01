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
        private void GenerateMemoReport1()
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


        private void GenerateMemoReport()
        {
            try
            {
                string username = Session["username"].ToString().Replace(@"/", @"");
                string memoNo = Request.QueryString["memoNo"].ToString();
                string fileName = Session["username"].ToString().Replace(@"-", @"");
                string returnstring = "";
                //string filePath = HostingEnvironment.MapPath($"~/Download/{fileName}");

                Components.ObjNav.GenerateMemoReport1(memoNo, String.Format("MEMO{0}.pdf", fileName), ref returnstring);
                myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("MEMO{0}.pdf", fileName)));
                byte[] bytes = Convert.FromBase64String(returnstring);

                string path = HostingEnvironment.MapPath("~/Download/" + $"Memo{fileName}.pdf");
                // Check if the file exists before setting the src attribute
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                FileStream stream = new FileStream(path, FileMode.CreateNew);
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(bytes, 0, bytes.Length);
                writer.Close();

                //File.WriteAllBytes(path, bytes);
                myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("MEMO{0}.pdf", fileName)));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                ex.Data.Clear();
            }
        }



    }
}