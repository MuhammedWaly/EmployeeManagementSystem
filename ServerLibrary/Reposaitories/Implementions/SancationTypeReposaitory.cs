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
    public class SancationTypeReposaitory : IGenericReposaitory<SanctionType>
    {
        private readonly ApplicationDbContext _context;

        public SancationTypeReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<SanctionType>> GetAllAsync()
        {
            return await _context.SanctionTypes.AsNoTracking().ToListAsync();

        }

        public async Task<SanctionType> GetByIdAsync(int Id)
        {
            var SanctionType = await _context.SanctionTypes.FindAsync(Id);
            return SanctionType;
        }

        public async Task<GeneralResponse> Insert(SanctionType item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "Snaction Type already added");

            _context.SanctionTypes.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(SanctionType item)
        {
            var SanctionType = await _context.SanctionTypes.FindAsync(item.Id);
            if (SanctionType is null) return NotFound();
            SanctionType.Name = item.Name;
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var SanctionType = await _context.SanctionTypes.FindAsync(Id);
            if (SanctionType is null) return NotFound();
            _context.SanctionTypes.Remove(SanctionType);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "SanctionTypes not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();
        private async Task<bool> CheckName(string Name)
        {
            var item = await _context.Cities.FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
            return item is null;
        }
    }
}
