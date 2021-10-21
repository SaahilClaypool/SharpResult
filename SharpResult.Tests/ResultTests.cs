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
}