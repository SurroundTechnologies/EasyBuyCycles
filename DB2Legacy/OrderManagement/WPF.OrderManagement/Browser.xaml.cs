//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.System.Browser.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System.Windows;
using A4DN.Core.WPF.Base;
using A4DN.Core.BOS.ViewModel;
using WPF.OrderManagement;
using WPF.OrderManagement.Shared;
using System.Windows.Controls;
using WPF.OrderManagement.TaskPanes;
using BOS.OrderManagement.Shared;

namespace WPF.OrderManagement
{
	/// <summary>
	/// Interaction logic for Browser.xaml
	/// </summary>
	public partial class Browser : AB_Browser
	{
		public Browser()
		{
			// Set the Splash Window Type
			ap_SplashWindowType = typeof(Splash);

			// TODO: Image for Wallpaper used as background when there are not any Module Explorer Tabs open
			// Set the image name and any other settings to get the desired look.
			// ap_WallpaperImageName = "MyWallPaper.jpg";
			// ap_Wallpaper.Margin = new Thickness(50, 0, 50, 0);
			// ap_Wallpaper.Width = ...;		   

			InitializeComponent();

		}        
		
		// Before Command Clicked
		// *** This method is called before the Base object has processed the command.
		//=============================================================
		protected override void am_BeforeProcessCommand(AB_Command command, RoutedEventArgs e)
		{
			
			switch (command.ap_CommandID)
			{
				//case "<CommandID>":

				//    Do Something ...

				//    set e.Handled to true to prevent the higher level from executing its command click logic and to prevent further processing by the Detail.
				//    e.Handled = true;

				//    break;

				case Constants.CMD_ORDERWIZARD:
					e.Handled = true;
					AB_SystemController.ap_WizardModel.am_ShowWizard(Constants.CMD_ORDERWIZARD);
					break;

				default:
					break;
			}
			
		}


		// After Command Clicked
		// *** This method is called After the Base object has processed the command.
		//=============================================================
		protected override void am_AfterProcessCommand(AB_Command command, RoutedEventArgs e)
		{

			switch (command.ap_CommandID)
			{
				//case "<CommandID>":

                //    Do Something ...

                //	  set e.Handled to true to prevent further processing by the Detail.
                //    e.Handled = true;
                
                //    break;

				default:
					break;
			}
			
		}

        public override void am_PerformCustomTaskDuringLoadSystem()
        {
            base.am_PerformCustomTaskDuringLoadSystem();

            Utilities.MessageConsole = ap_MessageConsole;
        }

		protected override void am_SetUpTaskPanes()
		{
			base.am_SetUpTaskPanes();
			AcceleratorTaskPane Accelerator = new AcceleratorTaskPane();
			am_EnrollTaskPane(Accelerator, "ACCELERATORTASKPANE", Dock.Bottom, false);
			HeadquartersTaskPane headQuarters = new HeadquartersTaskPane();
			am_EnrollTaskPane(headQuarters, "HEADQUARTERSTASKPANE", Dock.Bottom, false);
			SurroundTaskPane Surround = new SurroundTaskPane();
			am_EnrollTaskPane(Surround, "SURROUNDTASKPANE", Dock.Bottom, false);
		}
	}
}