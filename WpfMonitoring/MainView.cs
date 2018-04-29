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
        public SiteParameters TestConnection { get; set; }
        public ObservableCollection<SiteParameters> Sites { get; set; }

        public MainView()
        {
            Sites = new ObservableCollection<SiteParameters>();

        }

        public MainView(ObservableCollection<SiteParameters> sites,SiteParameters testConnection)
        {
            Sites = sites;
            TestConnection = testConnection;
        }


    }
}
