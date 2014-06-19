<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="NewPeriod.aspx.cs" Inherits="Ferramenta.Period.NewPeriod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hdnPeriodId" Value="0" />
    <div class="container-fluid outer">
        <div class="main-form">
            <div class="span12">
                <asp:PlaceHolder ID="phMessageWarning" runat="server" Visible="false">
                    <div class="control-group">
                        <div class="alert-block alert-info">
                            <div style="margin-left: 4px">
                                <i class="fa fa-info-circle fa-lg"></i>
                                <asp:Label ID="ltMessageWarning" Text="" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="phMessageWarning2" runat="server" Visible="false">
                    <div class="control-group">
                        <div class="alert-block alert-info">
                            <div style="margin-left: 4px">
                                <i class="fa fa-info-circle fa-lg"></i>
                                <asp:Label ID="ltMessageWarning2" Text="" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
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
                <div class="box">
                    <header>
                        <div class="icons">
                            <i class="fa fa-share "></i>
                        </div>
                        <h5>
                            <asp:Literal ID="Literal1" Text='<%$Resources: Label, period_register %>' runat="server" /></h5>
                        <ul class="nav pull-right">
                            <li>
                                <a href="#periodForm" data-toggle="collapse" class="accordion-toggle minimize-box">
                                    <i class="fa fa-chevron-down"></i>
                                </a>
                            </li>
                        </ul>
                    </header>
                    <div id="periodForm" class="form-horizontal" style="margin-top: 15px">
                        <div class="control-group">
                            <label for="text1" class="control-label">
                                <asp:Literal ID="Literal2" Text='<%$Resources: Label, name %>' runat="server" /></label>

                            <div class="controls">
                                <asp:TextBox CssClass="span6 input-tooltip validate[required]" ID="txtName" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="reservation">
                                <asp:Literal ID="Literal6" Text='<%$Resources: Label, convocation_period %>' runat="server" /></label>
                            <div class="controls">
                                <span><asp:Literal runat="server" Text='<%$Resources: Label, from %>' /></span>
                                <div class="input-prepend">
                                    <span class="add-on"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox CssClass="validate[required]" data-date-format="dd/mm/yyyy" Style="cursor: pointer; background-color: #fff" ID="txtConvocationInitialDate" runat="server"></asp:TextBox>
                                </div>
                                <span><asp:Literal ID="Literal7" runat="server" Text='<%$Resources: Label, to %>' /></span>
                                <div class="input-prepend">
                                    <span class="add-on"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox CssClass="validate[required]" data-date-format="dd/mm/yyyy" Style="cursor: pointer; background-color: #fff" ID="txtConvocationFinalDate" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="reservation">
                                <asp:Literal ID="Literal3" Text='<%$Resources: Label, submission_period %>' runat="server" /></label>
                            <div class="controls">
                                <span><asp:Literal ID="Literal8" runat="server" Text='<%$Resources: Label, from %>' /></span>
                                <div class="input-prepend">
                                    <span class="add-on"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox CssClass="validate[required]" data-date-format="dd/mm/yyyy" Style="cursor: pointer; background-color: #fff" ID="txtInitialDate" runat="server"></asp:TextBox>
                                </div>
                                <span><asp:Literal ID="Literal9" runat="server" Text='<%$Resources: Label, to %>' /></span>
                                 <div class="input-prepend">
                                    <span class="add-on"><i class="fa fa-calendar"></i></span>
                                    <asp:TextBox CssClass="validate[required]" data-date-format="dd/mm/yyyy" Style="cursor: pointer; background-color: #fff" ID="txtFinalDate" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <%--<div class="control-group">
                            <label class="control-label" for="reservation">
                                <asp:Literal ID="Literal4" Text='<%$Resources: Label, final_date %>' runat="server" /></label>
                            <div class="controls">
                                <span class="add-on"><i class="fa fa-calendar"></i></span>
                               
                            </div>
                        </div>--%>
                        <div class="control-group">
                            <label class="control-label">
                                <asp:Literal ID="Literal5" Text='<%$Resources: Label, status %>' runat="server" /></label>
                            <div class="controls">
                                <div id="text-toggle-button">
                                    <asp:CheckBox ID="chkAberto" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-actions">
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" class="btn btn-primary" Text='<%$Resources: Label, save %>' OnClientClick="return validate();" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#text-toggle-button').toggleButtons({
                width: 130,
                label: {
                    enabled: '<asp:Literal Text="<%$Resources: Label, open %>" runat="server" />',
                    disabled: '<asp:Literal Text="<%$Resources: Label, closed %>" runat="server" />'
                },
                style: {
                    enabled: "success",
                    disabled: "danger"
                }
            });

            var nowTemp = new Date();
            var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);

            var submissionInitial = $('#<%= txtConvocationInitialDate.ClientID %>').datepicker({
                language: 'pt-BR',
                onRender: function (date) {
                    return date.valueOf() < now.valueOf() ? 'disabled' : '';
                }
            }).on('changeDate', function (ev) {
                if (ev.date.valueOf() > submissionFinal.date.valueOf()) {
                    var newDate = new Date(ev.date)
                    newDate.setDate(newDate.getDate() + 1);
                    submissionFinal.setValue(newDate);
                }
                submissionInitial.hide();
                $('#<%= txtConvocationFinalDate.ClientID %>')[0].focus();
            }).data('datepicker');

            var submissionFinal = $('#<%= txtConvocationFinalDate.ClientID %>').datepicker({
                language: 'pt-BR',
                onRender: function (date) {
                    return date.valueOf() <= submissionInitial.date.valueOf() ? 'disabled' : '';
                }
            }).on('changeDate', function (ev) {
                if (ev.date.valueOf() > checkin.date.valueOf()) {
                    var newDate = new Date(ev.date)
                    newDate.setDate(newDate.getDate() + 1);
                    checkin.setValue(newDate);
                }
                submissionFinal.hide();
                $('#<%= txtInitialDate.ClientID %>')[0].focus();
            }).data('datepicker');


            var checkin = $('#<%= txtInitialDate.ClientID %>').datepicker({
                language: 'pt-BR',
                onRender: function (date) {
                    return date.valueOf() < now.valueOf() || date.valueOf() <= submissionFinal.date.valueOf() ? 'disabled' : '';
                }
            }).on('changeDate', function (ev) {
                if (ev.date.valueOf() > checkout.date.valueOf()) {
                    var newDate = new Date(ev.date)
                    newDate.setDate(newDate.getDate() + 1);
                    checkout.setValue(newDate);
                }
                checkin.hide();
                $('#<%= txtFinalDate.ClientID %>')[0].focus();
            }).data('datepicker');

            var checkout = $('#<%= txtFinalDate.ClientID %>').datepicker({
                language: 'pt-BR',
                onRender: function (date) {
                    return date.valueOf() <= checkin.date.valueOf() ? 'disabled' : '';
                }
            }).on('changeDate', function (ev) {
                checkout.hide();
            }).data('datepicker');
        })

        function validate() {
            if (!$("#form1").validationEngine('validate')) {
                return false;
            }
        }
    </script>
</asp:Content>
