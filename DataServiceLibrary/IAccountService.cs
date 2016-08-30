using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface IAccountService
    {
          Task<IEnumerable<AccountType>> Accounttypes();
          Task<IEnumerable<GenderType>> Gendertypes();
          Task<Subscriber> Add(Subscriber role);
          Subscriber Finduser(string username);
          Task<bool> IsUserNameExists(string username);
          Task<bool> IsUserEmailExists(string email);
          Task<bool> IsUniqueMobile(long mobileno);
          Task<Tuple<bool, bool, bool, Subscriber>> CheckLogin(string username, string password);
          Task<bool> FinduserAsync(int subscriberId=0,string username="");
          Task<Subscriber> GetuserAsync(int subscriberId = 0, string username = "");
          Task<SubscriberRoleviewModel[]> GetUserRole();
          Task<int> TotalUserRoles();
        Task<IEnumerable<Tuple<int, string>>> GetAllUsers();

    }
}
