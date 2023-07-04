using System;
using System.Collections.Generic;

namespace CardManagementApi.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Fee { get; set; }

        public virtual Card Card { get; set; } = null!;
    }
}
