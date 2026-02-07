using System.Net;

namespace cusho.Dtos;

public class ApiResponse<T>

{
    public HttpStatusCode StatusCode { get; init; }
    public bool Success { get; init; }
    public T? Data { get; init; }
    public List<string> Errors { get; init; }

    public ApiResponse()
    {
        StatusCode = HttpStatusCode.OK;
        Success = true;
        Errors = [];
        Data = default;
    }

    public ApiResponse(HttpStatusCode statusCode, T data)
    {
        StatusCode = statusCode;
        Success = true;
        Data = data;
        Errors = [];
    }

    public ApiResponse(HttpStatusCode statusCode, string error)
    {
        StatusCode = statusCode;
        Success = false;
        Errors = [error];
        Data = default;
    }

    public ApiResponse(HttpStatusCode statusCode, List<string> errors)
    {
        StatusCode = statusCode;
        Success = false;
        Errors = errors;
        Data = default;
    }
}