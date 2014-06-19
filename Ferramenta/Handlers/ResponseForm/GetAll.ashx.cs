using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Ferramenta.Handlers.ResponseForm
{
    /// <summary>
    /// Summary description for GetAll
    /// </summary>
    public class GetAll : App_Code.BaseHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            List<Lib.Entities.ResponseForm> responseForms = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            long periodId = 0;

            if (!String.IsNullOrEmpty(context.Request.Form["periodId"]))
            {
                var periodIdEncoded = context.Request.Form["periodId"];
                var periodIdString = Commons.SecurityUtils.descriptografar(periodIdEncoded);
                long.TryParse(periodIdString, out periodId);
            }

            using (Lib.Repositories.ResponseFormRepository rep = new Lib.Repositories.ResponseFormRepository(this.ActiveUser))
            {
                if (periodId > 0)
                {
                    responseForms = rep.getResponseFormsByPeriod(periodId);
                }
                else
                {
                    responseForms = rep.getActualOrLastPeriodResponseForms();
                }
            }

            if (responseForms != null)
            {
                //Adicionado tratamento para retornar todos os questionários de entidades e que possuem o status de submissão
                var result = responseForms.Where(f => f.User.UserTypeEnum == Lib.Enumerations.UserType.Entity && f.Submits != null && f.Submits.Count > 0).Select(rf => new
                {
                    Id = rf.Id,
                    UserName = rf.User.Name,
                    UserId = HttpUtility.UrlEncode(Commons.SecurityUtils.criptografar(rf.User.Id.ToString())),
                    UserType = Lib.Enumerations.EnumManager.getStringFromUserType(rf.User.UserTypeEnum, rf.User.Network),
                    ActiveUserType = this.ActiveUser.UserTypeEnum.ToString(),
                    SubmitStatus = Lib.Enumerations.EnumManager.getStringFromSubmitType(rf.Submits.OrderBy(f => f.Id).LastOrDefault().StatusEnum),
                    SubmitStatusEnum = rf.Submits.OrderBy(f => f.Id).LastOrDefault().StatusEnum.ToString(),
                    SubmitId = rf.Submits.OrderBy(f => f.Id).LastOrDefault().Id.ToString(),
                    UserAlreadyReview = rf.Reviews != null && rf.Reviews.Where(re => re.UserId == this.ActiveUser.Id).FirstOrDefault() != null ? true : false,
                    UserIsMaster = this.ActiveUser.UserTypeEnum == Lib.Enumerations.UserType.Master,
                    Reviews = rf.Reviews.Select(r => new
                    {
                        Id = r.Id,
                        Accepted = r.Accepted,
                        UserName = r.User.Name,
                    }),
                    PositiveReviews = rf.Reviews.Where(f => f.Accepted).Select(r => new
                    {
                        Id = r.Id,
                        Accepted = r.Accepted,
                        UserName = r.User.Name
                    }),
                    NegativeReviews = rf.Reviews.Where(f => f.Accepted == false).Select(r => new
                    {
                        Id = r.Id,
                        Accepted = r.Accepted,
                        UserName = r.User.Name
                    })
                }).ToList();


                context.Response.ContentType = "application/json";
                context.Response.Write(serializer.Serialize(result));
            }
            else
            {
                context.Response.Write("");
            }
        }
    }
}