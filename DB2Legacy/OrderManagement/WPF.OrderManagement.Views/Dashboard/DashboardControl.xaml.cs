using A4DN.Core.WPF.Base;
using A4DN.Core.WPF.Base.DataPresentation.Dashboard;

namespace WPF.Dashboard
{
	/// <summary>
	/// Interaction logic for OperationalDashboardControl.xaml
	/// </summary>
	public partial class DashboardControl : AB_DashboardModuleExplorer
	{
		public DashboardControl() : this(null)
		{

		}

		public DashboardControl(AB_VisualComponentInitializationArgs InitArgs) : base(InitArgs)
		{
			InitializeComponent();
		}

		protected override void am_SetParentProperties()
		{
			base.am_SetParentProperties();
			base.ap_ContentType = AB_ContentType.HeaderBarOnly;
		}
	}
}