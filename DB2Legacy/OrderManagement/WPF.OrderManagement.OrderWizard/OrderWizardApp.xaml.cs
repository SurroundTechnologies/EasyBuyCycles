using WPF.Wizards.OrderWizard;

namespace WPF.OrderManagement.OrderWizard
{
    /// <summary>
    /// Interaction logic for OrderWizardApp.xaml
    /// </summary>
    public partial class OrderWizardApp : OrderManagement.App
    {
        protected override void am_SetParentProperties()
        {
            base.am_SetParentProperties();

            ap_MainWindowType = typeof(OrderWizardWindow);
            ap_WpfLogonType = typeof(Logon);
        }
    }
}
