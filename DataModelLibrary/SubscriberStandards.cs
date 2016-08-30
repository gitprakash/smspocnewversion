using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{

    [Table("SubscriberStandards")]
    public partial class SubscriberStandards:BaseEntity
    {
        [Required]
        public int SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        [Required]
        public int StandardId { get; set; }
        public virtual Standard Standard { get; set; }
        public bool Active { get; set; }
      //  public virtual ICollection<SubscriberStandardContacts> SubscriberStandardContacts { get; set; }
       // public virtual ICollection<SubscriberStandardSections> SubscriberStandardSections { get; set; }
    }
}
