using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.DtoParameters;
using Routine.Api.Models.Dto;
using Routine.Api.Models.Entities;
using Routine.Api.Services;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    //[Route("api/[Controller]")]
    public class CompaniesController:ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository,IMapper mapper)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameter parameter)
        {

            var companies = await _companyRepository.GetCompaniesAsync(parameter);

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(companyDtos);
        }

        [HttpGet("{companyId}",Name = nameof(GetCompany))]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompany(companyId);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CompanyDto>(company));
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody]CompanyAddDto companyAdd)
        {
            var company = _mapper.Map<Company>(companyAdd);
            _companyRepository.AddCompany(company);
            await _companyRepository.SaveAsync();
            var returnDto = _mapper.Map<CompanyDto>(company);
            return CreatedAtAction(nameof(GetCompany), new {companyId = company.Id}, returnDto);
        }
    }
}