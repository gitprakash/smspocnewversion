using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataModelLibrary
{
    [Table("SubscriberRoles")]
    public class SubscriberRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int SubscriberId { get; set; }
        [Required]
        public int RoleId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public virtual Role role { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "date")]
        public DateTime ModifiedDate { get; set; }
        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
