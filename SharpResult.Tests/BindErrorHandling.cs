using System.Collections.Generic;
using Xunit;
using static SharpResult.Result;

namespace SharpResult.Tests;

public class BindTests
{
    public static string WithoutBind(string request)
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

    private static void saveRequest(string request)
    {
        if (request.Contains("X")) throw new System.Exception();
    }

    private static string canonicalizeEmail(string request)
    {
        return request;
    }

    private static bool validateRequest(string request) => !string.IsNullOrEmpty(request);

    
    public static string WithBind(string request) =>
        validate(request)
            .Bind(canonicalizeEmail)
            .Bind(trySaveRequest)
            .Bind(_ => "ok")
            .Match(ok => ok, err => err);

    private static Result<string, string> trySaveRequest(string arg)
    {
        try
        {
            saveRequest(arg);
            return arg;
        }
        catch
        {
            return Error("Error: save");
        }
    }

    private static Result<string, string> validate(string request)
    {
        if (string.IsNullOrEmpty(request))
            return Error("Error: validation");
        return request;
    }

    [Theory]
    [MemberData(nameof(Emails))]
    public void BindGiveSameResults(string email)
    {
        Assert.Equal(WithoutBind(email), WithBind(email));
    }

    public static IEnumerable<object[]> Emails = new List<object[]>()
    {
        new object[] { "saahil@claypools.org" },
        new object[] { "X@gmail.com" },
        new object[] { "" }
    };
}
