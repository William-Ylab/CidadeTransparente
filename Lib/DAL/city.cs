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
    
    public partial class city
    {
        public city()
        {
            this.groups = new HashSet<group>();
            this.responseforms = new HashSet<responseform>();
        }
    
        public long Id { get; set; }
        public string StateId { get; set; }
        public string Name { get; set; }
    
        public virtual state state { get; set; }
        public virtual ICollection<group> groups { get; set; }
        public virtual ICollection<responseform> responseforms { get; set; }
    }
}