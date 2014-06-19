using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class Group
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long CityId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public long? ResponsableId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public City City { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public User Responsable { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<User> Collaborators { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public long PeriodId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public Period Period { get; set; }
        #endregion 

        #region [Constructors]

        public Group()
        {

        }

        #endregion
    }
}
