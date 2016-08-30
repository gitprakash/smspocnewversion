//using SIRepository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DomainEntities;
//using System.Configuration;
//namespace Services
//{
//    public class BoSaleService : IBoSaleService
//    {
//        IGenericRepository<vwBOSaleReport> _boSaleRepostiory;
//        IGenericRepository<AuditEntry> _auditEntryRepostiory;
//        public BoSaleService(IGenericRepository<vwBOSaleReport> boSaleRepostiory,
//            IGenericRepository<AuditEntry> auditEntryRepostiory)
//        {
//            _boSaleRepostiory = boSaleRepostiory;
//            _auditEntryRepostiory = auditEntryRepostiory;
//        }
//        public async Task<IEnumerable<vwBOSaleReport>> GetStoreDetails(DomainEntities.JgGridParam jgGridParam)
//        {
//            List<Filter> filter = GetFilter(jgGridParam);
//            return await _boSaleRepostiory.GetAsync(jgGridParam.sidx, jgGridParam.sord == "asc", ((jgGridParam.page - 1) * jgGridParam.rows), jgGridParam.rows, filter);
//        }
//        public async Task<int> CountStores(JgGridParam jgGridParam)
//        {
//            List<Filter> filter = GetFilter(jgGridParam);
//            if (filter.Count == 0)
//            {
//                return await _boSaleRepostiory.CountAsync();
//            }
//            else
//            {
//                return await _boSaleRepostiory.CountAsync(filter);
//            }
//        }
//        public async Task<int> AddBoSale(vwBOSaleReport model)
//        {
//            string bosaleconnstr = GetBoSaleConnString(model);
//            string bosaleinsert = "INSERT INTO [ORPOS_Store] ([ConnStr] ,[POLL] ,[gdcActive]) Values(@p0,@p1,@p2)";
//            object[] parameters = { bosaleconnstr, 'Y', 1 };
//            return await _boSaleRepostiory.ExecuteSqlCommandAsync(bosaleinsert, parameters);
//        }

//        private static AuditEntry GetAuditEntry(int deptid,int EntityState, string statename,string user)
//        {
//            AuditEntry auditentry = new AuditEntry
//            {
//                EntitySetName = deptid.ToString(),
//                EntityTypeName = "tblorpos",
//                CreatedBy = user,
//                CreatedDate = DateTime.Now,
//                State = EntityState,
//                StateName = statename
               
//            };
//            return auditentry;
//        }
//        public async Task<List<Object>> GetBoSale(int storeid)
//        {
//            string bosaleinsert = "SELECT [ConnStr] ,[POLL] ,[gdcActive] FROM [ORPOS_Store] WHERE [ConnStr] LIKE @p0 + '%' ";
//            object[] parameters = { storeid.ToString() };
//            return await _boSaleRepostiory.SqlqueryAsync(typeof(BOSaleModel), bosaleinsert, parameters);
//        }

//        public async Task<Tuple<bool,string>> IsIPAddressExists(vwBOSaleReport brpv, string action)
//        {
//            string selectqry = "SELECT left(Connstr,CharIndex('=',Connstr,0)-1) as [ConnStr] ,[POLL] ,[gdcActive] FROM [ORPOS_Store] WHERE [ConnStr] LIKE '%' + @p0 + '%' AND POLL='Y' ";
//            object[] parameters = { brpv.IPAddress.ToString() };
//            var bosaledetails= await _boSaleRepostiory.SqlqueryAsync(typeof(BOSaleModel), selectqry, parameters);
//            if (bosaledetails.Count == 0)
//            {
//               return Tuple.Create(false, string.Empty);
//            }
//            else
//            {
//                BOSaleModel model = (BOSaleModel)bosaledetails[0];
//                if (action.Equals("Add"))
//                    return Tuple.Create(true, model.ConnStr);
//                else
//                { 
//                    if (model.ConnStr == brpv.deptID.ToString())
//                    {
//                        return Tuple.Create(false, model.ConnStr);
//                    }
//                    else
//                    {
//                        return Tuple.Create(true, model.ConnStr);
//                    }
//                }
//            }
//        }

