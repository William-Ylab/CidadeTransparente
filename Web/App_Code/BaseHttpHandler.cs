using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.App_Code
{
    public class BaseHttpHandler : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        #region IHttpHandler Members

        private BasePage _basePage = null;

        public BasePage BasePage
        {
            get
            {
                if (_basePage == null)
                    _basePage = new BasePage();

                return _basePage;
            }
        }

        private Lib.Entities.User _activeUser;
        public Lib.Entities.User ActiveUser
        {
            get
            {
                if (_activeUser == null)
                {
                    _activeUser = BasePage.ActiveUser;
                }


                return _activeUser;
            }
        }

        public virtual bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region [Public Methods]

        /// <summary>
        /// Verifica se o usuário está autenticado
        /// </summary>
        /// <returns></returns>
        public bool userAuthenticate()
        {
            bool userAuthenticate = false;
            if (ActiveUser != null)
            {
                userAuthenticate = true;
            }

            return userAuthenticate;
        }

        #endregion
    }
}