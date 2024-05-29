//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.Module.DropDown.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Windows;
using A4DN.Core.WPF.Base;
using BOS.CustomerViewModel;
using BOS.CustomerDataEntity;
using BOS.CustomerDataMaps;

namespace WPF.Customer
{
	/// <summary>
	/// Interaction logic for CustomerDropDown.xaml
	/// </summary>
	public partial class  CustomerDropDown : AB_DropDownBase
	{
		/// <summary>
		/// Dependency Property for Binding of foreign keys.  As this property is updated, a fetch will occur to load the drop down appropriately
		/// </summary>
 
		
        [AB_DropdownKeyProperty]
		public int? CustomerInternalID
		{
			get => (int?)GetValue(CustomerInternalIDProperty);
			set => SetValue(CustomerInternalIDProperty, value);
		}
        public static readonly DependencyProperty CustomerInternalIDProperty = DependencyProperty.Register(nameof(CustomerInternalID), typeof(int?), typeof(CustomerDropDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(AB_DropDownBase.IdChanged)));

		/// <summary>
		/// Sets properties to change the parent initialization. This method is called during the parent's constructor.
		/// </summary>
		protected override void am_SetParentProperties()
		{
			InitializeComponent();
			
			// The SB Interface ID used to get a handle to the module entity in order show a Detail or Search and Select window
			ap_SbInterfaceReferenceID = "CustomerDropDown";
			
			// An instance of the module-specific Busiess Process for performing IO to load the drop down
			ap_ViewModel = new CustomerVM();
			
			// The type of detail to open if the user clicks the open button
			ap_DetailType = typeof(CustomerDetail);

			// The default view to use when loading the drop down with multiple records
			ap_CurrentView = "YD1CLF1";
			
			//The name of the property that should be displayed in the Drop Down
			ap_ComboBoxDisplayMemberPath = CustomerEntity.NameProperty.ap_PropertyName;
			
			// The type of entity that will be loaded in this drop down
			ap_EntityType = typeof(CustomerEntity);

			//Default Max Count
			ap_MaxCount = 25;

			//Pass references of all Dependency Properties to base class so they can be managed there.
			
			// Setting dependency property lists directly has been replaced with applying [AB_Dropdown*Property] attributes on property definitions
			//ap_KeyDependencyProperties = new DependencyProperty[] { CustomerInternalIDProperty};     
	

			// Set to true when there are multiple properties in ap_KeyDependencyProperties. 
			// If any are optional, set ap_IsOptionalForFetch flag in [AB_DropdownKeyProperty] attribute.
			ap_RequireAllKeysFilledForFetch = ap_KeyDependencyProperties.Length > 1;

			// Set to false when entire collection can load into the combo box popup
			// ap_StartLoadingFromSelectedItem = false;
		}

		
		/// <summary>
		/// This method is for setting keys via code behind.  If there are multiple keys in the file, a fetch will be done each time a key is 
		/// set unless this method is called. ap_RequireAllKeysFilledForFetch only helps when the keys are empty, not when they're changing non-null values.
		/// </summary>
		public void SetValue(int? CustomerInternalID)    
		{
			// Turn off property change event triggers
			ap_KeysPopulating = true;

			this.CustomerInternalID = CustomerInternalID;   
		
			// Turn on property change event triggers, and fire an event to respond to the changed keys.
			ap_KeysPopulating = false;
			am_OnIdChanged(new DependencyPropertyChangedEventArgs(CustomerInternalIDProperty, null, CustomerInternalID));
		}
	}
}