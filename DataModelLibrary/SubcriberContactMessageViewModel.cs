using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLibrary
{
    public class SubcriberContactMessageViewModel
    {
        public Guid Id { get; set; }
      //  public Guid SubscriberContactGuid { get; set; }
        public string RollNo { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public long MobileNo { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime SentDateTime { get; set; }
        public string MessageError { get; set; }
    }
}
