using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta.City
{
    public partial class ManageCities : BasePage
    {
        protected override void AddRequiredUserTypes()
        {
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Admin);
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Master);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            updateInformationOnMasterPage(Resources.Label.manage_city, "fa fa-globe", "menu_manage_cities");

            if (!IsPostBack)
            {
                loadStates();

                loadPeriods();
            }
        }

        private void loadStates()
        {
            using (Lib.Repositories.StateCityRepository cont = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            {
                ddlStates.DataSource = cont.getAllStates();
                ddlStates.DataBind();
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
                        Id = Commons.SecurityUtils.criptografar(p.Id.ToString()),
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

        protected void btnNewCity_Click(object sender, EventArgs e)
        {
            Response.Redirect("/City/NewCity.aspx");
        }
    }
}