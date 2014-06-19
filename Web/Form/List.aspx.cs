using Site.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.Form
{
    public partial class List : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            loadRanking();
        }

        private void loadRanking()
        {
            using (Lib.Repositories.ResponseFormRepository repo = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
            {
                var responsesForms = repo.getAll();

                var listObjForm = responsesForms.Select(f => new
                {
                    CityName = f.City.Name + " (" + f.City.StateId + ")",
                    FormName = f.BaseForm.Name,
                    Id = f.Id,
                    TotalScore = f.TotalScore,
                    UserName = f.User.Name
                });

                gvFormsRanking.DataSource = listObjForm.OrderByDescending(f => f.TotalScore).ToList();
                gvFormsRanking.DataBind();
            }
        }

        protected void gvFormsRanking_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewForm")
            {
                Response.Redirect(String.Format("~/Form/View.aspx?rfid={0}", Commons.SecurityUtils.criptografar(e.CommandArgument.ToString())));
            }
        }
    }
}