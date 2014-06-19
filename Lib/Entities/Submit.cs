using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class Submit
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long ResponseFormId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public DateTime Date { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        [Required(AllowEmptyStrings = true, ErrorMessageResourceName = "REQUIRED_SUBMIT_OBSERVATION", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Observation { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public int Status { get; set; }

        public Enumerations.SubmitStatus StatusEnum
        {
            get
            {
                return (Enumerations.SubmitStatus)Enum.Parse(typeof(Enumerations.SubmitStatus), Status.ToString());
            }
        }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public ResponseForm ResponseForm { get; set; }

        #endregion 

        #region [Constructors]

        public Submit()
        {

        }

        #endregion
    }
}
