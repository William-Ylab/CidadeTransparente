using Lib.Repositories;
using Site.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.Form
{
    public partial class Ranking : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadStates();

                using (Lib.Repositories.PeriodRepository repository = new PeriodRepository(this.ActiveUser))
                {
                    var period = repository.getLastPublishedPeriod();

                    if (period != null && this.ActiveUser != null && this.ActiveUser.UserTypeEnum != Lib.Enumerations.UserType.Site)
                    {
                        lblTitle.Text = String.Format("{0} - {1} até {2}", period.Name, period.InitialDate.ToString("dd/MM/yyyy"), period.FinalDate.ToString("dd/MM/yyyy"));

                        //Verifica se existe questionário anterior
                        if (period.BaseForms != null && period.BaseForms.Count == 1)
                        {
                            hdLastPeriodId.Value = Commons.SecurityUtils.criptografar(period.Id.ToString());
                            phLastForm.Visible = true;
                        }
                    }
                    else
                    {
                        lblTitle.Text = "Resultado";
                    }
                }
            }
        }

        private void loadStates()
        {
            ddlState.Items.Add(new ListItem("Todos estados", ""));

            using (Lib.Repositories.StateCityRepository repository = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            {
                //ddlState
                foreach (var state in repository.getAllStates())
                {
                    ddlState.Items.Add(new ListItem(state.Name, state.Id.ToString()));
                }
            }
        }

        protected void btnLastForm_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("/User/Dashboard.aspx?p={0}", HttpUtility.UrlEncode(hdLastPeriodId.Value)));
        }
    }
}