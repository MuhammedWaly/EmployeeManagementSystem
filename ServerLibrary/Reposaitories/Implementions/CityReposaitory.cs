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
    public class CityReposaitory : IGenericReposaitory<City>
    {
        private readonly ApplicationDbContext _context;

        public CityReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<City>> GetAllAsync()
        {
            return await _context.Cities.ToListAsync();

        }

        public async Task<City> GetByIdAsync(int Id)
        {
            var dep = await _context.Cities.FindAsync(Id);
            return dep;
        }

        public async Task<GeneralResponse> Insert(City item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Cities already added");
            _context.Cities.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(City item)
        {
            var dep = await _context.Cities.FindAsync(item.Id);
            if (dep is null) return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var dep = await _context.Cities.FindAsync(Id);
            if (dep is null) return NotFound();
            _context.Cities.Remove(dep);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "Cities not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();

        private async Task<bool> CheckName(string Name)
        {
            var item = await _context.Cities.FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
            return item is null;
        }
    }
}
