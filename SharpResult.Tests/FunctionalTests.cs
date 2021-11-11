using System;
using System.Threading.Tasks;
using Xunit;
using static SharpResult.Result;
using SharpResult.FunctionalExtensions;
using System.Linq;


namespace SharpResult.Tests;

// WIP
public class Fn
{
    public static async Task DoSomething()
    {
        var f = string (string s) => Truncate(s, 3);
        var s = "testing";
        s
            .Tap(val => Console.WriteLine(val));

        
        await GetAfter(100)
            .Then(val => val * 100)
            .Then(async after => await Task.Delay(TimeSpan.FromMilliseconds(after)));

        var op = Option.Some(100);

        var isSome = 200.Filter(val => val > 50);
        var none = op.Filter(val => val > 200);
    }

    private static Result<int, string> multTwo(Result<int, string> res) => res.Bind(ok => ok * 2);

    static async Task<int> GetAfter(int ms) => await Task.Delay(TimeSpan.FromMilliseconds(ms)).Then(() => ms);

    public static string Truncate(string s, int length) => s[..length];

}