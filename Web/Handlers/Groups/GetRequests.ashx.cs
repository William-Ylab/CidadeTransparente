using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Site.Handlers.Groups
{
    /// <summary>
    /// Summary description for GetRequests
    /// </summary>
    public class GetRequests : App_Code.BaseHttpHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var sPeriodId= context.Request.Form["periodId"];

            long periodId = 0;
            long.TryParse(sPeriodId, out periodId);

            using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(this.ActiveUser))
            {
                var cities = repository.citiesRequests(this.ActiveUser.Id, periodId);

                var ret = cities.Select(f => new
                {
                    CityName = f.City.Name,
                    StateName = f.City.State.Name,
                    RequestStatus = Lib.Enumerations.EnumManager.getStringFromRequestStatus(f.RequestStatusEnum)
                }).ToList();

                context.Response.ContentType = "application/json";
                context.Response.Write(serializer.Serialize(ret));

            }
        }
    }
}