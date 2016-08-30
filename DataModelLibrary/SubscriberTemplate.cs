using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    [Table("SubscriberTemplate")]
    public class SubscriberTemplate:BaseEntity
    {
        [Required]
        public virtual int SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        [Required]
        public virtual int TemplateId { get; set; }
        public virtual Template Templates { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
