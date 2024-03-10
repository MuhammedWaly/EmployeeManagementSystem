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
    public class VacationTypeReposaitory : IGenericReposaitory<VacationType>
    {
        private readonly ApplicationDbContext _context;

        public VacationTypeReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<VacationType>> GetAllAsync()
        {
            return await _context.VacationTypes.AsNoTracking().ToListAsync();

        }

        public async Task<VacationType> GetByIdAsync(int Id)
        {
            var VacationType = await _context.VacationTypes.FindAsync(Id);
            return VacationType;
        }

        public async Task<GeneralResponse> Insert(VacationType item)
        {
            if (!await CheckName(item.Name)) return new GeneralResponse(false, "vacation Type already added");

            _context.VacationTypes.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(VacationType item)
        {
            var VacationType = await _context.VacationTypes.FindAsync(item.Id);
            if (VacationType is null) return NotFound();
            VacationType.Name = item.Name;
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var VacationType = await _context.VacationTypes.FindAsync(Id);
            if (VacationType is null) return NotFound();
            _context.VacationTypes.Remove(VacationType);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "VacationTypes not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();
        private async Task<bool> CheckName(string Name)
        {
            var item = await _context.Cities.FirstOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
            return item is null;
        }
    }
}
