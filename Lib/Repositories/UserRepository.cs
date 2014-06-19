using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entities;

namespace Lib.Repositories
{
    public class UserRepository : Abstract.AbstractRepository, IDisposable
    {
        private FDTContext context;
        private Lib.Entities.User ActiveUser;

        #region [Constructors]

        private UserRepository()
        {
            //this.context = context;
            context = new FDTContext();
        }

        public UserRepository(Lib.Entities.User activeUser)
        {
            this.ActiveUser = activeUser;
            context = new FDTContext();
        }

        #endregion

        #region IRepository<User> Members

        /// <summary>
        /// Salva uma entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void save(Entities.User entity)
        {
            List<Commons.LogHistory> logs = null;
            Lib.Repositories.LogHistoryRepository log = new LogHistoryRepository(this.ActiveUser);

            try
            {
                context = new FDTContext();

                //Caso um usuário seja editado ou criado com o tipo de entidades, o usuario ativo deve ser do tipo administrador master ou administrador, caso contrário lançar exceção
                if (entity.UserTypeEnum == Enumerations.UserType.Entity)
                {
                    if (this.ActiveUser.UserTypeEnum != Enumerations.UserType.Admin && this.ActiveUser.UserTypeEnum != Enumerations.UserType.Master && this.ActiveUser.Id != entity.Id && this.ActiveUser.UserTypeEnum != Enumerations.UserType.Site)
                        throw new DbEntityValidationException(String.Format(Resources.Messages.entity_users_update_or_create_not_allowed_by, this.ActiveUser.UserType));
                }

                if (entity.Id == 0)
                {
                    //Não podem ser inseridos novos usuarios do tipo administrador master
                    if (entity.UserTypeEnum == Enumerations.UserType.Master)
                        throw new DbEntityValidationException(Resources.Messages.master_admin_creation_not_allowed);

                    //Caso o usuário novo seja um administrador, apenas outro administrador/master deve ser o criador
                    if (entity.UserTypeEnum == Enumerations.UserType.Admin)
                    {
                        if (this.ActiveUser.UserTypeEnum != Enumerations.UserType.Master)
                            throw new DbEntityValidationException(Resources.Messages.just_master_admin_can_be_create_admin_users);
                    }

                    //Caso um usuário do tipo outros seja criado, o usuário ativo deve ser do tipo SITE, ADMINISTRADOR ou ADM MASTER
                    if (entity.UserTypeEnum == Enumerations.UserType.Others)
                    {
                        if (this.ActiveUser.UserTypeEnum != Enumerations.UserType.Site && this.ActiveUser.UserTypeEnum != Enumerations.UserType.Admin && this.ActiveUser.Id != entity.Id && this.ActiveUser.UserTypeEnum != Enumerations.UserType.Master)
                            throw new DbEntityValidationException(String.Format(Resources.Messages.other_users_creation_not_allowed_by, this.ActiveUser.UserType));
                    }

                    ////Se o usuário for do tipo entidade, cria a entity com o nome do usuário
                    //if (entity.UserTypeEnum == Lib.Enumerations.UserType.Entity)
                    //{
                    //    entity.Entity = new Lib.Entities.Entity();
                    //    entity.Entity.Name = entity.Name;
                    //}

                    context.Users.Add(entity);
                }
                else
                {
                    //Caso um usuário do tipo outros seja atualizado, o usuário ativo deve ser do tipo ADMINISTRADOR, ADM MASTER ou seja ele mesmo, caso contrário lançar exceção.
                    //Adicionado o usuario ativo do tipo SITE tbm.
                    if (entity.UserTypeEnum == Enumerations.UserType.Others)
                    {
                        if (this.ActiveUser.UserTypeEnum != Enumerations.UserType.Admin && this.ActiveUser.UserTypeEnum != Enumerations.UserType.Master && this.ActiveUser.Id != entity.Id && this.ActiveUser.UserTypeEnum != Enumerations.UserType.Site)
                            throw new DbEntityValidationException(String.Format(Resources.Messages.other_users_update_not_allowed_by, this.ActiveUser.UserType));

                    }

                    //Recuperando o usuário do banco
                    var user = getInstanceById(entity.Id); //context.BaseForms.Include("BaseBlocks").Include("Period").Where(e => e.Id == entity.Id).FirstOrDefault();
                    user.Name = entity.Name;
                    user.Email = entity.Email;
                    user.Thumb = entity.Thumb;
                    user.Mime = entity.Mime;
                    user.Active = entity.Active;
                    user.Password = entity.Password;
                    user.Organization = entity.Organization;
                    user.TermsOfUse = entity.TermsOfUse;
                    user.AcceptionTermsDate = entity.AcceptionTermsDate;

                    user.Address = entity.Address;
                    user.Area = entity.Area;
                    user.CityId = entity.CityId;
                    user.CNPJ = entity.CNPJ;
                    user.ContactName = entity.ContactName;
                    user.Nature = entity.Nature;
                    user.Neighborhood = entity.Neighborhood;
                    user.Network = entity.Network;
                    user.Number = entity.Number;
                    user.Phone = entity.Phone;
                    user.Range = entity.Range;
                    user.Region = entity.Region;
                    user.WebSite = entity.WebSite;
                    user.ZipCode = entity.ZipCode;
                    user.ContactEmail = entity.ContactEmail;

                    user.Network = false;

                    if (user.UserTypeEnum == Enumerations.UserType.Entity)
                    {
                        if (!entity.Network)
                        {
                            //Não é credenciado
                            user.Network = false;
                            user.NetworkApprovedById = null;
                            user.NetworkApproved = false;
                        }
                        else
                        {
                            //É credenciada
                            user.Network = entity.Network;

                            if (!user.NetworkApproved)
                            {
                                if (entity.NetworkApproved)
                                {
                                    //foi aprovado
                                    user.NetworkApproved = entity.NetworkApproved;
                                    user.NetworkApprovedById = this.ActiveUser.Id;
                                }
                                else
                                {
                                    //Continua não aprovado
                                    user.NetworkApproved = false;
                                    user.NetworkApprovedById = null;
                                }
                            }
                            else
                            {
                                if (!entity.NetworkApproved)
                                {
                                    //Foi desaprovado
                                    user.NetworkApproved = false;
                                    user.NetworkApprovedById = null;
                                }
                            }

                        }
                    }

                    ////salva o log.
                    //logs = log.getLogsHistory(baseForm, entity, ActiveUser, "Id", "Name", Enumerations.EntityType.BaseForm);
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
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.save", ex);
                throw new Exception("Lib.Repositories.UserRepository.save - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Salva uma entidade do tipo master
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void createUserMaster(Entities.User entity)
        {
            try
            {
                context = new FDTContext();

                if (entity.Id == 0)
                {
                    context.Users.Add(entity);
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
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.save", ex);
                throw new Exception("Lib.Repositories.UserRepository.save - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Autentica um usuário na aplicação
        /// </summary>
        /// <param name="login">Login do usuário</param>
        /// <param name="password">Senha do usuário (Não criptografada)</param>
        /// <returns>Retorna a instancia do usuário logado</returns>
        public Lib.Entities.User authenticateAdmins(string login, string password, bool isCookieAuthentication = false)
        {
            if (String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(password))
            {
                addErrorMessage("Login/Senha devem ser inseridos");
                return null;
            }

            string encryptPassword = Commons.SecurityUtils.criptografaSenha(password);
            Lib.Entities.User user = null;

            if (isCookieAuthentication)
            {
                user = context.Users.Where(e => e.Login.Equals(login) && e.Password.Equals(password) && e.Active.Equals(true) && (e.UserType == (int)Enumerations.UserType.Admin || e.UserType == (int)Enumerations.UserType.Master)).FirstOrDefault();
            }
            else
            {
                user = context.Users.Where(e => e.Login.Equals(login) && e.Password.Equals(encryptPassword) && e.Active.Equals(true) && (e.UserType == (int)Enumerations.UserType.Admin || e.UserType == (int)Enumerations.UserType.Master)).FirstOrDefault();
            }

            if (user == null)
            {
                addErrorMessage("Login/Senha inválidos");
                return null;
            }
            else
            {
                return user;
            }
        }

        /// <summary>
        /// Autentica um usuário na aplicação
        /// </summary>
        /// <param name="login">Login do usuário</param>
        /// <param name="password">Senha do usuário (Não criptografada)</param>
        /// <returns>Retorna a instancia do usuário logado</returns>
        public Lib.Entities.User authenticateEntityAndComum(string login, string password, bool isCookieAuthentication = false)
        {
            if (String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(password))
            {
                addErrorMessage("Login/Senha devem ser inseridos");
                return null;
            }

            string encryptPassword = Commons.SecurityUtils.criptografaSenha(password);
            Lib.Entities.User user = null;

            if (isCookieAuthentication)
            {
                user = context.Users.Include("ResponsableGroups").Include("Groups").Include("Groups.City").Include("Groups.City.State").Where(e => e.Login.Equals(login) && e.Password.Equals(password) && e.Active.Equals(true) && (e.UserType == (int)Enumerations.UserType.Entity || e.UserType == (int)Enumerations.UserType.Others)).FirstOrDefault();
            }
            else
            {
                user = context.Users.Include("ResponsableGroups").Include("Groups").Include("Groups.City").Include("Groups.City.State").Where(e => e.Login.Equals(login) && e.Password.Equals(encryptPassword) && e.Active.Equals(true) && (e.UserType == (int)Enumerations.UserType.Entity || e.UserType == (int)Enumerations.UserType.Others)).FirstOrDefault();
            }

            if (user == null)
            {
                addErrorMessage("Login/Senha inválidos");
                return null;
            }
            else
            {
                return user;
            }
        }

        /// <summary>
        /// Deleta uma entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void delete(User entity)
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
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.delete", ex);
                throw new Exception("Lib.Repositories.UserRepository.delete - " + ex.Message, ex);
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
                    context.Users.Remove(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.delete", ex);
                throw new Exception("Lib.Repositories.UserRepository.delete - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Busca a entidade por Id
        /// </summary>
        /// <param name="id">Id da entidade</param>
        /// <returns></returns>
        public Entities.User getInstanceById(long id)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                return context.Users
                    .Include("Groups")
                    .Include("Groups.City")
                    .Include("Groups.Period")
                    .Include("ResponsableGroups")
                    .Include("ResponsableGroups.City")
                    .Include("ResponsableGroups.Period")
                    .Include("City")
                    .Include("RequestCities")
                    .Include("NetworkApprovedBy").Where(e => e.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.getInstanceById", ex);
                throw new Exception("Lib.Repositories.UserRepository.getInstanceById - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca a entidade por email
        /// </summary>
        /// <param name="email">email da entidade</param>
        /// <returns></returns>
        public Entities.User getInstanceByEmail(string email)
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                return context.Users.Include("NetworkApprovedBy").Where(e => e.Email == email).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.getInstanceByEmail", ex);
                throw new Exception("Lib.Repositories.UserRepository.getInstanceByEmail - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Retorna todas as entidades
        /// </summary>
        public List<Entities.User> getAll()
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                return context.Users.Include("NetworkApprovedBy").OrderBy(f => f.Name).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.getAll", ex);
                throw new Exception("Lib.Repositories.UserRepository.getAll - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Retorna todas as entidades credenciadas e aprovadas
        /// </summary>
        public List<Entities.User> getAllAccreditedEntities()
        {
            try
            {
                //Buscando uma determinada entidade e seus relacionamentos
                return context.Users.Include("NetworkApprovedBy").Where(u => u.UserType == 2 && u.Network && u.NetworkApproved).OrderBy(f => f.Name).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.getAll", ex);
                throw new Exception("Lib.Repositories.UserRepository.getAll - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Seleciona uma lista de usuários
        /// </summary>
        public List<Entities.User> selectWhere(Func<Entities.User, bool> where)
        {
            try
            {
                return context.Users.Where<Entities.User>(where).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.UserRepository.selectWhere - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Seleciona uma lista de usuários
        /// </summary>
        public List<Entities.User> search(List<int> userTypes = null, bool? status = null)
        {
            try
            {
                var query = (from u in context.Users
                             select u).AsQueryable();

                if (userTypes != null && userTypes.Count > 0)
                    query = query.Where(u => userTypes.Contains(u.UserType)).AsQueryable();

                if (status.HasValue)
                {
                    query = query.Where(u => u.Active == status.Value).AsQueryable();
                }

                return query.OrderBy(u => u.Name).ToList();
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.selectWhere", ex);
                throw new Exception("Lib.Repositories.UserRepository.selectWhere - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Verifica se o login já está em uso
        /// </summary>
        /// <param name="userId">Id do usuário caso não seja criação</param>
        /// <param name="login">Login a ser verificado</param>
        /// <returns>TRUE caso login esteja em uso</returns>
        public bool loginAlreadyUsed(long userId, string login)
        {
            try
            {
                var user = context.Users.Where(u => u.Login == login).FirstOrDefault();

                if (user != null)
                {
                    //Verifica se o login é utilizado pelo mesmo usuário.
                    if (user.Id == userId)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.loginAlreadyUsed", ex);
                throw new Exception("Lib.Repositories.UserRepository.loginAlreadyUsed - " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Verifica se o email já está em uso
        /// </summary>
        /// <param name="userId">Id do usuário caso não seja criação</param>
        /// <param name="email">Email a ser verificado</param>
        /// <returns>TRUE caso email esteja em uso</returns>
        public bool emailAlreadyUsed(long userId, string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return false;
                }

                var user = context.Users.Where(u => u.Email == email).FirstOrDefault();

                if (user != null)
                {
                    //Verifica se o email é utilizado pelo mesmo usuário.
                    if (user.Id == userId)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.emailAlreadyUsed", ex);
                throw new Exception("Lib.Repositories.UserRepository.emailAlreadyUsed - " + ex.Message, ex);
            }

        }

        public void recoveryPassowrd(string email, string login)
        {
            var user = context.Users.Where(f => f.Email == email && f.Login == login).FirstOrDefault();

            if (user != null)
            {
                var password = Guid.NewGuid().ToString().Substring(0, 6);
                user.Password = Commons.SecurityUtils.criptografaSenha(password);

                save(user);

                Utils.EmailUtils emailUtils = new Utils.EmailUtils(this.ActiveUser);
                emailUtils.passwordRecovery(user, password);
            }
            else
            {
                throw new Exception("Usuário ou email não encontrado");
            }
        }

        public List<RequestCity> citiesRequests(long userId, long periodId)
        {
            return context.RequestCities.Include("City").Include("City.State").Where(f => f.UserId == userId && f.PeriodId == periodId).ToList();
        }

        /// <summary>
        /// Recupera a lista de solicitações feitas para uma determinada cidade e periodo.
        /// </summary>
        /// <param name="cityId">Código da cidade</param>
        /// <param name="periodId">Código do período</param>
        /// <returns></returns>
        public List<RequestCity> getAllRequestsFromCity(long cityId, long periodId)
        {
            return context.RequestCities.Include("User").Include("City").Include("City.State").Where(f => f.CityId == cityId && f.PeriodId == periodId).ToList();
        }

        /// <summary>
        /// Recupera a lista de solicitações feitas para uma determinado periodo.
        /// </summary>
        /// <param name="periodId">Código do período</param>
        /// <returns></returns>
        public List<RequestCity> getAllRequestsByPeriod(long periodId)
        {
            return context.RequestCities.Include("User").Include("City").Include("City.State").Where(f => f.PeriodId == periodId).ToList();
        }


        /// <summary>
        /// Solicita um novo município para avaliação
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="userId"></param>
        /// <param name="periodId"></param>
        /// <param name="requestType"></param>
        public void requestCity(long cityId, long userId, long periodId, Enumerations.RequestType requestType)
        {
            try
            {
                var selectedUser = getInstanceById(userId);

                if (selectedUser != null)
                {
                    if (selectedUser.isEntityAccreditedAndAppoved())
                    {
                        var existCity = selectedUser.RequestCities.Where(rc => rc.CityId == cityId && rc.PeriodId == periodId).FirstOrDefault();

                        if (existCity == null)
                        {
                            //Verifica se a solicitação ja foi aprovada em um período anterior
                            Lib.Enumerations.RequestStatus lastStatus = requestAlreadyApproved(userId, periodId, cityId, requestType);

                            if (lastStatus == Enumerations.RequestStatus.APPROVED)
                            {
                                //Pela regra, já podemos aprovar a solicitação
                                approveRequest(userId, periodId, cityId, requestType);
                            }

                            var newRequest = new Entities.RequestCity();
                            newRequest.CityId = cityId;
                            newRequest.PeriodId = periodId;
                            newRequest.RequestDate = DateTime.Now;
                            newRequest.UserId = userId;
                            newRequest.RequestType = (int)requestType;
                            newRequest.Status = (int)lastStatus;

                            context.RequestCities.Add(newRequest);

                            context.SaveChanges();
                        }
                        else
                        {
                            //Municipio já solicitado
                            throw new DbEntityValidationException(Resources.Messages.city_already_requested);
                        }
                    }
                    else
                    {
                        //Usuário não tem permissões para solicitar municípios
                        throw new DbEntityValidationException(Resources.Messages.request_city_not_allowed);
                    }
                }
                else
                {
                    //Usuário inválido
                    throw new DbEntityValidationException(Resources.Messages.invalid_user);
                }
            }
            catch (DbEntityValidationException ex)
            {
                //Adiciona na lista de erros os erros de DataAnnotation
                addErrorMessage(ex);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.requestCity", ex);
                throw new Exception("Lib.Repositories.UserRepository.requestCity - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Aprova a solicitação de responabilidade ou colaboração de uma determinada entidade para um município
        /// </summary>
        /// <param name="userId">Código do usuário (Entidade credenciada)</param>
        /// <param name="periodId">Código do Período a qual a entidade será colaborada ou responsável</param>
        /// <param name="cityId">Código do município</param>
        /// <param name="requestType">Se responsável ou colaboradora pelo município</param>
        public bool approveRequest(long userId, long periodId, long cityId, Enumerations.RequestType requestType)
        {
            try
            {
                var user = context.Users.Where(u => u.Id == userId).FirstOrDefault();

                if (user != null)
                {
                    //Verifica se a entidade é credenciada e aprovada
                    if (user.isEntityAccreditedAndAppoved())
                    {
                        if (requestType == Enumerations.RequestType.RESPONSABLE)
                        {
                            var group = context.Groups.Where(c => c.CityId == cityId && c.PeriodId == periodId).FirstOrDefault();
                            if (group != null)
                            {
                                //Alterando o responsável pelo o usuário
                                group.ResponsableId = userId;

                            }
                            else
                            {
                                //Grupo não existe, cria um novo
                                context.Groups.Add(new Group()
                                {
                                    CityId = cityId,
                                    PeriodId = periodId,
                                    ResponsableId = userId
                                });
                            }

                            context.SaveChanges();
                            return true;
                        }
                        else if (requestType == Enumerations.RequestType.COLLABORATOR)
                        {
                            var group = context.Groups.Include("Collaborators").Where(c => c.CityId == cityId && c.PeriodId == periodId).FirstOrDefault();
                            if (group != null)
                            {
                                //Alterando o responsável pelo o usuário
                                group.Collaborators.Add(user);

                            }
                            else
                            {
                                var newGroup = new Group()
                                {
                                    CityId = cityId,
                                    PeriodId = periodId,
                                    ResponsableId = null,
                                    Collaborators = new List<User>()
                                };

                                newGroup.Collaborators.Add(user);

                                //Grupo não existe, cria um novo
                                context.Groups.Add(newGroup);
                            }

                            context.SaveChanges();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (DbEntityValidationException ex)
            {
                //Adiciona na lista de erros os erros de DataAnnotation
                addErrorMessage(ex);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.approveRequest", ex);
                throw new Exception("Lib.Repositories.UserRepository.approveRequest - " + ex.Message, ex);
            }

            return false;
        }

        /// <summary>
        /// Aprova uma determinada solicitação
        /// </summary>
        /// <param name="id">Id da solicitação</param>
        public bool approveRequest(long id)
        {
            bool approved = false;
            var request = context.RequestCities.Where(u => u.Id == id).FirstOrDefault();

            if (request != null)
            {
                approved = approveRequest(request.UserId, request.PeriodId, request.CityId, request.RequestTypeEnum);

                if (approved)
                {
                    request.Status = (int)Enumerations.RequestStatus.APPROVED;

                    context.SaveChanges();
                }
            }

            return approved;
        }

        /// <summary>
        /// Remove o responsável de uma determinada cidade e um determinado período
        /// </summary>
        /// <param name="periodId">Código do período</param>
        /// <param name="cityId">Código da cidade</param>
        public void removeResponsable(long periodId, long cityId)
        {
            try
            {
                var group = context.Groups.Where(c => c.CityId == cityId && c.PeriodId == periodId).FirstOrDefault();
                if (group != null)
                {
                    //Alterando o responsável pelo o usuário
                    group.ResponsableId = null;

                }
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //Adiciona na lista de erros os erros de DataAnnotation
                addErrorMessage(ex);
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("Lib.Repositories.UserRepository.removeResponsable", ex);
                throw new Exception("Lib.Repositories.UserRepository.removeResponsable - " + ex.Message, ex);
            }
        }

        private Enumerations.RequestStatus requestAlreadyApproved(long userId, long periodId, long cityId, Enumerations.RequestType requestType)
        {
            //Busca o último período
            using (Lib.Repositories.PeriodRepository ctxPeriod = new PeriodRepository(this.ActiveUser))
            {
                var publishedPeriod = ctxPeriod.getLastPublishedPeriod();

                if (publishedPeriod != null)
                {
                    var responsable = context.Groups.Where(g => g.CityId == cityId && g.ResponsableId == userId && g.PeriodId == publishedPeriod.Id).FirstOrDefault();

                    //Verifica se a solicitação é de responsável ou colaborador
                    if (requestType == Enumerations.RequestType.RESPONSABLE)
                    {

                        if (responsable != null)
                        {
                            //No período anterior, está entidade era responsável pela cidade solicitada

                            //Verifica se já existe alguma solicitacao para esse municipio.
                            if (userCanBeResponsableForCity(cityId, userId, periodId))
                                return Enumerations.RequestStatus.APPROVED;
                            else
                                return Enumerations.RequestStatus.WAITING_APPROVE;
                        }
                        else
                        {
                            //No período anterior, está entidade não era responsável pela cidade solicitada
                            return Enumerations.RequestStatus.WAITING_APPROVE;
                        }
                    }
                    else if (requestType == Enumerations.RequestType.COLLABORATOR)
                    {
                        if (responsable != null)
                        {
                            //Entidade era responsável e agora quer ser colaboradora
                            if (userCanBeCollaboratorForCity(cityId, userId, periodId))
                                return Enumerations.RequestStatus.APPROVED;
                            else
                                return Enumerations.RequestStatus.WAITING_APPROVE;
                        }
                        else
                        {
                            //Verifica se a entidade era colaboradora no período anterior.
                            var request = context.Groups.Include("Collaborators").Where(g => g.CityId == cityId && g.PeriodId == publishedPeriod.Id).FirstOrDefault();

                            if (request != null)
                            {
                                //Verifica se o grupo do período anterior, tem o coloborador solicitado
                                if (request.Collaborators.Where(c => c.Id == userId).Count() == 1)
                                {
                                    //A entidade era uma colaboradora e continuará sendo nesse período.
                                    if (userCanBeCollaboratorForCity(cityId, userId, periodId))
                                        return Enumerations.RequestStatus.APPROVED;
                                    else
                                        return Enumerations.RequestStatus.WAITING_APPROVE;
                                }
                                else
                                {
                                    //Entidade não era colaboradora desse município
                                    return Enumerations.RequestStatus.WAITING_APPROVE;
                                }

                            }
                            else
                            {
                                //Cidade não tem responsável
                                return Enumerations.RequestStatus.WAITING_APPROVE;
                            }
                        }
                    }
                    else
                    {
                        //Nenhum solicitação valida (Colaborador ou Responsável)
                        return Enumerations.RequestStatus.WAITING_APPROVE;
                    }


                }
                else
                {
                    //Nenhum período publicado
                    return Enumerations.RequestStatus.WAITING_APPROVE;
                }

            }

        }

        public void updateCollaborators(List<long> userIdsToAdd, List<long> userIdsToRemove, long periodId, long cityId)
        {
            var group = context.Groups.Include("Collaborators").Where(c => c.CityId == cityId && c.PeriodId == periodId).FirstOrDefault();
            if (group != null)
            {
                //Removendo os calaboradores
                group.Collaborators.RemoveAll(c => userIdsToRemove.Contains(c.Id));

                //Adicionando os colaboradores
                userIdsToAdd.ForEach(uid =>
                    {
                        var user = context.Users.Where(u => u.Id == uid).FirstOrDefault();

                        if (user != null)
                        {
                            if (user.isEntityAccreditedAndAppoved())
                            {
                                group.Collaborators.Add(user);
                            }
                        }
                    });
            }
            else
            {
                var newGroup = new Group()
                {
                    CityId = cityId,
                    PeriodId = periodId,
                    ResponsableId = null,
                    Collaborators = new List<User>()
                };

                //Adicionando os colaboradores
                userIdsToAdd.ForEach(uid =>
                {
                    var user = context.Users.Where(u => u.Id == uid).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.isEntityAccreditedAndAppoved())
                        {
                            newGroup.Collaborators.Add(user);
                        }
                    }
                });

                //Grupo não existe, cria um novo
                context.Groups.Add(newGroup);
            }

            context.SaveChanges();
        }

        public List<User> getCollaborators(long cityId, long periodId)
        {
            var group = context.Groups.Include("Collaborators").Where(c => c.CityId == cityId && c.PeriodId == periodId).FirstOrDefault();

            if (group != null)
            {
                return group.Collaborators;
            }
            return null;
        }

        public User getResponsable(long cityId, long periodId)
        {
            var group = context.Groups.Include("Responsable").Where(c => c.CityId == cityId && c.PeriodId == periodId).FirstOrDefault();

            if (group != null)
            {
                return group.Responsable;
            }

            return null;
        }

        #endregion

        #region [Private Methods]


        /// <summary>
        /// Verifica se o usuário pode ser responsável pelo município no período informado
        /// </summary>
        /// <returns></returns>
        private bool userCanBeResponsableForCity(long cityId, long userId, long periodId)
        {
            var group = context.Groups.Include("Collaborators").Where(c => c.CityId == cityId && c.PeriodId == periodId).FirstOrDefault();

            if (group != null)
            {
                if (group.ResponsableId == null)
                    return true;
            }
            else
            {
                //Não existe nenhum grupo, entidade pode ser responsável normalmente
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verifica se o usuário pode ser colaborador pelo município no período informado
        /// </summary>
        /// <returns></returns>
        private bool userCanBeCollaboratorForCity(long cityId, long userId, long periodId)
        {
            var group = context.Groups.Include("Collaborators").Where(c => c.CityId == cityId && c.PeriodId == periodId).FirstOrDefault();

            if (group != null)
            {
                if (group.Collaborators != null)
                {
                    if (group.Collaborators.Where(c => c.Id == userId).Count() == 0)
                        return true;
                }
            }

            return false;
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
