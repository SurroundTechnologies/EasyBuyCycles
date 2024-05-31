//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.Module.Detail.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System.Collections.Generic;
using System.Windows.Documents;
using System.Globalization;
using A4DN.Core.WPF.Base;
using A4DN.Core.BOS.Base;
using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.BOS.ViewModel;
using BOS.OrderDataEntity;
using BOS.OrderViewModel;
using BOS.CustomerDataEntity;
using System.Windows.Controls;
using System.Windows;

namespace WPF.Order
{
	/// <summary>
	/// Interaction logic for OrderDetail.xaml
	/// </summary>
	public partial class OrderDetail : AB_DetailBase
	{        
		private OrderVM _ViewModel;

        public Panel OrderDetailLayout
        {
            get { return FieldsPanel; }
        }

        public Visibility NonWizardControlsVisibility
        {
            get { return (Visibility)GetValue(NonWizardControlsVisibilityProperty); }
            set { SetValue(NonWizardControlsVisibilityProperty, value); }
        }
        public static readonly DependencyProperty NonWizardControlsVisibilityProperty = DependencyProperty.Register("NonWizardControlsVisibility", typeof(Visibility), typeof(OrderDetail), new UIPropertyMetadata(Visibility.Visible));

		/// <summary>
		/// Type initializer / static constructor
		/// </summary>
		static OrderDetail()
		{
			RG_StaticInit();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		//=============================================================
		public OrderDetail(AB_DetailInitializationArgs detailInitArgs)
			 : base(detailInitArgs) { }
		public OrderDetail(){}

		
		/// <summary>
		/// Initializes static members
		/// </summary>
		private static void RG_StaticInit()
		{
			 // Enroll Culture Resource
			AB_SystemController.ap_SystemPropertyMethods.am_EnrollResourceCultureInfo(typeof(OrderDetail) , WPF.OrderManagement.Views.Properties.Resources.Culture);
		}

		/// <summary>
		/// Sets properties of the parent class before it is instantiated.
		/// If it is necessary to override what is set here or set additional parent properties, use <c>am_SetParentProperties</c>.
		/// </summary>
		private void RG_SetParentProperties()
		{            
			// Call Initialize Component in order to access XAML objects in Code Behind
			InitializeComponent();
			
			// Set the Base Tab Control, main fields tab, and the Audit tab so that the base class can manage these objects
			ap_MainTabControl = FieldsTabControl;
			ap_LayoutRoot = LayoutRoot;
			ap_MainFieldsTab = DetailFieldsTab;
			ap_AuditFieldsTab = AuditFieldsTab;

			// Set sub-browser Load ID - This is used for determining what sub-browsers to load.
			ap_SubBrowserLoadID = "OrderDetail";
			// Set Entity Type
			ap_EntityType = typeof(OrderEntity);
			
			// View Model
			_ViewModel = FindResource("OrderVM") as OrderVM;
			ap_ViewModel = _ViewModel; 

		}
		
		/// <summary>
		/// Sets properties to change the parent initialization. This method is called during the parent's constructor.
		/// </summary>
		protected override void am_SetParentProperties()
		{
			RG_SetParentProperties();
			
			base.am_SetParentProperties();
			
		}

		/// <summary>
		/// This method is called after the Loaded Event. At this point the XAML objects can be accessed.
		/// </summary>
		protected override void am_OnInitialized()
		{
			base.am_OnInitialized();

		}

		/// <summary>
		/// This method is called after the data is loaded.
		/// </summary>
		protected override void am_OnDataLoaded()
		{
			base.am_OnDataLoaded();

			// Current Loaded Data Entity
			var currentEntity = ap_CurrentEntity as OrderEntity;

			if (currentEntity != null && !currentEntity.OrderDate.HasValue && !currentEntity.OrderTime.HasValue)
			{
				var currentDatetime = System.DateTime.Now;
				currentEntity.OrderDate = currentDatetime.Date;
				currentEntity.OrderTime = currentDatetime.TimeOfDay;
			}
		}
		
		/// <summary>
		/// This method is called before the base object has processed the command.
		/// </summary>
		protected override void am_BeforeProcessCommand(AB_Command command, System.Windows.RoutedEventArgs e)
		{
			
			switch (command.ap_CommandID)
			{
				//case "<CommandID>":

                //    Do Something ...

                //	  set e.Handled to true to prevent the higher level from executing its command click logic and to prevent further processing by the Detail.
                //    e.Handled = true;
                
                //    break;

				default:
					break;
			}
			
		}

		/// <summary>
		/// This method is called after the base object has processed the command.
		/// </summary>
		protected override void am_AfterProcessCommand(AB_Command command, System.Windows.RoutedEventArgs e)
		{
			
			switch (command.ap_CommandID)
			{
				//case "<CommandID>":

                //    Do Something ...

                //    set e.Handled to true to prevent further processing by the Detail.
                //    e.Handled = true;
                
                //    break;

				default:
					break;
			}
			
		}


    }
}