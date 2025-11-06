using Components.ProceduralGeneration;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] private ProceduralGenerationMethod proceduralGenerationMethodRef;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (proceduralGenerationMethodRef == null || proceduralGenerationMethodRef.Grid == null)
        {
            Debug.LogError("CameraScaler: Missing ProceduralGenerationMethod reference or Grid.");
            return;
        }

        float gridWidth = proceduralGenerationMethodRef.Grid.Width;
        float gridHeight = proceduralGenerationMethodRef.Grid.Lenght;

        // Position camera centered above the grid
        transform.position = new Vector3(gridWidth * 0.5f, 20f, gridHeight * 0.5f);

        // Ensure camera is orthographic
        cam.orthographic = true;

        // Adjust camera size to fit grid horizontally + vertically
        float aspect = (float)Screen.width / Screen.height;
        float sizeByHeight = gridHeight / 2f;
        float sizeByWidth = gridWidth / (2f * aspect);

        cam.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
    }
}
