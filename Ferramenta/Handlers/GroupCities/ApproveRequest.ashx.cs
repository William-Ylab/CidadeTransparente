using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Ferramenta.App_Code;

namespace Ferramenta.Handlers.GroupCities
{
    /// <summary>
    /// Summary description for ApproveRequest
    /// </summary>
    public class ApproveRequest : BaseHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var sRequestId = context.Request.Form["rId"];

                long requestId = 0;
                long.TryParse(Commons.SecurityUtils.descriptografar(sRequestId), out requestId);

                using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    repository.approveRequest(requestId);

                    context.Response.ContentType = "text/plain";
                    if (!repository.HasErrors)
                    {
                        context.Response.Write("ok");
                    }
                    else
                    {
                        context.Response.Write("nok");
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Web.Handler.GroupCities.ApproveRequest.ProcessRequest", ex);
                context.Response.StatusCode = 500;
                context.Response.Write(String.Format(Resources.Message.unknow_error, ex.Message));
            }

        }
    }
}