using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class AccountService : IAccountService
    {
        IGenericRepository<Subscriber> msubscriberrepository;
        IGenericRepository<AccountType> maccountyprepository;
        IGenericRepository<GenderType> mgendertyprepository;
        IGenericRepository<SubscriberRoles> msubscriberrolesrepository;
        public AccountService(IGenericRepository<AccountType> accountyprepository,
            IGenericRepository<GenderType> gendertyprepository,
            IGenericRepository<Subscriber> subscriberrepository,
            IGenericRepository<SubscriberRoles> subscriberrolesrepository)
        {
            mgendertyprepository = gendertyprepository;
            maccountyprepository = accountyprepository;
            msubscriberrepository = subscriberrepository;
            msubscriberrolesrepository = subscriberrolesrepository;
        }
        public async Task<IEnumerable<AccountType>> Accounttypes()
        {
            return await maccountyprepository.GetAllAsync();
        }

        public async Task<IEnumerable<GenderType>> Gendertypes()
        {
            return await mgendertyprepository.GetAllAsync();
        }

        public async Task<Subscriber> Add(Subscriber subscriber)
        {
            return await msubscriberrepository.AddAsync(subscriber);
        }


        public async Task<bool> IsUserNameExists(string username)
        {
            return await msubscriberrepository.AnyAsync(s => s.Username == username);
        }
        public async Task<bool> IsUserEmailExists(string email)
        {
            return await msubscriberrepository.AnyAsync(s => s.Email == email);

        }
        public async Task<bool> IsUniqueMobile(long mobileno)
        {
            return await msubscriberrepository.AnyAsync(s => s.Mobile == mobileno);
        }
        public async Task<Tuple<bool, bool, bool, Subscriber>> CheckLogin(string username, string password)
        {
            bool isuserexits = false, ispasswordmatch = false, isactivated = false;
            var user = await msubscriberrepository.FindAsync(s => s.Username == username);
            if (user != null)
            {
                isuserexits = true;
                if (user.Password.Trim() == password.Trim())
                {
                    ispasswordmatch = true;
                }
                isactivated = user.IsActivated;
            }
            return Tuple.Create(isuserexits, ispasswordmatch, isactivated, user);
        }
        public Subscriber Finduser(string username)
        {
            return msubscriberrepository.Find(s => s.Username.Equals((username)));
        }
        public Subscriber Finduser(string username, bool includechild)
        {
            //if (includechild)
            //{
            //    Expression<Func<Subscriber, bool>> where = s => s.Username == username;
            //    var lstexrpess = new Expression<Func<Subscriber, object>>[1];

            //    Expression<Func<Subscriber, object>> childlist = s => s.Roles;
            //    return msubscriberrepository.GetAllLazyLoad(where, lstexrpess);
            //}
            return msubscriberrepository.Find(s => s.Username.Equals((username)));
        }
        public async Task<bool> FinduserAsync(int subscriberId = 0, string username = "")
        {
            if (subscriberId > 0)
                return await msubscriberrepository.AnyAsync(s => s.Id == subscriberId);
            if (!string.IsNullOrEmpty(username))
                return await msubscriberrepository.AnyAsync(s => s.Username == username);
            else
                return false;
        }
        public async Task<Subscriber> GetuserAsync(int subscriberId = 0, string username = "")
        {
            if (subscriberId > 0)
                return await msubscriberrepository.FindAsync(s => s.Id == subscriberId);
            if (!string.IsNullOrEmpty(username))
                return await msubscriberrepository.FindAsync(s => s.Username == username);
            else
                return null;
        }

        public async Task<SubscriberRoleviewModel[]> GetUserRole()
        {
            var result = await msubscriberrolesrepository.ToArrayAsync(sr => new SubscriberRoleviewModel
            {
                Id = sr.Id,
                SubscriberId = sr.SubscriberId,
                UserName = sr.Subscriber.Username,
                RoleId = sr.RoleId,
                RoleName = sr.role.Name,
                Status = sr.Active ? "Active" : "InActive"
            });
            return result;
        }
        public async Task<int> TotalUserRoles()
        {
            return await msubscriberrolesrepository.CountAsync();
        }
        public async Task<IEnumerable<Tuple<int,string>>> GetAllUsers()
        {
            var users= await msubscriberrepository.ToArrayAsync(s => new {UserName=s.Username,Id=s.Id});
            return users.Select(u => new Tuple<int, string>(u.Id, u.UserName));
        }

    }
}
