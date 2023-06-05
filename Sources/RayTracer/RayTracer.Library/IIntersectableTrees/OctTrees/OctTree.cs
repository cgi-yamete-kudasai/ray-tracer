using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using RayTracer.Library.Mathematics;
using RayTracer.Library.Shapes;

namespace RayTracer.Library.IIntersectableTrees.OctTrees;

public class OctTree : ITree, IIntersectable
{
    private List<IIntersectable> _objects;
    private OctTree? _parent;
    private static readonly int MaxTreeDepth = 1;

    private static Queue<IIntersectable> _pendingInsertion = new();

    private const int MinSize = 1;
    private int _treeDepth;

    private static bool _treeBuilt = false;
    private static bool _treeReady = false;

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

    public OctTree(BoundingBox region)
    {
        BB = region;
        _objects = new List<IIntersectable>();
    }

    public void Add(IIntersectable @object)
    {
        _pendingInsertion.Enqueue(@object);
        _treeReady = false;
    }

    public void Add(ImmutableArray<IIntersectable> objects)
    {
        foreach (var @object in objects)
        {
            _pendingInsertion.Enqueue(@object);
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

        _treeReady = true;
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
        if (dimensions is { X: <= MinSize, Y: <= MinSize, Z: <= MinSize })
        {
            return;
        }

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
        List<IIntersectable> objects = new List<IIntersectable>(1);
        objects[0] = @object;

        return new OctTree(bb, objects)
        {
            _parent = this,
            _treeDepth = _treeDepth + 1
        };
    }

    private void BuildTree()
    {
        if (_treeDepth >= MaxTreeDepth)
        {
            return;
        }

        if (BB.Size == 0)
        {
            RecalculateBoundingBox();
        }

        Vector3 dimensions = BB.Max - BB.Min;
        if (dimensions is { X: <= MinSize, Y: <= MinSize, Z: <= MinSize })
        {
            return;
        }

        BoundingBox[] octant = CreateNewOctant();

        List<IIntersectable>[] octantList = new List<IIntersectable>[8];
        InitializeOctantLists(octantList);

        PopulateOctantLists(octant, octantList);

        BuildChildNodes(octantList, octant);

        _treeBuilt = true;
        _treeReady = true;
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
            _parent = this,
            _treeDepth = _treeDepth + 1
        };
    }

    private void RecalculateBoundingBox()
    {
        //TODO: refactor to Pow 2
        BoundingBox bb = _objects[0].BB;
        for (int i = 1; i < _objects.Count; i++)
        {
            bb = BoundingBox.Union(bb, _objects[i].BB);
        }

        BB = bb;
    }

    private BoundingBox[] CreateNewOctant()
    {
        Vector3 dimensions = BB.Max - BB.Min;
        Vector3 half = dimensions / 2.0f;
        Vector3 center = BB.Min + half;

        BoundingBox[] octant = new BoundingBox[8];
        // octant[0] = new BoundingBox(BB.Min, center);
        // octant[1] = new BoundingBox(new Vector3(center.X, BB.Min.Y, BB.Min.Z),
        //     new Vector3(BB.Max.X, center.Y, center.Z));
        // octant[2] = new BoundingBox(new Vector3(center.X, BB.Min.Y, center.Z),
        //     new Vector3(BB.Max.X, center.Y, BB.Max.Z));
        // octant[3] = new BoundingBox(new Vector3(BB.Min.X, BB.Min.Y, center.Z),
        //     new Vector3(center.X, center.Y, BB.Max.Z));
        // octant[4] = new BoundingBox(new Vector3(BB.Min.X, center.Y, BB.Min.Z),
        //     new Vector3(center.X, BB.Max.Y, center.Z));
        // octant[5] = new BoundingBox(new Vector3(center.X, center.Y, BB.Min.Z),
        //     new Vector3(BB.Max.X, BB.Max.Y, center.Z));
        // octant[6] = new BoundingBox(center, BB.Max);
        // octant[7] = new BoundingBox(new Vector3(BB.Min.X, center.Y, center.Z),
        //     new Vector3(center.X, BB.Max.Y, BB.Max.Z));
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
        throw new NotImplementedException();
    }

    public BoundingBox BB { get; private set; }

    public bool TryIntersect(in Ray ray, out IntersectionResult result)
    {
        throw new NotImplementedException();
    }
}