using System;
using System.Threading.Tasks;

namespace SharpResult.FunctionalExtensions;
public static class FunctionalExtensions
{
    public static T Tap<T>(this T item, Action<T> action)
    {
        action(item);
        return item;
    }

    public static async Task<T> Tap<T>(this Task<T> item, Action<T> action)
    {
        var res = await item;
        action(res);
        return res;
    }

    public static async Task<K> Then<T, K>(this Task<T> @this, Func<T, K> fn) => fn(await @this);
    public static async Task<K> Then<T, A, K>(this Task<T> @this, Func<T, A, K> fn, A a) => fn(await @this, a);
    public static async Task<K> Then<T, A, B, K>(this Task<T> @this, Func<T, A, B, K> fn, A a, B b) => fn(await @this, a, b);
    public static async Task<K> Then<T, A, B, C, K>(this Task<T> @this, Func<T, A, B, C, K> fn, A a, B b, C c) => fn(await @this, a, b, c);

    public static async Task<K> Then<T, K>(this Task<T> @this, Func<T, Task<K>> fn) => await fn(await @this);
    public static async Task<K> Then<T, A, K>(this Task<T> @this, Func<T, A, Task<K>> fn, A a) => await fn(await @this, a);
    public static async Task<K> Then<T, A, B, K>(this Task<T> @this, Func<T, A, B, Task<K>> fn, A a, B b) => await fn(await @this, a, b);
    public static async Task<K> Then<T, A, B, C, K>(this Task<T> @this, Func<T, A, B, C, Task<K>> fn, A a, B b, C c) => await fn(await @this, a, b, c);

    public static async Task Then<T>(this Task<T> @this, Action<T> fn) => fn(await @this);
    public static async Task Then<T, A>(this Task<T> @this, Action<T, A> fn, A a) => fn(await @this, a);
    public static async Task Then<T, A, B>(this Task<T> @this, Action<T, A, B> fn, A a, B b) => fn(await @this, a, b);
    public static async Task Then<T, A, B, C>(this Task<T> @this, Action<T, A, B, C> fn, A a, B b, C c) => fn(await @this, a, b, c);

    public static async Task Then<T>(this Task<T> @this, Func<T, Task> fn) => await fn(await @this);
    public static async Task Then<T, A>(this Task<T> @this, Func<T, A, Task> fn, A a) => await fn(await @this, a);
    public static async Task Then<T, A, B>(this Task<T> @this, Func<T, A, B, Task> fn, A a, B b) => await fn(await @this, a, b);
    public static async Task Then<T, A, B, C>(this Task<T> @this, Func<T, A, B, C, Task> fn, A a, B b, C c) => await fn(await @this, a, b, c);
    
    public static async Task<K> Then<K>(this Task @this, Func<K> fn) { await @this; return fn(); }

}