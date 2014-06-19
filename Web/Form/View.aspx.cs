using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.Form
{
    public partial class View : App_Code.BasePage
    {
        Lib.Entities.ResponseForm form;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["rfid"]))
                {
                    hdnResponseId.Value = Commons.SecurityUtils.descriptografar(Request.QueryString["rfid"]);

                    var rep = loadRepeater();

                    if (rep.City != null)
                    {
                        lblTitle.Text = String.Format("{0} ({1}/{2}) - {3} - {4}", Resources.Message.view_questions, rep.City.Name, rep.City.State.Name, rep.TotalScore, rep.User.Name);
                    }
                    else
                    {
                        lblTitle.Text = String.Format("{0} - {1} - {2}", Resources.Message.view_questions, rep.TotalScore, rep.User.Name);
                    }
                }
            }
        }

        private Lib.Entities.ResponseForm loadRepeater()
        {
            using (Lib.Repositories.ResponseFormRepository repo = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
            {
                Lib.Entities.ResponseForm form = repo.getInstanceById(long.Parse(hdnResponseId.Value));

                litTracking.Text = form.TrackingNote.Replace("\r\n", "<br/>");

                this.form = form;

                //Altera o indice para mostrar incremental, conforme solicitado em FDT-69
                int count = 1;
                if (this.form.BaseForm.BaseBlocks != null)
                {
                    this.form.BaseForm.BaseBlocks.ForEach(bb =>
                        {
                            if (bb.BaseSubBlocks != null)
                            {
                                bb.BaseSubBlocks.ForEach(bsb =>
                                {
                                    if (bsb.BaseQuestions != null)
                                    {
                                        bsb.BaseQuestions.ForEach(bq =>
                                        {
                                            bq.Index = count;
                                            count++;
                                        });
                                    }
                                });
                            }
                        });
                }

                //Informa os  colaboradores e o responsável
                using (Lib.Repositories.UserRepository ctx = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    if (this.form.CityId.HasValue && this.form.BaseForm != null)
                    {
                        phRespCollab.Visible = true;
                        var responsable = ctx.getResponsable(this.form.CityId.Value, this.form.BaseForm.PeriodId);
                        var collaborators = ctx.getCollaborators(this.form.CityId.Value, this.form.BaseForm.PeriodId);

                        if (responsable != null)
                            lblResponsable.Text = responsable.Name;

                        if (collaborators != null && collaborators.Count > 0)
                            lblCollaborator.Text = String.Join(",", collaborators.Select(c => c.Name).ToList());
                    }
                }

                rptBlocks.DataSource = this.form.BaseForm.BaseBlocks;
                rptBlocks.DataBind();

                return form;
            }
        }

        public string getScore(string score)
        {
            if (!String.IsNullOrEmpty(score))
            {
                return score;
            }
            else
            {
                return "N/A";
            }
        }

        #region [Repeaters Databound]

        protected void rptBlocks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var block = (Lib.Entities.BaseBlock)e.Item.DataItem;

            Repeater rptSubBlocks = (Repeater)e.Item.FindControl("rptSubBlocks");

            if (rptSubBlocks != null && block != null)
            {
                rptSubBlocks.DataSource = block.BaseSubBlocks;
                rptSubBlocks.DataBind();
            }
        }

        protected void rptSubBlocks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var subblock = (Lib.Entities.BaseSubBlock)e.Item.DataItem;
            Repeater rptQuestions = (Repeater)e.Item.FindControl("rptQuestions");

            if (rptQuestions != null && subblock != null)
            {
                List<Lib.Entities.Answer> answers = new List<Lib.Entities.Answer>();
                subblock.BaseQuestions.ForEach(a =>
                {
                    answers.AddRange(a.Answers.Where(f => f.ResponseFormId == long.Parse(hdnResponseId.Value)).ToList());
                });

                rptQuestions.DataSource = answers;
                rptQuestions.DataBind();
            }
        }

        #endregion
    }
}