using A4DN.Core.BOS.Base;
using A4DN.Core.WPF.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPF.OrderManagement.Shared.QuickFind
{
	public abstract class QuickFindBaseVM : DependencyObject
	{
		public int ModuleNumber { get; protected set; }

		public string DialogTitle
		{
			get => (string)GetValue(DialogTitleProperty);
			set => SetValue(DialogTitleProperty, value);
		}
		public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register(nameof(DialogTitle), typeof(string), typeof(QuickFindBaseVM), new PropertyMetadata(default));

		public bool IsLoading
		{
			get => (bool)GetValue(IsLoadingProperty);
			set => SetValue(IsLoadingProperty, value);
		}
		public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(QuickFindBaseVM), new PropertyMetadata(false));

		public double DataGridOpacity
		{
			get => (double)GetValue(DataGridOpacityProperty);
			set => SetValue(DataGridOpacityProperty, value);
		}
		public static readonly DependencyProperty DataGridOpacityProperty = DependencyProperty.Register(nameof(DataGridOpacity), typeof(double), typeof(QuickFindBaseVM), new PropertyMetadata(1.0));

		public string SearchResultsHeading
		{
			get => (string)GetValue(SearchResultsHeadingProperty);
			set => SetValue(SearchResultsHeadingProperty, value);
		}
		public static readonly DependencyProperty SearchResultsHeadingProperty = DependencyProperty.Register(nameof(SearchResultsHeading), typeof(string), typeof(QuickFindBaseVM), new PropertyMetadata(default));

		public ObservableCollection<AB_BusinessObjectEntityBase> SearchResults { get; private set; } = new ObservableCollection<AB_BusinessObjectEntityBase>();

		public abstract void SetDataGridColumns(ObservableCollection<DataGridColumn> columns);

		protected DataGridColumn CheckBoxColumn(string header, string propertyName, double width)
		{
			return new DataGridCheckBoxColumn
			{
				Header = header,
				Binding = new Binding(propertyName),
				IsReadOnly = true,
				Width = new DataGridLength(width, DataGridLengthUnitType.Star)
			};
		}

		protected DataGridColumn TextColumn(string header, string propertyName, double width)
		{
			return new DataGridTextColumn
			{
				Header = header,
				Binding = new Binding(propertyName),
				IsReadOnly = true,
				Width = new DataGridLength(width, DataGridLengthUnitType.Star)
			};
		}

		public async Task LoadSearchResults(string searchString)
		{
			SearchResults.Clear();

			DataGridOpacity = 0.5;
			IsLoading = true;

			var retArgs = await _LoadSearchResults(searchString);

			DataGridOpacity = 1.0;
			IsLoading = false;

			if (retArgs.ap_IsSuccess)
			{
				if (retArgs.ap_RecordCount > 0)
				{
					foreach (AB_BusinessObjectEntityBase entity in retArgs.ap_OutputRecords)
					{
						SearchResults.Add(entity);
					}
				}
			}
			else
			{
				throw new ApplicationException(string.Join("\n", retArgs.ap_Messages.Select(m => m.Text)));
			}
		}

		protected abstract Task<AB_SelectReturnArgs> _LoadSearchResults(string searchString);

		public virtual void OpenDetail(AB_BusinessObjectEntityBase entity)
		{
			AB_SystemController.am_OpenDetail(entity.ap_UniqueName, entity.ap_UniqueKey, ModuleNumber);
		}
	}
}