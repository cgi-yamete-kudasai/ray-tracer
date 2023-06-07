using System.Collections;
using System.Collections.Generic;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Serialization;
using RayTracer.Library.Serialization.Serializers;

namespace RayTracer.Library.Shapes;

public class IntersectableList : ICollection<IIntersectable>, IIntersectable, ISerializable<IntersectableList>
{
    public int Count => _intersectables.Count;

    public bool IsReadOnly => false;

    public BoundingBox BB => _boundingBox;
    private List<IIntersectable> _intersectables;
    private BoundingBox _boundingBox;


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

    public bool TryIntersectAny(in Ray ray, out IntersectionResult result)
    {
        foreach (IIntersectable intersectable in this)
        {
            if (intersectable.TryIntersect(ray, out var intResult))
            {
                result = intResult;
                return true;
            }
        }

        result = default;
        return false;
    }

    public void Transform(WorldTransform wt)
    {
        foreach (var shape in _intersectables)
            shape.Transform(wt);
        
        _boundingBox = CalculateBoundingBox();
    }

    private BoundingBox CalculateBoundingBox()
    {
        BoundingBox bb = BoundingBox.Zero;

        foreach (var shape in _intersectables)
            BoundingBox.Union(ref bb, shape.BB);

        return bb;
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
        if (Count == 0)
        {
            _boundingBox = item.BB;
        }
        else
        {
            BoundingBox.Union(ref _boundingBox, item.BB);
        }


        _intersectables.Add(item);
    }

    public void Clear()
    {
        _intersectables.Clear();
        _boundingBox = BoundingBox.Zero;
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

    public static ISerializer<IntersectableList> Serializer => IntersectableListSerializer.Instance;
}