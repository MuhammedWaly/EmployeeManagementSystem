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
    public class GeneralDepartmentReposaitory : IGenericReposaitory<GeneralDepartment>
    {
        private readonly ApplicationDbContext _context;

        public GeneralDepartmentReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GeneralDepartment>> GetAllAsync()
        {
            return await _context.GeneralDepartments.ToListAsync();
  
        }

        public async Task<GeneralDepartment> GetByIdAsync(int Id)
        {
            var dep = await _context.GeneralDepartments.FindAsync(Id);
            return dep;
        }

        public async Task<GeneralResponse> Insert(GeneralDepartment item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "General Department already added");
            _context.GeneralDepartments.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(GeneralDepartment item)
        {
            var dep = await _context.GeneralDepartments.FindAsync(item.Id);
            if (dep is null) return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var dep = await _context.GeneralDepartments.FindAsync(Id);
            if (dep is null) return NotFound();
            _context.GeneralDepartments.Remove(dep);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "Department not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();

        private async Task<bool> CheckName(string Name)
        {
            var item = await _context.GeneralDepartments.FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
            return item is null;
        }
    }
}
