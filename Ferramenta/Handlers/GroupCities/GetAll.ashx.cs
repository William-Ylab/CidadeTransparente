using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Ferramenta.App_Code;

namespace Ferramenta.Handlers.GroupCities
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
                if (!String.IsNullOrWhiteSpace(context.Request.Form["state"]) && !String.IsNullOrWhiteSpace(context.Request.Form["period"]))
                {
                    var state = context.Request.Form["state"].ToString();
                    var period = context.Request.Form["period"].ToString();

                    long periodId = 0;

                    long.TryParse(Commons.SecurityUtils.descriptografar(period), out periodId);

                    if (periodId > 0)
                    {

                        List<Lib.Entities.City> cities = null;
                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        using (Lib.Repositories.StateCityRepository rep = new Lib.Repositories.StateCityRepository(this.ActiveUser))
                        {

                            cities = rep.getCitiesByUF(state, periodId);
                        }

                        if (cities != null)
                        {
                            var result = cities.Select(city => new
                            {
                                Id = Commons.SecurityUtils.criptografar(city.Id.ToString()),
                                Name = city.Name,
                                StateName = city.State.Name,
                                ResponsableName = city.Groups.Count == 1 ? city.Groups[0].Responsable != null ? city.Groups[0].Responsable.Name :"" : "",
                                TotalCollaborators = city.Groups.Count == 1 ? city.Groups[0].Collaborators.Count : 0
                            }).ToList();

                            context.Response.ContentType = "application/json";
                            context.Response.Write(serializer.Serialize(result));
                        }
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.Write("[]");
                    }
                }
                else
                {
                    context.Response.ContentType = "application/json";
                    context.Response.Write("[]");
                }

            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Web.Handler.GroupCities.GetAll.ProcessRequest", ex);
                context.Response.StatusCode = 500;
                context.Response.Write(String.Format(Resources.Message.unknow_error, ex.Message));
            }

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}