using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Repositories
{
    public interface IQueryRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool isTracked = true);

        IQueryable<T> GetWhere(Expression<Func<T,bool>> expression, bool isTracked = true);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, bool isTracked = true);

        Task<T> GetByIdAsync(string id, bool isTracked = true);
    }
}
