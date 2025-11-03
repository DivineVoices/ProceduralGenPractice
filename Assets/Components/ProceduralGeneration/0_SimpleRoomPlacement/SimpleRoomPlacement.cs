using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

namespace Components.ProceduralGeneration.SimpleRoomPlacement
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Simple Room Placement")]
    public class SimpleRoomPlacement : ProceduralGenerationMethod
    {
        [Header("Room Parameters")]
        [SerializeField] private int _maxRooms = 10;
        [SerializeField] private Vector2Int _roomMinSize;
        [SerializeField] private Vector2Int _roomMaxSize;
        private List<RectInt> _placedRooms;
        private int _roomCount;


        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            _placedRooms = new List<RectInt>();
            _roomCount = 0;
            for (int i = 0; i < _maxSteps; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();

                RectInt _tempRect = new RectInt();

                _tempRect.height = RandomService.Range(_roomMinSize.x, _roomMaxSize.x);
                _tempRect.width = RandomService.Range(_roomMinSize.y, _roomMaxSize.y);
                _tempRect.x = RandomService.Range(0, 63);
                _tempRect.y = RandomService.Range(0, 63);

                if (!CanPlaceRoom(_tempRect, 1)) 
                    continue;

                PlaceRoom(_tempRect);
                _placedRooms.Add(_tempRect);
                _roomCount++;

                if (_roomCount >= _maxRooms)
                    break;

                // Waiting between steps to see the result.
                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken : cancellationToken);
            }

            // Final ground building.
            BuildGround();
        }
        
        private void BuildGround()
        {
            var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
            
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
                    
                    GridGenerator.AddGridObjectToCell(chosenCell, groundTemplate, false);
                }
            }
        }

        private void PlaceRoom(RectInt room)
        {
            for (int ix = room.xMin; ix < room.xMax; ix++)
            {
                for (int iy = room.yMin; iy < room.yMax; iy++)
                {
                    if (!Grid.TryGetCellByCoordinates(ix, iy, out var cell))
                        continue;

                    AddTileToCell(cell, ROOM_TILE_NAME, true);
                }
            }
        }
        private Vector2Int GetRoomCenter(RectInt room)
        {
            return new Vector2Int(
                room.x + room.width / 2,
                room.y + room.height / 2
            );
        }
    }
}