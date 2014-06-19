using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class Home : App_Code.BasePage
    {
        private string filePath = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadRanking();
                loadStates();
                loadEntities();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Lib.Entities.BaseForm baseForm = null;
            string excelname = "file.xlsx";

            using (Lib.Repositories.BaseFormRepository repo = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
            {
                baseForm = repo.getInstanceByPeriodDate(DateTime.Now);
            }

            if (baseForm != null)
            {
                excelname = Commons.StringUtils.removeAccents(baseForm.Name) + ".xlsx";

                filePath = Server.MapPath("/App_Data/" + excelname);
                Lib.Utils.ExcelUtils utils = new Lib.Utils.ExcelUtils();

                bool save = utils.createExcel(filePath, true);

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

        protected void btnImport_Click(object sender, EventArgs e)
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

                            url = String.Format("~/Form/View.aspx?rfid={0}", id.ToString());
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

            loadRanking();

            if (!string.IsNullOrEmpty(url))
            {
                Response.Redirect(url);
            }
        }

        protected bool canShowSubmitButton(DataTableCollection tables, out List<string> notAnsweredQuestions, out List<string> incorrectAnsweredQuestions)
        {
            notAnsweredQuestions = new List<string>();
            incorrectAnsweredQuestions = new List<string>();

            List<String> AcceptedAnswers = new List<string>();
            AcceptedAnswers.Add("n/a");
            AcceptedAnswers.Add("0");
            AcceptedAnswers.Add("0.5");
            AcceptedAnswers.Add("0,5");
            AcceptedAnswers.Add("1");
            AcceptedAnswers.Add("1.0");
            AcceptedAnswers.Add("1,0");

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
            form.UserId = int.Parse(ddlEntidades.SelectedValue);
            form.CityId = int.Parse(hdnCityId.Value);

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
                rep.save(form);
            }

            return form.Id;
        }

        private void loadRanking()
        {
            using (Lib.Repositories.ResponseFormRepository repo = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
            {
                var responsesForms = repo.getAll();

                var listObjForm = responsesForms.Select(f => new
                {
                    CityName = f.City.Name + " (" + f.City.StateId + ")",
                    FormName = f.BaseForm.Name,
                    Id = f.Id,
                    TotalScore = f.TotalScore,
                    UserName = f.User.Name
                });

                gvFormsRanking.DataSource = listObjForm.OrderByDescending(f => f.TotalScore).ToList();
                gvFormsRanking.DataBind();
            }
        }

        protected void gvFormsRanking_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewForm")
            {
                Response.Redirect(String.Format("~/Form/View.aspx?rfid={0}", e.CommandArgument));
            }
        }

        private void loadStates()
        {
            using (Lib.Repositories.StateCityRepository repository = new Lib.Repositories.StateCityRepository(this.ActiveUser))
            {
                //ddlState
                foreach (var state in repository.getAllStates())
                {
                    ddlState.Items.Add(new ListItem(state.Name, state.Id.ToString()));
                }

                var cities = repository.getCitiesByUF(ddlState.SelectedValue);

                hdnCityId.Value = cities[0].Id.ToString();

                foreach (var city in cities)
                {
                    ddlCities.Items.Add(new ListItem(city.Name, city.Id.ToString()));
                }
            }
        }

        private void loadEntities()
        {
            using (Lib.Repositories.UserRepository repo = new Lib.Repositories.UserRepository(this.ActiveUser))
            {
                foreach(var item in repo.selectWhere(u => u.UserType == Lib.Enumerations.UserType.Entity && u.Entity.Network == true))
                {
                    ddlEntidades.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                }
            }
        }
    }
}