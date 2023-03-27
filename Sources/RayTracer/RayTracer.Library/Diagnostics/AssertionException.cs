using System;

namespace RayTracer.Library.Diagnostics;

public class AssertionException : ApplicationException
{
    public AssertionException(string message)
        : base($"ASSERT: {message}")
    { }
}
