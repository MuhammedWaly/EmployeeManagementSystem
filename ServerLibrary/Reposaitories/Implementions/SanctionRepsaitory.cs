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
    public class SanctionRepsaitory : IGenericReposaitory<Sanction>
    {
        private readonly ApplicationDbContext _context;

        public SanctionRepsaitory(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Sanction>> GetAllAsync()
        {
            return await _context.Sanctions.AsNoTracking().ToListAsync();

        }

        public async Task<Sanction> GetByIdAsync(int Id)
        {
            var Sanction = await _context.Sanctions.FindAsync(Id);
            return Sanction;
        }

        public async Task<GeneralResponse> Insert(Sanction item)
        {

            _context.Sanctions.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Sanction item)
        {
            var Sanction = await _context.Sanctions.FindAsync(item.Id);
            if (Sanction is null) return NotFound();
            Sanction.Punishment = item.Punishment;
            Sanction.PunishmentDate = item.PunishmentDate;
            Sanction.Date = item.Date;
            Sanction.SanctionTypeId = item.SanctionTypeId;

            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var Sanction = await _context.Sanctions.FindAsync(Id);
            if (Sanction is null) return NotFound();
            _context.Sanctions.Remove(Sanction);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "Sanctions not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();

    }
}
