using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class City
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public String StateId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = true)]
        [Required(AllowEmptyStrings = true, ErrorMessageResourceName = "REQUIRED_CITY_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(200, ErrorMessageResourceName = "MAX_LENGTH_CITY_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        public String Name { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public State State { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public List<Group> Groups { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public List<ResponseForm> ResponseForms { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public List<User> Users { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public List<RequestCity> RequestCities { get; set; }

        #endregion

        #region [Constructors]

        public City()
        {

        }

        #endregion
    }
}
