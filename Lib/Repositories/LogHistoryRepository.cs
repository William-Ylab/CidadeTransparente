using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Repositories
{
    public class LogHistoryRepository : Abstract.AbstractRepository, IDisposable
    {
        private FDTContext context;
        private Entities.User activeUser;
        private Commons.LogHistoryUtils logUtils;

        #region [Constructors]

        public LogHistoryRepository(Entities.User activeUser)
        {
            this.activeUser = activeUser;

            context = new FDTContext();
            var connection = context.Database.Connection;

            this.logUtils = new Commons.LogHistoryUtils(connection.ConnectionString);
        }

        #endregion

        #region IRepository<Payment> Members

        /// <summary>
        /// Método responsável por detectar e salvar as diferenças de 2 objetos.
        /// </summary>
        /// <param name="oldObject">objeto antes de salvar</param>
        /// <param name="newObject">objeto que será utilizado para atualizar</param>
        /// <param name="activeUser">usuario ativo no sistema</param>
        /// <param name="key">nome do campo chave - importante para comparar 2 objetos.</param>
        /// <param name="logField">nome do campo que será gerado o log caso haja (ex: Name em Role de employee)</param>
        internal List<Commons.LogHistory> getLogsHistory(Object oldObject, Object newObject, Entities.User activeUser, string key, string logField, Enumerations.EntityType entityType)
        {
            return this.logUtils.getLogs(oldObject, newObject, activeUser.Id);
        }

        public List<Lib.Log.LogHistory> getHistory(long entityId, Enumerations.EntityType entityType)
        {
            List<Lib.Log.LogHistory> ret = new List<Log.LogHistory>();

            this.logUtils.getHistoryList(entityId, entityType.ToString()).ForEach(f => ret.Add(new Lib.Log.LogHistory(this.activeUser, f)));

            return ret.OrderByDescending(f => f.ActionDate).ToList();
        }

        public void saveLogs(List<Commons.LogHistory> logs)
        {
            this.logUtils.saveLogHistory(logs);
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