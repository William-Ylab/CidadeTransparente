using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class BaseForm
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long PeriodId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = true)]
        [Required(AllowEmptyStrings = true, ErrorMessageResourceName = "REQUIRED_BASEFORM_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(100, ErrorMessageResourceName = "MAX_LENGTH_BASEFORM_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Name { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public DateTime CreationDate { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<BaseBlock> BaseBlocks { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public Period Period { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<ResponseForm> ResponseForms { get; set; }

        #endregion

        #region [Constructors]

        public BaseForm()
        {

        }

        #endregion
    }
}
