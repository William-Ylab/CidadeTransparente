using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Interfaces
{
    /// <summary>
    /// Classe base do repositório
    /// </summary>
    public interface IRepository<T>
    {
        void save(T entity);
        void delete(T entity);
        void delete(long id);

        T getInstanceById(long id);
        List<T> getAll();
        List<T> selectWhere(Func<T, bool> where, int maximumRows, int startRowIndex);
        List<T> selectWhere(Func<T, bool> where);
    }
}
