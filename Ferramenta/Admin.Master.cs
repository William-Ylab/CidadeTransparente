using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        private BasePage _basePage;
        public BasePage BasePage
        {
            get
            {
                if (_basePage == null)
                {
                    _basePage = new BasePage();
                }

                return _basePage;
            }
        }

        public string PageTitle
        {
            get
            {
                return this.Attributes["page_title"];
            }
        }

        public string PageIcon
        {
            get
            {
                return this.Attributes["page_icon"];
            }
        }

        public string ActiveMenuId
        {
            get
            {
                return this.Attributes["active_menu_id"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (BasePage.ActiveUser == null)
            {
                phUserInfo.Visible = false;
                BasePage.redirectToLoginPage();
            }
            else
            {
                phUserInfo.Visible = true;
                ltUserName.Text = BasePage.ActiveUser.Name;
                lkEditProfile.Attributes["href"] = String.Format("/User/NewUser.aspx?id={0}", HttpUtility.HtmlEncode(Commons.SecurityUtils.criptografar(BasePage.ActiveUser.Id.ToString())));

                if (BasePage.ActiveUser.Thumb != null)
                {
                    string base64String = Convert.ToBase64String(BasePage.ActiveUser.Thumb, 0, BasePage.ActiveUser.Thumb.Length);
                    imgUser.ImageUrl = String.Format("data:{0};base64,{1}", BasePage.ActiveUser.Mime, base64String);

                }

                switch (BasePage.ActiveUser.UserTypeEnum)
                {
                    case Lib.Enumerations.UserType.Master:
                        //Libera o menu de gestão de periodos apenas para o MAster
                        phPeriodManagerMenu.Visible = true;
                        ltUserType.Text = "Adm. Master";
                        break;
                    case Lib.Enumerations.UserType.Admin:
                        ltUserType.Text = "Administrador";
                        break;
                    case Lib.Enumerations.UserType.Entity:
                        ltUserType.Text = "Entidade";
                        break;
                    case Lib.Enumerations.UserType.Others:
                        ltUserType.Text = "Comum";
                        break;
                    case Lib.Enumerations.UserType.Site:
                        ltUserType.Text = "Site";
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Precisa estar aqui, pois a atualização do periodo acontece após o Page_Load e depois atualiza o período.
            phPeriodOpen.Visible = false;
            using (Lib.Repositories.PeriodRepository rep = new Lib.Repositories.PeriodRepository(this.BasePage.ActiveUser))
            {
                var period = rep.getPeriodOpen();

                if (period != null)
                {
                    linkToRequestCities.NavigateUrl = String.Format("/City/RequestCities.aspx?periodId={0}", HttpUtility.UrlEncode(Commons.SecurityUtils.criptografar(period.Id.ToString())));
                    linkToOpenForms.NavigateUrl = String.Format("/Form/ListResponseForm.aspx?periodId={0}", HttpUtility.UrlEncode(Commons.SecurityUtils.criptografar(period.Id.ToString())));
                    menu_period_period.Text = String.Format(Resources.Label.menu_period, period.FinalDate.ToString("dd/MM/yyyy"));
                    phPeriodOpen.Visible = true;
                }
                else
                {
                    menu_period_period.Text = String.Format(Resources.Label.menu_period, "");
                }
            }
        }
    }
}