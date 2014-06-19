using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Ferramenta.App_Code
{
    public class BasePage : Page
    {
        #region [Session and Cookies Constants]
        public const string SESSION_PAYMENT_LIST = "PPPaymentList";
        public const string SESSION_ACTIVE_USER = "FDTSessionActiveUser";

        public const string COOKIE_ACTIVE_USER = "FDTCookieActiveUser";
        #endregion

        #region [Properties

        #endregion
        public Lib.Entities.User ActiveUser
        {
            get
            {
                Lib.Entities.User user = null;

                //Se existe o usuário na Session
                if (HttpContext.Current.Session[SESSION_ACTIVE_USER] != null)
                {
                    user = HttpContext.Current.Session[SESSION_ACTIVE_USER] as Lib.Entities.User;
                }
                else
                {
                    //Se existe o usuário em Cookie
                    HttpCookie cookie = Context.Request.Cookies.Get(COOKIE_ACTIVE_USER);

                    if (cookie == null)
                    {
                        redirectToLoginPage();
                    }
                    else
                    {
                        string expire = cookie["expires"].ToString();
                        string login = cookie["login"].ToString();
                        string password = cookie["password"].ToString();
                        DateTime dateExpirationFormated = Convert.ToDateTime(expire, new System.Globalization.CultureInfo("en-US"));

                        //Valida se o cookie expirou
                        if (dateExpirationFormated <= DateTime.Now)
                        {
                            redirectToLoginPage();
                        }
                        else
                        {
                            login = Commons.SecurityUtils.descriptografar(login);
                            password = Commons.SecurityUtils.descriptografar(password);

                            using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(null))
                            {
                                user = repository.authenticateAdmins(login, password, true);

                                if (user == null)
                                {
                                    //Usuário não é válido, redireciona o usuário para a tela de login
                                    redirectToLoginPage();
                                }
                                else
                                {
                                    //Renova a session com o objeto de usuário
                                    HttpContext.Current.Session[SESSION_ACTIVE_USER] = user;
                                }
                            }
                        }
                    }
                }

                return user;
            }
        }

        public List<Lib.Enumerations.UserType> AcceptedUsersTypeInPage { get; private set; }

        public void redirectToLoginPage()
        {
            Context.Response.Redirect("~/Login.aspx");
        }

        #region [Override/Overridable Methods]

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AcceptedUsersTypeInPage = new List<Lib.Enumerations.UserType>();

            //Adiciona os tipos de usuários necessários para acesso a página
            AddRequiredUserTypes();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Verifica se o usuário tem acesso a página
            if (!userHasPermission())
            {
                //Usuário não tem permissão
                //redireciona para página PageForbidden
                Response.Redirect(String.Format("~/PageForbidden.aspx?m={0} ({1})", Resources.Message.restrict_page_message, String.Join(",", AcceptedUsersTypeInPage)));
            }
        }

        protected virtual void AddRequiredUserTypes() { }

        #endregion

        #region [Public Methods]

        public void updateInformationOnMasterPage(string pageTitle, string pageIcon, string selectedMenuId)
        {
            if (this.Master != null)
            {
                this.Master.Attributes["page_title"] = pageTitle;
                this.Master.Attributes["page_icon"] = pageIcon;
                this.Master.Attributes["active_menu_id"] = selectedMenuId;
            }
        }

        public void login(Lib.Entities.User user)
        {
            if (user == null)
            {
                Context.Response.Redirect("~/Login.aspx");
            }
            else
            {
                //Adiciona um novo cookie
                System.Web.HttpCookie cookie = new System.Web.HttpCookie(BasePage.COOKIE_ACTIVE_USER);
                cookie.Values.Add("login", Commons.SecurityUtils.criptografar(user.Login));
                cookie.Values.Add("password", Commons.SecurityUtils.criptografar(user.Password));

                //Adicionado o valor expire, pois quando recuperamos o cookie a propriedade expire não faz sentido no request, 
                //pois se o cookie existe, quer dizer que o mesmo não expirou, porém por garantia estou guardando a data
                cookie.Expires = DateTime.Now.AddYears(1);
                cookie.Values.Add("expires", cookie.Expires.ToString("yyyy-MM-dd HH:mm:ss"));

                Context.Response.Cookies.Add(cookie);

                Session[BasePage.SESSION_ACTIVE_USER] = user;

                Context.Response.Redirect("~/Form/ManageBaseForm.aspx");
            }
        }

        #endregion

        #region [Private Methods]

        private bool userHasPermission()
        {
            bool hasPermission = true;

            if (AcceptedUsersTypeInPage != null)
            {
                if (AcceptedUsersTypeInPage.Count > 0)
                {
                    if (ActiveUser != null)
                    {
                        if (AcceptedUsersTypeInPage.Contains(ActiveUser.UserTypeEnum))
                        {
                            hasPermission = true;
                        }
                        else
                        {
                            hasPermission = false;
                        }
                    }
                    else
                    {
                        hasPermission = false;
                    }
                }
            }
            return hasPermission;
        }

        #endregion
    }
}