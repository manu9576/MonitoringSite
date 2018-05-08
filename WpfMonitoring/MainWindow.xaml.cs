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

namespace WpfMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainView m_View = null;
        private Worker m_Worker = null;

        public MainWindow()
        {
            InitializeComponent();

            m_Worker = new Worker();
            m_View = new MainView(m_Worker.Parameter.SitesList,m_Worker.Parameter.ConnectionTest);

            DataContext = m_View;


        }

        ~MainWindow()
        {
            if (m_Worker != null)
            {
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

            if (item != null && item is SiteParameters)
            {
                m_Worker.RemoveSite((item as SiteParameters).SiteName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            m_Worker.TestMail();
        }
    }
}
