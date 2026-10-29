using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string UserName { get; set; }
        public bool IsPaid { get; set; }
        public decimal Total { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public IList<InvoiceLineListItemDto> Lines { get; set; } = new List<InvoiceLineListItemDto>();
    }
}
