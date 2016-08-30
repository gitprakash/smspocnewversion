using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    public class SubscriberStandardSectionViewModel
    {
        public long SubscriberStandardId { get; set; }
        public long SubscriberStandardSectionId { get; set; }
        public string SectionName { get; set; }
        public bool Active { get; set; }
    }
}
