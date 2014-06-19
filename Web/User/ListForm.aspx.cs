using Site.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.User
{
    public partial class ListForm : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillDDL();

                //Recuperando o último período publicado e o período aberto para avaliação
                Lib.Entities.Period lastPeriod = null;
                Lib.Entities.Period periodOpen = null;
                div_statusSubmissao.Visible = false;
                div_cities_requested.Visible = false;

                using (Lib.Repositories.PeriodRepository repository = new Lib.Repositories.PeriodRepository(this.ActiveUser))
                {
                    lastPeriod = repository.getLastPublishedPeriod();
                    periodOpen = repository.getPeriodOpen();
                }

                //Verifica se existe periodo
                if (lastPeriod != null)
                {
                    //Verifica se existe questionário anterior
                    if (lastPeriod.BaseForms != null && lastPeriod.BaseForms.Count == 1)
                    {
                        hdLastPeriodId.Value = Commons.SecurityUtils.criptografar(lastPeriod.Id.ToString());
                        phLastForm.Visible = true;
                    }
                }

                if (this.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Entity)
                {
                    if (this.ActiveUser.Groups != null && this.ActiveUser.Groups.Count > 0)
                        div_statusSubmissao.Visible = true;

                    if (periodOpen != null)
                    {
                        //Verifica se existe questionário para responder
                        if (periodOpen.BaseForms != null && periodOpen.BaseForms.Count == 1 && periodOpen.isInSubmissionPeriod())
                        {
                            hdCurrentPeriodId.Value = Commons.SecurityUtils.criptografar(periodOpen.Id.ToString());
                            phCurrentForm.Visible = true;
                        }

                        //Caso a entidade seja credenciada e caso esteja em periodo de convocação
                        if (periodOpen.isInConvocationPeriod())
                        {
                            ltRequestMessage.Text = String.Format(Resources.Message.request_cities_may_be_until, periodOpen.FinalDate);
                            if (this.ActiveUser.isEntityAccreditedAndAppoved())
                            {
                                div_cities_requested.Visible = true;
                                hdnPeriodId.Value = periodOpen.Id.ToString();
                            }
                            else
                            {
                                div_cities_requested.Visible = false;
                            }
                        }

                        //Verifica se existe algum formulário não aprovado do período aberto
                        using (Lib.Repositories.ResponseFormRepository rep = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                        {
                            var notApprovedForms = rep.getResponseFormsByPeriod(periodOpen.Id).Where(rf=>rf.isNotApproved()).ToList();

                            if (notApprovedForms != null && notApprovedForms.Count > 0)
                            {
                                phHasNotApprovedForms.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        private void fillDDL()
        {
            using (Lib.Repositories.PeriodRepository repository = new Lib.Repositories.PeriodRepository(this.ActiveUser))
            {
                var periods = repository.getAll(); //repository.getByUserId(this.ActiveUser.Id);

                ddlPeriods.Items.Add(new ListItem("Sem filtro", "0"));

                foreach (var item in periods)
                {
                    ddlPeriods.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                }
            }
        }

        protected void btnLastForm_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("/User/Dashboard.aspx?p={0}", HttpUtility.UrlEncode(hdLastPeriodId.Value)));
        }

        protected void btnCurrentForm_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("/User/Dashboard.aspx?p={0}", HttpUtility.UrlEncode(hdCurrentPeriodId.Value)));
        }

        protected void btnRequestCity_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("/User/Request.aspx");
        }
    }
}