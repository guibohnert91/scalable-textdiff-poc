using AutoMapper;
using ScalableDiff.Application.Models;
using ScalableDiff.Domain;
using ScalableDiff.Domain.Models;
using ScalableDiff.Domain.Stores;
using ScalableDiff.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace ScalableDiff.Application.Services
{
    /// <summary>
    /// Provides basic diffing functionality.
    /// </summary>
    public interface IDiffAppService
    {
        /// <summary>
        /// Creates an empty diff.
        /// </summary>
        /// <param name="id">The diff id.</param>
        /// <returns>True if operation sucessfully.</returns>
        Task<bool> CreateDiff(Guid id);

        /// <summary>
        /// Defines the left content to compare.
        /// </summary>
        /// <param name="content">The left content.</param>
        /// <returns>True if operation sucessfully.</returns>
        Task<bool> SetLeftDiffContent(DiffContent content);

        /// <summary>
        /// Defines the right content to compare.
        /// </summary>
        /// <param name="content">The right content.</param>
        /// <returns>True if operation sucessfully.</returns>
        Task<bool> SetRightDiffContent(DiffContent content);

        /// <summary>
        /// Executes the basic diffing funcionality.
        /// </summary>
        /// <param name="id">The diff id to execute.</param>
        /// <returns>The basic diff summary.</returns>
        Task<DiffSummary> ExecuteDiff(Guid id);
    }

    /// <inheritdoc />    
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

        /// <inheritdoc />    
        /// <exception cref="System.ArgumentException">
        /// <paramref name="id"/> is <c>empty</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <paramref name="id"/> already <c>exists</c>.
        /// </exception>
        public async Task<bool> CreateDiff(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id), "Diff id must not be empty.");

            // Currently not using the return diff value.
            await diffService.CreateAsync(id);

            return true;
        }

        /// <inheritdoc />    
        /// <exception cref="System.ArgumentException">
        /// <paramref name="id"/> is <c>empty</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <paramref name="id"/> don't <c>exists</c>.
        /// </exception>
        public virtual async Task<DiffSummary> ExecuteDiff(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id), "Diff id must not be empty.");

            var diff = await GetDiffAsync(id);

            var diffResult = await diffService.ExecuteAsync(diff);            
            var diffSummary = mapper.Map<DiffSummary>(diffResult);
            return diffSummary;
        }

        /// <inheritdoc />    
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="content"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <paramref name="id"/> don't <c>exists</c>.
        /// </exception>
        public virtual async Task<bool> SetLeftDiffContent(DiffContent content)
        {
            ValidateContent(content);

            var diff = await GetDiffAsync(content.Id);

            var diffData = mapper.Map<DiffData>(content);
            diff.SetLeftData(diffData);

            await diffStore.WriteAsync(diff.Id, diff);

            return true;
        }

        /// <inheritdoc />    
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="content"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <paramref name="id"/> don't <c>exists</c>.
        /// </exception>
        public virtual async Task<bool> SetRightDiffContent(DiffContent content)
        {
            ValidateContent(content);
            
            var diff = await GetDiffAsync(content.Id);
            var diffData = mapper.Map<DiffData>(content);
            diff.SetRightData(diffData);

            await diffStore.WriteAsync(diff.Id, diff);

            return true;
        }

        // Logic can be moved to a validator, keeping it simple here.
        private void ValidateContent(DiffContent content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            if(content.Id == Guid.Empty)
                throw new ArgumentException(nameof(content.Id), "Diff id must not be empty.");
        }

        private async Task<Diff> GetDiffAsync(Guid id)
        {
            var diff = await diffService.GetAsync(id);
            if (diff == null)
                throw new InvalidOperationException($"Invalid diff id.");

            return diff;
        }
    }
}
