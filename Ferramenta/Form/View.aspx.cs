using Ferramenta.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ferramenta.Form
{
    public partial class View : BasePage
    {
        Lib.Entities.ResponseForm form;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["rfid"]))
                {
                    hdnResponseId.Value = Request.QueryString["rfid"];

                    var rep = loadRepeater();

                    if (this.form.Reviews != null && this.form.Reviews.Count > 0)
                    {
                        var Review = this.form.Reviews.Where(f => f.UserId == this.ActiveUser.Id).FirstOrDefault();

                        if (Review != null)
                        {
                            lblSubmit.Text = Resources.Message.you_already_add_a_review;
                            pnlPublishBottom.Visible = false;
                            pnlPublishTop.Visible = false;
                        }
                    }

                    if (this.form.Submits != null && this.form.Submits.Count > 0)
                    {
                        var submit = this.form.Submits.OrderBy(f => f.Id).LastOrDefault();

                        if (submit.StatusEnum == Lib.Enumerations.SubmitStatus.Approved)
                        {
                            lblSubmit.Text = Resources.Message.this_form_has_been_approved_to_published;
                            pnlPublishBottom.Visible = false;
                            pnlPublishTop.Visible = false;
                        }
                    }

                    updateInformationOnMasterPage(Resources.Message.view_questions, "fa fa-share", "");

                    litTitle.Text = String.Format("{0}/{1} - {2} - {3}", rep.City.Name, rep.City.State.Name, rep.TotalScore, rep.User.Name);
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

                rptBlocks.DataSource = this.form.BaseForm.BaseBlocks;
                rptBlocks.DataBind();

                rptReviews.DataSource = this.form.Reviews;
                rptReviews.DataBind();

                rptSubmits.DataSource = this.form.Submits.OrderByDescending(f => f.Date) ;
                rptSubmits.DataBind();

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