using A4DN.Core.BOS.Base;
using BOS.CustomerDataEntity;
using BOS.CustomerViewModel;
using BOS.OrderManagement.Shared;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF.OrderManagement.Shared.QuickFind
{
	public class QuickFindCustomerVM : QuickFindBaseVM
	{
		public QuickFindCustomerVM()
		{
			ModuleNumber = Constants.MODULE_Customer;
			DialogTitle = "Quick Find Customer";
			SearchResultsHeading = "Customers:";
		}

		public override void SetDataGridColumns(ObservableCollection<DataGridColumn> columns)
		{
			columns.Add(TextColumn("Name", nameof(CustomerEntity.Name), 50));
			columns.Add(TextColumn("Contact Full Name", nameof(CustomerEntity.ContactFullName), 50));
		}

		protected override async Task<AB_SelectReturnArgs> _LoadSearchResults(string searchString)
		{
			using (var vm = new CustomerVM())
			{
				var query = new CustomerEntity().am_BuildDefaultQuery();

				query.am_AddWhereGroup("OR", new AB_QueryWhereClause(CustomerEntity.NameProperty.ap_PropertyName, "LIKE", searchString));

				var inArgs = new AB_SelectInputArgs<CustomerEntity>("YD1C", query);

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