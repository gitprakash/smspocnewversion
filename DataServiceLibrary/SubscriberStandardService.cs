using DataModelLibrary;
using Repositorylibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary
{
    public class SubscriberStandardService : ISubscriberStandardService
    {
        IGenericRepository<SubscriberStandards> mclassRepository;
        IGenericRepository<SubscriberSection> msubscribersectionRepository;
        IGenericRepository<SubscriberStandardSections> msectionRepository;
        public SubscriberStandardService(IGenericRepository<SubscriberStandards> classRepository,
            IGenericRepository<SubscriberStandardSections> sectionRepository,
             IGenericRepository<SubscriberSection> subscribersectionRepository)
        {
            mclassRepository = classRepository;
            msectionRepository = sectionRepository;
            msubscribersectionRepository = subscribersectionRepository;
        }
        public async Task<ICollection<SubscriberStandardViewModel>> GetStandards(int subscriberId)
        {
            Expression<Func<SubscriberStandards, SubscriberStandardViewModel>> select = s => new SubscriberStandardViewModel { SubscriberStandardId = s.Id, StandardName = s.Standard.Name };
            Expression<Func<SubscriberStandardViewModel, string>> orderby = s => s.StandardName;
            return await mclassRepository.FindAllAsync(c => c.SubscriberId == subscriberId && c.Active, select, orderby);
        }
        public async Task<ICollection<SubscriberStandardSectionViewModel>> GetSections(int subscirberStandardId)
        {
            Expression<Func<SubscriberStandardSections, SubscriberStandardSectionViewModel>> select = s => new SubscriberStandardSectionViewModel
            {
                SubscriberStandardId = s.SubscriberStandardsId,
                SubscriberStandardSectionId = s.Id,
                SectionName = s.SubscriberSection.Section.Name
            };
            Expression<Func<SubscriberStandardSectionViewModel, string>> orderby = s => s.SectionName;
            return await msectionRepository.FindAllAsync(c => c.SubscriberStandardsId == subscirberStandardId && c.Active, select, orderby);
        }

        public async Task<List<string>> GetClassListTask(int subscriberId)
        {
            Expression<Func<SubscriberStandards, bool>> whereExpression = ss => ss.SubscriberId == subscriberId;
            var result = await mclassRepository.FindAllAsync(whereExpression, ss => ss.Standard.Name);
            return result.ToList();
        }

        public async Task<List<string>> GetSectionListTask(int subscriberId)
        {
            Expression<Func<SubscriberSection, bool>> whereExpression = ss => ss.SubscriberId == subscriberId;
            var result = await msubscribersectionRepository.FindAllAsync(whereExpression, ss => ss.Section.Name);
            return result.ToList();
        }

        public async Task<List<SubscriberStandards>> AddBulkClassifNotExists(List<ContactViewModel> lstContactViewModels, int subscriberId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var excelclasslist = lstContactViewModels.Select(c => c.Class.Trim()).Distinct().ToList();
            var dbclass = await GetClassListTask(subscriberId);
            var newclass = excelclasslist.Where(n => dbclass.Contains(n) == false).AsParallel().ToList();
            var newdbclasslist = newclass.Select(c => new SubscriberStandards
            {
                SubscriberId = subscriberId,
                Standard = new Standard { Name = c },
                Active = true,
                CreatedAt = DateTime.Now,
                Guid = Guid.NewGuid()
            }).AsParallel().ToList();
            if (newdbclasslist.Count > 0)
            {
                await mclassRepository.AddRangeAsync(newdbclasslist);
            }
            Debug.WriteLine("AddBulkClassifNotExists took " + sw.ElapsedMilliseconds);
            return newdbclasslist;
        }
        public async Task<List<SubscriberSection>> AddBulkSectionsifNotExists(List<ContactViewModel> lstContactViewModels, int subscriberId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var excelsectionlist = lstContactViewModels.Where(c => c.Section != string.Empty).Select(c => c.Section).Distinct().ToList();
            var dbsections = await GetSectionListTask(subscriberId);
            var newclass = excelsectionlist.Where(n => dbsections.Contains(n) == false).AsParallel().ToList();
            var newdbsectionlist = newclass.Select(c => new SubscriberSection
            {
                SubscriberId = subscriberId,
                Section = new Section { Name = c },
                Active = true,
                CreatedAt = DateTime.Now,
                Guid = Guid.NewGuid()
            }).AsParallel().ToList();
            if (newdbsectionlist.Count > 0)
            {
                await msubscribersectionRepository.AddRangeAsync(newdbsectionlist);
            }
            Debug.WriteLine("AddBulkSectionsifNotExists took " + sw.ElapsedMilliseconds);

            return newdbsectionlist;
        }

        public async Task<List<Tuple<string, string>>> AddBulkClassSectionLinkIfNotExists(List<ContactViewModel> lstContactViewModels, int subscriberId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var uniqueclasssectionlst =
                lstContactViewModels
                .Where(c => c.Class != string.Empty && c.Section != string.Empty)
                .Select(c =>
                new
                {
                    Class = c.Class,
                    Section = c.Section
                })
                .Distinct();
            var dbclasssectionlst = await GetClassSectionListTask(subscriberId, s =>
                        new
                        {
                            Class = s.SubscriberStandards.Standard.Name,
                            SectionName = s.SubscriberSection.Section.Name
                        });
            var filternewclasssection = uniqueclasssectionlst
                .Where(c => dbclasssectionlst
                .Any(dbcs => dbcs.Class == c.Class && dbcs.SectionName == c.Section) == false)
                .AsParallel()
                .ToList();
            if (filternewclasssection.Count > 0)
            {
                //Getclass subscriberStandard Id
                var classdictionary = await GetClassDictionary(subscriberId);
                //Getclass subscriberStandard Id
                var dbsectiondictionary = await GetSectionDictionary(subscriberId);
                //construct class , section link
                var classsections = filternewclasssection.Select(
                    cs => new SubscriberStandardSections
                    {
                        Active = true,
                        CreatedAt = DateTime.Now,
                        SubscriberSectionId = dbsectiondictionary[cs.Section],
                        SubscriberStandardsId = classdictionary[cs.Class]
                    }).ToList();
                await msectionRepository.AddRangeAsync(classsections);
            }
            Debug.WriteLine("AddBulkClassSectionLinkIfNotExists took " + sw.ElapsedMilliseconds);
            return filternewclasssection.Select(s => Tuple.Create(s.Class, s.Section)).ToList();
        }

        private async Task<Dictionary<string, long>> GetSectionDictionary(int subscriberId)
        {
            var dbsection = await msubscribersectionRepository.FindAllAsync(
                s => s.SubscriberId == subscriberId,
                s => new
                {
                    Id = s.Id,
                    Name = s.Section.Name
                });
            var dbsectiondictionary = dbsection.ToDictionary(s => s.Name, s => s.Id);
            return dbsectiondictionary;
        }

        private async Task<Dictionary<string, long>> GetClassDictionary(int subscriberId)
        {
            var dbclass = await mclassRepository.FindAllAsync(c => c.SubscriberId == subscriberId,
                c => new
                {
                    Id = c.Id,
                    Name = c.Standard.Name
                });
            var classdictionary = dbclass.ToDictionary(c => c.Name, c => c.Id);
            return classdictionary;
        }
        
        private async Task<ICollection<TSelect>> GetClassSectionListTask<TSelect>(int subscriberId, Expression<Func<SubscriberStandardSections, TSelect>> select)
        {
            return await msectionRepository.FindAllAsync(
                 c => c.SubscriberStandards.SubscriberId == subscriberId,
                select);
        }

        public  async Task<List<ContactViewModel>> ExcelBulkUpdateClassSectionTask(int subscriberId, List<ContactViewModel> excellstContactViewModels)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var classdict = await GetClassDictionary(subscriberId);
            var dbsscs = await  GetClassSectionListTask(subscriberId,
                c => new
                {
                    Id = c.Id,
                    Class = c.SubscriberStandards.Standard.Name,
                    Section = c.SubscriberSection.Section.Name,
                }); 
            excellstContactViewModels.AsParallel().ForAll(
                cvm =>
                {
                    var ssc =
                        dbsscs.SingleOrDefault(
                            r =>
                                r.Class.Trim() == cvm.Class &&
                                r.Section.Trim() == cvm.Section);
                    if (ssc != null)
                    {
                        cvm.SubscriberStandardSectionId = ssc.Id;
                    }
                    cvm.SubscriberStandardId = classdict[cvm.Class];
                });
            Debug.WriteLine("ExcelBulkUpdateClassSectionTask took " + sw.ElapsedMilliseconds);
            return excellstContactViewModels;
        }

       
    }
}
