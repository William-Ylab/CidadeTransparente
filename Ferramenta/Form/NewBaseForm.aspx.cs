using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta.Form
{
    public partial class NewBaseForm : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Attributes["page_title"] = Resources.Label.create_a_new_baseform;
            this.Master.Attributes["page_icon"] = "fa fa-tasks";
            this.Master.Attributes["active_menu_id"] = "menu_manage_baseform";
        }
    }
}