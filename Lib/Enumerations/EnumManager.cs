using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Enumerations
{
    public static class EnumManager
    {

        public static string getStringFromUserType(UserType type, bool network = false)
        {
            switch (type)
            {
                case UserType.Master:
                    return Resources.Messages.ENUM_USERTYPE_MASTER;
                case UserType.Admin:
                    return Resources.Messages.ENUM_USERTYPE_ADMIN;
                case UserType.Entity:
                    if (network)
                        return Resources.Messages.ENUM_USERTYPE_ENTITY_NETWORK;
                    else
                        return Resources.Messages.ENUM_USERTYPE_ENTITY;
                case UserType.Others:
                    return Resources.Messages.ENUM_USERTYPE_COMMON;
                case UserType.Site:
                    return Resources.Messages.ENUM_USERTYPE_SITE;
                default:
                    return "";
            }
        }

        public static string getStringFromSubmitType(SubmitStatus type)
        {
            switch (type)
            {
                case Lib.Enumerations.SubmitStatus.Approved:
                    return Lib.Resources.Messages.ENUM_SUBMITTYPE_APPROVED;
                case Lib.Enumerations.SubmitStatus.NotApproved:
                    return Lib.Resources.Messages.ENUM_SUBMITTYPE_REPROVED;
                default:
                    return Lib.Resources.Messages.ENUM_SUBMITTYPE_SUBMITTED;
            }
        }

        public static string getStringFromRequestStatus(RequestStatus type)
        {
            switch (type)
            {
                case RequestStatus.WAITING_APPROVE:
                    return Lib.Resources.Messages.WAITING_APPROVE;
                case RequestStatus.APPROVED:
                    return Lib.Resources.Messages.APPROVED;
                default:
                    return Lib.Resources.Messages.WAITING_APPROVE;
            }
        }
    }
}
