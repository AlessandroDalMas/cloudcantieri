using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCantiere.DataAccess
{
    public interface IRepositoryBase<T, TKey> where T : new() where TKey : struct
    {
        IEnumerable<T> Get();

        T Get(int id);

        void Update(T value);

        int Insert(T value);

        void Delete(int id);
    }
}
