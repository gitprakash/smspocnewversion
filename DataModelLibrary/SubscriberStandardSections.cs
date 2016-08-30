using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{

    [Table("SubscriberStandardSections")]
    public class SubscriberStandardSections
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public long SubscriberStandardsId { get; set; }
        public virtual SubscriberStandards SubscriberStandards { get; set; }
        [Required]
        public long SubscriberSectionId { get; set; }
        public virtual SubscriberSection SubscriberSection { get; set; }
        public bool Active { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
