using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface IUserRoleService
    {
        Task<SubscriberRoles> AddUserRole(SubscriberRoles sroles);
        Task<SubscriberRoles> GetUserRole(int  sibscriberId);
        Task<IEnumerable<SubscriberRoleviewModel>> GetUserRoles(int subscriberId);
        Task<int> SaveAsync();
        Task<bool> CheckExists(int userId, int roleId);
    }
}
