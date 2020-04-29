using AutoMapper;
using Routine.Api.Models.Dto;
using Routine.Api.Models.Entities;

namespace Routine.Api.Profiles
{
    public class CompanyProfile:Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.CompanyName,
                    options =>
                        options.MapFrom(src => src.Name));
            CreateMap<CompanyAddDto, Company>();
        }
    }
}