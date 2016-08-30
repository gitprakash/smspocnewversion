using DataModelLibrary;
using System.Collections.Generic;
using System.Collections.Concurrent;
using DataServiceLibrary;

namespace SMSPOCWeb.Models
{
    public class BulkUploadStudentJsonModel
    {
        public bool Status { get; set; }
        public List<ContactViewModel> SuccessResult { get; set; }
        public ConcurrentBag<ErrorModal> ErrorResult { get; set; }
    }
}