using System;
using AutoMapper;
using Routine.Api.Models.Dto;
using Routine.Api.Models.Entities;

namespace Routine.Api.Profiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Name
                    , opt =>
                        opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Gender,
                    opt =>
                        opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Age,
                    opt =>
                        opt.MapFrom(src => DateTime.Now.Year - src.DateOfBirth.Year));

            CreateMap<EmployeeAddDto, Employee>();
        }
    }
}