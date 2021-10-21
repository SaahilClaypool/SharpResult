using System;

namespace SharpResult;

public struct Result<TOk, TError>
{
    public TOk Ok { get; set; }
    public TError Error { get; set; }
    public bool IsError { get; set; }
    public bool IsOk => !IsError;

    private Result(TOk ok, TError error, bool isError)
    {
        Ok = ok;
        Error = error;
        IsError = isError;
    }
    public Result(TOk ok) : this(ok, default, false) { }
    public Result(TError error) : this(default, error, true) { }

    public Result<TOut, TError> Bind<TOut>(Func<TOk, TOut> mapOk) => Bind(mapOk, err => err);
    public Result<TOut, TOutErr> Bind<TOut, TOutErr>(Func<TOk, TOut> mapOk, Func<TError, TOutErr> mapErr)
    {
        if (IsError)
            return Result.Error(mapErr(Error));
        return Result.Ok(mapOk(Ok));
    }

    public Result<TOut, TError> Bind<TOut>(Func<TOk, Result<TOut, TError>> mapOk) => Bind(mapOk, (Func<TError, Result<TOut, TError>>)(err => Result.Error(err)));
    public Result<TOut, TOutError> Bind<TOut, TOutError>(Func<TOk, Result<TOut, TOutError>> mapOk, Func<TError, Result<TOut, TOutError>> mapErr)
    {
        if (IsError)
            return mapErr(Error);
        return mapOk(Ok);
    }

    public TOut Match<TOut>(Func<TOk, TOut> mapOk, Func<TError, TOut> mapErr) =>
        !IsError ? mapOk(Ok) : mapErr(Error);

    public TOk Unwrap() =>
        !IsError ? Ok : throw new Exception($"Result had error: {Error}");

    public static implicit operator Result<TOk, TError>(DelayedOk<TOk> ok) =>
        new(ok.Value);

    public static implicit operator Result<TOk, TError>(DelayedError<TError> error) =>
        new(error.Value);

    public static implicit operator Result<TOk, TError>(TOk ok) =>
        new(ok);
}

public readonly struct DelayedOk<T>
{
    public T Value { get; }

    public DelayedOk(T value)
    {
        Value = value;
    }
}

public readonly struct DelayedError<T>
{
    public T Value { get; }

    public DelayedError(T value)
    {
        Value = value;
    }
}

public static class Result
{
    public static DelayedOk<TOk> Ok<TOk>(TOk ok) =>
        new(ok);

    public static DelayedError<TError> Error<TError>(TError error) =>
        new(error);

    public static bool IsOk<T, R>(Result<T, R> result) => result.Match(ok => true, err => false);
    public static T Unwrap<T, R>(Result<T, R> result) => result.Unwrap();
}

