using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Zera a session
            HttpCookie _cookie = HttpContext.Current.Request.Cookies[BasePage.COOKIE_ACTIVE_USER];
            if (_cookie != null)
            {
                _cookie.Expires = DateTime.Now.AddDays(-2);
                Response.Cookies.Add(_cookie);
            }

            HttpContext.Current.Session[BasePage.SESSION_ACTIVE_USER] = null;

            //Redirect para Login
            Response.Redirect("~/Login.aspx");
        }
    }
}