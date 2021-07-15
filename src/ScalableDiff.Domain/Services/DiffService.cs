using ScalableDiff.Domain.Factories;
using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.Stores;
using ScalableDiff.Domain.ValueObjects;
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
        /// Creates a diff with the supplied id.
        /// </summary>
        /// <param name="id">The diff id.</param>
        /// <returns>A blank diff.</returns>
        Task<Diff> CreateAsync(Guid id);

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
        private readonly IDiffFactory diffFactory;
                
        private readonly IDiffProcessor diffProcessor;

        private readonly IStore<Diff> diffStore;

        public DiffService(IDiffFactory diffFactory, IDiffProcessor diffProcessor, IStore<Diff> diffStore)
        {
            this.diffFactory = diffFactory;
            this.diffProcessor = diffProcessor;
            this.diffStore = diffStore;
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">
        /// <paramref name="id"/> is <c>empty</c>.
        /// </exception>
        /// /// <exception cref="System.InvalidOperationException">
        /// <paramref name="id"/> already <c>exists</c>.
        /// </exception>
        public async Task<Diff> CreateAsync(Guid id)
        {
            var existentDiff = await GetAsync(id);
            if (existentDiff != null)
                throw new InvalidOperationException("Diff already exists");

            var diff = diffFactory.Create(id);
            await diffStore.WriteAsync(id, diff);

            return diff;
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

            return await diffStore.ReadAsync(id);
        }
    }
}
