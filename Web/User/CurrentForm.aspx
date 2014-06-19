<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CurrentForm.aspx.cs" Inherits="Site.User.CurrentForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

        .truncate {
          width: 250px;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
        }

        ul.affix
        {
            position: fixed;
            top: 100px;
            /*left: 62px;*/
            width: 300px;
        }

        ul.affix-top
        {
            position: static;
        }

        ul.affix-bottom
        {
            position: absolute;
        }

        /* First level of nav */
        .sidenav
        {
            margin-top: 20px;
            margin-bottom: 30px;
            padding-top: 10px;
            padding-bottom: 10px;
            background-color: #f7f5fa;
            border-radius: 5px;
        }

        /* All levels of nav */
        .sidebar .nav > li > a
        {
            display: block;
            color: #716b7a;
            padding: 5px 20px;
        }

            .sidebar .nav > li > a:hover,
            .sidebar .nav > li > a:focus
            {
                text-decoration: none;
                background-color: #e5e3e9;
            }

        .sidebar .nav > .active > a,
        .sidebar .nav > .active:hover > a,
        .sidebar .nav > .active:focus > a
        {
            font-weight: bold;
            color: #563d7c;
            background-color: transparent;
        }

        /* Nav: second level */
        .sidebar .nav .nav
        {
            display: none;
        }

        .sidebar .nav > li.active .nav
        {
            display: block;
        }

        .sidebar .nav .nav
        {
            margin-bottom: 8px;
        }

            .sidebar .nav .nav > li > a
            {
                padding-top: 3px;
                padding-bottom: 3px;
                padding-left: 30px;
                font-size: 90%;
            }

        #content
        {
            margin-top: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdPeriodId" runat="server" />
    <asp:HiddenField ID="hdCityId" runat="server" />
    <asp:HiddenField ID="hdResponseFormId" runat="server" />
    <asp:HiddenField ID="hdCurrentBlockId" runat="server" />
    <asp:HiddenField ID="hdCurrentSubblockId" runat="server" />
    <div class="container">
        <ul class="breadcrumbs styler_bg_color" id="ul_title" runat="server" visible="false">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text=''></asp:Label></li>
        </ul>
        <div class="row">
            <asp:PlaceHolder runat="server" ID="phViewFormDetails" Visible="false">
                <div class="box">
                    <div class="row text-center">
                        <div class="col-md-12">
                            <button class="btn btn-primary btn-lg" id="btnFormDetails" runat="server" onserverclick="btnFormDetails_ServerClick">
                                <asp:Literal ID="Literal4" Text='<%$Resources: Message, see_form_details %>' runat="server" /></button>
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="phFinishForm" Visible="false">
                <div class="box">
                    <div class="row text-center">
                        <h4 style="color:  #5cb85c">
                            <asp:Literal ID="ltFinishText" Text='<%$Resources: Message, congratulations_form_was_finished %>' runat="server" /></h4>
                    </div>
                </div>
            </asp:PlaceHolder>

            <asp:PlaceHolder runat="server" ID="phMustRevised" Visible="false">
                <div class="box">
                    <div class="row text-center">
                        <h4 style="color: #86aa65">
                            <asp:Literal ID="Literal1" Text='<%$Resources: Message, form_must_revised %>' runat="server" /></h4>
                        <p>Observações:
                            <small><asp:Literal ID="ltNotAprovedObservations" Text="" runat="server" /></small>
                        </p>
                    </div>
                </div>
            </asp:PlaceHolder>
        </div>
        <div class="row">
            <nav id="affix-nav" class="sidebar col-md-4 col-sm-4 hidden-xs">
                <ul class="nav sidenav" data-spy="affix" data-offset-top="290" data-offset-bottom="450">
                    <asp:Repeater ID="rptIndex" runat="server">
                        <ItemTemplate>
                            <li class="active">
                                <a href='#b<%#Eval("Id") %>'><%#Eval("Name") %></a>
                                <ul class="nav">
                                    <asp:Repeater ID="rptSubblockIndex" runat="server" DataSource='<%#Eval("BaseSubBlocks") %>'>
                                        <ItemTemplate>
                                            <li>
                                                <a href='#sb<%#Eval("Id") %>'><%#Eval("Name") %></a></li>
                                                 <ul class="nav">
                                                    <asp:Repeater ID="rptQuestionsIndex" runat="server" DataSource='<%#Eval("BaseQuestions") %>'>
                                                        <ItemTemplate>
                                                            <li><a class="truncate" href='#qst<%#Eval("Id") %>'><%#Eval("Question") %></a></li>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                        </ItemTemplate>
                    </asp:Repeater>
                    <li>
                        <asp:PlaceHolder ID="phSaveForm" runat="server" Visible="false">
                            <div class="row container text-center">
                                <button id="btnSave" runat="server" class="btn btn-primary" onserverclick="btnSave_ServerClick"><i class="fa fa-fw fa-save"></i>Salvar questionário</button>

                            </div>
                            <div id="saveMessage" runat="server" visible="false" class="row container text-center color-green">
                                <i class="fa fa-fw fa-lg fa-check"></i>&nbsp;
                                <asp:Label ID="lblSaveSuccess" runat="server" Text="Questionário salvo!"></asp:Label>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="phSubmitForm" Visible="false">
                            <div class="row container text-center" style="margin-top: 10px">
                                <button class="btn btn-success" id="btnSubmitForm" runat="server" onserverclick="btnSubmitForm_ServerClick">
                                    <asp:Literal ID="Literal3" Text='<%$Resources: Message, submit_a_form %>' runat="server" /></button>
                            </div>
                        </asp:PlaceHolder>
                    </li>
                </ul>
            </nav>
            <section id="content" class="col-md-8">
                <asp:Repeater ID="rptForms" runat="server">
                    <ItemTemplate>
                        <article id='b<%#Eval("Id") %>'>
                            <h2><%#Eval("Name") %></h2>
                            <asp:Repeater ID="rptSubblocks" runat="server" DataSource='<%#Eval("BaseSubBlocks") %>'>
                                <ItemTemplate>
                                    <section id='sb<%#Eval("Id") %>'>
                                        <h3><%#Eval("Name") %></h3>
                                        <asp:Repeater ID="rptQuestions" runat="server" DataSource='<%#Eval("BaseQuestions") %>' OnItemDataBound="rptQuestions_ItemDataBound">
                                            <ItemTemplate>
                                                <div class="row">
                                                    <asp:HiddenField runat="server" Value='<%# Eval("Id") %>' ID="hdQuestionId" />
                                                    <p id='qst<%#Eval("Id") %>' style="text-indent: 10px;"><%# Eval("Question") %></p>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <asp:DropDownList ID="ddlAnswer" CssClass="form-control" runat="server" Style="height: 30px;">
                                                            <asp:ListItem Text='<%$Resources: Message, no_response %>' Value="" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="0.25" Value="025"></asp:ListItem>
                                                            <asp:ListItem Text="0.5" Value="05"></asp:ListItem>
                                                            <asp:ListItem Text="0.75" Value="075"></asp:ListItem>
                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="N/A" Value="NA"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <asp:TextBox ID="txtObservation" CssClass="form-control" placeholder='<%$Resources: Message, add_observations %>' TextMode="MultiLine" Rows="2" Columns="100" runat="server" />
                                                    </div>
                                                </div>
                                                <hr />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </section>
                                </ItemTemplate>
                            </asp:Repeater>
                        </article>
                    </ItemTemplate>
                </asp:Repeater>
            </section>
        </div>
        <!-- end of row -->
        <footer>
        </footer>
    </div>
    <!-- end of container -->
    <%--        <asp:PlaceHolder runat="server" ID="phFinishForm" Visible="false">
            <div class="box">
                <div class="row text-center">
                    <h4 style="color: #86aa65">
                        <asp:Literal ID="ltFinishText" Text='<%$Resources: Message, congratulations_form_was_finished %>' runat="server" /></h4>
                </div>
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="phBaseBlockView" runat="server" Visible="false">
            <div class="col-md-12 col-xs-12 col-sm-6">
                <ul id="blocks" runat="server" class="styled_list" data-color="" data-type="process_box">
                    <asp:Repeater ID="rptBlocks" runat="server">
                        <ItemTemplate>
                            <li>
                                <div class="name" style="width: 100%">
                                    <asp:Literal ID="lit" runat="server" Text='<%# Eval("Name") %>'></asp:Literal>
                                </div>
                                <div class="description" style="width: 100%">
                                    <a href='#' onclick="redirectToBlock('<%#Eval("Id") %>')">
                                        <asp:Literal ID="Literal1" Text='<%$Resources: Message, answer %>' runat="server" /></a>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="phBaseSubBlockView" runat="server" Visible="false">
            <ul id="subblocks" runat="server" class="styled_list" data-type="process_box">
                <asp:Repeater ID="rptSubBlocks" runat="server">
                    <ItemTemplate>
                        <li>
                            <div class="name" style="width: 100%">
                                <asp:Literal ID="litSubBlock" runat="server" Text='<%# Eval("Name") %>'></asp:Literal>
                            </div>
                            <div class="description" style="width: 100%">
                                <a href='#' onclick="redirectToSubBlock('<%#Eval("Id") %>')">
                                    <asp:Literal ID="Literal1" Text='<%$Resources: Message, answer %>' runat="server" /></a>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="phQuestionView" runat="server" Visible="false">
            <div class="box span12">
                <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                    <ItemTemplate>
                        <div class="row">
                            <asp:HiddenField runat="server" Value='<%# Eval("Id") %>' ID="hdQuestionId" />
                            <asp:Literal ID="litSubBlock" runat="server" Text='<%# Eval("Question") %>'></asp:Literal>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlAnswer" runat="server" Style="width: 150px">
                                    <asp:ListItem Text='<%$Resources: Message, no_response %>' Value="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="0.25" Value="025"></asp:ListItem>
                                    <asp:ListItem Text="0.5" Value="05"></asp:ListItem>
                                    <asp:ListItem Text="0.75" Value="075"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="N/A" Value="NA"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtObservation" placeholder='<%$Resources: Message, add_observations %>' TextMode="MultiLine" Rows="2" Columns="100" runat="server" CssClass="span8" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:PlaceHolder>

        <div class="span12">
            <div class="row">
                <div class="col-md-2 text-left">
                    <button class="btn btn-primary btn-lg" id="btnPreviousButton" runat="server" onserverclick="btnPreviousButton_ServerClick">
                        <asp:Literal ID="Literal1" Text='<%$Resources: Message, back %>' runat="server" /></button>
                </div>
                <div class="col-md-8"></div>
                <div class="col-md-2 text-right">
                    <button class="btn btn-primary btn-lg" id="btnNextButton" runat="server" onserverclick="btnNextButton_ServerClick">
                        <asp:Literal ID="Literal2" Text='<%$Resources: Message, next %>' runat="server" /></button>
                </div>
            </div>
        </div>

        <asp:PlaceHolder runat="server" ID="phSubmitForm" Visible="false">
            <div class="box">
                <div class="row text-center">
                    <div class="col-md-7">
                        <asp:DropDownList runat="server" ID="ddlCities" DataTextField="Name" DataValueField="Id">
                        </asp:DropDownList>
                        <asp:Label ID="lblCityError" Style="color: #b94a48" Text="" runat="server"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <button class="btn btn-primary btn-lg" id="btnSubmitForm" runat="server" onserverclick="btnSubmitForm_ServerClick">
                            <asp:Literal ID="Literal3" Text='<%$Resources: Message, submit_a_form %>' runat="server" /></button>


                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="phViewFormDetails" Visible="false">
            <div class="box">
                <div class="row text-center">
                    <div class="col-md-12">
                        <button class="btn btn-primary btn-lg" id="btnFormDetails" runat="server" onserverclick="btnFormDetails_ServerClick">
                            <asp:Literal ID="Literal4" Text='<%$Resources: Message, see_form_details %>' runat="server" /></button>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>--%>
    <script type="text/javascript">
        //$(document).ready(function () {
        //    $("#affix-ul").affix({
        //        offset: {
        //            top: 290
        //        }
        //    });
        //});

        //$(window).on('click.bs.affix.data-api', function () {
        //    setTimeout(function () {
        //        $target = $("#myNav li.active a").attr('href');
        //        $target = $($target);
        //        //alert($target);
        //        $top = $target.offset().top - $('#header .inner').height();

        //        window.scrollTo(0, $top);
        //        e.stopPropagtion();
        //        $("#myNav li.active a").removeClass('active');
        //    }, 10);

        //});

        function redirectToBlocks() {
            window.location = "CurrentForm.aspx?p=" + encodeURIComponent($("#<%=hdPeriodId.ClientID%>").val());
        }
        function redirectToBlock(blockId) {
            window.location = "CurrentForm.aspx?p=" + encodeURIComponent($("#<%=hdPeriodId.ClientID%>").val()) + "&bid=" + blockId;
        }

        function redirectToSubBlock(subblockId) {
            window.location = "CurrentForm.aspx?p=" + encodeURIComponent($("#<%=hdPeriodId.ClientID%>").val()) + "&sbid=" + subblockId;
        }
    </script>
</asp:Content>
