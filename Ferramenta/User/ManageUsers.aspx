<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="Ferramenta.User.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid outer">
        <div class="main-form">
            <div class="span12 text-right">
                <asp:Button ID="btnNewUser" class="btn btn-primary" Text='<%$Resources: Label, create_a_new_user %>' OnClick="btnNewUser_Click" runat="server" />
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
                                <asp:Literal ID="Literal2" Text='<%$Resources: Label, usertype %>' runat="server" /></label>
                            <div class="controls">
                                <label>
                                    <input id="master" class="uniform" type="checkbox" value="master"><asp:Literal ID="Literal3" Text='<%$Resources: Label,master  %>' runat="server" />
                                </label>
                                <label>
                                    <input id="admin" class="uniform" type="checkbox" value="admin"><asp:Literal ID="Literal4" Text='<%$Resources: Label,administrator  %>' runat="server" />
                                </label>
                                <label>
                                    <input id="entity" class="uniform" type="checkbox" value="entity"><asp:Literal ID="Literal5" Text='<%$Resources: Label, entity %>' runat="server" />
                                </label>
                                <label>
                                    <input id="common" class="uniform" type="checkbox" value="common"><asp:Literal ID="Literal6" Text='<%$Resources: Label, user_comum %>' runat="server" />
                                </label>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">
                                <asp:Literal ID="Literal7" Text='<%$Resources: Label, user_status %>' runat="server" /></label>
                            <div class="controls">
                                <select id="ddlStatus" class="span2">
                                    <option value="">
                                        <asp:Literal ID="Literal9" Text='<%$Resources: Label, withou_filter %>' runat="server" /></option>
                                    <option value="active">
                                        <asp:Literal ID="Literal8" Text='<%$Resources: Label, enables %>' runat="server" /></option>
                                    <option value="inactive">
                                        <asp:Literal ID="Literal10" Text='<%$Resources: Label, disables %>' runat="server" /></option>
                                </select>
                            </div>
                        </div>
                        <div class="form-actions">
                            <button id="btnFilter" class="btn btn-primary" onclick="searchUsers(); return false;">
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
                                    <th style="vertical-align:middle">
                                        <asp:Literal ID="Literal14" Text='<%$Resources: Label, name  %>' runat="server" /></th>
                                    <th style="vertical-align:middle">
                                        <asp:Literal ID="Literal25" Text='<%$Resources: Label, login %>' runat="server" /></th>
                                    <th style="vertical-align:middle">
                                        <asp:Literal ID="Literal15" Text='<%$Resources: Label, email %>' runat="server" /></th>
                                    <th style="text-align: center;vertical-align:middle">
                                        <asp:Literal ID="Literal12" Text='<%$Resources: Label, usertype  %>' runat="server" /></th>
                                    <th style="text-align: center;vertical-align:middle">
                                        <asp:Literal ID="Literal21" Text='<%$Resources: Label, nature  %>' runat="server" /></th>
                                    <th style="text-align: center;vertical-align:middle">
                                        <asp:Literal ID="Literal13" Text='<%$Resources: Label, status %>' runat="server" /></th>
                                                                        <th style="text-align: center">
                                        <asp:Literal ID="Literal22" Text='<%$Resources: Label, entity_network_approved %>' runat="server" /></th>
                                    <th style="text-align: center;vertical-align:middle">
                                        <asp:Literal ID="Literal20" Text='<%$Resources: Label, accepted_terms %>' runat="server" /></th>
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
            <td>${User}</td>
            <td>${Email}</td>
            <td class="span3" style="text-align: center">${Type}</td>
            <td class="span3" style="text-align: center">${Nature}</td>
            <td class="span2" style="text-align: center">{{if Status}}
                    <span class="label label-success">
                        <asp:Literal ID="Literal16" Text='<%$Resources: Label, enable %>' runat="server" /></span>
                {{else}}
                <span class="label label-important">
                    <asp:Literal ID="Literal17" Text='<%$Resources: Label, disable %>' runat="server" /></span>
                {{/if}}
            </td>
            <td class="span2" style="text-align: center">{{if Approved == 1}}
                    <span class="label label-success">
                        <asp:Literal ID="Literal23" Text='<%$Resources: Label, yes %>' runat="server" /></span>

                {{else}}
                    {{if Approved == 0}}
                    <span class="label label-important">
                        <asp:Literal ID="Literal24" Text='<%$Resources: Label, no %>' runat="server" /></span>
                {{else}}
                <span>-</span>
                {{/if}}
                {{/if}}
            </td>
            <td class="span2" style="text-align: center">{{if AcceptedTerms == 1}}
                    <span class="label label-success">
                        <asp:Literal ID="Literal18" Text='<%$Resources: Label, yes %>' runat="server" /></span>

                {{else}}
                    {{if AcceptedTerms == 0}}
                    <span class="label label-important">
                        <asp:Literal ID="Literal19" Text='<%$Resources: Label, no %>' runat="server" /></span>
                {{else}}
                <span>-</span>
                {{/if}}
                {{/if}}
            </td>
            <td class="span1" style="text-align: center">
                <button class="btn edit" onclick="redirectEditUser('${Id}');return false;"><i class="fa fa-edit"></i></button>
            </td>
        </tr>
    </script>
    <script type="text/javascript">
        var currentTable = null;

        $(document).ready(function () {
            var value = getValueByQueryString('ut');
            var types = value.split(",");
            for (var i in types) {
                $("#" + types[i]).attr('checked', true);
                $("#" + types[i]).uniform();
            }

            searchUsers();
        });

        function searchUsers() {
            //coloca o loading
            setLoadingToDiv($(".main-form"), '<asp:Literal Text="<%$Resources: Label, loading_users %>" runat="server" />');

            var itens = [];

            $("#filterBlock input:checked").each(function (i, v) {
                itens.push($(v).attr('id'));
            });
            $("#resultMessage").html('<asp:Literal Text="<%$Resources: Label, wait %>" runat="server" />');
            $.ajax({
                url: "../Handlers/User/GetAll.ashx",
                data: { ut: itens.join(), st: $("#ddlStatus").val() },
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
                        { "bSortable": true, "bSearchable": false, "aTargets": [4] },
                        { "bSortable": true, "bSearchable": false, "aTargets": [5] },
                        { "bSortable": true, "bSearchable": false, "aTargets": [6] },
                        { "bSortable": false, "bSearchable": false, "aTargets": [7] }];

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

        function redirectEditUser(id) {
            window.location = 'NewUser.aspx?id=' + encodeURIComponent(id);
        }
    </script>
</asp:Content>
