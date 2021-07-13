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

        Task<DiffSummary> ExecuteDiff(Guid id);
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

        public virtual async Task<DiffSummary> ExecuteDiff(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id), "Diff  id must not be empty.");

            var diff = await diffStore.ReadAsync(id);
            if (diff == null)
                return null;

            var diffResult = await diffService.ExecuteAsync(diff);
            
            var diffSummary = mapper.Map<DiffSummary>(diffResult);
            return diffSummary;
        }

        public virtual async Task<bool> SetLeftDiffContent(DiffContent content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            //todo: factory para criar do dominio ou converter
            var diff = await diffService.GetAsync(content.Id);

            var diffData = mapper.Map<DiffData>(content);
            diff.SetLeftData(diffData);

            await diffStore.WriteAsync(diff.Id, diff);

            return true;
        }

        public virtual async Task<bool> SetRightDiffContent(DiffContent content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var diff = await diffService.GetAsync(content.Id);

            var diffData = mapper.Map<DiffData>(content);
            diff.SetRightData(diffData);

            await diffStore.WriteAsync(diff.Id, diff);

            return true;
        }
    }
}
