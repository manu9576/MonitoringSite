using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MonitoringSite
{
    public class MailSender
    {

        public static void SendMailShutdownSite(string siteName)
        {

            SendMail( "Site " + siteName + " don't response");


        }

        public static void SendMail(string message)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("your_email_address@gmail.com");
            mail.To.Add("manu9576@hotmail.fr");
            mail.Subject = "Test Mail";
            mail.Body = message;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("surveysite123123@gmail.com", "azerqsdf");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            mail.Subject = "this is a test email.";

        }
    }
}
