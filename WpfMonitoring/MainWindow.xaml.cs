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
        private MainView m_view = null;
        private Worker m_Worker = null;

        public MainWindow()
        {
            InitializeComponent();
            m_view = new MainView();
            this.DataContext = m_view;
            m_Worker = new Worker(m_view.Sites);
        }

        private void Bt_action_Click(object sender, RoutedEventArgs e)
        {
            m_view.AddSite(tb_NomSite.Text);
        }

        private void MainWindows_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_Worker.Stop();
        }
    }
}
