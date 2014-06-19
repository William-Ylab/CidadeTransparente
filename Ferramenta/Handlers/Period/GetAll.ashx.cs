using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Ferramenta.App_Code;

namespace Ferramenta.Handlers.Period
{
    /// <summary>
    /// Summary description for GetAll
    /// </summary>
    public class GetAll : BaseHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                if (!userAuthenticate())
                {
                    throw new Exception(Resources.Message.user_not_allowed);
                }

                List<Lib.Entities.Period> periods = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                var initialDate = String.IsNullOrEmpty(context.Request.Form["id"]) ? DateTime.MinValue : DateTime.Parse(context.Request.Form["id"], new System.Globalization.CultureInfo("pt-BR"));
                var finalDate = String.IsNullOrEmpty(context.Request.Form["fd"]) ? DateTime.MaxValue : DateTime.Parse(context.Request.Form["fd"], new System.Globalization.CultureInfo("pt-BR"));
                var t = context.Request.Form["ut"].Split(",".ToCharArray());

                bool open = t.Contains("open") ? true : false;
                bool closed = t.Contains("closed") ? true : false;

                using (Lib.Repositories.PeriodRepository rep = new Lib.Repositories.PeriodRepository(this.ActiveUser))
                {
                    periods = rep.selectWhere(f => f.InitialDate >= initialDate && f.FinalDate <= finalDate);
                }

                if (!open)
                {
                    periods = periods.Where(f => f.Open == false).ToList();
                }

                if (!closed)
                {
                    periods = periods.Where(f => f.Open == true).ToList();
                }

                var result = periods.OrderBy(p=>p.FinalDate).Select(period => new
                {
                    Id = Commons.SecurityUtils.criptografar(period.Id.ToString()),
                    IdEncoded = HttpUtility.UrlEncode(Commons.SecurityUtils.criptografar(period.Id.ToString())),
                    Name = period.Name,
                    FinalDate = period.FinalDate.ToString("dd/MM/yyyy"),
                    InitialDate = period.InitialDate.ToString("dd/MM/yyyy"),
                    Open = period.Open,
                    Published = period.Published,
                    CanPublish = period.IsThereOneOrMoreResponseFormAccepted,
                    ActiveUserType = this.ActiveUser.UserTypeEnum.ToString(),
                    InitialConvocationDate = period.ConvocationInitialDate.ToString("dd/MM/yyyy"),
                    FinalConvocationDate = period.ConvocationFinalDate.ToString("dd/MM/yyyy")
                }).ToList();

                context.Response.ContentType = "application/json";
                context.Response.Write(serializer.Serialize(result));
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Web.Handler.User.GetAll.ProcessRequest", ex);
                context.Response.StatusCode = 500;
                context.Response.Write(String.Format(Resources.Message.unknow_error, ex.Message));
            }

        }
    }
}