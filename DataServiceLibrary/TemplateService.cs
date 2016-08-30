using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataModelLibrary;
using Repositorylibrary;

namespace DataServiceLibrary
{
    public class TemplateService : ITemplateService
    {
        private IGenericRepository<SubscriberTemplate> mtemplateRepository;
        public TemplateService(IGenericRepository<SubscriberTemplate> templateRepository)
        {
            mtemplateRepository = templateRepository;
        }

        public  async Task<ICollection<TemplateViewModel>> GetTemplates(int subcriberId)
        {
            Expression<Func<SubscriberTemplate, bool>> where = st => st.SubscriberId == subcriberId;
            Expression<Func<SubscriberTemplate, TemplateViewModel>> select = st =>
                new TemplateViewModel
                {
                    Id = st.Id,
                    Name = st.Templates.Name,
                    Description = st.Templates.Description,
                    CreatedAt=st.CreatedAt,
                };
            Expression<Func<TemplateViewModel, string>> orderby = st => st.Name;

            return await mtemplateRepository.FindAllAsync(match:where, select:select,sort:orderby);
        }

        public async Task<ICollection<TemplateViewModel>> GetPagedTemplates(int subcriberId, JgGridParam jgGridParam)
        {
            int pageIndex = Convert.ToInt32(jgGridParam.page) - 1;
            int pageSize = jgGridParam.rows;
            string sort = jgGridParam.sord ?? "asc";
            string ordercolumn = jgGridParam.sidx;
            bool desc = sort.ToUpper() == "ASC";
            Expression<Func<SubscriberTemplate, bool>> where = st => st.SubscriberId == subcriberId;
            Expression<Func<SubscriberTemplate, TemplateViewModel>> select = st =>
                new TemplateViewModel
                {
                    Id = st.Id, Name = st.Templates.Name, Description = st.Templates.Description,
                    Status = st.Active?"Active":"InActive"
                   
                };
            return await mtemplateRepository.GetPagedResult(pageSize * pageIndex, pageSize, ordercolumn, desc, select, where);
        }

        public async Task<int> TotalTemplates(int subcriberId)
        {
            Expression<Func<SubscriberTemplate, bool>> where = st => st.SubscriberId == subcriberId;
            return await mtemplateRepository.CountAsync(where);
        }

        
        public async Task<SubscriberTemplate> AddTemplate(TemplateViewModel templateViewModel, int subscirberId)
        {
            var stemplate = new SubscriberTemplate
            {
                SubscriberId = subscirberId,
                CreatedAt = DateTime.Now,
                Guid = Guid.NewGuid(),
                Templates =
                    new Template
                    {
                       
                        Name = templateViewModel.Name,
                        Description = templateViewModel.Description
                    },
                Active = templateViewModel.Status == "InActive" ? false : true
            };
            return await mtemplateRepository.AddAsync(stemplate);
        }
        
        public  async Task<int> SaveAsync()
        {
            return await mtemplateRepository.SaveAsync();
        }

        Expression<Func<T, bool>> CombineWithOr<T>(Expression<Func<T, bool>> firstExpression, Expression<Func<T, bool>> secondExpression)
        {
            // Create a parameter to use for both of the expression bodies.
            var parameter = Expression.Parameter(typeof(T), "x");
            // Invoke each expression with the new parameter, and combine the expression bodies with OR.
            var resultBody = Expression.Or(Expression.Invoke(firstExpression, parameter), Expression.Invoke(secondExpression, parameter));
            // Combine the parameter with the resulting expression body to create a new lambda expression.
            return Expression.Lambda<Func<T, bool>>(resultBody, parameter);
        }

        public async Task<SubscriberTemplate> FindTemplate(int subcriberId, long templateId = 0,
            string templatename = null)
        {
           /* Expression<Func<SubscriberTemplate, bool>> resultExpression = n => false;
            Expression<Func<SubscriberTemplate, bool>> where = st => st.SubscriberId == subcriberId;
            resultExpression = CombineWithOr(resultExpression, where);
            if (!string.IsNullOrEmpty(templatename))
            {
                Expression<Func<SubscriberTemplate, bool>> wherename = st => st.Templates.Name == templatename;
                resultExpression = CombineWithOr(resultExpression, wherename);
            }
            if (templateId > 0)
            {
                Expression<Func<SubscriberTemplate, bool>> whereid = st => st.TemplateId == templateId;
                resultExpression = CombineWithOr(resultExpression, whereid);
            }
           var finalwhere= resultExpression.Compile();
            //var query = query.Where(resultExpression.Compile());
           return await mtemplateRepository.FindAsync(resultExpression);*/
            SubscriberTemplate subscriberTemplate=null;
            if (!string.IsNullOrEmpty(templatename))
            {
                Expression<Func<SubscriberTemplate, bool>> wherename = st => st.SubscriberId == subcriberId && st.Templates.Name == templatename;
                  subscriberTemplate= await mtemplateRepository.FindAsync(wherename);

            }
            if (templateId > 0)
            {
                Expression<Func<SubscriberTemplate, bool>> whereid = st => st.SubscriberId == subcriberId && st.TemplateId == templateId;
                subscriberTemplate = await mtemplateRepository.FindAsync(whereid);
            }
            return subscriberTemplate;

        }
    }
}
