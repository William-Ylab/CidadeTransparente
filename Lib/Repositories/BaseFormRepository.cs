using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entities;

namespace Lib.Repositories
{
    public class BaseFormRepository : Abstract.AbstractRepository, IDisposable
    {
        private FDTContext context;
        private Lib.Entities.User ActiveUser;

        #region [Constructors]

        private BaseFormRepository()
        {
            //this.context = context;
            context = new FDTContext();
        }

        public BaseFormRepository(Lib.Entities.User activeUser)
        {
            this.ActiveUser = activeUser;
            context = new FDTContext();
        }

        #endregion

        #region IRepository<BaseFormRepository> Members

        /// <summary>
        /// Salva uma entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void save(BaseForm entity)
        {
            List<Commons.LogHistory> logs = null;
            Lib.Repositories.LogHistoryRepository log = new LogHistoryRepository(this.ActiveUser);

            try
            {
                context = new FDTContext();

                if (entity.Id == 0)
                {
                    if (entity.BaseBlocks == null || entity.BaseBlocks.Count == 0)
                    {
                        entity.BaseBlocks = new List<BaseBlock>();

                        //Cria os base blocks fixos
                        entity.BaseBlocks.Add(new BaseBlock()
                        {
                            Name = "BLOCO DE CONTEÚDO"
                        });
                        entity.BaseBlocks.Add(new BaseBlock()
                        {
                            Name = "BLOCO CANAIS DE INFORMAÇÃO"
                        });
                        entity.BaseBlocks.Add(new BaseBlock()
                        {
                            Name = "BLOCO CANAIS DE PARTICIPAÇÃO"
                        });
                    }


                    entity.CreationDate = DateTime.Now;

                    context.BaseForms.Add(entity);
                }
                else
                {
                    //Recuperando o questionário do banco
                    var baseForm = getInstanceById(entity.Id); //context.BaseForms.Include("BaseBlocks").Include("Period").Where(e => e.Id == entity.Id).FirstOrDefault();

                    ////salva o log.
                    //logs = log.getLogsHistory(baseForm, entity, ActiveUser, "Id", "Name", Enumerations.EntityType.BaseForm);

                    baseForm.Name = entity.Name;

                    //Limpa a lista de blocos
                    //baseForm.BaseBlocks.Clear();

                    //Percorre os blocos verificam quais são novos e quais foram removidos
                    if (entity.BaseBlocks != null)
                    {
                        //Adiciona os novos blocos
                        baseForm.BaseBlocks.AddRange(entity.BaseBlocks.Where(bb => bb.Id == 0).ToList());

                        //Atualiza as informações dos blocos e subblocks
                        foreach (var newBaseBlock in entity.BaseBlocks)
                        {
                            var currentBlock = baseForm.BaseBlocks.Where(f => f.Id == newBaseBlock.Id).FirstOrDefault();

                            if (currentBlock != null)
                            {
                                currentBlock.Name = newBaseBlock.Name;

                                //Adiciona os novos subblocos
                                currentBlock.BaseSubBlocks.AddRange(newBaseBlock.BaseSubBlocks.Where(bsb => bsb.Id == 0).ToList());

                                foreach (var newSubBlock in newBaseBlock.BaseSubBlocks)
                                {
                                    var currentSubBlock = currentBlock.BaseSubBlocks.Where(f => f.Id == newSubBlock.Id).FirstOrDefault();

                                    if (currentSubBlock != null)
                                    {
                                        currentSubBlock.Index = newSubBlock.Index;
                                        currentSubBlock.Name = newSubBlock.Name;
                                        currentSubBlock.Weight = newSubBlock.Weight;
                                    }
                                }
                            }
                        }



                        //Seleciona os blocos atuais
                        var listBlocksIds = entity.BaseBlocks.Select(b => b.Id).ToList();

                        var removedBlocks = baseForm.BaseBlocks.Where(bb => bb.Id != 0 && !listBlocksIds.Contains(bb.Id)).ToList();

                        //Remove do contexto os blocos removidos
                        removedBlocks.ForEach(rb =>
                        {
                            context.BaseBlocks.Remove(rb);
                        });

                        List<Entities.BaseSubBlock> removedSubBlocks = new List<BaseSubBlock>();

                        entity.BaseBlocks.ForEach(bb =>
                        {
                            var subBlocksIds = bb.BaseSubBlocks.Select(f => f.Id).ToList();

                            var baseblock = baseForm.BaseBlocks.Where(f => f.Id == bb.Id).FirstOrDefault();

                            removedSubBlocks.AddRange(baseblock.BaseSubBlocks.Where(bsb => bsb.Id != 0 && !subBlocksIds.Contains(bsb.Id)).ToList());
                        });

                        //Remove do contexto os blocos removidos
                        removedSubBlocks.ForEach(rb =>
                        {
                            context.BaseSubBlocks.Remove(rb);
                        });

                        //Removendo as perguntas dos blocos existentes
                        foreach (var entityBlock in entity.BaseBlocks.Where(bb => bb.Id > 0).ToList())
                        {
                            var currentBaseBlock = baseForm.BaseBlocks.Where(bb => bb.Id == entityBlock.Id).FirstOrDefault();

                            if (currentBaseBlock != null)
                            {

                                foreach (var entitySubBlock in entityBlock.BaseSubBlocks.Where(bb => bb.Id > 0).ToList())
                                {
                                    var currentSubBlock = currentBaseBlock.BaseSubBlocks.Where(bb => bb.Id == entitySubBlock.Id).FirstOrDefault();

                                    if (currentSubBlock != null)
                                    {
                                        if (entitySubBlock.BaseQuestions != null)
                                        {
                                            //Adiciona as novas questões
                                            currentSubBlock.BaseQuestions.AddRange(entitySubBlock.BaseQuestions.Where(bb => bb.Id == 0).ToList());

                                            //Atualiza as informações das perguntas
                                            entitySubBlock.BaseQuestions.ForEach(newbq =>
                                            {
                                                currentSubBlock.BaseQuestions.ForEach(curbq =>
                                                {
                                                    if (curbq.Id == newbq.Id)
                                                    {
                                                        curbq.Index = newbq.Index;
                                                        curbq.Question = newbq.Question;
                                                        curbq.Value = newbq.Value;
                                                        curbq.Tip = newbq.Tip;
                                                    }
                                                });
                                            });

                                            //Seleciona os blocos atuais
                                            var listQuestionsIds = entitySubBlock.BaseQuestions.Select(b => b.Id).ToList();

                                            var removedQuestions = currentSubBlock.BaseQuestions.Where(bq => bq.Id != 0 && !listQuestionsIds.Contains(bq.Id)).ToList();

                                            removedQuestions.ForEach(rq =>
                                            {
                                                //Remove a questão do contexto
                                                context.BaseQuestions.Remove(rq);
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                context.SaveChanges();

                if (logs != null)
                {
                    log.saveLogs(logs);
                }
            }
            catch (DbEntityValidationException ex)
            {
                //Adiciona na lista de erros os erros de DataAnnotation
                addErrorMessage(ex);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.save", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.save - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Deleta uma entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void delete(BaseForm entity)
        {
            try
            {
                if (entity != null)
                    delete(entity.Id);
            }
            catch (DbEntityValidationException ex)
            {
                //Adiciona na lista de erros os erros de DataAnnotation
                addErrorMessage(ex);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.delete", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.delete - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Deleta uma entidade
        /// </summary>
        /// <param name="id">Id da entidade</param>
        public void delete(long id)
        {
            try
            {
                var entity = getInstanceById(id);

                if (entity != null)
                {
                    context.BaseForms.Remove(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.delete", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.delete - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por Id
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public BaseForm getInstanceById(long id)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                var baseForm = context.BaseForms
                    .Include("BaseBlocks")
                    .Include("BaseBlocks.BaseSubBlocks")
                    .Include("Period")
                    .Where(e => e.Id == id).FirstOrDefault();

                baseForm = loadQuestions(baseForm);

                return orderBaseForm(baseForm);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.getInstanceById", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.getInstanceById - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por Id do bloco
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public BaseForm getInstanceByBlockId(long blockId)
        {
            try
            {
                var baseFormId = context.BaseBlocks.Where(bb => bb.Id == blockId).Select(bb => bb.BaseFormId).FirstOrDefault();

                if (baseFormId > 0)
                {
                    //Buscando uma determinada entidade e seus relacionamentos
                    var baseForm = context.BaseForms.Include("BaseBlocks")
                    .Include("BaseBlocks.BaseSubBlocks")
                    .Include("Period").Where(e => e.Id == baseFormId).FirstOrDefault();

                    baseForm = loadQuestions(baseForm);

                    return orderBaseForm(baseForm);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.getInstanceByBlockId", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.getInstanceByBlockId - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por Id do sub-bloco
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public BaseForm getInstanceBySubBlockId(long subblockId)
        {
            try
            {
                var baseFormId = context.BaseSubBlocks.Include("BaseBlock").Where(bsb => bsb.Id == subblockId).Select(bsb => bsb.BaseBlock.BaseFormId).FirstOrDefault();

                if (baseFormId > 0)
                {
                    //Buscando uma determinada entidade e seus relacionamentos
                    var baseForm = context.BaseForms.Include("BaseBlocks")
                    .Include("BaseBlocks.BaseSubBlocks")
                    .Include("Period").Where(e => e.Id == baseFormId).FirstOrDefault();

                    baseForm = loadQuestions(baseForm);

                    return orderBaseForm(baseForm);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.getInstanceBySubBlockId", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.getInstanceBySubBlockId - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por Id da questão
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public BaseForm getInstanceByQuestionId(long questionId)
        {
            try
            {
                var baseFormId = context.BaseQuestions
                    .Include("BaseBlock")
                    .Include("BaseBlock.BaseSubBlocks")
                    .Where(bq => bq.Id == questionId).Select(bb => bb.BaseSubBlock.BaseBlock.BaseFormId).FirstOrDefault();

                if (baseFormId > 0)
                {
                    //Buscando uma determinada entidade e seus relacionamentos
                    var baseForm = context.BaseForms.Include("BaseBlocks")
                                            .Include("BaseBlocks.BaseSubBlocks")
                                            .Include("Period").Where(e => e.Id == baseFormId).FirstOrDefault();

                    baseForm = loadQuestions(baseForm);

                    return orderBaseForm(baseForm);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.getInstanceByQuestionId", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.getInstanceByQuestionId - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por data de periodo
        /// </summary>
        /// <param name="date">Data do periodo</param>
        /// <returns></returns>
        public BaseForm getInstanceByPeriodDate(DateTime date)
        {
            try
            {
                var baseForm = (from bf in context.BaseForms.Include("BaseBlocks")
                                            .Include("BaseBlocks.BaseSubBlocks").Include("Period")
                                where bf.Period.InitialDate <= date && bf.Period.FinalDate >= date
                                select bf).FirstOrDefault();

                baseForm = loadQuestions(baseForm);

                return orderBaseForm(baseForm);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.getInstanceByPeriodDate", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.getInstanceByPeriodDate - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por data de periodo
        /// </summary>
        /// <param name="date">Data do periodo</param>
        /// <returns></returns>
        public BaseForm getInstanceByPeriodId(long id)
        {
            try
            {
                var baseForm = (from bf in context.BaseForms.Include("BaseBlocks")
                                    .Include("BaseBlocks.BaseSubBlocks").Include("Period")
                                where bf.Period.Id == id
                                select bf).FirstOrDefault();

                baseForm = loadQuestions(baseForm);

                return orderBaseForm(baseForm);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.getInstanceByPeriodId", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.getInstanceByPeriodId - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca todos as entidades
        /// </summary>
        /// <returns></returns>
        public List<BaseForm> getAll()
        {
            try
            {
                return context.BaseForms.ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.getAll", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.getAll - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Seleciona uma lista de entidades
        /// </summary>
        /// <param name="where">Condição para o select</param>
        /// <returns>Lista de entidades</returns>
        public List<BaseForm> selectWhere(Func<BaseForm, bool> where)
        {
            try
            {
                return context.BaseForms.Include("BaseBlocks").Include("BaseBlocks.BaseSubBlocks").Include("Period").Where<BaseForm>(where).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.selectWhere - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Seleciona uma lista de entidades, informando o limite de resultados e a partir de qual resultado.
        /// </summary>
        /// <param name="where">Condição para o select</param>
        /// <param name="maximumRows">Número máximo de retorno</param>
        /// <param name="startRowIndex">Inicio do objeto para retornar</param>
        /// <returns></returns>
        public List<BaseForm> selectWhere(Func<BaseForm, bool> where, int maximumRows, int startRowIndex)
        {
            try
            {
                return context.BaseForms.Include("BaseBlocks").Include("BaseBlocks.BaseSubBlocks").Include("Period").Where(where).Skip<BaseForm>(startRowIndex).Take(maximumRows).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.selectWhere - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca uma questão pelo id
        /// </summary>
        /// <returns></returns>
        public BaseQuestion getBaseQuestionById(long id)
        {
            try
            {
                return context.BaseQuestions.Where(f => f.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.getBaseQuestionById", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.getBaseQuestionById - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Retorna o formulário para download de acordo com o tipo de usuário.
        /// </summary>
        /// <returns></returns>
        public BaseForm getInstanceByUserType()
        {
            try
            {
                using (Lib.Repositories.PeriodRepository repository = new PeriodRepository(this.ActiveUser))
                {
                    var lastPeriod = repository.getLastPublishedPeriod();

                    if (this.ActiveUser.UserTypeEnum == Enumerations.UserType.Entity && this.ActiveUser.Groups != null && this.ActiveUser.Groups.Count > 0)
                    {
                        var period = repository.getPeriodOpen();

                        if (period != null)
                        {
                            var baseForm = getInstanceByPeriodId(period.Id);

                            if (baseForm != null)
                            {
                                return baseForm;
                            }
                        }
                    }

                    if (lastPeriod != null)
                    {
                        return context.BaseForms.Where(f => f.PeriodId == lastPeriod.Id).FirstOrDefault();
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.BaseFormRepository.getInstanceByUserType", ex);
                throw new Exception("Lib.Repositories.BaseFormRepository.getInstanceByUserType - " + ex.Message, ex);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            context.Dispose();
        }

        #endregion

        #region [Private Methods]

        private BaseForm orderBaseForm(BaseForm baseForm)
        {
            if (baseForm != null)
            {
                if (baseForm.BaseBlocks != null)
                {
                    //Orderna os blocos por indice
                    baseForm.BaseBlocks = baseForm.BaseBlocks.OrderBy(bb => bb.Id).ToList();

                    //Orderna as perguntas por indice
                    baseForm.BaseBlocks.ForEach(bb =>
                    {
                        if (bb.BaseSubBlocks != null)
                        {
                            bb.BaseSubBlocks = bb.BaseSubBlocks.OrderBy(bq => bq.Index).ToList();

                            bb.BaseSubBlocks.ForEach(bsb =>
                            {
                                if (bsb.BaseQuestions != null)
                                {
                                    bsb.BaseQuestions = bsb.BaseQuestions.OrderBy(f => f.Index).ToList();
                                }
                            });
                        }
                    });
                }
            }
            return baseForm;
        }

        private BaseForm loadQuestions(BaseForm baseForm)
        {
            if (baseForm != null)
            {
                baseForm.BaseBlocks.ForEach(bb =>
                {
                    if (bb.BaseSubBlocks != null)
                    {
                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            bsb.BaseQuestions = context.BaseQuestions.Include("Answers").Where(bq => bq.BaseSubBlockId == bsb.Id).ToList();
                        });
                    }
                });
            }

            return baseForm;
        }
        #endregion

        /// <summary>
        /// Copia um determinado formulário para um determinado periodo
        /// </summary>
        /// <param name="formId">Formulário a ser copiado</param>
        /// <param name="periodId">Periodo a qual deve ser copiado o formulário</param>
        /// <returns>TRUE caso o formulário foi copiado com sucesso</returns>
        public bool copyFormToPeriod(long formId, long periodId)
        {
            var copied = false;
            Lib.Entities.BaseForm newForm = new BaseForm();
            

            //Recupera a instancia do formulário
            Lib.Entities.BaseForm copyForm = getInstanceById(formId);

            if (copyForm != null)
            {
                newForm.PeriodId = periodId;
                newForm.Name = copyForm.Name;
                newForm.BaseBlocks = new List<BaseBlock>();
                
                copyForm.BaseBlocks.ForEach(bb =>
                {
                    var newBaseBlock = new BaseBlock();
                    newBaseBlock.Name = bb.Name;

                    newBaseBlock.BaseSubBlocks = new List<BaseSubBlock>();
                    bb.BaseSubBlocks.ForEach(bsb =>
                    {
                        var newBaseSubBlock = new BaseSubBlock();
                        newBaseSubBlock.Index = bsb.Index;
                        newBaseSubBlock.Name = bsb.Name;
                        newBaseSubBlock.Weight = bsb.Weight;
                        newBaseSubBlock.BaseQuestions = new List<BaseQuestion>();
                        bsb.BaseQuestions.ForEach(bq =>
                        {
                            var newBaseQuestions = new BaseQuestion();
                            newBaseQuestions.Index = bq.Index;
                            newBaseQuestions.Question = bq.Question;
                            newBaseQuestions.Tip = bq.Tip;
                            newBaseQuestions.Value = bq.Value;

                            newBaseSubBlock.BaseQuestions.Add(newBaseQuestions);
                        });
                        newBaseBlock.BaseSubBlocks.Add(newBaseSubBlock);
                    });

                    newForm.BaseBlocks.Add(newBaseBlock);
                });

                save(newForm);

                if (!HasErrors)
                {
                    copied = true;
                }
            }

            return copied;
        }
    }
}
