using InsideStore.Domain.Enum;

namespace InsideStore.Application.Results
{
    public class Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
        public static readonly Error NullValue = new("Error.NullValue", "Null Value was providad", ErrorType.Failure);

        public Error(ErrorType type)
        {
            Type = type;
        }

        public Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        private Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);

        public static Error BadRequest(string code, string description) =>
            new(code, description, ErrorType.BadRequest);

        public static Error Unauthorized(string code, string description) =>
            new(code, description, ErrorType.Unauthorized);

        public static Error Forbidden(string code, string description) =>
            new(code, description, ErrorType.Forbidden);

        public static Error Unknown() =>
            new("Server.Error", "Erro desconhecido");
    }
}
