//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lib.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class responseform
    {
        public responseform()
        {
            this.answers = new HashSet<answer>();
            this.submits = new HashSet<submit>();
        }
    
        public long Id { get; set; }
        public long BaseFormId { get; set; }
        public long CityId { get; set; }
        public long UserId { get; set; }
    
        public virtual ICollection<answer> answers { get; set; }
        public virtual baseform baseform { get; set; }
        public virtual city city { get; set; }
        public virtual user user { get; set; }
        public virtual ICollection<submit> submits { get; set; }
    }
}