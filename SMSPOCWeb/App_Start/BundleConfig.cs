using System.Web;
using System.Web.Optimization;

namespace SMSPOCWeb
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

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                       "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqgrid").Include(
                      "~/Scripts/free-jqGrid/jquery.jqgrid.min.js",
                      "~/Scripts/free-jqGrid/i18n/grid.locale-en.js")
                      );

            bundles.Add(new ScriptBundle("~/bundles/Role").Include(
                     "~/Scripts/Role.js")
                     );
            bundles.Add(new ScriptBundle("~/bundles/SubscriberRole").Include(
                    "~/Scripts/SubscriberRole.js")
                    );

            bundles.Add(new ScriptBundle("~/bundles/Contact").Include(
                "~/Scripts/Contact.js")
                );
            bundles.Add(new ScriptBundle("~/bundles/Sendsms").Include(
                "~/Scripts/Sendsms.js")
                );
            bundles.Add(new ScriptBundle("~/bundles/Template").Include(
                "~/Scripts/Template.js")
                );
            bundles.Add(new ScriptBundle("~/bundles/Messageutility").Include(
              "~/Scripts/Messageutility.js")
              );
            bundles.Add(new ScriptBundle("~/bundles/MessageHistory").Include(
            "~/Scripts/MessageHistory.js")
            );
            bundles.Add(new ScriptBundle("~/bundles/ExcelUploadStudent").Include(
          "~/Scripts/ExcelUploadStudent.js")
          );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/themes/base/jquery.ui.all.css",
                      "~/Content/ui.jqgrid.css"
                      ));
        }
    }
}
