using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.ValueObjects;
using ScalableDiff.Domain.Stores;
using System;
using System.Threading.Tasks;

namespace ScalableDiff.Domain
{
    /// <summary>
    /// Diff domain service.
    /// </summary>
    public interface IDiffService
    {
        /// <summary>
        /// Gets a diff with the supplied id.
        /// </summary>
        /// <param name="id">The diff id.</param>
        /// <returns>A Diff.</returns>
        Task<Diff> GetAsync(Guid id);

        /// <summary>
        /// Executes the supplied diff.
        /// </summary>
        /// <param name="diff">The diff to execute.</param>
        /// <returns>The difing processor result.</returns>
        Task<DiffProcessorResult> ExecuteAsync(Diff diff);
    }

    /// <inheritdoc />
    public class DiffService : IDiffService
    {
        private readonly IStore<Diff> diffStore;
        
        private readonly IDiffProcessor diffProcessor;

        public DiffService(IStore<Diff> diffStore, IDiffProcessor diffProcessor)
        {
            this.diffStore = diffStore;
            this.diffProcessor = diffProcessor;
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="diff"/> is <c>null</c>.
        /// </exception>
        public async Task<DiffProcessorResult> ExecuteAsync(Diff diff)
        {
            if (diff == null)
                throw new ArgumentNullException(nameof(diff), "Can't execute a null diff.");

            var diffResult = await diffProcessor.Execute(diff.Left, diff.Right);
            return diffResult;
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">
        /// <paramref name="id"/> is <c>empty</c>.
        /// </exception>
        public async Task<Diff> GetAsync(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException(nameof(id), "Diff id can't be empty.");

            var diffSession = await diffStore.ReadAsync(id);

            return diffSession ?? Diff.Create(id);
        }
    }
}
