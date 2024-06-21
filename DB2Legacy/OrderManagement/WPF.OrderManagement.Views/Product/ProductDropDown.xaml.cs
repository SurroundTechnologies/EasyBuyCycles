//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.Module.DropDown.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using A4DN.Core.WPF.Base;
using BOS.ProductDataEntity;
using BOS.ProductViewModel;
using System.Windows;

namespace WPF.Product
{
    /// <summary>
    /// Interaction logic for ProductDropDown.xaml
    /// </summary>
    public partial class  ProductDropDown : AB_DropDownBase
	{
		/// <summary>
		/// Dependency Property for Binding of foreign keys.  As this property is updated, a fetch will occur to load the drop down appropriately
		/// </summary>
 
        [AB_DropdownKeyProperty]
		public int? ProductInternalID
		{
			get => (int?)GetValue(ProductInternalIDProperty);
			set => SetValue(ProductInternalIDProperty, value);
		}
        public static readonly DependencyProperty ProductInternalIDProperty = DependencyProperty.Register(nameof(ProductInternalID), typeof(int?), typeof(ProductDropDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(AB_DropDownBase.IdChanged)));

        [AB_DropdownNonKeyProperty]
        public string Code
        {
            get => (string)GetValue(CodeProperty);
            set => SetValue(CodeProperty, value);
        }
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register(nameof(Code), typeof(string), typeof(ProductDropDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //[AB_DropdownNonKeyProperty]
        //public string ProductName
        //{
        //    get => (string)GetValue(ProductNameProperty);
        //    set => SetValue(ProductNameProperty, value);
        //}
        //public static readonly DependencyProperty ProductNameProperty = DependencyProperty.Register(nameof(ProductName), typeof(string), typeof(ProductDropDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [AB_DropdownNonKeyProperty]
        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }
        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(nameof(Category), typeof(string), typeof(ProductDropDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [AB_DropdownNonKeyProperty]
        public decimal? ListPrice
        {
            get => (decimal?)GetValue(ListPriceProperty);
            set => SetValue(ListPriceProperty, value);
        }
        public static readonly DependencyProperty ListPriceProperty = DependencyProperty.Register(nameof(ListPrice), typeof(decimal?), typeof(ProductDropDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [AB_DropdownNonKeyProperty]
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(ProductDropDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [AB_DropdownNonKeyProperty]
        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }
        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register(nameof(ImagePath), typeof(string), typeof(ProductDropDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Sets properties to change the parent initialization. This method is called during the parent's constructor.
        /// </summary>
        protected override void am_SetParentProperties()
		{
			InitializeComponent();
			
			// The SB Interface ID used to get a handle to the module entity in order show a Detail or Search and Select window
			ap_SbInterfaceReferenceID = "ProductDropDown";
			
			// An instance of the module-specific Busiess Process for performing IO to load the drop down
			ap_ViewModel = new ProductVM();
			
			// The type of detail to open if the user clicks the open button
			ap_DetailType = typeof(ProductDetail);

			// The default view to use when loading the drop down with multiple records
			ap_CurrentView = "YD1PLF1";
			
			//The name of the property that should be displayed in the Drop Down
			ap_ComboBoxDisplayMemberPath = ProductEntity.NameProperty.ap_PropertyName;
			
			// The type of entity that will be loaded in this drop down
			ap_EntityType = typeof(ProductEntity);

			//Default Max Count
			ap_MaxCount = 25;

			//Pass references of all Dependency Properties to base class so they can be managed there.
			
			// Setting dependency property lists directly has been replaced with applying [AB_Dropdown*Property] attributes on property definitions
			//ap_KeyDependencyProperties = new DependencyProperty[] { ProductInternalIDProperty};     
	

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
		public void SetValue(int? ProductInternalID)    
		{
			// Turn off property change event triggers
			ap_KeysPopulating = true;

			this.ProductInternalID = ProductInternalID;   
		
			// Turn on property change event triggers, and fire an event to respond to the changed keys.
			ap_KeysPopulating = false;
			am_OnIdChanged(new DependencyPropertyChangedEventArgs(ProductInternalIDProperty, null, ProductInternalID));
		}
	}
}