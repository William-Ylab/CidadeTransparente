using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class State
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public string Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = true)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "REQUIRED_STATE_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(50, ErrorMessageResourceName = "MAX_LENGTH_STATE_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Name { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<City> Cities { get; set; }

        #endregion 

        #region [Constructors]

        public State()
        {

        }

        #endregion
    }
}
