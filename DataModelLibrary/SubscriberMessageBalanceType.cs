using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    [ComplexType]
    public class SubscriberMessageBalanceType
    {
        [Required]
        public long OpeningCount { get; set; }
        [Required]
        public long RemainingCount { get; set; }
       
    }
}
