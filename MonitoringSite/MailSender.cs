using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MonitoringSite
{
    public class MailSender
    {

        public static void SendMailShutdownSite(string siteName,string to)
        {
            SendMail( "Site " + siteName + " don't work at " + DateTime.Now, to);
        }


        public static void SendMailOnlineSite(string siteName, string to)
        {
            SendMail("Site " + siteName + "  work again at " + DateTime.Now, to);
        }

        public static void SendMail(string message,string to)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("SurveySite@gmail.com");
            mail.To.Add(to);
            mail.Subject = "Mail for montoring site";
            mail.Body = message;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("surveysite123123@gmail.com", "azerqsdf");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            

        }
    }
}
