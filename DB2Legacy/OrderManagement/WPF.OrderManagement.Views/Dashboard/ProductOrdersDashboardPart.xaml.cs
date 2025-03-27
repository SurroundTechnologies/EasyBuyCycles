using A4DN.Core.BOS.Base;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.DataPresentation.Dashboard;
using BOS.OrderItemDataEntity;
using BOS.OrderItemViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        }

        public ObservableCollection<OrderItemEntity> OrderItems { get; } = new ObservableCollection<OrderItemEntity>();

        public override void am_LoadData()
        {
            _LoadData();
        }

        private void _LoadData()
        {
            AB_WPFHelperMethods.am_CallInBackground(
                beforeBackground: () =>
                {
                    LoadingSpinner.am_Go();
                    LoadingSpinner.Visibility = Visibility.Visible;
                    OrderItems.Clear();
                },
                inBackground: () =>
                {
                    using (var vm = new OrderItemVM())
                    {
                        var searchEntity = new OrderItemEntity();
                        var query = searchEntity.am_BuildDefaultQuery();
                        query.am_AddOrderBy("YD1IQT", Sequence.Descending);
                        var inArgs = new AB_SelectInputArgs<OrderItemEntity>("YD1I", AB_SearchMethods.InitialSearch, searchEntity, query, 5, false);
                        return vm.am_Select(inArgs).am_Cast<OrderItemEntity>();
					}
                },
                onSuccess: (retArgs) =>
                {
                    var results = retArgs.ap_OutputRecords.Cast<OrderItemEntity>();

                    foreach (OrderItemEntity ent in results)
                    {
					    OrderItems.Add(ent);
					}

                    LoadingSpinner.am_Stop();
                    LoadingSpinner.Visibility = Visibility.Collapsed;

                    base.am_LoadData();
                },
                onError: (retArgs) =>
                {
                    Utilities.MessageConsole.am_AddMessages(retArgs.ap_Messages, true, true);
					LoadingSpinner.am_Stop();
					LoadingSpinner.Visibility = Visibility.Collapsed;
				},
                onException: (e) =>
                {
					Utilities.MessageConsole.am_AddMessage(e.Message, true, true);
				});
        }
    }
}