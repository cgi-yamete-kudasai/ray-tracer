using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.IIntersectableTrees.OctTrees;

public class OctTree : IIntersectable
{
    public bool TreeReady;
    public BoundingBox BB { get; private set; }

    private readonly List<IIntersectable> _objects;


    // private OctTree? _parent;
    private const int MaxTreeDepth = 10;
    private const int MinObjectsPerNode = 20;

    private readonly Queue<IIntersectable> _pendingInsertion = new();

    private int _treeDepth;

    private bool _treeBuilt;

    private OctTree?[]? _childNodes;


    private OctTree(BoundingBox bb, List<IIntersectable> objects)
    {
        BB = bb;
        _objects = objects;
    }

    public OctTree()
    {
        _objects = new List<IIntersectable>();
        BB = BoundingBox.Zero;
    }

    public OctTree(ImmutableArray<IIntersectable> objects)
    {
        _objects = new List<IIntersectable>();
        BB = BoundingBox.Zero;
        Add(objects);
    }

    public OctTree(BoundingBox region)
    {
        BB = region;
        _objects = new List<IIntersectable>();
    }

    public void Add(IIntersectable @object)
    {
        if (@object is IntersectableList list)
        {
            Add(list);
            return;
        }

        _pendingInsertion.Enqueue(@object);
        TreeReady = false;
    }

    private void Add(IntersectableList list)
    {
        foreach (var @object in list)
        {
            Add(@object);
        }
    }

    public void Add(ImmutableArray<IIntersectable> objects)
    {
        foreach (var @object in objects)
        {
            Add(@object);
        }
    }

    public void UpdateTree()
    {
        if (!_treeBuilt)
        {
            while (_pendingInsertion.Count != 0)
            {
                _objects.Add(_pendingInsertion.Dequeue());
            }

            BuildTree();
        }
        else
        {
            while (_pendingInsertion.Count != 0)
            {
                Insert(_pendingInsertion.Dequeue());
            }
        }

        TreeReady = true;
    }

    private void Insert(IIntersectable @object)
    {
        if (_treeDepth >= MaxTreeDepth)
        {
            _objects.Add(@object);
            return;
        }

        Vector3 dimensions = BB.Max - BB.Min;
        Vector3 half = dimensions / 2.0f;
        Vector3 center = BB.Min + half;

        BoundingBox[] childOctant = CreateChildOctantIfNotExists(center);

        if (@object.BB.Size != 0 && BB.Contains(@object.BB) == ContainmentType.Contains)
        {
            bool found = TryInsertInChildOctant(@object, childOctant);

            if (!found)
            {
                _objects.Add(@object);
            }
        }
        else
        {
            throw new Exception("Node is bigger then Tree itself");
        }
    }

    private bool TryInsertInChildOctant(IIntersectable @object, BoundingBox[] childOctant)
    {
        for (int i = 0; i < 8; i++)
        {
            if (childOctant[i].Contains(@object.BB) == ContainmentType.Contains)
            {
                _childNodes ??= new OctTree[8];

                if (_childNodes[i] is null)
                {
                    _childNodes[i] = CreateNode(childOctant[i], @object);
                }
                else
                {
                    _childNodes[i]!.Insert(@object);
                }

                return true;
            }
        }

        return false;
    }

    private BoundingBox[] CreateChildOctantIfNotExists(Vector3 center)
    {
        BoundingBox[] childOctant = new BoundingBox[8];
        childOctant[0] = _childNodes?[0]?.BB ?? new BoundingBox(BB.Min, center);
        childOctant[1] = _childNodes?[1]?.BB ?? new BoundingBox(new Vector3(center.X, BB.Min.Y, BB.Min.Z),
            new Vector3(BB.Max.X, center.Y, center.Z));
        childOctant[2] = _childNodes?[2]?.BB ?? new BoundingBox(new Vector3(center.X, BB.Min.Y, center.Z),
            new Vector3(BB.Max.X, center.Y, BB.Max.Z));
        childOctant[3] = _childNodes?[3]?.BB ?? new BoundingBox(new Vector3(BB.Min.X, BB.Min.Y, center.Z),
            new Vector3(center.X, center.Y, BB.Max.Z));
        childOctant[4] = _childNodes?[4]?.BB ?? new BoundingBox(new Vector3(BB.Min.X, center.Y, BB.Min.Z),
            new Vector3(center.X, BB.Max.Y, center.Z));
        childOctant[5] = _childNodes?[5]?.BB ?? new BoundingBox(new Vector3(center.X, center.Y, BB.Min.Z),
            new Vector3(BB.Max.X, BB.Max.Y, center.Z));
        childOctant[6] = _childNodes?[6]?.BB ?? new BoundingBox(center, BB.Max);
        childOctant[7] = _childNodes?[7]?.BB ?? new BoundingBox(new Vector3(BB.Min.X, center.Y, center.Z),
            new Vector3(center.X, BB.Max.Y, BB.Max.Z));
        return childOctant;
    }

