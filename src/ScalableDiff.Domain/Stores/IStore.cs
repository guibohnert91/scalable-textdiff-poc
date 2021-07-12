using System;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Stores
{
    public interface IStore<TData>
    {
        Task<bool> WriteAsync(Guid id, TData data);

        Task<TData> ReadAsync(Guid id);
    }
}
