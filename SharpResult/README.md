# Sharp Result

https://github.com/SaahilClaypool/SharpResult

```cs
public class ResultTests
{
    static Result<int, string> MyFailableFunction(int x) => x switch { > 10 => 100, _ => Result.Error("error") };

    [Fact]
    public void BasicErrorConstruction()
    {
        var result = MyFailableFunction(10);
        Assert.True(result.IsError);
        Assert.Equal("error", result.Error);
        Assert.Equal("error", result.Match(ok => ok.ToString(), err => err));
    }

    [Fact]
    public void BasicOkConstruction()
    {
        var result = MyFailableFunction(100);
        Assert.False(result.IsError);
        Assert.True(result.IsOk);
        Assert.Equal(100, result.Ok);
        Assert.Equal(100, result.Match(ok => ok, err => throw new System.Exception("unreachable")));
    }
}
```