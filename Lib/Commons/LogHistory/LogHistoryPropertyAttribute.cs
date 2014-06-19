using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons
{
    public class LogHistoryPropertyAttribute : System.Attribute
    {
        public bool IgnoreProperty;
        public bool DefaultProperty;
        public bool Key;

        public LogHistoryPropertyAttribute()
        {
        }
    }
}
