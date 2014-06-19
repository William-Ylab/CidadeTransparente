using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Report
{
    public class ScorePerBlock
    {
        public string ColumnName { get; set; }

        public decimal TotalScore { get; set; }

        public decimal Percentage { get; set; }

        public string Label { get; set; }

        public decimal MaxScore { get; set; }

        ///// <summary>
        /////TODO: ver de deixar generico.
        ///// </summary>
        //public List<decimal> Values { get; set; }

        ///// <summary>
        ///// TODO: ver como pegar por refelection o tipo da lista acima
        ///// </summary>
        //public string ListType { get; set; }
    }
}
