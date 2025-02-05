using Microsoft.EntityFrameworkCore;
using PensionManagement.Application.Contracts;
using PensionManagement.Domain.Entities;
using PensionManagement.Infrastructure.Persistence;

namespace PensionManagement.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Member> GetByIdAsync(Guid id)
        {
            return await _context.Members.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members.Where(m => !m.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Member member)
        {
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var member = await GetByIdAsync(id);
            if (member != null)
            {
                member.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
