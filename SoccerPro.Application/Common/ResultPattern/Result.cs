using SoccerPro.Application.Common.Errors;
using System.Net;

namespace SoccerPro.Application.Common.ResultPattern
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public HttpStatusCode StatusCode { get; set; }
        public T? Value { get; }
        public Error Error { get; }

        private Result(Error error, HttpStatusCode statusCode)
        {
            Value = default;
            Error = error;
            IsSuccess = false;
            StatusCode = statusCode;
        }

        private Result(T? value)
        {
            Value = value;
            Error = Error.None;
            IsSuccess = true;
            StatusCode = HttpStatusCode.OK;
        }

        public static Result<T> Success(T value) => new(value);

        public static Result<T> Failure(Error error, HttpStatusCode statusCode) =>
            new(error, statusCode);
    }
}
