using AutoMapper;
using FluentValidation;
using PensionManagement.Application.Contracts;
using PensionManagement.Application.DTOs;
using PensionManagement.Domain.Entities;
using PensionManagement.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PensionManagement.Infrastructure.Repositories
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly AppDbContext _context;
        private readonly IValidator<TransactionHistoryDto> _validator;
        private readonly IMapper _mapper;

        public TransactionHistoryService(AppDbContext context, IValidator<TransactionHistoryDto> validator, IMapper mapper)
        {
            _context = context;
            _validator = validator;
            _mapper = mapper;
        }

        public void LogChange(TransactionHistoryDto transactionHistoryDto)
        {
            //Validate Dto
            var validationResult = _validator.Validate(transactionHistoryDto);
            if (!validationResult.IsValid) 
            { 
               throw new ValidationException(validationResult.Errors);
            }

            //Mapp Dto to the domain model
            var transactionHistory = new TransactionHistory
            {
                EntityType = transactionHistoryDto.EntityType,
                EntityId = transactionHistoryDto.EntityId,
                ActionType = transactionHistoryDto.ActionType,
                ChangeDate = transactionHistoryDto.ChangeDate,
            };

            _context.TransactionHistories.Add(transactionHistory);
            _context.SaveChanges();
        }

        public IEnumerable<TransactionHistoryDto> GetTransactionHistory()
        {
            var transactionHistories = _context.TransactionHistories.ToList();
            return _mapper.Map<IEnumerable<TransactionHistoryDto>>(transactionHistories);
        }
    }
}
