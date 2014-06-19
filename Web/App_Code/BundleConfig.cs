using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace Site.App_Code
{
    public class BundleConfig
    {
        public static void registerBundles(BundleCollection bundles)
        {
            var nullBuilder = new NullBuilder();
            var cssTransformer = new CssTransformer();
            IBundleTransform jsTransformer = new JsTransformer(new BundleTransformer.Yui.Minifiers.YuiJsMinifier());
            var nullOrderer = new NullOrderer();

            //Styles
            var bundleStyle = new Bundle("~/Design/Styles/base", new CssMinify())
                .Include("~/Design/Styles/style.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/jquery-hint.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/yl-modal.css", new CssRewriteUrlTransform());

            //Styles (Bootstrap)
            var bundleStyleBootstrap = new Bundle("~/Design/Styles/Bootstrap/bootstrap", new CssMinify())
                .Include("~/Design/Styles/Bootstrap/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/Bootstrap/bootstrap-theme.css", new CssRewriteUrlTransform())
                .Include("~/Scripts/Bootstrap/rs-plugin/css/settings.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/Bootstrap/style.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/Bootstrap/font-awesome.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/Bootstrap/media.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/Bootstrap/uniform.default.css", new CssRewriteUrlTransform());

            var bundleDatatables = new Bundle("~/Design/Styles/Datatables/datatables", new CssMinify())
                .Include("~/Design/Styles/Datatables/DT_bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/Datatables/responsive-tables.css", new CssRewriteUrlTransform());

            var bundleStyleValidate = new Bundle("~/Design/Styles/Validate/validate", new CssMinify())
                .Include("~/Design/Styles/Validate/validationEngine.jquery.css", new CssRewriteUrlTransform());


            //Javascript
            var js = new Bundle("~/Scripts/base", jsTransformer)
                .Include("~/Scripts/main.js");

            //Script de Jquery
            var jsJQuery = new Bundle("~/Scripts/jquery", jsTransformer)
                .Include("~/Scripts/jquery-{version}.js");

            var jsJQueryUI = new Bundle("~/Scripts/jqueryui", jsTransformer)
                .Include("~/Scripts/jquery-ui.js");

            //Plugins
            var jsPlugins = new Bundle("~/Scripts/plugins", jsTransformer)
                .Include("~/Scripts/jquery-jeditable.js")
                .Include("~/Scripts/jquery-sortable.js")
                .Include("~/Scripts/jquery-tmpl.js")
                .Include("~/Scripts/jquery-json-{version}.js")
                .Include("~/Scripts/jquery-meiomask.js")
                .Include("~/Scripts/jquery-hint.js")
                .Include("~/Scripts/yl-modal.js")
                .Include("~/Scripts/jquery.uniform.js");

            //Plugins
            var jsBootstrap = new Bundle("~/Scripts/Bootstrap/bootstrap", jsTransformer)
                .Include("~/Scripts/Bootstrap/bootstrap.js");

            //Themepunch do bootstrap
            var jsBootstrapThemepunch = new Bundle("~/Scripts/Bootstrap/rs-plugin/responsive", jsTransformer)
                .Include("~/Scripts/Bootstrap/rs-plugin/js/jquery.themepunch.revolution.js");

            //Plugins do boostraps
            var jsBootstrapPlugin = new Bundle("~/Scripts/Bootstrap/bootstrapplugins", jsTransformer)
                .Include("~/Scripts/Bootstrap/jquery.carouFredSel-6.2.1.js")
                .Include("~/Scripts/Bootstrap/jquery.easy-pie-chart.js")
                .Include("~/Scripts/Bootstrap/library.js")
                .Include("~/Scripts/Bootstrap/function.js");


            //DataTables
            var jsDataTables = new Bundle("~/Scripts/Datatables/datatables", jsTransformer)
                .Include("~/Scripts/Datatables/jquery.tablesorter.min.js")
                .Include("~/Scripts/Datatables/jquery.dataTables.js")
                .Include("~/Scripts/Datatables/DT_bootstrap.js")
                .Include("~/Scripts/Datatables/responsive-tables.js");


            var jsValidate = new Bundle("~/Scripts/Validate/validate", jsTransformer)
                .Include("~/Scripts/Validate/jquery.validate.js")
                .Include("~/Scripts/Validate/jquery.validationEngine.js")
                .Include("~/Scripts/Validate/jquery.validationEngine-pt_BR.js");

#if DEBUG
            //Remove o minify caso esteja em modo debug
            bundleStyle.Transforms.Clear();
            bundleStyleBootstrap.Transforms.Clear();
            bundleStyleValidate.Transforms.Clear();
            bundleDatatables.Transforms.Clear();

            js.Transforms.Clear();
            jsJQuery.Transforms.Clear();
            jsPlugins.Transforms.Clear();
            jsJQueryUI.Transforms.Clear();
            jsBootstrap.Transforms.Clear();
            jsBootstrapThemepunch.Transforms.Clear();
            jsBootstrapPlugin.Transforms.Clear();
            jsValidate.Transforms.Clear();
            jsDataTables.Transforms.Clear();
#endif

            bundles.Add(bundleStyle);
            bundles.Add(bundleStyleBootstrap);
            bundles.Add(bundleStyleValidate);
            bundles.Add(bundleDatatables);

            bundles.Add(js);
            bundles.Add(jsJQuery);
            bundles.Add(jsPlugins);
            bundles.Add(jsJQueryUI);
            bundles.Add(jsBootstrap);
            bundles.Add(jsBootstrapThemepunch);
            bundles.Add(jsBootstrapPlugin);
            bundles.Add(jsValidate);
            bundles.Add(jsDataTables);
        }
    }
}