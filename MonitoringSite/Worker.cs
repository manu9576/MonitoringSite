using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace MonitoringSite
{
    public class Worker : IDisposable
    {
        private const int TIME_TEST_SEC = 1;

        private ObservableCollection<SiteParameters> m_Sites = null;
        private Thread m_Pinging = null;
        private bool m_running = false;
        static Ping m_pinger = new Ping();

        static Worker()
        {
            m_pinger = new Ping();

        }

        public Worker(ObservableCollection<SiteParameters> Sites)
        {
            m_Sites = Sites;

            m_Pinging = new Thread(Th_ping)
            {
                IsBackground = true
            };

            m_Pinging.Start();

        }

        private void Th_ping()
        {
            m_running = true;

            while (m_running)
            {
                try
                {

                    foreach (SiteParameters site in m_Sites)
                    {
                        Console.WriteLine("Start ping for : " + site.SiteName);

                        site.SurveyTime += TIME_TEST_SEC;

                        if (!PingSite(site.SiteName))
                        {
                            site.OffLineTime += TIME_TEST_SEC;
                        }

                        Console.WriteLine("End ping for : " + site.SiteName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }


                // TODO change method to be sure off time elpased for each loop
                Thread.Sleep(TIME_TEST_SEC * 1000);
            }

        }

        public static bool PingSite(string webSite)
        {

            bool response = false;

            try
            {
                if (webSite != string.Empty)
                {
                    lock (m_pinger)
                    {
                        PingReply reply = m_pinger.Send(webSite, 500);

                        if (reply.Status == IPStatus.Success)
                        {
                            response = true;
                        }

                    }
                }
            }
            catch (PingException ex)
            {
                response = false;
                Console.WriteLine(ex.Message);
                // TODO log error message
            }
            return response;

        }

        public void Stop()
        {
            m_running = false;

            m_Pinging.Join();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
