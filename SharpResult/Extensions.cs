using System;
using System.Threading.Tasks;

namespace SharpResult.FunctionalExtensions;
public static class FunctionalExtensions
{
    public static K Then<T, K>(this T @this, Func<T, K> fn) => fn(@this);
    public static K Then<T, A, K>(this T @this, Func<T, A, K> fn, A a) => fn(@this, a);
    public static K Then<T, A, B, K>(this T @this, Func<T, A, B, K> fn, A a, B b) => fn(@this, a, b);
    public static K Then<T, A, B, C, K>(this T @this, Func<T, A, B, C, K> fn, A a, B b, C c) => fn(@this, a, b, c);

    public static async Task<K> ThenAsync<T, K>(this Task<T> @this, Func<T, K> fn) => fn(await @this);
    public static async Task<K> ThenAsync<T, A, K>(this Task<T> @this, Func<T, A, K> fn, A a) => fn(await @this, a);
    public static async Task<K> ThenAsync<T, A, B, K>(this Task<T> @this, Func<T, A, B, K> fn, A a, B b) => fn(await @this, a, b);
    public static async Task<K> ThenAsync<T, A, B, C, K>(this Task<T> @this, Func<T, A, B, C, K> fn, A a, B b, C c) => fn(await @this, a, b, c);

    public static async Task<K> ThenAsync<T, K>(this Task<T> @this, Func<T, Task<K>> fn) => await fn(await @this);
    public static async Task<K> ThenAsync<T, A, K>(this Task<T> @this, Func<T, A, Task<K>> fn, A a) => await fn(await @this, a);
    public static async Task<K> ThenAsync<T, A, B, K>(this Task<T> @this, Func<T, A, B, Task<K>> fn, A a, B b) => await fn(await @this, a, b);
    public static async Task<K> ThenAsync<T, A, B, C, K>(this Task<T> @this, Func<T, A, B, C, Task<K>> fn, A a, B b, C c) => await fn(await @this, a, b, c);

    public static void Then<T>(this T @this, Action<T> fn) => fn(@this);
    public static void Then<T, A>(this T @this, Action<T, A> fn, A a) => fn(@this, a);
    public static void Then<T, A, B>(this T @this, Action<T, A, B> fn, A a, B b) => fn(@this, a, b);
    public static void Then<T, A, B, C>(this T @this, Action<T, A, B, C> fn, A a, B b, C c) => fn(@this, a, b, c);

    public static async Task ThenAsync<T>(this Task<T> @this, Action<T> fn) => fn(await @this);
    public static async Task ThenAsync<T, A>(this Task<T> @this, Action<T, A> fn, A a) => fn(await @this, a);
    public static async Task ThenAsync<T, A, B>(this Task<T> @this, Action<T, A, B> fn, A a, B b) => fn(await @this, a, b);
    public static async Task ThenAsync<T, A, B, C>(this Task<T> @this, Action<T, A, B, C> fn, A a, B b, C c) => fn(await @this, a, b, c);

    public static async Task ThenAsync<T>(this Task<T> @this, Func<T, Task> fn) => await fn(await @this);
    public static async Task ThenAsync<T, A>(this Task<T> @this, Func<T, A, Task> fn, A a) => await fn(await @this, a);
    public static async Task ThenAsync<T, A, B>(this Task<T> @this, Func<T, A, B, Task> fn, A a, B b) => await fn(await @this, a, b);
    public static async Task ThenAsync<T, A, B, C>(this Task<T> @this, Func<T, A, B, C, Task> fn, A a, B b, C c) => await fn(await @this, a, b, c);
}