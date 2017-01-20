using System.Web;
using System.Web.Optimization;

namespace ParrotPaymentsApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-route.js"));

            bundles.Add(new ScriptBundle("~/bundles/ngBackbone").Include(
                        "~/ng-backbone/Module.js",
                        "~/ng-backbone/Service.js",
                        "~/ng-backbone/Controller.js"));

            
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-toggle.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/alertify").Include(
                      "~/Scripts/alertify.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/custom.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap-toggle.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/alertifyjs/alertify.css",
                      "~/Content/alertifyjs/themes/default.css",
                      "~/Content/site.css"));
        }
    }
}
