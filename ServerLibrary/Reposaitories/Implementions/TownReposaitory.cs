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
    public class TownReposaitory : IGenericReposaitory<Town>
    {
        private readonly ApplicationDbContext _context;

        public TownReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Town>> GetAllAsync()
        {
            return await _context.Towns.ToListAsync();

        }

        public async Task<Town> GetByIdAsync(int Id)
        {
            var dep = await _context.Towns.FindAsync(Id);
            return dep;
        }

        public async Task<GeneralResponse> Insert(Town item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Towns already added");
            _context.Towns.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Town item)
        {
            var dep = await _context.Towns.FindAsync(item.Id);
            if (dep is null) return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var dep = await _context.Towns.FindAsync(Id);
            if (dep is null) return NotFound();
            _context.Towns.Remove(dep);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "Towns not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();

        private async Task<bool> CheckName(string Name)
        {
            var item = await _context.Towns.FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
            return item is null;
        }
    }
}
