//using DomainEntities;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using Z.EntityFramework.Plus;

//namespace SIRepository
//{

//    public class GenericRepository<T> : IGenericRepository<T> where T : class
//    {
//        protected EntityContext _entities;
//        protected readonly IDbSet<T> _dbset;

//        public GenericRepository(EntityContext context)
//        {
//            _entities = context;
//            _dbset = context.Set<T>();
//        }

//        public virtual IEnumerable<T> GetAll()
//        {
//            return _dbset.AsEnumerable<T>();
//        }
//        public T Find(int Id)
//        {
//            return _dbset.Find(Id);
//        }

//        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
//        {
//            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
//            return query;
//        }

//        public virtual T Add(T entity)
//        {
//            return _dbset.Add(entity);
//        }

//        public virtual T Delete(T entity)
//        {
//            return _dbset.Remove(entity);
//        }

//        public virtual void Edit(T entity)
//        {
//            _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
//        }

//        public virtual async Task<int> SaveAsync()
//        {
//            return await _entities.SaveChangesAsync();
//        }
//        public virtual async Task<int> SaveAsync(string user)
//        {
//            return await _entities.SaveChangesAsync(user);
//        }
//        public async Task<T> FirstorDefaultAsync(Expression<Func<T, bool>> predicate)
//        {
//            return await _dbset.FirstOrDefaultAsync(predicate);
//        }
//        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
//        {
//            return await _dbset.AsNoTracking().CountAsync(predicate);
//        }
//        public async Task<int> CountAsync(List<Filter> filter = null)
//        {
//            Expression<Func<T, bool>> deleg = ExpressionBuilder.GetExpression<T>(filter);
//            return await _dbset.AsNoTracking().CountAsync(deleg);
//        }
//        public async Task<int> CountAsync()
//        {
//            return await _dbset.AsNoTracking().CountAsync();
//        }
//        public async virtual Task<IEnumerable<T>> GetAsync(string orderBy, bool asc, int skip, int take, List<Filter> filter = null)
//        {
//            IQueryable<T> query = _dbset.AsNoTracking();
//            Expression<Func<T, bool>> deleg = ExpressionBuilder.GetExpression<T>(filter);
//            if (filter.Count > 0)
//            {
//                query = query.Where(deleg);
//            }
//            if (!string.IsNullOrEmpty(orderBy))
//            {
//                return await query.OrderByAscDsc(orderBy, asc).Skip(skip).Take(take).ToListAsync();
//            }
//            return await query.ToListAsync();
//        }
//        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
//        {
//            return await _dbset.AsNoTracking().AnyAsync(predicate);
//        }
//        public async Task<int> ExecuteSqlCommandAsync(string sql,object[] parameters)
//        {
//            return await _entities.Database.ExecuteSqlCommandAsync(sql, parameters);
//        }
//        public async Task<List<object>> SqlqueryAsync(Type type,string sql, object[] parameters)
//        {
//            return  await _entities.Database.SqlQuery(type, sql, parameters).ToListAsync();
//        }
//    }
//}
