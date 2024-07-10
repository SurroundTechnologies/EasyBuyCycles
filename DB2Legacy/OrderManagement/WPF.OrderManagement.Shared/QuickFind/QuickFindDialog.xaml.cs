using A4DN.Core.BOS.Base;
using A4DN.Core.WPF.Base;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.OrderManagement.Shared.QuickFind
{
	/// <summary>
	/// Interaction logic for QuickFindDialog.xaml
	/// </summary>
	public partial class QuickFindDialog : AB_DialogBase
	{
		public QuickFindBaseVM ViewModel { get; private set; }

		private string _searchString;

		public QuickFindDialog(string searchString, QuickFindBaseVM viewModel)
		{
			Loaded += QuickFindDialog_Loaded;
			ViewModel = viewModel;
			_searchString = searchString;

			InitializeComponent();

			DataContext = this;
			this.Title = ViewModel.DialogTitle;
		}

		private async void QuickFindDialog_Loaded(object sender, RoutedEventArgs e)
		{
			DG_SearchResults.Columns.Clear();
			ViewModel.SetDataGridColumns(DG_SearchResults.Columns);
			DG_SearchResults.ItemsSource = ViewModel.SearchResults;

			ap_OkButton.Content = "Open Selected";
			ap_OkButton.IsEnabled = false;

			try
			{
				await ViewModel.LoadSearchResults(_searchString);

				if (ViewModel.SearchResults.Count == 1)
				{
					DG_SearchResults.SelectAll();
					_OpenSelectedDetailsAndCloseDialog();
				}

				if (ViewModel.SearchResults.Count == 0)
				{
					this.Close();
					MessageBox.Show($"No records containing {_searchString}.", "No Records Found", MessageBoxButton.OK);
				}
			}
			catch (Exception ex)
			{
				this.Close();
				MessageBox.Show(string.Join("\n", ex.Message), "No Records Found", MessageBoxButton.OK);
				return;
			}
		}

		private void _OpenSelectedDetailsAndCloseDialog()
		{
			foreach (AB_BusinessObjectEntityBase item in DG_SearchResults.SelectedItems)
			{
				ViewModel.OpenDetail(item);
			}
			this.Close();
		}

		public override void am_OnOkButtonClicked(object sender, RoutedEventArgs routedEventArgs)
		{
			_OpenSelectedDetailsAndCloseDialog();
			routedEventArgs.Handled = true; // We're taking care of closing dialog

			base.am_OnOkButtonClicked(sender, routedEventArgs);
		}

		private void OnDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			_OpenSelectedDetailsAndCloseDialog();
		}

		private void OnDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ap_OkButton.IsEnabled = DG_SearchResults.SelectedItems.Count > 0;
		}
	}
}