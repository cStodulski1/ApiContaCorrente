using System.Net;

namespace ApiContaCorrente.Models.Responses
{
    public record Result
    {
        public bool IsSuccess { get; init; }
        public string Message { get; init; }
        public HttpStatusCode StatusCode { get; init; }

        public static Result Success(string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new Result { IsSuccess = true, Message = message, StatusCode = statusCode };
        public static Result Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new Result { IsSuccess = false, Message = message, StatusCode = statusCode };
    }

    public record Result<T> : Result
    {
        public T Data { get; init; }

        public static Result<T> Success(T data, string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new Result<T> { IsSuccess = true, Data = data, Message = message, StatusCode = statusCode };

        public new static Result<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new Result<T> { IsSuccess = false, Message = message, StatusCode = statusCode };
    }

}