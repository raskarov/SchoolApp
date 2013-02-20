using System.Web;
using System.Web.Optimization;

namespace SchoolApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-migrate-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/noty").Include("~/Scripts/jquery.noty*"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                    "~/Scripts/fullcalendar*"));
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                "~/Scripts/Site.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                    "~/scripts/jquery.dataTables*",
                    "~/scripts/datatables.bootstrap*"));

            bundles.Add(new ScriptBundle("~/bundles/multi-select").Include("~/scripts/jquery.multi-select*"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/custom.css", "~/Content/theme.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
            bundles.Add(new StyleBundle("~/Content/font-awesome").Include("~/Content/font-awesome*"));
            bundles.Add(new StyleBundle("~/Content/multi-select").Include("~/Content/multi-select*"));
            bundles.Add(new StyleBundle("~/Content/fullcalendar").Include("~/Content/fullcalendar.css"));
            bundles.Add(new StyleBundle("~/Content/datatables").Include("~/content/datatables.bootstrap.css"));
        }
    }
}