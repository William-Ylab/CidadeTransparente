using Site.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.Account
{
    public partial class Login : System.Web.UI.Page
    {
        private BasePage page = new BasePage();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (Lib.Repositories.UserRepository repostory = new Lib.Repositories.UserRepository(null))
            {
                Lib.Entities.User user = repostory.authenticateEntityAndComum(txtUser.Text, txtPassword.Text);

                if (user != null)
                {
                    page.login(user, true);
                }
                else
                {
                    foreach (string error in repostory.Errors)
                    {
                        lblMessage.Text = error;
                    }
                }
            }
        }
    }
}