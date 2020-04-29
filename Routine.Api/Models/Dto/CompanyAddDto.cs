using System.Collections;
using System.Collections.Generic;

namespace Routine.Api.Models.Dto
{
    public class CompanyAddDto
    {
        public string Name { get; set; }
        public string Introduction { get; set; }

        public ICollection<EmployeeAddDto> Employees { get; set; }
    }
}