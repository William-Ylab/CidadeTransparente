using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class Period
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = true)]
        [Required(AllowEmptyStrings = true, ErrorMessageResourceName = "REQUIRED_PERIOD_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(150, ErrorMessageResourceName = "MAX_LENGTH_PERIOD_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Name { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public DateTime InitialDate { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public DateTime FinalDate { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public DateTime ConvocationInitialDate { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public DateTime ConvocationFinalDate { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public bool Open { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<BaseForm> BaseForms { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public bool Published { get; set; }

        public bool IsThereOneOrMoreResponseFormAccepted 
        {
            get
            {
                return isThereOneOrMoreResponseFormAccepted();
            }
        }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public List<RequestCity> RequestCities { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public List<Group> Groups { get; set; }

        #endregion

        #region [Constructors]

        public Period()
        {

        }

        #endregion

        #region [Methods]

        private bool isThereOneOrMoreResponseFormAccepted()
        {
            if (this.BaseForms != null)
            {
                foreach (var baseForm in this.BaseForms)
                {
                    if (baseForm.ResponseForms != null)
                    {
                        foreach (var responseForm in baseForm.ResponseForms)
                        {
                            if (responseForm.Submits != null)
                            {
                                var lastSubmit = responseForm.Submits.OrderBy(f => f.Id).LastOrDefault();

                                if (lastSubmit != null)
                                {
                                    if (lastSubmit.StatusEnum == Enumerations.SubmitStatus.Approved)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Verifica se o período está em convocação
        /// </summary>
        /// <returns></returns>
        public bool isInConvocationPeriod()
        {
            var dateNow = DateTime.Now;
            if (dateNow >= this.ConvocationInitialDate && dateNow <= this.ConvocationFinalDate)
                return true;

            return false;
        }

        /// <summary>
        /// Verifica se o período está em submissão
        /// </summary>
        /// <returns></returns>
        public bool isInSubmissionPeriod()
        {
            var dateNow = DateTime.Now;
            if (dateNow >= this.InitialDate && dateNow <= this.FinalDate)
                return true;

            return false;
        }

        #endregion
    }
}
