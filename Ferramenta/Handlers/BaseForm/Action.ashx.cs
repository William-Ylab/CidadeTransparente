using Ferramenta.App_Code;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Ferramenta.Handlers.BaseForm
{
    /// <summary>
    /// Summary description for Action
    /// </summary>
    public class Action : BaseHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                if (!String.IsNullOrWhiteSpace(context.Request.Form["t"])) // && !String.IsNullOrWhiteSpace(context.Request.QueryString["bf"])
                {
                    switch (context.Request.Form["t"].ToLower())
                    {
                        case "cf": //GetCurrentForm
                            //Carrega o questionário atual

                            long periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(context.Request.Form["p"].ToString()));

                            Lib.Entities.BaseForm form = getCurrentForm(periodId);

                            if (form != null)
                            {
                                var result = form.BaseBlocks.Select(bb => new
                                {
                                    BlockId = bb.Id,
                                    BlockName = bb.Name,

                                    SubBlocks = bb.BaseSubBlocks.Select(bq => new
                                    {
                                        SubblockId = bq.Id,
                                        SubBlockName = bq.Name,
                                        SubBlockIndex = bq.Index,
                                        SubBlockWeight = bq.Weight.ToString(new CultureInfo("pt-BR")),
                                        Questions = bq.BaseQuestions.Select(q => new
                                        {
                                            Id = q.Id,
                                            Question = q.Question.Replace("<br>", "\n"),
                                            Tip = q.Tip,
                                            Value = q.Value.ToString(new CultureInfo("pt-BR")),
                                            Index = q.Index
                                        }).ToList()
                                    }).ToList()
                                }).ToList();

                                context.Response.ContentType = "application/json";
                                context.Response.Write(serializer.Serialize(new { FormName = form.Name,PeriodStatus = form.Period.Open, BaseBlocks = result }));
                            }
                            else
                            {
                                context.Response.ContentType = "application/json";
                                context.Response.Write("[]");
                            
                            }
                            break;
                        case "co":
                            createFormFromCopy(context);
                            break;
                        case "nb":
                            saveSubBlock(context);
                            break;
                        case "nq":
                            saveQuestion(context);
                            break;
                        case "db": //Delete SubBlock
                            deleteSubBlock(context);
                            break;
                        case "dq": //Delete Question
                            deleteQuestion(context);
                            break;
                        case "ob": //Reorder Block
                            var paramBlockIds = context.Request.Form["oi"];
                            OrderBlockAux[] blockIds = { };

                            blockIds = (OrderBlockAux[])serializer.Deserialize(paramBlockIds, typeof(OrderBlockAux[]));
                            orderBlocks(context, blockIds);
                            break;
                        case "oq": //Reorder Questions
                            var paramblockId = context.Request.Form["bi"].ToString();
                            var paramQuestionsIds = context.Request.Form["oi"];
                            string[] questionsIds = { };

                            long blockId = 0;
                            long.TryParse(paramblockId, out blockId);

                            questionsIds = (string[])serializer.Deserialize(paramQuestionsIds, typeof(string[]));

                            orderQuestions(context, questionsIds, blockId);
                            break;
                        case "sfn":
                            using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
                            {
                                periodId = Convert.ToInt64(Commons.SecurityUtils.descriptografar(context.Request.Form["p"].ToString()));

                                var formAux = repository.getInstanceByPeriodId(periodId);
                                if (formAux == null)
                                {
                                    //Cria um novo formulário
                                    formAux = new Lib.Entities.BaseForm();
                                    formAux.Name = context.Request.Form["n"].ToString();
                                    formAux.PeriodId = periodId;
                                }
                                else
                                {
                                    //Atualiza o nome do formuário
                                    formAux.Name = context.Request.Form["n"].ToString();
                                }

                                repository.save(formAux);
                            }

                            break;
                        default:
                            context.Response.StatusCode = 500;
                            context.Response.Write(Resources.Message.parameter_not_found);
                            break;
                    }
                }
                else
                {
                    context.Response.StatusCode = 500;
                    context.Response.Write(Resources.Message.parameter_not_found);
                }
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Web.Handler.BaseForm.Action.ProcessRequest", ex);
                context.Response.StatusCode = 500;
                context.Response.Write(Resources.Message.couldnt_process_request);
            }
        }

        private void createFormFromCopy(HttpContext context)
        {
            var pPeriodId = Commons.SecurityUtils.descriptografar( context.Request.Form["p"].ToString());
            var pFormId = Commons.SecurityUtils.descriptografar(context.Request.Form["f"].ToString());

            long formId = 0;
            long periodId = 0;

            long.TryParse(pFormId, out formId);
            long.TryParse(pPeriodId, out periodId);

            if (formId > 0 && periodId >0)
            {
                using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(null))
                {
                    var copied = repository.copyFormToPeriod(formId, periodId);

                    if (copied)
                    {
                        context.Response.Write("ok");
                    }
                    else
                    {
                        context.Response.Write("nok");
                    }
                }
            }
        }

        private void deleteSubBlock(HttpContext context)
        {
            var pSubBlockId = context.Request.Form["id"].ToString();

            long subBlockId = 0;
            long.TryParse(pSubBlockId, out subBlockId);

            if (subBlockId > 0)
            {
                using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(null))
                {
                    var baseForm = repository.getInstanceBySubBlockId(subBlockId);

                    if (baseForm.BaseBlocks != null)
                    {
                        baseForm.BaseBlocks.ForEach(bb =>
                        {
                            if (bb.BaseSubBlocks.Where(bsb => bsb.Id == subBlockId).Count() > 0)
                            {
                                bb.BaseSubBlocks.RemoveAll(bsb => bsb.Id == subBlockId);
                            }
                        });

                        repository.save(baseForm);
                    }
                    context.Response.Write("ok");
                }
            }
        }

        //private void saveNewSubBlock(HttpContext context)
        //{
        //    var paramName = context.Request.Form["n"].ToString();
        //    var paramWeight = context.Request.Form["w"].ToString();
        //    var paramBlockId = context.Request.Form["bid"].ToString();

        //    if (!String.IsNullOrWhiteSpace(paramName) && !String.IsNullOrWhiteSpace(paramWeight) && !String.IsNullOrWhiteSpace(paramBlockId))
        //    {
        //        using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
        //        {
        //            var baseForm = repository.getInstanceByPeriodDate(DateTime.Now);
        //            //Adiciona um novo bloco o base block
        //            int lastIndex = 1;
        //            decimal weight = 0m;
        //            long blockId = 0;

        //            weight = Convert.ToDecimal(paramWeight, new CultureInfo("pt-BR"));

        //            long.TryParse(paramBlockId, out blockId);

        //            if (blockId > 0)
        //            {
        //                Lib.Entities.BaseBlock block = baseForm.BaseBlocks.Where(f => f.Id == blockId).FirstOrDefault();

        //                if (block != null)
        //                {
        //                    if (block.BaseSubBlocks.Count > 0)
        //                        lastIndex = block.BaseSubBlocks.Select(bq => bq.Index).Max() + 1;

        //                    block.BaseSubBlocks.Add(new Lib.Entities.BaseSubBlock()
        //                    {
        //                        Index = lastIndex,
        //                        Name = paramName,
        //                        Weight = weight,
        //                        BaseBlockId = blockId
        //                    });

        //                    repository.save(baseForm);
        //                    context.Response.Write(lastIndex);
        //                }
        //            }
        //        }
        //    }
        //}

        private void orderQuestions(HttpContext context, string[] stQuestionIds, long subblockId)
        {
            //converte os blocos para long
            List<long> questionListId = new List<long>();
            long questionId = 0;
            foreach (var s in stQuestionIds)
            {
                if (long.TryParse(s, out questionId))
                    questionListId.Add(questionId);
            }

            if (questionListId.Count > 0 && subblockId > 0)
            {
                using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(null))
                {
                    var baseForm = repository.getInstanceBySubBlockId(subblockId);

                    if (baseForm.BaseBlocks != null)
                    {
                        //Varre a lista de baseblocks a procura do subbloco
                        baseForm.BaseBlocks.ForEach(bb =>
                        {
                            if (bb.BaseSubBlocks != null)
                            {
                                bb.BaseSubBlocks.ForEach(bsb =>
                                {
                                    //Verifica se é o sub-bloco procurado
                                    if (bsb.Id == subblockId)
                                    {
                                        if (bsb.BaseQuestions != null)
                                        {
                                            //Verifica se a lista de blocos tem a mesma quantidade que os ids enviados
                                            if (bsb.BaseQuestions.Count == questionListId.Count)
                                            {
                                                for (int index = 0; index < questionListId.Count; index++)
                                                {
                                                    var question = bsb.BaseQuestions.Where(bq => bq.Id == questionListId[index]).FirstOrDefault();
                                                    if (question != null)
                                                    {
                                                        //Atualiza o indice pela ordem enviada
                                                        question.Index = index + 1;
                                                    }
                                                }

                                                repository.save(baseForm);
                                                context.Response.Write("ok");
                                            }
                                        }
                                    }
                                });

                            }

                        });
                    }
                }
            }
        }

        private void orderBlocks(HttpContext context, OrderBlockAux[] orderBlocks)
        {
            if (orderBlocks.Length > 0)
            {
                using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
                {
                    var baseForm = repository.getInstanceByBlockId(orderBlocks[0].Id);

                    //Verifica se a lista de blocos tem a mesma quantidade que os ids enviados
                    if (baseForm.BaseBlocks.Count == orderBlocks.Length)
                    {
                        foreach (OrderBlockAux oBAux in orderBlocks)
                        {
                            var block = baseForm.BaseBlocks.Where(bb => bb.Id == oBAux.Id).FirstOrDefault();

                            for (int index = 0; index < oBAux.SubBlocks.Count; index++)
                            {
                                var subblock = block.BaseSubBlocks.Where(bsb => bsb.Id == oBAux.SubBlocks[index]).FirstOrDefault();
                                if (subblock != null)
                                {
                                    //Atualiza o indice pela ordem enviada
                                    subblock.Index = index + 1;
                                }
                            }
                        }

                        repository.save(baseForm);
                        context.Response.Write("ok");
                    }
                }
            }
        }

        private void saveNewQuestion(HttpContext context)
        {
            var question = context.Request.Form["q"].ToString();
            var value = context.Request.Form["v"].ToString();
            var psubblockId = context.Request.Form["sbi"].ToString();

            long subblockId = 0;
            long.TryParse(psubblockId, out subblockId);

            if (subblockId > 0)
            {
                using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(null))
                {
                    var baseForm = repository.getInstanceBySubBlockId(subblockId);
                    int lastIndex = 1;
                    //Atualiza o base block
                    baseForm.BaseBlocks.ForEach(bb =>
                    {
                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            if (bsb.Id == subblockId)
                            {
                                decimal v = 0m;

                                v = Convert.ToDecimal(value, new CultureInfo("pt-BR"));

                                if (bsb.BaseQuestions.Count > 0)
                                    lastIndex = bsb.BaseQuestions.Select(bq => bq.Index).Max() + 1;

                                bsb.BaseQuestions.Add(new Lib.Entities.BaseQuestion()
                                {
                                    Question = question,
                                    Value = v,
                                    Tip = "",
                                    Index = lastIndex,
                                    BaseSubBlockId = bsb.Id
                                });
                            }
                        });
                    });

                    repository.save(baseForm);
                    context.Response.Write(lastIndex);
                }
            }
        }

        private void deleteQuestion(HttpContext context)
        {
            var pQuestionId = context.Request.Form["id"].ToString();

            long questionId = 0;
            long.TryParse(pQuestionId, out questionId);

            if (questionId > 0)
            {
                using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(null))
                {
                    var baseForm = repository.getInstanceByQuestionId(questionId);

                    ////Atualiza o base question
                    baseForm.BaseBlocks.ForEach(bb =>
                    {
                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            if (bsb.BaseQuestions.Where(bq => bq.Id == questionId).Count() > 0)
                            {
                                bsb.BaseQuestions.RemoveAll(bq => bq.Id == questionId);
                            }
                        });
                    });

                    repository.save(baseForm);

                    context.Response.Write("ok");
                }
            }
        }

        private void saveQuestion(HttpContext context)
        {
            var pQuestion =  context.Request.Form["q"].ToString().Replace("\n", "&lt;br/&gt;");
            var pValue = context.Request.Form["v"].ToString();
            var pQuestionId = context.Request.Form["qid"].ToString();
            var pSubBlockId = context.Request.Form["sbi"].ToString();

            if (string.IsNullOrEmpty(pValue))
            {
                pValue = "1";
            }

            long questionId = 0;
            long.TryParse(pQuestionId, out questionId);

            long subblockId = 0;
            long.TryParse(pSubBlockId, out subblockId);

            using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(null))
            {
                if (questionId > 0)
                {
                    var baseForm = repository.getInstanceByQuestionId(questionId);

                    //Atualiza o base question
                    baseForm.BaseBlocks.ForEach(bb =>
                    {
                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            bsb.BaseQuestions.ForEach(bq =>
                            {

                                if (bq.Id == questionId)
                                {
                                    bq.Question = pQuestion;
                                    bq.Value = Convert.ToDecimal(pValue, new CultureInfo("pt-BR"));
                                }
                            });
                        });
                    });

                    repository.save(baseForm);
                }
                else
                {
                    //Nova questão

                    var baseForm = repository.getInstanceBySubBlockId(subblockId);

                    int lastIndex = 1;
                    //Atualiza o base block
                    baseForm.BaseBlocks.ForEach(bb =>
                    {
                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            if (bsb.Id == subblockId)
                            {
                                if (bsb.BaseQuestions.Count > 0)
                                    lastIndex = bsb.BaseQuestions.Select(bq => bq.Index).Max() + 1;

                                bsb.BaseQuestions.Add(new Lib.Entities.BaseQuestion()
                                {
                                    Question = pQuestion,
                                    Value = Convert.ToDecimal(pValue, new CultureInfo("pt-BR")),
                                    Tip = "",
                                    Index = lastIndex,
                                    BaseSubBlockId = bsb.Id
                                });
                            }
                        });
                    });

                    repository.save(baseForm);
                }

                if (!repository.HasErrors)
                {
                    context.Response.Write("ok");
                }
                else
                {
                    context.Response.Write("nok");
                }
            }
        }

        private void saveSubBlock(HttpContext context)
        {
            var pname = context.Request.Form["n"].ToString();
            var pweight = context.Request.Form["w"].ToString();
            var psubblockId = context.Request.Form["sbi"].ToString();
            var pblockId = context.Request.Form["bid"].ToString();

            long subblockId = 0;
            long.TryParse(psubblockId, out subblockId);

            long blockId = 0;
            long.TryParse(pblockId, out blockId);

            using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(null))
            {

                if (subblockId > 0)
                {

                    var baseForm = repository.getInstanceBySubBlockId(subblockId);

                    //Atualiza o base block
                    baseForm.BaseBlocks.ForEach(bb =>
                    {
                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            if (bsb.Id == subblockId)
                            {

                                bsb.Name = pname;
                                bsb.Weight = Convert.ToDecimal(pweight, new CultureInfo("pt-BR"));
                            }
                        });
                    });
                    repository.save(baseForm);
                }
                else
                {
                    //Adiciona um novo bloco o base block
                    var baseForm = repository.getInstanceByBlockId(blockId);

                    int lastIndex = 1;

                    if (baseForm != null)
                    {
                        Lib.Entities.BaseBlock block = baseForm.BaseBlocks.Where(f => f.Id == blockId).FirstOrDefault();

                        if (block != null)
                        {
                            if (block.BaseSubBlocks.Count > 0)
                                lastIndex = block.BaseSubBlocks.Select(bq => bq.Index).Max() + 1;

                            block.BaseSubBlocks.Add(new Lib.Entities.BaseSubBlock()
                            {
                                Index = lastIndex,
                                Name = pname,
                                Weight = Convert.ToDecimal(pweight, new CultureInfo("pt-BR")),
                                BaseBlockId = blockId
                            });

                            repository.save(baseForm);
                        }
                    }

                }

                if (!repository.HasErrors)
                {
                    context.Response.Write("ok");
                }
                else
                {
                    context.Response.Write("nok");
                }
            }
        }

        private Lib.Entities.BaseForm getCurrentForm(long periodId)
        {
            Lib.Entities.BaseForm form = null;
            using (Lib.Repositories.BaseFormRepository repository = new Lib.Repositories.BaseFormRepository(this.ActiveUser))
            {
                form = repository.getInstanceByPeriodId(periodId);
            }

            return form;
        }
    }

    public class OrderBlockAux
    {
        public long Id { get; set; }

        public List<long> SubBlocks { get; set; }
    }
}