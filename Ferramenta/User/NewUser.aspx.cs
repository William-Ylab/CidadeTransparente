using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta.User
{
    public partial class NewUser : BasePage
    {
        protected override void AddRequiredUserTypes()
        {
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Admin);
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Master);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            updateInformationOnMasterPage(Resources.Label.new_user, "fa fa-user", "menu_manage_users");
            if (!IsPostBack)
            {
                hdnUrl.Value = Request.UrlReferrer.PathAndQuery;

                //Conforme alinha na task: FDT-168u um administrador pode criar outro administrador.
                ddlUserType.Items.Add(new ListItem(Lib.Enumerations.EnumManager.getStringFromUserType(Lib.Enumerations.UserType.Admin), "1"));
                ddlUserType.Items.Add(new ListItem(Lib.Enumerations.EnumManager.getStringFromUserType(Lib.Enumerations.UserType.Others), "3"));
                ddlUserType.Items.Add(new ListItem(Lib.Enumerations.EnumManager.getStringFromUserType(Lib.Enumerations.UserType.Entity), "2"));

                if (!String.IsNullOrEmpty(Request.QueryString["t"]))
                {
                    var type = Request.QueryString["t"].ToLower();

                    ddlUserType.SelectedValue = "1";
                    switch (type)
                    {
                        case "common":
                            updateInformationOnMasterPage(Resources.Label.new_common, "fa fa-user", "menu_manage_users");
                            ddlUserType.SelectedValue = "3";
                            loadStates(ddlCommonState);
                            loadCities(ddlCommonState, ddlCommonCity);
                            break;
                        case "entity":
                            updateInformationOnMasterPage(Resources.Label.new_entity, "fa fa-user", "menu_manage_users");
                            ddlUserType.SelectedValue = "2";
                            loadStates(ddlEntityState);
                            loadCities(ddlEntityState, ddlEntityCity);
                            break;
                        case "admin":
                            updateInformationOnMasterPage(Resources.Label.new_admin, "fa fa-user", "menu_manage_users");
                            break;
                        default:
                            break;
                    }
                }

                //Carrega o usuário se necessário
                if (!String.IsNullOrWhiteSpace(Page.Request.QueryString["id"]))
                {
                    try
                    {
                        hdUserId.Value = Page.Request.QueryString["id"];
                        long userId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdUserId.Value));

                        using (Lib.Repositories.UserRepository rep = new Lib.Repositories.UserRepository(ActiveUser))
                        {
                            var user = rep.getInstanceById(userId);

                            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Master);
                            if (user.UserTypeEnum != Lib.Enumerations.UserType.Master)
                            {
                                AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Admin);
                            }


                            //Adiciona o DDL o master, caso a edição seja de um usuário master
                            if (user.UserTypeEnum == Lib.Enumerations.UserType.Master)
                                ddlUserType.Items.Add(new ListItem(Lib.Enumerations.EnumManager.getStringFromUserType(Lib.Enumerations.UserType.Master), "0"));

                            loadForm(user);
                        }

                    }
                    catch (Exception ex)
                    {
                        Lib.Log.ErrorLog.saveError("Ferramenta.User.NewUser.Page_Load", ex);
                        phMessageError.Visible = false;
                        lblMessageError.Text = Resources.Message.problem_to_load_requested_user;
                    }

                }

                showFormBySelectedUserType();
            }
        }

        private void showFormBySelectedUserType()
        {
            phAdminForm.Visible = false;
            phCommonForm.Visible = false;
            phEntityForm.Visible = false;
            phOrganization.Visible = false;

            switch (ddlUserType.SelectedValue.ToLower())
            {
                case "0": //Master
                case "1": //Admin
                    phAdminForm.Visible = true;
                    if (ddlUserType.SelectedValue == "1")
                        phOrganization.Visible = true;
                    break;
                case "2": //Entity
                    phEntityForm.Visible = true;
                    loadStates(ddlEntityState);
                    loadCities(ddlEntityState, ddlEntityCity);
                    break;
                case "3": //Others
                    phCommonForm.Visible = true;
                    loadStates(ddlCommonState);
                    loadCities(ddlCommonState, ddlCommonCity);
                    break;
            }
        }

        private void loadForm(Lib.Entities.User user)
        {
            if (user == null)
            {
                return;
            }

            switch (user.UserTypeEnum)
            {
                case Lib.Enumerations.UserType.Master:
                case Lib.Enumerations.UserType.Admin:
                    phAdminForm.Visible = true;

                    txtAdminEmail.Text = user.Email;
                    txtAdminLogin.Text = user.Login;
                    txtAdminLogin.Enabled = false;
                    txtAdminPassword.Text = "";
                    txtAdminPassword.Attributes["placeholder"] = Resources.Message.leave_this_box_blank;
                    txtAdminPassword.CssClass = "span6";
                    txtAdminName.Text = user.Name;

                    if (user.UserTypeEnum == Lib.Enumerations.UserType.Admin)
                    {
                        phOrganization.Visible = true;
                        txtAdminOrganization.Text = user.Organization;
                    }

                    chkAdminStatus.Checked = user.Active;

                    if (user.Thumb != null)
                    {
                        string base64String = Convert.ToBase64String(user.Thumb, 0, user.Thumb.Length);
                        imgUser.ImageUrl = String.Format("data:{0};base64,{1}", user.Mime, base64String);
                        imgUser.Visible = true;
                    }

                    ddlUserType.SelectedValue = user.UserType.ToString();
                    ddlUserType.Enabled = false;

                    break;
                case Lib.Enumerations.UserType.Entity:
                    phEntityForm.Visible = true;

                    if (!user.TermsOfUse)
                    {
                        phMessageWarning.Visible = true;
                        lblMessageWarning.Text = Resources.Message.this_user_not_accepted_the_terms;
                    }

                    txtEntityAddress.Text = user.Address;
                    txtEntityArea.Text = user.Area;
                    txtEntityCNPJ.Text = user.CNPJ;
                    txtEntityEmail.Text = user.Email;
                    txtEntityLogin.Text = user.Login;
                    txtEntityLogin.Enabled = false;
                    txtEntityPassword.Text = "";
                    txtEntityPassword.Attributes["placeholder"] = Resources.Message.leave_this_box_blank;
                    txtEntityPassword.CssClass = "span6";
                    txtEntityName.Text = user.Name;
                    txtEntityNeighborhood.Text = user.Neighborhood;
                    txtEntityNumber.Text = user.Number;
                    txtEntityPhone.Text = user.Phone;
                    txtEntitySite.Text = user.WebSite;
                    txtEntityZipCode.Text = user.ZipCode;
                    txtEntityContactEmail.Text = user.ContactEmail;
                    txtEntityContactName.Text = user.ContactName;

                    ddlEntityRegion.SelectedValue = user.Region;
                    ddlEntityNature.SelectedValue = user.Nature;
                    ddlEntityRange.SelectedValue = user.Range;

                    chkEntityStatus.Checked = user.Active;
                    chkEntityNetwork.Checked = user.Network;
                    chkEntityApproved.Checked = user.NetworkApproved;

                    approvedBlock.Attributes["style"] = "display:none";
                    if (user.Network)
                        approvedBlock.Attributes["style"] = "";

                    ltApprovedBy.Text = "";
                    if (user.NetworkApproved && user.NetworkApprovedBy != null)
                        ltApprovedBy.Text = String.Format("<strong>{1}</strong> {0}", user.NetworkApprovedBy.Name, Resources.Message.approved_by);

                    if (user.Thumb != null)
                    {
                        string base64String = Convert.ToBase64String(user.Thumb, 0, user.Thumb.Length);
                        imgUser.ImageUrl = String.Format("data:{0};base64,{1}", user.Mime, base64String);
                        imgUser.Visible = true;
                    }

                    ddlUserType.SelectedValue = user.UserType.ToString();
                    ddlUserType.Enabled = false;

                    loadStates(ddlEntityState);

                    ddlEntityState.SelectedValue = "";
                    if (user.CityId.HasValue)
                    {
                        using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(this.ActiveUser))
                        {
                            var city = rep.getCityInstanceById(user.CityId.Value);
                            if (city != null)
                            {
                                ddlEntityState.SelectedValue = city.StateId;
                            }
                        }
                    }

                    loadCities(ddlEntityState, ddlEntityCity);
                    ddlEntityCity.SelectedValue = user.CityId != null ? user.CityId.ToString() : "";


                    //Carrega as cidades a qual a entidade é responsável e colaboradora
                    if (user.Groups != null)
                    {
                        var results = user.Groups.Select(g => new
                        {
                            City = g.City.Name,
                            State = g.City.StateId,
                            Period = g.Period.Name,
                        }).ToList();

                        rptCollabCities.DataSource = results;
                        rptCollabCities.DataBind();
                    }

                    if (user.ResponsableGroups != null)
                    {
                        var results = user.ResponsableGroups.Select(g => new
                        {
                            City = g.City.Name,
                            State = g.City.StateId,
                            Period = g.Period.Name,
                        }).ToList();

                        rptResponsableCities.DataSource = results;
                        rptResponsableCities.DataBind();
                    }

                    phResponsableCollabCities.Visible = true;

                    break;
                case Lib.Enumerations.UserType.Others:
                    phCommonForm.Visible = true;

                    txtCommonAddress.Text = user.Address;
                    txtCommonArea.Text = user.Area;
                    txtCommonEmail.Text = user.Email;
                    txtCommonLogin.Text = user.Login;
                    txtCommonLogin.Enabled = false;
                    txtCommonPassword.Text = "";
                    txtCommonPassword.Attributes["placeholder"] = Resources.Message.leave_this_box_blank;
                    txtCommonPassword.CssClass = "span6";
                    txtCommonName.Text = user.Name;
                    txtCommonNeighborhood.Text = user.Neighborhood;
                    txtCommonNumber.Text = user.Number;
                    txtCommonPhone.Text = user.Phone;
                    txtCommonSite.Text = user.WebSite;
                    txtCommonZipCode.Text = user.ZipCode;

                    ddlCommonRegion.SelectedValue = user.Region;
                    ddlCommonNature.SelectedValue = user.Nature;
                    ddlCommonRange.SelectedValue = user.Range;

                    chkCommonStatus.Checked = user.Active;

                    if (user.Thumb != null)
                    {
                        string base64String = Convert.ToBase64String(user.Thumb, 0, user.Thumb.Length);
                        imgUser.ImageUrl = String.Format("data:{0};base64,{1}", user.Mime, base64String);
                        imgUser.Visible = true;
                    }

                    ddlUserType.SelectedValue = user.UserType.ToString();
                    ddlUserType.Enabled = false;

                    loadStates(ddlCommonState);

                    ddlCommonState.SelectedValue = "";
                    if (user.CityId.HasValue)
                    {
                        using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(this.ActiveUser))
                        {
                            var city = rep.getCityInstanceById(user.CityId.Value);
                            if (city != null)
                            {
                                ddlCommonState.SelectedValue = city.StateId;
                            }
                        }
                    }

                    loadCities(ddlCommonState, ddlCommonCity);
                    ddlCommonCity.SelectedValue = user.CityId != null ? user.CityId.ToString() : "";

                    break;
                case Lib.Enumerations.UserType.Site:
                    break;
                default:
                    break;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            phMessageError.Visible = false;
            phMessageSuccess.Visible = false;
            lblMessageError.Text = String.Empty;
            lblMessageSuccess.Text = String.Empty;

            try
            {
                if (String.IsNullOrWhiteSpace(ddlUserType.SelectedValue))
                {
                    throw new ApplicationException(Resources.Message.user_type_is_mandatory);
                }

                MemoryStream ms = null;
                string mimeType = string.Empty;

                if (!String.IsNullOrWhiteSpace(fpUserPhoto.PostedFile.FileName))
                {
                    if (imageIsValid(fpUserPhoto.PostedFile.FileName))
                    {
                        ms = Lib.Utils.ImageUtils.redimensionarProporcionalmente(fpUserPhoto.FileContent, 75);
                    }
                    else
                    {
                        throw new ApplicationException(Resources.Message.invalid_image_extension);
                    }
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(hdUserId.Value))
                    {
                        //Novo usuário

                        //Busca a imagem default do sistema
                        if (File.Exists(Server.MapPath("~/Design/Images/noavatar.png")))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("~/Design/Images/noavatar.png"));

                            System.Drawing.Image imageSized = null;
                            System.Drawing.Size size = new System.Drawing.Size(75, 75);
                            imageSized = new System.Drawing.Bitmap(image, size);

                            ms = new MemoryStream();
                            imageSized.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }

                if (phAdminForm.Visible)
                {
                    saveAdmin(ms);
                }
                else if (phEntityForm.Visible)
                {
                    saveEntity(ms);
                }
                else if (phCommonForm.Visible)
                {
                    saveCommon(ms);
                }

                this.Page.MaintainScrollPositionOnPostBack = false;

                Response.Redirect("~" + hdnUrl.Value);
            }
            catch (Exception ex)
            {
                this.Page.MaintainScrollPositionOnPostBack = false;
                phMessageError.Visible = true;
                lblMessageError.Text = ex.Message;
            }

        }

        private void saveCommon(MemoryStream msThumb)
        {
            long userId = !String.IsNullOrWhiteSpace(hdUserId.Value) ? Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdUserId.Value)) : 0;

            using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(ActiveUser))
            {
                Lib.Entities.User user = null;

                if (userId > 0)
                {
                    //Carrega o usuário existente
                    user = repository.getInstanceById(userId);

                    //Caso o usuário deixou em branco, mantem a antiga senha
                    if (!String.IsNullOrWhiteSpace(txtCommonPassword.Text))
                        user.Password = Commons.SecurityUtils.criptografaSenha(txtCommonPassword.Text);
                }
                else
                {
                    //Novo usuário
                    user = new Lib.Entities.User();
                    user.Password = Commons.SecurityUtils.criptografaSenha(txtCommonPassword.Text);
                }

                user.UserType = Convert.ToInt32(ddlUserType.SelectedValue);
                user.Email = txtCommonEmail.Text;
                user.Login = txtCommonLogin.Text;
                user.Name = txtCommonName.Text;
                user.Organization = "";
                user.Active = chkCommonStatus.Checked;
                user.Address = txtCommonAddress.Text;
                user.Area = txtCommonArea.Text;
                user.CityId = null;

                if (!String.IsNullOrWhiteSpace(ddlCommonCity.SelectedValue))
                    user.CityId = Convert.ToInt64(ddlCommonCity.SelectedValue);

                user.Nature = ddlCommonNature.SelectedValue;
                user.Neighborhood = txtCommonNeighborhood.Text;
                user.Network = false;
                user.Number = txtCommonNumber.Text;
                user.Phone = txtCommonPhone.Text;
                user.Range = ddlCommonRange.SelectedValue;
                user.Region = ddlCommonRegion.SelectedValue;
                user.WebSite = txtCommonSite.Text;
                user.ZipCode = txtCommonZipCode.Text;

                if (msThumb != null)
                    user.Thumb = msThumb.ToArray();

                //Valida email e login
                if (!repository.emailAlreadyUsed(userId, user.Email))
                {
                    if (!repository.loginAlreadyUsed(userId, user.Login))
                    {
                        repository.save(user);

                        if (repository.HasErrors)
                        {
                            throw new ApplicationException(String.Join(",", repository.Errors));
                        }
                        else
                        {
                            string textLnk = String.Format("<a href='/User/ManageUsers.aspx?ut={0}'><strong>{1}</strong></a>", getUserTypeQS(user.UserTypeEnum), Resources.Message.click_here_if_you_want_back_to_the_list);
                            phMessageSuccess.Visible = true;
                            if (userId == 0)
                            {

                                lblMessageSuccess.Text = String.Format(Resources.Message.format_user_has_been_created_with_success, txtCommonName.Text, textLnk);
                            }
                            else
                            {
                                lblMessageSuccess.Text = String.Format(Resources.Message.format_user_has_been_updated_with_success, txtCommonName.Text, textLnk);

                                if (userId == this.ActiveUser.Id)
                                {
                                    //Atualiza as informações de login, imediatamente
                                    login(user);
                                }
                            }
                            //Response.Redirect("/User/ManageUsers.aspx");
                            //clearForm();
                        }
                    }
                    else
                    {
                        throw new ApplicationException(Resources.Message.login_already_used);
                    }
                }
                else
                {
                    throw new ApplicationException(Resources.Message.email_already_used);
                }
            }
        }

        private void saveEntity(MemoryStream msThumb)
        {
            long userId = !String.IsNullOrWhiteSpace(hdUserId.Value) ? Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdUserId.Value)) : 0;

            using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(ActiveUser))
            {
                Lib.Entities.User user = null;

                if (userId > 0)
                {
                    //Carrega o usuário existente
                    user = repository.getInstanceById(userId);

                    //Caso o usuário deixou em branco, mantem a antiga senha
                    if (!String.IsNullOrWhiteSpace(txtEntityPassword.Text))
                        user.Password = Commons.SecurityUtils.criptografaSenha(txtEntityPassword.Text);
                }
                else
                {
                    //Novo usuário
                    user = new Lib.Entities.User();
                    user.Password = Commons.SecurityUtils.criptografaSenha(txtEntityPassword.Text);
                }

                user.UserType = Convert.ToInt32(ddlUserType.SelectedValue);
                user.Email = txtEntityEmail.Text;
                user.Login = txtEntityLogin.Text;
                user.Name = txtEntityName.Text;
                user.Organization = "";
                user.Active = chkEntityStatus.Checked;
                user.Address = txtEntityAddress.Text;
                user.Area = txtEntityArea.Text;
                user.CityId = null;

                if (!String.IsNullOrWhiteSpace(ddlEntityCity.SelectedValue))
                    user.CityId = Convert.ToInt64(ddlEntityCity.SelectedValue);

                user.CNPJ = txtEntityCNPJ.Text;

                if (!validaCnpj(user.CNPJ))
                    throw new Exception("CNPJ inválido");

                user.Nature = ddlEntityNature.SelectedValue;
                user.Neighborhood = txtEntityNeighborhood.Text;
                user.Number = txtEntityNumber.Text;
                user.Phone = txtEntityPhone.Text;
                user.Range = ddlEntityRange.SelectedValue;
                user.Region = ddlEntityRegion.SelectedValue;
                user.WebSite = txtEntitySite.Text;
                user.ZipCode = txtEntityZipCode.Text;
                user.ContactName = txtEntityContactName.Text;
                user.ContactEmail = txtEntityContactEmail.Text;
                user.Network = chkEntityNetwork.Checked;
                user.NetworkApproved = chkEntityApproved.Checked;

                if (msThumb != null)
                    user.Thumb = msThumb.ToArray();

                //Valida email e login
                if (!repository.emailAlreadyUsed(userId, user.Email))
                {
                    if (!repository.loginAlreadyUsed(userId, user.Login))
                    {
                        repository.save(user);

                        if (repository.HasErrors)
                        {
                            throw new ApplicationException(String.Join(",", repository.Errors));
                        }
                        else
                        {
                            string textLnk = String.Format("<a href='/User/ManageUsers.aspx?ut={0}'><strong>{1}</strong></a>", getUserTypeQS(user.UserTypeEnum), Resources.Message.click_here_if_you_want_back_to_the_list);
                            phMessageSuccess.Visible = true;
                            loadForm(repository.getInstanceById(userId));
                            if (userId == 0)
                            {

                                lblMessageSuccess.Text = String.Format(Resources.Message.format_user_has_been_created_with_success, txtEntityName.Text, textLnk);
                            }
                            else
                            {
                                lblMessageSuccess.Text = String.Format(Resources.Message.format_user_has_been_updated_with_success, txtEntityName.Text, textLnk);

                                if (userId == this.ActiveUser.Id)
                                {
                                    //Atualiza as informações de login, imediatamente
                                    login(user);
                                }
                            }
                            //Response.Redirect("/User/ManageUsers.aspx");
                            //clearForm();
                        }
                    }
                    else
                    {
                        throw new ApplicationException(Resources.Message.login_already_used);
                    }
                }
                else
                {
                    throw new ApplicationException(Resources.Message.email_already_used);
                }
            }
        }

        private void saveAdmin(MemoryStream msThumb)
        {
            long userId = !String.IsNullOrWhiteSpace(hdUserId.Value) ? Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdUserId.Value)) : 0;

            using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(ActiveUser))
            {
                Lib.Entities.User user = null;

                if (userId > 0)
                {
                    //Carrega o usuário existente
                    user = repository.getInstanceById(userId);

                    //Caso o usuário deixou em branco, mantem a antiga senha
                    if (!String.IsNullOrWhiteSpace(txtAdminPassword.Text))
                        user.Password = Commons.SecurityUtils.criptografaSenha(txtAdminPassword.Text);
                }
                else
                {
                    //Novo usuário
                    user = new Lib.Entities.User();

                    user.Password = Commons.SecurityUtils.criptografaSenha(txtAdminPassword.Text);
                }

                user.UserType = Convert.ToInt32(ddlUserType.SelectedValue);
                user.Email = txtAdminEmail.Text;
                user.Login = txtAdminLogin.Text;
                user.Name = txtAdminName.Text;
                user.Organization = txtAdminOrganization.Text;
                user.Active = chkAdminStatus.Checked;

                if (msThumb != null)
                    user.Thumb = msThumb.ToArray();

                //Valida email e login
                if (!repository.emailAlreadyUsed(userId, user.Email))
                {
                    if (!repository.loginAlreadyUsed(userId, user.Login))
                    {
                        repository.save(user);

                        if (repository.HasErrors)
                        {
                            throw new ApplicationException(String.Join(",", repository.Errors));
                        }
                        else
                        {
                            string textLnk = String.Format("<a href='/User/ManageUsers.aspx?ut={0}'><strong>{1}</strong></a>", getUserTypeQS(user.UserTypeEnum), Resources.Message.click_here_if_you_want_back_to_the_list);
                            phMessageSuccess.Visible = true;
                            if (userId == 0)
                            {

                                lblMessageSuccess.Text = String.Format(Resources.Message.format_user_has_been_created_with_success, txtAdminName.Text, textLnk);
                            }
                            else
                            {
                                lblMessageSuccess.Text = String.Format(Resources.Message.format_user_has_been_updated_with_success, txtAdminName.Text, textLnk);

                                if (userId == this.ActiveUser.Id)
                                {
                                    //Atualiza as informações de login, imediatamente
                                    login(user);
                                }
                            }
                            //Response.Redirect("/User/ManageUsers.aspx");
                            //clearForm();
                        }
                    }
                    else
                    {
                        throw new ApplicationException(Resources.Message.login_already_used);
                    }
                }
                else
                {
                    throw new ApplicationException(Resources.Message.email_already_used);
                }
            }
        }

        private string getMimeType(string extension)
        {
            extension = extension.TrimStart('.');

            switch (extension.ToLower())
            {
                case "png":
                    return "image/png";
                case "jpg":
                case "jpeg":
                    return "image/jpeg";
                case "bmp":
                    return "image/bmp";
                default: return string.Empty;
            }
        }

        private bool imageIsValid(string fileName)
        {
            if (!String.IsNullOrWhiteSpace(getMimeType(Path.GetExtension(fileName))))
            {
                return true;
            }
            return false;
        }

        protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            showFormBySelectedUserType();
        }

        private string getUserTypeQS(Lib.Enumerations.UserType type)
        {
            switch (type)
            {
                case Lib.Enumerations.UserType.Master:
                    return "admin,master";
                case Lib.Enumerations.UserType.Admin:
                    return "admin";
                case Lib.Enumerations.UserType.Entity:
                    return "entity";
                case Lib.Enumerations.UserType.Others:
                    return "common";
                default:
                    return "";
            }
        }

        private void loadStates(DropDownList ddlTarget)
        {
            using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            {
                var states = rep.getAllStates();

                if (states != null)
                {
                    ddlTarget.DataSource = states.Select(s => new
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();

                    ddlTarget.DataBind();
                }
            }

            ddlTarget.Items.Insert(0, new ListItem(Resources.Label.select_an_item, ""));
        }

        private void loadCities(DropDownList ddlTargetState, DropDownList ddlTarget)
        {
            if (!String.IsNullOrWhiteSpace(ddlTargetState.SelectedValue))
            {
                using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(this.ActiveUser))
                {
                    var cities = rep.getCitiesByUF(ddlTargetState.SelectedValue);

                    if (cities != null)
                    {
                        ddlTarget.DataSource = cities.Select(s => new
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).ToList();

                        ddlTarget.DataBind();
                    }
                }
            }
            ddlTarget.Items.Insert(0, new ListItem(Resources.Label.select_an_item, ""));
        }

        protected void ddlEntityEstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadCities(ddlEntityState, ddlEntityCity);
        }

        protected void ddlCommonState_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadCities(ddlCommonState, ddlCommonCity);
        }

        public bool validaCnpj(string cnpj)
        {

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;

            int resto;

            string digito;

            string tempCnpj;

            cnpj = cnpj.Trim();

            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);

        }
    }
}