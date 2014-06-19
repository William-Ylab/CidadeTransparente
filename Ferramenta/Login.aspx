<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ferramenta.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>Amarribo - Governo Transparente</title>
    <link type="text/css" rel="stylesheet" href="Design/Styles/Bootstrap/bootstrap.css" />
    <link rel="stylesheet" href="Design/Styles/login.css" />
    <link href="http://netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="span4 offset4">
                    <div class="signin">
                        <div id="logo">
                            <img src="Design/Images/logo-amarribo.png" alt="Amarribo">
                        </div>
                        <div class="tab-content">
                            <div id="login" class="tab-pane active">
                                <form action="index.html" accept-charset="utf-8" method="post">
                                    <p class="muted tac">
                                        <asp:Literal ID="Literal44" Text='<%$Resources: Message, insert_your_login_and_password %>' runat="server" />
                                    </p>
                                    <div class="control-group">
                                        <div class="controls" style="text-align: center;">
                                            <div class="input-prepend">
                                                <span class="add-on"><i class="fa fa-user"></i></span>
                                                <asp:TextBox ID="txtUser" runat="server" placeholder='<%$Resources: Label, insert_your_user %>' />
                                            </div>
                                        </div>
                                        <div class="controls" style="text-align: center;">
                                            <div class="input-prepend">
                                                <span class="add-on"><i class="fa fa-lock"></i></span>
                                                <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" placeholder='<%$Resources: Label, insert_your_password %>' />
                                            </div>
                                        </div>
                                        <asp:PlaceHolder ID="phMessage" runat="server" Visible="false">
                                            <div class="controls" style="color: #b94a48;">
                                                <div class="alert-block">
                                                    <i class="fa fa-warning fa-lg"></i>
                                                    <asp:Label ID="lblMessage" Text="" runat="server" />
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                    </div>
                                    <button id="btnLogin" runat="server" class="btn btn-inverse btn-block " type="submit" onserverclick="btnLogin_ServerClick"><asp:Literal ID="Literal1" Text='<%$Resources: Label, do_login %>' runat="server" /></button>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
        <script>window.jQuery || document.write('<script src="assets/js/vendor/jquery-1.9.1.min.js"><\/script>')</script>
        <script type="text/javascript" src="assets/js/vendor/bootstrap.min.js"></script>--%>
    </form>
</body>
</html>
