using System.Web.Optimization;

namespace SeekMail.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/services").IncludeDirectory("~/app/services", "*.js"));
            bundles.Add(new ScriptBundle("~/bundles/controllers").IncludeDirectory("~/app/controllers", "*.js"));
        }
    }
}