using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WPF.OrderManagement.Shared
{
    /// <summary>
    /// Interaction logic for WizardSummary.xaml
    /// </summary>
    public partial class WizardSummary : UserControl, INotifyPropertyChanged
    {
        #region Properties
        private string _Header;
        public string Header
        {
            get { return _Header; }
            set
            {
                if (value != _Header)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        Visibility = Visibility.Visible;
                    }
                    _Header = value;
                    NotifyPropertyChanged("Header");
                }
            }
        }

        private string _Info;
        public string Info
        {
            get { return _Info; }
            set
            {
                if (value != _Info)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        Visibility = Visibility.Visible;
                    }
                    _Info = value;
                    NotifyPropertyChanged("Info");
                }
            }
        }

        #endregion

        public WizardSummary()
        {
            InitializeComponent();
            Visibility = Visibility.Collapsed;
        }

        public WizardSummary(string header, string info) : this()
        {
            Visibility = Visibility.Visible;
            Header = header;
            Info = info;
        }

        #region INotifyPropertyChanged Members

        void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}