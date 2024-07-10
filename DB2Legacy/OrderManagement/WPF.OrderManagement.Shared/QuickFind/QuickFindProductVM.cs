using A4DN.Core.BOS.Base;
using BOS.OrderManagement.Shared;
using BOS.ProductDataEntity;
using BOS.ProductViewModel;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF.OrderManagement.Shared.QuickFind
{
	public class QuickFindProductVM : QuickFindBaseVM
	{
		public QuickFindProductVM()
		{
			ModuleNumber = Constants.MODULE_Product;
			DialogTitle = "Quick Find Product";
			SearchResultsHeading = "Products:";
		}

		public override void SetDataGridColumns(ObservableCollection<DataGridColumn> columns)
		{
			columns.Add(TextColumn("Name", nameof(ProductEntity.Name), 50));
			columns.Add(TextColumn("Code", nameof(ProductEntity.Code), 50));
		}

		protected override async Task<AB_SelectReturnArgs> _LoadSearchResults(string searchString)
		{
			using (var vm = new ProductVM())
			{
				var query = new ProductEntity().am_BuildDefaultQuery();

				query.am_AddWhereGroup("OR", new AB_QueryWhereClause(ProductEntity.NameProperty.ap_PropertyName, "LIKE", searchString), new AB_QueryWhereClause(ProductEntity.CodeProperty.ap_PropertyName, "LIKE", searchString));

				var inArgs = new AB_SelectInputArgs<ProductEntity>("YD1P", query);

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