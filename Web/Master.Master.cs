using Site.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BasePage bp = new BasePage();

                //li_currentForm.Visible = false;
                //li_lastForm.Visible = false;
                //li_allforms.Visible = false;
                divlogin.Visible = true;
                divProfile.Visible = false;

                if (bp.ActiveUser != null)
                {
                    lblUserName.Text = bp.ActiveUser.Name;

                    if (bp.ActiveUser.Thumb != null)
                    {
                        string base64String = Convert.ToBase64String(bp.ActiveUser.Thumb, 0, bp.ActiveUser.Thumb.Length);
                        imgThumb.ImageUrl = String.Format("data:{0};base64,{1}", bp.ActiveUser.Mime, base64String);
                        imgThumb.Visible = true;
                    }
                }

                if (bp.ActiveUser != null && (bp.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Entity || bp.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Others))
                {
                    divlogin.Visible = false;
                    divProfile.Visible = true;
                    lblTitleHello.Text = String.Format("Olá {0}, seja bem-vindo!", bp.ActiveUser.Name);

                    //li_allforms.Visible = true;
                }

                if (bp.ActiveUser != null && bp.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Entity && bp.ActiveUser.TermsOfUse == false && bp.ActiveUser.Network)
                {
                    if (!Request.Url.ToString().ToLower().Contains("user/profile.aspx"))
                    {
                        Response.Redirect("~/User/Profile.aspx");
                    }
                }

                if (bp.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Site)
                {
                    List<string> paginasLiberadas = new List<string>();
                    paginasLiberadas.Add("account/login.aspx");
                    paginasLiberadas.Add("account/new.aspx");
                    paginasLiberadas.Add("default.aspx");

                    bool podeEntrar = false;

                    foreach (string urlPermitida in paginasLiberadas)
                    {
                        if (Request.Url.ToString().ToLower().Contains(urlPermitida.ToLower()))
                        {
                            podeEntrar = true;
                        }
                    }

                    if (!podeEntrar)
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                }
            }
        }
    }
}