using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ferramenta.Handlers.ResponseForm
{
    /// <summary>
    /// Summary description for Action
    /// </summary>
    public class Action : App_Code.BaseHttpHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                var action = context.Request.Form["action"];

                if (action == "send_observation")
                {
                    using (Lib.Repositories.ResponseFormRepository rep = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                    {
                        Lib.Entities.Review r = new Lib.Entities.Review();
                        r.Accepted = bool.Parse(context.Request.Form["accepted"]);
                        r.Observations = context.Request.Form["observations"];
                        r.ResponseFormId = long.Parse(context.Request.Form["rfId"]);
                        r.UserId = this.ActiveUser.Id;

                        //Se for o adm master e caso tenha reprovado, muda o status da submissão apenas
                        if (this.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Master)
                        {
                            if (!r.Accepted)
                            {
                                //Não reprovado, muda apenas o status de submissao
                                changeStatus(Lib.Enumerations.SubmitStatus.NotApproved, r.ResponseFormId, r.Observations);
                            }
                            else
                            {
                                rep.addReview(r);
                            }
                        }
                        else
                        {
                            rep.addReview(r);
                        }
                    }

                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Ok");
                }
                else if (action == "change_submit")
                {
                    var rfId = long.Parse(context.Request.Form["rfId"]);
                    var status = bool.Parse(context.Request.Form["accepted"]) == true ? Lib.Enumerations.SubmitStatus.Approved : Lib.Enumerations.SubmitStatus.NotApproved;

                    changeStatus(status, rfId, context.Request.Form["observations"]);

                    //using (Lib.Repositories.SubmitRepository rep = new Lib.Repositories.SubmitRepository(this.ActiveUser))
                    //{
                    //    Lib.Entities.Submit submit = new Lib.Entities.Submit();

                    //    if (submit != null)
                    //    {
                    //        if (bool.Parse(context.Request.Form["accepted"]))
                    //        {
                    //            submit.Status = (int)Lib.Enumerations.SubmitStatus.Approved;
                    //        }
                    //        else
                    //        {
                    //            submit.Status = (int)Lib.Enumerations.SubmitStatus.Submitted;
                    //        }

                    //        submit.ResponseFormId = long.Parse(context.Request.Form["rfId"]);
                    //        if (String.IsNullOrEmpty(context.Request.Form["observations"]))
                    //        {
                    //            submit.Observation = "";
                    //        }
                    //        else
                    //        {
                    //            submit.Observation = context.Request.Form["observations"];
                    //        }

                    //        rep.save(submit);
                    //    }
                    //}

                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Ok");
                }
                else if (action == "publish_all")
                {
                    List<long> ids = new List<long>();
                    var responsesIds = context.Request.Form["rfId"].Split(",".ToCharArray());
                    foreach (var item in responsesIds)
                    {
                        ids.Add(long.Parse(item));
                    }

                    using (Lib.Repositories.ResponseFormRepository rep = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                    {
                        using (Lib.Repositories.SubmitRepository repository = new Lib.Repositories.SubmitRepository(this.ActiveUser))
                        {
                            var responseFormList = rep.selectWhere(f => ids.Contains(f.Id)).ToList();

                            foreach (var item in responseFormList)
                            {
                                bool criarSubmit = false;

                                if (item.Submits != null && item.Submits.Count > 0)
                                {
                                    var ultimoSubmit = item.Submits.OrderBy(f => f.Id).LastOrDefault();

                                    if (ultimoSubmit.StatusEnum != Lib.Enumerations.SubmitStatus.Approved)
                                    {
                                        criarSubmit = true;
                                    }
                                }
                                else
                                {
                                    criarSubmit = true;
                                }

                                if (criarSubmit)
                                {

                                    Lib.Entities.Submit submit = new Lib.Entities.Submit();

                                    if (submit != null)
                                    {
                                        if (bool.Parse(context.Request.Form["accepted"]))
                                        {
                                            submit.Status = (int)Lib.Enumerations.SubmitStatus.Approved;
                                        }
                                        else
                                        {
                                            submit.Status = (int)Lib.Enumerations.SubmitStatus.Submitted;
                                        }

                                        submit.ResponseFormId = item.Id;
                                        submit.Observation = "";

                                        repository.save(submit);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Web.Handler.ResponseForm.Action.ProcessRequest", ex);
                context.Response.StatusCode = 500;
                context.Response.Write(Resources.Message.couldnt_process_request);
            }
        }


        public void changeStatus(Lib.Enumerations.SubmitStatus status, long responseFormId, string observations)
        {
            if (responseFormId > 0)
            {
                using (Lib.Repositories.SubmitRepository rep = new Lib.Repositories.SubmitRepository(this.ActiveUser))
                {
                    Lib.Entities.Submit submit = new Lib.Entities.Submit();

                    submit.ResponseFormId = responseFormId;
                    submit.Status = (int)status;
                    if (String.IsNullOrEmpty(observations))
                        submit.Observation = "";
                    else
                        submit.Observation = observations;

                    rep.save(submit);
                }
            }
        }
    }


}