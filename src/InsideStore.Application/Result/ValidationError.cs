using InsideStore.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsideStore.Application.Result
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
