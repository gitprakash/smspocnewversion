using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    [Table("SubcriberMessage")]
    public class Message:BaseEntity
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public int MessageCount { get; set; }
    }
}
