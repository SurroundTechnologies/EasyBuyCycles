using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.ViewModel;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.DataPresentation.Dashboard;
using BOS.OrderItemDataEntity;
using BOS.OrderItemViewModel;
using BOS.OrderManagement.Shared;
using BOS.ProductDataEntity;
using BOS.ProductViewModel;
using System.Windows;
using System.Windows.Controls;
using WPF.Product;

namespace WPF.Dashboard
{
	/// <summary>
	/// Interaction logic for PopularProductsImagesDashboardPart.xaml
	/// </summary>
	public partial class PopularProductsImagesDashboardPart : AB_DashboardPart
	{
		public PopularProductsImagesDashboardPart()
		{
			InitializeComponent();
			_bp.ae_SelectAsyncCompleted += new AB_ViewModel.ad_SelectCompletedDelegate(_bp_ae_SelectAsyncCompleted);
		}

		OrderItemVM _bp = new OrderItemVM();

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}

		void _bp_ae_SelectAsyncCompleted(System.Collections.IList EntityRecords, AB_SelectReturnArgs SelectReturnArgs)
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
				foreach (OrderItemEntity entity in SelectReturnArgs.ap_OutputRecords)
				{
					using (var vm = new ProductVM())
					{
						var inEntity = new ProductEntity() { ProductInternalID = entity.ProductInternalID };
						var inArgs = new AB_FetchInputArgs<ProductEntity>(inEntity);
						var retArgs = vm.am_Fetch(inArgs);

						if (retArgs.ap_IsSuccess)
						{
							Button entityButton = new Button();
							entityButton.ContentTemplate = (DataTemplate)this.Resources["buttonTemplate"];
							entityButton.Content = retArgs.ap_OutputEntity as ProductEntity;
							entityButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
							entityButton.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
							entityButton.Click += new RoutedEventHandler(entityButton_Click);
							entityButton.Margin = new Thickness(5, 0, 5, 5);
							buttonsPanel.Children.Add(entityButton);
						}
					}
				}
			}
		}

		void entityButton_Click(object sender, RoutedEventArgs e)
		{
			if (((Button)sender).Content is ProductEntity entity)
				AB_SystemController.am_OpenDetail(entity, typeof(ProductDetail), Constants.MODULE_Product);
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
			OrderItemEntity searchEntity = new OrderItemEntity();
			AB_Query query = searchEntity.am_BuildDefaultQuery();
			query.am_AddOrderBy("YD1IQT", Sequence.Descending);
			AB_SelectInputArgs<OrderItemEntity> inArgs = new AB_SelectInputArgs<OrderItemEntity>("YD1I", AB_SearchMethods.InitialSearch, searchEntity, query, 5, false);
			_bp.am_SelectAsync(inArgs);
		}
	}
}