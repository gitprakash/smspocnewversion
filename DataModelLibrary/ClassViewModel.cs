using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    public class ClassViewModel
    {
        [Key]
        public long Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string ClassName { get; set; }
        public bool Active { get; set; }
    }
}
