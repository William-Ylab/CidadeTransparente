<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" CodeBehind="Profile.aspx.cs" Inherits="Site.User.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <ul class="breadcrumbs styler_bg_color">
            <li>
                <asp:Label ID="lblTitle" runat="server" Text='Editar perfil'></asp:Label></li>
        </ul>
        <%--<a href="DashBoard.aspx">Voltar</a>--%>

        <div class="form-horizontal">
            <div class="control-group">
                <label class="control-label"></label>
                <div class="controls">
                    <asp:RadioButton ID="rbdOthers" ClientIDMode="Static" runat="server" onclick="javascript: $('#formorganizacao').hide(); acceptTerms();" GroupName="userType" Checked="true" />
                    <span>
                        <asp:Literal ID="Literal18" Text='<%$Resources: Message, iam_a_common_user %>' runat="server" /></span>
                    &nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbdEntity" ClientIDMode="Static" runat="server" onclick="javascript: $('#formorganizacao').show(); acceptTerms();" GroupName="userType" />
                    <span>
                        <asp:Literal ID="Literal19" Text='<%$Resources: Message, iam_an_entity_user %>' runat="server" /></span>
                </div>
            </div>
            <fieldset>
                <legend>
                    <asp:Literal ID="Literal28" Text='<%$Resources: Message, general_informations %>' runat="server" />
                </legend>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal1" Text='<%$Resources: Message, name %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox CssClass="form-control input-lg validate[required, maxSize[50]]" MaxLength="50" ID="txtname" placeholder="Digite seu nome" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group" id="div_email">
                    <label class="control-label">
                        <asp:Literal ID="Literal2" Text='<%$Resources: Message, email %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" onblur="validate('user_email', 'div_email'); return false;" CssClass="form-control input-lg validate[required,custom[email], maxSize[50]]" MaxLength="50" ID="txtemail" placeholder="Digite seu e-mail"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal3" Text='<%$Resources: Message, nature %>' runat="server" /></label>
                    <div class="controls">
                        <asp:DropDownList runat="server" ID="ddlNature" CssClass="form-control input-lg validate[required]">
                            <asp:ListItem Text="Selecione" Value="" />
                            <asp:ListItem Text="Academia" Value="Academia" />
                            <asp:ListItem Text="Conselho" Value="Conselho" />
                            <asp:ListItem Text="Empresa" Value="Empresa" />
                            <asp:ListItem Text="Governo" Value="Governo" />
                            <asp:ListItem Text="Movimento social" Value="Movimento social" />
                            <asp:ListItem Text="Outros" Value="Outros" />
                            <asp:ListItem Text="Organização Não Governamental" Value="Organização Não Governamental" />
                            <asp:ListItem Text="Pessoa física" Value="Pessoa física" />
                            <asp:ListItem Text="Representativa" Value="Representativa" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal4" Text='<%$Resources: Message, operation_area %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg validate[required, maxSize[100]]" MaxLength="100" ID="txtArea" placeholder="Área de atuação"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal5" Text='<%$Resources: Message, operation_range %>' runat="server" /></label>
                    <div class="controls">
                        <asp:DropDownList runat="server" ID="ddlAtuation" CssClass="form-control input-lg validate[required]">
                            <asp:ListItem Text="Selecione" Value="" />
                            <asp:ListItem Text="Estadual" Value="Estadual" />
                            <asp:ListItem Text="Internacional" Value="Internacional" />
                            <asp:ListItem Text="Municipal" Value="Municipal" />
                            <asp:ListItem Text="Nacional" Value="Nacional" />
                            <asp:ListItem Text="Regional" Value="Regional" />
                        </asp:DropDownList>
                    </div>
                </div>
            </fieldset>

            <fieldset>
                <legend>
                    <asp:Literal ID="Literal29" Text='<%$Resources: Message, address_information %>' runat="server" />
                </legend>

                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal6" Text='<%$Resources: Message, address %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg validate[required, maxSize[150]]" MaxLength="150" ID="txtAddress" placeholder="Endereço"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal7" Text='<%$Resources: Message, number %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg validate[required, maxSize[50]]" MaxLength="50" ID="txtNumber" placeholder="Número"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal8" Text='<%$Resources: Message, neighborhood %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg validate[required, maxSize[100]]" MaxLength="100" ID="txtNeighborn" placeholder="Bairro"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal9" Text='<%$Resources: Message, region %>' runat="server" /></label>
                    <div class="controls">
                        <asp:DropDownList runat="server" ID="ddlRegion" CssClass="validate[required]">
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
                        <asp:Literal ID="Literal10" Text='<%$Resources: Message, state %>' runat="server" /></label>
                    <div class="controls">
                        <asp:DropDownList runat="server" ID="ddlState" CssClass="validate[required]" onchange="return getCities();">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal11" Text='<%$Resources: Message, city %>' runat="server" /></label>
                    <div class="controls">
                        <asp:DropDownList runat="server" ID="ddlCities" onchange="getCityId();" CssClass="validate[required]">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal12" Text='<%$Resources: Message, zipcode %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg validate[required, maxSize[100]]" MaxLength="100" ID="txtCep" placeholder="CEP"></asp:TextBox>
                    </div>
                </div>
            </fieldset>

            <fieldset>
                <legend>
                    <asp:Literal ID="Literal30" Text='<%$Resources: Message, contact_information %>' runat="server" />
                </legend>

                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal13" Text='<%$Resources: Message, phone %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg validate[required, maxSize[100]]" MaxLength="100" ID="txtPhone" placeholder="Telefone"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group" id="div6">
                    <label class="control-label">
                        <asp:Literal ID="Literal16" Text='<%$Resources: Message, website %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg" MaxLength="100" ID="txtSite" placeholder="Site"></asp:TextBox>
                    </div>
                </div>
            </fieldset>

            <fieldset>
                <legend>
                    <asp:Literal ID="Literal31" Text='<%$Resources: Message, access_information %>' runat="server" />
                </legend>

                <div class="control-group" id="div_user">
                    <label class="control-label">
                        <asp:Literal ID="Literal14" Text='<%$Resources: Message, user %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" Enabled="false" CssClass="form-control input-lg" ID="txtuser" MaxLength="20" placeholder="Digite seu usuário"></asp:TextBox>
                        <asp:HiddenField ID="hdnEmail" runat="server" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal15" Text='<%$Resources: Message, password %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox TextMode="Password" runat="server" CssClass="form-control input-lg validate[minSize[6], maxSize[16]]" ID="txtconfirmpassword" placeholder="Digite sua senha ou deixe em branco caso não queira editar"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group" id="div8">
                    <label class="control-label">Foto</label>
                    <div class="controls">
                        <asp:FileUpload ID="fileUpload" runat="server" />
                        <asp:Image ID="imgThumb" Visible="false" ImageUrl="~/Design/Images/noavatar.png" runat="server" />
                        <asp:Label ID="lblErrorUpload" runat="server"></asp:Label>
                    </div>
                </div>
            </fieldset>

            <fieldset id="formorganizacao" style="display: none">
                <legend>
                    <asp:Literal ID="Literal32" Text='<%$Resources: Message, organization_information %>' runat="server" />
                </legend>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal20" Text='<%$Resources: Message, cnpj %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg validate[required]" MaxLength="100" ClientIDMode="Static" ID="txtCnpj" placeholder="Digite seu cnpj"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal21" Text='<%$Resources: Message, representant_name %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg validate[required, maxSize[100]]" MaxLength="100" ID="txtNomeRepresentante" placeholder="Nome do representante da organização"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        <asp:Literal ID="Literal22" Text='<%$Resources: Message, representant_email %>' runat="server" /></label>
                    <div class="controls">
                        <asp:TextBox runat="server" CssClass="form-control input-lg validate[required, maxSize[100]]" MaxLength="100" ID="txtEmailRepresentante" placeholder="E-mail do representante da organização"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label"></label>
                    <div class="controls">
                        <asp:CheckBox ID="chbCredenciada" runat="server" ClientIDMode="Static" onclick="acceptTerms()" />
                        <asp:Literal ID="Literal23" Text='<%$Resources: Message, iam_accredited_entity %>' runat="server" />
                    </div>
                </div>
            </fieldset>

            <fieldset id="formTerms" style="display: none">
                <legend>
                    <asp:Literal ID="Literal33" Text='<%$Resources: Message, term_of_use %>' runat="server" />
                </legend>
                <asp:Panel runat="server" ClientIDMode="Static" ID="div_terms">
                    <div class="alert-block alert-danger">
                        <div style="margin-left: 4px">
                            <strong>
                                <asp:Literal ID="Literal24" Text='<%$Resources: Message, warning %>' runat="server" /></strong>
                            <asp:Literal ID="Literal25" Text='<%$Resources: Message, you_need_accept_terms %>' runat="server" />
                        </div>
                    </div>
                </asp:Panel>

                <div class="control-group">
                    <label class="control-label"></label>
                    <div class="controls">
                        <asp:CheckBox ID="chbTermsAccepted" runat="server" ClientIDMode="Static" Checked="false" />
                        <asp:Literal ID="Literal27" Text='<%$Resources: Message, accept_terms %>' runat="server" />
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="alert-bt alert-danger" runat="server" visible="false" id="alert">
            <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
        </div>
        <div class="row text-right">
            
            <asp:Button ID="btnSave" runat="server" CssClass="styler_bg_color" OnClientClick="return validateBeforeSave();" Text="Editar" OnClick="btnSave_Click" />
        </div>
    </div>
    <asp:HiddenField ID="hdnCityId" Value="0" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= txtCep.ClientID%>').setMask('cep');
            $('#<%= txtPhone.ClientID%>').setMask('phone');
            $('#<%= txtCnpj.ClientID%>').setMask('cnpj');

            if ($('#rbdEntity').prop('checked')) {
                $('#formorganizacao').show();
                acceptTerms();
            }

            if ($('#div_terms').css('display') != 'none') {
                $('html, body').animate({
                    scrollTop: $('#div_terms').offset().top
                }, 2000);
            }
        });

        function acceptTerms() {
            if ($('#chbCredenciada').is(':checked')) {
                if ($('#formorganizacao').css('display') == 'none') {
                    $('#formTerms').hide();
                } else {
                    $('#formTerms').show();
                }
            }
            else {
                $('#formTerms').hide();
            }
        }

        function validate(action, divName) {
            $.ajax({
                url: "/Handlers/Account/Validate.ashx",
                type: 'POST',
                data: {
                    email: $('#<%= txtemail.ClientID%>').val(),
                    login: $('#<%= txtuser.ClientID%>').val(),
                    action: action
                },
                cache: false,
                success: function (data) {
                    $('#' + divName).removeClass('has-error');

                    if (data == 'NOK') {
                        if (action == 'user_login') {
                            showAlert('<asp:Literal Text="<%$Resources: Message, exists_login %>" runat="server" />', 'error', 3000);
                        } else {
                            showAlert('<asp:Literal Text="<%$Resources: Message, exists_email %>" runat="server" />', 'error', 3000);
                        }

                        $('#' + divName).addClass('has-error');
                    }
                }
            });

            return false;
        }

        function validateBeforeSave() {
            $('.has-error').each(function (i, v) {
                $(v).find('input').focus()
            });

            if ($('.has-error').length > 0) {
                return false;
            }

            if ($('#chbCredenciada').is(':checked')) {
                if (!$('#chbTermsAccepted').is(':checked')) {
                    showAlert('<asp:Literal Text="<%$Resources: Message, agree_terms %>" runat="server" />', 'errro', 3000);
                    $('#chbTermsAccepted').focus();

                    return false;
                }
            }

            return true;
        }

        function getCityId() {
            $('#<%= hdnCityId.ClientID %>').val($("#<%= ddlCities.ClientID%>").val());
        }

        function getCities() {
            $("#<%= ddlCities.ClientID%>").html('<option>Carregando...</option>');
            $("#<%= ddlCities.ClientID%>").attr('disabled', true);

            $.ajax({
                url: "/Handlers/GetCities.ashx",
                type: 'POST',
                data: {
                    sid: $('#<%= ddlState.ClientID %>').val(),
                    type: 'all'
                },
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
    </script>
</asp:Content>
