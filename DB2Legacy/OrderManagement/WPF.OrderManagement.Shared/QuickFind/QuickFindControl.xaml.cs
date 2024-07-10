using A4DN.Core.BOS.Base;
using A4DN.Core.SharedResources;
using A4DN.Core.WPF.Base;
using BOS.OrderManagement.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF.OrderManagement.Shared.QuickFind
{
	/// <summary>
	/// Interaction logic for QuickFindControl.xaml
	/// </summary>
	public partial class QuickFindControl : UserControl
	{
		public ObservableCollection<QuickFindOption> QuickFindOptions { get; private set; } = new ObservableCollection<QuickFindOption>();

		public QuickFindOption SearchType
		{
			get { return (QuickFindOption)GetValue(SearchTypeProperty); }
			set { SetValue(SearchTypeProperty, value); }
		}
		public static readonly DependencyProperty SearchTypeProperty = DependencyProperty.Register(nameof(SearchType), typeof(QuickFindOption), typeof(QuickFindControl), new PropertyMetadata(null));

		public string SearchString
		{
			get { return (string)GetValue(SearchStringProperty); }
			set { SetValue(SearchStringProperty, value); }
		}
		public static readonly DependencyProperty SearchStringProperty = DependencyProperty.Register(nameof(SearchString), typeof(string), typeof(QuickFindControl), new PropertyMetadata(null));

		public QuickFindControl()
		{
			// This collection defines the options that are available in the control. 
			var optionList = new List<QuickFindOption>
			{
				new QuickFindOption(typeof(QuickFindCustomerVM), Constants.MODULE_Customer),
				new QuickFindOption(typeof(QuickFindProductVM), Constants.MODULE_Product)
			}.Where(opt => opt.IsAccessible);

			optionList.ForEach(opt => QuickFindOptions.Add(opt));

			Loaded += QuickFindControl_Loaded;
			InitializeComponent();
			this.DataContext = this;
		}

		private ICommand _quickFindCommand;
		public ICommand QuickFindCommand
		{
			get => _quickFindCommand ?? (_quickFindCommand = new AB_CommandHandler(() => DoSearch(), () => !string.IsNullOrWhiteSpace(SearchString)));
		}

		private void QuickFindControl_Loaded(object sender, RoutedEventArgs e)
		{
			SearchType = QuickFindOptions.FirstOrDefault();
		}

		private void QuickFindSearchField_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				DoSearch();
			}
		}

		private void DoSearch()
		{
			// Create new VM for the search and show results in popup dialog

			QuickFindBaseVM viewModel = (QuickFindBaseVM)Activator.CreateInstance(SearchType.ViewModelType);

			if (viewModel != null)
				new QuickFindDialog(SearchString, viewModel).am_ShowDialog(AB_DialogButtons.OKCancel);
			else
				MessageBox.Show($"Failed to create a {SearchType.ViewModelType.Name} view model object");
		}

		private void SearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// Any time the SearchType selection changes, clear the search field so the placeholder text shows
			SearchString = null;
		}

		public class QuickFindOption
		{
			public string Label { get; private set; }
			public Type ViewModelType { get; private set; }
			public ImageSource ModuleIcon { get; private set; }
			public bool IsAccessible { get; private set; }

			private static readonly AB_MultiSizeImageBindingConverter _Converter = new AB_MultiSizeImageBindingConverter();

			public QuickFindOption(Type vmType, int moduleNumber)
			{
				ViewModelType = vmType;

				var module = AB_SystemController.ap_SystemPropertyMethods.am_GetModuleEntity(moduleNumber);

				if (module != null)
				{
					IsAccessible = true;
					Label = module.ModuleName;
					ModuleIcon = _Converter.Convert(module.Image, typeof(ImageSource), "16PixelImage", Thread.CurrentThread.CurrentCulture) as ImageSource;
				}
				else
				{
					IsAccessible = false;
				}
			}
		}

	}
}