using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class RoleService:IRoleService
    {
        IGenericRepository<Role> mrole;
        public RoleService(IGenericRepository<Role> role)
        {
            mrole = role;
        }
        public async Task<Role> Add(Role role)
        {
           return await mrole.AddAsync(role);
        }
        public async Task<Role> Edit(Role role)
        {
            return await mrole.UpdateAsync(role,role.Id);
        }
        public async Task<int> Delete(Role role)
        {
            return await mrole.DeleteAsync(role);
        }
        public async Task<bool> FindRoleAsync(int roleId=0,string name="")
        {
             if (roleId > 0)
                return await mrole.AnyAsync(s => s.Id == roleId);
            if (!string.IsNullOrEmpty(name))
                return await mrole.AnyAsync(s => s.Name == name);
            else
                return false;
        }
       public async Task<IEnumerable<Role>> GetRoles(int skip, int pagesize,string ordercolumn,bool desc)
        {
            return await mrole.GetPagedResult(skip, pagesize, ordercolumn,desc);
        }
       public async Task<int> TotalRoles()
       {
           return await mrole.CountAsync();
       }

        public async Task<IEnumerable<Tuple<int, string>>> GetAllRoles()
        {
            var users = await mrole.ToArrayAsync(s => new { Name = s.Name, Id = s.Id });
            return users.Select(u => new Tuple<int, string>(u.Id, u.Name));
        }

        public async Task<bool> IsRoleExists(string name)
       {
           return await mrole.AnyAsync(r => r.Name.Equals(name));
       }

    }
}
