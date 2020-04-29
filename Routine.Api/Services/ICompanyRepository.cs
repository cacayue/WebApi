using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Routine.Api.DtoParameters;
using Routine.Api.Models.Entities;

namespace Routine.Api.Services
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameter parameter);
        Task<Company> GetCompany(Guid companyId);
        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds);
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(Company company);
        Task<bool> CompanyExistsAsync(Guid companyId);

        Task<IEnumerable<Employee>> GetEmployees(Guid companyId, string genderDisplay,string query);
        Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId);
        void AddEmployee(Guid companyId,Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);

        Task<bool> SaveAsync(); 
    }
}