using AutoMapper;
using ScalableDiff.Application.Models;
using ScalableDiff.Domain;
using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.ValueObjects;
using ScalableDiff.Domain.Stores;
using System;
using System.Threading.Tasks;

namespace ScalableDiff.Application.Services
{
    public interface IDiffAppService
    {
        Task<bool> SetLeftDiffContent(DiffContent content);

        Task<bool> SetRightDiffContent(DiffContent content);

        Task<DiffSummary> ExecuteDiff(Guid sessionId);
    }

    public class DiffAppService : IDiffAppService
    {
        private readonly IDiffService diffService;

        private readonly IStore<Diff> diffStore;

        private readonly IMapper mapper;

        public DiffAppService(IDiffService diffService, IStore<Diff> diffStore, IMapper mapper)
        {
            this.diffService = diffService;
            this.diffStore = diffStore;
            this.mapper = mapper;
        }

        public virtual async Task<DiffSummary> ExecuteDiff(Guid sessionId)
        {
            var diffSession = await diffStore.ReadAsync(sessionId);
            if (diffSession == null)
                return DiffSummary.Create( $"Diff session with id '{sessionId}' not found.");

            var diffResult = await diffService.ExecuteAsync(diffSession);
            
            var diffSummary = mapper.Map<DiffSummary>(diffResult);
            return diffSummary;
        }

        public virtual async Task<bool> SetLeftDiffContent(DiffContent content)
        {
            //todo: factory para criar do dominio ou converter
            var diffSession = await diffService.GetSessionAsync(content.SessionId);

            var diffData = mapper.Map<DiffData>(content);
            diffSession.SetLeftData(diffData);

            await diffStore.WriteAsync(diffSession.Id, diffSession);

            return true;
        }

        public virtual async Task<bool> SetRightDiffContent(DiffContent content)
        {
            var diffSession = await diffService.GetSessionAsync(content.SessionId);

            var diffData = mapper.Map<DiffData>(content);
            diffSession.SetRightData(diffData);

            await diffStore.WriteAsync(diffSession.Id, diffSession);

            return true;
        }
    }
}
