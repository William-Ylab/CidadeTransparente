<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="RequestCities.aspx.cs" Inherits="Ferramenta.City.RequestCities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid outer">
        <div class="main-form">
            <%--<div class="span12 text-right">
                <asp:Button ID="btnNewCity" class="btn btn-primary" Text='<%$Resources: Label, create_a_new_city %>' OnClick="btnNewCity_Click" runat="server" />
            </div>--%>
            <%--<div class="span12">
                <div class="box">
                    <header>
                        <div class="icons">
                            <i class="fa fa-filter"></i>
                        </div>
                        <h5>
                            <asp:Literal ID="Literal1" Text='<%$Resources: Label, filter %>' runat="server" /></h5>
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
                            <label class="control-label">
                                <asp:Literal ID="Literal2" Text='<%$Resources: Label, period %>' runat="server" /></label>
                            <div class="controls">
                                <asp:DropDownList runat="server" ID="ddlPeriods" DataTextField="Name" DataValueField="Id">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">
                                <asp:Literal ID="Literal7" Text='<%$Resources: Label, state %>' runat="server" /></label>
                            <div class="controls">
                                <asp:DropDownList runat="server" ID="ddlStates" DataTextField="Name" DataValueField="Id">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-actions">
                            <button id="btnFilter" type="button" runat="server" class="btn btn-primary" onclick="searchCities(); return false;">
                                <asp:Literal ID="Literal11" Text='<%$Resources: Label, filter %>' runat="server" /></button>
                        </div>
                    </div>
                </div>
            </div>--%>
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
                                <tr>
                                    <th style="vertical-align: middle">
                                        <asp:Literal ID="Literal14" Text='<%$Resources: Label, entity_name  %>' runat="server" /></th>
                                    <th style="vertical-align: middle">
                                        <asp:Literal ID="Literal1" Text='<%$Resources: Label, request_city  %>' runat="server" /></th>
                                    <th style="vertical-align: middle">
                                        <asp:Literal ID="Literal15" Text='<%$Resources: Label, request_type %>' runat="server" /></th>
                                    <th style="vertical-align: middle">
                                        <asp:Literal ID="Literal25" Text='<%$Resources: Label, request_date %>' runat="server" /></th>
                                    <th style="text-align: center; vertical-align: middle">
                                        <asp:Literal ID="Literal12" Text='<%$Resources: Label, request_status  %>' runat="server" /></th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="row">
        <tr>
            <td><a href="/User/NewUser.aspx?id=${EntityId}">${EntityName}</a></td>
            <td><a href="/City/NewCity.aspx?id=${CityId}">${CityName}</a></td>
            <td class="span3" style="text-align: center">{{if RequestType == 0}}
                    <span class="label label-success">
                        <asp:Literal ID="Literal23" Text='<%$Resources: Label, responsable %>' runat="server" /></span>
                {{/if}}
                {{if RequestType == 1}}
                    <span class="label label-important">
                        <asp:Literal ID="Literal24" Text='<%$Resources: Label, collaborator %>' runat="server" /></span>
                {{/if}}
            </td>
            <td>${RequestDate}</td>
            <td style="text-align: center; vertical-align: middle">{{if RequestStatus == 0}}
                    <span class="label label-warning">
                        <asp:Literal ID="Literal8" Text='<%$Resources: Label, waiting_approve %>' runat="server" /></span>
                {{/if}}
                {{if RequestStatus == 1}}
                    <span class="label label-success">
                        <asp:Literal ID="Literal9" Text='<%$Resources: Label, approved %>' runat="server" /></span>
                {{/if}}</td>
            <td class="span2" style="text-align: center">{{if RequestStatus == 0}}
                <button class="btn btn-success" onclick="approveRequest('${Id}');return false;"><i class="fa fa-check"></i>Aprovar</button>
                {{/if}}
            </td>
        </tr>
    </script>
    <script type="text/javascript">
        var currentTable = null;
        $(document).ready(function () {
            loadRequest();
        });

        function approveRequest(id) {
            $.ajax({
                url: "../Handlers/GroupCities/ApproveRequest.ashx",
                data: { rId: id },
                type: 'POST',
                cache: false,
                success: function (data) {
                    if (data === "ok") {
                        window.location.href = window.location.href;
                    }
                    else {
                        showAlert("Problema ao aprovar esta entidade", 'error');
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

        function loadRequest() {
            $("#resultMessage").html('<asp:Literal Text="<%$Resources: Label, wait %>" runat="server" />');

            $.ajax({
                url: "../Handlers/GroupCities/GetRequests.ashx",
                data: { pId: getValueByQueryString('periodId') },
                type: 'POST',
                cache: false,
                success: function (data) {
                    if (currentTable != null) {
                        currentTable.fnClearTable();
                        currentTable.fnDestroy();
                    }
                    $("#resultMessage").html(data.length + ' ' + '<asp:Literal Text="<%$Resources: Label, registers_founded %>" runat="server" />');
                    $("#dataTable tbody").empty();
                    $("#row").tmpl(data).appendTo("#dataTable tbody");

                    //bizstrapTable();
                    var options = DATATABLE_SETTINGS;
                    options.aoColumnDefs = [
                        { "sType": "string-no-accents", "aTargets": [0] },
                        { "sType": "string-no-accents", "aTargets": [1] },
                        { "sType": "string-no-accents", "aTargets": [2] },
                        { "sType": "string-no-accents", "aTargets": [3] },
                        { "bSortable": false, "bSearchable": false, "aTargets": [4] }];

                    currentTable = $("#dataTable").dataTable(options);
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
