using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entities
{
    public class User
    {
        #region [Properties]

        [Key]
        [Commons.LogHistoryProperty(Key = true, IgnoreProperty = false, DefaultProperty = false)]
        public long Id { get; set; }

        //[Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        //public long? EntityId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = true)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "REQUIRED_USER_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(50, ErrorMessageResourceName = "MAX_LENGTH_USER_NAME", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Name { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "REQUIRED_USER_EMAIL", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(50, ErrorMessageResourceName = "MAX_LENGTH_USER_EMAIL", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Email { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "REQUIRED_USER_LOGIN", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(20, ErrorMessageResourceName = "MAX_LENGTH_USER_LOGIN", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string Login { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "REQUIRED_USER_PASSWORD", ErrorMessageResourceType = typeof(Resources.Messages))]
        [StringLength(256, ErrorMessageResourceName = "MAX_LENGTH_USER_PASSWORD", ErrorMessageResourceType = typeof(Resources.Messages))]
        public String Password { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public int UserType { get; set; }

        public Enumerations.UserType UserTypeEnum
        {
            get
            {
                return (Enumerations.UserType)Enum.Parse(typeof(Enumerations.UserType), UserType.ToString());
            }
        }

        //[Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        //public Entity Entity { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<Group> Groups { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<Group> ResponsableGroups { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = true, DefaultProperty = false)]
        public List<ResponseForm> ResponseForms { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public string Mime { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public byte[] Thumb { get; set; }

        public bool Active { get; set; }

        public List<Review> Reviews { get; set; }

        public string Organization { get; set; }

        public string CNPJ { get; set; }

        public bool Network { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string Nature { get; set; }

        public string Area { get; set; }
        
        public string Range { get; set; }

        public string Address { get; set; }

        public string Number { get; set; }

        public string Neighborhood { get; set; }

        public string Region { get; set; }

        public long? CityId { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public City City { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string WebSite { get; set; }

        public bool NetworkApproved { get; set; }

        public long? NetworkApprovedById { get; set; }

        public bool TermsOfUse { get; set; }

        public DateTime AcceptionTermsDate { get; set; }

        public List<User> UsersNetworkApproved { get; set; }

        public User NetworkApprovedBy { get; set; }

        [Commons.LogHistoryProperty(Key = false, IgnoreProperty = false, DefaultProperty = false)]
        public List<RequestCity> RequestCities { get; set; }

        #endregion

        #region [Constructors]

        public User()
        {
            this.Address = string.Empty;
            this.Area = string.Empty;
            this.CityId = null;
            this.CNPJ = string.Empty;
            this.ContactEmail = string.Empty;
            this.ContactName = string.Empty;
            this.Mime = string.Empty;
            this.Nature = string.Empty;
            this.Neighborhood = string.Empty;
            this.Network = false;
            this.NetworkApproved = false;
            this.NetworkApprovedById = null;
            this.Number = string.Empty;
            this.Organization = string.Empty;
            this.Phone = string.Empty;
            this.Range = string.Empty;
            this.Region = string.Empty;
            this.TermsOfUse = false;
            this.Thumb = null;
            this.WebSite = string.Empty;
            this.ZipCode = string.Empty;
        }

        #endregion

        #region [Public Methods]

        public bool isEntityAccreditedAndAppoved()
        {
            if (this.UserTypeEnum == Enumerations.UserType.Entity && this.NetworkApproved && this.Network)
                return true;

            return false;
        }

        #endregion

    }
}
