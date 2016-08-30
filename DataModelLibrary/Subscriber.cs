using System.Runtime.Remoting;

namespace DataModelLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subscriber")]
    public partial class Subscriber
    { 
        [Key]
        [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string FirstName { get; set; }

        [StringLength(50)]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool Active { get; set; }
         
        [Column(TypeName = "date")]
        public DateTime ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
         
        [Required]
        public long Mobile { get; set; }

        public int AccountTypeId { get; set; }

        public virtual AccountType AccountType { get; set; }

        public int GenderTypeId { get; set; }

        public virtual GenderType GenderType { get; set; }
        [Required]
        public bool IsActivated { get; set; }

        public virtual ICollection<SubscriberRoles> Roles { get; set; }

        public virtual ICollection<SubscriberStandards> SubscriberStandards { get; set; }
    }
}
