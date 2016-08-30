using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    public class SubscriberStandardViewModel
    {
        public long  SubscriberStandardId  { get; set; }  
        public int SubscriberId { get; set; } 
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public bool Active { get; set; }
    }
}
