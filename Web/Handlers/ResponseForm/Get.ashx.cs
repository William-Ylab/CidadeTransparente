using Site.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Site.Handlers.ResponseForm
{
    /// <summary>
    /// Summary description for Get
    /// </summary>
    public class Get : BaseHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Lib.Entities.ResponseForm> responseForms = null;

                var periodIdString = context.Request.Form["periodId"];
                var checks = context.Request.Form["chbs"].Split(",".ToCharArray());


                var accepted = false;
                var submitted = false;
                var completed = false;
                var incompleted = false;

                //accepted,submitted,completed,incompleted
                if (checks.Contains("accepted")) accepted = true;
                if (checks.Contains("submitted")) submitted = true;
                if (checks.Contains("completed")) completed = true;
                if (checks.Contains("incompleted")) incompleted = true;


                long periodId = 0;
                long.TryParse(periodIdString, out periodId);

                using (Lib.Repositories.ResponseFormRepository repository = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
                {
                    responseForms = repository.getResponseFormsByUserId(this.ActiveUser.Id);

                    if (periodId > 0)
                    {
                        responseForms = responseForms.Where(f => f.BaseForm.PeriodId == periodId).ToList();
                    }

                    if (responseForms != null)
                    {
                        var result = responseForms.Select(rf => new
                        {
                            Id = rf.Id,
                            IdCrypt = Commons.SecurityUtils.criptografar(rf.Id.ToString()),
                            CityState = rf.City != null ? String.Format("{0} - {1}", rf.City.Name, rf.City.StateId) : "N/I",
                            State = rf.City != null ? rf.City.StateId : "N/I",
                            ResponsableUser = rf.User != null ? rf.User.Name : "",
                            BaseFormName = rf.BaseForm.Name,
                            PeriodName = rf.BaseForm.Period.Name,
                            TotalAnswers = rf.Answers.Count,
                            TotalQuestions = rf.getTotalQuestions(),
                            StatusSubmit = rf.Submits != null && rf.Submits.Count > 0 ? Lib.Enumerations.EnumManager.getStringFromSubmitType(rf.Submits.OrderBy(f => f.Id).LastOrDefault().StatusEnum) : "Em andamento" 
                        }).ToList();


                        if (!completed)
                        {
                            result = result.Where(f => f.TotalAnswers != f.TotalQuestions).ToList();
                        }

                        if (!incompleted)
                        {
                            result = result.Where(f => f.TotalAnswers == f.TotalQuestions).ToList();
                        }

                        if (this.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Entity)
                        {
                            if (this.ActiveUser.Groups != null && this.ActiveUser.Groups.Count > 0)
                            {
                                if (!accepted)
                                {
                                    result = result.Where(s => s.StatusSubmit != "Approved").ToList();
                                }

                                if (!submitted)
                                {
                                    result = result.Where(s => s.StatusSubmit != "Submitted").ToList();
                                }
                            }
                        }

                        context.Response.Write(serializer.Serialize(result));
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Site.Handlers.ResponseForm.ProcessRequest", ex);
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
            }
        }
    }
}