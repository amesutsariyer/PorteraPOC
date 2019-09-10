using Microsoft.EntityFrameworkCore;
using PorteraPOC.DataAccess.Data;
using PorteraPOC.DataAccess.Interface;
using PorteraPOC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PorteraPOC.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PorteraDbContext _context;
        private bool _disposed;
        public UnitOfWork(PorteraDbContext context)
        {
            //Database initialize kapatıyorum.
            //Database.SetInitializer<AtomContext>(null);
            _context = context ?? throw new ArgumentException("context is null");
        }
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }
        public int SaveChanges()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var affectedRow = 0;
                try
                {
                    foreach (var dbEntityEntry in _context.ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Modified).ToList())
                    {
                        try
                        {
                            dbEntityEntry.Property<DateTime>("CreatedDate").IsModified = false;
                        }
                        catch (Exception ex)
                        {
                            //Ignored
                        }
                    }
                    affectedRow = _context.SaveChanges();
                    transaction.Commit();
                    return affectedRow;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Update the values of the entity that failed to save from the store 
                    ex.Entries.Single().Reload();
                    return -1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return -1;
                }
            }
        }
        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}