//        public async Task<int> EditBoSale(vwBOSaleReport model)
//        {
//            string bosaleconnstr = GetBoSaleConnString(model);
//            string bosaleinsert = "Update [ORPOS_Store] Set ConnStr=@p0,POLL=@p1 WHERE [ConnStr] LIKE @p2 + '%'";
//            object[] parameters = { bosaleconnstr, 'Y', model.deptID.ToString() };
//            return await _boSaleRepostiory.ExecuteSqlCommandAsync(bosaleinsert, parameters);
//        }
//        public async Task<int> Deactivate(int storeid)
//        {
//            string bosaleinsert = "Update [ORPOS_Store] Set POLL=@p0,gdcActive=0 WHERE [ConnStr] LIKE @p1 + '%'";
//            object[] parameters = { 'N', storeid.ToString() };
//            return await _boSaleRepostiory.ExecuteSqlCommandAsync(bosaleinsert, parameters);
//        }

//        private static string GetBoSaleConnString(vwBOSaleReport model)
//        {
//            string bosaleconnstr = ConfigurationManager.AppSettings["bosaleconnstrformat"];
//            bosaleconnstr = string.Format(bosaleconnstr, model.deptID, model.IPAddress);
//            return bosaleconnstr;
//        }
//        private static List<Filter> GetFilter(JgGridParam jgGridParam)
//        {
//            List<Filter> filter = new List<Filter>();
//            int deptid = 0;
//            if (!string.IsNullOrEmpty(jgGridParam.searchField))
//            {
//                Filter instancefilter = new Filter { PropertyName = jgGridParam.searchField, Operation = Op.Equals };
//                if (jgGridParam.searchField == "deptID")
//                {
//                    int.TryParse(jgGridParam.searchString, out deptid);
//                    instancefilter.Value = deptid;
//                }
//                else
//                {
//                    instancefilter.Value = jgGridParam.searchString;
//                }
//                filter.Add(instancefilter);
//            };
//            return filter;
//        }



//        public async Task<int> AddActionAudit(vwBOSaleReport wmodel, string user)
//        {
//            string bosaleconnstr = GetBoSaleConnString(wmodel);
//            AuditEntry auditentry = GetAuditEntry(wmodel.deptID,(int)Z.EntityFramework.Plus.AuditEntryState.EntityAdded, "EntityAdded",user);
//            var auditentrypropertylist = GetAuditEntryProperty(bosaleconnstr, "Y", "1");
//            auditentrypropertylist.ForEach((aep) => auditentry.AuditEntryProperties.Add(aep));
//            _auditEntryRepostiory.Add(auditentry);
//            return await _auditEntryRepostiory.SaveAsync();
//        }
//        public async Task<int> EditActionAudit(vwBOSaleReport wmodel, BOSaleModel boSaleModel, string user)
//        {
//            string bosaleconnstr = GetBoSaleConnString(wmodel);
//            AuditEntry auditentry = GetAuditEntry(wmodel.deptID,(int)Z.EntityFramework.Plus.AuditEntryState.EntityModified, "EntityModified",user);
//            auditentry.CreatedBy = user;
//            var aep = new AuditEntryProperty { PropertyName = "ConnStr", NewValue = bosaleconnstr, OldValue = boSaleModel.ConnStr, RelationName = "" };
//            auditentry.AuditEntryProperties.Add(aep);
//            _auditEntryRepostiory.Add(auditentry);
//            return await _auditEntryRepostiory.SaveAsync();
//        }
//        public async Task<int> DeleteActionAudit(int storeid, string user)
//        {
//            AuditEntry auditentry = GetAuditEntry(storeid,(int)Z.EntityFramework.Plus.AuditEntryState.EntityDeleted, "EntityDeleted",user);
//            List<AuditEntryProperty> auditentrypropertylist = new List<AuditEntryProperty>
//            {
//               new AuditEntryProperty { PropertyName = "POLL", NewValue = "N", OldValue = "Y" },
//               new AuditEntryProperty { PropertyName = "gdcActive", NewValue = "0", OldValue = "1"}
//            };
//            auditentrypropertylist.ForEach((aep) => auditentry.AuditEntryProperties.Add(aep));
//            _auditEntryRepostiory.Add(auditentry);
//            return await _auditEntryRepostiory.SaveAsync();
//        }

//        private static List<AuditEntryProperty> GetAuditEntryProperty(string bosaleconnstr, string poll, string gdcActive)
//        {
//            List<AuditEntryProperty> auditentrypropertylist = new List<AuditEntryProperty>
//            {
//               new AuditEntryProperty { PropertyName = "ConnStr", NewValue = bosaleconnstr, OldValue = "", RelationName = "" },
//               new AuditEntryProperty { PropertyName = "POLL", NewValue = poll, OldValue = "", RelationName = "" },
//               new AuditEntryProperty { PropertyName = "gdcActive", NewValue = gdcActive, OldValue = "", RelationName = "" }
//            };
//            return auditentrypropertylist;
//        }
//    }
//}
