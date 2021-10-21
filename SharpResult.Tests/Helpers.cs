global using static SharpResult.Tests.Helpers ;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace SharpResult.Tests;

public static class Helpers
{
    public static void AssertAll(params Action[] assertions)
    {
        var errors = new List<System.Exception>();
        foreach (var assertion in assertions)
        {
            try
            {
                assertion();
            }
            catch (Exception exc)
            {
                errors.Add(exc);
            }
        }

        if (errors.Any())
        {
            var seperator = "\n\n";
            string message = string.Join(seperator, errors);
            Assert.True(false, message);
        }
    }
}

public class BaseUnitTest
{
    protected readonly ITestOutputHelper output;

    public BaseUnitTest(ITestOutputHelper output) => this.output = output;
}