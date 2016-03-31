using System.Web;
using System.Web.Optimization;

namespace WebAngular
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/WebAngular")
                .Include("~/Scripts/WebAngular.js"));

            BundleTable.EnableOptimizations = true;

        }
    }
}
