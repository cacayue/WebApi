using System;
using Microsoft.EntityFrameworkCore;
using Routine.Api.Models;
using Routine.Api.Models.Entities;

namespace Routine.Api.Data
{
    public class RoutineDbContext:DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options):base(options)
        {
            
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).IsRequired().HasMaxLength(500);

            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Company)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>()
                .HasData(new Company()
                {
                    Id = Guid.Parse("f6a41019-4697-4048-ba96-ae94f58efec9"),
                    Name = "Microsoft",
                    Introduction = "Great Company"
                }, new Company()
                {
                    Id = Guid.Parse("83124751-6f30-4a49-b712-a255126bed87"),
                    Name = "Google",
                    Introduction = "Bad Company"
                }, new Company()
                {
                    Id = Guid.Parse("1afca17b-af76-4f79-a011-cea83f852e6a"),
                    Name = "Alibaba",
                    Introduction = "Fubao 4 Company"
                });

            modelBuilder.Entity<Employee>()
                .HasData(new Employee()
                {
                    Id = Guid.Parse("f6a41019-4697-4048-ba96-cea83f852e6a"),
                    CompanyId = Guid.Parse("f6a41019-4697-4048-ba96-ae94f58efec9"),
                    FirstName = "Li",
                    LastName = "si",
                    Gender = Gender.女,
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    EmployeeNo = "MSF324"
                }, new Employee()
                {
                    Id = Guid.Parse("1afca17b-4697-4048-af76-cea83f852e6a"),
                    CompanyId = Guid.Parse("f6a41019-4697-4048-ba96-ae94f58efec9"),
                    FirstName = "Wang",
                    LastName = "wu",
                    Gender = Gender.男,
                    DateOfBirth = DateTime.Now.AddYears(-20),
                    EmployeeNo = "MSF314"
                });
        }
    }
}