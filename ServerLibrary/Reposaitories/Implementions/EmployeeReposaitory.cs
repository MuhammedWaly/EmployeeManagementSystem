using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Reposaitories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Reposaitories.Implementions
{
    public class EmployeeReposaitory(ApplicationDbContext _context) : IGenericReposaitory<Employee>
    {
        public async Task<GeneralResponse> Delete(int Id)
        {
            if (Id <= 0) return new GeneralResponse(false, "Invalid Id");
            var employee = await _context.Employees.FirstOrDefaultAsync(e=>e.Id ==  Id);
            if (employee is null) return NotFound();
            _context.Employees.Remove(employee);
            await Commit();
            return Success();
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.AsNoTracking()
                .Include(d => d.Town)
                .ThenInclude(d=>d.City)
                .ThenInclude(d=>d.Country)
                .Include(d=>d.Branch)
                .ThenInclude(d=>d.Department)
                .ThenInclude(d=>d.GeneralDepartment)
                .ToListAsync();

        }

        public async Task<Employee> GetByIdAsync(int Id)
        {
            var dep = await _context.Employees.FindAsync(Id);
            return dep;
        }

        public async Task<GeneralResponse> Insert(Employee item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Employeees already added");
            _context.Employees.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Employee item)
        {
            var Employee = await _context.Employees.FindAsync(item.Id);
            if (Employee is null) return NotFound();
            Employee.Name = item.Name;
            Employee.Address = item.Address;
            Employee.TelephoneNumber = item.TelephoneNumber;
            Employee.Other = item.Other;
            Employee.BranchId = item.BranchId;
            Employee.TownId = item.TownId;
            Employee.Photo = item.Photo;
            Employee.CivilId = item.CivilId;
            Employee.FileNumber = item.FileNumber;
            Employee.JobNAme = item.JobNAme;
            //Employee.DepartmentId = item.DepartmentId;
            await Commit();
            return Success();
        }



        private static GeneralResponse NotFound() => new GeneralResponse(false, "Employee not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();
        private async Task<bool> CheckName(string Name)
        {
            var item = await _context.Employees.FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
            return item is null;
        }
    }
}
