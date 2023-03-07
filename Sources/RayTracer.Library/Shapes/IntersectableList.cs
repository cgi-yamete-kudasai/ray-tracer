using System.Collections;
using System.Collections.Generic;
using RayTracer.Library.Mathematics;

namespace RayTracer.Library.Shapes;

public class IntersectableList : ICollection<IIntersectable>, IIntersectable
{
    public int Count => _intersectables.Count;

    public bool IsReadOnly => false;

    private List<IIntersectable> _intersectables;

    public IntersectableList()
    {
        _intersectables = new();
    }

    public IntersectableList(IEnumerable<IIntersectable> intersectables)
    {
        _intersectables = new(intersectables);
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

    public IEnumerator<IIntersectable> GetEnumerator()
    {
        return _intersectables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(IIntersectable item)
    {
        _intersectables.Add(item);
    }

    public void Clear()
    {
        _intersectables.Clear();
    }

    public bool Contains(IIntersectable item)
    {
        return _intersectables.Contains(item);
    }

    public void CopyTo(IIntersectable[] array, int arrayIndex)
    {
        _intersectables.CopyTo(array, arrayIndex);
    }

    public bool Remove(IIntersectable item)
    {
        return _intersectables.Remove(item);
    }
}