using InsideStore.Domain.Enum;

namespace InsideStore.Application.Results
{
    public class ValidationError : Error
    {
        public List<Error> Errors;

        public ValidationError(List<Error> errors) : base(ErrorType.BadRequest)
        {
            Errors = errors;
        }
    }
}
