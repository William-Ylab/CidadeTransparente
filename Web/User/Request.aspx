<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Request.aspx.cs" Inherits="Site.User.Request" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <ul class="breadcrumbs styler_bg_color">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text='<%$Resources: Message, request_city %>'></asp:Label></li>
        </ul>

        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phError" runat="server" Visible="false">
                    <div class="alert-block alert-danger">
                        <div style="margin-left: 4px">
                            <strong>
                                <asp:Literal ID="Literal24" Text='<%$Resources: Message, warning %>' runat="server" /></strong>
                            <asp:Literal ID="ltErrorMessage" Text='' runat="server" />
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label">
                    <asp:Literal ID="Literal3" Text='<%$Resources: Message, state %>' runat="server" /></label>
                <div class="col-sm-10">
                    <asp:DropDownList ID="ddlState" runat="server" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" CssClass="validate[required]"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label">
                    <asp:Literal ID="Literal4" Text='<%$Resources: Message, city %>' runat="server" /></label>
                <div class="col-sm-10">
                    <asp:DropDownList ID="ddlCity" DataTextField="Name" DataValueField="Id" runat="server" CssClass="validate[required]"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label">
                    <asp:Literal ID="Literal1" Text='<%$Resources: Message, request_type %>' runat="server" /></label>
                <div class="col-sm-10 radio-inline">
                    <asp:RadioButton ID="rbCollab" Text='<%$Resources: Message, I_will_be_the_collaborator %>' GroupName="rb" runat="server" Checked="true" />
                    <asp:RadioButton ID="rbResp" Text='<%$Resources: Message, I_will_be_the_responsable %>' GroupName="rb" runat="server" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-10 text-right">
                <a href="/User/ListForm.aspx">
                    <asp:Literal Text='<%$Resources: Message, cancel %>' runat="server" /></a>
                <button id="btnRequest" runat="server" class="btn btn-primary" onclick="if(!validate()) return;" onserverclick="btnRequest_Click"><i class="fa fa-fw fa-lg fa-check"></i><asp:Literal ID="Literal2" Text='<%$Resources: Message, do_request_city %>' runat="server" /></button>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function validate() {
            if (!$("#form1").validationEngine('validate')) {
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
