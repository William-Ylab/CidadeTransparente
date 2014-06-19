<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ManageCities.aspx.cs" Inherits="Ferramenta.City.ManageCities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid outer">
        <div class="main-form">
            <div class="span12 text-right">
                <asp:Button ID="btnNewCity" class="btn btn-primary" Text='<%$Resources: Label, create_a_new_city %>' OnClick="btnNewCity_Click" runat="server" />
            </div>
            <div class="span12">
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
                                <tr>
                                    <th style="vertical-align: middle">
                                        <asp:Literal ID="Literal14" Text='<%$Resources: Label, name  %>' runat="server" /></th>
                                    <th style="vertical-align: middle">
                                        <asp:Literal ID="Literal25" Text='<%$Resources: Label, state %>' runat="server" /></th>
                                    <th style="vertical-align: middle">
                                        <asp:Literal ID="Literal15" Text='<%$Resources: Label, responsable_name %>' runat="server" /></th>
                                    <th style="text-align: center; vertical-align: middle">
                                        <asp:Literal ID="Literal12" Text='<%$Resources: Label, total_of_collaborators  %>' runat="server" /></th>
                                    <th></th>
                                    <%--Ações (Detalhes)--%>
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
            <td>${Name}</td>
            <td>${StateName}</td>
            <td class="span3" style="text-align: center">
                {{if ResponsableName != ""}}
                    <span class="label label-success">
                        <asp:Literal ID="Literal23" Text=' ${ResponsableName}' runat="server" /></span>
                {{else}}
                    <span class="label label-important">
                        <asp:Literal ID="Literal24" Text='<%$Resources: Label, no_responsable %>' runat="server" /></span>
                {{/if}}
               </td>

            <td class="span3" style="text-align: center">
                {{if TotalCollaborators > 0}}
                    <span class="label label-success">
                        <asp:Literal ID="Literal3" Text='${TotalCollaborators} colaborador(es)' runat="server" /></span>
                {{else}}
                    <span class="label label-important">
                        <asp:Literal ID="Literal4" Text='<%$Resources: Label, no_collaborator %>' runat="server" /></span>
                {{/if}}
               </td>
            <td class="span1" style="text-align: center">
                <button class="btn edit" onclick="redirectEditCity('${Id}');return false;"><i class="fa fa-edit"></i></button>
            </td>
        </tr>
    </script>
    <script type="text/javascript">
        var currentTable = null;

        $(document).ready(function () {
            //var value = getValueByQueryString('ut');
            //var types = value.split(",");
            //for (var i in types) {
            //    $("#" + types[i]).attr('checked', true);
            //    $("#" + types[i]).uniform();
            //}

            if ($.cookie("fdt_search_manage_cities") != null) {
                var filter = $.parseJSON($.cookie("fdt_search_manage_cities"));
                $("#<%=ddlStates.ClientID%>").val(filter.state);
                $("#<%=ddlPeriods.ClientID%>").val(filter.period);
            }

            searchCities();
        });

        function searchCities() {
            //coloca o loading
            setLoadingToDiv($(".main-form"), '<asp:Literal Text="<%$Resources: Label, loading_cities %>" runat="server" />');

            $("#resultMessage").html('<asp:Literal Text="<%$Resources: Label, wait %>" runat="server" />');
            $.ajax({
                url: "../Handlers/GroupCities/GetAll.ashx",
                data: { state: $("#<%=ddlStates.ClientID%>").val(), period: $("#<%=ddlPeriods.ClientID%>").val() },
                type: 'POST',
                cache: false,
                success: function (data) {
                    saveCookie("fdt_search_manage_cities", $.toJSON({ state: $("#<%=ddlStates.ClientID%>").val(), period: $("#<%=ddlPeriods.ClientID%>").val() }), 1);

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
                        { "bSortable": false, "bSearchable": false, "sType": "string-no-accents", "aTargets": [1] },
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

        function redirectEditCity(id) {
            window.location = 'NewCity.aspx?id=' + encodeURIComponent(id);
        }
    </script>
</asp:Content>
