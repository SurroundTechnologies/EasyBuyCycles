//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="MVC.System.Global.asax.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System.Web;
using System.Web.Optimization;

namespace MVC.OrderManagement
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/content/smartadmin").IncludeDirectory("~/content/css", "*.min.css"));
			
			bundles.Add(new StyleBundle("~/content/a4dn-css").IncludeDirectory("~/content/css", "a4dn.css"));

			bundles.Add(new ScriptBundle("~/scripts/a4dn-js").Include(
				"~/scripts/a4dn/jquery.encapsulatedPlugin.js",
				"~/scripts/a4dn/a4dn.*"
				));

			bundles.Add(new ScriptBundle("~/scripts/splitter").Include(
				"~/scripts/a4dn/jquery-splitter/jquery-2.1.4.min.js", 
				"~/scripts/a4dn/jquery-splitter/splitter.js"
				));

			bundles.Add(new ScriptBundle("~/scripts/jqueryval").Include(
					   "~/Scripts/a4dn/jquery-validate/*.min.js"));

			bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
				"~/scripts/app.config.js",
				"~/scripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
				"~/scripts/bootstrap/bootstrap.min.js",
				"~/scripts/bootstrap/bootstrap-switch.min.js",
				"~/scripts/notification/SmartNotification.min.js",
				"~/scripts/smartwidgets/jarvis.widget.min.js",
				"~/scripts/plugin/jquery-validate/jquery.validate.min.js",
				"~/scripts/plugin/masked-input/jquery.maskedinput.min.js",
				"~/scripts/plugin/select2/select2.min.js",
				"~/scripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
				"~/scripts/plugin/bootstrap-progressbar/bootstrap-progressbar.min.js",
				"~/scripts/plugin/msie-fix/jquery.mb.browser.min.js",
				"~/scripts/plugin/fastclick/fastclick.min.js",
				"~/scripts/app.min.js"
				));

			bundles.Add(new ScriptBundle("~/scripts/full-calendar").Include(
				"~/scripts/plugin/moment/moment.min.js",
				"~/scripts/plugin/fullcalendar/jquery.fullcalendar.min.js"
				));

			bundles.Add(new ScriptBundle("~/scripts/charts").Include(
				"~/scripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
				"~/scripts/plugin/sparkline/jquery.sparkline.min.js",
				"~/scripts/plugin/morris/morris.min.js",
				"~/scripts/plugin/morris/raphael.min.js",
				"~/scripts/plugin/flot/jquery.flot.cust.min.js",
				"~/scripts/plugin/flot/jquery.flot.resize.min.js",
				"~/scripts/plugin/flot/jquery.flot.time.min.js",
				"~/scripts/plugin/flot/jquery.flot.fillbetween.min.js",
				"~/scripts/plugin/flot/jquery.flot.orderBar.min.js",
				"~/scripts/plugin/flot/jquery.flot.pie.min.js",
				"~/scripts/plugin/flot/jquery.flot.tooltip.min.js",
				"~/scripts/plugin/dygraphs/dygraph-combined.min.js",
				"~/scripts/plugin/chartjs/chart.min.js",
				"~/scripts/moment.min.js"
				));

			bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
				"~/scripts/plugin/datatables/jquery.dataTables.min.js",
				"~/scripts/plugin/datatables/dataTables.colVis.min.js",
				"~/scripts/plugin/datatables/dataTables.tableTools.min.js",
				"~/scripts/plugin/datatables/dataTables.bootstrap.min.js",
				"~/scripts/plugin/datatable-responsive/datatables.responsive.min.js"
				));

			bundles.Add(new ScriptBundle("~/scripts/jq-grid").Include(
				"~/scripts/plugin/jqgrid/jquery.jqGrid.min.js",
				"~/scripts/plugin/jqgrid/grid.locale-en.min.js"
				));

			bundles.Add(new ScriptBundle("~/scripts/forms").Include(
				"~/scripts/plugin/jquery-form/jquery-form.min.js"
				));

			bundles.Add(new ScriptBundle("~/scripts/smart-chat").Include(
				"~/scripts/smart-chat-ui/smart.chat.ui.min.js",
				"~/scripts/smart-chat-ui/smart.chat.manager.min.js"
				));

			bundles.Add(new ScriptBundle("~/scripts/vector-map").Include(
				"~/scripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
				"~/scripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js"
				));

			bundles.Add(new StyleBundle("~/content/datetimepicker").Include("~/content/bootstrap-datetimepicker.min.css"));
			bundles.Add(new ScriptBundle("~/scripts/datetimepicker").Include("~/scripts/bootstrap-datetimepicker.min.js"));

			BundleTable.EnableOptimizations = true;
		}
	}
}