using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entities;

namespace Lib.Repositories
{
    public class ResponseFormRepository : Abstract.AbstractRepository, IDisposable
    {
        private FDTContext context;
        private Lib.Entities.User ActiveUser;

        #region [Constructors]

        private ResponseFormRepository()
        {
            //this.context = context;
            context = new FDTContext();
        }

        public ResponseFormRepository(Lib.Entities.User activeUser)
        {
            this.ActiveUser = activeUser;
            context = new FDTContext();
        }

        #endregion

        #region IRepository<ResponseFormRepository> Members

        /// <summary>
        /// Salva uma entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void save(ResponseForm entity, bool submitForm = true)
        {
            List<Commons.LogHistory> logs = null;
            Lib.Repositories.LogHistoryRepository log = new LogHistoryRepository(this.ActiveUser);

            try
            {
                context = new FDTContext();

                if (entity.Id == 0)
                {
                    if (this.ActiveUser.UserTypeEnum == Enumerations.UserType.Entity)
                    {
                        //Cria uma submissão com status submitted
                        if (submitForm)
                        {
                            if (entity.Submits == null)
                                entity.Submits = new List<Submit>();
                            entity.Submits.Add(new Submit()
                            {
                                Date = DateTime.Now,
                                Status = (int)Lib.Enumerations.SubmitStatus.Submitted,
                                Observation = String.Empty
                            });
                        }
                    }

                    context.ResponseForms.Add(entity);
                }
                else
                {
                    var entityAux = context.ResponseForms.Include("Answers").Include("Submits").Where(e => e.Id == entity.Id).FirstOrDefault();
                    entityAux.CityId = entity.CityId;

                    if (entityAux != null)
                    {
                        //Atualiza apenas as notas e observações das perguntas\
                        entityAux.Answers.ForEach(ans =>
                        {
                            var answerUpdated = entity.Answers.Where(an => an.Id == ans.Id).FirstOrDefault();
                            if (answerUpdated != null)
                            {
                                ans.Observation = answerUpdated.Observation;
                                ans.Score = answerUpdated.Score;
                            }
                        });
                    }

                    if (this.ActiveUser.UserTypeEnum == Enumerations.UserType.Entity)
                    {
                        //Cria uma submissão com status submitted
                        if (submitForm)
                        {
                            if (entityAux.Submits == null)
                                entityAux.Submits = new List<Submit>();
                            entityAux.Submits.Add(new Submit()
                            {
                                Date = DateTime.Now,
                                Status = (int)Lib.Enumerations.SubmitStatus.Submitted,
                                Observation = String.Empty
                            });
                        }
                    }

                    //Adiciona as novas questões se houverem
                    entityAux.Answers.AddRange(entity.Answers.Where(a => a.Id == 0).ToList());
                }

                context.SaveChanges();

                //refaz os cálculos e salva novamente.
                var rf = getInstanceById(entity.Id);
                rf.calculateTotalScore();

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
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.save", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.save - " + ex.Message, ex);
            }
        }

        public void submitForm(ResponseForm entity, string observations)
        {
            //ver o log hisotrico
            Lib.Entities.Submit submit = new Submit();
            submit.Date = DateTime.Now;
            submit.Observation = observations;
            submit.ResponseForm = entity;
            submit.Status = (int)Enumerations.SubmitStatus.NotApproved;

            context.Submits.Add(submit);
        }

