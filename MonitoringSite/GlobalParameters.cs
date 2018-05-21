using Logger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

namespace MonitoringSite
{
    public class GlobalParameters : INotifyPropertyChanged
    {
        // name of storing file
        private const string FILE_NAME = "Param.xml";
        // path & name to xml file
        private static string _pathSiteName = null;

        // site used for detection of connection
        private readonly string SITE_ONELINE_TEST = "8.8.8.8";

        // Sites list that connection is watched
        public ObservableCollection<SiteParameters> SitesList { get; set; }

        // Site for connection testing
        public SiteParameters ConnectionTest { get; set; }

        private int _TimeOutSec;

        // timeout for Ping and WebResquest
        public int TimeOutSec
        {
            get
            {
                return _TimeOutSec;
            }
            set
            {
                if (value != _TimeOutSec)
                {
                    _TimeOutSec = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int _TimeIntervalSec;

        public int TimeIntervalSec
        {
            get
            {
                return _TimeIntervalSec;
            }
            set
            {
                if (value != _TimeIntervalSec)
                {
                    _TimeIntervalSec = value;
                    RaisePropertyChanged();
                }
            }
        }

        private MessageLevel _logLevel;
        /// <summary>
        /// Minimum level of logging message
        /// </summary>
        public MessageLevel LogLevel
        {
            get
            {
                return _logLevel;
            }

            set
            {
                if (_logLevel != value)
                {
                    _logLevel = value;
                    Log.LogLevel = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _Mail;
        /// <summary>
        ///adresse for the mail in case of disconnection 
        /// </summary>
        public string Mail
        {
            get
            {
                return _Mail;
            }
            set
            {
                if (value != _Mail)
                {
                    _Mail = value;
                    RaisePropertyChanged();
                }
            }
        }


        public GlobalParameters()
        {
            SitesList = new ObservableCollection<SiteParameters>();
            ConnectionTest = new SiteParameters(SITE_ONELINE_TEST);
            Mail = string.Empty;
            LogLevel = MessageLevel.Information;
            _pathSiteName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MonitoringSite\\";
            TimeOutSec = 1;
            TimeIntervalSec = 10;
        }

        /// <summary>
        /// Save all parametres in XML file
        /// </summary>
        /// <returns>saving OK</returns>
        public bool SaveParameters()
        {
            Log.WriteVerbose($"Start saving Parameter : {_pathSiteName}");

            bool success = false;

            try
            {
                if (!Directory.Exists(_pathSiteName))
                    Directory.CreateDirectory(_pathSiteName);


                XmlSerializer xs = new XmlSerializer(typeof(GlobalParameters));
                using (StreamWriter wr = new StreamWriter(_pathSiteName + FILE_NAME))
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

        /// <summary>
        /// Load the parameter for XML file
        /// </summary>
        /// <returns>parameter, default if XML file don't exist</returns>
        public static GlobalParameters LoadParameters()
        {
            Log.WriteVerbose("Start loading Parameter : {_pathSiteName}");

            GlobalParameters param = null;

            try
            {
                if (File.Exists(_pathSiteName + FILE_NAME))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(GlobalParameters));
                    using (StreamReader rd = new StreamReader(_pathSiteName + FILE_NAME))
                    {
                        param = xs.Deserialize(rd) as GlobalParameters;
                    }

                    Log.WriteVerbose("LoadSites find : " + string.Join(" - ", param.SitesList.Select(s => s.SiteName).ToList()));

                }
                else
                {
                    Log.WriteWarning("No preview Site file found!");
                    param = new GlobalParameters();

                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                param = new GlobalParameters();
            }

            return param;

        }

        private void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
