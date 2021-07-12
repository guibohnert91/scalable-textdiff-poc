using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.ValueObjects;
using ScalableDiff.Domain.Stores;
using System;
using System.Threading.Tasks;

namespace ScalableDiff.Domain
{
    public interface IDiffService
    {
        Task<Diff> GetSessionAsync(Guid sessionId);

        Task<DiffProcessorResult> ExecuteAsync(Diff session);
    }

    public class DiffService : IDiffService
    {
        private readonly IStore<Diff> diffStore;
        
        private readonly IDiffProcessor diffProcessor;

        public DiffService(IStore<Diff> diffStore, IDiffProcessor diffProcessor)
        {
            this.diffStore = diffStore;
            this.diffProcessor = diffProcessor;
        }

        public async Task<DiffProcessorResult> ExecuteAsync(Diff session)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session), "Can't execute a null session.");

            var diffResult = await diffProcessor.Execute(session.Left, session.Right);
            return diffResult;
        }

        public async Task<Diff> GetSessionAsync(Guid sessionId)
        {
            if(sessionId == Guid.Empty)
                throw new ArgumentException(nameof(sessionId), "Session id can't be empty.");

            var diffSession = await diffStore.ReadAsync(sessionId);

            return diffSession ?? Diff.Create(sessionId);
        }
    }
}