        /// <summary>
        /// Deleta uma entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void delete(ResponseForm entity)
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
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.delete", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.delete - " + ex.Message, ex);
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
                    context.ResponseForms.Remove(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.delete", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.delete - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por Id
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public ResponseForm getInstanceById(long id)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                var responseForm = context.ResponseForms
                    .Include("BaseForm")
                    .Include("Answers")
                    .Include("City")
                    .Include("City.State")
                    .Include("User")
                                        .Include("Submits")
                    .Where(e => e.Id == id).FirstOrDefault();

                if (responseForm != null)
                {
                    responseForm.Reviews = context.Reviews.Where(r => r.ResponseFormId == responseForm.Id).ToList();

                    responseForm.Reviews.ForEach(r =>
                    {
                        r.User = context.Users.Where(f => f.Id == r.UserId).FirstOrDefault();
                    });

                    //carrega os baseblocks
                    responseForm.BaseForm.BaseBlocks = context.BaseBlocks.Where(bb => bb.BaseFormId == responseForm.BaseFormId).ToList();

                    responseForm.BaseForm.BaseBlocks.ForEach(bb =>
                    {
                        //carrega os basesubblocks
                        bb.BaseSubBlocks = context.BaseSubBlocks.Where(f => f.BaseBlockId == bb.Id).ToList();

                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            //carrega os basequestions
                            bsb.BaseQuestions = context.BaseQuestions.Where(bq => bq.BaseSubBlockId == bsb.Id).ToList();
                        });
                    });



                    responseForm.Answers = responseForm.Answers.OrderBy(ans => ans.BaseQuestion.BaseSubBlock.Index).ToList();
                }

                return responseForm;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.getInstanceById", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.getInstanceById - " + ex.Message, ex);
            }

        }

        ///// <summary>
        ///// Busca a entidade por data de periodo
        ///// </summary>
        ///// <param name="date">Data do periodo</param>
        ///// <returns></returns>
        //public ResponseForm getInstanceByPeriodDate(DateTime date)
        //{
        //    try
        //    {
        //        var ResponseForm = (from bf in context.ResponseForms.Include("BaseBlocks").Include("BaseBlocks.BaseQuestions").Include("Period")
        //                        where bf.Period.InitialDate <= date && bf.Period.FinalDate >= date
        //                        select bf).FirstOrDefault();

        //        return ResponseForm;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.getInstanceByPeriodDate", ex);
        //        throw new Exception("Lib.Repositories.ResponseFormRepository.getInstanceByPeriodDate - " + ex.Message, ex);
        //    }

        //}

        ///// <summary>
        ///// Busca a entidade por data de periodo
        ///// </summary>
        ///// <param name="date">Data do periodo</param>
        ///// <returns></returns>
        //public ResponseForm getInstanceByPeriodId(long id)
        //{
        //    try
        //    {
        //        var ResponseForm = (from bf in context.ResponseForms.Include("BaseBlocks").Include("BaseBlocks.BaseQuestions").Include("Period")
        //                        where bf.Period.Id == id
        //                        select bf).FirstOrDefault();

        //        return ResponseForm;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.getInstanceByPeriodId", ex);
        //        throw new Exception("Lib.Repositories.ResponseFormRepository.getInstanceByPeriodId - " + ex.Message, ex);
        //    }

        //}

        /// <summary>
        /// Busca todos as entidades
        /// </summary>
        /// <returns></returns>
        public List<ResponseForm> getAll()
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                var responseForms = context.ResponseForms
                    .Include("BaseForm")
                    .Include("Answers")
                    .Include("City")
                    .Include("City.State")
                    .Include("Submits")
                    .Include("User").ToList();

                responseForms.ForEach(responseForm =>
                {
                    responseForm.Reviews = context.Reviews.Where(r => r.ResponseFormId == responseForm.Id).ToList();

                    responseForm.Reviews.ForEach(r =>
                    {
                        r.User = context.Users.Where(f => f.Id == r.UserId).FirstOrDefault();
                    });

                    //carrega os baseblocks
                    responseForm.BaseForm.BaseBlocks = context.BaseBlocks.Where(bb => bb.BaseFormId == responseForm.BaseFormId).ToList();

                    responseForm.BaseForm.BaseBlocks.ForEach(bb =>
                    {
                        //carrega os basesubblocks
                        bb.BaseSubBlocks = context.BaseSubBlocks.Where(f => f.BaseBlockId == bb.Id).ToList();

                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            //carrega os basequestions
                            bsb.BaseQuestions = context.BaseQuestions.Where(bq => bq.BaseSubBlockId == bsb.Id).ToList();
                        });
                    });



                    responseForm.Answers = responseForm.Answers.OrderBy(ans => ans.BaseQuestion.BaseSubBlock.Index).ToList();
                });

                return responseForms;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.getAll", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.getAll - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca todos as entidades do periodo atual ou do ultimo periodo
        /// </summary>
        /// <returns></returns>
        public List<ResponseForm> getActualOrLastPeriodResponseForms()
        {
            try
            {
                Entities.Period period = null;
                using (Lib.Repositories.PeriodRepository rep = new PeriodRepository(this.ActiveUser))
                {
                    period = rep.getLastPeriod(DateTime.Now);
                }

                if (period == null)
                {
                    return null;
                }

                List<long> baseFormsIds = null;
                baseFormsIds = period.BaseForms.Select(f => f.Id).ToList();

                //Buscando uma determinada entidade e seus relacionamentos
                var responseForms = context.ResponseForms
                    .Include("BaseForm")
                    .Include("Answers")
                    .Include("City")
                    .Include("City.State")
                    .Include("User")
                    .Include("Submits")
                    .Where(b => baseFormsIds.Contains(b.BaseFormId)).ToList();
                //


                responseForms.ForEach(responseForm =>
                {
                    responseForm.Reviews = context.Reviews.Where(r => r.ResponseFormId == responseForm.Id).ToList();

                    responseForm.Reviews.ForEach(r =>
                    {
                        r.User = context.Users.Where(f => f.Id == r.UserId).FirstOrDefault();
                    });

                    //carrega os baseblocks
                    responseForm.BaseForm.BaseBlocks = context.BaseBlocks.Where(bb => bb.BaseFormId == responseForm.BaseFormId).ToList();

                    responseForm.BaseForm.BaseBlocks.ForEach(bb =>
                    {
                        //carrega os basesubblocks
                        bb.BaseSubBlocks = context.BaseSubBlocks.Where(f => f.BaseBlockId == bb.Id).ToList();

                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            //carrega os basequestions
                            bsb.BaseQuestions = context.BaseQuestions.Where(bq => bq.BaseSubBlockId == bsb.Id).ToList();
                        });
                    });



                    responseForm.Answers = responseForm.Answers.OrderBy(ans => ans.BaseQuestion.BaseSubBlock.Index).ToList();
                });

                return responseForms;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.getAll", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.getAll - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca todos as entidades do periodo escolhido
        /// </summary>
        /// <returns></returns>
        public List<ResponseForm> getResponseFormsByPeriod(long periodId)
        {
            try
            {
                Entities.Period period = null;
                using (Lib.Repositories.PeriodRepository rep = new PeriodRepository(this.ActiveUser))
                {
                    period = rep.getInstanceById(periodId);
                }

                if (period == null)
                {
                    return null;
                }

                List<long> baseFormsIds = null;
                baseFormsIds = period.BaseForms.Select(f => f.Id).ToList();

                //Buscando uma determinada entidade e seus relacionamentos
                var responseForms = context.ResponseForms
                    .Include("BaseForm")
                    .Include("Answers")
                    .Include("City")
                    .Include("City.State")
                    .Include("User")
                    .Include("Submits")
                    .Where(b => baseFormsIds.Contains(b.BaseFormId)).ToList();
                //


                responseForms.ForEach(responseForm =>
                {
                    responseForm.Reviews = context.Reviews.Where(r => r.ResponseFormId == responseForm.Id).ToList();

                    responseForm.Reviews.ForEach(r =>
                    {
                        r.User = context.Users.Where(f => f.Id == r.UserId).FirstOrDefault();
                    });

                    //carrega os baseblocks
                    responseForm.BaseForm.BaseBlocks = context.BaseBlocks.Where(bb => bb.BaseFormId == responseForm.BaseFormId).ToList();

                    responseForm.BaseForm.BaseBlocks.ForEach(bb =>
                    {
                        //carrega os basesubblocks
                        bb.BaseSubBlocks = context.BaseSubBlocks.Where(f => f.BaseBlockId == bb.Id).ToList();

                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            //carrega os basequestions
                            bsb.BaseQuestions = context.BaseQuestions.Where(bq => bq.BaseSubBlockId == bsb.Id).ToList();
                        });
                    });



                    responseForm.Answers = responseForm.Answers.OrderBy(ans => ans.BaseQuestion.BaseSubBlock.Index).ToList();
                });

                return responseForms;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.getAll", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.getAll - " + ex.Message, ex);
            }

        }

        public List<ResponseForm> getResponseFormsByUserId(long userId, bool collabForms = false)
        {
            try
            {
                List<ResponseForm> responseForms = new List<ResponseForm>();
                //Recupera os grupos
                User user = null;
                using (Lib.Repositories.UserRepository repUser = new UserRepository(this.ActiveUser))
                {
                    user = repUser.getInstanceById(userId);
                }

                //Buscando os questionários que o usuário preencheu
                responseForms.AddRange(context.ResponseForms
                    .Include("BaseForm")
                    .Include("BaseForm.Period")
                    .Include("Answers")
                    .Include("City")
                    .Include("City.State")
                    .Include("User")
                    .Include("Submits")
                    .Where(b => b.UserId == userId).ToList());

                //Buscando os formulários que a entidade é colaboradora
                //Para cada grupo que o usuário é colaborador, busca os questionários do periodo e usuário responsável preencheu
                foreach (var group in user.Groups)
                {
                    var forms = context.ResponseForms
                        .Include("BaseForm")
                        .Include("BaseForm.Period")
                        .Include("Answers")
                        .Include("City")
                        .Include("City.State")
                        .Include("User")
                        .Include("Submits")
                        .Where(b => b.UserId == group.ResponsableId && b.BaseForm.PeriodId == group.PeriodId && (b.CityId == group.CityId || b.CityId == null)).ToList();

                    forms.ForEach(f =>
                    {
                        if (!responseForms.Select(c => c.Id).ToList().Contains(f.Id))
                        {
                            responseForms.Add(f);
                        }
                    });
                }


                responseForms.ForEach(responseForm =>
                {
                    responseForm.Reviews = context.Reviews.Where(r => r.ResponseFormId == responseForm.Id).ToList();

                    responseForm.Reviews.ForEach(r =>
                    {
                        r.User = context.Users.Where(f => f.Id == r.UserId).FirstOrDefault();
                    });

                    //carrega os baseblocks
                    responseForm.BaseForm.BaseBlocks = context.BaseBlocks.Where(bb => bb.BaseFormId == responseForm.BaseFormId).ToList();

                    responseForm.BaseForm.BaseBlocks.ForEach(bb =>
                    {
                        //carrega os basesubblocks
                        bb.BaseSubBlocks = context.BaseSubBlocks.Where(f => f.BaseBlockId == bb.Id).ToList();

                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            //carrega os basequestions
                            bsb.BaseQuestions = context.BaseQuestions.Where(bq => bq.BaseSubBlockId == bsb.Id).ToList();
                        });
                    });



                    responseForm.Answers = responseForm.Answers.OrderBy(ans => ans.BaseQuestion.BaseSubBlock.Index).ToList();
                });

                return responseForms;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.getAll", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.getAll - " + ex.Message, ex);
            }

        }

        public List<ResponseForm> getResponseFormsByUserIdAndPeriodId(long userId, long periodId)
        {
            try
            {
                Entities.Period period = null;
                using (Lib.Repositories.PeriodRepository rep = new PeriodRepository(this.ActiveUser))
                {
                    period = rep.getInstanceById(periodId);
                }

                if (period == null)
                {
                    return null;
                }

                List<long> baseFormsIds = null;
                baseFormsIds = period.BaseForms.Select(f => f.Id).ToList();

                //Buscando uma determinada entidade e seus relacionamentos
                var responseForms = context.ResponseForms
                    .Include("BaseForm")
                    .Include("BaseForm.Period")
                    .Include("Answers")
                    .Include("City")
                    .Include("City.State")
                    .Include("User")
                    .Include("Submits")
                    .Where(b => b.UserId == userId && baseFormsIds.Contains(b.BaseFormId)).ToList();
                //


                responseForms.ForEach(responseForm =>
                {
                    responseForm.Reviews = context.Reviews.Where(r => r.ResponseFormId == responseForm.Id).ToList();

                    responseForm.Reviews.ForEach(r =>
                    {
                        r.User = context.Users.Where(f => f.Id == r.UserId).FirstOrDefault();
                    });

                    //carrega os baseblocks
                    responseForm.BaseForm.BaseBlocks = context.BaseBlocks.Where(bb => bb.BaseFormId == responseForm.BaseFormId).ToList();

                    responseForm.BaseForm.BaseBlocks.ForEach(bb =>
                    {
                        //carrega os basesubblocks
                        bb.BaseSubBlocks = context.BaseSubBlocks.Where(f => f.BaseBlockId == bb.Id).ToList();

                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            //carrega os basequestions
                            bsb.BaseQuestions = context.BaseQuestions.Where(bq => bq.BaseSubBlockId == bsb.Id).ToList();
                        });
                    });



                    responseForm.Answers = responseForm.Answers.OrderBy(ans => ans.BaseQuestion.BaseSubBlock.Index).ToList();
                });

                return responseForms;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.getAll", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.getAll - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Seleciona uma lista de entidades
        /// </summary>
        /// <param name="where">Condição para o select</param>
        /// <returns>Lista de entidades</returns>
        public List<ResponseForm> selectWhere(Func<ResponseForm, bool> where)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                var responseForms = context.ResponseForms
                    .Include("BaseForm")
                    .Include("Answers")
                    .Include("City")
                    .Include("City.State")
                    .Include("Submits")
                    .Include("User")
                    .Where<ResponseForm>(where)
                    .ToList();

                responseForms.ForEach(responseForm =>
                {
                    responseForm.Reviews = context.Reviews.Where(r => r.ResponseFormId == responseForm.Id).ToList();

                    responseForm.Reviews.ForEach(r =>
                    {
                        r.User = context.Users.Where(f => f.Id == r.UserId).FirstOrDefault();
                    });

                    //carrega os baseblocks
                    responseForm.BaseForm.BaseBlocks = context.BaseBlocks.Where(bb => bb.BaseFormId == responseForm.BaseFormId).ToList();

                    responseForm.BaseForm.BaseBlocks.ForEach(bb =>
                    {
                        //carrega os basesubblocks
                        bb.BaseSubBlocks = context.BaseSubBlocks.Where(f => f.BaseBlockId == bb.Id).ToList();

                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            //carrega os basequestions
                            bsb.BaseQuestions = context.BaseQuestions.Where(bq => bq.BaseSubBlockId == bsb.Id).ToList();
                        });
                    });



                    responseForm.Answers = responseForm.Answers.OrderBy(ans => ans.BaseQuestion.BaseSubBlock.Index).ToList();
                });

                return responseForms;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.selectWhere - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Seleciona uma lista de entidades, informando o limite de resultados e a partir de qual resultado.
        /// </summary>
        /// <param name="where">Condição para o select</param>
        /// <param name="maximumRows">Número máximo de retorno</param>
        /// <param name="startRowIndex">Inicio do objeto para retornar</param>
        /// <returns></returns>
        public List<ResponseForm> selectWhere(Func<ResponseForm, bool> where, int maximumRows, int startRowIndex)
        {
            try
            {
                return context.ResponseForms.Include("BaseBlocks").Include("BaseBlocks.BaseQuestions").Include("Period").Where(where).Skip<ResponseForm>(startRowIndex).Take(maximumRows).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.selectWhere - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Adiciona uma review ao responseForm
        /// </summary>
        /// <param name="review"></param>
        /// <param name="responseFormId"></param>
        public void addReview(Review review)
        {
            if (review != null && review.ResponseFormId > 0)
            {
                review.Date = DateTime.Now;

                context.Reviews.Add(review);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Busca todos as entidades do ultimo periodo publicado e que possuam o ultimo submit como accepted
        /// </summary>
        /// <returns></returns>
        public List<ResponseForm> getRanking()
        {
            try
            {
                Lib.Entities.Period period = null;
                List<Lib.Entities.ResponseForm> ranking = new List<ResponseForm>();

                using (Lib.Repositories.PeriodRepository repository = new PeriodRepository(this.ActiveUser))
                {
                    period = repository.getLastPublishedPeriod();
                }

                if (period == null)
                {
                    return new List<ResponseForm>();
                }

                var baseFormsIds = period.BaseForms.Select(g => g.Id).ToList();

                //Buscando uma determinada entidade e seus relacionamentos
                var responseForms = context.ResponseForms
                    .Include("BaseForm")
                    .Include("Answers")
                    .Include("City")
                    .Include("City.State")
                    .Include("Submits")
                    .Include("User")
                    .Where(f => baseFormsIds.Contains(f.BaseFormId))
                    .ToList();

                responseForms.ForEach(responseForm =>
                {
                    responseForm.Submits = context.Submits.Where(r => r.ResponseFormId == responseForm.Id).ToList();
                    var submit = responseForm.Submits.OrderBy(f => f.Id).LastOrDefault();

                    if (submit != null)
                    {
                        if (submit.StatusEnum == Enumerations.SubmitStatus.Approved)
                        {
                            ranking.Add(responseForm);
                        }
                    }
                });

                ranking.ForEach(responseForm =>
                {
                    responseForm.Reviews = context.Reviews.Where(r => r.ResponseFormId == responseForm.Id).ToList();

                    responseForm.Reviews.ForEach(r =>
                    {
                        r.User = context.Users.Where(f => f.Id == r.UserId).FirstOrDefault();
                    });

                    //carrega os baseblocks
                    responseForm.BaseForm.BaseBlocks = context.BaseBlocks.Where(bb => bb.BaseFormId == responseForm.BaseFormId).ToList();

                    responseForm.BaseForm.BaseBlocks.ForEach(bb =>
                    {
                        //carrega os basesubblocks
                        bb.BaseSubBlocks = context.BaseSubBlocks.Where(f => f.BaseBlockId == bb.Id).ToList();

                        bb.BaseSubBlocks.ForEach(bsb =>
                        {
                            //carrega os basequestions
                            bsb.BaseQuestions = context.BaseQuestions.Where(bq => bq.BaseSubBlockId == bsb.Id).ToList();
                        });
                    });



                    responseForm.Answers = responseForm.Answers.OrderBy(ans => ans.BaseQuestion.BaseSubBlock.Index).ToList();
                });

                return ranking;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.ResponseFormRepository.getRanking", ex);
                throw new Exception("Lib.Repositories.ResponseFormRepository.getRanking - " + ex.Message, ex);
            }

        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            context.Dispose();
        }

        #endregion
    }
}
