using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Reposaitories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Reposaitories.Implementions
{
    public class OvertimeReposaitory:IGenericReposaitory<Overtime>
    {
        private readonly ApplicationDbContext _context;

        public OvertimeReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Overtime>> GetAllAsync()
        {
            return await _context.Overtimes.AsNoTracking().ToListAsync();

        }

        public async Task<Overtime> GetByIdAsync(int Id)
        {
            var overTime = await _context.Overtimes.FindAsync(Id);
            return overTime;
        }

        public async Task<GeneralResponse> Insert(Overtime item)
        {
            _context.Overtimes.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Overtime item)
        {
            var overTime = await _context.Overtimes.FindAsync(item.Id);
            if (overTime is null) return NotFound();
            overTime.StartDate = item.StartDate;
            overTime.EndDate = item.EndDate;
            overTime.OvertimeTypeId = item.OvertimeTypeId;
            
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var overTime = await _context.Overtimes.FindAsync(Id);
            if (overTime is null) return NotFound();
            _context.Overtimes.Remove(overTime);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "Overtimes not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();



    }
}
