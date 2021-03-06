using System;
using System.Threading.Tasks;
using Xunit;
using static SharpResult.Result;
using SharpResult.FunctionalExtensions;

namespace SharpResult.Tests;

public class ResultTests
{
    [Fact]
    public void BasicErrorConstruction()
    {
        Result<int, string> result = Error("error");
        Assert.True(result.IsError);
        Assert.Equal("error", result.Match(_ => throw new Exception(), error => error));
        Assert.Equal("error", result.Match(ok => ok.ToString(), err => err));
    }

    [Fact]
    public void BasicOkConstruction()
    {
        Result<int, string> result = 100;
        Assert.False(result.IsError);
        Assert.True(result.IsOk);
        Assert.Equal(100, result.Unwrap());
        Assert.Equal(100, result.Match(ok => ok, err => throw new System.Exception("unreachable")));
    }

    [Fact]
    public void Async_ExtensionsExample()
    {
        var add1 = (int inp) => inp + 1;
        var add1Delay = (int inp) => Task.FromResult(inp + 1);

        Result<int, string> ok = Result.Ok(10);
        ok.Bind(add1).Bind(add1Delay).Bind(val => val.Then(add1));
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
        Assert.Equal("error", tryError.Match(_ => throw new Exception(), error => error).Message);
    }

    [Fact]
    public void Bind_AppliesFunctionsToOks()
    {
        var mult2 = int (int input) => input * 2;
        var add1 = int (int input) => input + 1;
        var add1IfGreaterThan3 = Result<int, string> (int input) => input switch
            {
                > 3 => input + 1, _ => Error("value too small")
            };
        Result<int, string> ok = Ok(1);
        var result = ok.Bind(mult2).Bind(add1);
        Assert.Equal(3, result.Unwrap());
        var err = result.Bind(add1IfGreaterThan3);
        Assert.True(err.IsError);
        var finalOutput = result.Bind(add1).Bind(add1IfGreaterThan3);
        Assert.Equal(5, finalOutput.Unwrap());
    }

    [Fact]
    public void Try_FunctionsThrowingExceptionsAreConvertedToErrorsIfInCatchTypes()
    {
        var errorFunc = string () => throw new System.DivideByZeroException("error");
        var tryOk = Result.Try(errorFunc, typeof(System.DivideByZeroException));
        Assert.False(tryOk.IsOk);
        Assert.Equal("error", tryOk.Match(_ => throw new Exception(), err => err).Message);
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

    [Fact]
    public async Task Try_FunctionsReturningValuesAreConvertedToOkAsync()
    {
        static Task<string> okFunc() => Task.FromResult("ok");

        var tryOk = await Result.Try(okFunc);
        Assert.True(tryOk.IsOk);
        Assert.Equal(await okFunc(), tryOk.Unwrap());
    }

    [Fact]
    public async Task Try_FunctionsThrowingExceptionsAreConvertedToErrorsAsync()
    {
        static Task<string> errorFunc() => throw new System.Exception("error");

        var tryError = await Result.Try(errorFunc);
        Assert.False(tryError.IsOk);
        Assert.Equal("error", tryError.Match(_ => throw new Exception(), err => err).Message);
    }

    [Fact]
    public void ToOption_Ok_Some()
    {
        Result<int, string> result = Ok(100);
        var option = result.ToOption();
        Assert.True(option.IsSome);
        Assert.Equal(100, option.Unwrap());
    }

    [Fact]
    public void ToOption_Error_None()
    {
        Result<int, string> result = Error("err");
        // implicit
        Option<int> option = result;
        Assert.True(option.IsNone);
    }
}