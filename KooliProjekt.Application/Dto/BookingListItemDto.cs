using System;

namespace KooliProjekt.Application.Dto
{
    public class BookingListItemDto
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public string RegistrationNo { get; set; }
        public string UserName { get; set; }
    }
}
