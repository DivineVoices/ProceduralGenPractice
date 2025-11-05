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
        private List<RectInt> _placedRooms;
        private int _roomCount;


        protected override async UniTask ApplyGenerationAsync(CancellationToken cancellationToken)
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
                _tempRect.x = RandomService.Range(0, Grid.Width - _tempRect.width);
                _tempRect.y = RandomService.Range(0, Grid.Lenght - _tempRect.height);

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

        private void GetRoomPath(Vector2Int center1, Vector2Int center2)
        {
            if (center1.x != center2.x)
            {
                int difference = center2.x - center1.x;
            }
        }

        private void CreateDogLegPath(Vector2Int center1, Vector2Int center2)
        {
            bool horizontalFirst = RandomService.Chance(0.5f);

            if (horizontalFirst) {
                //CreateHorizontalCorridor(center1.x, center2.y)
            }
        }

    }
}