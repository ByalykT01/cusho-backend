using System.Diagnostics.CodeAnalysis;

namespace cusho.Common;

public class Result<T>
{
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Error))]
    public bool  IsFailure => !IsSuccess; 
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
    
    public static implicit operator Result<T>(T value) => Success(value);
}
