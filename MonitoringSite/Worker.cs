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
using System.Net;

namespace MonitoringSite
{
    public class Worker
    {
        

        public GlobalParameters Parameter { get; set; }
        
        private Thread m_Pinging = null;
        private bool m_running = false;

        public Worker()
        {
            
            Parameter = new GlobalParameters();
            Parameter.TimeIntervalSec = 2;

            

            m_Pinging = new Thread(Th_ping)
            {
                IsBackground = true
            };

            m_Pinging.Start();
        }

        public void LoadParamaters()
        {
            Parameter = GlobalParameters.LoadParameters();
            Log.LogLevel = Parameter.LogLevel;
        }

        public void SaveParameter()
        {
            Parameter.SaveParameters();
        }

        public bool AddSite(string name)
        {
            bool response = false;

            if (!Parameter.SitesList.Select(s => s.SiteName).Contains(name))
            {
                lock (Parameter)
                {
                    SiteParameters siteadded = new SiteParameters(name);
                    siteadded.AddEvent("Adding");
                    Parameter.ConnectionTest.OnConnectionChanged += siteadded.ChangdeConnectionStatus;
                    siteadded.ChangdeConnectionStatus(null,Parameter.ConnectionTest.IsOnline);
                    Parameter.SitesList.Add(siteadded);
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
            Stopwatch timeElapsed = null;

            while (m_running)
            {
                try
                {
                    timeElapsed = Stopwatch.StartNew();
                    
                    // Avoid that interval change during a loop
                    int interval = Parameter.TimeIntervalSec;

                    TestSite(Parameter.ConnectionTest, interval,true);

                    if (Parameter.ConnectionTest.IsOnline)
                    {
                        lock (Parameter)
                        {
                            Parallel.ForEach(Parameter.SitesList, site =>
                            {
                                TestSite(site, interval,false);
                            });
                        }
                    }

                    timeElapsed.Stop();

                    Thread.Sleep(Parameter.TimeIntervalSec * 1000 - (int)timeElapsed.ElapsedMilliseconds);                     
                }
                catch (Exception ex)
                {
                    Log.WriteError(ex);
                }   
            }
        }

        public static bool PingSite(string siteName,int timeOut)
        {
            bool pingResponse = false;

            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    Ping pinger = new Ping();

                    PingReply reply = pinger.Send(siteName,timeOut );

                    Log.WriteVerbose("Ping on site " + siteName + " response : " + reply.Status);

                    pingResponse = reply.Status == IPStatus.Success;
                }
               
            }
            catch(Exception ex)
            {
                Log.WriteError(ex);
                pingResponse = false;
            }

            return pingResponse;
        }

        public static bool TestSite(string siteName,int timeOut)
        {
            bool response = false;

            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    if (!siteName.Contains(@"http://") || !siteName.Contains(@"https://"))
                        siteName = @"http://" + siteName;

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(siteName);
                    request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                    request.Method = "HEAD";
                    request.Timeout = timeOut;
                    HttpWebResponse res = request.GetResponse() as HttpWebResponse;

                    response = (res== null || res.StatusCode == HttpStatusCode.OK);

                    Log.WriteVerbose("test site " + siteName + " response : " + res?.StatusCode ?? " null");

                }

            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                response = false;
            }
            return response;

        }


        public void TestSite(SiteParameters webSite,int interval,bool pingTest)
        {
            try
            {
                if (webSite != null)
                {

                    webSite.SurveyTime += TimeSpan.FromTicks(interval * 10000000);

                    if(pingTest)
                        webSite.IsOnline = PingSite(webSite.SiteName, Parameter.TimeOutSec);
                    else
                        webSite.IsOnline = TestSite(webSite.SiteName,Parameter.TimeOutSec);

                    if (!webSite.IsOnline)
                    {
                        webSite.OffLineTime += TimeSpan.FromTicks(interval*10000000);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }


        }


        private void ChangdeOnlineStatus(SiteParameters site, bool statusLine)
        {
            if (statusLine)
            {
                site.AddEvent("Site offline");
                MailSender.SendMailShutdownSite(site.SiteName,Parameter.Mail);
            }
            else
            {
                site.AddEvent("Site online");
                MailSender.SendMailOnlineSite(site.SiteName, Parameter.Mail);
            }
        }

        public void Stop()
        {
     

            m_running = false;
            
            m_Pinging.Join();
        }

        public void TestMail()
        {
            MailSender.SendMailShutdownSite("test",Parameter.Mail);
        }

        public SiteParameters GetSiteByName(string name)
        {

            var rep = Parameter.SitesList.Where(s => s.SiteName == name);

            if (rep == null || rep.Count() > 1)
            {
                throw new Exception("Get site : null or duplicate");
            }
                       

            return rep.First();
        }

    }
}
