using Site.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.Form
{
    public partial class Upload : BasePage
    {
        private string filePath = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Recupera o questionário de acordo com o periodo solicitado
                if (this.Request.QueryString["p"] != null)
                {
                    hdPeriodId.Value = this.Request.QueryString["p"].ToString();
                    var periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdPeriodId.Value));

                    phEntityUser.Visible = false;
                    phCommonUser.Visible = false;

                    if (this.ActiveUser.isEntityAccreditedAndAppoved())
                    {
                        if (this.ActiveUser.ResponsableGroups != null && this.ActiveUser.ResponsableGroups.Count > 0)
                        {
                            li_selectcity.Visible = true;
                            loadResponsableCities(ddlCities, periodId);
                            phEntityUser.Visible = true;
                        }
                        else
                        {
                            li_selectcity.Visible = false;
                            phCommonUser.Visible = true;
                        }
                    }
                    else
                    {
                        phCommonUser.Visible = true;
                        li_selectcity.Visible = false;
                    }

                    using (Lib.Repositories.BaseFormRepository rep = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
                    {
                        var baseForm = rep.getInstanceByPeriodId(periodId);

                        if (baseForm != null)
                        {
                            ulsteps.Visible = true;
                            lblMessage.Visible = false;
                        }
                        else
                        {
                            lblMessage.Visible = true;
                            ulsteps.Visible = false;
                        }
                    }
                }
                else { lblTitle.Text = string.Format("Desculpe, mas não encontramos a página solicitada."); }
            }

        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                Lib.Entities.BaseForm baseForm = null;
                string excelname = "file.xlsx";
                var periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdPeriodId.Value));

                using (Lib.Repositories.BaseFormRepository repo = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
                {
                    baseForm = repo.getInstanceByPeriodId(periodId);
                }

                if (baseForm != null)
                {
                    excelname = Commons.StringUtils.removeSpecialCaracters(baseForm.Name) + ".xlsx";

                    filePath = Server.MapPath("/App_Data/" + excelname);
                    Lib.Utils.ExcelUtils utils = new Lib.Utils.ExcelUtils();

                    bool save = utils.createExcel(baseForm, filePath, true);

                    if (save)
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(filePath);

                        Response.Clear();
                        Response.Buffer = true;

                        Response.AddHeader("content-disposition", "attachment;filename=" + excelname);
                        Response.ContentEncoding = Encoding.UTF8;

                        Response.Cache.SetCacheability(HttpCacheability.Private);
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.WriteFile(file.FullName);

                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Upload.aspx.cs", ex);
                hdnError.Value = "Não foi possível gerar o excel, por favor entre em contato com o administrador.";
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ddlCities.SelectedValue))
            {
                Lib.Utils.ExcelUtils utils = new Lib.Utils.ExcelUtils();
                string extensionDefault = ".xlsx";
                string url = "";

                if (fuForm.HasFile)
                {
                    string extension = System.IO.Path.GetExtension(fuForm.FileName);

                    if (extensionDefault != extension)
                    {
                        hdnError.Value = "O arquivo deve ser do tipo excel (xlsx).";
                    }
                    else
                    {
                        try
                        {
                            filePath = Server.MapPath("/App_Data/fileUploaded.xlsx");
                            fuForm.SaveAs(filePath);

                            DataSet dataSet = utils.importExcelFile(filePath);
                            List<string> notAnsweredQuestions = null;
                            List<string> incorrectAnsweredQuestions = null;

                            bool canShowButton = canShowSubmitButton(dataSet.Tables, out notAnsweredQuestions, out incorrectAnsweredQuestions);

                            if (canShowButton)
                            {
                                //rptListBlocks.DataSource = dataSet.Tables;
                                //rptListBlocks.DataBind();

                                long id = saveResponseForm(dataSet.Tables);

                                url = String.Format("~/Form/View.aspx?rfid={0}", Commons.SecurityUtils.criptografar(id.ToString()));
                            }
                            else
                            {
                                StringBuilder sb = new StringBuilder();

                                if (notAnsweredQuestions.Count > 0)
                                {
                                    sb.AppendLine(String.Format("Não foi possível carregar o arquivo pois ainda existem perguntas sem respostas."));
                                    sb.AppendLine();

                                    foreach (var item in notAnsweredQuestions)
                                    {
                                        sb.AppendLine(item);
                                    }
                                }

                                if (incorrectAnsweredQuestions.Count > 0)
                                {
                                    sb.AppendLine();
                                    sb.AppendLine();
                                    sb.AppendLine(String.Format("Perguntas com respostas incorretas: "));
                                    sb.AppendLine();

                                    foreach (var item in incorrectAnsweredQuestions)
                                    {
                                        sb.AppendLine(item);
                                    }
                                }

                                litNotAnsweredQuestions.Text = sb.ToString().Replace("\r\n", "<br/>");
                            }
                        }
                        catch
                        {
                            hdnError.Value = "Arquivo inválido. Exporte o questionário e preencha novamente.";
                        }
                    }
                }
                else
                {
                    hdnError.Value = "Por favor, selecione o questionário respondido.";
                }

                if (!string.IsNullOrEmpty(url))
                {
                    Response.Redirect(url);
                }
            }
        }

        protected bool canShowSubmitButton(DataTableCollection tables, out List<string> notAnsweredQuestions, out List<string> incorrectAnsweredQuestions)
        {
            notAnsweredQuestions = new List<string>();
            incorrectAnsweredQuestions = new List<string>();

            List<String> AcceptedAnswers = new List<string>();
            AcceptedAnswers.Add("n/a");
            AcceptedAnswers.Add("N/A");
            AcceptedAnswers.Add("0");
            AcceptedAnswers.Add("0.0");
            AcceptedAnswers.Add("0.00");
            AcceptedAnswers.Add("0,0");
            AcceptedAnswers.Add("0,00");
            AcceptedAnswers.Add("0.25");
            AcceptedAnswers.Add("0,25");
            AcceptedAnswers.Add("0.5");
            AcceptedAnswers.Add("0,5");
            AcceptedAnswers.Add("0.50");
            AcceptedAnswers.Add("0,50");
            AcceptedAnswers.Add("0.75");
            AcceptedAnswers.Add("0,75");
            AcceptedAnswers.Add("1");
            AcceptedAnswers.Add("1.0");
            AcceptedAnswers.Add("1,0");
            AcceptedAnswers.Add("1.00");
            AcceptedAnswers.Add("1,00");

            foreach (DataTable table in tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row[4].ToString() == "P")
                    {
                        if (String.IsNullOrEmpty(row[2].ToString()))
                        {
                            notAnsweredQuestions.Add(row[0] + " - " + row[1]);
                        }
                        else
                        {
                            if (!AcceptedAnswers.Contains(row[2].ToString()))
                            {
                                incorrectAnsweredQuestions.Add(row[0] + " - " + row[1]);
                            }
                        }
                    }
                }
            }

            if (notAnsweredQuestions.Count > 0 || incorrectAnsweredQuestions.Count > 0)
            {
                return false;
            }

            return true;
        }

        private long saveResponseForm(DataTableCollection tables)
        {
            Lib.Entities.ResponseForm form = new Lib.Entities.ResponseForm();
            form.Answers = new List<Lib.Entities.Answer>();
            form.UserId = this.ActiveUser.Id;

            form.CityId = null;

            Lib.Entities.Answer resposta = null;
            foreach (DataTable table in tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    resposta = new Lib.Entities.Answer();
                    resposta.BaseQuestionId = long.Parse(row.ItemArray[5].ToString());
                    resposta.Observation = row.ItemArray[3].ToString();

                    decimal score = 0;
                    if (decimal.TryParse(row.ItemArray[2].ToString(), NumberStyles.Any, new System.Globalization.CultureInfo("en-US"), out score))
                    {
                        resposta.Score = score;
                    }

                    form.BaseFormId = long.Parse(row.ItemArray[6].ToString());
                    form.Answers.Add(resposta);

                    resposta = null;
                }
            }

            using (Lib.Repositories.ResponseFormRepository rep = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
            {
                //Tratamento para caso a entidade não tenha cidade responsável, a gente não submita o questionário para avaliação
                if (this.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Entity)
                {
                    if (this.ActiveUser.ResponsableGroups != null && this.ActiveUser.ResponsableGroups.Count > 0)
                    {
                        form.CityId = Convert.ToInt64(ddlCities.SelectedValue);
                        rep.save(form);
                    }
                    else
                    {
                        //Entidade sem nenhuma cidade responsável
                        rep.save(form, false);
                    }
                }
                else
                {
                    //Usuário comum
                    rep.save(form, false);
                }
            }

            return form.Id;
        }

        private void loadCities(DropDownList ddlCities, long periodId)
        {
            //Seleciona as cidades que a entidade pode submeter no periodo
            ddlCities.Items.Clear();

            using (Lib.Repositories.StateCityRepository ctx = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            {
                var listResponsableCities = ctx.getCitiesFromResponsableUser(this.ActiveUser.Id, periodId);

                //Precisa ser responsável por alguma cidade
                if (listResponsableCities != null && listResponsableCities.Count > 0)
                {
                    List<Lib.Entities.ResponseForm> responsesForm = null;

                    using (var context = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                        responsesForm = context.getResponseFormsByUserIdAndPeriodId(this.ActiveUser.Id, periodId);

                    foreach (var responsableCity in listResponsableCities)
                    {
                        if (responsesForm != null && responsesForm.Count > 0)
                        {
                            var formByCity = responsesForm.Where(f => f.CityId == responsableCity.Id).FirstOrDefault();

                            if (formByCity != null)
                            {
                                if (!formByCity.isSubmitted() && !formByCity.isApproved() && !formByCity.isNotApproved())
                                {
                                    ddlCities.Items.Add(new ListItem(String.Format("{0} (Em andamento)", responsableCity.Name), Commons.SecurityUtils.criptografar(responsableCity.Id.ToString())));
                                }
                                else if (formByCity.isNotApproved())
                                {
                                    ddlCities.Items.Add(new ListItem(String.Format("{0} (Reprovado)", responsableCity.Name), Commons.SecurityUtils.criptografar(responsableCity.Id.ToString())));
                                }
                            }
                            else
                            {
                                ddlCities.Items.Add(new ListItem(responsableCity.Name, Commons.SecurityUtils.criptografar(responsableCity.Id.ToString())));
                            }
                        }
                        else
                        {
                            //Nenhum questionário respondido
                            //Usuário pode selecionar N cidades
                            ddlCities.Items.Add(new ListItem(responsableCity.Name, Commons.SecurityUtils.criptografar(responsableCity.Id.ToString())));
                        }
                    }
                }
            }


            //using (Lib.Repositories.StateCityRepository ctx = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            //{
            //    var listResponsableCities = ctx.getCitiesFromResponsableUser(this.ActiveUser.Id, periodId);
            //    hdnCityId.Value = listResponsableCities[0].Id.ToString();
            //    foreach (var city in listResponsableCities)
            //    {
            //        ddlCities.Items.Add(new ListItem(String.Format("{0} - {1}", city.Name, city.StateId), city.Id.ToString()));
            //    }
            //}

            //var states = cities.Select(f => f.State).Distinct().ToList();

            //if (states != null && states.Count > 0)
            //{
            //    foreach (var state in states.OrderBy(f => f.Name).ToList())
            //    {
            //        ddlState.Items.Add(new ListItem(state.Name, state.Id.ToString()));
            //    }

            //    hdnCityId.Value = cities[0].Id.ToString();

            //    foreach (var city in cities.Where(f => f.StateId == ddlState.SelectedValue).ToList())
            //    {
            //        ddlCities.Items.Add(new ListItem(city.Name, city.Id.ToString()));
            //    }
            //}

            //using (Lib.Repositories.StateCityRepository repository = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            //{
            //    //ddlState
            //    foreach (var state in repository.getAllStates())
            //    {
            //        ddlState.Items.Add(new ListItem(state.Name, state.Id.ToString()));
            //    }

            //    var cities = repository.getCitiesByUF(ddlState.SelectedValue);

            //    hdnCityId.Value = cities[0].Id.ToString();

            //    foreach (var city in cities)
            //    {
            //        ddlCities.Items.Add(new ListItem(city.Name, city.Id.ToString()));
            //    }
            //}
        }
    }
}