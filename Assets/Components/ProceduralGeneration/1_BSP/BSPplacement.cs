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
            _roomMinSize
        );

        _nodes.Add(root);

        bool didSplit = true;
        while (didSplit && _nodes.Count < _maxRooms)
        {
            didSplit = false;
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].IsLeaf && _nodes[i].CanSplit())
                {
                    _nodes[i].Split();
                    _nodes.Add(_nodes[i]._child1);
                    _nodes.Add(_nodes[i]._child2);
                    didSplit = true;

                    await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
                }
            }
        }

        foreach (var node in _nodes)
        {
            if (node.IsLeaf)
                PlaceRoom(node.RoomBounds);
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
