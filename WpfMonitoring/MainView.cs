using MonitoringSite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMonitoring
{
    public class MainView
    {

        public ObservableCollection<SiteParameters> Sites { get; set; }


        public MainView()
        {
            Sites = new ObservableCollection<SiteParameters>();

        }

        public void AddSite(string name)
        {
            SiteParameters newSite = new SiteParameters(name);
            Sites.Add(newSite);
        }

    }
}
