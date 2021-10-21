using Xunit;
using Xunit.Abstractions;
using static SharpResult.Result;

namespace SharpResult.Tests;

public class JsonTests : BaseUnitTest
{
    public JsonTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void OkToJson()
    {
        Result<string, string> result = Ok("ok");
        var jsonString = ToJson(result);
        var fromJson = FromJson<Result<string, string>>(jsonString);
        AssertSame(result, jsonString, fromJson);
    }

    [Fact]
    public void ErrToJson()
    {
        Result<string, string> result = Error("error");
        var jsonString = ToJson(result);
        var fromJson = FromJson<Result<string, string>>(jsonString);
        output.WriteLine(jsonString);
        AssertSame(result, jsonString, fromJson);
    }

    private static void AssertSame(Result<string, string> result, string jsonString, Result<string, string> fromJson)
    {
        AssertAll(
            () => Assert.NotEmpty(jsonString),
            () => Assert.Equal(result.IsOk, fromJson.IsOk),
            () => Assert.Equal(result.Match(ok => ok, error => error), fromJson.Match(ok => ok, error => error))
        );
    }


    
    public static string ToJson<T>(T item) => System.Text.Json.JsonSerializer.Serialize(item);
    public static T FromJson<T>(string item) => System.Text.Json.JsonSerializer.Deserialize<T>(item);
}