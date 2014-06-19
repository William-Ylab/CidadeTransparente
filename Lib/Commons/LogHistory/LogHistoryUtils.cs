using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Commons
{
    //Ver a possibilidade de criar uma SQLUtils para fazer a conexão com o banco de dados ao invés de deixar aqui.
    public class LogHistoryUtils
    {
        #region Propriedades

        private string QUERY_SELECT_HISTORY = "SELECT * FROM log_history WHERE (EntityId={0} AND EntityType='{1}') OR (ParentEntityId={0} AND ParentEntityType='{1}');";

        private string QUERY_CREATE_TABLE = @"CREATE TABLE `log_history` (
                                      `TrackingId` bigint(50) NOT NULL AUTO_INCREMENT,
                                      `EntityId` bigint(50) NOT NULL DEFAULT '0',
                                      `ParentEntityId` bigint(50) NOT NULL DEFAULT '0',
                                      `EntityType` varchar(255) NOT NULL DEFAULT '',
                                      `ParentEntityType` varchar(255) NOT NULL DEFAULT '',
                                      `Field` varchar(255) NOT NULL DEFAULT '',
                                      `BeforeValue` mediumtext NOT NULL,
                                      `AfterValue` mediumtext NOT NULL,
                                      `ActionDate` datetime NOT NULL DEFAULT '1901-01-01 00:00:00',
                                      `ActionUserId` bigint(50) NOT NULL DEFAULT '0',
                                      `Action` varchar(255) NOT NULL DEFAULT '',
                                      PRIMARY KEY (`TrackingId`)
                                    ) ENGINE=InnoDB DEFAULT CHARSET=latin1;";

        private const string QUERY_INSERT_LOGHISTORY = @"INSERT INTO `log_history` (`EntityId`, 
                                                                                  `ParentEntityId`, 
                                                                                  `EntityType`, 
                                                                                  `ParentEntityType`, 
                                                                                  `Field`, 
                                                                                  `BeforeValue`, 
                                                                                  `AfterValue`,
                                                                                  `ActionDate`,
                                                                                  `ActionUserId`,
                                                                                  `Action`) VALUES 
                                                                                   ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')";

        private MySqlConnection connection;

        public string ConnectionString { get; set; }

        #endregion

        #region Construtores

        public LogHistoryUtils(string connectionString)
        {
            this.ConnectionString = connectionString;
            connection = new MySqlConnection();
            connection.ConnectionString = this.ConnectionString;
        }

        #endregion

        #region Metodos Mysql

        private void createCurrentTableLog()
        {
            execute(String.Format(QUERY_CREATE_TABLE));
        }

        private void execute(string query)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

            MySqlCommand _command = new MySqlCommand(query, connection);
            _command.ExecuteNonQuery();

            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }

        private DataSet select(string query)
        {


            if (connection.State != ConnectionState.Open)
                connection.Open();

            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataSet dataSetResult = new DataSet();

            adapter.Fill(dataSetResult);

            if (connection.State != ConnectionState.Closed)
                connection.Close();

            return dataSetResult;
        }

        private void insertLogHistory(long entityId, string entityType, long parentEntityId, string parentEntityType, string field, string oldValue, string newValue, string action, long userId)
        {
            string insertQuery = string.Empty;

            try
            {
                //Monta a query de save do log
                insertQuery = String.Format(QUERY_INSERT_LOGHISTORY,
                                            SQLUtils.formatStringToSQL(entityId.ToString()),
                                            SQLUtils.formatStringToSQL(parentEntityId.ToString()),
                                            SQLUtils.formatStringToSQL(entityType),
                                            SQLUtils.formatStringToSQL(parentEntityType),
                                            SQLUtils.formatStringToSQL(field),
                                            SQLUtils.formatStringToSQL(oldValue),
                                            SQLUtils.formatStringToSQL(newValue),
                                            SQLUtils.formatDateTimeToSQL(DateTime.Now),
                                            SQLUtils.formatStringToSQL(userId.ToString()),
                                            SQLUtils.formatStringToSQL(action));

                //Executa a query
                execute(insertQuery);
            }
            catch (Exception ex)
            {
                //Verifica se o erro gerado é de tabela inexistente
                if (SQLUtils.isTableDoesNotExist(ex))
                {
                    //Cria a tabela dinamicamente.
                    createCurrentTableLog();

                    //Executa a inserção novamente
                    execute(insertQuery);
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        public void saveLogHistory(List<LogHistory> logs)
        {
            foreach (var item in logs)
            {
                insertLogHistory(item.EntityId, item.EntityType, item.ParentEntityId, item.ParentEntityType, item.Field, item.BeforeValue, item.AfterValue, item.Action.ToString(), item.ActionUserId);
            }
        }

        public List<LogHistory> getLogs(Object oldObject, Object newObject, long userId)
        {
            return getDiff(oldObject, newObject, userId);
        }

        public List<LogHistory> getHistoryList(long entityId, string entityType)
        {
            List<LogHistory> result = new List<LogHistory>();

            DataSet dataSetResult = select(String.Format(QUERY_SELECT_HISTORY, entityId, entityType));

            if (dataSetResult != null)
            {
                if (dataSetResult.Tables != null)
                {
                    if (dataSetResult.Tables[0].Rows != null)
                    {
                        foreach (DataRow row in dataSetResult.Tables[0].Rows)
                        {
                            result.Add(new LogHistory(row));
                        }
                    }
                }
            }

            return result;
        }

        private List<LogHistory> getDiff(Object oldObj, Object newObj, long userId)
        {
            List<LogHistory> ret = new List<LogHistory>();

            try
            {
                if (oldObj == null || newObj == null)
                {
                    return ret;
                }

                string propertyKeyName = getKeyName(oldObj);

                if (string.IsNullOrEmpty(propertyKeyName)) 
                {
                    throw new Exception("Não foi encontrada a propriedade chave desta entidade.");
                }

                var oldObjId = oldObj.GetType().GetProperty(propertyKeyName).GetValue(oldObj, null);
                var newObjId = newObj.GetType().GetProperty(propertyKeyName).GetValue(newObj, null);

                LogHistory log = new LogHistory();
                var oldProperties = getPropertiesToValidate(oldObj);
                var newProperties = getPropertiesToValidate(newObj);

                string entityType = "";
                if (newObj.GetType().BaseType.Name.Equals("object", StringComparison.CurrentCultureIgnoreCase))
                {
                    entityType = newObj.GetType().UnderlyingSystemType.Name;
                }
                else
                {
                    entityType = newObj.GetType().BaseType.Name;
                }

                foreach (var oldProperty in oldProperties)
                {
                    var newProperty = newProperties.Where(f => f.Name == oldProperty.Name).FirstOrDefault();
                    if (newProperty != null)
                    {
                        if (oldProperty.CanRead && newProperty.CanRead)
                        {
                            var oldValue = oldProperty.GetValue(oldObj, null);
                            var newValue = newProperty.GetValue(newObj, null);

                            if (oldValue == null && newValue == null)
                            {
                                continue;
                            }
                            else
                            {
                                if (oldProperty.PropertyType.IsGenericType && ((oldProperty.PropertyType).GetGenericTypeDefinition() == typeof(List<>)))
                                {
                                    IList oldPropertyList = oldProperty.GetValue(oldObj, null) as IList;
                                    IList newPropertyList = newProperty.GetValue(newObj, null) as IList;

                                    ret.AddRange(verifyDifferentItems(oldPropertyList, newPropertyList, entityType, long.Parse(newObjId.ToString()), propertyKeyName, Enumerations.LogHistoryAction.Delete, userId));
                                    ret.AddRange(verifyDifferentItems(newPropertyList, oldPropertyList, entityType, long.Parse(newObjId.ToString()), propertyKeyName, Enumerations.LogHistoryAction.Update, userId));
                                }
                                else
                                {
                                    if (oldValue == null && newValue != null)
                                    {
                                        ret.Add(createLog(long.Parse(oldObjId.ToString()), entityType, 0, "None", oldProperty.Name, "-", newValue.ToString(), Enumerations.LogHistoryAction.Update, userId));
                                    }
                                    else if (oldValue != null && newValue == null)
                                    {
                                        ret.Add(createLog(long.Parse(oldObjId.ToString()), entityType, 0, "None", oldProperty.Name, oldValue.ToString(), "-", Enumerations.LogHistoryAction.Update, userId));
                                    }
                                    else if (oldValue.ToString() != newValue.ToString())
                                    {
                                        if (oldProperty.PropertyType.Name == "Decimal" || oldProperty.PropertyType.Name == "Double")
                                        {
                                            if (double.Parse(oldValue.ToString()) != double.Parse(newValue.ToString()))
                                            {
                                                ret.Add(createLog(long.Parse(oldObjId.ToString()), entityType, 0, "None", oldProperty.Name, oldValue.ToString(), newValue.ToString(), Enumerations.LogHistoryAction.Update, userId));
                                            }
                                        }
                                        else
                                        {
                                            ret.Add(createLog(long.Parse(oldObjId.ToString()), entityType, 0, "None", oldProperty.Name, oldValue.ToString(), newValue.ToString(), Enumerations.LogHistoryAction.Update, userId));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<LogHistory> verifyDifferentItems(IList propertiesList, IList newPropertiesList, string parentEntityType, long parentId, string key, Enumerations.LogHistoryAction action, long userId)
        {
            List<LogHistory> ret = new List<LogHistory>();

            if (propertiesList == null) return ret;
            if (newPropertiesList == null) return ret;

            foreach (var itemOld in propertiesList)
            {
                string nameProperty = getDefaultPropertyName(itemOld);

                if (string.IsNullOrEmpty(nameProperty)) { throw new Exception("Não foi encontrado uma propriedade default para esta entidade"); }

                long itemId = long.Parse(itemOld.GetType().GetProperty(key).GetValue(itemOld, null).ToString());
                string itemName = itemOld.GetType().GetProperty(nameProperty).GetValue(itemOld, null).ToString();

                bool contains = containsPropertyWithSameId(newPropertiesList, itemId, key);
                string entityTypeOld = itemOld.GetType().Name;

                if (entityTypeOld.ToLower().Contains("dynamic") || entityTypeOld.ToLower().Contains("object"))
                {
                    entityTypeOld = itemOld.GetType().BaseType.Name;
                }

                if (contains == false)
                {
                    if (action == Enumerations.LogHistoryAction.Update)
                    {
                        ret.Add(createLog(itemId, entityTypeOld, parentId, parentEntityType, entityTypeOld.ToString(), "-", itemName, action, userId));
                    }
                    else
                    {
                        ret.Add(createLog(itemId, entityTypeOld, parentId, parentEntityType, entityTypeOld.ToString(), itemName, "-", action, userId));
                    }
                }
            }

            return ret;
        }

        private bool containsPropertyWithSameId(IList propertiesList, long entityId, string propertyKey)
        {
            foreach (var itemOld in propertiesList)
            {
                long oldItemId = long.Parse(itemOld.GetType().GetProperty(propertyKey).GetValue(itemOld, null).ToString());

                if (entityId == oldItemId)
                {
                    return true;
                }
            }

            return false;
        }

        private LogHistory createLog(long entityId,
                                            string entityType,
                                            long parentEntityId,
                                            string parentEntityType,
                                            string field,
                                            string oldValue,
                                            string newValue,
                                            Enumerations.LogHistoryAction action,
                                            long userId)
        {

            LogHistory log = new LogHistory();
            log.Action = action;
            log.ActionDate = DateTime.Now;
            log.ActionUserId = userId;
            log.EntityId = entityId;
            log.EntityType = entityType;
            log.Field = field;
            log.ParentEntityId = parentEntityId;
            log.ParentEntityType = parentEntityType;
            log.AfterValue = newValue;
            log.BeforeValue = oldValue;

            return log;
        }

        #region Métodos auxiliares

        private string getKeyName(Object oldObj)
        {
            foreach (var property in oldObj.GetType().GetProperties())
            {
                LogHistoryPropertyAttribute[] attrs = (LogHistoryPropertyAttribute[])(property.GetCustomAttributes(typeof(LogHistoryPropertyAttribute), true));

                foreach (LogHistoryPropertyAttribute attr in attrs)
                {
                    if (attr != null)
                    {
                        if (attr.Key == true)
                        {
                            return property.Name;
                        }
                    }
                }
            }

            return String.Empty;
        }

        private string getDefaultPropertyName(Object oldObj)
        {
            foreach (var property in oldObj.GetType().GetProperties())
            {
                LogHistoryPropertyAttribute[] attrs = (LogHistoryPropertyAttribute[])(property.GetCustomAttributes(typeof(LogHistoryPropertyAttribute), true));

                foreach (LogHistoryPropertyAttribute attr in attrs)
                {
                    if (attr != null)
                    {
                        if (attr.DefaultProperty == true)
                        {
                            return property.Name;
                        }
                    }
                }
            }

            return String.Empty;
        }

        private List<PropertyInfo> getPropertiesToValidate(Object obj)
        {
            List<PropertyInfo> ret = new List<PropertyInfo>();

            foreach (var property in obj.GetType().GetProperties())
            {
                LogHistoryPropertyAttribute[] attrs = (LogHistoryPropertyAttribute[])(property.GetCustomAttributes(typeof(LogHistoryPropertyAttribute), true));

                if (attrs.Length > 0)
                {
                    foreach (LogHistoryPropertyAttribute attr in attrs)
                    {
                        if (attr != null)
                        {
                            if (attr.IgnoreProperty == false)
                            {
                                ret.Add(property);
                            }
                        }
                    }
                }
                else
                {
                    ret.Add(property);
                }
            }

            return ret;
        }

        #endregion
    }

    public class LogHistory
    {
        public long TrackingId { get; set; }

        public long EntityId { get; set; }

        public long ParentEntityId { get; set; }

        public string EntityType { get; set; }

        public string ParentEntityType { get; set; }

        public string Field { get; set; }

        public string BeforeValue { get; set; }

        public string AfterValue { get; set; }

        public DateTime ActionDate { get; set; }

        public long ActionUserId { get; set; }

        /*[13-10-2013 Átila] Comentei pois ninguém utilizava e no banco não estavamos guardando o id do usuário, pois era usado o ActionUserId*/
        //public long UserId { get; set; }

        public Enumerations.LogHistoryAction Action { get; set; }

        public LogHistory()
        {
        }

        public LogHistory(DataRow row)
        {
            if (row != null)
            {
                TrackingId = long.Parse(row["TrackingId"].ToString());
                EntityId = long.Parse(row["EntityId"].ToString());
                ParentEntityId = long.Parse(row["ParentEntityId"].ToString());
                EntityType = row["EntityType"].ToString();
                ParentEntityType = row["ParentEntityType"].ToString();
                Field = row["Field"].ToString();
                BeforeValue = row["BeforeValue"].ToString();
                AfterValue = row["AfterValue"].ToString();
                ActionDate = Convert.ToDateTime(row["ActionDate"].ToString());
                ActionUserId = long.Parse(row["ActionUserId"].ToString());
                Action = (Enumerations.LogHistoryAction)Enum.Parse(typeof(Enumerations.LogHistoryAction), row["Action"].ToString());

            }
        }
    }
}
