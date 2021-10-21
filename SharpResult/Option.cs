using System;

namespace SharpResult;

public struct Option<T>
{
    public T Value { get; set; }
    public bool HasValue { get; set; }
    public bool IsSome => HasValue;
    public bool IsNone => !HasValue;

    private Option(T value, bool hasValue)
    {
        Value = value;
        HasValue = hasValue;
    }

    public Option(T value) : this(value, true)
    { }

    public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none) =>
        HasValue ? some(Value) : none();

    public void Match(Action<T> some, Action none)
    {
        if (HasValue)
            some(Value);
        else
            none();
    }

    public TOut Map<TOut>(Func<T, TOut> map, Func<TOut> none) =>
        HasValue ? map(Value) : none();

    public Option<TOut> Bind<TOut>(Func<T, TOut> map) =>
        HasValue ? new Option<TOut>(map(Value)) : new Option<TOut>();

    public Option<TOut> Bind<TOut>(Func<T, Option<TOut>> bind) =>
        HasValue ? bind(Value) : new Option<TOut>();

    public T Unwrap() =>
        HasValue ? Value : throw new Exception("Option was none");


    public static implicit operator Option<T>(NoneOption _) => new();
    public static implicit operator Option<T>(T value) => value != null ? Option.Some(value) : Option.None;
}

public readonly struct NoneOption
{ }

public static class Option
{
    public static Option<T> Some<T>(T value) => new(value);
    public static NoneOption None { get; } = new NoneOption();

    public static bool IsSome<T>(Option<T> option) => option.Match(some => true, () => false);
    public static T Unwrap<T>(Option<T> option) => option.Unwrap();
}