using Ferramenta.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferramenta.Period
{
    public partial class List : BasePage
    {
        protected override void AddRequiredUserTypes()
        {
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Master);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            updateInformationOnMasterPage(Resources.Label.period_list, "fa fa-calendar", "menu_period_manage");
        }

        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Period/NewPeriod.aspx");
        }
    }
}