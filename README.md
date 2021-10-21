# Sharp Result

- [Github](https://github.com/SaahilClaypool/SharpResult)
- [Nuget](https://www.nuget.org/packages/SharpResult/)

Inspired by:
- [railway oriented programming](https://www.youtube.com/watch?v=srQt1NAHYC0&t=3009s)
- [simulating type inference](https://tyrrrz.me/blog/return-type-inference)


**Features**:

- Easily box and unbox error values.
    - Success results are implicitly converted to success results
    - Error types can be created *without* specifying generics.
    - `Match` can be used to convert both success and errors to the same unwrapped type
    - `Unwrap` can be used to unsafely get the successful values
- Easily chain together results (Railway oriented programming with `Bind`)

## Examples

> Example unboxing results
```cs
using static SharpResult.Result;

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
}
```

> Example bind usage

```cs
public string HandleRequest(string request) =>
    validate(request)
        .Bind(canonicalizeEmail)
        .Bind(trySaveRequest)
        .Bind(_ => "ok")
        .Match(ok => ok, err => err);

private static Result<string, string> validate(string request) { }
private static Result<string, string> trySaveRequest(string arg) { }
// bind can take either functions taking / returning results, OR functions taking the success / error values
private static string canonicalizeEmail(string request) { } 
```

> Without bind

``` cs
// without bind
public string HandleRequestWithoutBind(string request)
{
    var isValid = validateRequest(request);
    if (!isValid)
    {
        return "Error: validation";
    }

    canonicalizeEmail(request);

    try 
    {
        saveRequest(request);
    }
    catch
    {
        return "Error: save";
    }

    return "ok";
}

```