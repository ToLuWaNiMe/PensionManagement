using AutoMapper;
using PensionManagement.Application.Contracts;
using PensionManagement.Application.DTOs;
using PensionManagement.Domain.Entities;

namespace PensionManagement.Application.Services
{
    public class MemberService
    {
        private readonly IMemberRepository _repository;
        private readonly IMapper _mapper;

        public MemberService(IMemberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberDto>> GetAllAsync()
        {
            var members = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MemberDto>>(members);
        }

        public async Task<MemberDto> GetByIdAsync(Guid id)
        {
            var member = await _repository.GetByIdAsync(id);
            return _mapper.Map<MemberDto>(member);
        }

        public async Task AddAsync(MemberDto memberDto)
        {
            var member = _mapper.Map<Member>(memberDto);
            await _repository.AddAsync(member);
        }

        public async Task UpdateAsync(Guid id, MemberDto memberDto)
        {
            var existingMember = await _repository.GetByIdAsync(id);
            if (existingMember != null)
            {
                _mapper.Map(memberDto, existingMember);
                await _repository.UpdateAsync(existingMember);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
