using System;

namespace KooliProjekt.Application.Dto
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public string RegistrationNo { get; set; }
        public string UserName { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
        public decimal StartKm { get; set; }
        public decimal FinishKm { get; set; }
        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }
    }
}
