<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Ferramenta.Period.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid outer">
        <div class="main-form">
            <div class="span12 text-right">
                <asp:Button ID="btnNewUser" CssClass="btn btn-primary"  Text='<%$Resources: Label, create_a_new_period %>' OnClick="btnNewUser_Click" runat="server" />
            </div>
            <div class="span12">
                <div class="box">
                    <header>
                        <div class="icons">
                            <i class="fa fa-filter"></i>
                        </div>
                        <h5>Filtro</h5>
                        <ul class="nav pull-right">
                            <li>
                                <a href="#filterBlock" data-toggle="collapse" class="accordion-toggle minimize-box">
                                    <i class="fa fa-chevron-up"></i>
                                </a>
                            </li>
                        </ul>
                    </header>
                    <div id="filterBlock" class="body in collapse form-horizontal">
                        <div class="control-group">
                            <label class="control-label"><asp:Literal ID="Literal1" Text='<%$Resources: Label, period_status %>' runat="server" /></label>
                            <div class="controls">
                                <label>
                                    <input id="open" checked="checked" class="uniform" type="checkbox" value="open"><asp:Literal ID="Literal2" Text='<%$Resources: Label, open %>' runat="server" />
                                </label>
                                <label>
                                    <input id="closed" checked="checked" class="uniform" type="checkbox" value="closed"><asp:Literal ID="Literal3" Text='<%$Resources: Label,closed  %>' runat="server" />
                                </label>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label"><asp:Literal ID="Literal4" Text='<%$Resources: Label, initial_date_period %>' runat="server" /></label>
                            <div class="controls">
                                <div class="input-prepend">
                                    <span class="add-on"><i class="fa fa-calendar"></i></span>
                                    <input type="text" data-date-format="dd/mm/yyyy" readonly="true" style="cursor: pointer; background-color: #fff" id="txtInitialDate" />
                                </div>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label"><asp:Literal ID="Literal5" Text='<%$Resources: Label, final_date_period %>' runat="server" /></label>
                            <div class="controls">
                                <div class="input-prepend">
                                    <span class="add-on"><i class="fa fa-calendar"></i></span>
                                    <input type="text" data-date-format="dd/mm/yyyy" readonly="true" style="cursor: pointer; background-color: #fff" id="txtFinalDate" />
                                </div>
                            </div>
                        </div>
                        <div class="form-actions">
                            <button id="btnFilter" class="btn btn-primary" onclick="searchPeriods(); return false;"><asp:Literal ID="Literal6" Text='<%$Resources: Label, filter %>' runat="server" /></button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span12">
                <div class="box">
                    <header>
                        <div class="icons">
                            <i class="fa fa-filter"></i>
                        </div>
                        <h5 id="resultMessage"></h5>
                        <ul class="nav pull-right">
                            <li>
                                <a href="#result" data-toggle="collapse" class="accordion-toggle minimize-box">
                                    <i class="fa fa-chevron-up"></i>
                                </a>
                            </li>
                        </ul>
                    </header>
                    <div id="result" class="body collapse in">
                        <table id="dataTable" class="table table-bordered table-condensed table-hover table-striped">
                            <thead>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    </div>
    <script type="text/x-jquery-tmpl" id="modal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel"><asp:Literal ID="Literal7" Text='<%$Resources: Label, publish_questions %>' runat="server" /></h4>
                </div>
                <div class="modal-body">
                    <span><asp:Literal ID="Literal8" Text='<%$Resources: Label, publish_questions_terms1 %>' runat="server" /></span><br />
                    <br />
                    <span><asp:Literal ID="Literal9" Text='<%$Resources: Label, publish_questions_terms2 %>' runat="server" /></span>
                    <br />
                    <br />
                    <br />
                    <a href="/Form/ListResponseForm.aspx?periodId=${IdEncoded}" target="_blank"><asp:Literal ID="Literal10" Text='<%$Resources: Label, visualize_questions_tobe_published %>' runat="server" /></a>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal"><asp:Literal ID="Literal11" Text='<%$Resources: Label, cancel %>' runat="server" /></button>
                    <button type="button" onclick="savePublished('${Id}'); return false;" class="btn btn-success"><asp:Literal ID="Literal12" Text='<%$Resources: Label,publish_questions  %>' runat="server" /></button>
                </div>
            </div>
        </div>
    </script>
    <script type="text/x-jquery-tmpl" id="row">
        <tr>
            <td>${Name}</td>
            <td>${InitialConvocationDate}</td>
            <td>${FinalConvocationDate}</td>
            <td>${InitialDate}</td>
            <td>${FinalDate}</td>
            <td class="span2" style="text-align: center">{{if Open}}
                    <span class="label label-success"><asp:Literal ID="Literal13" Text='<%$Resources: Label, open  %>' runat="server" /></span>
                {{else}}
                    <span class="label label-important"><asp:Literal ID="Literal14" Text='<%$Resources: Label, closed %>' runat="server" /></span>
                {{/if}}
            </td>
            {{if ActiveUserType == "Master"}}
                <td class="span3" style="text-align: center">{{if Published}}
                        <span class="label label-success"><asp:Literal ID="Literal15" Text='<%$Resources: Label, period_already_published %>' runat="server" /></span>
                    {{else}}
                        {{if CanPublish}}
                            <button onclick="showModal('${Id}', '${IdEncoded}'); return false;" class="btn btn-inverse"><asp:Literal ID="Literal16" Text='<%$Resources: Label, publish_period %>' runat="server" /></button>
                    {{else}}
                            <span class="label label-important"><asp:Literal ID="Literal17" Text='<%$Resources: Label, period_couldnt_be_published %>' runat="server" /></span>
                    {{/if}}
                    {{/if}}
                </td>
            {{/if}}
            <td class="span1" style="text-align: center">
                <button class="btn edit" onclick="redirectPeriod('${Id}');return false;"><i class="fa fa-edit"></i></button>
            </td>
        </tr>
    </script>
    <script type="text/x-jquery-tmpl" id="header">
        <tr>
            <th><asp:Literal ID="Literal18" Text="<%$Resources: Label, name %>" runat="server" /></th>
            <th><asp:Literal ID="Literal19" Text="<%$Resources: Label, initial_convocation_date %>" runat="server" /></th>
            <th><asp:Literal ID="Literal20" Text="<%$Resources: Label, final_convocation_date %>" runat="server" /></th>
            <th><asp:Literal ID="Literal21" Text="<%$Resources: Label, initial_submission_date %>" runat="server" /></th>
            <th><asp:Literal ID="Literal22" Text="<%$Resources: Label, final_submission_date %>" runat="server" /></th>
            <th style="width: 50px;"><asp:Literal ID="Literal23" Text="<%$Resources: Label, status %>" runat="server" /></th>
            {{if ActiveUserType == "Master"}}
                <th><asp:Literal ID="Literal24" Text="<%$Resources: Label, publish %>" runat="server" /></th>
            {{/if}}
            <th></th>
        </tr>
    </script>
    <script type="text/javascript">
        var currentTable = null;

        $(document).ready(function () {
            var checkin = $('#txtInitialDate').datepicker({
                language: 'pt-BR',
            }).on('changeDate', function (ev) {
                checkin.hide();
            }).data('datepicker');

            var checkout = $('#txtFinalDate').datepicker({
                language: 'pt-BR'
            }).on('changeDate', function (ev) {
                checkout.hide();
            }).data('datepicker');

            searchPeriods();
        });

        function searchPeriods() {
            //coloca o loading
            setLoadingToDiv($(".main-form"), '<asp:Literal Text="<%$Resources: Label, loading_periods %>" runat="server" />');

            var itens = [];

            $("#filterBlock input:checked").each(function (i, v) {
                itens.push($(v).attr('id'));
            });

            $("#resultMessage").html('<asp:Literal Text="<%$Resources: Label, wait %>" runat="server" />');
            $.ajax({
                url: "../Handlers/Period/GetAll.ashx",
                data: {
                    ut: itens.join(),
                    id: $("#txtInitialDate").val(),
                    fd: $("#txtFinalDate").val()
                },
                type: 'POST',
                cache: false,
                success: function (data) {
                    if (currentTable != null) {
                        currentTable.fnClearTable();
                        currentTable.fnDestroy();
                    }

                    $("#dataTable thead").empty();
                    $("#dataTable tbody").empty();

                    if (data.length > 0) {

                        $("#resultMessage").html(data.length + ' ' + '<asp:Literal Text="<%$Resources: Label, registers_founded %>" runat="server" />');
                        $("#row").tmpl(data).appendTo("#dataTable tbody");
                        $("#header").tmpl(data[0]).appendTo("#dataTable thead");

                        var options = DATATABLE_SETTINGS;
                        options.aaSorting = [[2, "desc"]];
                        options.aoColumnDefs = [
                            { "sType": "string-no-accents", "aTargets": [0] },
                            { "sType": "date", "aTargets": [1] },
                            { "sType": "date", "aTargets": [2] },
                            { "sType": "date", "aTargets": [3] },
                            { "sType": "date", "aTargets": [4] },
                            { "sType": "string-no-accents", "aTargets": [5] },
                            { "bSortable": false, "bSearchable": true, "aTargets": [6] },
                            { "bSortable": false, "bSearchable": false, "aTargets": [7] }];

                        currentTable = $("#dataTable").dataTable(options);
                    } else {
                        $("#resultMessage").html('0 ' + '<asp:Literal Text="<%$Resources: Label, registers_founded %>" runat="server" />');

                    }
                },
                error: function (error) {
                    showAlert(error.responseText, 'error');
                },
                complete: function () {
                    removeLoadingToDiv($(".main-form"));
                }
            });
        }

        function redirectPeriod(id) {
            window.location = 'NewPeriod.aspx?id=' + encodeURIComponent(id);
        }

        function showModal(Id, IdEncoded) {
            var obj = new Object();
            obj["Id"] = Id;
            obj["IdEncoded"] = IdEncoded;

            $('#myModal').html('');
            $('#modal').tmpl(obj).appendTo('#myModal');

            $('#myModal').modal();
        }

        function savePublished(Id) {
            //coloca o loading
            setLoadingToDiv($(".main-form"), '<asp:Literal Text="<%$Resources: Label, loading_periods %>" runat="server" />');

            $("#resultMessage").html('<asp:Literal Text="<%$Resources: Label, wait %>" runat="server" />');

            $.ajax({
                url: "../Handlers/Period/Action.ashx",
                data: {
                    Id: Id
                },
                type: 'POST',
                cache: false,
                success: function (data) {
                    $('#myModal').modal('hide');

                    searchPeriods();
                },
                error: function (error) {
                    showAlert(error.responseText, 'error');
                },
                complete: function () {
                    removeLoadingToDiv($(".main-form"));
                }
            });
        }
    </script>
</asp:Content>
