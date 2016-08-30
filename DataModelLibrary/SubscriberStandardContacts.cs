using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{

    [Table("SubscriberStandardContacts")]
    public class SubscriberStandardContacts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public long SubscriberStandardsId { get; set; }
        public virtual SubscriberStandards SubscriberStandards { get; set; }
        [Required]
        public long ContactId { get; set; }
        public virtual Contact Contact { get; set; }
        public int? SubscriberStandardSectionsId { get; set; }
        public virtual SubscriberStandardSections SubscriberStandardSections { get; set; }
        public bool Active { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
