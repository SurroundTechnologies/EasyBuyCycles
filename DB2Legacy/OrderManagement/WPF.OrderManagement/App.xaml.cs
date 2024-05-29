//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.System.Application.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using A4DN.Core.WPF.Base;
using A4DN.Core.BOS.DataController;

namespace WPF.OrderManagement
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : AB_Application
	{
		/// <summary>
		/// Sets properties to change the parent initialization. This method is called during the parent's constructor.
		/// </summary>
		protected override void am_SetParentProperties()
		{
			ap_MainWindowType = typeof(Browser);
			ap_WpfLogonType = typeof(Logon);
		}
	   
		/// <summary>
		/// Merges any additional lower level resources.
		/// </summary>
		public override void am_LoadSupportingApplicationResources()
		{   
			base.am_MergeAdditionalResources(new Uri("WPF.OrderManagement.Shared;component/Resources/ApplicationResourceDictionary.xaml", UriKind.Relative));

		}
		
		/// <summary>
		/// Sets up Themes for application
		/// </summary>
		public override void am_SetUpThemes()
		{   
			// Set the Default Theme for the User
			// ap_DefaultThemeID = AB_SystemController.ap_ThemeModel.ap_Theme_SoftBlue;
		}


		/// <summary>
		/// Apply Skin for application. If not skin is provided, the default skin will be used
		/// </summary>
		public override void am_ApplySkin()
		{
			base.am_ApplySkin();

			//AB_SystemController.ap_AcceleratorSkin.am_SetSkin("<SkinName>", "<SkinURIPath>");

		}
		
		/// <summary>
		/// Merges any additional resources to extend the built in Accelerator Themes
		/// </summary>
		protected override void am_OnThemeChanged(string newThemeID)
		{
			base.am_OnThemeChanged(newThemeID);

			switch (newThemeID)
			{
				case AB_ThemeModel.ap_GraphiteThemeID:
					base.am_MergeAdditionalResources(new Uri("WPF.OrderManagement.Shared;component/Resources/Themes/GraphiteThemeResourceDictionary.xaml", UriKind.Relative));
					break;
				case AB_ThemeModel.ap_SoftBlueThemeID:
					base.am_MergeAdditionalResources(new Uri("WPF.OrderManagement.Shared;component/Resources/Themes/SoftBlueThemeResourceDictionary.xaml", UriKind.Relative));
					break;
				case AB_ThemeModel.ap_SoftGreenThemeID:
					base.am_MergeAdditionalResources(new Uri("WPF.OrderManagement.Shared;component/Resources/Themes/SoftGreenThemeResourceDictionary.xaml", UriKind.Relative));
					break;
				case AB_ThemeModel.ap_BlackThemeID:
                    base.am_MergeAdditionalResources(new Uri("WPF.OrderManagement.Shared;component/Resources/Themes/BlackThemeResourceDictionary.xaml", UriKind.Relative));
					break;
				default:
					break;
			}
		}
	}
}