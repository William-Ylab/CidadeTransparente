<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Site.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
                    <!-- slider start -->
                    <div class="fullwidthbanner-container" id="main_slider">
                        <div class="fullwidthbanner">
                            <ul>
                                <li class="slide3" data-transition="boxfade" data-slotamount="5"  data-masterspeed="300">
                                    <!-- slide 3 main image -->
                                    <img alt="" src="/Design/Images/Bootstrap/slide2.jpg" />
                                    <div class="caption sft" data-x="15" data-y="75" data-speed="2500" data-start="1000" data-easing="easeInBack">
                                        <!-- slide 3 caption 1 -->
                                        <p class="cap-1">Governo transparente.</p>
                                    </div>
                                    <div class="caption sfl" data-x="15" data-y="115" data-speed="1500" data-start="1000" data-easing="easeInBack">
                                        <!-- slide 3 caption 2 -->
                                        <p class="cap-2"><span>Responda já ao questionário</span><br />
                                            <span>e ajude a melhorar o seu país!</span>
                                        </p>
                                    </div>
                                    <div class="caption sfl" data-x="15" data-y="235" data-speed="1500" data-start="1000" data-easing="easeInBack">
                                        <!-- slide 3 caption 3 -->
                                        <p class="cap-3">Lorem ipsum dolor sit amet, consectetur<br /> 
                                            adipiscing elit. Integer lorem quam, adipiscing condimentum tristique ve,<br />
                                            hicula, lacus ut suscipit fermentum, turpis .</p>
                                    </div>
                                    <div class="caption sfb" data-x="480" data-y="75" data-speed="2000" data-start="1000" data-easing="easeInBack">
                                        <!-- slide 3 layer image 1 -->
                                        <img src="/Design/Images/Bootstrap/devices.png" alt="" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <!-- /end slider -->

    <div class="container">
       <!-- start about us -->
        <div id="about_marker"></div>
        <div class="mod row" id="about_block">
            <!-- column 1 caption -->
            <div class="col-md-12 col-xs-12 col-sm-12">
                <div class="caption styler_color">Governo transparente</div>
            </div>
            <div class="col-md-6 col-xs-12 col-sm-6">
                <div class="article">
                    <!-- column 1 headline -->
                    <h1>O QUE É?</h1>
                    <!-- column 1 text -->
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer lorem quam, adipiscing condimentum tristique vel, eleifend sed turpis. Curabitur pellentesque massa eu nulla consequat sed porttitor arcu porttitor.</p>
                    <p>Quisque volutpat pharetra felis, eu cursus lorem molestie vitae. Nulla vehicula, lacus ut suscipit fermentum, turpis felis ultricies dui, ut rhoncus libero augue at libero. Morbi ut arcu dolor. Maecenas id nulla nec nibh viverra vehicula. Cras feugiat, magna eu lacinia ullamcorper, augue est sodales nibh, ut vulputate augue est sed nunc. Suspendisse sagittist.</p>
                    <p>Maecenas id nulla nec nibh viverra vehicula. Cras feugiat, magna eu lacinia ullamcorper, augue est sodales nibh, ut vulputate augue est sed nunc. Suspendisse sagittist.</p>
                </div>
            </div>
            <div class="col-md-6 col-xs-12 col-sm-6">
                <div class="article">
                    <!-- column 1 headline -->
                    <h1>PROPOSTA</h1>
                    <!-- column 1 text -->
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer lorem quam, adipiscing condimentum tristique vel, eleifend sed turpis. Curabitur pellentesque massa eu nulla consequat sed porttitor arcu porttitor.</p>
                    <p>Quisque volutpat pharetra felis, eu cursus lorem molestie vitae. Nulla vehicula, lacus ut suscipit fermentum, turpis felis ultricies dui, ut rhoncus libero augue at libero. Morbi ut arcu dolor. Maecenas id nulla nec nibh viverra vehicula. Cras feugiat, magna eu lacinia ullamcorper, augue est sodales nibh, ut vulputate augue est sed nunc. Suspendisse sagittist.</p>
                    <p>Maecenas id nulla nec nibh viverra vehicula. Cras feugiat, magna eu lacinia ullamcorper, augue est sodales nibh, ut vulputate augue est sed nunc. Suspendisse sagittist.</p>
                </div>
            </div>
        </div>
        <!-- /end about us -->

         <!-- call to action bar -->
        <div class="order_block"><i></i><a href="/Form/upload.aspx" class="order_btn styler_bg_color">Clique aqui!</a> <span>Responda o questionário agora!</span></div>

        <!-- start infogrphics bar -->
        <div id="features_marker"></div>
        <div class="infograph">
            <sup class="styler_bg_color"></sup>
            <!-- infographics bar headline -->
            <div class="header styler_color">Dados já obtidos!</div>
            <div class="row">
                <!-- infographic column 1 -->
                <div class="col-md-6 col-xs-3 col-sm-3">
                    <div class="graph">
                        <div class="graph_inner">
                            <!-- infographic column 1 - large number -->
                            <div class="large_num styler_color">98</div>
                        </div>
                    </div>
                    <div class="desc styler_bg_color">
                        <i class="styler_border_color"></i>
                        <!-- infographics column 1 headline -->
                        <div class="name">Munícipios contabilizados</div>
                        <!-- infographics column 1 text body -->
                        <div class="text">98 municípios já estão nos índices!</div>
                    </div>
                </div>
                <!-- infographic column 4 -->
                <div class="col-md-6 col-xs-3 col-sm-3">
                    <div class="graph">
                        <div class="graph_inner">
                            <!-- infogrphic column 4 pie chart -->
                            <div class="easy-pie-chart styler_infograph" data-percent="60" data-trackcolor="#6ed1ff" data-color="#3698D8" data-linewidth="25" data-size="140" data-cap="butt"><span class="percent">60</span></div>
                        </div>
                    </div>
                    <div class="desc styler_bg_color">
                        <i class="styler_border_color"></i>
                        <!-- infographics column 4 headline -->
                        <div class="name">Questionários por entidades</div>
                        <!-- infographics column 4 text body -->
                        <div class="text">60% das entidades representantes já responderam ao questionário!</div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /end infographics bar -->
    </div>
    <script type="text/javascript">
    </script>
</asp:Content>
