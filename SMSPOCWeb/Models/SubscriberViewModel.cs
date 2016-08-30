namespace SMSPOCWeb.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public  class SubscriberViewModel
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        [System.Web.Mvc.Remote("IsUserNameExists", "Account", HttpMethod="Post")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [NotMapped]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        [MinLength(3)]
        public string FirstName { get; set; }

        [StringLength(50)]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        [EmailAddress]
        [System.Web.Mvc.Remote("IsUserEmailExists", "Account", HttpMethod = "Post")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Entered phone format is not valid.")]
        [System.Web.Mvc.Remote("IsUniqueMobile", "Account", HttpMethod = "Post")]
        public long Mobile { get; set; }

        public int AccountTypeId { get; set; }

        public int GenderTypeId { get; set; }


    }
}

