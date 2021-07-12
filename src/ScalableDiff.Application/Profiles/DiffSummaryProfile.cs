using AutoMapper;
using ScalableDiff.Application.Models;
using ScalableDiff.Domain.ValueObjects;

namespace ScalableDiff.Application.Profiles
{
    public class DiffSummaryProfile : Profile
    {
        public DiffSummaryProfile()
        {
            CreateMap<DiffSummary, DiffProcessorResult>().ReverseMap();
        }
    }
}
