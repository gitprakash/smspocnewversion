using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface ISubscriberStandardService
    {
        Task<ICollection<SubscriberStandardViewModel>> GetStandards(int subscriberId);
        Task<ICollection<SubscriberStandardSectionViewModel>> GetSections(int subscirberStandardId);
        Task<List<SubscriberSection>> AddBulkSectionsifNotExists(List<ContactViewModel> lstContactViewModels, int subscriberId);
        Task<List<SubscriberStandards>> AddBulkClassifNotExists(List<ContactViewModel> lstContactViewModels, int subscriberId);
        Task<List<Tuple<string, string>>> AddBulkClassSectionLinkIfNotExists(List<ContactViewModel> lstContactViewModels, int subscriberId);
        Task<List<ContactViewModel>> ExcelBulkUpdateClassSectionTask(int subscriberId, List<ContactViewModel> excellstContactViewModels);
    }
}
