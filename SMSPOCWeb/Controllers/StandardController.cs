using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using System.Threading.Tasks;
using SMSPOCWeb.Models;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
    public class StandardController : Controller
    {
        private Model1 db = new Model1();
        // GET: /Standard/
        public ActionResult Index()
        {
            var authuser = ((CustomIdentity)User.Identity).User.Id;
            var subscriberstandards = db.SubscriberStandards.Where(s=>s.SubscriberId==authuser)
                .Include(s => s.Standard)
                .Include(s => s.Subscriber);
            return View(subscriberstandards.ToList());
        }
 
        // GET: /Standard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Standard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClassViewModel classvm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var authuser = ((CustomIdentity)User.Identity).User.Id;
                    if (await db.SubscriberStandards.AnyAsync(ss => ss.Standard.Name.Equals(classvm.ClassName) && ss.SubscriberId==authuser))
                          throw new Exception("Standard already exists");
                    var standard = new Standard { Name = classvm.ClassName};
                    var subscriberclasses = new SubscriberStandards
                    {
                        Active = true,
                        CreatedAt = DateTime.Now,
                        Guid = Guid.NewGuid(),
                        SubscriberId = authuser,
                        Standard = standard
                    };
                    
                   db.SubscriberStandards.Add(subscriberclasses);
                   db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(classvm);
        }

        // GET: /Standard/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subscriberstandards =await db.SubscriberStandards.FindAsync(id);
            if (subscriberstandards == null)
            {
                return HttpNotFound();
            }
            var classvm = new ClassViewModel
            {
                ClassName = subscriberstandards.Standard.Name,
                Id = subscriberstandards.Id,
                Active = subscriberstandards.Active

            };
            return View(classvm);
        }

        // POST: /Standard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClassViewModel classvm)
        {
            var authuser = ((CustomIdentity)User.Identity).User.Id;
            if (ModelState.IsValid)
            {
                try
                {
                    if (await db.SubscriberStandards.AnyAsync(ss => ss.Standard.Name.Equals(classvm.ClassName)
                                                                    && ss.SubscriberId == authuser
                                                                    && ss.Id != classvm.Id))
                        throw new Exception(string.Format("Standard{0}already exists", classvm.ClassName));
                    var dbclass = await db.SubscriberStandards.FindAsync(classvm.Id);
                    if (dbclass != null)
                    {
                        dbclass.Standard.Name = classvm.ClassName;
                        dbclass.Active = classvm.Active;
                        db.SaveChanges(); 
                    }
                    else
                    {
                        throw new Exception(string.Format("Unabe to find Standard {0} details, try again", classvm.ClassName));
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
           
            return View(classvm);
        }

        // GET: /Standard/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriberStandards subscriberstandards = db.SubscriberStandards.Find(id);
            if (subscriberstandards == null)
            {
                return HttpNotFound();
            }
            return View(subscriberstandards);
        }

        // POST: /Standard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubscriberStandards subscriberstandards = db.SubscriberStandards.Find(id);
            subscriberstandards.Active = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
