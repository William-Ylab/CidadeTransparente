using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Security;
using System.Web.SessionState;
using Site.App_Code;

namespace Site
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            BundleTable.EnableOptimizations = true;
            BundleConfig.registerBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            try
            {
                Response.Filter = null;

                //Verifica se o tipo de erro é HTTPException
                if (ex is HttpException)
                {
                    HttpException httpException = ex as HttpException;

                    //Verifica se é erro 404, se for, não loga erro, pois não tem necessidade, apenas precisa redirecionar o usuário
                    if (httpException.GetHttpCode() == 404)
                        return;
                }

                HttpContext.Current.Cache["error:" + Session.SessionID] = Lib.Log.ErrorLog.getStringErrorFromException(ex).Replace("\r\n", "<br/>");

                //Loga o erro no banco
                Lib.Log.ErrorLog.saveError("Web.ApplicationError", ex);
            }
            catch (Exception ex1)
            {
                throw ex1;
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}