using System;
using System.Threading.Tasks;
using Xunit;
using static SharpResult.Result;
using SharpResult.FunctionalExtensions;


namespace SharpResult.Tests;

// WIP
public static class Fn
{
    public static async Task DoSomething()
    {
        var s = "testing";
        s
            .Then(Truncate, 3)
            .Then(Console.WriteLine);

        
        await GetAfter(100)
            .ThenAsync(val => val * 100)
            .ThenAsync(async after => await Task.Delay(TimeSpan.FromMilliseconds(after)));

        var op = Option.Some(100);

        var isSOme = 200.Filter(val => val > 50);
        var none = op.Filter(val => val > 200);
    }

    static async Task<int> GetAfter(int ms) => Task.Delay(TimeSpan.FromMilliseconds(ms)).Then(_ => ms);

    public static string Truncate(string s, int length) => s[..length];

}