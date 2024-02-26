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
    public class BranchReposaitory : IGenericReposaitory<Branch>
    {
        private readonly ApplicationDbContext _context;

        public BranchReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Branch>> GetAllAsync()
        {
            return await _context.Branches.AsNoTracking().Include(d=>d.Department).ToListAsync();

        }

        public async Task<Branch> GetByIdAsync(int Id)
        {
            var dep = await _context.Branches.FindAsync(Id);
            return dep;
        }

        public async Task<GeneralResponse> Insert(Branch item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Branches already added");
            _context.Branches.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Branch item)
        {
            var branch = await _context.Branches.FindAsync(item.Id);
            if (branch is null) return NotFound();
            branch.Name = item.Name;
            branch.DepartmentId = item.DepartmentId;
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var dep = await _context.Branches.FindAsync(Id);
            if (dep is null) return NotFound();
            _context.Branches.Remove(dep);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "Branches not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();

        private async Task<bool> CheckName(string Name)
        {
            var item = await _context.Branches.FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
            return item is null;
        }
    }
}
