using A4DN.Core.WPF.Base;
using System.Windows.Input;

namespace WPF.OrderManagement.TaskPanes
{
	/// <summary>
	/// Interaction logic for HeadquartersTaskPane.xaml
	/// </summary>
	public partial class HeadquartersTaskPane : AB_TaskPanePresenter
	{
		public HeadquartersTaskPane()
		{
			InitializeComponent();
		}

		protected override void am_SetParentProperties()
		{
			base.am_SetParentProperties();
			ap_Title = "Headquarters";
			ap_PreferredHeightIfDockedToTopOrBottom = 140;
			ap_PreferredWidthIfDockedToLeftOrRight = 300;
			ap_ImageFileName = "Headquarters.png";
		}

		private void GoogleStackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			_OpenUrl(@"https://maps.google.com/maps?q=650+Bloomfield+Avenue,+Bloomfield,+NJ&hl=en&sll=26.206071,-81.779882&sspn=0.012995,0.026157&oq=650+Bloomfield+Avenue,+Bloo&hnear=650+Bloomfield+Ave,+Bloomfield,+New+Jersey+07003&t=m&z=16");
		}

		private void _OpenUrl(string Url)
		{
			System.Diagnostics.Process.Start(Url);
		}
	}
}