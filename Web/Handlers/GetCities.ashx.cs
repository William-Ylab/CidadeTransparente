using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Site.Handlers
{
    /// <summary>
    /// Summary description for GetCities
    /// </summary>
    public class GetCities : App_Code.BaseHttpHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string stateId = context.Request.Form["sid"];

            if (context.Request.Form["type"] != "all" && (this.ActiveUser.Groups != null && this.ActiveUser.Groups.Count > 0))
            {
                var groups = this.ActiveUser.Groups.Where(f => f.City.StateId == stateId).ToList();

                var cities = groups.Select(f => new
                {
                    CityName = f.City.Name,
                    CityId = f.CityId,
                    StateId = f.City.StateId
                }).ToList();

                context.Response.ContentType = "application/json";
                context.Response.Write(serializer.Serialize(cities));
            }
            else
            {
                using (Lib.Repositories.StateCityRepository repo = new Lib.Repositories.StateCityRepository(this.ActiveUser))
                {
                    var cities = repo.getCitiesByUF(stateId).Select(f => new
                    {
                        CityName = f.Name,
                        CityId = f.Id,
                        StateId = f.StateId
                    });

                    context.Response.ContentType = "application/json";
                    context.Response.Write(serializer.Serialize(cities));
                }
            }
        }
    }
}