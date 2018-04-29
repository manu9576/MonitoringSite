using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitoringSite;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Test_MonitoringSite
{
    [TestClass]
    public class TestWorker
    {
        private SiteParameters googleDNS = new SiteParameters("8.8.8.8");
        private SiteParameters googleDNS2 = new SiteParameters("8.8.4.4");
        private SiteParameters noneExistingSite = new SiteParameters("255.255.255.25");
        private SiteParameters falseIP = new SiteParameters("8..8");

        [TestMethod]
        public void PingSite_Ok()
        {

            bool result = Worker.PingSite(googleDNS,1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PingSite_FalseIP()
        {

            bool result = Worker.PingSite(falseIP,1);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void PingSite_NoneExistingIP()
        {

            bool result = Worker.PingSite(noneExistingSite, 1);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void PingSite_MultiThread()
        {

            SiteParameters[] ips = { googleDNS, noneExistingSite, falseIP,googleDNS };

            Parallel.ForEach(ips, (ip) =>
            {
                bool result = Worker.PingSite(ip,1);
            });

        }

        [TestMethod]
        public void PingSite_EmptyString()
        {

            bool result = Worker.PingSite(new SiteParameters( string.Empty),1);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void Th_Ping_SitePing()
        {

            Worker wk = new Worker();
            wk.AddSite(googleDNS.SiteName);
            wk.AddSite(noneExistingSite.SiteName);


            Thread.Sleep(10000);
            wk.Stop();


            wk.RemoveSite(googleDNS.SiteName);
            wk.RemoveSite(noneExistingSite.SiteName);


            Assert.AreEqual(googleDNS.OffLineTime, new TimeSpan(), "Not offtime possible for 8.8.8.8");
            Assert.AreEqual(noneExistingSite.OffLineTime, noneExistingSite.SurveyTime, "Offtime must be egal to Suvey time for none existing site");

        }



    }
}
