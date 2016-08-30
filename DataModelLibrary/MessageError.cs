using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataModelLibrary
{
    [Table("MessageError")]
    public class MessageError : BaseEntity
    {
        public string Text { get; set; }
    }
}
