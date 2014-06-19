using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ferramenta.Handlers.Period
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
                var periodIdString = context.Request.Form["Id"];
                long periodId = 0;
                long.TryParse(Commons.SecurityUtils.descriptografar(periodIdString), out periodId);

                if (periodId > 0)
                {
                    using (Lib.Repositories.PeriodRepository repository = new Lib.Repositories.PeriodRepository(this.ActiveUser))
                    {
                        var period = repository.getInstanceById(periodId);

                        if (period != null)
                        {
                            period.Published = true;
                            period.Open = false;

                            repository.save(period);
                        }
                        else
                        {
                            throw new Exception(Resources.Message.period_not_found);
                        }
                    }
                }
                else
                {
                    throw new Exception(Resources.Message.period_id_not_found);
                }

                context.Response.ContentType = "text/plain";
                context.Response.Write("Ok");
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Web.Handler.ResponseForm.Action.ProcessRequest", ex);
                context.Response.StatusCode = 500;
                context.Response.Write(Resources.Message.couldnt_process_request);
            }
        }
    }
}