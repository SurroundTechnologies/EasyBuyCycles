//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.Module.ContentWindow.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using A4DN.Core.WPF.Base;
using A4DN.Core.BOS.DataController;
using A4DN.Core.BOS.ViewModel;
using BOS.CustomerViewModel;
using BOS.CustomerDataEntity;
using BOS.CustomerDataMaps;
using WPF.OrderManagement.Shared;
using A4DN.Core.WPF.Base.DataPresentation.TreeListView;
using BOS.OrderDataEntity;
using BOS.OrderItemDataEntity;
using A4DN.Core.BOS.Base;
using BOS.OrderViewModel;
using System.ComponentModel;
using System.Linq;
using BOS.OrderItemViewModel;
using A4DN.Core.WPF.Base.DataPresentation;

namespace WPF.Customer
{
    /// <summary>
    /// Content Window for the module. Contains areas to handle commands before and after the parent has handled them. Interacts with the ViewModel.
    /// </summary>
    public partial class CustomerExpContent : AB_ContentWindowBase
    {
        CustomerVM _ViewModel;

        /// <summary>
        /// Type initializer / static constructor
        /// </summary>
        static CustomerExpContent()
        {
            RG_StaticInit();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerExpContent(AB_VisualComponentInitializationArgs initializationArgs)
            : base(initializationArgs) { }


        /// <summary>
        /// Initializes static members
        /// </summary>
        private static void RG_StaticInit()
        {
            // Enroll Culture Resource
            AB_SystemController.ap_SystemPropertyMethods.am_EnrollResourceCultureInfo(typeof(CustomerExpContent), WPF.OrderManagement.Views.Properties.Resources.Culture);
        }

        /// <summary>
        /// Sets properties to change the parent initialization. This method is called during the parent's constructor.
        /// If it is necessary to override what is set here or set additional parent properties, use <c>am_SetParentProperties</c>.
        /// </summary>
        private void RG_SetParentProperties()
        {
            // Call Initialize Component im order to access XAML objects in Code Behind
            InitializeComponent();

            ap_DataMaps = new CustomerMaps().ap_FieldMaps;
            _ViewModel = FindResource("CustomerVM") as CustomerVM;
            ap_ViewModel = _ViewModel;
            ap_MainDetailType = typeof(CustomerDetail);

            ap_ModuleDetails.Add(new AB_ModuleDetailDefinition(1, typeof(CustomerDetail), 0, items => (from CustomerEntity item in items
                                                                                                       select new TreeEntity
                                                                                                       {
                                                                                                           Name = item.Name,
                                                                                                           Contact_Sales_ProductCode = item.ContactFullName,
                                                                                                           Address_Quantity = item.BillingAddressLine,
                                                                                                           Email = item.Email,
                                                                                                           Telephone_OrderDate = item.Telephone,
                                                                                                           ap_Entity = item
                                                                                                       })));
            ap_ModuleDetails.Add(new AB_ModuleDetailDefinition(3, "OrderDetail", 1, items => (from OrderEntity item in items
                                                                                              select new TreeEntity
                                                                                              {
                                                                                                  Telephone_OrderDate = item.OrderDate.GetValueOrDefault().ToShortDateString(),
                                                                                                  Name = item.ShippingAddressName,
                                                                                                  Address_Quantity = item.CustomerName,
                                                                                                  Contact_Sales_ProductCode = item.SalesPersonName,
                                                                                                  ap_Entity = item
                                                                                              }).OrderBy(x => (x.ap_Entity as OrderEntity).OrderInternalID)));
            ap_ModuleDetails.Add(new AB_ModuleDetailDefinition(2, "OrderItemDetail", 2, items => (from OrderItemEntity item in items
                                                                                                  select new TreeEntity
                                                                                                  {
                                                                                                      Name = item.ProductName,
                                                                                                      Contact_Sales_ProductCode = item.ProductCode,
                                                                                                      Address_Quantity = item.Quantity.GetValueOrDefault().ToString(),
                                                                                                      ap_Entity = item
                                                                                                  }).OrderBy(x => (x.ap_Entity as OrderItemEntity).OrderItemInternalID)));
        }

        /// <summary>
        /// This method is called during the parent's constructor. Set properties to change the parent initialization.
        /// </summary>
        protected override void am_SetParentProperties()
        {
            RG_SetParentProperties();
        }

        /// <summary>
        /// This method is called after the Loaded event. At this point the XAML objects can be accessed.
        /// </summary>
        protected override void am_OnInitialized()
        {
            base.am_OnInitialized();

            var tree = ap_DataPresenter.ap_TreeList;
            tree.ap_ShowImages = true;
            tree.ItemExpanded += Tree_ItemExpanded;

            //Define Columns that will be displayed in TreeListView
            var columns = tree.ap_Columns;

            columns.Add(new AB_TreeListViewColumn
            {
                ap_SortProperty = "Name",
                DisplayMemberBinding = new Binding("Name"),
                Width = 300,
                Header = "Name"
            });

            columns.Add(new AB_TreeListViewColumn
            {
                ap_SortProperty = "Contact_Sales_ProductCode",
                DisplayMemberBinding = new Binding("Contact_Sales_ProductCode"),
                Width = 200,
                Header = "Contact/Sales"
            });


            columns.Add(new AB_TreeListViewColumn
            {
                ap_SortProperty = "Address_Quantity",
                DisplayMemberBinding = new Binding("Address_Quantity"),
                Width = 200,
                Header = "Address/Quantity"
            });

            columns.Add(new AB_TreeListViewColumn
            {
                ap_SortProperty = "Telephone_OrderDate",
                DisplayMemberBinding = new Binding("Telephone_OrderDate"),
                Width = 200,
                Header = "Telephone/OrderDate"
            });

            columns.Add(new AB_TreeListViewColumn
            {
                ap_SortProperty = "Email",
                DisplayMemberBinding = new Binding("Email"),
                Width = 200,
                Header = "Email"
            });

            //Define the final level to avoid unnecessary expander.
            var levelSetting = tree.ap_LevelSettings.FirstOrDefault(o => o.ap_Level == 2);
            if (levelSetting != null)
            {
                levelSetting.ap_HideExpander = true;
            }
        }

        private void _FetchOrders(object sender, DoWorkEventArgs e)
        {
            var treeEntity = (TreeEntity)e.Argument;
            var customer = (CustomerEntity)treeEntity.ap_Entity;

            var searchEntity = new OrderEntity { CustomerInternalID = customer.CustomerInternalID };
            var inArgs = new AB_SelectInputArgs<OrderEntity>("YD10", AB_SearchMethods.InitialSearch, searchEntity, searchEntity.am_BuildDefaultQuery(), 25, true);

            var orderVM = new OrderVM();

            e.Result = new object[] { orderVM.am_Select(inArgs), treeEntity };
        }

        private void _FetchOrders_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = (object[])e.Result;

            var retArgs = (AB_SelectReturnArgs<OrderEntity>)args[0];
            var treeEntity = (TreeEntity)args[1];

            if (retArgs != null && retArgs.ap_ReturnCode == "OK")
            {
                var levelSetting = ap_DataPresenter.ap_TreeList.ap_LevelSettings.FirstOrDefault(o => o.ap_Level == (treeEntity.ap_Level + 1));
                if (levelSetting != null && levelSetting.ap_DataTransform != null)
                {
                    var list = levelSetting.ap_DataTransform(retArgs.ap_RecordSet).ToList();
                    list.ForEach(o => treeEntity.ap_Children.Add(o));
                }
            }

            ap_ModuleExplorerComponent.am_StopSpinner();
        }

