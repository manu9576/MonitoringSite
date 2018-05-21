using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MonitoringSite
{ 
    public delegate void ConnectionStatusChanged(SiteParameters site, bool status);

    public class SiteParameters : INotifyPropertyChanged
    {

        public event ConnectionStatusChanged OnConnectionChanged;

        public SiteParameters()
        {
            Events = new ObservableCollection<SiteEvent>();
            SiteName = string.Empty;
            SurveyTime = new TimeSpan();
            OffLineTime = new TimeSpan();
        }

        public SiteParameters(string siteName)
        {
            Events = new ObservableCollection<SiteEvent>();
            SiteName = siteName;
            SurveyTime = new TimeSpan();
            OffLineTime = new TimeSpan();
        }

        private bool m_IsOnline = false;
        public bool IsOnline
        {
            get
            {
                return m_IsOnline;
            }
            set
            {
                if(m_IsOnline != value)
                {
                    OnConnectionChanged?.Invoke(this,value);
                    m_IsOnline = value;
                }
            }
        }


        public void ChangdeConnectionStatus(SiteParameters site, bool statusLine)
        {
            if (statusLine)
            {
                AddEvent("Connection online");
            }
            else
            {
                AddEvent("Connection offline");
            }
        }


        public void AddEvent(string message)
        {
            Events.Add(new SiteEvent(message));
        }

        public string SiteName { get; set; }

        public TimeSpan _surveyTime;
        [XmlIgnore]
        public TimeSpan SurveyTime
        {
            get
            {
                return _surveyTime;
            }
            set
            {
                if (value != _surveyTime)
                {
                    _surveyTime = value;
                    RaisePropertyChanged("SurveyTime");
                }
            }
        }
        // For XML saving 
        public long SurveyTimeTicks
        {
            get
            {
                return SurveyTime.Ticks;
            }
            set
            {
                SurveyTime = new TimeSpan(value);
            }
        }


        public TimeSpan _offLineTime;
        [XmlIgnore]
        public TimeSpan OffLineTime
        {
            get
            {
                return _offLineTime;
            }
            set
            {
                if (value != _offLineTime)
                {
                    _offLineTime = value;
                    RaisePropertyChanged("OffLineTime");
                }
            }
        }
        // For XML saving of OffLineTime
        public long OffLineTimeTicks
        {
            get
            {
                return OffLineTime.Ticks;
            }
            set
            {
                OffLineTime = new TimeSpan(value);
            }
        }

        public ObservableCollection<SiteEvent> Events { get; set; }

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
