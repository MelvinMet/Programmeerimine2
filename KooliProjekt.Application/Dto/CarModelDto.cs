
namespace KooliProjekt.Application.Dto
{
    public class CarModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ManufacturerName { get; set; }
        public int CarManufacturerId { get; set; }
        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }
        public int CarCount { get; set; }
    }
}
