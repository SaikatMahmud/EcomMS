using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcomMS.DAL.Interfaces
{
    public interface IUser<T>
    {
        T GetCustomer(string username, string password, string? includeProperties = null);
        T GetEmployee(string username, string password, string? includeProperties = null);
        bool IsUsernameUnique(string username);
        T Create(T obj);
        T Update(T obj);
    }
}
