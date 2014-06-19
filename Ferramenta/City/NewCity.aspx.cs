using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ferramenta.App_Code;

namespace Ferramenta.City
{
    public partial class NewCity : BasePage
    {
        protected override void AddRequiredUserTypes()
        {
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Admin);
            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Master);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            updateInformationOnMasterPage(Resources.Label.new_city, "fa fa-globe", "menu_manage_cities");

            if (!IsPostBack)
            {
                loadStates();
                loadEntities();

                //Carrega o município se necessário
                if (!String.IsNullOrWhiteSpace(Page.Request.QueryString["id"]))
                {
                    try
                    {
                        hdCityId.Value = Page.Request.QueryString["id"];
                        long cityId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdCityId.Value));

                        using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(ActiveUser))
                        {
                            var city = rep.getCityInstanceById(cityId);

                            AcceptedUsersTypeInPage.Add(Lib.Enumerations.UserType.Master);
                            if (city != null)
                            {
                                txtCityName.Text = city.Name;
                                ddlState.SelectedValue = city.StateId;

                                loadPeriodInformation(city);

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Lib.Log.ErrorLog.saveError("Ferramenta.City.NewCity.Page_Load", ex);
                        phMessageError.Visible = false;
                        lblMessageError.Text = Resources.Message.problem_to_load_requested_user;
                    }

                }
                else
                {
                    loadPeriodInformation(null);
                }
            }
        }

        //private void loadRequestCities(long cityId, long periodId)
        //{
        //    using (Lib.Repositories.UserRepository ctx = new Lib.Repositories.UserRepository(this.ActiveUser))
        //    {
        //        var listPendingRequests = ctx.getAllRequestsFromCity(cityId, periodId);

        //        listPendingRequests.Select(l=>l.
        //    }
        //}

        private void loadPeriodInformation(Lib.Entities.City city)
        {
            phOpenPeriodInformation.Visible = false;
            using (Lib.Repositories.PeriodRepository ctx = new Lib.Repositories.PeriodRepository(this.ActiveUser))
            {
                var periodOpen = ctx.getPeriodOpen();

                if (periodOpen != null)
                {
                    ltOpenPeriodTitle.Text = String.Format("Responsáveis e colaboradores no período ({0})", periodOpen.Name);
                    hdOpenPeriodId.Value = Commons.SecurityUtils.criptografar(periodOpen.Id.ToString());
                    phOpenPeriodInformation.Visible = true;

                    if (city != null)
                    {
                        //Recupera o grupo da cidade e periodo selecionado
                        var group = city.Groups.Where(g => g.PeriodId == periodOpen.Id).FirstOrDefault();

                        if (group != null)
                        {
                            var list = group.Collaborators.Select(c => new
                            {
                                Id = c.Id,
                                Name = c.Name
                            }).ToList();


                            JavaScriptSerializer js = new JavaScriptSerializer();
                            hdCurrentCollabsJson.Value = js.Serialize(list);

                            hdCollabValues.Value = String.Join(",", list.Select(i => i.Id).ToList());
                            hdCurrentCollabs.Value = hdCollabValues.Value;

                            if (group.Responsable != null)
                            {
                                ddlResponsable.SelectedValue = group.ResponsableId.ToString();
                            }
                        }

                        //loadRequestCities(city.Id, periodOpen.Id);
                    }
                }
            }
        }

        private void loadEntities()
        {
            using (Lib.Repositories.UserRepository cont = new Lib.Repositories.UserRepository(this.ActiveUser))
            {
                var entities = cont.getAllAccreditedEntities();

                if (entities != null)
                {
                    var list = entities.Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();

                    ddlCollaborator.DataSource = list;
                    ddlCollaborator.DataBind();
                    ddlCollaborator.Items.Insert(0, new ListItem(Resources.Label.select_a_collaborator, ""));

                    ddlResponsable.DataSource = list;
                    ddlResponsable.DataBind();
                    ddlResponsable.Items.Insert(0, new ListItem(Resources.Label.any_responsable_selected, ""));

                    if (list.Count > 0)
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        hdCollabJson.Value = js.Serialize(list);
                    }

                }

            }
        }

        private void loadStates()
        {
            using (Lib.Repositories.StateCityRepository cont = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            {
                ddlState.DataSource = cont.getAllStates();
                ddlState.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            phMessageError.Visible = false;
            phMessageSuccess.Visible = false;
            lblMessageError.Text = String.Empty;
            lblMessageSuccess.Text = String.Empty;

            if (String.IsNullOrWhiteSpace(txtCityName.Text))
            {
                phMessageError.Visible = true;
                lblMessageError.Text = Resources.Message.city_name_is_mandatory;
                return;
            }

            string mimeType = string.Empty;

            long cityId = !String.IsNullOrWhiteSpace(hdCityId.Value) ? Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdCityId.Value)) : 0;

            using (Lib.Repositories.StateCityRepository repository = new Lib.Repositories.StateCityRepository(ActiveUser))
            {
                Lib.Entities.City city = null;

                if (cityId > 0)
                {
                    //Carrega o cidade existente
                    city = repository.getCityInstanceById(cityId);
                }
                else
                {
                    //Nova cidade
                    city = new Lib.Entities.City();
                }

                city.Name = txtCityName.Text;
                city.StateId = ddlState.SelectedValue;

                repository.saveCity(city);

                if (repository.HasErrors)
                {
                    phMessageError.Visible = true;
                    lblMessageError.Text = String.Join(",", repository.Errors);
                }
                else
                {
                    //Adiciona o responsável
                    if (!String.IsNullOrWhiteSpace(ddlResponsable.SelectedValue))
                    {
                        addResponsable(long.Parse(ddlResponsable.SelectedValue), city.Id);
                    }
                    else
                    {
                        removeResponsable(city.Id);
                    }

                    //Separa os colaboradores adicionados e os removidos
                    updateCollaborators(city.Id);

                    Response.Redirect("~/City/ManageCities.aspx");
                }
            }
        }



        private void updateCollaborators(long cityId)
        {
            var newCollaborators = hdCollabValues.Value.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var currentCollaborators = hdCurrentCollabs.Value.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);

            List<long> removedEntities = new List<long>();
            List<long> addedEntities = new List<long>();

            foreach (var currentId in currentCollaborators)
            {
                if (!newCollaborators.Contains(currentId))
                {
                    //Entidade não é mais colaboradora
                    removedEntities.Add(Convert.ToInt64(currentId));
                }
            }

            foreach (var currentId in newCollaborators)
            {
                if (!currentCollaborators.Contains(currentId))
                {
                    //Entidade não é mais colaboradora
                    addedEntities.Add(Convert.ToInt64(currentId));
                }
            }

            if (addedEntities.Count > 0 || removedEntities.Count > 0)
            {
                //Adiciona o responsável para o período aberto
                long periodOpenId = 0;

                long.TryParse(Commons.SecurityUtils.descriptografar(hdOpenPeriodId.Value), out periodOpenId);

                if (periodOpenId > 0)
                {
                    using (Lib.Repositories.UserRepository ctx = new Lib.Repositories.UserRepository(this.ActiveUser))
                    {
                        ctx.updateCollaborators(addedEntities, removedEntities, periodOpenId, cityId);
                    }
                }
            }

        }

        private void addResponsable(long userId, long cityId)
        {
            //Adiciona o responsável para o período aberto
            long periodOpenId = 0;

            long.TryParse(Commons.SecurityUtils.descriptografar(hdOpenPeriodId.Value), out periodOpenId);

            if (periodOpenId > 0)
            {
                using (Lib.Repositories.UserRepository ctx = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    ctx.approveRequest(userId, periodOpenId, cityId, Lib.Enumerations.RequestType.RESPONSABLE);
                }
            }
        }

        private void removeResponsable(long cityId)
        {
            //Adiciona o responsável para o período aberto
            long periodOpenId = 0;

            long.TryParse(Commons.SecurityUtils.descriptografar(hdOpenPeriodId.Value), out periodOpenId);

            if (periodOpenId > 0)
            {
                using (Lib.Repositories.UserRepository ctx = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    ctx.removeResponsable(periodOpenId, cityId);
                }
            }
        }
    }
}