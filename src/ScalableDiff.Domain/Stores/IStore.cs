using System;
using System.Threading.Tasks;

namespace ScalableDiff.Domain.Stores
{
    /// <summary>
    /// Diff domain store.
    /// Provides access to reading and writing data with a key-value pair approach.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IStore<TData>
    {
        /// <summary>
        /// Writes the data with the supplied id.
        /// </summary>
        /// <param name="id">The data id to store.</param>
        /// <param name="data">The data to store.</param>
        /// <returns>True if success.</returns>
        Task<bool> WriteAsync(Guid id, TData data);

        /// <summary>
        /// Reads data with the supplied id.
        /// </summary>
        /// <param name="id">The data id to read.</param>
        /// <returns>The existing data or a null value.</returns>
        Task<TData> ReadAsync(Guid id);
    }
}
