using A4DN.Core.WPF.Base;
using BOS.OrderManagement.Shared.Properties;
using System.Windows;
using System.Windows.Input;

namespace WPF.OrderManagement.TaskPanes
{
	public partial class AcceleratorTaskPane : AB_TaskPanePresenter
	{
		public AcceleratorTaskPane()
		{
			InitializeComponent();
		}

		protected override void am_SetParentProperties()
		{
			base.am_SetParentProperties();
			ap_Title = "Accelerator";
			ap_PreferredHeightIfDockedToTopOrBottom = 140;
			ap_PreferredWidthIfDockedToLeftOrRight = 300;
			ap_ImageFileName = "AcceleratorIcon.png";
		}

		private void _OpenUrl()
		{
			string url = "http://www.surroundtech.com/SoftwareSolutions/Accelerator_Software_Solutions/Accelerated_Development.aspx";
			System.Diagnostics.Process.Start(url);
		}

		private void Button_Click(object sender, RoutedEventArgs e) => _OpenUrl();
		private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e) => _OpenUrl();
	}
}