using System.Collections.Generic;
using System.Collections.ObjectModel;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class IntersectableList : List<IIntersectable>, IIntersectable
{
    private IIntersectable? _lastIntersectable;
    public IntersectableList(IIntersectable[] intersectables) : base(intersectables)
    {
    }
    public bool TryIntersect(in Ray ray, out IntersectionResult result)
    {
        result = default;
        bool resultExists = false;
        float t = float.MaxValue;

        foreach (IIntersectable intersectable in this)
        {
            if (intersectable.TryIntersect(ray, out IntersectionResult temp))
            {
                if (temp.T < t)
                {
                    result = temp;
                    t = temp.T;
                    resultExists = true;
                }
            }
        }

        return resultExists;
    }
}