using Microsoft.EntityFrameworkCore;
using PensionManagement.Application.Contracts;
using PensionManagement.Domain.Entities;
using PensionManagement.Infrastructure.Persistence;

namespace PensionManagement.Infrastructure.Repositories
{
    public class BenefitRepository : IBenefitRepository
    {
        private readonly AppDbContext _context;

        public BenefitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Benefit> GetByIdAsync(int id)
        {
            return await _context.Benefits
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Benefit>> GetAllByMemberIdAsync(int memberId)
        {
            return await _context.Benefits
                .Where(b => b.MemberId == memberId)
                .ToListAsync();
        }

        public async Task AddAsync(Benefit benefit)
        {
            await _context.Benefits.AddAsync(benefit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Benefit benefit)
        {
            _context.Benefits.Update(benefit);
            await _context.SaveChangesAsync();
        }
    }
}
