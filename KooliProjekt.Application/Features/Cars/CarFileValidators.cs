using FluentValidation;

namespace KooliProjekt.Application.Features.Cars
{
    public class AddFileCommandValidator : AbstractValidator<AddFileCommand>
    {
        public AddFileCommandValidator() { RuleFor(x => x.Id).GreaterThan(0); RuleFor(x => x.FileName).NotEmpty().Must(IsSafeName).WithMessage("File name must be a plain file name."); RuleFor(x => x.InputStream).NotNull(); }
        internal static bool IsSafeName(string name) => !string.IsNullOrWhiteSpace(name) && name == System.IO.Path.GetFileName(name) && !name.Contains("..") && !name.Contains('/') && !name.Contains('\\');
    }
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator() { RuleFor(x => x.Id).GreaterThan(0); RuleFor(x => x.FileName).Must(AddFileCommandValidator.IsSafeName).WithMessage("File name must be a plain file name."); }
    }
    public class GetFileQueryValidator : AbstractValidator<GetFileQuery>
    {
        public GetFileQueryValidator() { RuleFor(x => x.Id).GreaterThan(0); RuleFor(x => x.FileName).Must(AddFileCommandValidator.IsSafeName).WithMessage("File name must be a plain file name."); }
    }
    public class ListFilesQueryValidator : AbstractValidator<ListFilesQuery>
    {
        public ListFilesQueryValidator() { RuleFor(x => x.Id).GreaterThan(0); }
    }
}
