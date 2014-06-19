using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class Answer
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long BaseQuestionId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long ResponseFormId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public decimal? Score { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        [Required(AllowEmptyStrings = true, ErrorMessageResourceName = "REQUIRED_ANSWERS_OBSERVATION", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Observation { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public virtual BaseQuestion BaseQuestion { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public virtual ResponseForm ResponseForm { get; set; }

        #endregion 

        #region [Constructors]

        public Answer()
        {

        }

        #endregion

        #region [Methods]

        /// <summary>
        /// cálcula a nota da pergunta
        /// </summary>
        internal decimal calculateNP(ref List<string> tracking)
        {
            if (tracking == null)
            {
                tracking = new List<string>();
            }

            //resposta foi N/A
            if (this.Score.HasValue == false)
            {
                return 0;
            }

            decimal ret = this.Score.Value * this.BaseQuestion.Value;

            StringBuilder sbuilder = new StringBuilder();
            sbuilder.AppendLine(string.Format("Calculo NP (Pergunta {0} - {1}.{2}):", this.BaseQuestion.BaseSubBlock.BaseBlock.Name, this.BaseQuestion.BaseSubBlock.Index, this.BaseQuestion.Index));
            sbuilder.AppendLine(string.Format("Peso: {0}; Pontuação {1}; NP = Peso * Pontuação; NP = {2};", this.BaseQuestion.Value, this.Score, ret));

            tracking.Add(sbuilder.ToString());

            return ret;
        }

        #endregion
    }
}
