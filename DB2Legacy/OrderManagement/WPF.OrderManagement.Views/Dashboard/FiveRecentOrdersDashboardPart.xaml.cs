using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.ViewModel;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.DataPresentation.Dashboard;
using BOS.OrderDataEntity;
using BOS.OrderManagement.Shared;
using BOS.OrderViewModel;
using System.Windows;
using System.Windows.Controls;
using WPF.Order;

namespace WPF.Dashboard
{
	/// <summary>
	/// Interaction logic for FiveRecentOrdersDashboardPart.xaml
	/// </summary>
	public partial class FiveRecentOrdersDashboardPart : AB_DashboardPart
	{
		OrderVM _orderbp = new OrderVM();
		public FiveRecentOrdersDashboardPart()
		{
			DataContext = this;
			InitializeComponent();
			_orderbp.ae_SelectAsyncCompleted += new AB_ViewModel.ad_SelectCompletedDelegate(_orderbp_ae_SelectAsyncCompleted);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}

		void _orderbp_ae_SelectAsyncCompleted(System.Collections.IList EntityRecords, AB_SelectReturnArgs SelectReturnArgs)
		{
			buttonsPanel.Children.Clear();

			if (SelectReturnArgs == null || SelectReturnArgs.ap_OutputRecords == null || SelectReturnArgs.ap_OutputRecords.Count == 0)
			{
				tbNoRecords.Visibility = Visibility.Visible;
				return;
			}
			else
			{
				tbNoRecords.Visibility = Visibility.Collapsed;
				foreach (OrderEntity entity in SelectReturnArgs.ap_OutputRecords)
				{
					Button entityButton = new Button();
					entityButton.ContentTemplate = (DataTemplate)this.Resources["buttonTemplate"];
					entityButton.Content = entity;
					entityButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
					entityButton.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
					entityButton.Click += new RoutedEventHandler(entityButton_Click);
					entityButton.Margin = new Thickness(5, 0, 5, 5);
					buttonsPanel.Children.Add(entityButton);
				}
			}
		}

		void entityButton_Click(object sender, RoutedEventArgs e)
		{
			OrderEntity entity = ((Button)sender).Content as OrderEntity;
			AB_SystemController.am_OpenDetail(entity, typeof(OrderDetail), Constants.MODULE_Order);
		}

		public override void am_LoadData()
		{
			base.am_LoadData();

			_LoadData();
		}

		public override void am_Refresh()
		{
			base.am_Refresh();

			_LoadData();
		}

		private void _LoadData()
		{
			buttonsPanel.Children.Clear();
			OrderEntity searchEntity = new OrderEntity();
			AB_Query query = searchEntity.am_BuildDefaultQuery();
			query.am_AddOrderBy("OrderDateTime", Sequence.Descending);
			AB_SelectInputArgs<OrderEntity> inArgs = new AB_SelectInputArgs<OrderEntity>("YD1O", AB_SearchMethods.InitialSearch, searchEntity, query, 5, false);
			_orderbp.am_SelectAsync(inArgs);
		}
	}
}