using System;
using System.Linq;
using System.Threading.Tasks;

namespace SharpResult;

// disable obselete warnings inside this package
#pragma warning disable 612,618
public struct Result<TOk, TError>
{
    [Obsolete("Use 'Unwrap' to access directly, throwing an error if undefined")]
    public TOk Ok { get; set; }

    [Obsolete("Use 'Match' to access safely")]
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

    public Result<TOut, Exception> Try<TOut>(Func<TOk, TOut> mapOk)
    {
        try
        {
            return Bind(mapOk, _ => new Exception("try on failed result."));
        }
        catch(Exception ex)
        {
            return Result.Error(ex);
        }
    }

    public Option<TOk> ToOption() => Match(ok => Option.Some(ok), _ => Option.None);

    public TOut Match<TOut>(Func<TOk, TOut> mapOk, Func<TError, TOut> mapErr) =>
        !IsError ? mapOk(Ok) : mapErr(Error);

    public TOk Unwrap() =>
        !IsError ? Ok : throw new Exception($"Result had error: {Error}");

    public TError UnwrapError() =>
        IsError ? Error : throw new Exception($"Result had no error: {Ok}");

    public static implicit operator Result<TOk, TError>(DelayedOk<TOk> ok) =>
        new(ok.Value);

    public static implicit operator Result<TOk, TError>(DelayedError<TError> error) =>
        new(error.Value);

    public static implicit operator Result<TOk, TError>(TOk ok) =>
        new(ok);

    public static implicit operator Option<TOk>(Result<TOk, TError> result) =>
        result.ToOption();
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
    public static Option<T> ToOption<T, R>(Result<T, R> result) => result.ToOption();

    public static Result<T, Exception> Try<T> (Func<T> action, params Type[] catchTypes)
    {
        try
        {
            return action();
        }
        catch (Exception e)
        {
            if (catchTypes.Length == 0 || catchTypes.Contains(e.GetType()))
            {
                return Error(e);
            }
            throw;
        }
    }

    public static async Task<Result<T, Exception>> Try<T> (Func<Task<T>> action, params Type[] catchTypes)
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            if (catchTypes.Length == 0 || catchTypes.Contains(e.GetType()))
            {
                return Error(e);
            }
            throw;
        }
    }
}

public static class ResultAsyncExtensions
{
    public static async Task<Result<TOut, TError>> Bind<TOk, TOut, TError>(this Task<Result<TOk, TError>> res, Func<TOk, TOut> mapOk) =>
        (await res).Bind(mapOk, err => err);

    public static async Task<Result<TOut, TOutError>> Bind<TOk, TOut, TError, TOutError>(this Task<Result<TOk, TError>> res, Func<TOk, TOut> mapOk, Func<TError, TOutError> mapErr) =>
        (await res).Bind(mapOk, mapErr);

    public static async Task<Result<TOut, TError>> Bind<TOk, TOut, TError>(this Task<Result<TOk, TError>> res, Func<TOk, Result<TOut, TError>> mapOk) =>
        (await res).Bind(mapOk, err => (Result<TOut, TError>)Result.Error(err));

    public static async Task<Result<TOut, TError>> Bind<TOk, TOut, TError>(this Task<Result<TOk, TError>> res, Func<TOk, Result<TOut, TError>> mapOk, Func<TError, Result<TOut, TError>> mapErr) =>
        (await res).Bind(mapOk, mapErr);

    public static async Task<TOut> Bind<TOk, TError, TOut>(this Task<Result<TOk, TError>> res, Func<TOk, TOut> mapOk, Func<TError, TOut> mapErr) =>
        (await res).Match(mapOk, mapErr);

}
