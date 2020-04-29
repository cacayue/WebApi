using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models.Dto;
using Routine.Api.Models.Entities;
using Routine.Api.Services;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeeController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeeController(IMapper mapper,ICompanyRepository companyRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(Guid companyId,
            [FromQuery(Name = "gender")]string genderDisplay,
            [FromQuery]string query)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employees = await _companyRepository.GetEmployees(companyId, genderDisplay, query);

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }

        [HttpGet("{employeeId}",Name = nameof(GetEmployee))] 
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid companyId, Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await _companyRepository.GetEmployeeAsync(companyId,employeeId);

            if (employee == null)
            {
                return NotFound();
            }
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId, [FromBody]EmployeeAddDto employeeAdd)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var entity = _mapper.Map<Employee>(employeeAdd);
            _companyRepository.AddEmployee(companyId,entity);
            await _companyRepository.SaveAsync();
            var returnDto = _mapper.Map<EmployeeDto>(entity);
            return CreatedAtAction(nameof(GetEmployee), new { companyId , employeeId = entity.Id }, returnDto);
        }
    }
}