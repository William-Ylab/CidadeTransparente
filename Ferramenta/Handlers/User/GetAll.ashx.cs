using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Ferramenta.App_Code;

namespace Ferramenta.Handlers.User
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
                List<Lib.Entities.User> users = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                using (Lib.Repositories.UserRepository rep = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    List<int> list = null;
                    if (!String.IsNullOrWhiteSpace(context.Request.Form["ut"]))
                    {
                        string[] filter = context.Request.Form["ut"].Split(',');
                        list = getUserTypes(filter);
                    }

                    bool? status = null;

                    if (!String.IsNullOrWhiteSpace(context.Request.Form["st"]))
                    {
                        var qStatus = context.Request.Form["st"].ToLower();

                        if (qStatus == "active")
                            status = true;
                        else if (qStatus == "inactive")
                            status = false;
                    }

                    users = rep.search(list, status);

                }

                if (this.ActiveUser.UserTypeEnum != Lib.Enumerations.UserType.Master)
                {
                    users = users.Where(f => f.UserTypeEnum != Lib.Enumerations.UserType.Master).ToList();
                }

                var result = users.Select(user => new
                {
                    Id = Commons.SecurityUtils.criptografar(user.Id.ToString()),
                    Name = user.Name,
                    Email = user.Email,
                    Type = Lib.Enumerations.EnumManager.getStringFromUserType(user.UserTypeEnum, user.Network),
                    AcceptedTerms = user.UserTypeEnum == Lib.Enumerations.UserType.Entity ? (user.TermsOfUse ? "1" : "0") : "-1",
                    Approved = user.Network ? (user.NetworkApproved ? "1" : "0") : "-1",
                    Nature = user.Nature,
                    Status = user.Active,
                    User = user.Login
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

        private List<int> getUserTypes(string[] filter)
        {
            List<int> list = new List<int>();

            foreach (var f in filter)
            {
                switch (f.ToLower())
                {
                    case "admin":
                        list.Add((int)Lib.Enumerations.UserType.Admin);
                        break;
                    case "master":
                        list.Add((int)Lib.Enumerations.UserType.Master);
                        break;
                    case "entity":
                        list.Add((int)Lib.Enumerations.UserType.Entity);
                        break;
                    case "common":
                        list.Add((int)Lib.Enumerations.UserType.Others);
                        break;
                    default:
                        break;
                }
            }
            if (list.Count > 0)
                return list;
            else
                return list;
        }
    }
}