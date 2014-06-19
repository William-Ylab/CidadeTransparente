using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Lib.Entities
{
    public class RequestCity
    {
        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long CityId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long UserId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long PeriodId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public int RequestType { get; set; }

        public Enumerations.RequestType RequestTypeEnum
        {
            get
            {
                return (Enumerations.RequestType)Enum.Parse(typeof(Enumerations.RequestType), RequestType.ToString());
            }
        }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public int Status { get; set; }

        public Enumerations.RequestStatus RequestStatusEnum
        {
            get
            {
                return (Enumerations.RequestStatus)Enum.Parse(typeof(Enumerations.RequestStatus), Status.ToString());
            }
        }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public DateTime RequestDate { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public City City { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public Period Period { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public User User { get; set; }
    }
}
