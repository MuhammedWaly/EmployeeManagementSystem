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
    public class OvertimeTypeReposaitory : IGenericReposaitory<OvertimeType>
    {
        private readonly ApplicationDbContext _context;

        public OvertimeTypeReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<OvertimeType>> GetAllAsync()
        {
            return await _context.OvertimeTypes.AsNoTracking().ToListAsync();

        }

        public async Task<OvertimeType> GetByIdAsync(int Id)
        {
            var OvertimeType = await _context.OvertimeTypes.FindAsync(Id);
            return OvertimeType;
        }

        public async Task<GeneralResponse> Insert(OvertimeType item)
        {
            _context.OvertimeTypes.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(OvertimeType item)
        {
            var OvertimeType = await _context.OvertimeTypes.FindAsync(item.Id);
            if (OvertimeType is null) return NotFound();
            OvertimeType.Name = item.Name;
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var OvertimeType = await _context.OvertimeTypes.FindAsync(Id);
            if (OvertimeType is null) return NotFound();
            _context.OvertimeTypes.Remove(OvertimeType);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "OvertimeTypes not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();

    }
}
