using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace DataServiceLibrary
{
    public class ContactService : IContactService
    {
        IGenericRepository<Contact> mcontactRepository;
        //  IGenericRepository<SubscriberStandards> mssRepository;
        IGenericRepository<SubscriberStandardContacts> msscRepository;
        public ContactService(IGenericRepository<Contact> contactRepository, IGenericRepository<SubscriberStandardContacts> sscRepository)
        {
            mcontactRepository = contactRepository;
            msscRepository = sscRepository;

        }

        public static Dictionary<string, string> ContactsearchFielddictionary()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("RollNo", "Contact.RollNo");
            dict.Add("Name", "Contact.Name");
            dict.Add("Mobile", "Contact.Mobile");
            dict.Add("BloodGroup", "Contact.BloodGroup");
            dict.Add("Class", "SubscriberStandards.Standard.Name");
            dict.Add("Section", "SubscriberStandardSections.SubscriberSection.Section.Name");
            return dict;
        }

        public static Dictionary<string, string> ContactsearchOperationdictionary()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("eq", "=");//'ne','lt','le','gt','ge','bw','bn','in','ni','ew','en','cn','nc'
            dict.Add("ne", "!=");
            dict.Add("le", "<");
            dict.Add("ge", ">");
            dict.Add("cn", "Like");
            return dict;
        }
        public static string getcolumnstr(JgGridParam jgGridParam)
        {
            string searhfield = ContactsearchFielddictionary()[jgGridParam.searchField];
            var intarr = new string[] {"Mobile"};
            string searchop = ContactsearchOperationdictionary()[jgGridParam.searchOper];
            return (searhfield + searchop + (intarr.Contains(jgGridParam.searchField) ? jgGridParam.searchString : ('"' + jgGridParam.searchString + '"')));
        }

        public async Task<IEnumerable<ContactViewModel>> Contacts(int subcriberId, JgGridParam jgGridParam)
        {
            int pageIndex = Convert.ToInt32(jgGridParam.page) - 1;
            int pageSize = jgGridParam.rows;
            string sort = jgGridParam.sord ?? "asc";
            string ordercolumn = jgGridParam.sidx;
            bool desc = sort.ToUpper() == "ASC";
            string where = "SubscriberStandards.SubscriberId =" + subcriberId;
            if (!string.IsNullOrEmpty(jgGridParam.searchField) && !string.IsNullOrEmpty(jgGridParam.searchOper)
                && !string.IsNullOrEmpty(jgGridParam.searchString))
            {
                string search = getcolumnstr(jgGridParam);
                where = where + " AND " + search;
            }
            Expression<Func<SubscriberStandardContacts, ContactViewModel>> select = s => new ContactViewModel
            {
                Section = s.SubscriberStandardSections.SubscriberSection.Section.Name,
                Class = s.SubscriberStandards.Standard.Name,
                Name = s.Contact.Name,
                BloodGroup = s.Contact.BloodGroup,
                Mobile = s.Contact.Mobile,
                RollNo = s.Contact.RollNo,
                Id = s.Id,
                SubscriberStandardId = s.SubscriberStandardsId,
                SubscriberStandardSectionId = s.SubscriberStandardSectionsId,
                Status = s.Active ? "Active" : "InActive"
            };
            return await msscRepository.GetPagedResult(pageSize * pageIndex, pageSize, ordercolumn, desc, select, where);
        }
        /* private static List<Filter> GetFilter(JgGridParam jgGridParam)
         {
             List<Filter> filter = new List<Filter>();
             int rollno = 0;
             if (!string.IsNullOrEmpty(jgGridParam.searchField))
             {
                 Filter instancefilter = new Filter { PropertyName = jgGridParam.searchField, Operation = Op.Equals };
                 if (jgGridParam.searchField == "RollNo")
                 {
                     // int.TryParse(jgGridParam.searchString, out rollno);
                     instancefilter.PropertyName = "Contact.RollNo";
                     instancefilter.Value = rollno;
                 }
                 else
                 {
                     instancefilter.Value = jgGridParam.searchString;
                 }
                 filter.Add(instancefilter);
             };
             return filter;
         }*/
        public async Task<int> TotalContacts(int subcriberId, JgGridParam jgGridParam)
        {
            string where = "SubscriberStandards.SubscriberId =" + subcriberId;
            if (!string.IsNullOrEmpty(jgGridParam.searchField) && !string.IsNullOrEmpty(jgGridParam.searchOper)
                && !string.IsNullOrEmpty(jgGridParam.searchString))
            {
                string search = getcolumnstr(jgGridParam);
                where = where + " AND " + search;
            }
            //Expression<Func<SubscriberStandardContacts, bool>> where = s => s.SubscriberStandards.SubscriberId == subcriberId && s.Active;
            return await msscRepository.CountAsync(where);
        }
        public async Task<Contact> AddContact(ContactViewModel contactvm)
        {
            var contact = new Contact();
            contact = GetContact(contactvm, contact);
            contact.Active = true;
            contact.CreatedDate = DateTime.Now;
            var ssc = new SubscriberStandardContacts
            {
                SubscriberStandardsId = contactvm.SubscriberStandardId,
                Active = true,
                SubscriberStandardSectionsId = contactvm.SubscriberStandardSectionId,
                CreatedAt = DateTime.Now,
                Contact = contact
            };
            var dbsave = await msscRepository.AddAsync(ssc);
            return contact;
            //return await mcontactRepository.AddAsync(contact);
        }

        public async Task<int> EditContact(ContactViewModel contactvm)
        {
            var ssscontact = await msscRepository.GetAsync(contactvm.Id);
            if (ssscontact == null)
            {
                throw new Exception("Unable to find student Contact details");
            }
            ssscontact.Contact = GetContact(contactvm, ssscontact.Contact);
            ssscontact.SubscriberStandardSectionsId = contactvm.SubscriberStandardSectionId;
            ssscontact.SubscriberStandardsId = contactvm.SubscriberStandardId;
            ssscontact.Active = contactvm.Status == "InActive" ? false : true;
            return await msscRepository.SaveAsync();
        }
        public Contact GetContact(ContactViewModel cv, Contact contact)
        {
            contact.Name = cv.Name;
            contact.Mobile = cv.Mobile;
            contact.RollNo = cv.RollNo;
            contact.BloodGroup = cv.BloodGroup;
            return contact;
        }
        public async Task<bool> IsUniqueMobile(long mobileno)
        {
            return await mcontactRepository.AnyAsync(s => s.Mobile == mobileno);
        }

        public async Task<SubscriberStandardContacts> FindContact(long Id)
        {
            return await msscRepository.GetAsync(Id);
        }
        public async Task<int> SaveAsync()
        {
            return await msscRepository.SaveAsync();
        }

        public async Task<bool> IsUniqueRollNo(int subscriberId, string rollNo)
        {
            return
                await
                    msscRepository.AnyAsync(
                        s => s.SubscriberStandards.SubscriberId == subscriberId && s.Contact.RollNo.Equals(rollNo));
        }

        public List<ContactViewModel> GetContactViewModels(DataTable dt)
        {
            var result = dt.AsEnumerable().AsParallel().Select(dr => new ContactViewModel()
            {
                RollNo = dr["RollNo"].ToString().Trim(),
                Name = dr["Name"].ToString().Trim(),
                Mobile = Convert.ToInt64(dr["Mobile"]),
                BloodGroup = dr["Blood Group"] == DBNull.Value ? string.Empty : dr["Blood Group"].ToString().Trim(),
                Class = dr["Class"].ToString().Trim(),
                Section = dr["Section"] == DBNull.Value ? string.Empty : dr["Section"].ToString().Trim()
            }).ToList();
            return result;
        }

        public async Task<ConcurrentBag<ErrorModal>> CheckExcelBuilkRollNoExistsTask(int subscriberId, List<ContactViewModel> lstContactViewModels)
        {
            var errorlist = new ConcurrentBag<ErrorModal>();
            var result = await GetSubscriberContact(subscriberId);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            lstContactViewModels.AsParallel().ForAll(
               cvm =>
               {
                   if (result.Any(r => r.RollNo.Trim() == cvm.RollNo && r.Class.Trim() == cvm.Class && r.Section.Trim() == cvm.Section))
                   {
                       if (!string.IsNullOrEmpty(cvm.Section))
                       {
                           errorlist.Add(new ErrorModal
                           {
                               ErrorMessage = "Duplicate found in Roll No",
                               ErrorDescription = string.Format(
                                   "Roll No {0} already exists in Class {1} Section {2}",
                                   cvm.RollNo, cvm.Class, cvm.Section)
                           });

                       }
                       else
                       {
                           errorlist.Add(new ErrorModal
                           {
                               ErrorMessage = "Duplicate found in Roll No",
                               ErrorDescription = string.Format(
                                  "Roll No {0} already exists in Class {1}",
                                  cvm.RollNo, cvm.Class)
                           });
                       }
                   }
               }
               );
            Debug.WriteLine("IsRollNoExists took " + sw.ElapsedMilliseconds);
            return errorlist;
        }
        public async Task<List<ContactViewModel>> ExcelBulkUploadContact(int subscriberId, List<ContactViewModel> excellstContactViewModels)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var contactlist = excellstContactViewModels.AsParallel().Select(c => new SubscriberStandardContacts
            {
                Contact = new Contact
                {
                    Active = true,
                    CreatedDate = DateTime.Now,
                    Name = c.Name,
                    BloodGroup = c.BloodGroup,
                    Mobile = c.Mobile,
                    RollNo = c.RollNo
                },
                Active = true,
                SubscriberStandardsId = c.SubscriberStandardId,
                SubscriberStandardSectionsId = c.SubscriberStandardSectionId,
                CreatedAt = DateTime.Now
            }).ToList();
            Debug.WriteLine("ExcelBulkUpdateClassSectionTask took " + sw.ElapsedMilliseconds);
            var result = await msscRepository.AddRangeAsyncWithReturnAll(contactlist);
            return result.Where(r => r.Id > 0).Select(r => new ContactViewModel
            {
                Name = r.Contact.Name,
                RollNo = r.Contact.RollNo,
                Mobile = r.Contact.Mobile
            }).ToList();
        }
        private async Task<List<ContactViewModel>> GetSubscriberContact(int subscriberId)
        {
            var result = await msscRepository.FindAllAsync(s => s.SubscriberStandards.SubscriberId == subscriberId,
                s =>
                    new ContactViewModel
                    {
                        RollNo = s.Contact.RollNo,
                        Class = s.SubscriberStandards.Standard.Name,
                        Section =
                            s.SubscriberStandardSections == null
                                ? string.Empty
                                : s.SubscriberStandardSections.SubscriberSection.Section.Name,
                        SubscriberStandardId = s.SubscriberStandardsId,
                        SubscriberStandardSectionId = s.SubscriberStandardSectionsId,
                    });
            return result.ToList();
        }
    }

}
