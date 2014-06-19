using Site.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.User
{
    public partial class DashBoard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (this.Request.QueryString["p"] != null)
                    {
                        hdPeriodId.Value = this.Request.QueryString["p"].ToString();
                        var periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdPeriodId.Value));

                        lblTitle.Text = string.Format(Resources.Message.answer_question);

                        //loadGv();

                        using (Lib.Repositories.PeriodRepository rep = new Lib.Repositories.PeriodRepository(this.ActiveUser))
                        {
                            var lastPeriod = rep.getLastPublishedPeriod();

                            Lib.Entities.BaseForm currentForm = null;
                            using (Lib.Repositories.BaseFormRepository repBase = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
                            {
                                currentForm = repBase.getInstanceByPeriodId(periodId);
                            }

                            if (currentForm != null)
                            {
                                //Se for diferente do periodo aberto, não mostra a lista de cidades para submissão
                                if (periodId != lastPeriod.Id)
                                {
                                    if(!loadResponsableCities(ddlResponsableCities, periodId))
                                        ddlResponsableCities.Visible = false;
                                }
                                else
                                {
                                    ddlResponsableCities.Visible = false;
                                }

                                lblFormName.Text = currentForm.Name;
                                lblPeriod.Text = currentForm.Period.Name;
                                phStepToAnswer.Visible = true;
                            }
                            else
                            {
                                throw new ApplicationException(String.Format("Questionário para o período {0} não encontrado", periodId));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Site.User.Dashboard", ex);
                throw ex;
            }
        }
       
        protected void btnCurrentResponseForm_ServerClick(object sender, EventArgs e)
        {
            if (ddlResponsableCities.Visible)
            {
                if (!String.IsNullOrWhiteSpace(ddlResponsableCities.SelectedValue))
                    Response.Redirect(String.Format("CurrentForm.aspx?p={0}&c={1}", HttpUtility.UrlEncode(hdPeriodId.Value), HttpUtility.UrlEncode(ddlResponsableCities.SelectedValue)));
            }
            else
            {
                Response.Redirect(String.Format("CurrentForm.aspx?p={0}", HttpUtility.UrlEncode(hdPeriodId.Value)));
            }
        }

        protected void btnCurrentResponseFormOffline_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("/Form/Upload.aspx?p=" + HttpUtility.UrlEncode(hdPeriodId.Value));
        }
    }
}