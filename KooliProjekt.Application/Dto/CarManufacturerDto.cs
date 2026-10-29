using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class CarManufacturerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<CarModelListItemDto> Models { get; set; } = new List<CarModelListItemDto>();
    }
}
