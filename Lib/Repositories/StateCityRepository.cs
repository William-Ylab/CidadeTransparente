using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entities;

namespace Lib.Repositories
{
    public class StateCityRepository : Abstract.AbstractRepository, IDisposable
    {
        private FDTContext context;
        private Lib.Entities.User ActiveUser;

        #region [Constructors]

        private StateCityRepository()
        {
            //this.context = context;
            context = new FDTContext();
        }

        public StateCityRepository(Lib.Entities.User activeUser)
        {
            this.ActiveUser = activeUser;
            context = new FDTContext();
        }

        #endregion

        #region IRepository<StateCityRepository> Members

        public void saveCity(City entity)
        {
            List<Commons.LogHistory> logs = null;
            Lib.Repositories.LogHistoryRepository log = new LogHistoryRepository(this.ActiveUser);

            try
            {
                context = new FDTContext();

                if (entity.Id == 0)
                {
                    context.Cities.Add(entity);
                }
                else
                {
                    //Recuperando a cidade do banco
                    var city = getCityInstanceById(entity.Id);

                    //salva o log.
                    //logs = log.getLogsHistory(period, entity, ActiveUser, "Id", "Name", Enumerations.EntityType.Period);

                    city.Name = entity.Name;
                    city.StateId = entity.StateId;
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
                Log.ErrorLog.saveError("Lib.Repositories.StateCityRepository.saveCity", ex);
                throw new Exception("Lib.Repositories.StateCityRepository.saveCity - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca a entidade por Id
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public State getStateInstanceById(string id)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                return context.States.Include("Cities").Where(e => e.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.StateCityRepository.getStateInstanceById", ex);
                throw new Exception("Lib.Repositories.StateCityRepository.getStateInstanceById - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca a entidade por Id
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public City getCityInstanceById(long id)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                var city = context.Cities.Include("State").Where(e => e.Id == id).FirstOrDefault();

                if(city != null)
                    city.Groups = context.Groups.Include("Responsable").Include("Collaborators").Where(g => g.CityId == city.Id).ToList();

                return city;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.StateCityRepository.getCityInstanceById", ex);
                throw new Exception("Lib.Repositories.StateCityRepository.getCityInstanceById - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca todos as entidades
        /// </summary>
        /// <returns></returns>
        public List<City> getAllCities()
        {
            try
            {
                return context.Cities.Include("State").ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.StateCityRepository.getAllCities", ex);
                throw new Exception("Lib.Repositories.StateCityRepository.getAllCities - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca todos as entidades
        /// </summary>
        /// <returns></returns>
        public List<State> getAllStates()
        {
            try
            {
                return context.States.Include("Cities").OrderBy(f => f.Name).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.StateCityRepository.getAllStates", ex);
                throw new Exception("Lib.Repositories.StateCityRepository.getAllStates - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca todos as entidades
        /// </summary>
        /// <returns></returns>
        public List<City> getCitiesByUF(string UF)
        {
            try
            {
                return context.Cities.Include("State").Where(f => f.StateId == UF).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.StateCityRepository.getCitiesByUF", ex);
                throw new Exception("Lib.Repositories.StateCityRepository.getCitiesByUF - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca todas as entidades com os grupos de acordo com o periodo
        /// </summary>
        /// <returns></returns>
        public List<City> getCitiesByUF(string UF, long periodId)
        {
            try
            {
                var cities = context.Cities.Include("State").Where(f => f.StateId == UF).ToList();

                if (cities != null)
                {
                    cities.ForEach(c =>
                    {
                        c.Groups = context.Groups.Include("Responsable").Include("Collaborators").Where(g => g.CityId == c.Id && g.PeriodId == periodId).ToList();
                    });
                }

                return cities;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.StateCityRepository.getCitiesByUF", ex);
                throw new Exception("Lib.Repositories.StateCityRepository.getCitiesByUF - " + ex.Message, ex);
            }

        }

        public List<City> getCitiesFromResponsableUser(long userId, long periodId)
        {
            return context.Groups.Where(c => c.PeriodId == periodId && c.ResponsableId == userId).Select(c => c.City).ToList();
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
