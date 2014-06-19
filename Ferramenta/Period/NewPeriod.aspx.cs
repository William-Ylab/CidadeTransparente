using Ferramenta.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferramenta.Period
{
    public partial class NewPeriod : BasePage
    {
        protected override void AddRequiredUserTypes()
        {
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Master);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            updateInformationOnMasterPage(Resources.Label.new_period, "fa fa-calendar", "menu_period_manage");
            
            txtInitialDate.Attributes.Add("readonly", "readonly");
            txtFinalDate.Attributes.Add("readonly", "readonly");

            if (!Page.IsPostBack)
            {
                hdnPeriodId.Value = Request.QueryString["id"];

                if (!String.IsNullOrEmpty(hdnPeriodId.Value))
                {
                    loadForm();
                }

                verifyWarnings();
            }
        }

        private void verifyWarnings()
        {
            using (Lib.Repositories.PeriodRepository rep = new Lib.Repositories.PeriodRepository(this.ActiveUser))
            {
                var periodOpen = rep.getPeriodOpen();
                DateTime lastDate = DateTime.MinValue;

                var allPeriods = rep.getAll();
                if (allPeriods.Count > 0)
                {
                    lastDate = allPeriods.Max(p => p.FinalDate);
                }

                if (periodOpen != null)
                {
                    phMessageWarning.Visible = true;
                    ltMessageWarning.Text = String.Format(Resources.Message.exists_an_open_period_from_to, periodOpen.InitialDate, periodOpen.FinalDate);
                }

                if (lastDate != DateTime.MinValue)
                {
                    phMessageWarning2.Visible = true;
                    ltMessageWarning2.Text = String.Format(Resources.Message.need_create_period_from, lastDate);
                }
            }
        }

        private void loadForm()
        {
            using (Lib.Repositories.PeriodRepository rep = new Lib.Repositories.PeriodRepository(this.ActiveUser))
            {
                hdnPeriodId.Value = Page.Request.QueryString["id"];
                long periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdnPeriodId.Value));

                var period = rep.getInstanceById(periodId);
                txtName.Text = period.Name;
                txtFinalDate.Text = period.FinalDate.ToString("dd/MM/yyyy");
                txtInitialDate.Text = period.InitialDate.ToString("dd/MM/yyyy");
                txtConvocationInitialDate.Text = period.ConvocationInitialDate.ToString("dd/MM/yyyy");
                txtConvocationFinalDate.Text = period.ConvocationFinalDate.ToString("dd/MM/yyyy");

                txtName.Enabled = false;
                //txtInitialDate.Enabled = false;

                chkAberto.Checked = period.Open;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var cult = new System.Globalization.CultureInfo("pt-BR");

            long periodId = 0;
            phMessageError.Visible = false;
            phMessageSuccess.Visible = false;
            lblMessageError.Text = String.Empty;
            lblMessageSuccess.Text = String.Empty;

            Lib.Entities.Period p = new Lib.Entities.Period();
            p.ConvocationInitialDate = DateTime.Parse(txtConvocationInitialDate.Text, cult);
            p.ConvocationFinalDate = DateTime.Parse(txtConvocationFinalDate.Text, cult);
            p.FinalDate = DateTime.Parse(txtFinalDate.Text, cult);
            p.InitialDate = DateTime.Parse(txtInitialDate.Text, cult);
            p.Name = txtName.Text;
            p.Open = chkAberto.Checked;

            if (!String.IsNullOrEmpty(hdnPeriodId.Value))
            {
                periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdnPeriodId.Value));

                p.Id = periodId;
            }

            using (Lib.Repositories.PeriodRepository repository = new Lib.Repositories.PeriodRepository(this.ActiveUser))
            {
                repository.save(p);

                if (repository.HasErrors)
                {
                    phMessageError.Visible = true;
                    lblMessageError.Text = String.Join(",", repository.Errors);
                }
                else
                {
                    phMessageSuccess.Visible = true;
                    if (periodId == 0)
                    {

                        lblMessageSuccess.Text = String.Format("Período '{0}' foi criado com sucesso!", txtName.Text);
                    }
                    else
                    {
                        lblMessageSuccess.Text = String.Format("Período '{0}' foi atualizado com sucesso!", txtName.Text);
                    }

                    Response.Redirect("~/Period/List.aspx");
                }
            }
        }

        private void clearValues()
        {
            txtFinalDate.Text = "";
            txtInitialDate.Text = "";
            txtName.Text = "";
            txtConvocationFinalDate.Text = "";
            txtConvocationInitialDate.Text = "";
            chkAberto.Checked = false;
        }
    }
}