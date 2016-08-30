using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public interface IRoleService
    {
        Task<Role> Add(Role role);
        Task<Role> Edit(Role role);
        Task<int> Delete(Role role);
        Task<bool> FindRoleAsync(int roleId=0,string name="");
        Task<IEnumerable<Role>> GetRoles(int skip, int pagesize, string ordercolumn,bool desc);
        Task<int> TotalRoles();
        Task<IEnumerable<Tuple<int, string>>> GetAllRoles();
        Task<bool> IsRoleExists(string name);
    }
}
