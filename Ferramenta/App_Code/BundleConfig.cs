using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace Ferramenta.App_Code
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
            var cssBase = new Bundle("~/Design/Styles/base", new CssMinify())
            .Include("~/Design/Styles/style_6in1.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/style.css", new CssRewriteUrlTransform());

            var cssPlugin = new Bundle("~/Design/Styles/plugins", new CssMinify())
                .Include("~/Design/Styles/yl-modal.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/jquery-toggle-buttons.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/jquery-hint.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/bootstrap-tagsinput.css", new CssRewriteUrlTransform());

            var cssBootstrap = new Bundle("~/Design/Styles/Bootstrap/bootstrap", new CssMinify())
                .Include("~/Design/Styles/Bootstrap/bootstrap.css")
                .Include("~/Design/Styles/Bootstrap/bootstrap-responsive.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/Bootstrap/uniform.default.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/Bootstrap/proggress.css", new CssRewriteUrlTransform());


            var cssCalendar = new Bundle("~/Design/Styles/Calendar/calendar", new CssMinify())
                .Include("~/Design/Styles/Calendar/fullcalendar.css", new CssRewriteUrlTransform());

            var cssDatatables = new Bundle("~/Design/Styles/Datatables/datatables", new CssMinify())
                .Include("~/Design/Styles/Datatables/DT_bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/Datatables/responsive-tables.css", new CssRewriteUrlTransform());

            var cssTEditor = new Bundle("~/Design/Styles/TEditor/editor", new CssMinify())
               .Include("~/Design/Styles/TEditor/style.css", new CssRewriteUrlTransform());

            var cssValidate = new Bundle("~/Design/Styles/Validate/validate", new CssMinify())
               .Include("~/Design/Styles/Validate/validationEngine.jquery.css", new CssRewriteUrlTransform());

            var cssDatePicker = new Bundle("~/Design/Styles/DatePicker/datepicker", new CssMinify())
                .Include("~/Design/Styles/DatePicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Design/Styles/DatePicker/datepicker.css", new CssRewriteUrlTransform());


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
                .Include("~/Scripts/jquery-tmpl.js")
                .Include("~/Scripts/jquery-json-{version}.js")
                .Include("~/Scripts/jquery-hint.js")
                .Include("~/Scripts/jquery.uniform.js")
                .Include("~/Scripts/jquery.toggle.buttons.js")
                .Include("~/Scripts/yl-modal.js")
                .Include("~/Scripts/jquery-meiomask.js")
                .Include("~/Scripts/bootstrap-tagsinput.js")
                .Include("~/Scripts/jquery.cookie.js");

            //Bootstrap
            var jsBootstrap = new Bundle("~/Scripts/Bootstrap/bootstrap", jsTransformer)
                .Include("~/Scripts/Bootstrap/bootstrap.js")
                .Include("~/Scripts/Bootstrap/main.js");

            //Calendar
            var jsCalendar = new Bundle("~/Scripts/Calendar/calendar", jsTransformer)
                .Include("~/Scripts/Calendar/fullcalendar.js");

            //DataTables
            var jsDataTables = new Bundle("~/Scripts/Datatables/datatables", jsTransformer)
                .Include("~/Scripts/Datatables/jquery.tablesorter.min.js")
                .Include("~/Scripts/Datatables/jquery.dataTables.js")
                .Include("~/Scripts/Datatables/DT_bootstrap.js")
                .Include("~/Scripts/Datatables/responsive-tables.js");

            //Flot
            var jsFlot = new Bundle("~/Scripts/Flot/flot", jsTransformer)
            .Include("~/Scripts/Flot/jquery.flot.js")
            .Include("~/Scripts/Flot/jquery.colorhelpers.js")
            .Include("~/Scripts/Flot/jquery.easy-pie-chart.js")
            .Include("~/Scripts/Flot/jquery.flot.crosshair.js")
            .Include("~/Scripts/Flot/jquery.flot.fillbetween.js")
            .Include("~/Scripts/Flot/jquery.flot.image.js")
            .Include("~/Scripts/Flot/jquery.flot.navigate.js")
            .Include("~/Scripts/Flot/jquery.flot.pie.js")
            .Include("~/Scripts/Flot/jquery.flot.resize.js")
            .Include("~/Scripts/Flot/jquery.flot.selection.js")
            .Include("~/Scripts/Flot/jquery.flot.stack.js")
            .Include("~/Scripts/Flot/jquery.flot.symbol.js")
            .Include("~/Scripts/Flot/jquery.flot.threshold.js")
            .Include("~/Scripts/Flot/jquery.sparkline.min.js")
            .Include("~/Scripts/Flot/excanvas.js");

            //MarkitUp
            var jsMarkitup = new Bundle("~/Scripts/Markitup/editor", jsTransformer)
                .Include("~/Scripts/Markitup/*.js");

            var jsTEditor = new Bundle("~/Scripts/TEditor/editor", jsTransformer)
                .Include("~/Scripts/TEditor/jquery-te-1.4.0.js");

            var jsValidate = new Bundle("~/Scripts/Validate/validate", jsTransformer)
                .Include("~/Scripts/Validate/jquery.validate.js")
                .Include("~/Scripts/Validate/jquery.validationEngine.js")
                .Include("~/Scripts/Validate/jquery.validationEngine-pt_BR.js");

            var jsDatePicker = new Bundle("~/Scripts/DatePicker/datepicker", jsTransformer)
                .Include("~/Scripts/DatePicker/date.js")
                .Include("~/Scripts/DatePicker/jquery-daterangepicker.js")
                .Include("~/Scripts/DatePicker/jquery-datepicker.js");

#if DEBUG
            //Remove o minify caso esteja em modo debug
            cssBase.Transforms.Clear();
            cssPlugin.Transforms.Clear();
            cssBootstrap.Transforms.Clear();
            cssCalendar.Transforms.Clear();
            cssDatatables.Transforms.Clear();
            cssTEditor.Transforms.Clear();
            cssValidate.Transforms.Clear();
            cssDatePicker.Transforms.Clear();

            js.Transforms.Clear();
            jsJQuery.Transforms.Clear();
            jsPlugins.Transforms.Clear();
            jsJQueryUI.Transforms.Clear();
            jsBootstrap.Transforms.Clear();
            jsCalendar.Transforms.Clear();
            jsDataTables.Transforms.Clear();
            jsFlot.Transforms.Clear();
            jsMarkitup.Transforms.Clear();
            jsTEditor.Transforms.Clear();
            jsValidate.Transforms.Clear();
            jsDatePicker.Transforms.Clear();

#endif

            bundles.Add(cssBase);
            bundles.Add(cssPlugin);
            bundles.Add(cssBootstrap);
            bundles.Add(cssCalendar);
            bundles.Add(cssDatatables);
            bundles.Add(cssTEditor);
            bundles.Add(cssValidate);
            bundles.Add(cssDatePicker);

            bundles.Add(js);
            bundles.Add(jsJQuery);
            bundles.Add(jsPlugins);
            bundles.Add(jsJQueryUI);
            bundles.Add(jsBootstrap);
            bundles.Add(jsCalendar);
            bundles.Add(jsDataTables);
            bundles.Add(jsFlot);
            bundles.Add(jsMarkitup);
            bundles.Add(jsTEditor);
            bundles.Add(jsValidate);
            bundles.Add(jsDatePicker);
        }
    }
}