using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataModelLibrary;
using SMSPOCWeb.Models;

namespace SMSPOCWeb.Controllers
{
    public class SectionController : Controller
    {
        private Model1 db = new Model1();

        // GET: Section
        public async Task<ActionResult> Index()
        {
            var svm = await db.SubscriberSection.ToListAsync();
            var svmlist = svm.Select(s => new SectionViewModel {Id = s.Id, Name = s.Section.Name, Active = s.Active});
            return View(svmlist);
        }

        

        // GET: Section/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] SectionViewModel sectionViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await db.SubscriberSection.AnyAsync(s => s.Section.Name == sectionViewModel.Name))
                    {
                        throw new Exception("Section already exists");
                    }
                    var authuser = ((CustomIdentity)User.Identity).User.Id;
                    var section = new Section { Name = sectionViewModel.Name };
                    var subscribersection = new SubscriberSection
                    {
                        CreatedAt = DateTime.Now,
                        SubscriberId = authuser,
                        Guid = Guid.NewGuid(),
                        Section = section,
                        Active = true
                    };
                    db.SubscriberSection.Add(subscribersection);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                       ModelState.AddModelError("", ex.Message);
                }
            }
            return View(sectionViewModel);
        }

        // GET: Section/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sections = await db.SubscriberSection.FindAsync(id);
            if (sections == null)
            {
                return HttpNotFound();
            }
            var svm = SectionViewModel(sections);
            return View(svm);
        }

        private static SectionViewModel SectionViewModel(SubscriberSection sections)
        {
            var svm = new SectionViewModel {Id = sections.Id, Name = sections.Section.Name, Active = sections.Active};
            return svm;
        }

        // POST: Section/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SectionViewModel sectionViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var authuser = ((CustomIdentity)User.Identity).User.Id;
                    if (await db.SubscriberSection.AnyAsync(ss => ss.Section.Name.Equals(sectionViewModel.Name)
                                                                       && ss.SubscriberId == authuser
                                                                       && ss.Id != sectionViewModel.Id))
                        throw new Exception(string.Format("Standard{0}already exists", sectionViewModel.Name));

                    var dbsection = await db.SubscriberSection.FindAsync(sectionViewModel.Id);
                    if (dbsection != null)
                    {
                        dbsection.Section.Name = sectionViewModel.Name;
                        dbsection.Active = sectionViewModel.Active;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception(string.Format("Unabe to find Standard {0} details, try again", sectionViewModel.Name));
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(sectionViewModel);
        }

        // GET: Section/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sections = await db.SubscriberSection.FindAsync(id);
            if (sections == null)
            {
                return HttpNotFound();
            }
            var svm = SectionViewModel(sections);
            return View(svm);
        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var sections = await db.SubscriberSection.FindAsync(id);
            sections.Active = false;
            await db.SaveChangesAsync();
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
