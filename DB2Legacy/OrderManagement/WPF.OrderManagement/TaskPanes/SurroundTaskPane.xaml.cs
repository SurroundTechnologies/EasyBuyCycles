using A4DN.Core.WPF.Base;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.OrderManagement.TaskPanes
{
	/// <summary>
	/// Interaction logic for SurroundTaskPane.xaml
	/// </summary>
	public partial class SurroundTaskPane : AB_TaskPanePresenter
	{
		public SurroundTaskPane()
		{
			InitializeComponent();
		}

		protected override void am_SetParentProperties()
		{
			base.am_SetParentProperties();
			ap_Title = "Surround";
			ap_PreferredHeightIfDockedToTopOrBottom = 120;
			ap_PreferredWidthIfDockedToLeftOrRight = 300;
			ap_ImageFileName = "SurroundTask.png";
		}

		private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			var textBlock = sender as TextBlock;
			var text = textBlock.Text;
			if (text.Contains('@'))
			{
				// Email Address prefex with mailto
				text = "mailto:" + text;
			}
			else
			{
				text = "http://" + text;
			}

			_OpenUrl(text);
		}

		private void _OpenUrl(string Url)
		{
			System.Diagnostics.Process.Start(Url);
		}
	}
}