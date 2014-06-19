using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Site.App_Code;

namespace Site.User
{
    public partial class Request : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStates();
            }
        }

        private void loadStates()
        {
            using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            {
                var states = rep.getAllStates();

                if (states != null)
                {
                    ddlState.DataSource = states;
                    ddlState.DataBind();

                    ddlState.Items.Insert(0, new ListItem(Resources.Message.select_a_state, ""));

                    ddlCity.Items.Clear();
                    ddlCity.Items.Insert(0, new ListItem(Resources.Message.select_a_state, ""));
                }

            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ddlState.SelectedValue))
            {
                using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(this.ActiveUser))
                {
                    var cities = rep.getCitiesByUF(ddlState.SelectedValue);

                    if (cities != null)
                    {
                        ddlCity.DataSource = cities;
                        ddlCity.DataBind();

                        ddlCity.Items.Insert(0, new ListItem(Resources.Message.select_a_city1, ""));
                    }
                }
            }
            else
            {
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem(Resources.Message.select_a_state, ""));
            }
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ddlCity.SelectedValue))
            {
                phError.Visible = false;

                Lib.Enumerations.RequestType requestType = rbResp.Checked ? Lib.Enumerations.RequestType.RESPONSABLE : Lib.Enumerations.RequestType.COLLABORATOR;

                //Salvar requisição.
                using (Lib.Repositories.PeriodRepository per = new Lib.Repositories.PeriodRepository(this.ActiveUser))
                {

                    var period = per.getOpenPeriodWithConvocation();
                    using (Lib.Repositories.UserRepository ctx = new Lib.Repositories.UserRepository(this.ActiveUser))
                    {

                        if (period != null)
                        {
                            ctx.requestCity(Convert.ToInt64(ddlCity.SelectedValue), this.ActiveUser.Id, period.Id, requestType);

                            if (ctx.HasErrors)
                            {
                                phError.Visible = true;
                                ltErrorMessage.Text = String.Join(",", ctx.Errors);
                            }
                            else
                            {
                                Response.Redirect("/User/ListForm.aspx");
                            }
                        }
                    }
                }
            }
        }
    }
}