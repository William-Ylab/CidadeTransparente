<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="Site.User.DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .table-line-through tr td {
            line-height: 50px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdPeriodId" runat="server" />
    <div class="container">
        <ul class="breadcrumbs styler_bg_color">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text=''></asp:Label></li>
        </ul>
        <div class="span12">
            <div class="box ">
                <%--<h4>
                    <asp:Literal ID="Literal1" Text='<%$Resources: Message, you_are_answer_following_form %>' runat="server" /></h4>--%>
                <div class="row">
                    <div class="col-md-2">
                        <b>
                            <asp:Literal ID="Literal2" Text='<%$Resources: Message, form %>' runat="server" /></b>
                    </div>
                    <div class="col-md-10">
                        <asp:Label ID="lblFormName" Text="" runat="server" />
                    </div>
                    <div class="col-md-2">
                        <b>
                            <asp:Literal ID="Literal3" Text='<%$Resources: Message, period %>' runat="server" /></b>
                    </div>
                    <div class="col-md-10">
                        <asp:Label ID="lblPeriod" Text="" runat="server" />
                    </div>
                </div>
            </div>
            <div class="box ">
                <asp:PlaceHolder ID="phStepToAnswer" runat="server" Visible="false">
<%--                    <div class="row">
                        <h4>
                            <asp:Literal ID="Literal4" Text='<%$Resources: Message, how_to_prefer_fill_the_form %>' runat="server" /></h4>
                    </div>--%>
                    <div class="row col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h3 class="panel-title">
                                    <asp:Literal ID="Literal14" Text='<%$Resources: Message, answer_online %>' runat="server" /></h3>
                            </div>
                            <div class="panel-body">
                                <label>
                                    <asp:Literal ID="Literal15" Text='<%$Resources: Message, online_mode_message %>' runat="server" /></label>
                                <asp:DropDownList runat="server" ID="ddlResponsableCities" DataTextField="Text" DataValueField="Id">
                        </asp:DropDownList>
                                <button id="btnCurrentResponseForm" style="float: right" runat="server" class="btn btn-lg btn-primary" onserverclick="btnCurrentResponseForm_ServerClick">
                                    <i class="fa fa-pencil-square-o"></i>
                                    <asp:Literal ID="Literal10" Text='<%$Resources: Message, next %>' runat="server" /></button>
                            </div>
                        </div>
                    </div>
                    <div style="float:right" class="row col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h3 class="panel-title">
                                    <asp:Literal ID="Literal11" Text='<%$Resources: Message, answer_offline %>' runat="server" />
                                </h3>
                            </div>
                            <div class="panel-body">
                                <label>
                                    <asp:Literal ID="Literal7" Text='<%$Resources: Message, offline_mode_message %>' runat="server" /></label>
                                <button id="btnCurrentResponseFormOffline" style="float: right" runat="server" class="btn btn-lg btn-primary" onserverclick="btnCurrentResponseFormOffline_ServerClick">
                                    <i class="fa fa-pencil-square-o"></i>
                                    <asp:Literal ID="Literal9" Text='<%$Resources: Message, next %>' runat="server" /></button>
                            </div>
                        </div>
                    </div>
                    <%--<div>
                        <div class="col-md-6">
                            <h3>
                                <asp:Literal ID="Literal5" Text='<%$Resources: Message, offline_mode %>' runat="server" /></h3>
                            <label>
                                <asp:Literal ID="Literal7" Text='<%$Resources: Message, offline_mode_message %>' runat="server" /></label>
                            <button id="btnCurrentResponseFormOffline" runat="server" class="btn btn-danger btn-lg" onserverclick="btnCurrentResponseFormOffline_ServerClick">
                                <i class="fa fa-pencil-square-o"></i>
                                <asp:Literal ID="Literal9" Text='<%$Resources: Message, click_here2 %>' runat="server" /></button>
                        </div>
                        <div class="col-md-6">
                            <h3>
                                <asp:Literal ID="Literal6" Text='<%$Resources: Message, online_mode %>' runat="server" /></h3>
                            <label>
                                <asp:Literal ID="Literal8" Text='<%$Resources: Message, online_mode_message %>' runat="server" /></label>
                            <button id="btnCurrentResponseForm" runat="server" class="btn btn-success btn-lg" onserverclick="btnCurrentResponseForm_ServerClick">
                                <i class="fa fa-pencil-square-o"></i>
                                <asp:Literal ID="Literal10" Text='<%$Resources: Message, click_here2 %>' runat="server" /></button>
                        </div>
                    </div>--%>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
</asp:Content>
