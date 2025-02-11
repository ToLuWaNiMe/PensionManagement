using PensionManagement.Application.DTOs;
using PensionManagement.Domain.Enums;

namespace PensionManagement.Application.Contracts
{
    public interface ITransactionHistoryService
    {
        void LogChange(TransactionHistoryDto transactionHistoryDto);
        IEnumerable<TransactionHistoryDto> GetTransactionHistory();
    }
}
