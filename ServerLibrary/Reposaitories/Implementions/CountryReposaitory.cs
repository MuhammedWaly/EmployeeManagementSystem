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
    public  class CountryReposaitory : IGenericReposaitory<Country>
    {
        private readonly ApplicationDbContext _context;

        public CountryReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Country>> GetAllAsync()
        {
            return await _context.Countries.ToListAsync();

        }

        public async Task<Country> GetByIdAsync(int Id)
        {
            var dep = await _context.Countries.FindAsync(Id);
            return dep;
        }

        public async Task<GeneralResponse> Insert(Country item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Countries already added");
            _context.Countries.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Country item)
        {
            var dep = await _context.Countries.FindAsync(item.Id);
            if (dep is null) return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var dep = await _context.Countries.FindAsync(Id);
            if (dep is null) return NotFound();
            _context.Countries.Remove(dep);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "Countries not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();

        private async Task<bool> CheckName(string Name)
        {
            var item = await _context.Countries.FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
            return item is null;
        }

    }
}
