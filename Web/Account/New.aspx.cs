using Site.App_Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.Account
{
    public partial class New : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadStates();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (chbCredenciada.Checked && chbTermsAccepted.Checked == false)
            {
                lblerroterms.Text = Resources.Message.agree_terms;
                return;
            }

            using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(this.ActiveUser))
            {
                Lib.Entities.User user = new Lib.Entities.User();
                user.Active = true;
                user.Email = txtemail.Text;
                user.Login = txtuser.Text;
                user.Name = txtname.Text;
                user.Password = Commons.SecurityUtils.criptografaSenha(txtconfirmpassword.Text);

                if (rbdEntity.Checked)
                {
                    user.UserType = (int)Lib.Enumerations.UserType.Entity;
                }
                else
                {
                    user.UserType = (int)Lib.Enumerations.UserType.Others;
                }
                

                user.Region = ddlRegion.SelectedValue;
                user.Area = txtArea.Text;
                user.Range = ddlAtuation.SelectedValue;
                user.Address = txtAddress.Text;
                user.Number = txtNumber.Text;
                user.Neighborhood = txtNeighborn.Text;
                user.Region = ddlRegion.SelectedValue;
                user.ZipCode = txtCep.Text;
                user.CityId = long.Parse(ddlCities.SelectedValue);
                user.Phone = txtPhone.Text;
                user.WebSite = txtSite.Text;
                user.Nature = ddlNature.SelectedValue;

                user.CNPJ = txtCnpj.Text ;
                user.ContactName = txtNomeRepresentante.Text;
                user.Organization = "";
                user.ContactEmail = txtEmailRepresentante.Text;

                user.Network = chbCredenciada.Checked;
                user.AcceptionTermsDate = DateTime.Now;
                user.TermsOfUse = true;

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
                        lblErrorUpload.Text = Resources.Message.invalid_image_extension;
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

                this.login(user, true);
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
        private void loadStates()
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

                    foreach (var city in states.Where(f => f.Id == ddlState.SelectedValue).FirstOrDefault().Cities.OrderBy(f => f.Name))
                    {
                        ddlCities.Items.Add(new ListItem(city.Name, city.Id.ToString()));
                    }

                    hdnCityId.Value = ddlCities.SelectedValue;
                }
            }
        }
    }
}