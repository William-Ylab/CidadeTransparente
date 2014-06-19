using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta
{
    public partial class PageForbidden : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrWhiteSpace(Request.QueryString["m"]))
                {
                    lblMessage.Text = Request.QueryString["m"];
                }
                else
                {
                    lblMessage.Text = Resources.Message.restrict_page_message;
                }
            }
        }
    }
}