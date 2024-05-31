using A4DN.Core.WPF.Base.DataPresentation.Dashboard;
using BOS.OrderManagement.ExternalAPIs;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WPF.Dashboard
{
	/// <summary>
	/// Interaction logic for WeatherInfoDashboardPart.xaml
	/// </summary>
	public partial class WeatherInfoDashboardPart : AB_DashboardPart
	{
		public const string WeatherSite = "https://weather.com/weather/today/l/";
        public string ZipCode { get; set; }

        public WeatherInfoDashboardPart()
		{
			InitializeComponent();
			DataContext = this;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Navigate(WeatherSite);
		}

		public override void am_Refresh()
		{
			base.am_Refresh();
			Navigate(WeatherSite);
		}

		void Navigate(string url)
		{
			string ipAddress = IPAPIClient.GetPublicIp();
			ZipCode = IPAPIClient.GetZipCode(ipAddress).GetAwaiter().GetResult();

			if (!string.IsNullOrWhiteSpace(ZipCode))
			{
				string address = url + ZipCode;
				webViewBrowser.Source = new Uri(address);
			}
		}

		private void btn_Back_Click(object sender, System.Windows.RoutedEventArgs e) => webViewBrowser.GoBack();
		
		private void btn_Forward_Click(object sender, System.Windows.RoutedEventArgs e) => webViewBrowser.GoForward();	
	}
}
