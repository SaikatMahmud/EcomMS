using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Interfaces
{
    public interface IRepo<T> where T : class
    {
        IEnumerable<T> Get(string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        bool Create(T obj);
        bool Update(T obj);
        bool Delete(T obj);
        (IEnumerable<T>, int TotalCount, int FilteredCount) GetCustomizedListData(Expression<Func<T, bool>> filter, int skip, int take, string? includeProperties = null);
        (IEnumerable<T>, int TotalCount, int FilteredCount) GetCustomizedListData(int skip, int take, string? includeProperties = null);

    }
}
