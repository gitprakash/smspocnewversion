using DataModelLibrary;
using DataServiceLibrary;
using SMSPOCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Excel;
using System.Collections.Concurrent;
using System.Web.Script.Serialization;

namespace SMSPOCWeb.Controllers
{
    [Authorize(Roles = "Subscriber")]
    public class ContactController : Controller
    {
        IContactService mcontactService;
        ISubscriberStandardService mclassService;
        public ContactController(IContactService contactService, ISubscriberStandardService classService)
        {
            mcontactService = contactService;
            mclassService = classService;
        }
        public ActionResult GetContactView()
        {
            return View();
        }
        public ActionResult UploadView()
        {
            return View("UploadStudent");
        }
        public async Task<JsonResult> Index(JgGridParam jgGridParam)
        {
            try
            {

                var identity = (CustomIdentity)User.Identity;
                var contacts = await mcontactService.Contacts(identity.User.Id, jgGridParam);
                int totalRecords = await mcontactService.TotalContacts(identity.User.Id, jgGridParam);
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
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> Add([Bind(Exclude = "Id")]ContactViewModel contactvm)
        {
            try
            {
                Contact contact;
                if (ModelState.IsValid)
                {
                    contact = await mcontactService.AddContact(contactvm);
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }
                var resultstatus = new { Status = "success", Id = contact.Id };
                return Json(resultstatus, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Edit(ContactViewModel contactvm)
        {
            try
            {
                int saved = 0;
                if (ModelState.IsValid)
                {
                    saved = await mcontactService.EditContact(contactvm);
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }
                var resultstatus = new { Status = "success", Id = saved };
                return Json(resultstatus, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = new { Status = "error", error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int Id)
        {
            try
            {
                SubscriberStandardContacts contact;
                int saved = 0;
                if (ModelState.IsValid)
                {
                    contact = await mcontactService.FindContact(Id);
                    if (contact == null)
                    {
                        throw new Exception("Unable to find student contact details");
                    }
                    contact.Active = false;
                    contact.Contact.Active = false;
                    saved = await mcontactService.SaveAsync();
                }
                else
                {
                    string messages = GetModelStateError();
                    throw new Exception(messages);
                }
                var resultstatus = new { Status = "success", Id = saved };
                return Json(resultstatus, JsonRequestBehavior.AllowGet);

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
        public Contact GetContact(ContactViewModel cv, Contact contact)
        {
            contact.Name = cv.Name;
            contact.Mobile = cv.Mobile;
            contact.RollNo = cv.RollNo;
            contact.BloodGroup = cv.BloodGroup;
            return contact;
        }
        public async Task<JsonResult> GetStandards()
        {
            var identity = (CustomIdentity)User.Identity;
            var Standards = await mclassService.GetStandards(identity.User.Id);
            return Json(Standards, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetSections(int subscriberStandardId)
        {
            var Sections = await mclassService.GetSections(subscriberStandardId);
            return Json(Sections, JsonRequestBehavior.AllowGet);
        }


        private ErrorModal GetErrorModal(string errormsg, string errordesc)
        {
            return new ErrorModal
            {
                ErrorMessage = errormsg,
                ErrorDescription = errordesc
            };
        }
        public JsonResult GetJsonErrorResult(string errormsg, string errordesc)
        {
            var jsonresult = new
            {
                Status = "error",
                error = new List<ErrorModal>
                                    {
                                        GetErrorModal(errormsg,errordesc)
                                    }
            };
            return Json(jsonresult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<BulkUploadStudentJsonModel> ValidateExcelFile(HttpPostedFileBase upload)
        {
            var errorlist = new ConcurrentBag<ErrorModal>();
            var cvmresult = new List<ContactViewModel>();
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    if (upload.FileName.EndsWith(".xls") || upload.FileName.EndsWith(".xlsx"))
                    {
                        var result = GetDataSetfromExcel(upload);
                        var tupledsstatus = result.ValidateStudentTemplate();
                        if (tupledsstatus.Count()>0)
                        {
                            errorlist.AddRange(tupledsstatus);
                        }
                        cvmresult = mcontactService.GetContactViewModels(result.Tables[0]);
                        var identity = (CustomIdentity)User.Identity;
                        var lstexistrollno = await mcontactService.CheckExcelBuilkRollNoExistsTask(identity.User.Id, cvmresult);
                        if (lstexistrollno.Count > 0)
                        {
                            errorlist.AddRange(lstexistrollno);
                        }
                    }
                    else {
                        var errormodal = GetErrorModal("File format issue", "This file format is not supported, it should be xls , xlsx");
                        errorlist.Add(errormodal);
                    }
                }
                else
                {
                    var errormodal = GetErrorModal("Problem with input file ", "Please upload valid file");
                    errorlist.Add(errormodal);
                }
            }
            catch (Exception ex)
            {
                var errormodal = GetErrorModal("Exception occured ", ex.Message);
                errorlist.Add(errormodal);
            }
            return (new BulkUploadStudentJsonModel
            {
                Status = (errorlist.Count == 0) ? true : false,
                SuccessResult = (errorlist.Count == 0) ? cvmresult : new List<ContactViewModel>(),
                ErrorResult = errorlist
            });
        }
        private DataSet GetDataSetfromExcel(HttpPostedFileBase upload)
        {
            using (Stream stream = upload.InputStream)
            {
                IExcelDataReader reader = null;
                if (upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                reader.IsFirstRowAsColumnNames = true;
                DataSet result = reader.AsDataSet();
                reader.Close();
                return result;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveExcelFile(HttpPostedFileBase upload)
        {
            try
            {
                var validatefielresult = await ValidateExcelFile(upload);
                if (validatefielresult.Status)
                {
                    var cvmresult = validatefielresult.SuccessResult;
                    var identity = (CustomIdentity)User.Identity; 
                    var classadded = await mclassService.AddBulkClassifNotExists(cvmresult, identity.User.Id);
                    var classbulksection = await mclassService.AddBulkSectionsifNotExists(cvmresult, identity.User.Id);
                    var classsectionlink = await mclassService.AddBulkClassSectionLinkIfNotExists(cvmresult, identity.User.Id);
                    await mclassService.ExcelBulkUpdateClassSectionTask(identity.User.Id, cvmresult);
                    var contacts = await mcontactService.ExcelBulkUploadContact(identity.User.Id, cvmresult);
                    var jsonresult = new { Status = "success", result = contacts };
                    return Json(new BulkUploadStudentJsonModel
                    {
                        Status = true,
                        SuccessResult = contacts
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                { 
                    return Json(validatefielresult, JsonRequestBehavior.AllowGet);
                } 
            }
            catch (Exception ex)
            {
                return Json(new BulkUploadStudentJsonModel
                {
                    Status = false,
                    ErrorResult = new ConcurrentBag<ErrorModal>() { new ErrorModal { ErrorMessage = "Exception occured", ErrorDescription = ex.Message } }
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}