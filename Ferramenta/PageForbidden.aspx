<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="PageForbidden.aspx.cs" Inherits="Ferramenta.PageForbidden" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid outer">
        <div class="main-form" style="height:500px">
            <h5>
                <asp:Literal ID="lblMessage" runat="server" />
            </h5>
        </div>
    </div>
</asp:Content>
