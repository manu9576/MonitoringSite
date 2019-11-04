using MonitoringSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoreMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Worker m_Worker = null;

        public MainWindow()
        {

            InitializeComponent();

            m_Worker = new Worker();
            m_Worker.LoadParamaters();
            DataContext = m_Worker.Parameter;

        }

        ~MainWindow()
        {
            if (m_Worker != null)
            {
                m_Worker.SaveParameter();
                m_Worker.Stop();
            }
        }

        private void Bt_action_Click(object sender, RoutedEventArgs e)
        {
            m_Worker.AddSite(tb_NomSite.Text);
        }

        private void Bt_delete_Click(object sender, RoutedEventArgs e)
        {
            object item = dg_Sites.SelectedItem;

            if (item != null && item is SiteParameters site)
            {
                m_Worker.RemoveSite(site.SiteName);
            }
        }

        private void Parameter_Click(object sender, RoutedEventArgs e)
        {
            Parameters windowParameter = new Parameters(m_Worker.Parameter);

            windowParameter.ShowDialog();

            windowParameter.Close();
        }

    }
}
