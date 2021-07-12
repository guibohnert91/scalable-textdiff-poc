using ScalableDiff.Domain.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScalableDiff.Infrastructure.Stores
{
    public abstract class StoreBase<TData> : IStore<TData>
    {
        protected static Dictionary<Guid, TData> memory = new Dictionary<Guid, TData>();

        public virtual Task<TData> ReadAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id), "Id must not be empty");

            TData session;
            if (memory.TryGetValue(id, out session))
                return Task.FromResult(session);

            return Task.FromResult(session);
        }

        public virtual Task<bool> WriteAsync(Guid id, TData data)
        {
            if(data == null)
                throw new ArgumentNullException(nameof(data));

            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id), "Id must not be empty");

            if (memory.ContainsKey(id))
                memory.Remove(id);

            memory.Add(id, data);

            return Task.FromResult(true);
        }
    }
}
