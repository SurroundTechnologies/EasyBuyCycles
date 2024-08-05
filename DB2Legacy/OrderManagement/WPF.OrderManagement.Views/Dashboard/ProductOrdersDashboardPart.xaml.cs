using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.ViewModel;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.DataPresentation.Dashboard;
using BOS.OrderItemDataEntity;
using BOS.OrderItemViewModel;
using BOS.ShippingAddressDataEntity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WPF.OrderManagement.Shared;

namespace WPF.Dashboard
{
	/// <summary>
	/// Interaction logic for ProductOrdersDashboardPart.xaml
	/// </summary>
	public partial class ProductOrdersDashboardPart : AB_DashboardPart
	{
		public ProductOrdersDashboardPart()
		{
			InitializeComponent();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			//ap_MessageControl = tbNoRecords;
			//ap_DataDisplayControl = chart;
		}

        public AB_SmartCollection<OrderItemEntity> OrderItems { get; } = new AB_SmartCollection<OrderItemEntity>();

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
			OrderItems.Clear();
			OrderItemEntity searchEntity = new OrderItemEntity();
			AB_Query query = searchEntity.am_BuildDefaultQuery();
			query.am_AddOrderBy("YD1IQT", Sequence.Descending);

			AB_WPFHelperMethods.am_CallInBackground(
				inBackground: () =>
				{
					using (var vm = new OrderItemVM())
					{
						var inArgs = new AB_SelectInputArgs<OrderItemEntity>("YD1I", AB_SearchMethods.InitialSearch, searchEntity, query, 5, false);
						return vm.am_Select(inArgs);
					}
				},
				onSuccess: (retArgs) =>
				{
					if (retArgs == null || retArgs.ap_OutputRecords == null || retArgs.ap_OutputRecords.Count == 0)
					{
						return;
					}
					var results = retArgs.ap_OutputRecords.Cast<OrderItemEntity>();
					OrderItems.am_Reset(results);
				},
				onError: (retArgs) =>
				{
					Utilities.MessageConsole.am_AddMessages(retArgs.ap_Messages, true, true);
				});
		}
	}
}