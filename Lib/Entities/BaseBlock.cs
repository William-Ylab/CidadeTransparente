using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class BaseBlock
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long BaseFormId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = true)]
        [Required(AllowEmptyStrings = true, ErrorMessageResourceName = "REQUIRED_BASEBLOCK_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(50, ErrorMessageResourceName = "MAX_LENGTH_BASEBLOCK_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Name { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public BaseForm BaseForm { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public virtual List<BaseSubBlock> BaseSubBlocks { get; set; }

        #endregion

        #region [Constructors]

        public BaseBlock()
        {

        }

        #endregion

        #region [Method]

        public decimal? calculateNB(long responseFormId, ref List<String> tracking)
        {
            decimal sb = 0;
            int totalAnswersnotNA = 0;

            if (this.BaseSubBlocks != null)
            {
                foreach (var subBlock in this.BaseSubBlocks)
                {
                    int totalAnswersnotNAAux = 0;

                    sb += subBlock.calculate(responseFormId, out totalAnswersnotNAAux, ref tracking);

                    totalAnswersnotNA += totalAnswersnotNAAux;
                }
            }

            StringBuilder sbuilder = new StringBuilder();

            if (totalAnswersnotNA == 0)
            {
                sbuilder.AppendLine(string.Format("Bloco {0} foi desconsiderado pois só havia respostas N/A", this.Name));

                tracking.Add(sbuilder.ToString());

                return null;
            }

            decimal ret = sb / totalAnswersnotNA;

            sbuilder.AppendLine(string.Format("Calculo NB (bloco {0}):", this.Name));
            sbuilder.AppendLine(string.Format("Total perguntas nao N/A: {0}; SSB {1}; NB = SSB/TotalPerguntasNaoN/A; NB = {2};", totalAnswersnotNA, sb, ret));

            tracking.Add(sbuilder.ToString());

            return ret;
        }

        #endregion
    }
}
