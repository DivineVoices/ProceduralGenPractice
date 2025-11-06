using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using VTools.Grid;
using VTools.RandomService;
using VTools.ScriptableObjectDatabase;

[CreateAssetMenu(menuName = "Procedural Generation Method/Cellular")]
public class CellularAutomata : ProceduralGenerationMethod
{
    [SerializeField] private float _noiseLevel;
    [SerializeField] private List<Vector2Int> _surroundingTiles;
    [SerializeField] private int _grassCount;
    [SerializeField] private int _grassRequirement = 4;
    [SerializeField] private bool _generateWater; //If _grassCount < _grassRequirement, Tile = Water
    protected override async UniTask ApplyGenerationAsync(CancellationToken cancellationToken)
    {
        var grassTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
        var waterTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Water");
        BuildGround();

        for (int i = 0; i < _maxSteps; i++)
        {
            for (int j = 0; j < Grid.Width; j++)
            { 
                for (int k = 0; k < Grid.Lenght; k++)
                {
                    _surroundingTiles = new List<Vector2Int>();
                    foreach (var offset in NeighborOffsets)
                    {
                        int nx = j + offset.x;
                        int ny = k + offset.y;

                        if (Grid.TryGetCellByCoordinates(nx, ny, out var neighbor))
                            _surroundingTiles.Add(new Vector2Int(nx, ny));
                    }
                    _grassCount = 0;
                    foreach (Vector2Int tile in _surroundingTiles)
                    {
                        if (Grid.TryGetCellByCoordinates(tile.x, tile.y, out Cell cell))
                        {
                            if (cell.GridObject != null && cell.GridObject.Template != null)
                            {
                                if (cell.GridObject.Template.Name == GRASS_TILE_NAME)
                                    _grassCount++;
                            }
                        }
                        if (_grassCount > _grassRequirement)
                        {
                            if (Grid.TryGetCellByCoordinates(j, k, out var chosenCell))
                                GridGenerator.AddGridObjectToCell(chosenCell, grassTemplate, true);
                        }
                        else
                        {
                            if (_generateWater)
                            { 
                                if (Grid.TryGetCellByCoordinates(j, k, out var chosenCell))
                                    GridGenerator.AddGridObjectToCell(chosenCell, waterTemplate, true);
                            }
                        }
                    }

                }
            }
            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }
    }

    protected new void BuildGround()
    {
        var grassTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
        var waterTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Water");

        // Instantiate ground blocks
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int z = 0; z < Grid.Lenght; z++)
            {
                if (!Grid.TryGetCellByCoordinates(x, z, out var chosenCell))
                {
                    Debug.LogError($"Unable to get cell on coordinates : ({x}, {z})");
                    continue;
                }
                bool grassTile = RandomService.Chance(_noiseLevel);
                if (grassTile)
                    GridGenerator.AddGridObjectToCell(chosenCell, grassTemplate, false);
                else
                    GridGenerator.AddGridObjectToCell(chosenCell, waterTemplate, false);

            }
        }
    }

    private static readonly Vector2Int[] NeighborOffsets = new Vector2Int[]
    {
        new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
        new Vector2Int(-1,  0),                        new Vector2Int(1,  0),
        new Vector2Int(-1,  1), new Vector2Int(0,  1), new Vector2Int(1,  1),
    };
}
