namespace DataModelLibrary
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ContactViewModel
    {
        
        public long Id { get; set; }
        [Required]
        public long SubscriberStandardId { get; set; }

        public int? SubscriberStandardSectionId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200),MinLength(2)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Please enter 10 digit Mobile Number")]
        public long Mobile { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public string Class { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public string Section { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string RollNo { get; set; }

        public string BloodGroup { get; set; }
        public string Status { get; set; }

    }
}

