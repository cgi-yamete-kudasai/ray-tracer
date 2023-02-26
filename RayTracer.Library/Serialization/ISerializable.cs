namespace RayTracer.Library.Serialization;

public interface ISerializable<T>
{
    static abstract ISerializer<T> Serializer { get; }
}