    private OctTree CreateNode(BoundingBox bb, IIntersectable @object)
    {
        List<IIntersectable> objects = new List<IIntersectable>(1)
        {
            [0] = @object
        };

        return new OctTree(bb, objects)
        {
            // _parent = this,
            _treeDepth = _treeDepth + 1
        };
    }

    private void BuildTree()
    {
        if (_treeDepth >= MaxTreeDepth || _objects.Count <= MinObjectsPerNode)
        {
            return;
        }

        if (BB.Size == 0)
        {
            RecalculateBoundingBox();
        }

        BoundingBox[] octant = CreateNewOctant();

        List<IIntersectable>[] octantList = new List<IIntersectable>[8];
        InitializeOctantLists(octantList);

        PopulateOctantLists(octant, octantList);

        BuildChildNodes(octantList, octant);

        _treeBuilt = true;
        TreeReady = true;
    }

    private void PopulateOctantLists(BoundingBox[] octant, List<IIntersectable>[] octantList)
    {
        List<IIntersectable> deleteList = new List<IIntersectable>();

        //TODO: refactor with for
        foreach (IIntersectable obj in _objects)
        {
            for (int i = 0; i < 8; i++)
            {
                if (octant[i].Contains(obj.BB) == ContainmentType.Contains)
                {
                    octantList[i].Add(obj);
                    deleteList.Add(obj);
                    break;
                }
            }
        }

        foreach (IIntersectable obj in deleteList)
        {
            _objects.Remove(obj);
        }
    }

    private static void InitializeOctantLists(List<IIntersectable>[] octantList)
    {
        for (int i = 0; i < 8; i++)
        {
            octantList[i] = new List<IIntersectable>();
        }
    }

    private void BuildChildNodes(List<IIntersectable>[] octantList, BoundingBox[] octant)
    {
        for (int i = 0; i < 8; i++)
        {
            if (octantList[i].Count != 0)
            {
                _childNodes ??= new OctTree[8];
                _childNodes[i] = CreateNode(octant[i], octantList[i]);
                _childNodes[i]?.BuildTree();
            }
        }
    }


    private OctTree? CreateNode(BoundingBox bb, List<IIntersectable> objects)
    {
        if (objects.Count == 0)
        {
            return null;
        }

        return new OctTree(bb, objects)
        {
            // _parent = this,
            _treeDepth = _treeDepth + 1
        };
    }

    private void RecalculateBoundingBox()
    {
        //TODO: refactor to Pow 2
        BoundingBox bb = _objects[0].BB;
        for (int i = 1; i < _objects.Count; i++)
        {
            BoundingBox.Union(ref bb, _objects[i].BB);
        }

        BB = bb;
    }

    private BoundingBox[] CreateNewOctant()
    {
        Vector3 dimensions = BB.Max - BB.Min;
        Vector3 half = dimensions / 2.0f;
        Vector3 center = BB.Min + half;

        BoundingBox[] octant = new BoundingBox[8];
        for (int i = 0; i < 8; i++)
        {
            octant[i] = CreateNewOctant(i, center);
        }

        return octant;
    }

    private BoundingBox CreateNewOctant(int i, Vector3 center)
    {
        switch (i)
        {
            case 0: return new BoundingBox(BB.Min, center);
            case 1:
                return new BoundingBox(new Vector3(center.X, BB.Min.Y, BB.Min.Z),
                    new Vector3(BB.Max.X, center.Y, center.Z));
            case 2:
                return new BoundingBox(new Vector3(center.X, BB.Min.Y, center.Z),
                    new Vector3(BB.Max.X, center.Y, BB.Max.Z));
            case 3:
                return new BoundingBox(new Vector3(BB.Min.X, BB.Min.Y, center.Z),
                    new Vector3(center.X, center.Y, BB.Max.Z));
            case 4:
                return new BoundingBox(new Vector3(BB.Min.X, center.Y, BB.Min.Z),
                    new Vector3(center.X, BB.Max.Y, center.Z));
            case 5:
                return new BoundingBox(new Vector3(center.X, center.Y, BB.Min.Z),
                    new Vector3(BB.Max.X, BB.Max.Y, center.Z));
            case 6: return new BoundingBox(center, BB.Max);
            case 7:
                return new BoundingBox(new Vector3(BB.Min.X, center.Y, center.Z),
                    new Vector3(center.X, BB.Max.Y, BB.Max.Z));
            default:
                throw new ArgumentException();
        }
    }


