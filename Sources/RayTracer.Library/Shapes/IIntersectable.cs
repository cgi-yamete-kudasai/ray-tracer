﻿using System.Text.Json.Serialization;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

[JsonPolymorphic]
[JsonDerivedType(typeof(Sphere))]
public interface IIntersectable
{
    bool TryIntersect(in Ray ray, out Vector3 point);

    Vector3 GetNormal(in Vector3 point);
}
