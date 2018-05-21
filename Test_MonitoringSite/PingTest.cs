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

            bool result = Worker.PingSite(Constantes.GoogleDNS_IP,500);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PingSite_FalseIP()
        {

            bool result = Worker.PingSite(Constantes.False_IP, 500);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void PingSite_NoneExistingIP()
        {

            bool result = Worker.PingSite(Constantes.NoneExisting_IP, 500);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void PingSite_MultiThread()
        {

            string[] sites = 
            {
                Constantes.GoogleDNS_IP,
                Constantes.NoneExisting_IP,
                Constantes.False_IP,
                Constantes.GoogleDNS_IP
            };

            Parallel.ForEach(sites, (site) =>
            {
                bool result = Worker.PingSite(site, 500);
            });

        }

        [TestMethod]
        public void PingSite_EmptyString()
        {

            bool result = Worker.PingSite(string.Empty, 500);

            Assert.IsTrue(!result);
        }
    }
}
