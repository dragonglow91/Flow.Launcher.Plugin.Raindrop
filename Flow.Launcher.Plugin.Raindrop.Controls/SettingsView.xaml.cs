using Flow.Launcher.Plugin.Raindrop.Core;
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

namespace Flow.Launcher.Plugin.Raindrop.Controls
{
    /// <summary>
    /// SettingsView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView(Settings settings)
        {
            DataContext = new SettingsViewModel(settings);
            InitializeComponent();
        }
    }
}