    public void Transform(WorldTransform wt)
    {
        ReturnAllToQueue();
        _childNodes = null;
        TransformObjects(wt);
        BB = BoundingBox.Zero;
        _treeBuilt = false;
        TreeReady = false;
        UpdateTree();
    }

    private void TransformObjects(WorldTransform wt)
    {
        foreach (var @object in _pendingInsertion)
        {
            @object.Transform(wt);
        }
    }

    private void ReturnAllToQueue()
    {
        foreach (IIntersectable obj in _objects)
        {
            _pendingInsertion.Enqueue(obj);
        }

        _objects.Clear();

        if (_childNodes == null)
        {
            return;
        }

        foreach (var child in _childNodes)
        {
            child?.ReturnAllToQueue();
        }
    }


    public bool TryIntersect(in Ray ray, out IntersectionResult result)
    {
        bool resultExists = false;
        float tResult = float.MaxValue;
        result = default;

        if (_objects.Count == 0 && !HasChildren)
        {
            result = new IntersectionResult();
            return false;
        }

        if (TryIntersectObjects(in ray, out IntersectionResult objectsResult))
        {
            resultExists = true;
            tResult = objectsResult.T;
            result = objectsResult;
        }

        Span<float> tOctant = stackalloc float[8];
        Span<int> tOctantIndex = stackalloc int[8];

        tOctantIndex = InitSpans(ray, tOctantIndex, ref tOctant);

        tOctant.Sort(tOctantIndex);

        for (int i = 0; i < 8; i++)
        {
            if (Math.Abs(tOctant[i] - float.MaxValue) > 0.0001f)
            {
                bool childIntersected = _childNodes![tOctantIndex[i]]!.TryIntersect(in ray,
                    out IntersectionResult childNodeResult);
                if (childIntersected && childNodeResult.T < tResult)
                {
                    resultExists = true;
                    result = childNodeResult;

                    return resultExists;
                }
            }
        }

        return resultExists;
    }

    public bool TryIntersectAny(in Ray ray, out IntersectionResult result)
    {
        result = default;

        if (_objects.Count == 0 && !HasChildren)
        {
            result = new IntersectionResult();
            return false;
        }

        if (TryIntersectObjects(in ray, out IntersectionResult objectsResult))
        {
            result = objectsResult;
            return true;
        }

        Span<float> tOctant = stackalloc float[8];
        Span<int> tOctantIndex = stackalloc int[8];

        tOctantIndex = InitSpans(ray, tOctantIndex, ref tOctant);

        tOctant.Sort(tOctantIndex);

        for (int i = 0; i < 8; i++)
        {
            if (Math.Abs(tOctant[i] - float.MaxValue) > 0.0001f)
            {
                if (_childNodes![tOctantIndex[i]]!.TryIntersect(in ray,
                        out IntersectionResult childNodeResult))
                {
                    result = childNodeResult;

                    return true;
                }
            }
        }

        return false;
    }

    private Span<int> InitSpans(Ray ray, Span<int> tOctantIndex, ref Span<float> tOctant)
    {
        for (int i = 0; i < 8; i++)
        {
            tOctantIndex[i] = i;
            tOctant[i] = float.MaxValue;
            if (_childNodes?[i] != null && _childNodes[i]!.BB.IntersectsWithRay(in ray, out float t))
            {
                tOctant[i] = t;
            }
        }

        return tOctantIndex;
    }


    private bool TryIntersectObjects(in Ray ray, out IntersectionResult result)
    {
        result = default;
        bool resultExists = false;
        float t = float.MaxValue;

        foreach (var @object in _objects)
        {
            if (@object.TryIntersect(ray, out IntersectionResult temp))
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


    private bool HasChildren
    {
        get
        {
            if (_childNodes is null)
            {
                return false;
            }

            for (int i = 0; i < 8; i++)
            {
                if (_childNodes[i] != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}