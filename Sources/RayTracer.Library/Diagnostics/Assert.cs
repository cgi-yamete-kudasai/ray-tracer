using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RayTracer.Library.Diagnostics;

public static class Assert
{
    [DoesNotReturn]
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public static void Fail(
        [CallerMemberName] string? name = null,
        [CallerLineNumber] int line = -1,
        [CallerFilePath] string? file = null)
    {
        throw new AssertionException($"Fail in member {name} on line {line} ({file})");
    }

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public static void True(
        [DoesNotReturnIf(false)] bool value,
        [CallerArgumentExpression(nameof(value))] string? expression = null,
        [CallerMemberName] string? name = null,
        [CallerLineNumber] int line = -1,
        [CallerFilePath] string? file = null)
    {
        if (!value)
            throw new AssertionException($"Expected true, got false: {expression} in member {name} on line {line} ({file})");
    }

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public static void False(
        [DoesNotReturnIf(true)] bool value,
        [CallerArgumentExpression(nameof(value))] string? expression = null,
        [CallerMemberName] string? name = null,
        [CallerLineNumber] int line = -1,
        [CallerFilePath] string? file = null)
    {
        if (value)
            throw new AssertionException($"Expected false, got true: {expression} in member {name} on line {line} ({file})");
    }

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public static void Equal<T>(
        T? lhs,
        T? rhs,
        [CallerMemberName] string? name = null,
        [CallerLineNumber] int line = -1,
        [CallerFilePath] string? file = null)
    {
        if (!EqualityComparer<T>.Default.Equals(lhs, rhs))
            throw new AssertionException($"Expected values to be equal in member {name} on line {line} ({file})");
    }

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public static void NotEqual<T>(
        T? lhs,
        T? rhs,
        [CallerMemberName] string? name = null,
        [CallerLineNumber] int line = -1,
        [CallerFilePath] string? file = null)
    {
        if (EqualityComparer<T>.Default.Equals(lhs, rhs))
            throw new AssertionException($"Expected values to be not equal in member {name} on line {line} ({file})");
    }
}
