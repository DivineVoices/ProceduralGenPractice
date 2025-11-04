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
    private List<RectInt> _nodes;
    private int _roomCount;

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        RectInt _firstNodeRect = new RectInt();
        _firstNodeRect.x = 0;
        _firstNodeRect.y = 0;
        _firstNodeRect.width = Grid.Width;
        _firstNodeRect.height = Grid.Lenght;
        for (int i = 0; i < _maxSteps; i++)
        {
            BSPNode _tempNode = new BSPNode(_firstNodeRect, Grid, RandomService, _roomMinSize);
            if (_tempNode.CanSplit())
                _tempNode.Split();

            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }
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
