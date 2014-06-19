<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Site.Account.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <ul class="breadcrumbs styler_bg_color">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text='Iniciar sessão'></asp:Label></li>
        </ul>
        <div align="center">
            <div>
                <div class="form-group col-xs-7" style="float: none;">
                    <asp:TextBox ID="txtUser" runat="server" type="text" CssClass="form-control input-lg validate[required]" placeholder="Usuário"></asp:TextBox>
                </div>
                <div class="form-group col-xs-7" style="float: none;">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control input-lg validate[required]" placeholder="Senha"></asp:TextBox>
                </div>
                <div class="form-group col-xs-7" style="float: none;">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
                <div class="form-group col-xs-7" style="float: none;">
                    <a href="#" style="margin-right: 40px" data-toggle="modal" data-target="#myModal"><asp:Literal ID="Literal44" Text='<%$Resources: Message, recover_password %>' runat="server" /></a>
                    <a href="/Account/New.aspx" style="margin-right: 40px"><asp:Literal ID="Literal1" Text='<%$Resources: Message, register %>' runat="server" /></a>
                    <asp:Button ID="btnLogin" CssClass="styler_bg_color" runat="server" Text='<%$Resources: Message, insert_session %>' OnClick="btnLogin_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title"><asp:Literal ID="Literal2" Text='<%$Resources: Message, recover_password %>' runat="server" /></h4>
                </div>
                <div class="modal-body">
                    <p><asp:Literal ID="Literal3" Text='<%$Resources: Message, input_your_email %>' runat="server" /></p>
                    <input type="text" id="txtRecoveryPasswordEmail" class="form-control input-lg validate[required, custom[email]]" />
                    <p><asp:Literal ID="Literal4" Text='<%$Resources: Message, input_your_user %>' runat="server" /></p>
                    <input type="text" id="txtRecoveryPasswordUser" class="form-control input-lg validate[required]" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal"><asp:Literal ID="Literal5" Text='<%$Resources: Message, close %>' runat="server" /></button>
                    <button type="button" onclick="recoveryPassword(); return false;" class="btn btn-primary"><asp:Literal ID="Literal6" Text='<%$Resources: Message, recover_password %>' runat="server" /></button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function recoveryPassword() {
            if ($("#form1").validationEngine('validate')) {
                $.ajax({
                    url: "/Handlers/Account/RecoveryPassword.ashx",
                    type: 'POST',
                    data: {
                        email: $('#txtRecoveryPasswordEmail').val(),
                        user: $('#txtRecoveryPasswordUser').val()
                    },
                    cache: false,
                    success: function (data) {
                        $('#myModal').modal('hide');
                        $('#txtRecoveryPasswordEmail').val('');
                        $('#txtRecoveryPasswordUser').val('');

                        showAlert('<asp:Literal Text="<%$Resources: Message, we_sent_an_email_with_you_password_and_login %>" runat="server" />', 'success', 5000);
                    },
                    error: function (data) {
                        $('#myModal').modal('hide');
                        $('#txtRecoveryPasswordEmail').val('');
                        $('#txtRecoveryPasswordUser').val('');

                        showAlert('<asp:Literal Text="<%$Resources: Message, problem_to_send_your_passoword %>" runat="server" />', 'error', 5000);
                    }
                });
            }
            return false;
        }
    </script>
</asp:Content>
