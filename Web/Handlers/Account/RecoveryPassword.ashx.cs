using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Handlers.Account
{
    /// <summary>
    /// Summary description for RecoveryPassword
    /// </summary>
    public class RecoveryPassword : App_Code.BaseHttpHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                string email = context.Request.Form["email"];
                string user = context.Request.Form["user"];

                using (Lib.Repositories.UserRepository repository = new Lib.Repositories.UserRepository(this.ActiveUser))
                {
                    try
                    {
                        repository.recoveryPassowrd(email, user);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("Ok");
                    }
                    catch (Exception ex1)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.StatusCode = 500;
                        context.Response.Write(ex1.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
            }
        }
    }
}