using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_MonitoringSite
{
    [TestClass]
    public class TestMail
    {
        [TestMethod]
        public void TestSendingMail()
        {
            MonitoringSite.MailSender.SendMail("test","manu9576@hotmail.fr");
        }
    }
}
