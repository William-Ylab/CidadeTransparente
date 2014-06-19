using Ferramenta.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferramenta.Form
{
    public partial class ListResponseForm : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            updateInformationOnMasterPage(Resources.Label.manage_response_form, "fa fa-comments", "menu_period");
        }
    }
}