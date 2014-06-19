using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta.User
{
    public partial class ManageUsers : BasePage
    {
        protected override void AddRequiredUserTypes()
        {
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Admin);
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Master);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                btnNewUser.Text = "Criar novo usuário";

                if (!String.IsNullOrEmpty(Request.QueryString["ut"]))
                {
                    var type = Request.QueryString["ut"];

                    switch (type)
                    {
                        case "common":
                            btnNewUser.Text = "Criar novo usuário";
                            updateInformationOnMasterPage(Resources.Label.common_users, "fa fa-users", "menu_manage_users");

                            break;
                        case "admin,master":
                            btnNewUser.Text = "Criar novo administrador";
                            updateInformationOnMasterPage(Resources.Label.managers, "fa fa-users", "menu_manage_users");

                            break;
                        case "entity":
                            btnNewUser.Text = "Criar nova entidade";
                            updateInformationOnMasterPage(Resources.Label.entities, "fa fa-users", "menu_manage_users");

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ut"]))
            {
                var type = Request.QueryString["ut"];

                if (type == "common")
                {
                    Response.Redirect("~/User/NewUser.aspx?t=common");
                }
                else if (type == "admin,master")
                {
                    Response.Redirect("~/User/NewUser.aspx?t=admin");
                }
                else if (type == "entity")
                {
                    Response.Redirect("~/User/NewUser.aspx?t=entity");
                }
                else
                {
                    Response.Redirect("~/User/NewUser.aspx");
                }
            }
        }
    }
}