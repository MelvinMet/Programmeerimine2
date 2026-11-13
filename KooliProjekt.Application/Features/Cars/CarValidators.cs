using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Cars
{
    public class GetCarQueryValidator : AbstractValidator<GetCarQuery>
    {
        public GetCarQueryValidator(ApplicationDbContext db) { RuleFor(x => x.Id).GreaterThan(0).MustAsync((id, ct) => db.Cars.AnyAsync(c => c.Id == id, ct)).WithMessage("Car does not exist."); }
    }
    public class ListCarsQueryValidator : AbstractValidator<ListCarsQuery>
    {
        public ListCarsQueryValidator() { RuleFor(x => x.Page).GreaterThanOrEqualTo(1); RuleFor(x => x.PageSize).InclusiveBetween(1, 100); }
    }
    public class SaveCarCommandValidator : AbstractValidator<SaveCarCommand>
    {
        public SaveCarCommandValidator(ApplicationDbContext db)
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.RegistrationNo).NotEmpty().MaximumLength(20);
            RuleFor(x => x.CarModelId).GreaterThan(0).MustAsync((id, ct) => db.CarModels.AnyAsync(m => m.Id == id, ct)).WithMessage("Car model does not exist.");
            RuleFor(x => x.Id).MustAsync(async (id, ct) => id == 0 || await db.Cars.AnyAsync(c => c.Id == id, ct)).WithMessage("Car does not exist.");
        }
    }
    public class DeleteCarCommandValidator : AbstractValidator<DeleteCarCommand>
    {
        public DeleteCarCommandValidator(ApplicationDbContext db) { RuleFor(x => x.Id).GreaterThan(0).MustAsync((id, ct) => db.Cars.AnyAsync(c => c.Id == id, ct)).WithMessage("Car does not exist."); }
    }
}
