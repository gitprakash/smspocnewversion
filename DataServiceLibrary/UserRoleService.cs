using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class UserRoleService:IUserRoleService
    {
        IGenericRepository<SubscriberRoles> msubscriberrolesrepository;
        
        public UserRoleService(IGenericRepository<SubscriberRoles> subscriberrolesrepository )
        {
            msubscriberrolesrepository = subscriberrolesrepository;
            
        }
       public async Task<SubscriberRoles> AddUserRole(SubscriberRoles sroles)
       {
           return await msubscriberrolesrepository.AddAsync(sroles);
       }
       public async Task<bool> CheckExists(int userId,int roleId)
       {
           return await msubscriberrolesrepository.AnyAsync(sr=>sr.SubscriberId==userId&&sr.RoleId==roleId);
       }
       public async Task<SubscriberRoles> GetUserRole(int subscriberId)
       {
           return await msubscriberrolesrepository.GetAsync(subscriberId);
       }

       public async Task<IEnumerable<SubscriberRoleviewModel>> GetUserRoles(int subscriberId)
       {
           return
               await
                   msubscriberrolesrepository.FindAllAsync(s => s.SubscriberId == subscriberId,
                       sr => new SubscriberRoleviewModel
                       {
                           Id = sr.Id,
                           SubscriberId = sr.SubscriberId,
                           UserName = sr.Subscriber.Username,
                           RoleId = sr.RoleId,
                           RoleName = sr.role.Name,
                           Status = sr.Active ? "Active" : "InActive"
                       });
       }

       public async Task<int> SaveAsync()
       {
           return await msubscriberrolesrepository.SaveAsync();
       }
    }
}
