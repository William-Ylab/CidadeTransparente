using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Report
{
    public class ReportManager
    {
        private Entities.User activeUser = null;

        #region [Construtores]

        private ReportManager() { }

        public ReportManager(Entities.User activeUser)
        {
            this.activeUser = activeUser;
        }

        #endregion

        #region [Métodos]

        public List<ScorePerBlock> generateScorePerBlockChart(long responseFormId)
        {
            List<ScorePerBlock> ret = new List<ScorePerBlock>();

            using (Lib.Repositories.ResponseFormRepository repository = new Repositories.ResponseFormRepository(this.activeUser))
            {
                Entities.ResponseForm responseForm = repository.getInstanceById(responseFormId);

                if (responseForm != null && responseForm.BaseForm != null && responseForm.BaseForm.BaseBlocks != null)
                {
                    foreach (Entities.BaseBlock block in responseForm.BaseForm.BaseBlocks)
                    {
                        foreach (Entities.BaseSubBlock subBlock in block.BaseSubBlocks)
                        {
                            ScorePerBlock score = new ScorePerBlock();
                            score.ColumnName = subBlock.Name;

                            var answers = responseForm.Answers.Where(f => f.BaseQuestion.BaseSubBlockId == subBlock.Id && f.Score != null).Sum(a => a.Score);

                            //Recupera o total de N/A no bloco, para que possamos desconsiderar da porcetagem
                            var totalNAAnswers = responseForm.Answers.Where(f => f.BaseQuestion.BaseSubBlockId == subBlock.Id && f.Score == null).Count();

                            score.MaxScore = subBlock.BaseQuestions.Count - totalNAAnswers;
                            score.TotalScore = (decimal)answers;
                            decimal percentage = score.MaxScore > 0 ? (decimal)answers / score.MaxScore : 0;

                            if (score.MaxScore == 0)
                            {
                                score.Percentage = 0;
                                score.Label = "N/A";
                            }
                            else
                            {
                                score.Percentage = Math.Round(percentage, 2);
                                score.Label = (Math.Round(percentage, 2) * 100).ToString() + "%";
                            }
                                

                            ret.Add(score);
                        }
                    }
                }
            }

            return ret;
        }

        public List<ScorePerBlock> generateScorePerFormChart(long responseFormId)
        {
            List<ScorePerBlock> ret = new List<ScorePerBlock>();
            decimal totalScoreInForm = 0m;
            decimal totalNAScore = 0m;
            decimal totalScoreAchieved = 0m;

            using (Lib.Repositories.ResponseFormRepository repository = new Repositories.ResponseFormRepository(this.activeUser))
            {
                Entities.ResponseForm responseForm = repository.getInstanceById(responseFormId);

                if (responseForm != null && responseForm.BaseForm != null && responseForm.BaseForm.BaseBlocks != null)
                {
                    //Soma todas as questões
                    responseForm.BaseForm.BaseBlocks.ForEach(bb =>
                    {
                        bb.BaseSubBlocks.ForEach(sbb =>
                        {
                            totalScoreInForm += sbb.BaseQuestions.Count;
                        });
                    });

                    //Recupera o total de score por sub bloco
                    ScorePerBlock score = null;
                    foreach (Entities.BaseBlock block in responseForm.BaseForm.BaseBlocks)
                    {
                        foreach (Entities.BaseSubBlock subBlock in block.BaseSubBlocks)
                        {
                            score = new ScorePerBlock();
                            score.ColumnName = subBlock.Name;

                            totalNAScore += responseForm.Answers.Where(f => f.BaseQuestion.BaseSubBlockId == subBlock.Id && f.Score == null).Count();

                            var answers = responseForm.Answers.Where(f => f.BaseQuestion.BaseSubBlockId == subBlock.Id && f.Score != null).Sum(a => a.Score);

                            score.MaxScore = totalScoreInForm;
                            score.TotalScore = (decimal)answers;

                            decimal percentage = score.TotalScore / score.MaxScore;

                            totalScoreAchieved += score.TotalScore;
                            score.Percentage = Math.Round(percentage * 100, 2);
                            score.Label = Math.Round(percentage * 100, 2).ToString();

                            ret.Add(score);
                        }
                    }

                    //Cria o score de NA.
                    score = new ScorePerBlock();
                    score.ColumnName = "N/A";
                    score.MaxScore = totalScoreInForm;
                    score.TotalScore = totalNAScore;
                    score.Percentage = Math.Round((score.TotalScore / score.MaxScore) * 100, 2);
                    score.Label = Math.Round((score.TotalScore / score.MaxScore) * 100, 2).ToString();
                    ret.Add(score);

                    //Cria o score de Pontos faltantes para o 100%
                    score = new ScorePerBlock();
                    score.ColumnName = "Não pontuado";
                    score.MaxScore = totalScoreInForm;
                    score.TotalScore = (totalScoreInForm - totalNAScore) - totalScoreAchieved;
                    score.Percentage = Math.Round((score.TotalScore / score.MaxScore) * 100, 2);
                    score.Label = Math.Round((score.TotalScore / score.MaxScore) * 100, 2).ToString();
                    ret.Add(score);
                }
            }

            return ret;
        }

        #endregion
    }
}
