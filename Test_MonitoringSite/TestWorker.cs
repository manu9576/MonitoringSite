using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitoringSite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test_MonitoringSite
{
    class TestWorker
    {
        public object SuiviSite { get; private set; }

        [TestMethod]
        public void PingSite_Ok()
        {

            bool result = Worker.PingSite("8.8.8.8");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PingSite_FalseIP()
        {

            bool result = Worker.PingSite("8..8");

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void PingSite_NoneExistingIP()
        {

            bool result = Worker.PingSite("255.255.255.255");

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void PingSite_MultiThread()
        {

            string[] ips = { "8.8.8.8", "255.255.255.255", "255.255.255.255", "0.0.0.1", string.Empty };

            Parallel.ForEach(ips, (ip) =>
            {
                bool result = Worker.PingSite(ip);
            });

        }

        [TestMethod]
        public void PingSite_EmptyString()
        {

            bool result = Worker.PingSite(string.Empty);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void Th_Ping_SitePing()
        {

            ObservableCollection<SiteParameters> Sites = new ObservableCollection<SiteParameters>();

            SiteParameters googleSite = new SiteParameters("8.8.8.8");
            SiteParameters wrongSite = new SiteParameters("255.255.255.255");

            Sites.Add(googleSite);
            Sites.Add(wrongSite);


            Worker wk = new Worker(Sites);
            Thread.Sleep(10000);
            wk.Stop();

            Assert.AreEqual(googleSite.OffLineTime, 0, "Not offtime possible for 8.8.8.8");
            Assert.AreEqual(wrongSite.OffLineTime, wrongSite.SurveyTime, "Offtime must be egal to Suvey time for none existing site");

        }

    }
}
