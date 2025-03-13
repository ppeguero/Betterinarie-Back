using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Betterinarie_Back.Core.Interfaces.Implementation
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}
