<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="NewUser.aspx.cs" Inherits="Ferramenta.User.NewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdnUrl" runat="server" Value="" />
    <asp:HiddenField runat="server" ID="hdUserId" Value="" />
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

                    <div class="control-group">
                        <label class="control-label">
                            <asp:Literal ID="Literal1" Text='<%$Resources: Label, register_type %>' runat="server" /></label>
                        <div class="controls">
                            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="span6 validate[required]" AutoPostBack="true" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <asp:PlaceHolder runat="server" ID="phAdminForm" Visible="false">
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal2" Text='<%$Resources: Label, general_informations %>' runat="server" />
                            </legend>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal3" Text='<%$Resources: Label, name %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtAdminName" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <asp:PlaceHolder ID="phOrganization" runat="server" Visible="false">
                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal4" Text='<%$Resources: Label, organization %>' runat="server" /></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtAdminOrganization" CssClass="span6 validate[required]" runat="server" />
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal5" Text='<%$Resources: Label, email %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtAdminEmail" CssClass="span6 validate[required, custom[email]]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal6" Text='<%$Resources: Label, status %>' runat="server" /></label>
                                <div class="controls">
                                    <div class="toggle-button">
                                        <asp:CheckBox ID="chkAdminStatus" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal7" Text='<%$Resources: Label, login_informations %>' runat="server" /></legend>
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal8" Text='<%$Resources: Label, login %>' runat="server" /></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtAdminLogin" CssClass="span6 validate[required, minSize[6], maxSize[30]]" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal9" Text='<%$Resources: Label, password %>' runat="server" /></label>
                                    <div class="controls">
                                        <asp:TextBox TextMode="Password" ID="txtAdminPassword" CssClass="span6 validate[required, minSize[6], maxSize[30]]" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phEntityForm" Visible="false">
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal10" Text='<%$Resources: Label, general_informations %>' runat="server" />
                            </legend>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal11" Text='<%$Resources: Label, organization_name %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityName" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal12" Text='<%$Resources: Label, cnpj %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityCNPJ" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal13" Text='<%$Resources: Label, institutional_email %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityEmail" CssClass="span6 validate[required, custom[email]]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal14" Text='<%$Resources: Label, nature %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlEntityNature" CssClass="span6 validate[required]">
                                        <asp:ListItem Text="Selecione" Value="" />
                                        <asp:ListItem Text="Academia" Value="Academia" />
                                        <asp:ListItem Text="Movimento social" Value="Movimento social" />
                                        <asp:ListItem Text="Organização Não Governamental" Value="Organização Não Governamental" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal15" Text='<%$Resources: Label, operation_area %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityArea" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal16" Text='<%$Resources: Label, operation_range %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlEntityRange" CssClass="span6 validate[required]">
                                        <asp:ListItem Text="Selecione" Value="" />
                                        <asp:ListItem Text="Estadual" Value="Estadual" />
                                        <asp:ListItem Text="Internacional" Value="Internacional" />
                                        <asp:ListItem Text="Municipal" Value="Municipal" />
                                        <asp:ListItem Text="Nacional" Value="Nacional" />
                                        <asp:ListItem Text="Regional" Value="Regional" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal17" Text='<%$Resources: Label, status %>' runat="server" /></label>
                                <div class="controls">
                                    <div class="toggle-button">
                                        <asp:CheckBox ID="chkEntityStatus" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal18" Text='<%$Resources: Label, accredited %>' runat="server" /></legend>
                        </fieldset>
                        <div class="control-group">
                            <label class="control-label">
                                <asp:Literal ID="Literal19" Text='<%$Resources: Label, question_this_entity_is_accredited %>' runat="server" /></label>
                            <div class="controls">
                                <div id="network_check">
                                    <asp:CheckBox ID="chkEntityNetwork" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div id="approvedBlock" runat="server" class="control-group" style="display: none">
                            <label class="control-label">
                                <asp:Literal ID="Literal20" Text='<%$Resources: Label, question_approve_that_this_entity_is_accredited %>' runat="server" /></label>
                            <div class="controls">
                                <div id="approved_check">
                                    <asp:CheckBox ID="chkEntityApproved" runat="server" Checked="false" />
                                </div>
                                <small>
                                    <asp:Literal runat="server" ID="ltApprovedBy"></asp:Literal></small>
                            </div>
                        </div>
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal21" Text='<%$Resources: Label, address_informations %>' runat="server" /></legend>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal22" Text='<%$Resources: Label, address %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityAddress" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal23" Text='<%$Resources: Label, number %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityNumber" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal24" Text='<%$Resources: Label, neighborhood %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityNeighborhood" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal25" Text='<%$Resources: Label, region %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlEntityRegion" CssClass="span6 validate[required]">
                                        <asp:ListItem Text="Selecione" Value="" />
                                        <asp:ListItem Text="Centro-Oeste" Value="Centro-Oeste" />
                                        <asp:ListItem Text="Nordeste" Value="Nordeste" />
                                        <asp:ListItem Text="Norte" Value="Norte" />
                                        <asp:ListItem Text="Sudeste" Value="Sudeste" />
                                        <asp:ListItem Text="Sul" Value="Sul" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal26" Text='<%$Resources: Label, state %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlEntityState" CssClass="span6 validate[required]" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityEstate_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal27" Text='<%$Resources: Label, city %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlEntityCity" CssClass="span6 validate[required]" DataTextField="Name" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal28" Text='<%$Resources: Label, zipcode %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityZipCode" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>


                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal29" Text='<%$Resources: Label, contact_informations %>' runat="server" /></legend>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal30" Text='<%$Resources: Label, organization_representative_name %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityContactName" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal31" Text='<%$Resources: Label, organization_representative_email %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityContactEmail" CssClass="span6 validate[required, custom[email]]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal32" Text='<%$Resources: Label, phone %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntityPhone" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal33" Text='<%$Resources: Label, website %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEntitySite" CssClass="span6 validate[custom[url]]" runat="server" placeholder="Exemplo: http://www.meusite.com.br" />
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal34" Text='<%$Resources: Label, login_informations %>' runat="server" /></legend>
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal35" Text='<%$Resources: Label, login %>' runat="server" /></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtEntityLogin" CssClass="span6 validate[required, minSize[6], maxSize[30]]" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal36" Text='<%$Resources: Label, password %>' runat="server" /></label>
                                    <div class="controls">
                                        <asp:TextBox TextMode="Password" ID="txtEntityPassword" CssClass="span6 validate[required, minSize[6], maxSize[30]]" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phCommonForm" Visible="false">
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal37" Text='<%$Resources: Label, general_informations %>' runat="server" />
                            </legend>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal38" Text='<%$Resources: Label, name %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCommonName" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal39" Text='<%$Resources: Label, email %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCommonEmail" CssClass="span6 validate[required, custom[email]]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal40" Text='<%$Resources: Label, nature %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlCommonNature" CssClass="span6 validate[required]">
                                        <asp:ListItem Text="Selecione" Value="" />
                                        <asp:ListItem Text="Academia" Value="Academia" />
                                        <asp:ListItem Text="Conselho" Value="Conselho" />
                                        <asp:ListItem Text="Empresa" Value="Empresa" />
                                        <asp:ListItem Text="Governo" Value="Governo" />
                                        <asp:ListItem Text="Movimento Social" Value="Movimento Social" />
                                        <asp:ListItem Text="Organização Não Governamental" Value="Organização Não Governamental" />
                                        <asp:ListItem Text="Outros" Value="Outros" />
                                        <asp:ListItem Text="Pessoa Física" Value="Pessoa Física" />
                                        <asp:ListItem Text="Representativa" Value="Representativa" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal41" Text='<%$Resources: Label, operation_area %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCommonArea" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal42" Text='<%$Resources: Label, operation_range %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlCommonRange" CssClass="span6 validate[required]">
                                        <asp:ListItem Text="Selecione" Value="" />
                                        <asp:ListItem Text="Estadual" Value="Estadual" />
                                        <asp:ListItem Text="Internacional" Value="Internacional" />
                                        <asp:ListItem Text="Municipal" Value="Municipal" />
                                        <asp:ListItem Text="Nacional" Value="Nacional" />
                                        <asp:ListItem Text="Regional" Value="Regional" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal43" Text='<%$Resources: Label, status %>' runat="server" /></label>
                                <div class="controls">
                                    <div class="toggle-button">
                                        <asp:CheckBox ID="chkCommonStatus" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal44" Text='<%$Resources: Label, address_informations %>' runat="server" /></legend>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal45" Text='<%$Resources: Label, address %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCommonAddress" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal46" Text='<%$Resources: Label, number %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCommonNumber" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal47" Text='<%$Resources: Label, neighborhood %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCommonNeighborhood" CssClass="span6 validate[required]" runat="server" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal48" Text='<%$Resources: Label, region %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlCommonRegion" CssClass="span6 validate[required]">
                                        <asp:ListItem Text="Selecione" Value="" />
                                        <asp:ListItem Text="Centro-Oeste" Value="Centro-Oeste" />
                                        <asp:ListItem Text="Nordeste" Value="Nordeste" />
                                        <asp:ListItem Text="Norte" Value="Norte" />
                                        <asp:ListItem Text="Sudeste" Value="Sudeste" />
                                        <asp:ListItem Text="Sul" Value="Sul" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal49" Text='<%$Resources: Label, state %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlCommonState" CssClass="span6 validate[required]" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlCommonState_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal50" Text='<%$Resources: Label, city %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:DropDownList runat="server" ID="ddlCommonCity" CssClass="span6 validate[required]" DataTextField="Name" DataValueField="Id">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal51" Text='<%$Resources: Label, zipcode %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCommonZipCode" CssClass="span6 validate[required]" runat="server" placeholder="Exemplo: 00000-111" />
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal52" Text='<%$Resources: Label, contact_informations %>' runat="server" /></legend>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal53" Text='<%$Resources: Label, phone %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCommonPhone" CssClass="span6 validate[required]" runat="server" placeholder="Exemplo: (11) 555551111" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">
                                    <asp:Literal ID="Literal54" Text='<%$Resources: Label, website %>' runat="server" /></label>
                                <div class="controls">
                                    <asp:TextBox ID="txtCommonSite" CssClass="span6 validate[custom[url]]" runat="server" placeholder="Exemplo: http://www.meusite.com.br" />
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>
                                <asp:Literal ID="Literal55" Text='<%$Resources: Label, login_informations %>' runat="server" /></legend>
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal56" Text='<%$Resources: Label, login %>' runat="server" /></label>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCommonLogin" CssClass="span6 validate[required, minSize[6], maxSize[30]]" runat="server" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">
                                        <asp:Literal ID="Literal57" Text='<%$Resources: Label, password %>' runat="server" /></label>
                                    <div class="controls">
                                        <asp:TextBox TextMode="Password" ID="txtCommonPassword" CssClass="span6 validate[required, minSize[6], maxSize[30]]" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </asp:PlaceHolder>
                    <div class="control-group">
                        <label class="control-label">
                            <asp:Literal ID="Literal58" Text='<%$Resources: Label, profile_photo %>' runat="server" /></label>
                        <div class="controls">
                            <asp:FileUpload ID="fpUserPhoto" runat="server" />
                            <asp:Image ID="imgUser" ImageUrl="~/Design/Images/noavatar.png" CssClass="media-object img-polaroid user-img" runat="server" Visible="false" />
                        </div>
                    </div>
                    <asp:PlaceHolder ID="phResponsableCollabCities" runat="server" Visible="false">
                        <div class="container-fluid row">
                            <div class="span6">
                                <fieldset>
                                    <legend>
                                        <asp:Literal ID="Literal59" Text='<%$Resources: Label, collaborator_by_cities %>' runat="server" /></legend>
                                    <div class="form-horizontal">
                                        <asp:Repeater ID="rptCollabCities" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-bordered table-condensed table-hover table-striped">
                                                    <thead>
                                                        <tr>
                                                            <th style="vertical-align: middle">
                                                                <asp:Literal ID="Literal14" Text='<%$Resources: Label, city  %>' runat="server" /></th>
                                                            <th style="vertical-align: middle">
                                                                <asp:Literal ID="Literal25" Text='<%$Resources: Label, state %>' runat="server" /></th>
                                                                                                                        <th style="vertical-align: middle">
                                                                <asp:Literal ID="Literal61" Text='<%$Resources: Label, period %>' runat="server" /></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#Eval("City") %></td>
                                                    <td><%#Eval("State") %></td>
                                                    <td><%#Eval("Period") %></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody></table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="span6">
                                <fieldset>
                                    <legend>
                                        <asp:Literal ID="Literal60" Text='<%$Resources: Label, responsable_by_cities %>' runat="server" /></legend>
                                    <div class="form-horizontal">
                                        <asp:Repeater ID="rptResponsableCities" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-bordered table-condensed table-hover table-striped">
                                                    <thead>
                                                        <tr>
                                                            <th style="vertical-align: middle">
                                                                <asp:Literal ID="Literal14" Text='<%$Resources: Label, city  %>' runat="server" /></th>
                                                            <th style="vertical-align: middle">
                                                                <asp:Literal ID="Literal25" Text='<%$Resources: Label, state %>' runat="server" /></th>
                                                            <th style="vertical-align: middle">
                                                                <asp:Literal ID="Literal61" Text='<%$Resources: Label, period %>' runat="server" /></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#Eval("City") %></td>
                                                    <td><%#Eval("State") %></td>
                                                    <td><%#Eval("Period") %></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody></table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <div class="form-actions">
                        <asp:Button ID="Button1" CssClass="btn btn-primary " Text="Salvar" OnClientClick="validate();" OnClick="btnSave_Click" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#<%=txtEntityCNPJ.ClientID%>').setMask('cnpj');

            $('#<%=txtCommonZipCode.ClientID%>').setMask('cep');
            $('#<%=txtEntityZipCode.ClientID%>').setMask('cep');
            $('#<%=txtCommonPhone.ClientID%>').setMask('phone');
            $('#<%=txtEntityPhone.ClientID%>').setMask('phone');

            $('.toggle-button').toggleButtons({
                width: 130,
                label: {
                    enabled: '<asp:Literal Text="<%$Resources: Label, active %>" runat="server" />',
                    disabled: '<asp:Literal Text="<%$Resources: Label, inactive %>" runat="server" />'
                },
                style: {
                    enabled: "success",
                    disabled: "danger"
                }
            });

            $('#network_check').toggleButtons({
                width: 130,
                label: {
                    enabled: '<asp:Literal Text="<%$Resources: Label, yes %>" runat="server" />',
                    disabled: '<asp:Literal Text="<%$Resources: Label, no %>" runat="server" />'
                },
                style: {
                    enabled: "success",
                    disabled: "danger"
                },
                onChange: function ($el, status, e) {
                    if (status) {
                        $('#<%=approvedBlock.ClientID%>').fadeIn();
                    } else {
                        if ($('#<%=chkEntityApproved.ClientID%>').attr('checked') == 'checked') {
                            $('#approved_check').toggleButtons('toggleState');
                        }

                        $('#<%=approvedBlock.ClientID%>').hide();
                    }
                }
            });

            $('#approved_check').toggleButtons({
                width: 130,
                label: {
                    enabled: '<asp:Literal Text="<%$Resources: Label, yes %>" runat="server" />',
                    disabled: '<asp:Literal Text="<%$Resources: Label, no %>" runat="server" />'
                },
                style: {
                    enabled: "success",
                    disabled: "danger"
                }
            });

            $('.table').dataTable(DATATABLE_SETTINGS);
        });

        function validate() {
            if (!$("#form1").validationEngine('validate')) {
                removeLoadingToDiv($("#form_new_user"));
                return false;
            }
        }

    </script>
</asp:Content>
