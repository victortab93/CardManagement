using System;
using System.Collections.Generic;

namespace CardManagementApi.Models
{
    public partial class Card
    {
        public Card()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public decimal Balance { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
