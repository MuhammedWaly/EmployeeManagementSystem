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
    public class VacationReposaitory : IGenericReposaitory<Vacation>
    {
        private readonly ApplicationDbContext _context;

        public VacationReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Vacation>> GetAllAsync()
        {
            return await _context.Vacations.AsNoTracking().ToListAsync();

        }

        public async Task<Vacation> GetByIdAsync(int Id)
        {
            var Vacation = await _context.Vacations.FindAsync(Id);
            return Vacation;
        }

        public async Task<GeneralResponse> Insert(Vacation item)
        {
            
            _context.Vacations.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Vacation item)
        {
            var vacation = await _context.Vacations.FindAsync(item.Id);
            if (vacation is null) return NotFound();
            vacation.StartDate = item.StartDate;
            vacation.VacationTypeId = item.VacationTypeId;
            
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var Vacation = await _context.Vacations.FindAsync(Id);
            if (Vacation is null) return NotFound();
            _context.Vacations.Remove(Vacation);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "Vacations not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();
        
    }
}
