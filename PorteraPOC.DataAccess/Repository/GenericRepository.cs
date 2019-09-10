using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PorteraPOC.DataAccess.Data;
using System.Linq.Expressions;
using PorteraPOC.Entity;

namespace PorteraPOC.DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class 
    {
        private readonly PorteraDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(PorteraDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking();
        }
        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking().FirstOrDefault();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);

            if (entity.GetType().GetProperty("IsDeleted") != null)
            {
                T _entity = entity;
                _entity.GetType().GetProperty("IsDeleted")?.SetValue(_entity, false);
            }
        }
        public void AddRange(List<T> entityList)
        {
            _dbSet.AddRange(entityList);
            foreach (var item in entityList)
            {
                var baseEntities = item as BaseEntity;
                if (baseEntities != null)
                {
                    baseEntities.CreatedDate = DateTime.Now;
                    baseEntities.GetType().GetProperty("IsDeleted")?.SetValue(baseEntities, false);
                }
            }
        }
        public void Update(T entity)
        {
            var baseEntities = entity as BaseEntity;
            var state = _context.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
         
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(T entity)
        {
            T entityDelete = entity;
            var baseEntities = entity as BaseEntity;
            if (baseEntities != null)
            {
                baseEntities.ModifiedDate = DateTime.Now;
            }
            //Entity olarak referans verilmiş property'lerin içini boşaltır
            entity.GetType().GetProperties()
                .Where(z =>
                    (z.PropertyType.FullName ?? "").Contains("Entities")
                ).ToList()
                .ForEach(x => x.SetValue(entity, null));
            // IsDelete alanı olan tablolarda kayıt silinmez ve IsDelete alanı update edilir.
            if (entity.GetType().GetProperty("IsDeleted") != null)
            {
                entityDelete.GetType().GetProperty("IsDeleted")?.SetValue(entityDelete, true);
                Update(entityDelete);
            }
            else
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                    _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            }
        }
    }
}
