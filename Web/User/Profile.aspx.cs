using Site.App_Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.User
{
    public partial class Profile : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadForm();
            }
        }

        private void loadForm()
        {
            using (Lib.Repositories.UserRepository rep = new Lib.Repositories.UserRepository(this.ActiveUser))
            {
                var user = rep.getInstanceById(this.ActiveUser.Id);

                if (user.Network && user.UserTypeEnum == Lib.Enumerations.UserType.Entity && user.TermsOfUse == false)
                {
                    div_terms.Visible = true;
                }
                else
                {
                    div_terms.Visible = false;
                }

                txtconfirmpassword.Text = "";
                txtemail.Text = user.Email;
                txtname.Text = user.Name;
                txtuser.Text = user.Login;
                hdnEmail.Value = user.Email;

                ddlRegion.SelectedValue = user.Region;
                txtArea.Text = user.Area;
                ddlAtuation.SelectedValue = user.Range;
                txtAddress.Text = user.Address;
                txtNumber.Text = user.Number;
                txtNeighborn.Text = user.Neighborhood;
                ddlRegion.SelectedValue = user.Region;
                txtCep.Text = user.ZipCode;

                if (user.CityId.HasValue)
                {
                    loadStates(user.City.StateId, user.CityId.Value);
                }
                else
                {
                    loadStates("", 0);
                }

                txtPhone.Text = user.Phone;
                txtSite.Text = user.WebSite;
                ddlNature.SelectedValue = user.Nature;

                txtCnpj.Text = user.CNPJ;
                txtNomeRepresentante.Text = user.ContactName;
                txtEmailRepresentante.Text = user.ContactEmail;

                if (user.Thumb != null)
                {
                    string base64String = Convert.ToBase64String(user.Thumb, 0, user.Thumb.Length);
                    imgThumb.ImageUrl = String.Format("data:{0};base64,{1}", user.Mime, base64String);
                    imgThumb.Visible = true;
                }

                chbCredenciada.Checked = user.Network;
                chbCredenciada.Enabled = false;

                if (user.TermsOfUse)
                {
                    chbTermsAccepted.Enabled = false;
                    chbTermsAccepted.Checked = true;
                }

                if (user.UserTypeEnum == Lib.Enumerations.UserType.Entity)
                {
                    rbdEntity.Checked = true;
                    rbdOthers.Checked = false;
                }
                else
                {
                    rbdEntity.Checked = false;
                    rbdOthers.Checked = true;
                }

                rbdEntity.Enabled = false;
                rbdOthers.Enabled = false;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            alert.Visible = false;

            if (chbCredenciada.Checked && chbTermsAccepted.Checked == false)
            {
                lblErrorMessage.Text = "Você deve aceitar os termos.";
                alert.Visible = true;
                return;
            }

            if (!validaCnpj(txtCnpj.Text))
            {
                lblErrorMessage.Text = "CNPJ inválido.";
                alert.Visible = true;
                return;
            }

            if (this.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Others || this.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Entity)
            {
                using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    Lib.Entities.User user = repository.getInstanceById(this.ActiveUser.Id);
                    user.Email = txtemail.Text;
                    user.Name = txtname.Text;

                    if (!String.IsNullOrEmpty(txtconfirmpassword.Text))
                    {
                        user.Password = Commons.SecurityUtils.criptografaSenha(txtconfirmpassword.Text);
                    }

                    user.Region = ddlRegion.SelectedValue;
                    user.Area = txtArea.Text;
                    user.Range = ddlAtuation.SelectedValue;
                    user.Address = txtAddress.Text;
                    user.Number = txtNumber.Text;
                    user.Neighborhood = txtNeighborn.Text;
                    user.Region = ddlRegion.SelectedValue;
                    user.ZipCode = txtCep.Text;
                    user.CityId = long.Parse(hdnCityId.Value);
                    user.Phone = txtPhone.Text;
                    user.WebSite = txtSite.Text;
                    user.Nature = ddlNature.SelectedValue;

                    user.CNPJ = txtCnpj.Text;

                    


                    user.ContactName = txtNomeRepresentante.Text;
                    user.Organization = "";
                    user.ContactEmail = txtEmailRepresentante.Text;

                    user.Network = chbCredenciada.Checked;

                    if (chbTermsAccepted.Checked)
                    {
                        user.AcceptionTermsDate = DateTime.Now;
                        user.TermsOfUse = true;
                    }

                    MemoryStream ms = null;
                    string mimeType = string.Empty;

                    if (!String.IsNullOrWhiteSpace(fileUpload.PostedFile.FileName))
                    {
                        if (imageIsValid(fileUpload.PostedFile.FileName))
                        {
                            ms = Lib.Utils.ImageUtils.redimensionarProporcionalmente(fileUpload.FileContent, 75);
                        }
                        else
                        {
                            lblErrorUpload.Text = "Imagem inválida (Tipos aceitos .bmp, .jpg, .jpeg e .png)";
                            return;
                        }
                    }
                    else
                    {
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

                    if (ms != null)
                        user.Thumb = ms.ToArray();

                    user.Mime = "image/png";

                    repository.save(user);

                    if (repository.HasErrors == false)
                    {
                        this.ActiveUser.Name = user.Name;
                        updateUser(user);
                    }
                }
            }
            
            Response.Redirect("~/User/ListForm.aspx");
        }

        private bool imageIsValid(string fileName)
        {
            if (!String.IsNullOrWhiteSpace(getMimeType(Path.GetExtension(fileName))))
            {
                return true;
            }
            return false;
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

        private void loadStates(string stateId, long cityId)
        {
            using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            {
                var states = rep.getAllStates();

                if (states != null && states.Count > 0)
                {
                    foreach (var state in states.OrderBy(f => f.Name).ToList())
                    {
                        ddlState.Items.Add(new ListItem(state.Name, state.Id));
                    }


                    List<Lib.Entities.City> statesAux = null;

                    if (!String.IsNullOrEmpty(stateId))
                    {
                        ddlState.SelectedValue = stateId;
                        statesAux = states.Where(f => f.Id == stateId).FirstOrDefault().Cities.OrderBy(f => f.Name).ToList();
                    }else{
                        statesAux = states.Where(f => f.Id == ddlState.SelectedValue).FirstOrDefault().Cities.OrderBy(f => f.Name).ToList();
                    }

                    foreach (var city in statesAux)
                    {
                        ddlCities.Items.Add(new ListItem(city.Name, city.Id.ToString()));

                    }

                    if (cityId > 0)
                    {
                        ddlCities.SelectedValue = cityId.ToString();
                        hdnCityId.Value = cityId.ToString();
                    }
                    else
                    {
                        hdnCityId.Value = ddlCities.SelectedValue;
                    }
                }
            }
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