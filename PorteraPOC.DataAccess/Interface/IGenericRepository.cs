using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PorteraPOC.DataAccess
{
    public interface IGenericRepository<T> where T : class 
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        T GetById(object id);
        T Get(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddRange(List<T> entityList);
        void Update(T entity);
        void Delete(T entity);
    }
}
