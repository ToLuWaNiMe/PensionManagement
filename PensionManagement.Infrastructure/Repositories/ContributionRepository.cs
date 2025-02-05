using Microsoft.EntityFrameworkCore;
using PensionManagement.Application.Contracts;
using PensionManagement.Domain.Entities;
using PensionManagement.Infrastructure.Persistence;

namespace PensionManagement.Infrastructure.Repositories
{
    public class ContributionRepository : IContributionRepository
    {
        private readonly AppDbContext _context;

        public ContributionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Contribution> GetByIdAsync(Guid id)
        {
            return await _context.Contributions
                .Include(c => c.Member)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Contribution>> GetAllByMemberIdAsync(Guid memberId)
        {
            return await _context.Contributions
                .Where(c => c.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contribution>> GetAllPendingContributionsAsync()
        {
            return await _context.Contributions
                .Where(c => !c.IsValid.HasValue) // Fetch only unvalidated contributions
                .ToListAsync();
        }


        public async Task AddAsync(Contribution contribution)
        {
            await _context.Contributions.AddAsync(contribution);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contribution contribution)
        {
            _context.Contributions.Update(contribution);
            await _context.SaveChangesAsync();
        }
    }
}
