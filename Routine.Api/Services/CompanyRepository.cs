using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.DtoParameters;
using Routine.Api.Models;
using Routine.Api.Models.Entities;

namespace Routine.Api.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RoutineDbContext _context;

        public CompanyRepository(RoutineDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameter parameter)
        {
            if (parameter == null)
            {
                throw new Exception(nameof(parameter));
            }

            var items = _context.Companies as IQueryable<Company>;

            if (string.IsNullOrWhiteSpace(parameter.CompanyName) && string.IsNullOrWhiteSpace(parameter.SearchTerm))
            {
                return await items.ToListAsync();
            }

            if (!string.IsNullOrWhiteSpace(parameter.CompanyName))
            {
                parameter.CompanyName = parameter.CompanyName.Trim();

                items = items.Where(c => c.Name == parameter.CompanyName);
            }

            if (!string.IsNullOrWhiteSpace(parameter.SearchTerm))
            {
                parameter.SearchTerm = parameter.SearchTerm.Trim();

                items = items.Where(c => c.Name.Contains(parameter.SearchTerm)
                || c.Introduction.Contains(parameter.SearchTerm));
            }

            return await items.ToListAsync();
        }

        public async Task<Company> GetCompany(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }

            return await _context.Companies.Where(c => companyIds.Contains(c.Id))
                .OrderBy(c=>c.Name)
                .ToListAsync();
        }

        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            company.Id =Guid.NewGuid();

            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }

            _context.Companies.Add(company);
        }

        public void UpdateCompany(Company company)
        {
            _context.Entry(company).State = EntityState.Modified;
        }

        public void DeleteCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            _context.Remove(company);
        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _context.Companies.AnyAsync(x => x.Id == companyId);
        }

        public async Task<IEnumerable<Employee>> GetEmployees(Guid companyId, string genderDisplay,string query)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            var items = _context.Employees.Where(e => e.CompanyId == companyId);

            if (string.IsNullOrWhiteSpace(genderDisplay) && string.IsNullOrWhiteSpace(query))
            {
                return await items.OrderBy(e => e.FirstName)
                    .ToListAsync();
            }

            if (!string.IsNullOrWhiteSpace(genderDisplay))
            {
                genderDisplay = genderDisplay.Trim();
                var gender = Enum.Parse<Gender>(genderDisplay);

                items = items.Where(e=>e.Gender == gender);
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim();
                items = items.Where(e => e.EmployeeNo.Contains(query)
                                         || e.FirstName.Contains(query)
                                         || e.LastName.Contains(query));
            }
            return await items.OrderBy(e => e.EmployeeNo)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            return await _context.Employees.Where(e => e.CompanyId == companyId && e.Id == employeeId).FirstOrDefaultAsync();
        }

        public void AddEmployee(Guid companyId,Employee employee)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            employee.Id = Guid.NewGuid();
            employee.CompanyId = companyId;
            _context.Employees.Add(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
        }

        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}