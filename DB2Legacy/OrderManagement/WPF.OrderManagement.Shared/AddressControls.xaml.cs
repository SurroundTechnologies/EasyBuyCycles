using A4DN.Core.WPF.Base;
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
using WPF.OrderManagement.Shared;

namespace WPF.OrderManagement.Shared
{
    /// <summary>
    /// Interaction logic for AddressControls.xaml
    /// </summary>
    public partial class AddressControls : UserControl
    {
        #region Properties

        public string FullAddressLine
        {
            get { return (string)GetValue(FullAddressLineProperty); }
            set { SetValue(FullAddressLineProperty, value); }
        }
        public static readonly DependencyProperty FullAddressLineProperty = DependencyProperty.Register("FullAddressLine", typeof(string), typeof(AddressControls), new PropertyMetadata(null));

        public string FullAddressBlock
        {
            get { return (string)GetValue(FullAddressBlockProperty); }
            set { SetValue(FullAddressBlockProperty, value); }
        }
        public static readonly DependencyProperty FullAddressBlockProperty = DependencyProperty.Register("FullAddressBlockBlock", typeof(string), typeof(AddressControls), new PropertyMetadata(null));

        public AB_MessageConsole MessageConsole
        {
            get { return (AB_MessageConsole)GetValue(MessageConsoleProperty); }
            set { SetValue(MessageConsoleProperty, value); }
        }
        public static readonly DependencyProperty MessageConsoleProperty = DependencyProperty.Register("MessageConsole", typeof(AB_MessageConsole), typeof(AddressControls), new PropertyMetadata(null));

        #endregion

        public AddressControls()
        {
            InitializeComponent();
        }

        private void Btn_Click_OpenMaps(object sender, System.Windows.RoutedEventArgs e) => Utilities.OpenWithGoogleMaps(FullAddressLine);
        private void Btn_Click_CopyAddressLine(object sender, System.Windows.RoutedEventArgs e) => Utilities.CopyToClipboard(FullAddressLine, MessageConsole);
        private void Btn_Click_CopyAddressBlock(object sender, System.Windows.RoutedEventArgs e) => Utilities.CopyToClipboard(FullAddressBlock, MessageConsole);


    }
}
