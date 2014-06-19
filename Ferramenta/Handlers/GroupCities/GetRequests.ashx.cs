using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Ferramenta.App_Code;

namespace Ferramenta.Handlers.GroupCities
{
    /// <summary>
    /// Summary description for GetRequests
    /// </summary>
    public class GetRequests : BaseHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var sPeriodId = context.Request.Form["pId"];
                var sCityId = context.Request.Form["cId"];

                long periodId = 0;
                long.TryParse(Commons.SecurityUtils.descriptografar(sPeriodId), out periodId);

                long cityId = 0;
                
                if (!String.IsNullOrWhiteSpace(sCityId))
                    long.TryParse(Commons.SecurityUtils.descriptografar(sCityId), out cityId);

                using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    var requests = cityId > 0 ? repository.getAllRequestsFromCity(cityId, periodId) : repository.getAllRequestsByPeriod(periodId);

                    var ret = requests.Select(f => new
                    {
                        Id = Commons.SecurityUtils.criptografar(f.Id.ToString()),
                        EntityName = f.User.Name,
                        EntityId = HttpUtility.UrlEncode( Commons.SecurityUtils.criptografar(f.UserId.ToString())),
                        RequestDate = f.RequestDate.ToString("dd/MM/yyyy"),
                        RequestType = f.RequestType,
                        RequestStatus = f.Status,
                        CityName = f.City.Name,
                        CityId = HttpUtility.UrlEncode( Commons.SecurityUtils.criptografar(f.CityId.ToString()))
                    }).ToList();

                    context.Response.ContentType = "application/json";
                    context.Response.Write(serializer.Serialize(ret));

                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Web.Handler.GroupCities.GetRequests.ProcessRequest", ex);
                context.Response.StatusCode = 500;
                context.Response.Write(String.Format(Resources.Message.unknow_error, ex.Message));
            }

        }
    }
}