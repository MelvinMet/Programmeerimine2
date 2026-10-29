
namespace KooliProjekt.Application.Dto
{
    public class CarDto
    {
        public int Id { get; set; }
        public string RegistrationNo { get; set; }
        public string ModelName { get; set; }
        public string ManufacturerName { get; set; }
        public int CarModelId { get; set; }
        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }
    }
}
