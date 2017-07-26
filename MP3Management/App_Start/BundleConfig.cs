using System.Web;
using System.Web.Optimization;

namespace MP3Management
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
            "~/Scripts/angular.js", "~/Scripts/angular-route.js",
            "~/Scripts/angular-animate.js", "~/Scripts/angular-aria.js",
            "~/Scripts/angular-material.js"));

            bundles.Add(new ScriptBundle("~/bundles/AngularCustom").IncludeDirectory(
                "~/app", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                                 "~/Content/bootstrap.css",
                                 "~/Content/angular-material.css",
                                 "~/Content/site.css").Include("~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));
        }
    }
}