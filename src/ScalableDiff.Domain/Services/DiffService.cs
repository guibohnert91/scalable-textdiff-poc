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
        /// Gets a session with the supplied id.
        /// </summary>
        /// <param name="sessionId">The diff id.</param>
        /// <returns>A Diff.</returns>
        Task<Diff> GetSessionAsync(Guid sessionId);

        /// <summary>
        /// Executes the supplied diff.
        /// </summary>
        /// <param name="session">The diff to execute.</param>
        /// <returns>The difing processor result.</returns>
        Task<DiffProcessorResult> ExecuteAsync(Diff session);
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
        /// <paramref name="session"/> is <c>null</c>.
        /// </exception>
        public async Task<DiffProcessorResult> ExecuteAsync(Diff session)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session), "Can't execute a null session.");

            var diffResult = await diffProcessor.Execute(session.Left, session.Right);
            return diffResult;
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">
        /// <paramref name="sessionId"/> is <c>empty</c>.
        /// </exception>
        public async Task<Diff> GetSessionAsync(Guid sessionId)
        {
            if(sessionId == Guid.Empty)
                throw new ArgumentException(nameof(sessionId), "Session id can't be empty.");

            var diffSession = await diffStore.ReadAsync(sessionId);

            return diffSession ?? Diff.Create(sessionId);
        }
    }
}
