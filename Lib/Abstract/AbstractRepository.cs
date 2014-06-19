using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Abstract
{
    public class AbstractRepository
    {
        #region [Private Methods]

        public bool HasErrors
        {
            get
            {
                if (Errors == null)
                    return false;

                return Errors != null ? (Errors.Count > 0 ? true : false) : false;
            }
        }

        public List<string> Errors { get; private set; }

        #endregion

        #region [Private Methods]
        protected void addErrorMessage(string message)
        {
            if (Errors == null)
                Errors = new List<string>();

            Errors.Add(message);
        }

        protected void addErrorMessage(DbEntityValidationException exception)
        {

            if (exception != null)
            {
                if (exception.EntityValidationErrors != null && exception.EntityValidationErrors.Count() > 0)
                {
                    foreach (DbEntityValidationResult result in exception.EntityValidationErrors)
                    {
                        if (result.ValidationErrors != null)
                        {
                            foreach (var error in result.ValidationErrors)
                            {
                                addErrorMessage(error.ErrorMessage);
                            }
                        }
                    }
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(exception.Message))
                    {
                        addErrorMessage(exception.Message);
                    }
                }
            }
        }
        #endregion
    }
}
