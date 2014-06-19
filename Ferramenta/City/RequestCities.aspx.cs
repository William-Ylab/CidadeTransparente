using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferramenta.City
{
    public partial class RequestCities : App_Code.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                updateInformationOnMasterPage(Resources.Label.request_cities, "fa fa-comments", "menu_period");
            }
        }
    }
}