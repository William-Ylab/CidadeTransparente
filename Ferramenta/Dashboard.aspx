<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Ferramenta.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="Design/Styles/Bootstrap/proggress.css">
    <link type="text/css" rel="stylesheet" href="Design/Styles/Datatables/DT_bootstrap.css" />
    <link type="text/css" rel="stylesheet" href="Design/Styles/Datatables/responsive-tables.css">
    <link type="text/css" rel="stylesheet" href="Design/Styles/Calendar/fullcalendar.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid outer">
        <div class="row-fluid">
            <!-- .inner -->
            <div class="span12 inner">
                <div class="tac">
                    <ul class="stats_box">
                        <li>
                            <div class="sparkline bar_week"></div>
                            <div class="stat_text">
                                <strong>2.345</strong>Weekly Visit
                                            <span class="percent down"><i class="icon-caret-down"></i>-16%</span>
                            </div>
                        </li>
                        <li>
                            <div class="sparkline line_day"></div>
                            <div class="stat_text">
                                <strong>165</strong>Daily Visit
                                            <span class="percent up"><i class="icon-caret-up"></i>+23%</span>
                            </div>
                        </li>
                        <li>
                            <div class="sparkline pie_week"></div>
                            <div class="stat_text">
                                <strong>$2 345.00</strong>Weekly Sale
                                            <span class="percent">0%</span>
                            </div>
                        </li>
                        <li>
                            <div class="sparkline stacked_month"></div>
                            <div class="stat_text">
                                <strong>$678.00</strong>Monthly Sale
                                            <span class="percent down"><i class="icon-caret-down"></i>-10%</span>
                            </div>
                        </li>
                    </ul>
                </div>
                <hr>
                <div class="tac">
                    <ul class="stats_box">
                        <li>
                            <div class="semicircle-progressbar-span">
                                <input data-readonly="true" data-fgcolor="#f1c40f" data-inputcolor="#333" data-width="120" data-height="60" rel="16" value="0" readonly data-anglearc="180" data-angleoffset="-90" data-max="100" />
                            </div>
                            <div class="stat_text">
                                <strong>2.345</strong>Weekly Visit (16%)
                            </div>
                        </li>
                        <li>
                            <div class="semicircle-progressbar-span">
                                <input data-readonly="true" data-fgcolor="#2773ae" data-inputcolor="#333" data-width="120" data-height="60" rel="23" value="0" readonly data-anglearc="180" data-angleoffset="-90" data-max="100" />
                            </div>
                            <div class="stat_text">
                                <strong>165</strong>Daily Visit (23%)
                            </div>
                        </li>
                        <li>
                            <div class="semicircle-progressbar-span">
                                <input data-readonly="true" data-fgcolor="#42c114" data-inputcolor="#333" data-width="120" data-height="60" rel="60" value="0" readonly data-anglearc="180" data-angleoffset="-90" data-max="100" />
                            </div>
                            <div class="stat_text">
                                <strong>$2 345.00</strong>Weekly Sale (60%)
                            </div>
                        </li>
                        <li>
                            <div class="semicircle-progressbar-span">
                                <input data-readonly="true" data-fgcolor="#6f31f6" data-inputcolor="#333" data-width="120" data-height="60" rel="80" value="0" readonly data-anglearc="180" data-angleoffset="-90" data-max="100" />
                            </div>
                            <div class="stat_text">
                                <strong>$678.00</strong>Monthly Sale (80%)
                            </div>
                        </li>
                    </ul>
                </div>
                <hr>
                <div class="tac">
                    <ul class="stats_box">
                        <li>
                            <div class="easy-pie-chart percentage" data-percent="16" data-color="#D15B47" data-line="20" data-cap="butt">
                                <span class="percent">16</span>
                                %
                            </div>
                            <div class="stat_text">
                                <strong>2.345</strong>Weekly Visit (16%)
                            </div>
                        </li>
                        <li>
                            <div class="easy-pie-chart percentage" data-percent="23" data-color="#ffc100" data-line="23" data-cap="butt">
                                <span class="percent">23</span>
                                %
                            </div>
                            <div class="stat_text">
                                <strong>165</strong>Daily Visit (23%)
                            </div>
                        </li>
                        <li>
                            <div class="easy-pie-chart percentage" data-percent="60" data-color="#5caf91" data-line="23" data-cap="butt">
                                <span class="percent">60</span>
                                %
                            </div>
                            <div class="stat_text">
                                <strong>$2 345.00</strong>Weekly Sale (60%)
                            </div>
                        </li>
                        <li>
                            <div class="easy-pie-chart percentage" data-percent="80" data-color="#e93760" data-line="23" data-cap="butt">
                                <span class="percent">80</span>
                                %
                            </div>
                            <div class="stat_text">
                                <strong>$678.00</strong>Monthly Sale (80%)
                            </div>
                        </li>
                    </ul>
                </div>
                <hr>
                <div class="row-fluid">
                    <div class="span12">
                        <div class="box">
                            <header>
                                <h5>Line Chart</h5>
                            </header>
                            <div class="body" id="trigo" style="height: 250px;"></div>
                        </div>
                    </div>
                </div>
                <hr>
                <div class="row-fluid">
                    <div class="span12">
                        <div class="box">
                            <header>
                                <h5>Calendar</h5>
                            </header>
                            <div id="calendar_content" class="body">
                                <div id='calendar'></div>
                            </div>
                        </div>
                    </div>
                </div>
                <!--BEGIN LATEST COMMENT-->
                <!-- .row-fluid -->
                <div class="row-fluid">
                    <!-- .span6 -->
                    <div class="span6">
                        <!-- .box -->
                        <div class="box comments">
                            <header>
                                <div class="icons">
                                    <i class="fa fa-comments"></i>
                                </div>
                                <h5>Latest Comment</h5>
                            </header>
                            <!-- .body -->
                            <div class="body">
                                <div class="media">
                                    <a href="#" class="pull-left">
                                        <img data-src="holder.js/64x64" class="media-object" alt="64x64" style="width: 64px; height: 64px;" src="http://bizstrap.themeleaf.com/admin/assets/img/charles.jpg">
                                    </a>
                                    <div class="media-body">
                                        <div class="popover right">
                                            <div class="arrow"></div>
                                            <h3 class="popover-title">Popover right</h3>
                                            <div class="popover-content">
                                                <p>Sed posuere consectetur est at lobortis. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="media">
                                    <a href="#" class="pull-right">
                                        <img data-src="holder.js/64x64" class="media-object" alt="64x64" style="width: 64px; height: 64px;" src="http://bizstrap.themeleaf.com/admin/assets/img/lucy.jpg">
                                    </a>
                                    <div class="media-body">
                                        <div class="popover left">
                                            <div class="arrow"></div>
                                            <h3 class="popover-title">Popover right</h3>
                                            <div class="popover-content">
                                                <p>Sed posuere consectetur est at lobortis. Aenean eu leo quam. Pellentesque ornare sem lacinia quam venenatis vestibulum.</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- /.body -->
                        </div>
                        <!-- /.box -->
                    </div>
                    <!-- /.span6 -->
                    <!-- .span6 -->
                    <div class="span6">
                        <div class="box">
                            <header>
                                <div class="icons"><i class="icon-move"></i></div>
                                <h5>Dynamic Table</h5>
                            </header>
                            <div id="collapse4" class="body">
                                <table id="dataTable" class="table table-bordered table-condensed table-hover table-striped">
                                    <thead>
                                        <tr>
                                            <th>Rendering engine</th>
                                            <th>Browser</th>
                                            <th>Platform(s)</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Trident</td>
                                            <td>Internet
                                                            Explorer
                                                            4.0
                                            </td>
                                            <td>Win 95+</td>
                                        </tr>
                                        <tr>
                                            <td>Trident</td>
                                            <td>Internet
                                                            Explorer 5.0
                                            </td>
                                            <td>Win 95+</td>
                                        </tr>
                                        <tr>
                                            <td>Trident</td>
                                            <td>Internet
                                                            Explorer 5.5
                                            </td>
                                            <td>Win 95+</td>
                                        </tr>
                                        <tr>
                                            <td>Trident</td>
                                            <td>Internet
                                                            Explorer 6
                                            </td>
                                            <td>Win 98+</td>
                                        </tr>
                                        <tr>
                                            <td>Trident</td>
                                            <td>Internet Explorer 7</td>
                                            <td>Win XP SP2+</td>
                                        </tr>
                                        <tr>
                                            <td>Trident</td>
                                            <td>AOL browser (AOL desktop)</td>
                                            <td>Win XP</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Firefox 1.0</td>
                                            <td>Win 98+ / OSX.2+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Firefox 1.5</td>
                                            <td>Win 98+ / OSX.2+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Firefox 2.0</td>
                                            <td>Win 98+ / OSX.2+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Firefox 3.0</td>
                                            <td>Win 2k+ / OSX.3+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Camino 1.0</td>
                                            <td>OSX.2+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Camino 1.5</td>
                                            <td>OSX.3+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Netscape 7.2</td>
                                            <td>Win 95+ / Mac OS 8.6-9.2</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Netscape Browser 8</td>
                                            <td>Win 98SE+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Netscape Navigator 9</td>
                                            <td>Win 98+ / OSX.2+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Mozilla 1.0</td>
                                            <td>Win 95+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Mozilla 1.1</td>
                                            <td>Win 95+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Mozilla 1.2</td>
                                            <td>Win 95+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Mozilla 1.3</td>
                                            <td>Win 95+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Mozilla 1.4</td>
                                            <td>Win 95+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Mozilla 1.5</td>
                                            <td>Win 95+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Mozilla 1.6</td>
                                            <td>Win 95+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Mozilla 1.7</td>
                                            <td>Win 98+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Mozilla 1.8</td>
                                            <td>Win 98+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Seamonkey 1.1</td>
                                            <td>Win 98+ / OSX.2+</td>
                                        </tr>
                                        <tr>
                                            <td>Gecko</td>
                                            <td>Epiphany 2.20</td>
                                            <td>Gnome</td>
                                        </tr>
                                        <tr>
                                            <td>Webkit</td>
                                            <td>Safari 1.2</td>
                                            <td>OSX.3</td>
                                        </tr>
                                        <tr>
                                            <td>Webkit</td>
                                            <td>Safari 1.3</td>
                                            <td>OSX.3</td>
                                        </tr>
                                        <tr>
                                            <td>Webkit</td>
                                            <td>Safari 2.0</td>
                                            <td>OSX.4+</td>
                                        </tr>
                                        <tr>
                                            <td>Webkit</td>
                                            <td>Safari 3.0</td>
                                            <td>OSX.4+</td>
                                        </tr>
                                        <tr>
                                            <td>Webkit</td>
                                            <td>OmniWeb 5.5</td>
                                            <td>OSX.4+</td>
                                        </tr>
                                        <tr>
                                            <td>Webkit</td>
                                            <td>iPod Touch / iPhone</td>
                                            <td>iPod</td>
                                        </tr>
                                        <tr>
                                            <td>Webkit</td>
                                            <td>S60</td>
                                            <td>S60</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Opera 7.0</td>
                                            <td>Win 95+ / OSX.1+</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Opera 7.5</td>
                                            <td>Win 95+ / OSX.2+</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Opera 8.0</td>
                                            <td>Win 95+ / OSX.2+</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Opera 8.5</td>
                                            <td>Win 95+ / OSX.2+</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Opera 9.0</td>
                                            <td>Win 95+ / OSX.3+</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Opera 9.2</td>
                                            <td>Win 88+ / OSX.3+</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Opera 9.5</td>
                                            <td>Win 88+ / OSX.3+</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Opera for Wii</td>
                                            <td>Wii</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Nokia N800</td>
                                            <td>N800</td>
                                        </tr>
                                        <tr>
                                            <td>Presto</td>
                                            <td>Nintendo DS browser</td>
                                            <td>Nintendo DS</td>
                                        </tr>
                                        <tr>
                                            <td>KHTML</td>
                                            <td>Konqureror 3.1</td>
                                            <td>KDE 3.1</td>
                                        </tr>
                                        <tr>
                                            <td>KHTML</td>
                                            <td>Konqureror 3.3</td>
                                            <td>KDE 3.3</td>
                                        </tr>
                                        <tr>
                                            <td>KHTML</td>
                                            <td>Konqureror 3.5</td>
                                            <td>KDE 3.5</td>
                                        </tr>
                                        <tr>
                                            <td>Tasman</td>
                                            <td>Internet Explorer 4.5</td>
                                            <td>Mac OS 8-9</td>
                                        </tr>
                                        <tr>
                                            <td>Tasman</td>
                                            <td>Internet Explorer 5.1</td>
                                            <td>Mac OS 7.6-9</td>
                                        </tr>
                                        <tr>
                                            <td>Tasman</td>
                                            <td>Internet Explorer 5.2</td>
                                            <td>Mac OS 8-X</td>
                                        </tr>
                                        <tr>
                                            <td>Misc</td>
                                            <td>NetFront 3.1</td>
                                            <td>Embedded devices</td>
                                        </tr>
                                        <tr>
                                            <td>Misc</td>
                                            <td>NetFront 3.4</td>
                                            <td>Embedded devices</td>
                                        </tr>
                                        <tr>
                                            <td>Misc</td>
                                            <td>Dillo 0.8</td>
                                            <td>Embedded devices</td>
                                        </tr>
                                        <tr>
                                            <td>Misc</td>
                                            <td>Links</td>
                                            <td>Text only</td>
                                        </tr>
                                        <tr>
                                            <td>Misc</td>
                                            <td>Lynx</td>
                                            <td>Text only</td>
                                        </tr>
                                        <tr>
                                            <td>Misc</td>
                                            <td>IE Mobile</td>
                                            <td>Windows Mobile 6</td>
                                        </tr>
                                        <tr>
                                            <td>Misc</td>
                                            <td>PSP browser</td>
                                            <td>PSP</td>
                                        </tr>
                                        <tr>
                                            <td>Other browsers</td>
                                            <td>All others</td>
                                            <td>-</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <!-- /.span6 -->
                </div>
                <!-- /.row-fluid -->
                <!--END LATEST COMMENT-->
            </div>
            <!-- /.inner -->
        </div>
    </div>

