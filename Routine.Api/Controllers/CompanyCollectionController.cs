using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Routine.Api.Helpers;
using Routine.Api.Models.Dto;
using Routine.Api.Models.Entities;
using Routine.Api.Services;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companycollection")]
    public class CompanyCollectionController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public CompanyCollectionController(IMapper mapper,ICompanyRepository companyRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
        }

        [HttpGet("{ids}", Name = nameof(GetCompanyCollection))]
        public async Task<IActionResult> GetCompanyCollection([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var entities = await _companyRepository.GetCompaniesAsync(ids);

            if (ids.Count() != entities.Count())
            {
                return NotFound();
            }

            var dtoToReturn = _mapper.Map<IEnumerable<CompanyDto>>(entities);

            return Ok(dtoToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompanyCollection(IEnumerable<CompanyAddDto> companyAddDtos)
        {
            if (!EnumerableExtensions.Any(companyAddDtos))
            {
                throw new Exception("参数错误");
            }

            var companies = _mapper.Map<IEnumerable<Company>>(companyAddDtos);

            foreach (var company in companies)
            {
                _companyRepository.AddCompany(company);
            }

            await _companyRepository.SaveAsync();

            var dtosToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companies);


            var ids = string.Join(",", dtosToReturn.Select(x => x.Id));
            return CreatedAtRoute(nameof(GetCompanyCollection),new {ids = ids },dtosToReturn);
        }
    }
}