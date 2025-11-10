using NUnit.Framework;
using UnityEngine;
using VTools.RandomService;
using System.Collections.Generic;

public class BSPNode
{
    private readonly VTools.Grid.Grid _grid;
    private readonly RandomService _randomService;
    private readonly Vector2Int _minSize;
    private readonly Vector2Int _maxSize;

    public bool IsLeaf = true;
    public BSPNode _parent;
    public BSPNode _child1;
    public BSPNode _child2;
    public RectInt Bounds;
    public RectInt RoomBounds;

    public BSPNode(RectInt bounds, VTools.Grid.Grid grid, RandomService randomService, Vector2Int minSize, Vector2Int maxSize)
    {
        Bounds = bounds;
        _grid = grid;
        _randomService = randomService;
        _minSize = minSize;
        _maxSize = maxSize;
        RoomBounds = CreateRoomWithinBounds(bounds);
    }

    public bool CanSplit()
    {
        bool canSplitHorizontally = Bounds.width >= _minSize.x * 2;
        bool canSplitVertically = Bounds.height >= _minSize.y * 2;

        return canSplitHorizontally && canSplitVertically;
    }

    public List<BSPNode> Split()
    {
        bool splitVert = _randomService.Chance(0.5f);
        List<BSPNode> children = new List<BSPNode>();

        if (splitVert)
        {
            int minSplit = _minSize.x;
            int maxSplit = Bounds.width - _minSize.x;
            if (minSplit >= maxSplit) return children;

            int splitPoint = _randomService.Range(minSplit, maxSplit);
            RectInt child1Rect = new RectInt(Bounds.x, Bounds.y, splitPoint, Bounds.height);
            RectInt child2Rect = new RectInt(Bounds.x + splitPoint, Bounds.y, Bounds.width - splitPoint, Bounds.height);
            children = BuildChildren(child1Rect, child2Rect);
        }
        else
        {
            int minSplit = _minSize.y;
            int maxSplit = Bounds.height - _minSize.y;
            if (minSplit >= maxSplit) return children;

            int splitPoint = _randomService.Range(minSplit, maxSplit);
            RectInt child1Rect = new RectInt(Bounds.x, Bounds.y, Bounds.width, splitPoint);
            RectInt child2Rect = new RectInt(Bounds.x, Bounds.y + splitPoint, Bounds.width, Bounds.height - splitPoint);
            children = BuildChildren(child1Rect, child2Rect);
        }

        IsLeaf = false;
        return children;
    }

    private List<BSPNode> BuildChildren(RectInt child1Rect, RectInt child2Rect)
    {
        List<BSPNode> children = new List<BSPNode>();
        BSPNode child1 = new BSPNode(child1Rect, _grid, _randomService, _minSize, _maxSize);
        BSPNode child2 = new BSPNode(child2Rect, _grid, _randomService, _minSize, _maxSize);

        _child1 = child1;
        _child2 = child2;
        child1._parent = this;
        child2._parent = this;

        children.Add(child1);
        children.Add(child2);
        return children;
    }

    private RectInt CreateRoomWithinBounds(RectInt bounds)
    {
        int roomWidth = _randomService.Range(_minSize.x, Mathf.Min(_maxSize.x, bounds.width));
        int roomHeight = _randomService.Range(_minSize.y, Mathf.Min(_maxSize.y, bounds.height));

        int maxX = bounds.x + bounds.width - roomWidth;
        int maxY = bounds.y + bounds.height - roomHeight;

        if (maxX < bounds.x) maxX = bounds.x;
        if (maxY < bounds.y) maxY = bounds.y;

        int roomX = _randomService.Range(bounds.x, maxX + 1);
        int roomY = _randomService.Range(bounds.y, maxY + 1);

        return new RectInt(roomX, roomY, roomWidth, roomHeight);
    }
}