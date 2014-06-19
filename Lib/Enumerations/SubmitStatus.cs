using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Enumerations
{
    public enum SubmitStatus : int
    {
        NotApproved = 0,
        Approved = 1,
        Submitted = 2, 
        MustRevised = 3
    }
}
