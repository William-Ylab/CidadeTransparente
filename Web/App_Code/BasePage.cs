using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.App_Code
{
    public class BasePage : Page
    {
        #region [Session and Cookies Constants]

        public const string SESSION_ACTIVE_USER = "SITEFDTSessionActiveUser";
        public const string COOKIE_ACTIVE_USER = "SITEFDTCookieActiveUser";

        #endregion

        #region [Properties]

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
                        return user;
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
                            return user;
                        }
                        else
                        {
                            try
                            {
                                login = Commons.SecurityUtils.descriptografar(login);
                                password = Commons.SecurityUtils.descriptografar(password);

                                using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(null))
                                {
                                    user = repository.authenticateEntityAndComum(login, password, true);

                                    if (user == null)
                                    {
                                        //Usuário recuperado no cookie não existe no banco, envia para o logout para limpar a session e cookie
                                        redirectToLogout();
                                    }
                                    else
                                    {
                                        //Renova a session com o objeto de usuário
                                        HttpContext.Current.Session[SESSION_ACTIVE_USER] = user;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //Usuário recuperado no cookie não existe no banco, envia para o logout para limpar a session e cookie
                                redirectToLogout();
                            }
                        }
                    }
                }

                return user;
            }
        }

        public List<Lib.Enumerations.UserType> AcceptedUsersTypeInPage { get; private set; }

        #endregion

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

        public void redirectToLoginPage()
        {
            using (Lib.Repositories.UserRepository rep = new Lib.Repositories.UserRepository(null))
            {
                var user = rep.selectWhere(f => f.UserTypeEnum == Lib.Enumerations.UserType.Site).FirstOrDefault();

                if (user != null)
                {
                    login(user, false);
                }
            }

            Context.Response.Redirect("~/Default.aspx");
        }

        public void redirectToLogout()
        {
            Context.Response.Redirect("~/Account/Logout.aspx");
        }

        public void login(Lib.Entities.User user, bool redirect)
        {
            if (user == null)
            {
                Context.Response.Redirect("~/Account/Login.aspx");
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

                if (redirect)
                {
                    Context.Response.Redirect("~/Default.aspx");
                }
            }
        }

        public void updateUser(Lib.Entities.User user)
        {
            Session[BasePage.SESSION_ACTIVE_USER] = user;
        }

        public bool loadResponsableCities(DropDownList ddlCities, long periodId)
        {
            //Seleciona as cidades que a entidade pode submeter no periodo
            ddlCities.Items.Clear();

            using (Lib.Repositories.StateCityRepository ctx = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            {
                var listResponsableCities = ctx.getCitiesFromResponsableUser(this.ActiveUser.Id, periodId);

                //Precisa ser responsável por alguma cidade
                if (listResponsableCities != null && listResponsableCities.Count > 0)
                {
                    List<Lib.Entities.ResponseForm> responsesForm = null;

                    using (var context = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                        responsesForm = context.getResponseFormsByUserIdAndPeriodId(this.ActiveUser.Id, periodId);

                    foreach (var responsableCity in listResponsableCities)
                    {
                        if (responsesForm != null && responsesForm.Count > 0)
                        {
                            var formByCity = responsesForm.Where(f => f.CityId == responsableCity.Id).FirstOrDefault();

                            if (formByCity != null)
                            {
                                if (!formByCity.isSubmitted() && !formByCity.isApproved() && !formByCity.isNotApproved())
                                {
                                    ddlCities.Items.Add(new ListItem(String.Format("{0} (Em andamento)", responsableCity.Name), Commons.SecurityUtils.criptografar(responsableCity.Id.ToString())));
                                }
                                else if (formByCity.isNotApproved())
                                {
                                    ddlCities.Items.Add(new ListItem(String.Format("{0} (Reprovado)", responsableCity.Name), Commons.SecurityUtils.criptografar(responsableCity.Id.ToString())));
                                }
                            }
                            else
                            {
                                ddlCities.Items.Add(new ListItem(responsableCity.Name, Commons.SecurityUtils.criptografar(responsableCity.Id.ToString())));
                            }
                        }
                        else
                        {
                            //Nenhum questionário respondido
                            //Usuário pode selecionar N cidades
                            ddlCities.Items.Add(new ListItem(responsableCity.Name, Commons.SecurityUtils.criptografar(responsableCity.Id.ToString())));
                        }
                    }
                }
            }

            if (ddlCities.Items.Count == 0)
            {
                ddlCities.Items.Add(new ListItem("Nenhum município encontrado", ""));
                return false;
            }

            return true;
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