using InsideStore.Application.Results;
using InsideStore.Domain.Enum;

namespace InsideStore.Api.ResultExtensions
{
    public static class ResultExtensions
    {
        public static IResult ToProblem(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }
            var errorType = result.Error.Type;

            return Results.Problem(
                statusCode: GetStatusCode(errorType),
                title: GetTitle(errorType),
                type: GetType(errorType),
                extensions: GetErrors(result));
        }

        static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {

                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.BadRequest => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.InternalServerError => StatusCodes.Status500InternalServerError
            };

        static string GetTitle(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Forbidden => "Forbidden",
                ErrorType.BadRequest => "Bad Request",
                ErrorType.NotFound => "Not found",
                ErrorType.Conflict => "Conflict",
                ErrorType.Unauthorized => "Unauthorized",
                ErrorType.InternalServerError => "Server Failure"
            };

        static string GetType(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Forbidden => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                ErrorType.BadRequest => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                ErrorType.Conflict => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
                ErrorType.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                ErrorType.InternalServerError => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };

        static Dictionary<string, object?> GetErrors(Result result)
        {
            if (result.Error is ValidationError validationError)
            {
                return new Dictionary<string, object?>
            {
                { "errors", validationError.Errors }
            };
            }

            return new Dictionary<string, object?>
        {
            { "errors", new List<Error>() { result.Error } }
        };
        }
    }
}
