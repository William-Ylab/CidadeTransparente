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
    
    public partial class period
    {
        public period()
        {
            this.baseforms = new HashSet<baseform>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
        public System.DateTime InitialDate { get; set; }
        public System.DateTime FinalDate { get; set; }
    
        public virtual ICollection<baseform> baseforms { get; set; }
    }
}
