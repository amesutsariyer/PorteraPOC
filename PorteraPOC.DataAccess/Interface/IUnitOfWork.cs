using System;
using System.Collections.Generic;
using System.Text;

namespace PorteraPOC.DataAccess.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        /// <summary>
        /// Değişiklikleri kaydet
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
