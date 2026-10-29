
namespace KooliProjekt.Application.Dto
{
    public class InvoiceLineListItemDto
    {
        public int Id { get; set; }
        public string LineItem { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
        public int BookingId { get; set; }
    }
}
