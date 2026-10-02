using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.Application.Data
{
    public class InvoiceLine
    {
        public int Id { get; set; }

        public string LineItem { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }

        public int BookingId { get; set; }

        public Booking Booking { get; set; } = null!;
    }
}
