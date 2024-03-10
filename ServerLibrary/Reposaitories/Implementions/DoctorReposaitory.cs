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
    public class DoctorReposaitory : IGenericReposaitory<Doctor>
    {
        private readonly ApplicationDbContext _context;

        public DoctorReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.AsNoTracking().ToListAsync();

        }

        public async Task<Doctor> GetByIdAsync(int Id)
        {
            var doc = await _context.Doctors.FindAsync(Id);
            return doc;
        }

        public async Task<GeneralResponse> Insert(Doctor item)
        {
            _context.Doctors.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Doctor item)
        {
            var doc = await _context.Doctors.FindAsync(item.Id);
            if (doc is null) return NotFound();
            doc.MidicalRecommendation = item.MidicalRecommendation;
            doc.MidicalDiagnose = item.MidicalDiagnose;
            doc.Date = item.Date;
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var doc = await _context.Doctors.FindAsync(Id);
            if (doc is null) return NotFound();
            _context.Doctors.Remove(doc);
            await Commit();
            return Success();
        }

        private static GeneralResponse NotFound() => new GeneralResponse(false, "Doctors not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process Completed");
        private async Task Commit() => await _context.SaveChangesAsync();

    }
}

