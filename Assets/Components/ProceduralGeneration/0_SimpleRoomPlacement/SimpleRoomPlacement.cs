using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Overlays;
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

        [Header("Path Settings")]
        [SerializeField] private bool _generatePaths = true;

        private List<RectInt> _placedRooms;
        private int _roomCount;


        protected override async UniTask ApplyGenerationAsync(CancellationToken cancellationToken)
        {
            _placedRooms = new List<RectInt>();
            _roomCount = 0;
            for (int i = 0; i < _maxSteps; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                RectInt _tempRect = new RectInt();

                _tempRect.height = RandomService.Range(_roomMinSize.x, _roomMaxSize.x);
                _tempRect.width = RandomService.Range(_roomMinSize.y, _roomMaxSize.y);
                _tempRect.x = RandomService.Range(0, Grid.Width - _tempRect.width);
                _tempRect.y = RandomService.Range(0, Grid.Lenght - _tempRect.height);

                if (!CanPlaceRoom(_tempRect, 1))
                    continue;

                PlaceRoom(_tempRect);
                _placedRooms.Add(_tempRect);
                _roomCount++;

                if (_roomCount >= _maxRooms)
                    break;

                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
            }

            if (_generatePaths)
            {
                ConnectRoomsWithCorridors();
            }

            BuildGround();
        }

        private void ConnectRoomsWithCorridors()
        {
            if (_placedRooms.Count < 2) return;

            for (int i = 0; i < _placedRooms.Count - 1; i++)
            {
                Vector2Int center1 = GetRoomCenter(_placedRooms[i]);
                Vector2Int center2 = GetRoomCenter(_placedRooms[i + 1]);

                CreateDogLegPath(center1, center2);
            }
        }

        private void GetRoomPath(Vector2Int center1, Vector2Int center2)
        {
            if (center1.x != center2.x)
            {
                int difference = center2.x - center1.x;
                CreateHorizontalCorridor(center1.x, center2.x, center1.y);
            }
            if (center1.y != center2.y)
            {
                int difference = center2.y - center1.y;
                CreateVerticalCorridor(center1.y, center2.y, center2.x);
            }
        }

        private void CreateDogLegPath(Vector2Int center1, Vector2Int center2)
        {
            bool horizontalFirst = RandomService.Chance(0.5f);

            if (horizontalFirst)
            {
                CreateHorizontalCorridor(center1.x, center2.x, center1.y);
                CreateVerticalCorridor(center1.y, center2.y, center2.x);
            }
            else
            {
                CreateVerticalCorridor(center1.y, center2.y, center1.x);
                CreateHorizontalCorridor(center1.x, center2.x, center2.y);
            }
        }

        private void CreateHorizontalCorridor(int xStart, int xEnd, int y)
        {
            int startX = Mathf.Min(xStart, xEnd);
            int endX = Mathf.Max(xStart, xEnd);

            for (int x = startX; x <= endX; x++)
            {
                if (Grid.TryGetCellByCoordinates(x, y, out var cell))
                {
                    AddTileToCell(cell, CORRIDOR_TILE_NAME, true);
                }
            }
        }

        private void CreateVerticalCorridor(int yStart, int yEnd, int x)
        {
            int startY = Mathf.Min(yStart, yEnd);
            int endY = Mathf.Max(yStart, yEnd);

            for (int y = startY; y <= endY; y++)
            {
                if (Grid.TryGetCellByCoordinates(x, y, out var cell))
                {
                    AddTileToCell(cell, CORRIDOR_TILE_NAME, true);
                }
            }
        }
    }
}