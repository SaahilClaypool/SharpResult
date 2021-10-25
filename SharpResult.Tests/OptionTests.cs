using System.Linq;
using Xunit;
using static SharpResult.Option;

namespace SharpResult.Tests;

public class OptionTests
{
    static Option<int> MyOkayFunction(int x) => x switch { > 10 => 100, _ => None };

    [Fact]
    public void BasicErrorConstruction()
    {
        var ok = MyOkayFunction(10);
        Assert.False(ok.IsSome);
        Assert.Equal("none", ok.Match(ok => throw new System.Exception("unreachable"), none: () => "none"));
    }

    [Fact]
    public void BasicOkConstruction()
{
        var ok = MyOkayFunction(100);
        Assert.True(ok.IsSome);
        Assert.Equal(100, ok.Match(ok => ok, none: () => throw new System.Exception("unreachable")));
        Assert.Equal(100, ok.Unwrap());
    }
}