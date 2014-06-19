<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Site.Form.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <ul class="breadcrumbs styler_bg_color">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text='<%$Resources: Message, sent_questions %>'></asp:Label></li>
        </ul>
        <asp:GridView ID="gvFormsRanking" runat="server" AutoGenerateColumns="false" CssClass="styler_table" Style="margin-top: 40px;" OnRowCommand="gvFormsRanking_RowCommand" 
            HeaderStyle-CssClass="styler_bg_color">
            <Columns>
                <asp:TemplateField HeaderText='<%$Resources: Message, name_of_city %>'>
                    <ItemTemplate>
                        <%# Eval("CityName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText='<%$Resources: Message, form_name %>'>
                    <ItemTemplate>
                        <%# Eval("FormName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText='<%$Resources: Message, pointing %>'>
                    <ItemTemplate>
                        <%# Eval("TotalScore") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText='<%$Resources: Message, entity_name_ %>'>
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
            <EmptyDataTemplate>
                <asp:Literal ID="litNoRows" runat="server" Text='<%$Resources: Message, no_row_forms %>'></asp:Literal>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
