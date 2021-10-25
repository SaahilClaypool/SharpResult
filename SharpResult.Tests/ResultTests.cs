using Xunit;
using static SharpResult.Result;

namespace SharpResult.Tests;

public class ResultTests
{
    [Fact]
    public void BasicErrorConstruction()
    {
        Result<int, string> result = Error("error");
        Assert.True(result.IsError);
        Assert.Equal("error", result.Error);
        Assert.Equal("error", result.Match(ok => ok.ToString(), err => err));
    }

    [Fact]
    public void BasicOkConstruction()
    {
        Result<int, string> result = 100;
        Assert.False(result.IsError);
        Assert.True(result.IsOk);
        Assert.Equal(100, result.Ok);
        Assert.Equal(100, result.Match(ok => ok, err => throw new System.Exception("unreachable")));
    }

    [Fact]
    public void Try_FunctionsReturningValuesAreConvertedToOk()
    {
        static string okFunc() => "ok";

        var tryOk = Result.Try(okFunc);
        Assert.True(tryOk.IsOk);
        Assert.Equal(okFunc(), tryOk.Unwrap());
    }

    [Fact]
    public void Try_FunctionsThrowingExceptionsAreConvertedToErrors()
    {
        static string errorFunc() => throw new System.Exception("error");

        var tryError = Result.Try(errorFunc);
        Assert.False(tryError.IsOk);
        Assert.Equal("error", tryError.Error.Message);
    }

    [Fact]
    public void Try_FunctionsThrowingExceptionsAreConvertedToErrorsIfInCatchTypes()
    {
        var errorFunc = string () => throw new System.DivideByZeroException("error");
        var tryOk = Result.Try(errorFunc, typeof(System.DivideByZeroException));
        Assert.False(tryOk.IsOk);
        Assert.Equal("error", tryOk.Error.Message);
    }

    [Fact]
    public void Try_FunctionsThrowingExceptionsThrowWhenNotInCatchTypes()
    {
        var errorFunc = string () => throw new System.DivideByZeroException("error");
        try
        {
            var tryOk = Result.Try(errorFunc, typeof(System.Exception));
            Assert.True(false);
        }
        catch (System.Exception e)
        {
            Assert.Equal("error", e.Message);
        }
    }
}