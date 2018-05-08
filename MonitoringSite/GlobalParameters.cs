using Logger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MonitoringSite
{
    public class GlobalParameters
    {
        private const string FILE_NAME = "Param.xml";
        private static string m_pathSiteName = null;
        private readonly string SITE_ONELINE_TEST = "8.8.8.8";



        public ObservableCollection<SiteParameters> SitesList { get; set; }
        public SiteParameters ConnectionTest { get; set; }

        public GlobalParameters()
        {
            SitesList = new ObservableCollection<SiteParameters>();
            ConnectionTest = new SiteParameters(SITE_ONELINE_TEST);

        }

        public bool SaveSites()
        {
            bool success = false;

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(GlobalParameters));
                using (StreamWriter wr = new StreamWriter(m_pathSiteName + FILE_NAME))
                {
                    xs.Serialize(wr, this);
                }

                success = true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                success = false;
            }

            return success;
        }

        public static GlobalParameters Load()
        {
            GlobalParameters param = null;

            try
            {
                m_pathSiteName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MonitoringSite\\";

                if (!Directory.Exists(m_pathSiteName))
                    Directory.CreateDirectory(m_pathSiteName);

              

                if (File.Exists(m_pathSiteName + FILE_NAME))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(GlobalParameters));
                    using (StreamReader rd = new StreamReader(m_pathSiteName + FILE_NAME))
                    {
                        param = xs.Deserialize(rd) as GlobalParameters;
                    }

                    Log.WriteWarning("LoadSites find : " + string.Join(" - ", param.SitesList.Select(s => s.SiteName).ToList()));

                }
                else
                {
                    Log.WriteWarning("No preview Site file found!");
                    param = new GlobalParameters();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                param = new GlobalParameters();
            }

            return param;

        }
    }
}
