using DataModelLibrary;
using DataServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUserRolesController : Controller
    {
        IUserRoleService muserroleService;
        IAccountService maccountService;
        IRoleService mroleService;
        public ManageUserRolesController(IUserRoleService userroleService,
            IRoleService roleService,
            IAccountService accountService)
        {
            muserroleService = userroleService;
            mroleService = roleService;
            maccountService = accountService;
        }
        public ActionResult GetUserRolesView()
        {
            return View("UserRoles");
        }

        public async Task<JsonResult> UserRoles(string sidx, string sort, int page, int rows)
        {
            sort = sort ?? "asc";
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var usersrolelist = await maccountService.GetUserRole();
            int totalRecords = await maccountService.TotalUserRoles();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = usersrolelist
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> Add(SubscriberRoleviewModel subscriberRoleviewModel)
        {
            try
            {
                await ValidateUserDetails(subscriberRoleviewModel);
                if (await muserroleService.CheckExists(subscriberRoleviewModel.SubscriberId, subscriberRoleviewModel.RoleId))
                {
                    throw new Exception(string.Format("User {0} and role {1} already mapped", subscriberRoleviewModel.UserName, subscriberRoleviewModel.RoleName));
                }
                SubscriberRoles sroles = new SubscriberRoles
                {
                    RoleId = subscriberRoleviewModel.RoleId,
                    SubscriberId = subscriberRoleviewModel.SubscriberId,
                    Active = subscriberRoleviewModel.Status == "Active" ? true : false
                };
                await muserroleService.AddUserRole(sroles);
                var result = new { Status = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task ValidateUserDetails(SubscriberRoleviewModel subscriberRoleviewModel)
        {
            bool isUserExists = await maccountService.FinduserAsync(subscriberRoleviewModel.SubscriberId);
            bool isRoleExistis = await mroleService.FindRoleAsync(subscriberRoleviewModel.RoleId);
            if (!isUserExists)
            {
                throw new Exception(string.Format("user {0} not exists", subscriberRoleviewModel.UserName));
            }
            if (!isRoleExistis)
            {
                throw new Exception(string.Format("Role {0} not exists", subscriberRoleviewModel.RoleName));
            }
        }

        [HttpPost]
        public async Task<JsonResult> Edit(SubscriberRoleviewModel subscriberRoleviewModel)
        {
            try
            {
                var status=subscriberRoleviewModel.Status == "Active" ? true : false;
                await ValidateUserDetails(subscriberRoleviewModel);
                var userRoles = await muserroleService.GetUserRoles(subscriberRoleviewModel.SubscriberId);
                if (
                    userRoles.Any(
                        ur => ur.RoleId == subscriberRoleviewModel.RoleId && ur.Id != subscriberRoleviewModel.Id))
                {
                    throw new Exception(string.Format("User {0} and role {1} already mapped",
                        subscriberRoleviewModel.UserName, subscriberRoleviewModel.RoleName));
                }
                if (
                    userRoles.Any(
                        ur =>
                            ur.RoleId == subscriberRoleviewModel.RoleId && ur.Id == subscriberRoleviewModel.Id &&
                            ur.Status == subscriberRoleviewModel.Status))
                {
                    throw new Exception("No Change found");
                }
                var userRole = await muserroleService.GetUserRole(subscriberRoleviewModel.Id);
                userRole.Active = status;
                userRole.SubscriberId = subscriberRoleviewModel.SubscriberId;
                userRole.RoleId = subscriberRoleviewModel.RoleId;
                await muserroleService.SaveAsync();
                var result = new { Status = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await maccountService.GetAllUsers();
            return Json(users.Select(u => new { Id = u.Item1, Name = u.Item2 }), JsonRequestBehavior.AllowGet);
        }
    }
}