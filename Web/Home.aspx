<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Home.aspx.cs" Inherits="Site.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <!--Stylesheet-->
    <link href="<%= @System.Web.Optimization.BundleTable.
      Bundles.ResolveBundleUrl("~/Design/Styles/base") %>"
        rel="Stylesheet" type="text/css" />


    <script src="<%= @System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl("~/Scripts/jquery") %>" type="text/javascript"></script>
    <script src="<%= @System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl("~/Scripts/jqueryui") %>" type="text/javascript"></script>
    <script src="<%= @System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl("~/Scripts/base") %>" type="text/javascript"></script>
    <script src="<%= @System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl("~/Scripts/plugins") %>" type="text/javascript"></script>
    <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnDownload" Text="Exportar questionário" runat="server" OnClick="btnDownload_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    Estado/cidade do formulário
                </td>
                <td>
                    <asp:DropDownList ID="ddlState" runat="server" onchange="return getCities();"></asp:DropDownList>
                    <asp:DropDownList ID="ddlCities" runat="server" onchange="getCityId(); return false;"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Entidades
                </td>
                <td>
                    <asp:DropDownList ID="ddlEntidades" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="fuForm" runat="server" />
                </td>
                <td>
                    <asp:Button ID="btnImport" Text="Importar questionário" OnClick="btnImport_Click" runat="server" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnCityId" Value="0" runat="server" />
    </div>

    <asp:Repeater runat="server" ID="rptListBlocks">
        <ItemTemplate>
            <h2>
                <asp:Label ID="Label1" Text='<%#((System.Data.DataTable)Container.DataItem).TableName %>' runat="server" /></h2>
            <hr />
            <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="true" DataSource='<%#Container.DataItem %>'></asp:GridView>
        </ItemTemplate>
        <FooterTemplate>
            <br />
        </FooterTemplate>
    </asp:Repeater>
        
        <asp:Literal ID="litNotAnsweredQuestions" runat="server"></asp:Literal>
        <hr />
        <h1 style="margin-top: 20px;">Questionários já enviados.</h1>
        <asp:GridView ID="gvFormsRanking" runat="server" AutoGenerateColumns="false" style="margin-top: 40px;" OnRowCommand="gvFormsRanking_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Nome da cidade">
                    <ItemTemplate>
                        <%# Eval("CityName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nome do questionário">
                    <ItemTemplate>
                        <%# Eval("FormName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pontuação">
                    <ItemTemplate>
                        <%# Eval("TotalScore") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nome da entidade">
                    <ItemTemplate>
                        <%# Eval("UserName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkViewForm" runat="server" Text="Visualizar" CommandArgument='<%# Eval("Id") %>' CommandName="ViewForm"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
            <div id="master_alert" class="alert" style="display: none">
        <span></span>
    </div>
        <asp:HiddenField ID="hdnError" runat="server" Value="" />
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            executeErrorMsg();
        });

        function getCityId() {
            $('#<%= hdnCityId.ClientID %>').val($("#<%= ddlCities.ClientID%>").val());
        }

        function getCities() {
            $("#<%= ddlCities.ClientID%>").html('<option>Carregando...</option>');
            $("#<%= ddlCities.ClientID%>").attr('disabled', true);

            $.ajax({
                url: "/Handlers/GetCities.ashx",
                type: 'POST',
                data: { sid: $('#<%= ddlState.ClientID %>').val() },
                cache: false,
                success: function (data) {
                    var options = [];

                    $('#<%= hdnCityId.ClientID %>').val(data[0].CityId);

                    for (var i = 0; i < data.length; i++) {
                        options.push('<option value="',
                          data[i].CityId, '">',
                          data[i].CityName, '</option>');
                    }

                    $("#<%= ddlCities.ClientID%>").html(options.join(''));
                },
                complete: function (event) {
                    $("#<%= ddlCities.ClientID%>").attr('disabled', false);
                }
            });

            return false;
        }

        function executeErrorMsg() {
            var msg = $('#<%= hdnError.ClientID%>').val();

            if (msg != '' && msg != undefined) {
                showAlert(msg, 'error', 5000);
            }

            $('#<%= hdnError.ClientID%>').val('');
        }
    </script>
</body>
</html>
