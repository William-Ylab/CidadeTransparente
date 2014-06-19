using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    internal partial class FDTContext : DbContext, IDisposable
    {
        #region [Properties]

        public virtual DbSet<Entities.Answer> Answers { get; set; }
        public virtual DbSet<Entities.BaseBlock> BaseBlocks { get; set; }
        public virtual DbSet<Entities.BaseSubBlock> BaseSubBlocks { get; set; }
        public virtual DbSet<Entities.BaseForm> BaseForms { get; set; }
        public virtual DbSet<Entities.BaseQuestion> BaseQuestions { get; set; }
        public virtual DbSet<Entities.City> Cities { get; set; }
        //public virtual DbSet<Entities.Entity> Entities { get; set; }
        public virtual DbSet<Entities.Group> Groups { get; set; }
        public virtual DbSet<Entities.Period> Periods { get; set; }
        public virtual DbSet<Entities.ResponseForm> ResponseForms { get; set; }
        public virtual DbSet<Entities.State> States { get; set; }
        public virtual DbSet<Entities.Submit> Submits { get; set; }
        public virtual DbSet<Entities.User> Users { get; set; }
        public virtual DbSet<Entities.Review> Reviews { get; set; }
        public virtual DbSet<Entities.RequestCity> RequestCities { get; set; }

        #endregion

        #region [Constructor]

        public FDTContext()
            : base("FdtConnection")
        {
            Answers = Set<Entities.Answer>();
            BaseBlocks = Set<Entities.BaseBlock>();
            BaseSubBlocks = Set<Entities.BaseSubBlock>();
            BaseForms = Set<Entities.BaseForm>();
            BaseQuestions = Set<Entities.BaseQuestion>();
            Cities = Set<Entities.City>();
            //Entities = Set<Entities.Entity>();
            Groups = Set<Entities.Group>();
            Periods = Set<Entities.Period>();
            ResponseForms = Set<Entities.ResponseForm>();
            States = Set<Entities.State>();
            Submits = Set<Entities.Submit>();
            Users = Set<Entities.User>();
            Reviews = Set<Entities.Review>();
            RequestCities = Set<Entities.RequestCity>();
        }

        #endregion

        #region [Public Methods]

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                if (ex.EntityValidationErrors != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("DbEntityValidationException errors");

                    foreach (DbEntityValidationResult result in ex.EntityValidationErrors)
                    {
                        if (result.ValidationErrors != null)
                        {
                            foreach (var error in result.ValidationErrors)
                            {
                                sb.AppendFormat("PropertyName: {0} - ErrorMessage: {1}", error.PropertyName, error.ErrorMessage);
                                sb.AppendLine("\n--------------------\n");
                            }
                        }
                    }
                    //Efetua o log do erro
                    Log.ErrorLog.saveError("FDTContext.save", sb.ToString());
                }
                throw ex;
            }
            catch (Exception ex)
            {
                Log.ErrorLog.saveError("FDTContext.save", ex);
                throw ex;
            }
        }

        #endregion
    }
}
