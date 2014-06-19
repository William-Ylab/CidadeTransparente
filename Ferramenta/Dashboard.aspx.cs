using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferramenta
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Attributes["page_title"] = Resources.Label.dashboard;
            this.Master.Attributes["page_icon"] = "fa fa-home";
            this.Master.Attributes["active_menu_id"] = "menu_dashboard";
        }
    }
}