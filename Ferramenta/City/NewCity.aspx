<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="NewCity.aspx.cs" Inherits="Ferramenta.City.NewCity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hdCityId" Value="" />
    <asp:HiddenField runat="server" ID="hdOpenPeriodId" Value="" />
    <asp:HiddenField runat="server" ID="hdCollabJson" Value="" />
    <asp:HiddenField runat="server" ID="hdCurrentCollabs" Value="" />
    <asp:HiddenField runat="server" ID="hdCurrentCollabsJson" Value="" />
    <div class="container-fluid outer" id="form_new_user">
        <div class="row-fluid">
            <div class="span12 inner">
                <div class="form-horizontal">
                    <asp:PlaceHolder ID="phMessageError" runat="server" Visible="false">
                        <div class="control-group">
                            <div class="alert-block alert-error">
                                <div style="margin-left: 4px">
                                    <i class="fa fa-warning fa-lg"></i>
                                    <asp:Label ID="lblMessageError" Text="" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phMessageSuccess" runat="server" Visible="false">
                        <div class="control-group">
                            <div class="alert-block alert-success">
                                <div style="margin-left: 4px">
                                    <i class="fa fa-thumbs-o-up fa-lg"></i>
                                    <asp:Label ID="lblMessageSuccess" Text="" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phMessageWarning" runat="server" Visible="false">
                        <div class="control-group">
                            <div class="alert-block alert-info">
                                <div style="margin-left: 4px">
                                    <i class="fa fa-info-circle fa-lg"></i>
                                    <asp:Label ID="lblMessageWarning" Text="" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <fieldset>
                        <legend>
                            <asp:Literal ID="Literal2" Text='<%$Resources: Label, city_informations %>' runat="server" />
                        </legend>
                        <div class="control-group">
                            <label class="control-label">
                                <asp:Literal ID="Literal3" Text='<%$Resources: Label, city_name %>' runat="server" /></label>
                            <div class="controls">
                                <asp:TextBox ID="txtCityName" CssClass="span6 validate[required]" runat="server" />
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">
                                <asp:Literal ID="Literal5" Text='<%$Resources: Label, state %>' runat="server" /></label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlState" runat="server" DataTextField="Name" DataValueField="Id">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </fieldset>

                    <asp:PlaceHolder ID="phOpenPeriodInformation" runat="server" Visible="true">
                        <fieldset>
                            <legend>
                                <asp:Literal ID="ltOpenPeriodTitle" Text="" runat="server" />
                            </legend>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal4" Text='<%$Resources: Label, responsable %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddlResponsable" runat="server" DataTextField="Name" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal1" Text='<%$Resources: Label, collaborators %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCollaborators" CssClass="span12 validate[required]" runat="server" Enabled="false" />
                                    <asp:HiddenField ID="hdCollabValues" runat="server" Value="" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal6" Text='<%$Resources: Label, add_a_new_collaborator %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddlCollaborator" runat="server" DataTextField="Name" DataValueField="Id">
                                    </asp:DropDownList>
                                    <button type="button" onclick="addCollab()" id="btnAddCollab" class="btn btn-success btn-sm"><i class="fa fa-fw fa-1x fa-plus"></i></button>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal7" Text="Solicitações pendentes" runat="server" />
                            </legend>
                            <div class="control-group" style="padding-top: 20px">
                                <table id="dataTable" class="table table-bordered table-condensed table-hover table-striped">
                                    <thead>
                                        <tr>
                                            <th style="vertical-align: middle">
                                                <asp:Literal ID="Literal14" Text='<%$Resources: Label, entity_name  %>' runat="server" /></th>
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

                        </fieldset>
                    </asp:PlaceHolder>
                    <%-- <fieldset>
                            <legend><asp:Literal ID="Literal7" Text='<%$Resources: Label, requests_pending %>' runat="server" /></legend>
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <label class="control-label"><asp:Literal ID="Literal8" Text='<%$Resources: Label, login %>' runat="server" /></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtAdminLogin" CssClass="span6 validate[required, minSize[6], maxSize[30]]" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label"><asp:Literal ID="Literal9" Text='<%$Resources: Label, password %>' runat="server" /></label>
                                    <div class="controls">
                                        <asp:TextBox TextMode="Password" ID="txtAdminPassword" CssClass="span6 validate[required, minSize[6], maxSize[30]]" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>--%>
                    <div class="form-actions">
                        <asp:Button ID="btnSave" CssClass="btn btn-primary " Text="Salvar" OnClientClick="validate();" OnClick="btnSave_Click" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/x-jquery-tmpl" id="row">
        <tr>
             <td><a href="/User/NewUser.aspx?id=${EntityId}">${EntityName}</a></td>
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
            <td style="text-align: center; vertical-align: middle">
                {{if RequestStatus == 0}}
                    <span class="label label-warning">
                        <asp:Literal ID="Literal8" Text='<%$Resources: Label, waiting_approve %>' runat="server" /></span>
                {{/if}}
                {{if RequestStatus == 1}}
                    <span class="label label-success">
                        <asp:Literal ID="Literal9" Text='<%$Resources: Label, approved %>' runat="server" /></span>
                {{/if}}</td>
            <td class="span2" style="text-align: center">
                {{if RequestStatus == 0}}
                <button class="btn btn-success" onclick="approveRequest('${Id}');return false;"><i class="fa fa-check"></i> Aprovar</button>
                {{/if}}
            </td>
        </tr>
    </script>
    <script type="text/javascript">
        var currentTable = null;
        $(document).ready(function () {
            $('#<%=txtCollaborators.ClientID%>').tagsinput({
                itemValue: 'Id',
                itemText: 'Name',
                typeahead: {
                    source: function (query) {
                        return $.parseJSON($("#<%=hdCollabJson.ClientID%>").val());
                    }
                }
            });

            var list = $.parseJSON($("#<%=hdCurrentCollabsJson.ClientID%>").val());
            for (var i in list) {
                $('#<%=txtCollaborators.ClientID%>').tagsinput('add', list[i]);
            }

            loadRequest();
        });


        function approveRequest(id) {
            $.ajax({
                url: "../Handlers/GroupCities/ApproveRequest.ashx",
                data: { rId: id},
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

            $.ajax({
                url: "../Handlers/GroupCities/GetRequests.ashx",
                data: { cId: $("#<%=hdCityId.ClientID%>").val(), pId: $("#<%=hdOpenPeriodId.ClientID%>").val() },
                type: 'POST',
                cache: false,
                success: function (data) {
                    if (currentTable != null) {
                        currentTable.fnClearTable();
                        currentTable.fnDestroy();
                    }

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

        function validate() {
            $("#<%=hdCollabValues.ClientID%>").val($('#<%=txtCollaborators.ClientID%>').val());
        }

        function addCollab() {
            var selectedCollabId = $('#<%=ddlCollaborator.ClientID%>').val();
            var selectedCollabName = $('#<%=ddlCollaborator.ClientID%> option[value="' + selectedCollabId + '"]').text();

            var selectedCollabs = $('#<%=txtCollaborators.ClientID%>').val().split(",");

            var contains = false;
            for (var i in selectedCollabs) {
                if (selectedCollabs[i] == selectedCollabId)
                    contains = true;
            }

            if (!contains) {
                $('#<%=txtCollaborators.ClientID%>').tagsinput('add', { Id: selectedCollabId, Name: selectedCollabName });
            }
        }
    </script>
</asp:Content>
