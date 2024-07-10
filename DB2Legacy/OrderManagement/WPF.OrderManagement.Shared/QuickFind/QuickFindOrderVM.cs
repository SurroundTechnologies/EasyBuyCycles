using A4DN.Core.BOS.Base;
using BOS.OrderDataEntity;
using BOS.OrderManagement.Shared;
using BOS.OrderViewModel;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF.OrderManagement.Shared.QuickFind
{
	public class QuickFindOrderVM : QuickFindBaseVM
	{
		public QuickFindOrderVM()
		{
			ModuleNumber = Constants.MODULE_Order;
			DialogTitle = "Quick Find Order";
			SearchResultsHeading = "Orders:";
		}

		public override void SetDataGridColumns(ObservableCollection<DataGridColumn> columns)
		{
			columns.Add(TextColumn("Name", nameof(OrderEntity.PurchaseOrderNumberID), 50));
		}

		protected override async Task<AB_SelectReturnArgs> _LoadSearchResults(string searchString)
		{
			using (var vm = new OrderVM())
			{
				var query = new OrderEntity().am_BuildDefaultQuery();

				query.am_AddWhereGroup("OR", new AB_QueryWhereClause(OrderEntity.PurchaseOrderNumberIDProperty.ap_PropertyName, "LIKE", searchString));

				var inArgs = new AB_SelectInputArgs<OrderEntity>("YD1P", query);

				try
				{
					var retArgs = await Task.Run(() => vm.am_Select(inArgs));
					return retArgs;
				}
				catch (Exception e)
				{
					return new AB_SelectReturnArgs(AB_ReturnCodes.ER, string.Join("\n", e.am_GatherDetailedExceptionMessages()));
				}
			}
		}
	}
}