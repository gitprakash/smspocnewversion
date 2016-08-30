using DataServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using SMSPOCWeb.Models;
using System.Web.Security;
using System.Web.Script.Serialization;

namespace SMSPOCWeb.Controllers
{
    public class AccountController : Controller
    {
        IAccountService maccountService;
        public AccountController(IAccountService accountService)
        {
            maccountService = accountService;
        }
        public ActionResult Index()
        {
            return View();
        }




        public async Task<ActionResult> Register()
        {
            ViewBag.AccountTypeID = await maccountService.Accounttypes();
            ViewBag.GenderTypeID = await maccountService.Gendertypes();
            return View();
        }

        public Subscriber GetSubscriber(SubscriberViewModel subscriberviewmodel)
        {
            Subscriber subscriber = new Subscriber
            {
                AccountTypeId = subscriberviewmodel.AccountTypeId,
                Active = true,
                Email = subscriberviewmodel.Email,
                FirstName = subscriberviewmodel.FirstName,
                Mobile = subscriberviewmodel.Mobile,
                GenderTypeId = subscriberviewmodel.GenderTypeId,
                LastName = subscriberviewmodel.LastName,
                Password = subscriberviewmodel.Password,
                Username = subscriberviewmodel.Username,
                IsActivated = false
            };
            return subscriber;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Exclude = "IsActivated")]SubscriberViewModel subscriberviewmodel)
        {
            if (ModelState.IsValid)
            {
                Subscriber subscriber = GetSubscriber(subscriberviewmodel);
                var useradd = await maccountService.Add(subscriber);
                return RedirectToAction("Index", "Home");
            }
            ModelState.Remove("Password");
            return View(subscriberviewmodel);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel l, string ReturnUrl = "")
        {
            try
            {

                Tuple<bool, bool, bool, Subscriber> tupleuser = await maccountService.CheckLogin(l.Username, l.Password);
                // if  account not exists
                if (!tupleuser.Item1)
                {
                    ModelState.AddModelError("", "Invalid User,please register");
                }
                //if password not matched
                else if (!tupleuser.Item2)
                {
                    ModelState.AddModelError("", "Invalid Password");
                }
                //if account not activated
                else if (!tupleuser.Item3)
                {
                    ModelState.AddModelError("", "Please Activate Your Account");
                }
                else
                {
                    Subscriber suser = tupleuser.Item4;
                    SubscriberViewModel dbuser = new SubscriberViewModel { Id = suser.Id, Username = suser.Username, Email = suser.Email };

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string data = js.Serialize(dbuser);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, suser.Username, DateTime.Now, DateTime.Now.AddMinutes(20), l.RememberMe, data);
                    string encToken = FormsAuthentication.Encrypt(ticket);
                    HttpCookie authoCookies = new HttpCookie(FormsAuthentication.FormsCookieName, encToken);
                    Response.Cookies.Add(authoCookies);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Start", "Home");
                    }
                }
                ModelState.Remove("Password");
                return View(l);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(l);
            }
        }
        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> IsUserNameExists(string userName)
        {
            var userexists = await maccountService.IsUserNameExists(userName.Trim());
            if (userexists)
            {
                return Json(string.Format("User Name {0} already exists", userName), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> IsUserEmailExists(string email)
        {
            var emailexists = await maccountService.IsUserEmailExists(email.Trim());
            if (emailexists)
            {
                return Json(string.Format("Email {0} already exists", email), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> IsUniqueMobile(long mobile)
        {
            var emailexists = await maccountService.IsUniqueMobile(mobile);
            if (emailexists)
            {
                return Json(string.Format("Mobile number  {0} already exists", mobile), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}
