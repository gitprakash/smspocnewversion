using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using DataServiceLibrary;
using SMSPOCWeb.Models;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
    public class TemplateController : Controller
    {
        // GET: Template
        private ITemplateService mtemplateService;
        public TemplateController(ITemplateService templateService)
        {
            mtemplateService = templateService;
        }

        public ActionResult TemplateView()
        {
            return View();
        }

        //public async Task<JsonResult> GetTemplates()
        //{
        //    var identity = (CustomIdentity)User.Identity;
        //    var alltemplate = await mtemplateService.GetTemplates(identity.User.Id);
        //    return Json(alltemplate, JsonRequestBehavior.AllowGet);
        //}

        public async Task<JsonResult> Index(JgGridParam jgGridParam)
        {
            var identity = (CustomIdentity)User.Identity;
           // sort = jgGridParam.sort ?? "asc";
           // int pageIndex = Convert.ToInt32(jgGridParam.page) - 1;
            //int pageSize = jgGridParam.rows;
            var contacts = await mtemplateService.GetPagedTemplates(identity.User.Id, jgGridParam);
            int totalRecords = await mtemplateService.TotalTemplates(identity.User.Id);
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)jgGridParam.rows);
            var jsonData = new
            {
                total = totalPages,
                jgGridParam.page,
                records = totalRecords,
                rows = contacts
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> Add([Bind(Exclude = "Id")]TemplateViewModel templatevm)
        {
            try
            {
                SubscriberTemplate subscriberTemplate;
                if (ModelState.IsValid)
                {
                    var identity = (CustomIdentity)User.Identity;
                    var st = await mtemplateService.FindTemplate(identity.User.Id, templatename: templatevm.Name);
                    if (st != null)
                    {
                        throw new Exception("Template already exists");
                    }
                    subscriberTemplate = await mtemplateService.AddTemplate(templatevm, identity.User.Id);
                    var resultstatus = new { Status = "success", Id = subscriberTemplate.Id };
                    return Json(resultstatus, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public async Task<JsonResult> Edit(TemplateViewModel templatevm)
        {
            try
            {
                SubscriberTemplate subscriberTemplate;
                if (ModelState.IsValid)
                {
                    var identity = (CustomIdentity)User.Identity;
                    var st = await mtemplateService.FindTemplate(identity.User.Id, templatevm.Id);
                    if (st == null)
                    {
                        throw new Exception("Template not already exists");
                    }
                    st.Templates.Name = templatevm.Name;
                    st.Templates.Description = templatevm.Description;
                    st.Active = templatevm.Status == "Active" ? true : false;
                    int saved = await mtemplateService.SaveAsync();
                    var resultstatus = new { Status = "success", Id = saved };
                    return Json(resultstatus, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<JsonResult> Delete(int Id)
        {
            try
            {
                SubscriberStandardContacts contact;
                if (ModelState.IsValid)
                {
                    var identity = (CustomIdentity)User.Identity;
                    var st = await mtemplateService.FindTemplate(identity.User.Id, Id);
                    if (st == null)
                    {
                        throw new Exception("Template not already exists");
                    }

                    st.Active = false;
                   int saved = await mtemplateService.SaveAsync();
                    var resultstatus = new { Status = "success", Id = saved };
                    return Json(resultstatus, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }


            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetModelStateError()
        {
            string messages = string.Join("; ", ModelState.Values
                                .SelectMany(x => x.Errors)
                                .Select(x => x.ErrorMessage));
            return messages;
        }


    }
}