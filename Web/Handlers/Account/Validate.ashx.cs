using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Handlers.Account
{
    /// <summary>
    /// Summary description for Validate
    /// </summary>
    public class Validate : App_Code.BaseHttpHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            var action = context.Request.Form["action"];
            if (action == "user_email")
            {
                using (Lib.Repositories.UserRepository rep = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    bool res = rep.emailAlreadyUsed(this.ActiveUser.Id, context.Request.Form["email"]);

                    if (res)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("NOK");
                    }
                    else
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("OK");
                    }
                }
            }
            else if (action == "user_login")
            {
                using (Lib.Repositories.UserRepository rep = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    bool res = rep.loginAlreadyUsed(this.ActiveUser.Id, context.Request.Form["login"]);

                    if (res)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("NOK");
                    }
                    else
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("OK");
                    }
                }
            }
        }
    }
}