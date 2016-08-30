using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using DataServiceLibrary;
using Newtonsoft.Json.Schema;
using SMSPOCWeb.Models;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
    public class NotifyController : AsyncController
    {
        // GET: Notify
        IMessageService m_messageService;
        public NotifyController(IMessageService messageService)
        {
            m_messageService = messageService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> SendMessage(List<MessageViewModel> messageViewModel,string message,int messagecount)
        {
            try
            {
                 var identity = (CustomIdentity)User.Identity;
                 var msgstatusviewmodel = new List<MessageViewModel>();
                 if (messageViewModel != null && messagecount >= 1 && !string.IsNullOrEmpty(message))
                 {
                     if (ModelState.IsValid)
                     {
                         if (!await m_messageService.CheckMessageBalance(messageViewModel.Count(), messagecount, identity.User.Id))
                         {
                             throw new Exception("Insufficient Message Balance, Contact Administator to update Your Package");
                         }
                         msgstatusviewmodel =  await m_messageService.SendMessage(messageViewModel, message, messagecount,identity.User.Id);
                     }
                     else
                     {
                         string messages = GetModelStateError();
                         throw new Exception(messages);
                     }
                     var jsonresult = new {SuccessResult= msgstatusviewmodel, Status = true, JsonRequestBehavior.AllowGet };
                     return Json(jsonresult, JsonRequestBehavior.AllowGet);
                 }
                 else
                 {
                     throw new Exception("Invalid inputs, check your selected contact, message details");
                 }

            }
            catch (Exception ex)
            {
                var result = new { Status = false, ErrorResult = ex.GetBaseException().Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SendSms()
        {
            return View();
        }

        public ActionResult GetMessageHistoryView()
        {
            return View();
        }

        public async Task<JsonResult> GetMessageHistory(JgGridParam jgGridParam)
        {
            var identity = (CustomIdentity)User.Identity;
            if (jgGridParam != null)
            {
                var messages = await m_messageService.MessageHistory(jgGridParam, identity.User.Id);
                int totalRecords = await m_messageService.TotalMessageHistory(identity.User.Id);
                var totalPages = (int) Math.Ceiling((float) totalRecords/(float) jgGridParam.rows);
                var jsonData = new
                {
                    total = totalPages,
                    jgGridParam.page,
                    records = totalRecords,
                    rows = messages
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            throw  new Exception("invalid inputs ");
        }

        public async Task<JsonResult> Resend(Guid Id)
        {
            var identity = (CustomIdentity)User.Identity;
            int count = await m_messageService.ResendMessage(identity.User.Id,Id);
            var resultstatus = new { Status = "success", Id = count };
            return Json(resultstatus, JsonRequestBehavior.AllowGet);
        }
        private string GetModelStateError()
        {
            string messages = string.Join("; ", ModelState.Values
                                .SelectMany(x => x.Errors)
                                .Select(x => x.ErrorMessage));
            return messages;
        }

        public async Task<JsonResult> GetSubscriberSMS()
        {
            var identity = (CustomIdentity)User.Identity;
            var messagetuple = await m_messageService.GetMessageBalance(identity.User.Id);
            var result = new { Openingsms=messagetuple.Item1,balancesms=messagetuple.Item2};
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}