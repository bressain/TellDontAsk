using System.Collections.Generic;

namespace TellDontAsk
{
    public interface IDatabase
    {
        IEnumerable<T> GetRows<T>(int id);
        void Update<T>(T value);
    }
}