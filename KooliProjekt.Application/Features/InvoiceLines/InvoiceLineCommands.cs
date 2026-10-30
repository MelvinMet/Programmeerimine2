using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class SaveInvoiceLineCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string LineItem { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
        public int BookingId { get; set; }
    }
    public class SaveInvoiceLineCommandHandler : KooliProjekt.Application.Features.SaveEntityCommandHandler<SaveInvoiceLineCommand, InvoiceLine>
    {
        public SaveInvoiceLineCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(SaveInvoiceLineCommand command) => command.Id;
    }
    public class DeleteInvoiceLineCommand : IRequest<OperationResult>, ITransactional { public int Id { get; set; } }
    public class DeleteInvoiceLineCommandHandler : KooliProjekt.Application.Features.DeleteEntityCommandHandler<DeleteInvoiceLineCommand, InvoiceLine>
    {
        public DeleteInvoiceLineCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(DeleteInvoiceLineCommand command) => command.Id;
    }
}
