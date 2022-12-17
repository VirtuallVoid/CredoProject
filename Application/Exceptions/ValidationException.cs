using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Exceptions
{
    public class ValidationException : Exception
    {
        public string Code { get; set; }
        public ValidationException() : base("Validation failure have occurred.")
        {
        }
        public string Error { get; }
        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            foreach (var failure in failures)
            {
                Error = failure.ErrorMessage;
                Code = failure.ErrorCode;
                break;
            }
        }
    }
}
