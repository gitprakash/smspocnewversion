using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    [Table("SubscriberSection")]
    public partial class SubscriberSection:BaseEntity
    {
        [Required]
        public int SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        [Required]
        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
        public bool Active { get; set; }
    }
}
