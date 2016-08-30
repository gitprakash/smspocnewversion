using DataModelLibrary;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace DataServiceLibrary
{
    public interface IContactService
    {
        Task<IEnumerable<ContactViewModel>> Contacts(int subcriberId, JgGridParam jgGridParam);
        Task<bool> IsUniqueRollNo(int subscriberId, string rollNo);
        Task<int> TotalContacts(int subcriberId, JgGridParam jgGridParam);
        Task<Contact> AddContact(ContactViewModel contact);
        Task<int> EditContact(ContactViewModel contactvm);
        Task<bool> IsUniqueMobile(long mobileno);
        Task<SubscriberStandardContacts> FindContact(long Id);
        List<ContactViewModel> GetContactViewModels(DataTable dt);
        Task<ConcurrentBag<ErrorModal>> CheckExcelBuilkRollNoExistsTask(int subscriberId, List<ContactViewModel> lstContactViewModels);
        Task<List<ContactViewModel>> ExcelBulkUploadContact(int subscriberId, List<ContactViewModel> excellstContactViewModels);
        Task<int> SaveAsync();
    }
}