        private void _FetchOrderItems(object sender, DoWorkEventArgs e)
        {
            var treeEntity = (TreeEntity)e.Argument;
            var order = (OrderEntity)treeEntity.ap_Entity;

            var searchEntity = new OrderItemEntity { OrderInternalID = order.OrderInternalID };
            var inArgs = new AB_SelectInputArgs<OrderItemEntity>("YD10", AB_SearchMethods.InitialSearch, searchEntity, searchEntity.am_BuildDefaultQuery(), 25, true);

            var orderItemVM = new OrderItemVM();
            e.Result = new object[] { orderItemVM.am_Select(inArgs), treeEntity };
        }

        private void _FetchOrderItems_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            var args = (object[])e.Result;

            var retArgs = (AB_SelectReturnArgs<OrderItemEntity>)args[0];
            var treeEntity = (TreeEntity)args[1];

            if (retArgs != null && retArgs.ap_ReturnCode == "OK")
            {
                var levelSetting = ap_DataPresenter.ap_TreeList.ap_LevelSettings.FirstOrDefault(o => o.ap_Level == (treeEntity.ap_Level + 1));
                if (levelSetting != null && levelSetting.ap_DataTransform != null)
                {
                    var list = levelSetting.ap_DataTransform(retArgs.ap_RecordSet).ToList();
                    list.ForEach(o => treeEntity.ap_Children.Add(o));
                }
            }

