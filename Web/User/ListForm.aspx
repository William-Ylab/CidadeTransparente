<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListForm.aspx.cs" Inherits="Site.User.ListForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= @System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl("~/Design/Styles/Datatables/datatables") %>" rel="Stylesheet" type="text/css" />
    <script src="<%= @System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl("~/Scripts/Datatables/datatables") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdLastPeriodId" runat="server" />
    <asp:HiddenField ID="hdCurrentPeriodId" runat="server" />
    <asp:HiddenField ClientIDMode="Static" ID="hdnPeriodId" runat="server" Value="0" />

    <div class="container">
        <ul class="breadcrumbs styler_bg_color">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text='<%$Resources: Message, dashboard %>'></asp:Label></li>
        </ul>
        <asp:PlaceHolder ID="phHasNotApprovedForms" runat="server" Visible="false">
            <div class="alert-bt alert-block alert-warning text-center">
                <p style="padding:10px;">
                    <asp:Literal runat="server" Text="<%$Resources: Message, there_are_not_approved_forms %>" />
                </p>
            </div>
        </asp:PlaceHolder>
        <div class="span12">
            <div class="box">
                <asp:PlaceHolder runat="server" ID="phLastForm" Visible="false">
                    <div class="col-md-6">
                        <span>
                            <asp:Literal ID="Literal1" Text='<%$Resources: Message, fill_form_and_compare %>' runat="server" /></span>
                        <button id="btnLastForm" runat="server" class="btn btn-primary" onserverclick="btnLastForm_ServerClick">
                            <i class="fa fa-pencil-square-o"></i>
                            <asp:Literal ID="Literal2" Text='<%$Resources: Message, click_here_and_form %>' runat="server" /></button>
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phCurrentForm" Visible="false">
                    <div class="col-md-6">
                        <span>
                            <asp:Literal ID="Literal4" Text='<%$Resources: Message, period_open_to_avail %>' runat="server" /></span>
                        <button id="btnCurrentForm" runat="server" class="btn btn-primary" onserverclick="btnCurrentForm_ServerClick">
                            <i class="fa fa-pencil-square-o"></i>
                            <asp:Literal ID="Literal3" Text='<%$Resources: Message, click_here_and_form %>' runat="server" /></button>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
        <div class="clear"></div>
        <div class="span12" runat="server" id="div_cities_requested" style="margin-top: 30px">
            <div class="box">
                <header>
                    <h3>
                        <asp:Literal ID="Literal19" runat="server" Text='<%$Resources: Message,requesteds_cityname  %>'></asp:Literal></h3>
                </header>
                <div id="requesttable" class="body in collapse form-horizontal">
                    <div style="margin-bottom: 3em">
                        <table id="tblRequestCities" class="table table-bordered table-condensed table-hover table-striped">
                            <thead>
                                <tr>
                                    <th>
                                        <asp:Literal ID="Literal20" Text='<%$Resources: Message, requested_cityname %>' runat="server" /></th>
                                    <th class="text-center">
                                        <asp:Literal ID="Literal17" Text='<%$Resources: Message, state %>' runat="server" /></th>
                                    <th class="text-center">
                                        <asp:Literal ID="Literal18" Text='<%$Resources: Message, request_status %>' runat="server" /></th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <div class="text-right">
                        <span class="alert-block alert-warning" style="padding: 5px"><i class="fa fa-fw fa-warning fa-lg"></i>
                            <asp:Literal ID="ltRequestMessage" Text="" runat="server" /></span>&nbsp;
                        <button id="btnRequestCity" runat="server" class="btn btn-primary" onserverclick="btnRequestCity_ServerClick">
                            <i class="fa fa-plus fa-fw"></i>
                            <asp:Literal ID="Literal16" Text='<%$Resources: Message, request_city %>' runat="server" /></button>
                    </div>
                </div>
            </div>
        </div>
        <div class="clear"></div>
        <div class="span12" style="margin-top: 30px; margin-bottom: 4em;">
            <div class="box">
                <header>
                    <h3>
                        <asp:Literal ID="Literal21" runat="server" Text='<%$Resources: Message, answer_form %>'></asp:Literal></h3>
                </header>
                <div id="filterBlock" class="body in collapse form-horizontal">
                    <div class="row">
                        <div class="control-group col-md-4" id="div_statusSubmissao" runat="server">
                            <label class="control-label">
                                <asp:Literal ID="Literal5" Text='<%$Resources: Message, submit_status %>' runat="server" /></label>
                            <div class="controls">
                                <label>
                                    <input id="accepted" class="uniform" type="checkbox" value="accepted" /><asp:Literal ID="Literal6" Text='<%$Resources: Message, accept %>' runat="server" />
                                </label>
                                <label>
                                    <input id="submitted" class="uniform" type="checkbox" value="submitted" /><asp:Literal ID="Literal7" Text='<%$Resources: Message, submit %>' runat="server" />
                                </label>
                            </div>
                        </div>
                        <div class="control-group col-md-4">
                            <label class="control-label">
                                <asp:Literal ID="Literal8" Text='<%$Resources: Message, form %>' runat="server" /></label>
                            <div class="controls">
                                <label>
                                    <input id="completed" class="uniform" type="checkbox" value="completed" /><asp:Literal ID="Literal9" Text='<%$Resources: Message, complete_forms %>' runat="server" />
                                </label>
                                <label>
                                    <input id="incompleted" class="uniform" type="checkbox" value="incompleted" /><asp:Literal ID="Literal10" Text='<%$Resources: Message, incomplete_forms %>' runat="server" />
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="control-group">
                            <label class="control-label">
                                <asp:Literal ID="Literal11" Text='<%$Resources: Message, periods %>' runat="server" /></label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlPeriods" runat="server" CssClass="span2"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="text-right col-md-12">
                            <button id="btnFilter" class="btn btn-primary" onclick="searchResponseForms(); return false;">
                                <i class="fa fa-filter fa-fw"></i>
                                <asp:Literal ID="Literal12" Text='<%$Resources: Message, filter %>' runat="server" /></button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="responseFormsBox" class="span12">
            <div id="result" class="body collapse in">
                <table id="dataTable" class="table table-bordered table-condensed table-hover table-striped">
                    <thead>
                        <tr>
                            <th>
                                <asp:Literal ID="Literal13" Text='<%$Resources: Message, name %>' runat="server" /></th>
                            <th class="text-center">
                                <asp:Literal ID="Literal22" Text='<%$Resources: Message, city %>' runat="server" /></th>
                            <th class="text-center">
                                <asp:Literal ID="Literal23" Text='<%$Resources: Message, responsable %>' runat="server" /></th>
                            <th class="text-center">
                                <asp:Literal ID="Literal14" Text='<%$Resources: Message, period %>' runat="server" /></th>
                            <th class="text-center">
                                <asp:Literal ID="Literal15" Text='<%$Resources: Message, status %>' runat="server" /></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div class="text-center">
                    <h5 id="resultMessage"></h5>
                </div>
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="row">
        <tr>
            <td>${BaseFormName}</td>
            <td class="text-center">${CityState}</td>
            <td class="text-center">${ResponsableUser}</td>
            <td class="text-center">${PeriodName}</td>
            <td class="text-center">${StatusSubmit}</td> <%--${TotalAnswers}/${TotalQuestions}--%>
            <td class="span1" style="text-align: center">
                <button class="btn edit" onclick="redirectResponseForm('${IdCrypt}');return false;"><i class="fa fa-edit"></i></button>
            </td>
        </tr>
    </script>
    <script type="text/x-jquery-tmpl" id="request_city_row">
        <tr>
            <td>${CityName}</td>
            <td class="text-center">${StateName}</td>
            <td class="text-center">${RequestStatus}</td>
        </tr>
    </script>
    <script type="text/javascript">
        var currentTable = null;
        var currentTableRequestCities = null;

        $(document).ready(function () {
            $('#filterBlock input[type=checkbox]').each(function (i, v) {
                $(v).attr('checked', true);
                $(v).uniform();
            });

            searchResponseForms();
            searchRequestedCities();
        });

        function searchRequestedCities() {
            if ($('#hdnPeriodId').val() == '0') {
                return;
            }

            $.ajax({
                url: "../Handlers/Groups/GetRequests.ashx",
                type: 'POST',
                cache: false,
                data: {
                    periodId: $('#hdnPeriodId').val()
                },
                success: function (data) {
                    if (currentTableRequestCities != null) {
                        currentTableRequestCities.fnClearTable();
                        currentTableRequestCities.fnDestroy();
                    }

                    $("#tblRequestCities tbody").empty();
                    $("#request_city_row").tmpl(data).appendTo("#tblRequestCities tbody");

                    //bizstrapTable();
                    var options = DATATABLE_SETTINGS;
                    options.aoColumnDefs = [
                        { "sType": "string-no-accents", "aTargets": [0] },
                        { "sType": "string-no-accents", "aTargets": [1] },
                        { "bSortable": false, "aTargets": [2] }];

                    currentTableRequestCities = $("#tblRequestCities").dataTable(options);
                },
                error: function (error) {
                    showAlert(error.responseText, 'error');
                }
            });
        }

        function searchResponseForms() {
            //coloca o loading
            setLoadingToDiv($("#responseFormsBox"), '<asp:Literal Text="<%$Resources: Message, loading_forms %>" runat="server" />');

            var itens = [];

            $("#filterBlock input:checked").each(function (i, v) {
                itens.push($(v).attr('id'));
            });

            $("#resultMessage").html('<asp:Literal Text="<%$Resources: Message, wait %>" runat="server" />');

            $.ajax({
                url: "../Handlers/ResponseForm/Get.ashx",
                type: 'POST',
                cache: false,
                data: {
                    periodId: $('#<%= ddlPeriods.ClientID %>').val(),
                    chbs: itens.join()
                },
                success: function (data) {
                    if (currentTable != null) {
                        currentTable.fnClearTable();
                        currentTable.fnDestroy();
                    }

                    $("#resultMessage").html(data.length + ' registro(s) encontrado(s)');
                    $("#dataTable tbody").empty();
                    $("#row").tmpl(data).appendTo("#dataTable tbody");

                    //bizstrapTable();
                    var options = DATATABLE_SETTINGS;
                    options.aoColumnDefs = [
                        { "sType": "string-no-accents", "aTargets": [0] },
                        { "sType": "string-no-accents", "aTargets": [1] },
                        { "sType": "string-no-accents", "aTargets": [2] },
                        { "sType": "string-no-accents", "aTargets": [3] },
                        { "sType": "string-no-accents", "aTargets": [4] },
                        { "bSortable": false, "bSearchable": false, "aTargets": [5] }];

                    currentTable = $("#dataTable").dataTable(options);
                },
                error: function (error) {
                    showAlert(error.responseText, 'error');
                },
                complete: function () {
                    removeLoadingToDiv($("#responseFormsBox"));
                }
            });
        }

        function redirectResponseForm(id) {
            window.location = '/Form/View.aspx?rfid=' + encodeURIComponent(id);
        }
    </script>
</asp:Content>
