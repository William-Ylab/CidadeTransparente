using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entities;

namespace Lib.Repositories
{
    public class PeriodRepository : Abstract.AbstractRepository, IDisposable
    {
        private FDTContext context;
        private Lib.Entities.User ActiveUser;

        #region [Constructors]

        private PeriodRepository()
        {
            //this.context = context;
            context = new FDTContext();
        }

        public PeriodRepository(Lib.Entities.User activeUser)
        {
            this.ActiveUser = activeUser;
            context = new FDTContext();
        }

        #endregion

        #region IRepository<PeriodRepository> Members

        /// <summary>
        /// Salva uma entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void save(Period entity)
        {
            List<Commons.LogHistory> logs = null;
            Lib.Repositories.LogHistoryRepository log = new LogHistoryRepository(this.ActiveUser);

            try
            {
                context = new FDTContext();

                validatePeriod(entity);

                if (entity.Id == 0)
                {
                    context.Periods.Add(entity);
                }
                else
                {
                    //Recuperando o questionário do banco
                    var period = context.Periods.Where(e => e.Id == entity.Id).FirstOrDefault();

                    //salva o log.
                    //logs = log.getLogsHistory(period, entity, ActiveUser, "Id", "Name", Enumerations.EntityType.Period);
                    period.Name = entity.Name;
                    period.FinalDate = entity.FinalDate;
                    period.InitialDate = entity.InitialDate;
                    period.ConvocationInitialDate = entity.ConvocationInitialDate;
                    period.ConvocationFinalDate = entity.ConvocationFinalDate;
                    period.Open = entity.Open;
                    period.Published = entity.Published;
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
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.save", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.save - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Deleta uma entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void delete(Period entity)
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
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.delete", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.delete - " + ex.Message, ex);
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
                    context.Periods.Remove(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.delete", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.delete - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por Id
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public Period getInstanceById(long id)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                return context.Periods.Include("BaseForms").Where(e => e.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.getInstanceById", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.getInstanceById - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por Data
        /// </summary>
        /// <param name="id">Data do periodo</param>
        /// <returns></returns>
        public Period getInstanceByDate(DateTime date)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                return context.Periods.Include("BaseForms").Where(e => e.InitialDate <= date && e.FinalDate >= date).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.getInstanceByDate", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.getInstanceByDate - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca o ultimo periodo dada uma datafinal
        /// </summary>
        /// <param name="id">Data do periodo</param>
        /// <returns></returns>
        public Period getLastPeriod(DateTime date)
        {
            try
            {
                var period = context.Periods.Include("BaseForms").Where(e => date >= e.InitialDate && date <= e.FinalDate).FirstOrDefault();
                if (period == null)
                {
                    //Buscando uma determinada entidade e seus relacionamentos
                    period = context.Periods.Include("BaseForms").OrderByDescending(f => f.FinalDate).FirstOrDefault();
                }

                return period;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.getLastPeriod", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.getLastPeriod - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca o período aberto
        /// </summary>
        /// <returns></returns>
        public Period getPeriodOpen()
        {
            try
            {
                Period period = null;

                period = context.Periods.Include("BaseForms").Where(e => e.Open == true).FirstOrDefault();

                return period;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.getPeriodOpen", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.getPeriodOpen - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Verifica se tem algum periodo aberto e hoje está dentro do período de convocação
        /// </summary>
        /// <returns></returns>
        public Period getOpenPeriodWithConvocation()
        {
            try
            {
                Period period = null;

                period = context.Periods.Where(e => e.Open == true && e.ConvocationInitialDate <= DateTime.Now && e.ConvocationFinalDate >= DateTime.Now).FirstOrDefault();

                return period;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.isThereOpenPeriodWithConvocation", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.isThereOpenPeriodWithConvocation - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Verifica se tem algum periodo aberto e hoje está dentro do período de submissão
        /// </summary>
        /// <returns></returns>
        public Period getOpenPeriodWithSubmission()
        {
            try
            {
                Period period = null;

                period = context.Periods.Where(e => e.Open == true && e.InitialDate <= DateTime.Now && e.FinalDate >= DateTime.Now).FirstOrDefault();

                return period;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.isThereOpenPeriodWithSubmission", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.isThereOpenPeriodWithSubmission - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca todos as entidades
        /// </summary>
        /// <returns></returns>
        public List<Period> getAll()
        {
            try
            {
                return context.Periods.ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.getAll", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.getAll - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca todos as entidades de periodo que um usuario possui questionario
        /// </summary>
        /// <returns></returns>
        public List<Period> getByUserId(long userId)
        {
            try
            {
                var baseforms = context.ResponseForms.Where(f => f.UserId == userId).Select(f => f.BaseForm).ToList();

                var periodsIds = baseforms.Select(f => f.PeriodId).Distinct().ToList();


                return context.Periods.Where(f => periodsIds.Contains(f.Id)).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.getAll", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.getAll - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Seleciona uma lista de entidades
        /// </summary>
        /// <param name="where">Condição para o select</param>
        /// <returns>Lista de entidades</returns>
        public List<Period> selectWhere(Func<Period, bool> where)
        {
            try
            {
                var periods = context.Periods.Include("BaseForms").Where<Period>(where).ToList();

                periods.ForEach(p =>
                {
                    p.BaseForms.ForEach(bf =>
                    {
                        bf.ResponseForms = context.ResponseForms.Where(rf => rf.BaseFormId == bf.Id).ToList();

                        bf.ResponseForms.ForEach(rf =>
                        {
                            rf.Submits = context.Submits.Where(s => s.ResponseFormId == rf.Id).ToList();
                        });
                    });
                });


                return periods;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.selectWhere - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Seleciona uma lista de entidades, informando o limite de resultados e a partir de qual resultado.
        /// </summary>
        /// <param name="where">Condição para o select</param>
        /// <param name="maximumRows">Número máximo de retorno</param>
        /// <param name="startRowIndex">Inicio do objeto para retornar</param>
        /// <returns></returns>
        public List<Period> selectWhere(Func<Period, bool> where, int maximumRows, int startRowIndex)
        {
            try
            {
                return context.Periods.Include("BaseForms").Where(where).Skip<Period>(startRowIndex).Take(maximumRows).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.selectWhere - " + ex.Message, ex);
            }

        }

        public void validatePeriod(Period entity)
        {

            /*
             * 
             * |---------| |------------------||------|
            */

            //Verifica se a data inicial da convocação é maior que a data final de convocação
            if (entity.ConvocationInitialDate > entity.ConvocationFinalDate)
                throw new DbEntityValidationException(Resources.Messages.PERIOD_CONVOCATION_INITIAL_DATE_MORETHAN_FINAL_DATE);

            //Verifica se a data inicial é maior que a data final
            if (entity.InitialDate > entity.FinalDate)
                throw new DbEntityValidationException(Resources.Messages.PERIOD_INITIAL_DATE_MORETHAN_FINAL_DATE);

            if(entity.ConvocationFinalDate > entity.InitialDate)
                throw new DbEntityValidationException(Resources.Messages.PERIOD_CONVOCATION_MUST_BEFORE_SUBMISSION);

            if (entity.Id == 0)
            {
                //Verifica se existe algum periodo com data maior que a data inicial do novo periodo
                if (context.Periods.Where(p => p.FinalDate >= entity.InitialDate).Count() > 0)
                    throw new DbEntityValidationException(Resources.Messages.PERIOD_INVALID_INITIAL_DATE);
            }
            else if (entity.Id > 0) //Verifica se já está cadastrado o periodo
            {
                //Verifica se existe Datas menores que a data inicial do periodo a ser salvo
                var periods = context.Periods.Where(p => p.InitialDate <= entity.InitialDate && entity.Id != p.Id).ToList();

                //Para periodo retornado, verifica se a data final do periodo é menor que a data inicial do periodo a ser salvo
                periods.ForEach(p =>
                {
                    if (p.FinalDate >= entity.InitialDate)
                    {
                        //Datas se encavalam
                        throw new DbEntityValidationException(Resources.Messages.PERIOD_WITH_CONCURRENCE_DATES);
                    }
                });

                //Verifica se existe Datas maiores que a data final do periodo a ser salvo
                periods = context.Periods.Where(p => p.FinalDate >= entity.FinalDate && entity.Id != p.Id).ToList();

                //Para periodo retornado, verifica se a data inicial do periodo é maior que a data final do periodo a ser salvo
                periods.ForEach(p =>
                {
                    if (p.InitialDate <= entity.FinalDate)
                    {
                        //Datas se encavalam
                        throw new DbEntityValidationException(Resources.Messages.PERIOD_WITH_CONCURRENCE_DATES);
                    }
                });
            }

            if (entity.Open == true)
            {
                //Não pode existir nenhum outro periodo aberto
                var period = context.Periods.Where(p => p.Open && entity.Id != p.Id).FirstOrDefault();
                if (period != null)
                    throw new DbEntityValidationException(Resources.Messages.PERIOD_HAS_ANOTHER_OPEN);
            }

        }

        /// <summary>
        /// Busca o ultimo periodo dada uma datafinal
        /// </summary>
        /// <param name="id">Data do periodo</param>
        /// <returns></returns>
        public Period getLastPublishedPeriod()
        {
            try
            {
                return context.Periods.Include("BaseForms").Where(f => f.Published).OrderByDescending(f => f.FinalDate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.PeriodRepository.getLastPublishedPeriod", ex);
                throw new Exception("Lib.Repositories.PeriodRepository.getLastPublishedPeriod - " + ex.Message, ex);
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
