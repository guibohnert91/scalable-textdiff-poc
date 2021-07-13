using AutoMapper;
using ScalableDiff.Application.Models;
using ScalableDiff.Domain.ValueObjects;

namespace ScalableDiff.Application.Profiles
{
    /// <summary>
    /// Defines the mapping between the DiffContent and another models.
    /// </summary>
    public class DiffContentProfile : Profile
    {        
        public DiffContentProfile()
        {
            CreateMap<DiffContent, DiffData>();
        }
    }
}
