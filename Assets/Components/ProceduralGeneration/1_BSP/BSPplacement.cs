using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Procedural Generation Method/BSP")]
public class BSPplacement : ProceduralGenerationMethod
{
    [Header("Room Parameters")]
    [SerializeField] private int _maxRooms = 10;
    [SerializeField] private int _childrenCount = 2;
    [SerializeField] private Vector2Int _roomMinSize;
    [SerializeField] private Vector2Int _roomMaxSize;
    [NonSerialized] private int loops = 0;
    private List<RectInt> _nodeRects;
    private List<BSPNode> _nodes;
    private int _roomCount;

    protected override async UniTask ApplyGenerationAsync(CancellationToken cancellationToken)
    {
        _nodes = new List<BSPNode>();

        BSPNode root = new BSPNode(
            new RectInt(0, 0, Grid.Width, Grid.Lenght),
            Grid,
            RandomService,
            _roomMinSize,
            _roomMaxSize
        );

        _nodes.Add(root);

        bool didSplit = true;
        while (didSplit && _nodes.Count < _maxRooms)
        {
            didSplit = false;
            List<BSPNode> nodesToProcess = new List<BSPNode>(_nodes);

            foreach (var node in nodesToProcess)
            {
                if (node.IsLeaf && node.CanSplit() && _nodes.Count < _maxRooms)
                {
                    var children = node.Split();
                    if (children.Count > 0)
                    {
                        _nodes.AddRange(children);
                        didSplit = true;

                        if (_nodes.Count >= _maxRooms) break;

                        await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
                    }
                }
            }
        }

        foreach (var node in _nodes)
        {
            if (node.IsLeaf)
            {
                if (CanPlaceRoom(node.RoomBounds, 1))
                {
                    PlaceRoom(node.RoomBounds);
                }
            }
        }

        BuildGround();
    }

    private void CheckNodes(BSPNode node)
    {
        if (node._child1 == null)
        {
            Debug.Log(loops);
            loops = 0;
            return;
        }
        loops++;
        CheckNodes(node._child1);
    }
}