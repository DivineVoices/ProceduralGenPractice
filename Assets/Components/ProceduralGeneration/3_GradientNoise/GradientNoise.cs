using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

[CreateAssetMenu(menuName = "Procedural Generation Method/Noise")]
public class GradientNoise : ProceduralGenerationMethod
{
    [SerializeField] private FastNoiseLite noise = new FastNoiseLite();

    [Header("Noise Parameters")]
    [SerializeField, Tooltip("Zoom Level"), Range(0.01f, 1.0f)] private float freakquency = 0.1f;
    [SerializeField, Tooltip("How big/Small variations can go"), Range(0.5f, 1.5f)] private float amp = 1f;

    [Header("Noise Parameters")]
    [SerializeField, Tooltip("How many 'Detail Maps' will be merged into the noise"), Range(1, 5)] private int octaves = 3;
    [SerializeField, Tooltip("How many details will be added"), Range(1f, 3f)] private float lacunarity = 2.0f;
    [SerializeField, Tooltip("How much details matter"), Range(0.5f, 1.0f)] private float persistance = 0.5f;

    [Header("Heights")]
    [SerializeField, Range(-1, 1)] private float waterheight = -0.6f;
    [SerializeField, Range(-1, 1)] private float sandheight = -0.3f;
    [SerializeField, Range(-1, 1)] private float grassheight = 0.8f;
    [SerializeField, Range(-1, 1)] private float rockheight = 1f;

    protected override async UniTask ApplyGenerationAsync(CancellationToken cancellationToken)
    {
        var WaterTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Water");
        var SandTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Sand");
        var GrassTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
        var RockTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Rock");
        SetAll(noise);
        BuildGround();
        float[,] noiseData = new float[Grid.Width, Grid.Lenght];

        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Lenght; y++)
            {
                noiseData[x, y] = noise.GetNoise(x, y);
            }
        }
        for (int j = 0; j < Grid.Width; j++)
        {
            for (int k = 0; k < Grid.Lenght; k++)
            {
                if (!Grid.TryGetCellByCoordinates(j, k, out var chosenCell))
                {
                    Debug.LogError($"Unable to get cell on coordinates : ({j}, {k})");
                    continue;
                }
                switch (noiseData[j, k])
                {
                    case var _ when (noiseData[j, k] > rockheight):
                        GridGenerator.AddGridObjectToCell(chosenCell, RockTemplate, true);
                        break;
                    case var _ when (noiseData[j, k] > grassheight):
                        GridGenerator.AddGridObjectToCell(chosenCell, GrassTemplate, true);
                        break;
                    case var _ when (noiseData[j, k] > sandheight):
                        GridGenerator.AddGridObjectToCell(chosenCell, SandTemplate, true);
                        break;
                    case var _ when (noiseData[j, k] > waterheight):
                        GridGenerator.AddGridObjectToCell(chosenCell, WaterTemplate, true);
                        break;
                    default:
                        //GridGenerator.AddGridObjectToCell(chosenCell, WaterTemplate, true);
                        break;

                }
            }
        }
        await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
    }



    protected new void BuildGround()
    {
        var grassTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");

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
                GridGenerator.AddGridObjectToCell(chosenCell, grassTemplate, false);
            }
        }
    }

    protected void SetAll(FastNoiseLite noise)
    {
        noise.SetSeed(GridGenerator._seed);
        noise.SetFrequency(freakquency);
        noise.SetDomainWarpAmp(amp);
        noise.SetFractalOctaves(octaves);
        noise.SetFractalLacunarity(lacunarity);
        noise.SetFractalGain(persistance);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
    }

}