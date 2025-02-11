using PensionManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensionManagement.Domain.Entities
{
    public class TransactionHistory
    {
        public int Id { get; set; }
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public ActionType ActionType { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;
        public string ChangedData { get; set; } = string.Empty;
    }

}
