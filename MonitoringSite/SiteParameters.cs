using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MonitoringSite
{
    public class SiteParameters : INotifyPropertyChanged
    {
        public SiteParameters(string siteName)
        {
            SiteName = siteName;
            SurveyTime = 0;
            OffLineTime = 0;
        }


        public string SiteName { get; private set; }

        public Int64 surveyTime;
        public Int64 SurveyTime
        {
            get
            {
                return surveyTime;
            }
            set
            {
                if (value != surveyTime)
                {
                    surveyTime = value;
                    RaisePropertyChanged("SurveyTime");
                }
            }
        }

        public Int64 offLineTime;
        public Int64 OffLineTime
        {
            get
            {
                return offLineTime;
            }
            set
            {
                if (value != offLineTime)
                {
                    offLineTime = value;
                    RaisePropertyChanged("OffLineTime");
                }
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
