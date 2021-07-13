using AutoMapper;
using ScalableDiff.Application.Models;
using ScalableDiff.Domain.ValueObjects;

namespace ScalableDiff.Application.Profiles
{
    /// <summary>
    /// Defines the mapping between the DiffSummaryProfile and another models.
    /// </summary>
    public class DiffSummaryProfile : Profile
    {
        public DiffSummaryProfile()
        {
            CreateMap<DiffSummary, DiffProcessorResult>();
        }
    }
}
