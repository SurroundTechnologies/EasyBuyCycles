using A4DN.Core.BOS.Base;
using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.DataPresentation.Dashboard;
using BOS.OrderDataEntity;
using BOS.OrderManagement.Shared;
using BOS.OrderViewModel;
using System.Windows;
using System.Windows.Controls;
using WPF.Order;
using WPF.OrderManagement.Shared;

namespace WPF.Dashboard
{
    /// <summary>
    /// Interaction logic for FiveRecentOrdersDashboardPart.xaml
    /// </summary>
    public partial class FiveRecentOrdersDashboardPart : AB_DashboardPart
    {
        public FiveRecentOrdersDashboardPart()
        {
            DataContext = this;
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        void entityButton_Click(object sender, RoutedEventArgs e)
        {
            var entity = ((Button)sender).Content as OrderEntity;
            AB_SystemController.am_OpenDetail(entity, typeof(OrderDetail), Constants.MODULE_Order);
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
                    using (var vm = new OrderVM())
                    {
                        var searchEntity = new OrderEntity();
                        var query = searchEntity.am_BuildDefaultQuery();
                        query.am_AddOrderBy(OrderEntity.OrderDateTimeProperty.ap_PropertyName, Sequence.Descending);
                        var inArgs = new AB_SelectInputArgs<OrderEntity>("YD1O", AB_SearchMethods.InitialSearch, searchEntity, query, 5, false);
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
                        foreach (OrderEntity entity in retArgs.ap_OutputRecords)
                        {
                            var entityButton = new Button();
                            entityButton.ContentTemplate = (DataTemplate)this.Resources["buttonTemplate"];
                            entityButton.Content = entity;
                            entityButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                            entityButton.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                            entityButton.Click += new RoutedEventHandler(entityButton_Click);
                            entityButton.Margin = new Thickness(5, 0, 5, 5);
                            buttonsPanel.Children.Add(entityButton);
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