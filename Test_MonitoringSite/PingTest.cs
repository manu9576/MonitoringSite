using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitoringSite;

namespace Test_MonitoringSite
{
    [TestClass]
    public class PingTest
    {

        [TestMethod]
        public void PingSite_Ok()
        {

            bool result = Worker.PingSite(new SiteParameters(Constantes.GoogleDNS), 1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PingSite_FalseIP()
        {

            bool result = Worker.PingSite(new SiteParameters(Constantes.FalseIP), 1);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void PingSite_NoneExistingIP()
        {

            bool result = Worker.PingSite(new SiteParameters(Constantes.NoneExistingSite), 1);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void PingSite_MultiThread()
        {

            SiteParameters[] ips = 
                {
                new SiteParameters(Constantes.GoogleDNS),
                new SiteParameters(Constantes.NoneExistingSite),
                new SiteParameters(Constantes.FalseIP),
                new SiteParameters(Constantes.GoogleDNS)
            };

            Parallel.ForEach(ips, (ip) =>
            {
                bool result = Worker.PingSite(ip, 1);
            });

        }

        [TestMethod]
        public void PingSite_EmptyString()
        {

            bool result = Worker.PingSite(new SiteParameters(string.Empty), 1);

            Assert.IsTrue(!result);
        }
    }
}
