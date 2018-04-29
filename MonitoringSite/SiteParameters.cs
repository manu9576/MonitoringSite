using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MonitoringSite
{
    public class SiteParameters : INotifyPropertyChanged
    {

       


        public SiteParameters()
        {
            SiteName = string.Empty;
            SurveyTime = new TimeSpan();
            OffLineTime = new TimeSpan();
        }

        public SiteParameters(string siteName)
        {
            SiteName = siteName;
            SurveyTime = new TimeSpan();
            OffLineTime = new TimeSpan();
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

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
