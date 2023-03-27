using System;

namespace RayTracer.Library.Serialization;

public class SerializationException : ApplicationException
{
    public SerializationException(string message)
        : base(message)
    { }
}
