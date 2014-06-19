using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class ResponseForm
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long BaseFormId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long? CityId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public long UserId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public string TrackingNote { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public decimal TotalScore { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<Answer> Answers { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public BaseForm BaseForm { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public City City { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public User User { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<Submit> Submits { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<Review> Reviews { get; set; }

        #endregion

        #region [Constructors]

        public ResponseForm()
        {
            this.TrackingNote = "";
        }

        #endregion

        #region [Methods]

        public double calculateTotalScore()
        {
            DateTime inicio = DateTime.Now;

            List<String> tracking = new List<string>();

            //Seja NTI = Nota Transparência da Informação, NC = Nota do Bloco Conteúdo e NCI = Nota Canais de Informação, temos:
            //      NTI = Raiz Quadrada de NC * NCI
            double blockResult = 1;

            var baseBlocksIds = this.BaseForm.BaseBlocks.Select(f => f.Id).OrderBy(f => f).ToList();

            string nomeBlocos = String.Empty;
            bool allBBhasNA = true;
            this.BaseForm.BaseBlocks.Where(f => baseBlocksIds.Take(2).Contains(f.Id)).ToList().ForEach(f =>
            {
                decimal? nb = f.calculateNB(this.Id, ref tracking);
                nomeBlocos += f.Name + "; ";

                if (nb.HasValue == false)
                {
                    return;
                }

                allBBhasNA = false;

                blockResult *= Double.Parse(nb.ToString());
            });

            //[20-01-2014: FDT-135] Caso todos os blocos forem N/A, o resultado será 0.
            if (allBBhasNA)
                blockResult = 0;


            tracking.Add(String.Format("Resultado da multiplicação dos blocos ({0}) - (NC * Nci): {1}", nomeBlocos, blockResult));

            double NTI = Math.Sqrt(blockResult);

            tracking.Add(String.Format("Nota Transparência da Informação (Nti)= raiz quadrada de NC * Nci: {0}", NTI));

            //var blocoCDP = this.BaseForm.BaseBlocks.Where(f => f.Id == 3).FirstOrDefault();

            var blocoCDP = this.BaseForm.BaseBlocks[2];

            var blocoAux = blocoCDP.calculateNB(this.Id, ref tracking);

            double NCP = 0;
            if (blocoAux.HasValue)
            {
                NCP = Double.Parse(blocoAux.ToString());
            }


            tracking.Add(String.Format("Notal do bloco canais de participação (NCP): {0}", NCP));

            double IT = (0.7 * NTI) + (0.3 * NCP);

            tracking.Add(String.Format(" Índice de Transparência = 0,7 * Nti + 0,30 * Ncp (IT): {0}", IT));

            this.TotalScore = Convert.ToDecimal(IT);

            tracking.Add("Tempo total: " + DateTime.Now.Subtract(inicio).Milliseconds + " ms");

            tracking.ForEach(f =>
                {
                    this.TrackingNote += f + "\r\n";
                });

            return IT;
        }

        public int getTotalQuestions()
        {
            int ret = 0;

            if (this.BaseForm != null)
            {
                if (this.BaseForm.BaseBlocks != null)
                {
                    foreach (var block in this.BaseForm.BaseBlocks)
                    {
                        if (block.BaseSubBlocks != null)
                        {
                            foreach (var bsb in block.BaseSubBlocks)
                            {
                                if (bsb.BaseQuestions != null)
                                {
                                    ret += bsb.BaseQuestions.Count;
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Retornar TRUE se caso o ultimo submit esteja com status de formulário submetido
        /// </summary>
        /// <returns></returns>
        public bool isSubmitted()
        {
            if (this.Submits != null)
            {
                if (this.Submits.Count > 0)
                {
                    //Ordena pelo submit mais recente e verifica se o status é submitted
                    var firstSubmit = this.Submits.OrderByDescending(s => s.Date).First();

                    if (firstSubmit.StatusEnum == Lib.Enumerations.SubmitStatus.Submitted)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Retornar TRUE se caso o ultimo submit esteja com status de formulário aprovado
        /// </summary>
        /// <returns></returns>
        public bool isApproved()
        {
            if (this.Submits != null)
            {
                if (this.Submits.Count > 0)
                {
                    //Ordena pelo submit mais recente e verifica se o status é submitted
                    var firstSubmit = this.Submits.OrderByDescending(s => s.Date).First();

                    if (firstSubmit.StatusEnum == Lib.Enumerations.SubmitStatus.Approved)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Retornar TRUE se caso o ultimo submit esteja com status de formulário não aprovado
        /// </summary>
        /// <returns></returns>
        public bool isNotApproved()
        {
            if (this.Submits != null)
            {
                if (this.Submits.Count > 0)
                {
                    //Ordena pelo submit mais recente e verifica se o status é submitted
                    var firstSubmit = this.Submits.OrderByDescending(s => s.Date).First();

                    if (firstSubmit.StatusEnum == Lib.Enumerations.SubmitStatus.NotApproved)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string getNotApprovedObservations()
        {
            if (this.isNotApproved())
            {
                if (this.Submits != null)
                {
                    if (this.Submits.Count > 0)
                    {
                        //Ordena pelo submit mais recente e verifica se o status é submitted
                        var firstSubmit = this.Submits.OrderByDescending(s => s.Date).First();

                        if (firstSubmit.StatusEnum == Lib.Enumerations.SubmitStatus.NotApproved)
                        {
                            return firstSubmit.Observation;
                        }
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Retornar TRUE se caso o questionário está completamente respondido
        /// </summary>
        /// <returns></returns>
        public bool isAlreadyAnswered()
        {
            bool isAlreadyAnswered = true;

            //Compara o formulário respondido e recupera o total de questões respondidas por blocos
            using (Lib.Repositories.BaseFormRepository rep = new Repositories.BaseFormRepository(null))
            {
                var baseForm = rep.getInstanceById(this.BaseFormId);

                baseForm.BaseBlocks.ForEach(bb =>
                {
                    var totalQuestionInBlock = 0;
                    var totalQuestionAnswered = 0;
                    bb.BaseSubBlocks.ForEach(bsb =>
                    {
                        totalQuestionInBlock += bsb.BaseQuestions.Count;
                        totalQuestionAnswered += this.Answers.Where(a => bsb.BaseQuestions.Select(bq => bq.Id).Contains(a.BaseQuestionId)).Count();
                    });

                    //Se um determinado bloco tiver o total diferente de respondido o questionário não foi concluido.
                    if (totalQuestionInBlock != totalQuestionAnswered)
                    {
                        isAlreadyAnswered = false;
                    }
                });
            }
            return isAlreadyAnswered;
        }

        #endregion
    }
}
