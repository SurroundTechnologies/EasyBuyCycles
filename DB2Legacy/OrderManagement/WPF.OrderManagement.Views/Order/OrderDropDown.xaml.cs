//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.Module.DropDown.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using A4DN.Core.WPF.Base;
using BOS.OrderDataEntity;
using BOS.OrderViewModel;
using System;
using System.Windows;

namespace WPF.Order
{
    /// <summary>
    /// Interaction logic for OrderDropDown.xaml
    /// </summary>
    public partial class  OrderDropDown : AB_DropDownBase
	{
		/// <summary>
		/// Dependency Property for Binding of foreign keys.  As this property is updated, a fetch will occur to load the drop down appropriately
		/// </summary>
		
        [AB_DropdownKeyProperty]
		public int? OrderInternalID
		{
			get => (int?)GetValue(OrderInternalIDProperty);
			set => SetValue(OrderInternalIDProperty, value);
		}
        public static readonly DependencyProperty OrderInternalIDProperty = DependencyProperty.Register(nameof(OrderInternalID), typeof(int?), typeof(OrderDropDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(AB_DropDownBase.IdChanged)));

        /// <summary>
        /// Sets properties to change the parent initialization. This method is called during the parent's constructor.
        /// </summary>
        protected override void am_SetParentProperties()
		{
			InitializeComponent();
			
			// The SB Interface ID used to get a handle to the module entity in order show a Detail or Search and Select window
			ap_SbInterfaceReferenceID = "OrderDropDown";
			
			// An instance of the module-specific Busiess Process for performing IO to load the drop down
			ap_ViewModel = new OrderVM();
			
			// The type of detail to open if the user clicks the open button
			ap_DetailType = typeof(OrderDetail);

			// The default view to use when loading the drop down with multiple records
			ap_CurrentView = "YD1O";
			
			//The name of the property that should be displayed in the Drop Down
			ap_ComboBoxDisplayMemberPath = OrderEntity.PurchaseOrderNumberIDProperty.ap_PropertyName;
			
			// The type of entity that will be loaded in this drop down
			ap_EntityType = typeof(OrderEntity);

			//Default Max Count
			ap_MaxCount = 25;

			//Pass references of all Dependency Properties to base class so they can be managed there.
			
			// Setting dependency property lists directly has been replaced with applying [AB_Dropdown*Property] attributes on property definitions
			// ap_KeyDependencyProperties = new DependencyProperty[] { OrderInternalIDProperty};     
	

			// Set to true when there are multiple properties in ap_KeyDependencyProperties. 
			// If any are optional, set ap_IsOptionalForFetch flag in [AB_DropdownKeyProperty] attribute.
			// ap_RequireAllKeysFilledForFetch = ap_KeyDependencyProperties.Length > 1;

			// Set to false when entire collection can load into the combo box popup
			// ap_StartLoadingFromSelectedItem = false;
		}

		
		/// <summary>
		/// This method is for setting keys via code behind.  If there are multiple keys in the file, a fetch will be done each time a key is 
		/// set unless this method is called. ap_RequireAllKeysFilledForFetch only helps when the keys are empty, not when they're changing non-null values.
		/// </summary>
		public void SetValue(int? OrderInternalID)    
		{
			// Turn off property change event triggers
			ap_KeysPopulating = true;

			this.OrderInternalID = OrderInternalID;   
		
			// Turn on property change event triggers, and fire an event to respond to the changed keys.
			ap_KeysPopulating = false;
			am_OnIdChanged(new DependencyPropertyChangedEventArgs(OrderInternalIDProperty, null, OrderInternalID));
		}
	}
}