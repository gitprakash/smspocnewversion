using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
   [Table("SubscriberMessageBalanceHistory")]
    public class SubscriberMessageBalanceHistory:BaseEntity
    {
        public virtual Subscriber Subcriber { get; set; }
        [Required]
        public int SubcriberId { get; set; }
        [Required]
        public long OpeningCount { get; set; }
        [Required]
        public long RemainingCount { get; set; }
    }
}
