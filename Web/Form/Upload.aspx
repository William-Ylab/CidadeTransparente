<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Upload.aspx.cs" Inherits="Site.Form.Upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdPeriodId" runat="server" />
    <div class="container">
        <ul class="breadcrumbs styler_bg_color">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text='<%$Resources: Message, title_expimpform %>'></asp:Label></li>
        </ul>
        <div>
            <div class="col-md-12 col-xs-12 col-sm-6">
                <h3>
                    <asp:Label ID="lblMessage" runat="server" Text='<%$Resources: Message, no_question %>'></asp:Label></h3>
                <ul id="ulsteps" runat="server" class="styled_list" data-color="#d94c4b, #d56558, #d7824c, #eda637, #86aa65, #8aabaf" data-type="process_box">
                    <li>
                        <div class="name" style="width: 100%">
                            <asp:Literal ID="lit" runat="server" Text='<%$Resources: Message, export_name %>'></asp:Literal>
                        </div>
                        <div class="desc" style="width: 100%">
                            <asp:Literal ID="Literal1" runat="server" Text='<%$Resources: Message, export_value %>'></asp:Literal>
                            <asp:LinkButton ID="btnExport" runat="server" OnClick="btnDownload_Click" Text='<%$Resources: Message, click_here %>'></asp:LinkButton>
                        </div>
                    </li>
                    <li>
                        <div class="name" style="width: 100%">
                            <asp:Literal ID="Literal2" runat="server" Text='<%$Resources: Message, answer_name %>'></asp:Literal>
                        </div>
                        <div class="desc" style="width: 100%">
                            <asp:Literal ID="Literal3" runat="server" Text='<%$Resources: Message, answer_value %>'></asp:Literal>
                        </div>
                    </li>
                    <li runat="server" id="li_selectcity">
                        <div class="name" style="width: 100%">
                            <asp:Literal ID="Literal4" runat="server" Text='<%$Resources: Message, city_name %>'></asp:Literal>
                        </div>
                        <div class="desc" style="width: 100%">
                            <asp:Literal ID="Literal5" runat="server" Text='<%$Resources: Message, city_value %>'></asp:Literal><br />
                            <%--<asp:DropDownList ID="ddlState" runat="server" onchange="return getCities();"></asp:DropDownList>--%>
                            <asp:DropDownList ID="ddlCities" runat="server"></asp:DropDownList>
                        </div>
                    </li>
                    <li>
                        <div class="name" style="width: 100%">
                            <asp:Label ID="Literal8" runat="server" Text='<%$Resources: Message, file_name %>'></asp:Label>
                        </div>
                        <div class="desc" style="width: 100%">
                            <asp:Literal ID="Literal9" runat="server" Text='<%$Resources: Message, file_value %>'></asp:Literal>
                            <br />
                            <asp:FileUpload ID="fuForm" runat="server" />
                        </div>
                    </li>
                    <li id="li-last-number">
                        <asp:PlaceHolder ID="phCommonUser" runat="server">
                            <div class="name" style="width: 100%">
                                <asp:Label ID="Literal10" runat="server" Text='<%$Resources: Message, submit_name %>'></asp:Label>
                            </div>
                            <div class="desc" style="width: 100%">
                                <asp:Literal ID="Literal11" runat="server" Text='<%$Resources: Message, submit_value %>'></asp:Literal>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnImport_Click" Text='<%$Resources: Message, click_here %>' style="margin-left: 5px;"></asp:LinkButton>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phEntityUser" runat="server">
                            <div class="name" style="width: 100%">
                                <asp:Label ID="Label2" runat="server" Text='<%$Resources: Message, submit_name_entity %>'></asp:Label>
                            </div>
                            <div class="desc" style="width: 100%;">
                                <asp:Literal ID="Literal6" runat="server" Text='<%$Resources: Message, submit_value_entity %>'></asp:Literal>
                                <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick='return confirmAction();' OnClick="btnImport_Click" style="margin-left: 5px;" Text='<%$Resources: Message, click_here %>'></asp:LinkButton>
                            </div>
                        </asp:PlaceHolder>
                    </li>
                </ul>
            </div>
            <%-- <table>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnDownload" CssClass="styler_bg_color" Text='<%$Resources: Message, export_form %>' runat="server" OnClick="btnDownload_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal runat="server" ID="litTitle" Text='<%$Resources: Message, city_state_form %>'></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlState" runat="server" onchange="return getCities();"></asp:DropDownList>
                        <asp:DropDownList ID="ddlCities" runat="server" onchange="getCityId(); return false;"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal runat="server" ID="Literal1" Text='<%$Resources: Message, entities %>'></asp:Literal>
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
                        <asp:Button ID="btnImport" CssClass="styler_bg_color" Text='<%$Resources: Message, import_form %>' OnClick="btnImport_Click" runat="server" />
                    </td>
                </tr>
            </table>--%>
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
    </div>
    <asp:HiddenField ID="hdnError" runat="server" Value="" />
    <script type="text/javascript">
        $(document).ready(function () {
            executeErrorMsg();

            setTimeout(changeColor, 200);
        });


        function changeColor() {
            $('#li-last-number').find('.num').css('background-color', '#86aa65');
        }

        function confirmAction() {
            if (!confirm('<asp:Literal Text="<%$Resources: Message, are_you_sure_submit_form %>" runat="server" />')) {
                return false;
            } else {
                return true;
            }
        }

        function executeErrorMsg() {
            var msg = $('#<%= hdnError.ClientID%>').val();

            if (msg != '' && msg != undefined) {
                showAlert(msg, 'error', 5000);
            }

            $('#<%= hdnError.ClientID%>').val('');
            }
    </script>

</asp:Content>
