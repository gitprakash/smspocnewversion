using DataModelLibrary;
using DataServiceLibrary;
using SMSPOCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SMSPOCWeb.Controllers
{
     [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        // GET: Role
        IRoleService mroleService;
        public RoleController(IRoleService roleService)
        {
            mroleService = roleService;
        }

        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult GetRoleView()
        {
            return View("Index");
        }

        public async Task<JsonResult> Index(string sidx, string sort, int page, int rows)
        {
            sort = sort ?? "asc";
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var Rolelist = await mroleService.GetRoles(pageIndex * pageSize, pageSize, sidx, sort.ToUpper() == "DESC");
            int totalRecords = await mroleService.TotalRoles();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = Rolelist
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Create(Role Role)
        {
            try
            {
                Role.CreatedBy = "prakash";
                Role.CreatedDate = DateTime.Now;
                Role dbrole= await mroleService.Add(Role);
                return dbrole.Name;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResult> GetAllRoles()
        {
            var roles = await mroleService.GetAllRoles();
            return Json(roles.Select(u => new { Id = u.Item1, Name = u.Item2 }), JsonRequestBehavior.AllowGet);
        }
    }
}