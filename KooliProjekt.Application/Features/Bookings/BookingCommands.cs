using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Bookings
{
    public class SaveBookingCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public decimal StartKm { get; set; }
        public decimal FinishKm { get; set; }
        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
    }
    public class SaveBookingCommandHandler : KooliProjekt.Application.Features.SaveEntityCommandHandler<SaveBookingCommand, Booking>
    {
        public SaveBookingCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(SaveBookingCommand command) => command.Id;
    }
    public class DeleteBookingCommand : IRequest<OperationResult>, ITransactional { public int Id { get; set; } }
    public class DeleteBookingCommandHandler : KooliProjekt.Application.Features.DeleteEntityCommandHandler<DeleteBookingCommand, Booking>
    {
        public DeleteBookingCommandHandler(ApplicationDbContext db) : base(db) { }
        protected override int GetId(DeleteBookingCommand command) => command.Id;
    }
}
