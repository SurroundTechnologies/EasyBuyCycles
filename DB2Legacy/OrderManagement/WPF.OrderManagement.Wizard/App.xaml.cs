using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF.OrderManagement.Wizard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : AppMain
    {
        protected override void am_SetParentProperties()
        {
            base.am_SetParentProperties();

            ap_MainWindowType = typeof(WizardWindow);
            ap_WpfLogonType = typeof(Logon);
        }
    }
}
