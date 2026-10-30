using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public bool IsPaid { get; set; }
    }
    public class SaveInvoiceCommandHandler : KooliProjekt.Application.Features.SaveEntityCommandHandler<SaveInvoiceCommand, Invoice>
    {
        public SaveInvoiceCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(SaveInvoiceCommand command) => command.Id;
    }
    public class DeleteInvoiceCommand : IRequest<OperationResult>, ITransactional { public int Id { get; set; } }
    public class DeleteInvoiceCommandHandler : KooliProjekt.Application.Features.DeleteEntityCommandHandler<DeleteInvoiceCommand, Invoice>
    {
        public DeleteInvoiceCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(DeleteInvoiceCommand command) => command.Id;
    }
}
