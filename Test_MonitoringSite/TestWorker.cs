using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitoringSite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Test_MonitoringSite
{
    [TestClass]
    public class TestWorker
    {
 


        [TestMethod]
        public void Th_Ping_SitePing()
        {

            Worker wk = new Worker();
            wk.AddSite(Constantes.GoogleDNS);
            wk.AddSite(Constantes.NoneExistingSite);


            wk.TimeIntervalSec = 1;

            Thread.Sleep(10000);
            wk.Stop();

            SiteParameters siteGoogle = wk.GetSiteByName(Constantes.GoogleDNS);
            SiteParameters siteNonExisting = wk.GetSiteByName(Constantes.NoneExistingSite);


            Assert.AreEqual(siteGoogle.OffLineTime, new TimeSpan(), "Not offtime possible for 8.8.8.8");
            Assert.AreEqual(siteNonExisting.OffLineTime, siteNonExisting.SurveyTime, "Offtime must be egal to Suvey time for none existing site");

        }


        [TestMethod]
        public void Th_Ping_SitePingInterval()
        {

            Worker wk = new Worker();
            wk.AddSite(Constantes.GoogleDNS2);

            Stopwatch timeElapsed = Stopwatch.StartNew();

            wk.TimeIntervalSec = 1;

            Thread.Sleep(10050);
            wk.Stop();

            SiteParameters site = wk.GetSiteByName(Constantes.GoogleDNS2);

            if ((site.SurveyTime - timeElapsed.Elapsed) > TimeSpan.FromSeconds(0.5))
                Assert.Fail("Difference too importante > 0.5 sec");

            wk.RemoveSite(site.SiteName);


        }

        [TestMethod]
        public void Worker_AddingSite()
        {

            Worker wk = new Worker();

            wk.Parameter.SitesList.Clear();

            wk.AddSite(Constantes.GoogleDNS);
            Assert.AreEqual(wk.Parameter.SitesList.Count, 1,"Adding site : count must be 1");

            wk.AddSite(Constantes.GoogleDNS2);
            Assert.AreEqual(wk.Parameter.SitesList.Count, 2, "Adding new site : count must be 2");

            wk.AddSite(Constantes.GoogleDNS);
            Assert.AreEqual(wk.Parameter.SitesList.Count, 2, "Adding existing site : count must be 2");

            wk.RemoveSite(null);

            wk.RemoveSite(string.Empty);

            wk.RemoveSite(Constantes.NoneExistingSite);

            wk.RemoveSite(Constantes.GoogleDNS);
            Assert.AreEqual(wk.Parameter.SitesList.Count, 1, "Remove site : count must be 1");

        }

        
    }
}
