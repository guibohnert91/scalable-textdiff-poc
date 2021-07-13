using ScalableDiff.Domain.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScalableDiff.Infrastructure.Stores
{
    /// <summary>
    /// Abstract implementation of a data store using in memory data.
    /// </summary>
    /// <typeparam name="TData">The data to store.</typeparam>
    public abstract class StoreBase<TData> : IStore<TData>
    {
        protected static Dictionary<Guid, TData> memory = new Dictionary<Guid, TData>();

        /// <summary>
        /// Reads data with the supplied id.
        /// </summary>
        /// <param name="id">The data id to read.</param>
        /// <returns>The existing data or a null value.</returns>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="id"/> is <c>empty</c>.
        /// </exception>
        public virtual Task<TData> ReadAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id), "Id must not be empty");

            TData data;
            if (memory.TryGetValue(id, out data))
                return Task.FromResult(data);

            return Task.FromResult(data);
        }

        /// <summary>
        /// Writes the data with the supplied id.
        /// </summary>
        /// <param name="id">The data id to store.</param>
        /// <param name="data">The data to store.</param>
        /// <returns>True if success.</returns>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="id"/> is <c>empty</c>.
        /// </exception>
        /// /// <exception cref="System.ArgumentNullException">
        /// <paramref name="data"/> is <c>null</c>.
        /// </exception>
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
