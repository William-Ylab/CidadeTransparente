using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta.Form
{
    public partial class ManageBaseForm : BasePage
    {
        //protected override void AddRequiredUserTypes()
        //{
        //    AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Admin);
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            updateInformationOnMasterPage(Resources.Label.manage_form, "fa fa-tasks", "menu_manage_baseform");

            if (!IsPostBack)
            {
                loadPeriods();
                getPreviousForms();

            }
        }

        private void getPreviousForms()
        {
            using (var rep = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
            {
                var listForms = rep.getAll().Select(f=> new
                    {
                        IdCrypt = Commons.SecurityUtils.criptografar( f.Id.ToString()),
                        Name = f.Name
                    }).OrderBy(f=>f.Name).ToList();

                if (listForms != null && listForms.Count > 0)
                {
                    ddlPreviousForms.DataSource = listForms;
                    ddlPreviousForms.DataBind();
                }
                else
                {
                    btnDesireCopyForm.Visible = false;
                }
            }
        }

        private void loadPeriods()
        {
            using (var rep = new Lib.Repositories.PeriodRepository(this.ActiveUser))
            {
                var periods = rep.getAll();

                if (periods != null && periods.Count > 0)
                {

                    var list = periods.OrderBy(p => p.InitialDate).ToList().Select(p => new
                    {
                        IdCrypt = Commons.SecurityUtils.criptografar(p.Id.ToString()),
                        Name = p.Open ? String.Format("{0} (Aberto)", String.Format(Resources.Label.list_item_period_label, p.Name, p.InitialDate, p.FinalDate)) : String.Format(Resources.Label.list_item_period_label, p.Name, p.InitialDate, p.FinalDate),
                        Open = p.Open
                    }).ToList();


                    ddlPeriods.DataSource = list;
                    ddlPeriods.DataBind();

                    var periodOpen = list.Where(a => a.Open == true).FirstOrDefault();

                    if (periodOpen != null)
                        ddlPeriods.SelectedIndex = list.IndexOf(periodOpen);
                }
                else
                {
                    ddlPeriods.Items.Add(new ListItem(Resources.Message.no_period_has_found, ""));
                }
            }
        }
    }
}