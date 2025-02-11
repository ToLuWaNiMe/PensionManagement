using AutoMapper;
using PensionManagement.Application.Contracts;
using PensionManagement.Application.DTOs;
using PensionManagement.Domain.Entities;
using PensionManagement.Domain.Enums;

namespace PensionManagement.Application.Services
{
    public class MemberService
    {
        private readonly IMemberRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITransactionHistoryService _transactionHistoryService;

        public MemberService(IMemberRepository repository, IMapper mapper, ITransactionHistoryService transactionHistoryService)
        {
            _repository = repository;
            _mapper = mapper;
            _transactionHistoryService = transactionHistoryService;
        }

        public async Task<IEnumerable<MemberDto>> GetAllAsync()
        {
            var members = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MemberDto>>(members);
        }

        public async Task<MemberDto> GetByIdAsync(int id)
        {
            var member = await _repository.GetByIdAsync(id);
            return _mapper.Map<MemberDto>(member);
        }

        public async Task AddAsync(MemberDto memberDto)
        {
            var member = _mapper.Map<Member>(memberDto);
            await _repository.AddAsync(member);
            var transactionHistoryDto = new TransactionHistoryDto
            {
                EntityType = EntityType.Member,
                EntityId = memberDto.Id,
                ActionType = ActionType.INSERT,
                ChangeDate = DateTime.UtcNow,
            };

            _transactionHistoryService.LogChange(transactionHistoryDto);
        }

        public async Task UpdateAsync(int id, MemberDto memberDto)
        {
            var existingMember = await _repository.GetByIdAsync(id);
            if (existingMember != null)
            {
                _mapper.Map(memberDto, existingMember);
                await _repository.UpdateAsync(existingMember);

                var transactionHistoryDto = new TransactionHistoryDto
                {
                    EntityType = EntityType.Member,
                    EntityId = memberDto.Id,
                    ActionType = ActionType.UPDATE,
                    ChangeDate = DateTime.UtcNow,
                };

                _transactionHistoryService.LogChange(transactionHistoryDto);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);

            var transactionHistoryDto = new TransactionHistoryDto
            {
                EntityType = EntityType.Member,
                EntityId = id,
                ActionType = ActionType.DELETE,
                ChangeDate = DateTime.UtcNow,
            };

            _transactionHistoryService.LogChange(transactionHistoryDto);
        }
    }
}
