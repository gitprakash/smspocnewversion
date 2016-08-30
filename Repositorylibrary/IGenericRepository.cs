using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Repositorylibrary
{
    public interface IGenericRepository<TObject> where TObject : class
    {
        ICollection<TObject> GetAll();
        Task<ICollection<TObject>> GetAllAsync();
        TObject Get(int id);
        Task<TObject> GetAsync(long id);
        Task<bool> AnyAsync(Expression<Func<TObject, bool>> match);
        TObject Find(Expression<Func<TObject, bool>> match);
        Task<TObject> FindAsync(Expression<Func<TObject, bool>> match);
        ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match);
        Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match);
        Task<ICollection<TResult>> FindAllAsync<TResult>(Expression<Func<TObject, bool>> match, Expression<Func<TObject, TResult>> project);
        Task<ICollection<TResult>> FindAllAsync<TResult, Tkey>(Expression<Func<TObject, bool>> match,
             Expression<Func<TObject, TResult>> select, Expression<Func<TResult, Tkey>> sort);
        Task<ICollection<TObject>> GetPagedResult(int skip, int take, string sortcolumn, bool asc, Expression<Func<TObject, bool>> match = null, List<Filter> filter = null);
        TObject Add(TObject t);
        Task<TObject> AddAsync(TObject t);
        TObject Update(TObject updated, int key);
        Task<TObject> UpdateAsync(TObject updated, long key);
        void Delete(TObject t);
        Task<int> DeleteAsync(TObject t);
        int Count();
        Task<int> CountAsync(Expression<Func<TObject, bool>> match = null);
        Task<int> CountAsync(string where);

        Task<TResult[]> ToArrayAsync<TResult>(Expression<Func<TObject, TResult>> select);
        IQueryable<TObject> GetAllLazyLoad(Expression<Func<TObject, bool>> filter, params Expression<Func<TObject, object>>[] children);

        Task<ICollection<TResult>> GetPagedResult<TResult>(int skip, int take, string ordercolumn, bool desc, Expression<Func<TObject, TResult>> project,
            Expression<Func<TObject, bool>> match = null, List<Filter> filter = null);

        Task<ICollection<TResult>> GetPagedResult<TResult>(int skip, int take, string ordercolumn, bool desc, Expression<Func<TObject, TResult>> project,
            string wherestr);

        Task<int> AddRangeAsync(List<TObject> t);
        Task<List<TObject>> AddRangeAsyncWithReturnAll(List<TObject> t);
        IEnumerable<TObject> AddRangeAsyncWithtransaction(List<TObject> t);
        Task<int> SaveAsync();


    }
}
