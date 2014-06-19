using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Log
{
    public class LogHistory : Commons.LogHistory
    {
        private Lib.Entities.User activeUser;

        internal LogHistory() { }

        public LogHistory(Lib.Entities.User activeUser, Commons.LogHistory log)
        {
            //FixME - ver um jeito de melhorar, seja por reflection ou não..
            this.activeUser = activeUser;
            this.Action = log.Action;
            this.ActionDate = log.ActionDate;
            this.ActionUserId = log.ActionUserId;
            this.AfterValue = log.AfterValue;
            this.BeforeValue = log.BeforeValue;
            this.EntityId = log.EntityId;
            this.EntityType = log.EntityType;
            this.Field = log.Field;
            this.ParentEntityId = log.ParentEntityId;
            this.ParentEntityType = log.ParentEntityType;
            this.TrackingId = log.TrackingId;
        }


        //public Entities.User Employee
        //{
        //    get
        //    {
        //        using (Lib.Repositories.UserRepository repository = new Repositories.UserRepository(this.activeUser))
        //        {
        //            return repository.getInstanceById(this.ActionUserId);
        //        }
        //    }
        //}
    }
}
