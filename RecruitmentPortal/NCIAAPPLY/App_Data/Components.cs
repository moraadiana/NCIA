using NCIAAPPLY.NAVWS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Mail;

namespace NCIAAPPLY
{
    public class Components
    {
        public static SqlConnection connection;
        public static string CompanyName = "NCIA TEST";
        public static Recruitment ObjNav
        {
            get
            {
                var ws = new Recruitment();
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

        // Get connection to NAV
        public static SqlConnection GetconnToNAV()
        {
            try
            {
                if (connection == null || connection.State == ConnectionState.Closed)
                {
                    var sqlConnectionString = ConfigurationManager.AppSettings["SqlConnection"];

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


        /*  public static void Context_BuildingRequest(object sender, Microsoft.OData.Client.BuildingRequestEventArgs e)
          {
              e.Headers.Add("Authorization", "Basic d2VicG9ydGFsczpXZWJwb3J0YWxzQDIwMjQ=");
          }*/

        // Sent email alerts
        public static void SendEmailAlerts(string recipient, string subject, string body)
        {
            string senderEmail = "dynamicsselfservice@gmail.com";
            string senderPassword = "ydujienvejtdojgv";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(senderEmail);
            message.Subject = subject;
            message.To.Add(new MailAddress(recipient));
            message.Body = $"<html><body>{body}</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 25;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);
        }

       
    }
}