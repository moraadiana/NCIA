﻿using NCIASupplier.NAVWS;
using System;
using System.Net;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using System.Data;

namespace NCIASupplier
{
    public class Components
    {
        public static SqlConnection connection;
        public static string Company_Name = "Ncia";
        public static Supplier ObjNav
        {
            get
            {
                var ws = new Supplier();
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
                var smtpClient = new SmtpClient("smtp.gmail.com", 25);

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

        public static SqlConnection getconnToNAV()
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
    }
}