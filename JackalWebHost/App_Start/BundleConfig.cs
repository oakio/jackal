using System.Web.Optimization;

namespace JackalWebHost
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundle/styles")
                .Include("~/Content/bootstrap/css/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundle/scripts")
                .Include("~/Scripts/google-analytics.js")
                .Include("~/Scripts/jquery-2.1.3.js")
                .Include("~/Content/bootstrap/js/bootstrap.js"));
        }
    }
}