<%--    <script src="Scripts/Flot/jquery.flot.js"></script>
    <script src="Scripts/Flot/jquery.flot.pie.js"></script>
    <script src="Scripts/Flot/jquery.flot.selection.js"></script>
    <script src="Scripts/Flot/jquery.flot.resize.js"></script>
    <script src="Scripts/Flot/jquery.sparkline.min.js"></script>

    <script type="text/javascript" src="Scripts/Datatables/jquery.dataTables.js"></script>
    <script type="text/javascript" src="Scripts/Datatables/jquery.tablesorter.min.js"></script>
    <script type="text/javascript" src="Scripts/Datatables/responsive-tables.js"></script>
    <script type="text/javascript" src="Scripts/Datatables/DT_bootstrap.js"></script>

    <script src="Scripts/Flot/jquery.sparkline.min.js"></script>
     <script src="Scripts/Flot/jquery.easy-pie-chart.min.js"></script>

    <script src="Scripts/Calendar/fullcalendar.js"></script>--%>

    <%--<script src="assets/js/lib/jquery.mousewheel.js"></script>
    <script src="assets/js/lib/jquery.sparkline.min.js"></script>
    <script src="assets/js/lib/flot/jquery.flot.js"></script>
    <script src="assets/js/lib/flot/jquery.flot.pie.js"></script>
    <script src="assets/js/lib/flot/jquery.flot.selection.js"></script>
    <script src="assets/js/lib/flot/jquery.flot.resize.js"></script>
    <script src="assets/js/waypoints.min.js"></script>
    <script src="assets/js/jquery.knob.js"></script>
    <script src="assets/js/jquery.easy-pie-chart.min.js"></script>
    <script src="assets/js/lib/fullcalendar.min.js"></script>


    <script type="text/javascript" src="assets/js/lib/jquery.tablesorter.min.js"></script>
    <script type="text/javascript" src="assets/js/lib/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/lib/DT_bootstrap.js"></script>
    <script src="assets/js/lib/responsive-tables.js"></script>--%>
    <script type="text/javascript">
        jQuery(function () {
            bizstrapTable();
            dashboard();


            var oldie = jQuery.browser.msie && jQuery.browser.version < 9;

            jQuery('.easy-pie-chart.percentage').each(function () {
                jQuery(this).easyPieChart({
                    barColor: jQuery(this).data('color'),
                    trackColor: '#ffffff',
                    scaleColor: false,
                    lineCap: jQuery(this).data('cap'),
                    lineWidth: jQuery(this).data('line'),
                    animate: oldie ? false : 3000,
                    size: 120
                }).css('color', jQuery(this).data('color'));
            });


            //// Triggering only when it is inside viewport
            //jQuery('.semicircle-progressbar-span input').waypoint(function () {
            //    var cur = jQuery(this);
            //    // Triggering now
            //    cur.knob();
            //    // Animating the value
            //    if (cur.val() == 0) {
            //        var val = cur.attr("rel");
            //        if (val < 0) val = val - 1;
            //        jQuery({ value: 0 }).animate({ value: val }, {
            //            duration: 3000,
            //            easing: 'swing',
            //            step: function () {
            //                cur.val(Math.ceil(this.value)).trigger('change');
            //            }
            //        })
            //    }
            //}
            //    , {
            //        triggerOnce: true,
            //        offset: function () {
            //            return jQuery(window).height() - jQuery(this).outerHeight();
            //        }
            //    }
            //);
        });
    </script>

    <script src="Scripts/Bootstrap/main.js"></script>

    <%--<script type="text/javascript" src="../js/style-switcher/style-switcher.js"></script>--%>

</asp:Content>
