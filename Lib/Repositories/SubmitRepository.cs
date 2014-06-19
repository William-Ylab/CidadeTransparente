using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Repositories
{
    public class SubmitRepository : Abstract.AbstractRepository, IDisposable
    {
        private FDTContext context;
        private Lib.Entities.User ActiveUser;

        #region [Constructors]

        private SubmitRepository()
        {
            //this.context = context;
            context = new FDTContext();
        }

        public SubmitRepository(Lib.Entities.User activeUser)
        {
            this.ActiveUser = activeUser;
            context = new FDTContext();
        }

        #endregion

        #region IRepository<Entity> Members

        /// <summary>
        /// Salva uma entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void save(Entities.Submit entity)
        {
            //List<Commons.LogHistory> logs = null;
            Lib.Repositories.LogHistoryRepository log = new LogHistoryRepository(this.ActiveUser);

            try
            {
                context = new FDTContext();
                if (entity.Id == 0)
                {
                    entity.Date = DateTime.Now;
                    context.Submits.Add(entity);
                }
                else
                {
                    var entityAux = context.Submits.Where(e => e.Id == entity.Id).FirstOrDefault();

                    //salva o log.
                    //logs = log.getLogsHistory(entityAux, entity, ActiveUser, "Id", "Name", Lib.Enumerations.EntityType.Entity);

                    //entityAux.Date = DateTime.Now;
                    entityAux.Observation = entity.Observation;
                    entityAux.Status = entity.Status;
                }

                context.SaveChanges();

                //if (logs != null)
                //{
                //    log.saveLogs(logs);
                //}
            }
            catch (DbEntityValidationException ex)
            {
                //Adiciona na lista de erros os erros de DataAnnotation
                addErrorMessage(ex);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.SubmitRepository.save", ex);
                throw new Exception("Lib.Repositories.SubmitRepository.save - " + ex.Message, ex);
            }
        }

        public void delete(Entities.Submit entity)
        {
            try
            {
                if (entity != null)
                {
                    delete(entity.Id);
                }
            }
            catch (DbEntityValidationException ex)
            {
                //Adiciona na lista de erros os erros de DataAnnotation
                addErrorMessage(ex);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.SubmitRepository.delete", ex);
                throw new Exception("Lib.Repositories.SubmitRepository.delete - " + ex.Message, ex);
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
                    context.Submits.Remove(entity);

                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                //Adiciona na lista de erros os erros de DataAnnotation
                addErrorMessage(ex);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.SubmitRepository.delete", ex);
                throw new Exception("Lib.Repositories.SubmitRepository.delete - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca a entidade por Id
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public Entities.Submit getInstanceById(long id)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                return context.Submits.Where(e => e.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.SubmitRepository.getInstanceById", ex);
                throw new Exception("Lib.Repositories.SubmitRepository.getInstanceById - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Retorna todas as entidades
        /// </summary>
        public List<Entities.Submit> getAll()
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                return context.Submits.OrderBy(f => f.Date).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.SubmitRepository.getAll", ex);
                throw new Exception("Lib.Repositories.SubmitRepository.getAll - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Seleciona uma lista de entidades
        /// </summary>
        /// <param name="where">Condição para o select</param>
        /// <returns>Lista de entidades</returns>
        public List<Entities.Submit> selectWhere(Func<Entities.Submit, bool> where)
        {
            try
            {
                return context.Submits.Where<Entities.Submit>(where).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.SubmitRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.SubmitRepository.selectWhere - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Seleciona uma lista de entidades, informando o limite de resultados e a partir de qual resultado.
        /// </summary>
        /// <param name="where">Condição para o select</param>
        /// <param name="maximumRows">Número máximo de retorno</param>
        /// <param name="startRowIndex">Inicio do objeto para retornar</param>
        /// <returns></returns>
        public List<Entities.Submit> selectWhere(Func<Entities.Submit, bool> where, int maximumRows, int startRowIndex)
        {
            try
            {
                return context.Submits.Where(where).Skip<Entities.Submit>(startRowIndex).Take(maximumRows).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.SubmitRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.SubmitRepository.selectWhere - " + ex.Message, ex);
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
