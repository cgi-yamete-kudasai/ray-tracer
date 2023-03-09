﻿using System;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Library.Serialization.Serializers;

namespace RayTracer.Library.Shapes;

public class Disc : IIntersectable, ISerializable<Disc>
{
    public Vector3 Center { get; }
    public Vector3 Normal { get; }
    public float Radius { get; }

    public Disc(Vector3 center, Vector3 normal, float radius)
    {
        Center = center;
        Normal = Vector3.Normalize(normal);
        Radius = radius;
    }

    public bool TryIntersect(in Ray ray, out IntersectionResult result)
    {
        float denominator = Vector3.Dot(Normal, ray.Direction);
        
        if (MathF.Abs(denominator) > IntersectionHelper.INTERSECTION_TOLERANCE)
        {
            Vector3 vector = Center - ray.Origin;
            float t = Vector3.Dot(vector, Normal) / denominator;
            
            if (t >= 0)
            {
                result = new (ray.Origin + t * ray.Direction,t, Normal);
                return (result.Point - Center).Length() <= Radius;
            }
        }
        
        result = default;
        return false;
    }
    
    static ISerializer<Disc> ISerializable<Disc>.Serializer => DiscSerializer.Instance;

}