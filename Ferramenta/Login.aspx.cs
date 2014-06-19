using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta
{
    public partial class Login : System.Web.UI.Page
    {
        private BasePage page = new BasePage();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_ServerClick(object sender, EventArgs e)
        {
            phMessage.Visible = false;
            lblMessage.Text = "";

            if (String.IsNullOrWhiteSpace(txtUser.Text) || String.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMessage.Text = Resources.Message.login_password_must_be_filled;
                phMessage.Visible = true;
                return;
            }

            using (Lib.Repositories.UserRepository repostory = new Lib.Repositories.UserRepository(null))
            {
                Lib.Entities.User user = repostory.authenticateAdmins(txtUser.Text, txtPassword.Text);

                if (user != null)
                {
                    page.login(user);
                }
                else
                {
                    foreach (string error in repostory.Errors)
                    {
                        lblMessage.Text += "\n" + error;
                    }

                    phMessage.Visible = true;
                    return;
                }
            }
        }
    }
}