using NCIASTaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace NCIASTaff
{
    public class Components
    {
        public static SqlConnection connection;
        public static string Company_Name = "NCIA";

        public static string ReportsPath()
        {
            string currDir = "";
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            string root = Directory.GetCurrentDirectory();
            currDir = root;
            return currDir;
        }
        public static Staffportall ObjNav
        {
            get
            {
                var ws = new Staffportall();
                try
                {
                    var credentials = new NetworkCredential(ConfigurationManager.AppSettings["W_USER"], ConfigurationManager.AppSettings["W_PWD"]);
                    ws.Credentials = credentials;
                    ws.PreAuthenticate = true;
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return ws;
            }
        }

        public static void SentEmailAlerts(string address, string subject, string message)
        {
            try
            {
                string email = "dynamicsselfservice@gmail.com";
                string password = "ydujienvejtdojgv";

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(address));
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }
        }

        public static SqlConnection GetconnToNAV()
        {
            try
            {
                if (connection == null || connection.State == ConnectionState.Closed)
                {
                    var sqlConnectionString = ConfigurationManager.AppSettings["SqlConnection"];

                   // connection = new SqlConnection(@"Data Source=102.210.149.230;Initial Catalog=Ncia;MultipleActiveResultSets=true;Async=true;User ID=webportals;Password=login*4");
                    connection = new SqlConnection(sqlConnectionString);

                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return connection;
        }

       /* public static string EmployeeGender
        {
            get
            {
                string s = "";

                try
                {
                    string strSQL = String.Format("SELECT [Gender] FROM [" + Components.Company_Name + "$HRM-Employee C$bf65ec43-e187-4491-8d5f-10241a637a81] WHERE No_ = '" + HttpContext.Current.Session["username"].ToString() + "'");
                    SqlCommand command = new SqlCommand(strSQL, GetconnToNAV());
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            s = (Convert.ToInt32(dr["Gender"])).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return s;
            }
        }
       */
       public static string EmployeeGender
{
    get
    {
        string genderValue = "";

        try
        {
            // Retrieve username from session
            var username = HttpContext.Current.Session["username"];
            if (username == null || string.IsNullOrEmpty(username.ToString()))
            {
                throw new Exception("Session variable 'username' is not set or is empty.");
            }

            // Initialize the web service client for GetStaffGender
            var client = new Staffportall(); // Replace with actual client name
                    client.Credentials = new NetworkCredential("webportals", "Webportals@2024");
                    genderValue = client.GetStaffGender(username.ToString()); // Call the AL procedure via web service

            if (string.IsNullOrEmpty(genderValue))
            {
                Console.WriteLine("Gender not returned for user: " + username.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error retrieving EmployeeGender: " + ex.Message);
        }

        return genderValue;
    }
}

        public static bool IsNumeric(string no)
        {
            double result;
            if (double.TryParse(no, out result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ValidNumber(string numberToValidate)
        {
            bool b = false;
            try
            {
                numberToValidate = ValidateNumber(numberToValidate);

                if (numberToValidate.Length > 0)
                {
                    //throw exception if not double number.
                    double d = Convert.ToDouble(numberToValidate);

                    //success/valid double number
                    b = true;
                }
            }
            catch (Exception ex)
            {
                //cSite.SendErrorToDeveloper(ex);
                ex.Data.Clear();
            }
            return b;
        }

        public static string ValidateNumber(string Entry)
        {
            string r = Entry;

            try
            {
                Entry = ValidateEntry(Entry);

                string s = ",()";//sql illegal entry characters

                Entry = Entry.Trim();

                char[] c = s.ToCharArray();

                for (int i = 0; i < c.Length; i++)
                {
                    Entry = Entry.Replace(c[i].ToString(), "");
                }
                r = Entry;
            }
            catch (Exception)
            {
                throw;
            }
            return r;
        }
        public static string ValidateEntry(string Entry)
        {
            string r = Entry;
            try
            {
                if (Entry.Length > 250) Entry = Entry.Substring(0, 250);

                string s = "'";//sql illegal entry characters

                Entry = Entry.Trim();//remove spaces

                char[] c = s.ToCharArray();

                for (int i = 0; i < c.Length; i++)
                    if (Entry.Contains(c[i].ToString()))
                    {
                        //Entry = Entry.Replace(c[i].ToString(), "" );//blank
                        Entry = Entry.Replace(c[i].ToString(), "\'" + c[i].ToString());//escape character
                    }

                s = "--";//sql illegal entry characters

                if (Entry.Contains(s))
                    Entry = Entry.Replace(s, "");//blank

                r = Entry;
            }
            catch (Exception)
            {
                throw;
            }
            return r;
        }
    }
}