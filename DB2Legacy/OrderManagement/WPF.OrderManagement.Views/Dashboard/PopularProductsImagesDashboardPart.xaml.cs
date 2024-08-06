using A4DN.Core.BOS.Base;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.DataPresentation.Dashboard;
using BOS.OrderItemDataEntity;
using BOS.OrderItemViewModel;
using BOS.OrderManagement.Shared;
using BOS.ProductDataEntity;
using BOS.ProductViewModel;
using System.Windows;
using System.Windows.Controls;
using WPF.OrderManagement.Shared;
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
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        void entityButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content is ProductEntity entity)
                AB_SystemController.am_OpenDetail(entity, typeof(ProductDetail), Constants.MODULE_Product);
        }

        public override void am_LoadData()
        {
            AB_WPFHelperMethods.am_CallInBackground(
                beforeBackground: () =>
                {
                    LoadingSpinner.am_Go();
                    LoadingSpinner.Visibility = Visibility.Visible;
                    buttonsPanel.Children.Clear();
                },
                inBackground: () =>
                {
                    using (var vm = new OrderItemVM())
                    {
                        var searchEntity = new OrderItemEntity();
                        var query = searchEntity.am_BuildDefaultQuery();
                        query.am_AddOrderBy("YD1IQT", Sequence.Descending);
                        var inArgs = new AB_SelectInputArgs<OrderItemEntity>("YD1I", AB_SearchMethods.InitialSearch, searchEntity, query, 5, false);
                        return vm.am_Select(inArgs);
                    }
                },
                onSuccess: (retArgs) =>
                {
                    if (retArgs == null || retArgs.ap_OutputRecords == null || retArgs.ap_OutputRecords.Count == 0)
                    {
                        tbNoRecords.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        tbNoRecords.Visibility = Visibility.Collapsed;
                        foreach (OrderItemEntity entity in retArgs.ap_OutputRecords)
                        {
                            using (var vm = new ProductVM())
                            {
                                var inEntity = new ProductEntity() { ProductInternalID = entity.ProductInternalID };
                                var inArgs = new AB_FetchInputArgs<ProductEntity>(inEntity);
                                var fetchArgs = vm.am_Fetch(inArgs);

                                if (fetchArgs.ap_IsSuccess)
                                {
                                    Button entityButton = new Button();
                                    entityButton.ContentTemplate = (DataTemplate)this.Resources["buttonTemplate"];
                                    entityButton.Content = fetchArgs.ap_OutputEntity as ProductEntity;
                                    entityButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    entityButton.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                                    entityButton.Click += new RoutedEventHandler(entityButton_Click);
                                    entityButton.Margin = new Thickness(5, 0, 5, 5);
                                    buttonsPanel.Children.Add(entityButton);
                                }
                            }
                        }
                    }

                    LoadingSpinner.am_Stop();
                    LoadingSpinner.Visibility = Visibility.Collapsed;
                    buttonsPanel.Visibility = Visibility.Visible;

                    base.am_LoadData();
                },
                onError: (retArgs) =>
                {
                    Utilities.MessageConsole.am_AddMessages(retArgs.ap_Messages, true, true);
                });
        }
    }
}