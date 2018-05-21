using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitoringSite;

namespace Test_MonitoringSite
{
    [TestClass]
    public class TestSite
    {

        [TestMethod]
        public void TestSite_Ok()
        {

            bool result = Worker.TestSite(Constantes.Google, 500);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestSite_False()
        {

            bool result = Worker.TestSite(Constantes.FalseSite, 500);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void TestSite_NoneExisting()
        {

            bool result = Worker.TestSite(Constantes.NoneExistingSite, 500);

            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void TestSite_MultiThread()
        {
            bool allTestOk = true;

            List<Tuple<string, bool>> sites = new List<Tuple<string, bool>>()
            {
                new Tuple<string, bool>(Constantes.Google,true ),
                new Tuple<string, bool>(Constantes.NoneExistingSite,false ),
                new Tuple<string, bool>(Constantes.FalseSite,false ),
                new Tuple<string, bool>(Constantes.Google2,true )
            };


            Parallel.ForEach(sites, (site) =>
            {
                bool result = Worker.TestSite(site.Item1, 500);
                Console.WriteLine("Result of site " + site.Item1 + " is " + site.Item2);
                allTestOk = allTestOk & (result == site.Item2);
            });

            Assert.IsTrue(allTestOk, "At least one Site has a wrong response");

        }

        [TestMethod]
        public void TestSite_EmptyString()
        {

            bool result = Worker.PingSite(string.Empty, 500);

            Assert.IsTrue(!result);
        }

    }
}
