using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Logger;

namespace MonitoringSite
{
    public class Worker
    {
        
        public int TimeIntervalSec { get; set; }


        public GlobalParameters Parameter { get; set; }
        
        private Thread m_Pinging = null;
        private bool m_running = false;


        public Worker()
        {
            Parameter = GlobalParameters.Load();

            TimeIntervalSec = 2;

            m_Pinging = new Thread(Th_ping)
            {
                IsBackground = true
            };

            m_Pinging.Start();
        }

        public bool AddSite(string name)
        {
            bool response = false;

            if (!Parameter.SitesList.Select(s => s.SiteName).Contains(name))
            {
                lock (Parameter)
                {
                    Parameter.SitesList.Add(new SiteParameters(name));
                    response = true;
                }
            }
            return response;
        }

        public bool RemoveSite(string name)
        {
            bool response = false;

            if (name != string.Empty)
            {
                SiteParameters site = Parameter.SitesList.Where(s => s.SiteName == name).FirstOrDefault();

                if(Parameter.SitesList.Contains(site))
                {
                    lock(Parameter)
                    {
                        Parameter.SitesList.Remove(site);
                        response = true;
                    }
                }
            }

            return response;
        }

        private void Th_ping()
        {
            m_running = true;
            bool connectionTest = false;
            Stopwatch timeElapsed = null;

            while (m_running)
            {
                try
                {
                    timeElapsed = Stopwatch.StartNew();
                    
                    // Avoid that interval change during a loop
                    int interval = TimeIntervalSec;

                    connectionTest = PingSite(Parameter.ConnectionTest, interval);

                    if (connectionTest)
                    {
                        lock (Parameter)
                        {
                            Parallel.ForEach(Parameter.SitesList, site =>
                            {
                                PingSite(site, interval);
                            });
                        }
                    }

                    timeElapsed.Stop();

                    Thread.Sleep(TimeIntervalSec * 1000 - (int)timeElapsed.ElapsedMilliseconds);                     
                }
                catch (Exception ex)
                {
                    Log.WriteError(ex);
                }   
            }
        }

        public static bool PingSite(SiteParameters webSite,int interval)
        {

            bool response = false;

            try
            {
                if (webSite != null)
                {
                    Ping pinger = new Ping();
                    
                    PingReply reply = pinger.Send(webSite.SiteName, 500);

                    Log.WriteInformation("Ping on site " + webSite.SiteName + " response : " + reply.Status);

                    webSite.SurveyTime += TimeSpan.FromTicks(interval * 10000000);

                    response = reply.Status == IPStatus.Success;

                    if (!response)
                    {
                        webSite.OffLineTime += TimeSpan.FromTicks(interval*10000000);
                    }
                }
            }
            catch (Exception ex)
            {
                response = false;
                Log.WriteError(ex);
            }

            return response;

        }

        public void Stop()
        {
            Parameter.SaveSites();

            m_running = false;
            
            m_Pinging.Join();
        }


    }
}
