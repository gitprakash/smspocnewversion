using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary 
{
    public class MessageViewModel
    {
        [Required] 
        public long Id { get; set; }
       
        public string Name { get; set; }
        [Required]
        public long Mobile { get; set; }

        public string Standard { get; set; }

        public string Section { get; set; }

        public string RollNo { get; set; }

        public bool SentStatus { get; set; }

        public DateTime? SentTime { get; set; }

        public string MessageError { get; set; }
    }
}
