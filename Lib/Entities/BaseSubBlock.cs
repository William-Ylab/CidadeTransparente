using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class BaseSubBlock
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long BaseBlockId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = true)]
        [Required(AllowEmptyStrings = true, ErrorMessageResourceName = "REQUIRED_BASEBLOCK_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(100, ErrorMessageResourceName = "MAX_LENGTH_BASEBLOCK_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Name { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public decimal Weight { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public int Index { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public virtual BaseBlock BaseBlock { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public virtual List<BaseQuestion> BaseQuestions { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public decimal? Percent { get; private set; }

        #endregion

        #region [Constructors]

        public BaseSubBlock()
        {
            this.Percent = -1;
        }

        #endregion

        #region [Method]

        internal decimal calculate(long responseFormId, out int totalAnswersNotNA, ref List<String> tracking)
        {
            decimal ret = 0;
            totalAnswersNotNA = 0;

            if (tracking == null)
            {
                tracking = new List<string>();
            }

            if (this.BaseQuestions != null)
            {
                decimal bss = 0;

                List<Answer> answers = new List<Answer>();

                this.BaseQuestions.ForEach(f =>
                {
                    if (f.Answers != null)
                    {
                        answers.AddRange(f.Answers.Where(a => a.ResponseFormId == responseFormId).ToList());
                    }
                });


                foreach (var item in answers)
                {
                    bss += item.calculateNP(ref tracking);
                }

                totalAnswersNotNA = answers.Where(f => f.Score != null).Count();

                ret = bss * this.Weight;

                StringBuilder sbuilder = new StringBuilder();
                sbuilder.AppendLine(string.Format("Calculo NSB (Sub-bloco {0} - {1}):", this.Index, this.BaseBlock.Name));
                sbuilder.AppendLine(string.Format("Peso: {0}; BSS {1}; NSB = Peso * BSS; NSB = {2};", this.Weight, bss, ret));

                tracking.Add(sbuilder.ToString());
            }

            return ret;
        }

        public decimal? calculatePercent(long responseFormId)
        {
            decimal ret = 0;
            if (this.BaseQuestions != null)
            {
                decimal totalScore = 0;

                List<Answer> answers = new List<Answer>();

                this.BaseQuestions.ForEach(f =>
                {
                    if (f.Answers != null)
                    {
                        answers.AddRange(f.Answers.Where(a => a.ResponseFormId == responseFormId && a.Score.HasValue).ToList());
                    }
                });

                //somente N/A
                if (answers.Count == 0)
                {
                    Percent = null;
                    return null;
                }

                var tracking = new List<string>();
                foreach (var item in answers)
                {
                    totalScore += item.calculateNP(ref tracking);
                }

                //Como o máximo valor de resposta é 1, o máximo de pontos que pode ter é o valor do peso.
                decimal totalValue = answers.Sum(f => f.BaseQuestion.Value);

                if (totalValue == 0)
                {
                    return 0;
                }

                ret = totalScore / totalValue;
            }

            Percent = ret;
            
            return ret;
        }

        public string getColorByPercent(long responseFormId)
        {
            decimal? percent = this.Percent;

            if(this.Percent == -1m)
                percent = calculatePercent(responseFormId);
            
            if(this.Percent.HasValue == false){
                return "8aabaf";
            }
                    
            if(percent <= 0.2m)
            {
                return "d94c4b";
            }
            else if(percent <= 0.4m)
            {
                return "d56558";
            }
            else if(percent <= 0.6m)
            {
                return "d7824c";
            }
            else if(percent <= 0.8m)
            {
                return "eda637";
            }
            else //(percent <= 1m)
            {
                return "86aa65";
            }
        }


        #endregion
    }
}
