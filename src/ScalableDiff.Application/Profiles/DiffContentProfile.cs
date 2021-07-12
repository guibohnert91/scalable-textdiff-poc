using AutoMapper;
using ScalableDiff.Application.Models;
using ScalableDiff.Domain.ValueObjects;

namespace ScalableDiff.Application.Profiles
{
    public class DiffContentProfile : Profile
    {
        public DiffContentProfile()
        {
            CreateMap<DiffContent, DiffData>();
        }
    }
}
