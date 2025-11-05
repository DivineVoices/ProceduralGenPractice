using NUnit.Framework;
using UnityEngine;
using VTools.RandomService;
using System.Collections.Generic;

public class BSPNode
{
    private readonly VTools.Grid.Grid _grid;
    private readonly RandomService _randomService;
    private readonly Vector2Int _minSize;

    public bool IsLeaf;
    public BSPNode _parent;
    public BSPNode _child1;
    public BSPNode _child2;
    public RectInt Bounds;
    public RectInt RoomBounds;
    //private RectInt? _finalRoomRect;

    public BSPNode(RectInt bounds, VTools.Grid.Grid grid, RandomService randomService, Vector2Int minSize)
    {
        Bounds = bounds;
        _grid = grid;
        _randomService = randomService;
        _minSize = minSize;
        RoomBounds = bounds;
    }

    public bool CanSplit()
    {
        bool canSplitHorizontally = Bounds.width >= _minSize.x * 2;
        bool canSpliVertically = Bounds.height >= _minSize.y * 2;

        if (canSplitHorizontally && canSpliVertically)
            return true;
        else
            return false;
    }
    public List<BSPNode> Split()
    {
        bool SplitVert = _randomService.Chance(0.5f);
        List<BSPNode> Children = new List<BSPNode>();
        if (SplitVert)
        {
            Debug.Log("Vertical Split");
            int splitPoint = _randomService.Range(1, Bounds.width);
            RectInt _child1Rect = new RectInt(Bounds.x, Bounds.y, splitPoint, Bounds.height);
            RectInt _child2Rect = new RectInt(Bounds.x + splitPoint, Bounds.y, Bounds.width - splitPoint, Bounds.height);
            Children = BuildChildren(_child1Rect, _child2Rect);
            return Children;
        }
        else
        {
            Debug.Log("Horizontal Split");
            int splitPoint = _randomService.Range(1, Bounds.height);
            RectInt _child1Rect = new RectInt(Bounds.x, Bounds.y, Bounds.width, splitPoint);
            RectInt _child2Rect = new RectInt(Bounds.x, Bounds.y + splitPoint, Bounds.width, Bounds.height - splitPoint );
            Children = BuildChildren(_child1Rect, _child2Rect);
            return Children;
        }
    }

    private List<BSPNode> BuildChildren(RectInt _child1Rect, RectInt _child2Rect)
    {
        List<BSPNode> Children = new List<BSPNode>();
        BSPNode child1 = new BSPNode(_child1Rect, _grid, _randomService, _minSize);
        BSPNode child2 = new BSPNode(_child2Rect, _grid, _randomService, _minSize);
        Debug.Log("Child1 startX = " + child1.Bounds.x);
        Debug.Log("Child2 startX = " + child2.Bounds.x);
        _child1 = child1;
        _child2 = child2;
        child1._parent = this;
        child2._parent = this;
        Children.Add(child1);
        Children.Add(child2);
        return Children;
    }
}
