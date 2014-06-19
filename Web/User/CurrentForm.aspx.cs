using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Site.App_Code;

namespace Site.User
{
    public partial class CurrentForm : BasePage
    {
        private const string SESSION_CURRENT_RESPONSEFORM = "FDT_CURRENT_RESPONSE_FORM";
        private const string SESSION_CURRENT_FORM = "FDT_CURRENT_FORM";

        private const string greenColor = "#86aa65";
        private const string orangeColor = "#eda637";
        private const string redColor = "#d94c4b";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Lib.Entities.BaseForm form = null;

                    //Recupera o questionário de acordo com o periodo solicitado
                    if (this.Request.QueryString["p"] != null)
                    {
                        hdPeriodId.Value = this.Request.QueryString["p"].ToString();
                        var periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(this.Request.QueryString["p"].ToString()));
                        
                        hdCityId.Value = string.Empty;
                        long? cityId = null;

                        if (!String.IsNullOrWhiteSpace(this.Request.QueryString["c"]))
                        {
                            hdCityId.Value = this.Request.QueryString["c"].ToString();
                            cityId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(this.Request.QueryString["c"].ToString()));
                        }

                        using (var context = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
                        {
                            form = context.getInstanceByPeriodId(periodId);
                        }

                        if (form != null)
                        {
                            Session[SESSION_CURRENT_FORM] = form;
                            Session[SESSION_CURRENT_RESPONSEFORM] = null;

                            //hdPeriodId.Value = Commons.SecurityUtils.criptografar(form.PeriodId.ToString());

                            using (Lib.Repositories.PeriodRepository repository = new Lib.Repositories.PeriodRepository(this.ActiveUser))
                            {
                                var periodOpen = repository.getPeriodOpen();

                                if (cityId.HasValue)
                                {
                                    using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(this.ActiveUser))
                                    {
                                        Lib.Entities.City city = rep.getCityInstanceById(cityId.Value);

                                        if (city != null)
                                        {
                                            ul_title.Visible = true;
                                            lblTitle.Text = String.Format("Formulário referente à cidade de {0}-{1}", city.Name, city.StateId);
                                        }
                                    }
                                }
                                

                                if (periodOpen.Id == periodId)
                                {
                                    //Periodo aberto, precisa ter uma cidade selecionada
                                    if(String.IsNullOrWhiteSpace(hdCityId.Value))
                                        Response.Redirect("/User/ListForm.aspx");
                                }
                            }

                            //Verifica se existe algum formulário respondido
                            using (var context = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                            {
                                var responsesForm = context.getResponseFormsByUserIdAndPeriodId(this.ActiveUser.Id, form.PeriodId);

                                var selectedResponseForm = responsesForm.Where(rf => rf.CityId == cityId).FirstOrDefault();

                                if (selectedResponseForm != null)
                                {
                                    Session[SESSION_CURRENT_RESPONSEFORM] = selectedResponseForm;
                                    hdResponseFormId.Value = Commons.SecurityUtils.criptografar(selectedResponseForm.Id.ToString());
                                }
                            }

                            rptIndex.DataSource = form.BaseBlocks.ToList();
                            rptIndex.DataBind();

                            rptForms.DataSource = form.BaseBlocks.ToList();
                            rptForms.DataBind();

                            verifyFormStatus();
                        }
                        else
                        {
                            Response.Redirect("/User/ListForm.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("/User/ListForm.aspx");
                    }
                }
                catch (Exception ex)
                {
                    Lib.Log.ErrorLog.saveError("Site.User.CurrentForm", ex);

                    Response.Redirect("/User/ListForm.aspx");
                }
            }
        }

        private void verifyFormStatus()
        {
            phSaveForm.Visible = true;
            btnSubmitForm.Visible = true;
            phMustRevised.Visible = false;
            ltNotAprovedObservations.Text = string.Empty;
            ltFinishText.Text = string.Empty;

            Lib.Entities.ResponseForm responseForm = (Lib.Entities.ResponseForm)Session[SESSION_CURRENT_RESPONSEFORM];

            if (responseForm != null)
            {
                if (responseForm.isAlreadyAnswered())
                {
                    var periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdPeriodId.Value));

                    phFinishForm.Visible = true;

                    if (this.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Entity && !formIsApproved())
                    {
                        //Precisa ser uma entidade rede
                        if (this.ActiveUser.isEntityAccreditedAndAppoved())
                        {
                            //Verifica se o questionário está concluido e mostra o botão para submeter
                            if (responseForm.CityId != null && responseForm.CityId > 0)
                            {
                                using (Lib.Repositories.PeriodRepository repository = new Lib.Repositories.PeriodRepository(this.ActiveUser))
                                {
                                    var periodOpen = repository.getPeriodOpen();

                                    if (periodOpen.isInSubmissionPeriod())
                                    {
                                        phSubmitForm.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        phViewFormDetails.Visible = true;
                    }

                    if (responseForm.isApproved())
                    {
                        ltFinishText.Text = Resources.Message.congratulations_your_form_has_been_filled;
                        phSaveForm.Visible = false;
                        btnSubmitForm.Visible = false;
                    }
                    else if (responseForm.isSubmitted())
                    {
                        ltFinishText.Text = Resources.Message.congratulations_your_form_has_been_filled_and_submited;
                        phSaveForm.Visible = false;
                        btnSubmitForm.Visible = false;
                    }
                    else if (responseForm.isNotApproved())
                    {
                        phMustRevised.Visible = true;
                        ltNotAprovedObservations.Text = responseForm.getNotApprovedObservations();
                    }
                }
            }
        }

        #region [Custom Events]

        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var hdQuestionId = (HiddenField)e.Item.FindControl("hdQuestionId");
            var ddlAnswer = (DropDownList)e.Item.FindControl("ddlAnswer");
            var txtObservation = (TextBox)e.Item.FindControl("txtObservation");

            if (Session[SESSION_CURRENT_RESPONSEFORM] != null)
            {
                var responseForm = (Lib.Entities.ResponseForm)Session[SESSION_CURRENT_RESPONSEFORM];

                var questionAnswered = responseForm.Answers.Where(a => a.BaseQuestionId == Convert.ToInt64(hdQuestionId.Value)).FirstOrDefault();

                if (questionAnswered != null)
                {
                    txtObservation.Text = questionAnswered.Observation;
                    ddlAnswer.SelectedValue = setScore(questionAnswered.Score);
                }

                //Se o questionário estiver Aprovado ou submetido, não será possivel editar
                if (responseForm.isSubmitted() || responseForm.isApproved())
                {
                    ddlAnswer.Enabled = false;
                    txtObservation.Enabled = false;
                }
            }
        }

        protected void btnSubmitForm_ServerClick(object sender, EventArgs e)
        {
            //lblCityError.Text = string.Empty;
            try
            {
                var responseForm = getResponseFormUpdated();

                if (responseForm.CityId != null)
                {

                    using (var context = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                    {
                        //Salva o questionário e submita para avaliação
                        context.save(responseForm, true);
                    }

                    redirectToFormDetails(responseForm.Id);
                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Web.CurrentForm.btnSaveForm_ServerClick", ex);
            }
        }

        #endregion

        //#region [Private Methods]

        private void loadResponsableCities()
        {
            //using (Lib.Repositories.StateCityRepository ctx = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            //{
            //    var listResponsableCities = ctx.getCitiesFromResponsableUser(this.ActiveUser.Id, periodId);

            //    //Precisa ser responsável por alguma cidade
            //    if (listResponsableCities != null && listResponsableCities.Count > 0)
            //    {
            //        if (responseForm.CityId == null)
            //        {
            //            if (listResponsableCities.Count > 0)
            //            {
            //                //Usuário pode selecionar N cidades
            //                ddlCities.DataSource = listResponsableCities;
            //                ddlCities.DataBind();
            //                ddlCities.Items.Insert(0, new ListItem(Resources.Message.select_a_city, ""));

            //                //Disponibiliza o botão para submeter o formulário para avaliação, caso o questionário tenha sido aprovado, a entidade não pode mais submeter
            //                phSubmitForm.Visible = true;
            //            }
            //            else
            //            {
            //                phSubmitForm.Visible = false;
            //            }
            //        }
            //        else
            //        {
            //            //Informa apenas a cidade que o usuário já selecionou
            //            var filtered = listResponsableCities.Where(c => c.Id == responseForm.CityId).ToList();
            //            if (filtered.Count > 0)
            //            {
            //                ddlCities.DataSource = filtered;
            //                ddlCities.DataBind();

            //                phSubmitForm.Visible = true;
            //            }
            //            else
            //            {
            //                phSubmitForm.Visible = false;
            //            }
            //        }
            //    }
            //}
        }

        private Lib.Entities.ResponseForm getResponseFormUpdated()
        {
            long periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdPeriodId.Value));


            long? cityId = null;

            if (!String.IsNullOrWhiteSpace(hdCityId.Value))
                cityId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdCityId.Value));

            Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];

            //Pega o questionário salvo 
            Lib.Entities.ResponseForm responseForm = getResponseForm(cityId, periodId);

            if (responseForm == null)
            {
                //Novo questionário
                responseForm = new Lib.Entities.ResponseForm();
                responseForm.Answers = new List<Lib.Entities.Answer>();
                responseForm.BaseFormId = baseForm.Id;
                responseForm.CityId = cityId;
                responseForm.UserId = this.ActiveUser.Id;
            }

            if (rptForms != null)
            {
                foreach (RepeaterItem blockItem in rptForms.Items)
                {
                    var rptSubblocks = (Repeater)blockItem.FindControl("rptSubblocks");

                    if (rptSubblocks != null)
                    {
                        foreach (RepeaterItem subblockItem in rptSubblocks.Items)
                        {
                            var rptQuestions = (Repeater)subblockItem.FindControl("rptQuestions");
                            if (rptQuestions != null)
                            {
                                foreach (RepeaterItem questionItem in rptQuestions.Items)
                                {
                                    var hdQuestionId = (HiddenField)questionItem.FindControl("hdQuestionId");
                                    var ddlAnswer = (DropDownList)questionItem.FindControl("ddlAnswer");
                                    var txtObservation = (TextBox)questionItem.FindControl("txtObservation");


                                    var selectedValue = getScore(ddlAnswer.SelectedValue);
                                    if (selectedValue != -1)
                                    {
                                        //Verifica se a questão já foi respondida
                                        Lib.Entities.Answer answer = responseForm.Answers.Where(a => a.BaseQuestionId == Convert.ToInt64(hdQuestionId.Value)).FirstOrDefault();

                                        //Atualiza/Setta as informações da questão atual.
                                        if (answer == null)
                                        {
                                            //Pergunta não foi respondida
                                            answer = new Lib.Entities.Answer();
                                            answer.BaseQuestionId = Convert.ToInt64(hdQuestionId.Value);

                                            answer.Observation = txtObservation.Text;
                                            answer.Score = selectedValue;

                                            responseForm.Answers.Add(answer);
                                        }
                                        else
                                        {
                                            answer.Observation = txtObservation.Text;
                                            answer.Score = selectedValue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return responseForm;
        }

        private Lib.Entities.ResponseForm getResponseForm(long? cityId, long periodId)
        {
            Lib.Entities.ResponseForm responseForm = null;
            //Verifica se existe algum formulário respondido
            using (var context = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
            {
                var responsesForm = context.getResponseFormsByUserIdAndPeriodId(this.ActiveUser.Id, periodId);

                if (responsesForm != null && responsesForm.Count > 0)
                {
                    responseForm = responsesForm.Where(r => r.CityId == cityId).FirstOrDefault();
                }
            }

            return responseForm;
        }

        private decimal? getScore(string p)
        {
            switch (p)
            {
                case "":
                    return -1;
                case "NA":
                    return null;
                case "0":
                    return 0;
                case "1":
                    return 1;
                case "025":
                    return 0.25m;
                case "05":
                    return 0.50m;
                case "075":
                    return 0.75m;
            }

            return -1;
        }

        private string setScore(decimal? score)
        {
            if (score == null)
            {
                return "NA";
            }
            else if (score == 0)
            {
                return "0";
            }
            else if (score == 0.25m)
            {
                return "025";
            }
            else if (score == 1)
            {
                return "1";
            }
            else if (score == 0.50m)
            {
                return "05";
            }
            else if (score == 0.75m)
            {
                return "075";
            }
            return "";
        }

        //private void createBreadCrumb(long blockId, string blockName, long subblockId, string subblockName)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    if (blockId == 0 && subblockId == 0)
        //    {
        //        sb.AppendFormat("<li class=\"active\" style='color: #fff'>BLOCOS</li>");
        //    }
        //    else
        //    {
        //        //Não está na página de blocos
        //        sb.AppendFormat("<li><a href=\"#\" onclick=\"redirectToBlocks()\" style='color: #fff'>BLOCOS</a></li>");

        //        if (blockId > 0 && subblockId > 0)
        //        {
        //            //Está na página de questões
        //            sb.AppendFormat("<li><a href='#' onclick=\"redirectToBlock('{0}')\" style='color: #fff'>{1}</a></li>", blockId, blockName.ToUpper());
        //            sb.AppendFormat("<li class=\"active\" style='color: #fff'>{0}</li>", subblockName.ToUpper());
        //        }
        //        else if (blockId > 0)
        //        {
        //            //Está na página de subblocos
        //            sb.AppendFormat("<li class=\"active\" style='color: #fff'>{0}</li>", blockName.ToUpper());
        //        }
        //    }

        //    ltFormBreadCrumb.Text = sb.ToString();

        //    //<%--<li><a href="#">Home</a></li>
        //    //<li><a href="#" style="color:#fff">Library</a></li>
        //    //<li class="active">Data</li>--%>
        //}

        //private bool hasNextSubblock()
        //{
        //    if (String.IsNullOrWhiteSpace(hdCurrentBlockId.Value) && String.IsNullOrWhiteSpace(hdCurrentSubblockId.Value))
        //        return false;

        //    bool hasNextSubBlock = false;
        //    long currentBlockId = Convert.ToInt64(hdCurrentBlockId.Value);
        //    long currentSubBlockId = Convert.ToInt64(hdCurrentSubblockId.Value);

        //    Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];

        //    if (baseForm != null)
        //    {
        //        //Seleciona o bloco atual
        //        baseForm.BaseBlocks.ForEach(bb =>
        //        {
        //            var currentSubblockIndex = bb.BaseSubBlocks.FindIndex(bsb => bsb.Id == currentSubBlockId);

        //            if (currentSubblockIndex != -1)
        //            {
        //                if (bb.BaseSubBlocks.Count > (currentSubblockIndex + 1))
        //                {
        //                    hasNextSubBlock = true;
        //                }
        //            }
        //        });
        //    }

        //    return hasNextSubBlock;
        //}

        //private bool hasNextBlock()
        //{
        //    if (String.IsNullOrWhiteSpace(hdCurrentBlockId.Value) && String.IsNullOrWhiteSpace(hdCurrentSubblockId.Value))
        //        return false;

        //    bool hasNextBlock = false;
        //    long currentBlockId = Convert.ToInt64(hdCurrentBlockId.Value);
        //    long currentSubBlockId = Convert.ToInt64(hdCurrentSubblockId.Value);

        //    Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];

        //    if (baseForm != null)
        //    {
        //        //Seleciona o bloco atual
        //        var currentBlockIndex = baseForm.BaseBlocks.FindIndex(bb => bb.Id == currentBlockId);

        //        if (currentBlockIndex != -1)
        //        {
        //            if (baseForm.BaseBlocks.Count > (currentBlockIndex + 1))
        //            {
        //                hasNextBlock = true;
        //            }
        //        }
        //    }

        //    return hasNextBlock;
        //}

        //private bool hasPreviousSubblock()
        //{
        //    if (String.IsNullOrWhiteSpace(hdCurrentBlockId.Value) && String.IsNullOrWhiteSpace(hdCurrentSubblockId.Value))
        //        return false;

        //    bool hasPreviousSubBlock = false;
        //    long currentBlockId = Convert.ToInt64(hdCurrentBlockId.Value);
        //    long currentSubBlockId = Convert.ToInt64(hdCurrentSubblockId.Value);

        //    Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];

        //    if (baseForm != null)
        //    {
        //        //Seleciona o bloco atual
        //        baseForm.BaseBlocks.ForEach(bb =>
        //        {
        //            var currentSubblockIndex = bb.BaseSubBlocks.FindIndex(bsb => bsb.Id == currentSubBlockId);

        //            if (currentSubblockIndex != -1)
        //            {
        //                if (bb.BaseSubBlocks.Count > (currentSubblockIndex - 1) && (currentSubblockIndex - 1) >= 0)
        //                {
        //                    hasPreviousSubBlock = true;
        //                }
        //            }
        //        });
        //    }

        //    return hasPreviousSubBlock;
        //}

        //private bool hasPreviousBlock()
        //{
        //    if (String.IsNullOrWhiteSpace(hdCurrentBlockId.Value) && String.IsNullOrWhiteSpace(hdCurrentSubblockId.Value))
        //        return false;

        //    bool hasNextBlock = false;
        //    long currentBlockId = Convert.ToInt64(hdCurrentBlockId.Value);
        //    long currentSubBlockId = Convert.ToInt64(hdCurrentSubblockId.Value);

        //    Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];

        //    if (baseForm != null)
        //    {
        //        //Seleciona o bloco atual
        //        var currentBlockIndex = baseForm.BaseBlocks.FindIndex(bb => bb.Id == currentBlockId);

        //        if (currentBlockIndex != -1)
        //        {
        //            if (baseForm.BaseBlocks.Count > (currentBlockIndex - 1) && (currentBlockIndex - 1) >= 0)
        //            {
        //                hasNextBlock = true;
        //            }
        //        }
        //    }

        //    return hasNextBlock;
        //}

        private void saveForm()
        {
            try
            {
                var responseForm = getResponseFormUpdated();

                //Só pode atualizar se o questionário estiver NotApproved
                if (!responseForm.isApproved())
                {
                    using (var context = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                    {
                        responseForm.CityId = null;

                        if (!String.IsNullOrWhiteSpace(hdCityId.Value))
                            responseForm.CityId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(hdCityId.Value));

                        //Salva o questionário mais não submita para avaliação
                        context.save(responseForm, false);

                        Session[SESSION_CURRENT_RESPONSEFORM] = responseForm;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Web.CurrentForm.btnSaveForm_ServerClick", ex);
            }
        }

        //private void setColorInBlocks()
        //{
        //    List<string> colorList = new List<string>();

        //    if (Session[SESSION_CURRENT_RESPONSEFORM] != null && Session[SESSION_CURRENT_FORM] != null)
        //    {
        //        //Compara o formulário respondido e recupera o total de questões respondidas por blocos
        //        Lib.Entities.ResponseForm responseForm = (Lib.Entities.ResponseForm)Session[SESSION_CURRENT_RESPONSEFORM];
        //        Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];

        //        baseForm.BaseBlocks.ForEach(bb =>
        //        {
        //            var totalQuestionInBlock = 0;
        //            var totalQuestionAnswered = 0;
        //            bb.BaseSubBlocks.ForEach(bsb =>
        //            {
        //                totalQuestionInBlock += bsb.BaseQuestions.Count;
        //                totalQuestionAnswered += responseForm.Answers.Where(a => bsb.BaseQuestions.Select(bq => bq.Id).Contains(a.BaseQuestionId)).Count();
        //            });

        //            //Se o número de questões contidas no bloco for igual ao número de questões respondidas, o bloco ficará verde, caso contrário ficará vermelho
        //            if (totalQuestionInBlock == totalQuestionAnswered)
        //            {
        //                colorList.Add(greenColor);
        //            }
        //            else
        //            {
        //                colorList.Add(redColor);
        //            }
        //        });
        //    }
        //    else if (Session[SESSION_CURRENT_FORM] != null)
        //    {
        //        Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];
        //        //Recupera a quantidade de blocos para montar as cores

        //        for (int i = 0; i < baseForm.BaseBlocks.Count - 1; i++) { colorList.Add(redColor); }
        //    }


        //    blocks.Attributes["data-color"] = String.Join(",", colorList);
        //}

        //private void setColorInSubBlocks(long baseBlockId)
        //{
        //    List<string> colorList = new List<string>();

        //    if (Session[SESSION_CURRENT_RESPONSEFORM] != null && Session[SESSION_CURRENT_FORM] != null)
        //    {
        //        //Compara o formulário respondido e recupera o total de questões respondidas por blocos
        //        Lib.Entities.ResponseForm responseForm = (Lib.Entities.ResponseForm)Session[SESSION_CURRENT_RESPONSEFORM];
        //        Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];

        //        var selectedBlock = baseForm.BaseBlocks.Where(bb => bb.Id == baseBlockId).FirstOrDefault();


        //        selectedBlock.BaseSubBlocks.ForEach(bsb =>
        //        {
        //            var totalQuestionInSubBlock = 0;
        //            var totalQuestionAnswered = 0;

        //            totalQuestionInSubBlock += bsb.BaseQuestions.Count;
        //            totalQuestionAnswered += responseForm.Answers.Where(a => bsb.BaseQuestions.Select(bq => bq.Id).Contains(a.BaseQuestionId)).Count();

        //            //Se o número de questões contidas no subbloco for igual ao número de questões respondidas, o bloco ficará verde, caso contrário ficará vermelho
        //            if (totalQuestionInSubBlock == totalQuestionAnswered)
        //            {
        //                colorList.Add(greenColor);
        //            }
        //            else
        //            {
        //                colorList.Add(redColor);
        //            }
        //        });
        //    }
        //    else if (Session[SESSION_CURRENT_FORM] != null)
        //    {
        //        Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];
        //        //Recupera a quantidade de subblocos que o bloco selecionado contem para montar as cores

        //        var selectedBlock = baseForm.BaseBlocks.Where(bb => bb.Id == baseBlockId).FirstOrDefault();

        //        if (selectedBlock != null)
        //        {
        //            for (int i = 0; i < selectedBlock.BaseSubBlocks.Count - 1; i++) { colorList.Add(redColor); }
        //        }
        //    }


        //    subblocks.Attributes["data-color"] = String.Join(",", colorList);

        //}

        //private bool formIsFinished()
        //{
        //    bool isFinished = true;

        //    if (Session[SESSION_CURRENT_RESPONSEFORM] != null && Session[SESSION_CURRENT_FORM] != null)
        //    {
        //        //Compara o formulário respondido e recupera o total de questões respondidas por blocos
        //        Lib.Entities.ResponseForm responseForm = (Lib.Entities.ResponseForm)Session[SESSION_CURRENT_RESPONSEFORM];
        //        Lib.Entities.BaseForm baseForm = (Lib.Entities.BaseForm)Session[SESSION_CURRENT_FORM];

        //        baseForm.BaseBlocks.ForEach(bb =>
        //        {
        //            var totalQuestionInBlock = 0;
        //            var totalQuestionAnswered = 0;
        //            bb.BaseSubBlocks.ForEach(bsb =>
        //            {
        //                totalQuestionInBlock += bsb.BaseQuestions.Count;
        //                totalQuestionAnswered += responseForm.Answers.Where(a => bsb.BaseQuestions.Select(bq => bq.Id).Contains(a.BaseQuestionId)).Count();
        //            });

        //            //Se um determinado bloco tiver o total diferente de respondido o questionário não foi concluido.
        //            if (totalQuestionInBlock != totalQuestionAnswered)
        //            {
        //                isFinished = false;
        //            }
        //        });
        //    }
        //    else if (Session[SESSION_CURRENT_FORM] != null)
        //    {
        //        //Não existe um questionário de resposta
        //        isFinished = false;
        //    }

        //    return isFinished;
        //}

        private bool formIsApproved()
        {
            if (Session[SESSION_CURRENT_RESPONSEFORM] != null)
            {
                //Compara o formulário respondido e recupera o total de questões respondidas por blocos
                Lib.Entities.ResponseForm responseForm = (Lib.Entities.ResponseForm)Session[SESSION_CURRENT_RESPONSEFORM];

                return responseForm.isApproved();
            }

            //Não existe um questionário de resposta
            return false;
        }

        private bool formIsSubmitted()
        {
            if (Session[SESSION_CURRENT_RESPONSEFORM] != null)
            {
                //Compara o formulário respondido e recupera o total de questões respondidas por blocos
                Lib.Entities.ResponseForm responseForm = (Lib.Entities.ResponseForm)Session[SESSION_CURRENT_RESPONSEFORM];

                return responseForm.isSubmitted();
            }

            //Não existe um questionário de resposta
            return false;
        }

        //#endregion

        #region [Redirects]

        private void redirectToForm()
        {
            Response.Redirect("~/User/CurrentForm.aspx?p=" + HttpUtility.UrlEncode(hdPeriodId.Value));
        }

        private void redirectToBlock(long blockId)
        {
            Response.Redirect("~/User/CurrentForm.aspx?p=" + HttpUtility.UrlEncode(hdPeriodId.Value) + "&bid=" + blockId);
        }

        private void redirectToSubBlock(long subblockId)
        {
            Response.Redirect("~/User/CurrentForm.aspx?p=" + HttpUtility.UrlEncode(hdPeriodId.Value) + "&sbid=" + subblockId);
        }

        private void redirectToFormDetails(long formId)
        {
            //var id = HttpUtility.UrlEncode(Commons.SecurityUtils.criptografar(formId.ToString()));
            Response.Redirect("~/Form/View.aspx?rfid=" + Commons.SecurityUtils.criptografar(formId.ToString()));
        }
        #endregion

        protected void btnFormDetails_ServerClick(object sender, EventArgs e)
        {
            long formId = 0;

            if (long.TryParse(Commons.SecurityUtils.descriptografar(hdResponseFormId.Value), out formId))
            {
                redirectToFormDetails(formId);
            }
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            saveForm();
            saveMessage.Visible = true;
            verifyFormStatus();
        }
    }


}