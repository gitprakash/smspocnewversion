using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    [Table("MessageStatus")]
    public class MessageStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, 4), Display(Name = "Message Status Enum")]
        public MessageStatusEnum Name { get; set; }
         
    }
    public enum MessageStatusEnum
    {
        Sent = 1,
       [Display(Name = "Not Sent")]
        NotSent = 2,
        NotDelivered = 3,
    }
}