            ap_ModuleExplorerComponent.am_StopSpinner();
        }

        private void Tree_ItemExpanded(object sender, AB_ItemExpandedEventArgs e)
        {
            var treeEntity = (TreeEntity)e.Entity;
            if (treeEntity.ap_Children.Count > 0 || e.Loaded) //Skip if data already loaded.
                return;

            var background = new BackgroundWorker();

            //Define work to be done at each level.
            switch (e.Level)
            {
                case 0:
                    {
                        background.DoWork += _FetchOrders;
                        background.RunWorkerCompleted += _FetchOrders_Complete;
                    }
                    break;
                case 1:
                    {
                        background.DoWork += _FetchOrderItems;
                        background.RunWorkerCompleted += _FetchOrderItems_Complete;
                    }
                    break;
                default:
                    return;
            }

            ap_ModuleExplorerComponent.am_StartSpinner();
            background.RunWorkerAsync(treeEntity);
        }

        /// <summary>
        /// This method is called before the base object has processed the command.
        /// </summary>
        protected override void am_BeforeProcessCommand(AB_Command command, RoutedEventArgs e)
        {
            var selectedEntity = ap_SelectedEntity as CustomerEntity;

            switch (command.ap_CommandID)
            {
                case Constants.CMD_CopyAddressLine:
                    Utilities.CopyToClipboard(selectedEntity.BillingAddressLine);
                    e.Handled = true;
                    break;
                case Constants.CMD_CopyAddressBlock:
                    Utilities.CopyToClipboard(selectedEntity.BillingAddressBlock);
                    e.Handled = true;
                    break;
                case Constants.CMD_OpenInMaps:
                    Utilities.OpenWithGoogleMaps(selectedEntity.BillingAddressLine);
                    e.Handled = true;
                    break;
                case Constants.CMD_CallCustomer:
                    Utilities.CallCustomer(selectedEntity.Telephone);
                    e.Handled = true;
                    break;
                case Constants.CMD_EmailCustomer:
                    Utilities.EmailCustomer(selectedEntity.Email);
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This method is called after the base object has processed the command.
        /// </summary>
        protected override void am_AfterProcessCommand(AB_Command command, RoutedEventArgs e)
        {
            switch (command.ap_CommandID)
            {
                default:
                    break;
            }
        }

        public class TreeEntity : AB_TreeListEntity
        {
            public string Name { get; set; }
            public string Contact_Sales_ProductCode { get; set; }
            public string Address_Quantity { get; set; }
            public string Telephone_OrderDate { get; set; }
            public string Email { get; set; }
        }
    }
}