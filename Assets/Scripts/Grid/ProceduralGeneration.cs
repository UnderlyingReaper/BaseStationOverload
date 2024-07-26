using System.Linq;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public Grid grid;
    public GameObject[] buildingPrefabs;
    public GameObject[] treePrefabs;
    public Vector3[] basePositions;
    public float[] weights;
    public int rows = 100;
    public int columns = 100;
    public float cellSize = 1f;
    public float noiseScale1 = 0.1f;
    public float noiseScale2 = 0.05f;
    public float threshold = 0.6f;

    GameObject[,] _placedTrees;
    HouseTrackers _houseTrackers;



    void Start()
    {
        _houseTrackers = GetComponent<HouseTrackers>();

        _placedTrees = new GameObject[columns, rows];
        GenerateTrees();
        GenerateGrid();
    }

    void GenerateTrees()
    {
        // Calculate the starting offsets to center the grid
        int startX = -columns / 2;
        int startZ = -rows / 2;

        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                // Calculate the grid positions with offset
                int gridX = startX + x;
                int gridZ = startZ + z;

                Vector3Int cellPosition = new Vector3Int(gridX, 0, gridZ);

                // Check if the cell position is in the basePositions list
                if (basePositions.Contains(cellPosition)) continue;

                Vector3 worldPosition = grid.CellToWorld(cellPosition); // Center the prefab in the cell

                // Instantiate a tree at each cell
                GameObject placedTree = _placedTrees[x, z] = Instantiate(treePrefabs[Random.Range(0, 2)], worldPosition, Quaternion.identity, transform);

                placedTree.transform.localScale = new Vector3(1, Random.Range(1, 2), 1);
                placedTree.transform.GetChild(0).transform.localPosition = new Vector3(Random.Range(0, 1.0f), 0, Random.Range(0, 1.0f));
                placedTree.layer = gameObject.layer;
            }
        }
    }

    void GenerateGrid()
    {
        // Calculate the starting offsets to center the grid
        int startX = -columns / 2;
        int startZ = -rows / 2;

        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                // Calculate the grid positions with offset
                int gridX = startX + x;
                int gridZ = startZ + z;

                // Calculate Perlin noise values for the current cell
                float noiseValue1 = Mathf.PerlinNoise(gridX * noiseScale1, gridZ * noiseScale1);
                float noiseValue2 = Mathf.PerlinNoise(gridX * noiseScale2, gridZ * noiseScale2);

                // Combine the noise values
                float combinedNoiseValue = (noiseValue1 + noiseValue2) / 2;

                // Check if the combined noise value is above the threshold
                if (combinedNoiseValue > threshold)
                {
                    Vector3Int cellPosition = new Vector3Int(gridX, 0, gridZ);
                    Vector3 worldPosition = grid.CellToWorld(cellPosition); // Center the prefab in the cell

                    // Remove the tree if a house is placed
                    if (_placedTrees[x, z] != null) Destroy(_placedTrees[x, z]);

                    GameObject selectedPrefab = GetWeightedRandomPrefab();
                    GameObject build = Instantiate(selectedPrefab, worldPosition, Quaternion.identity, transform);
                    build.layer = gameObject.layer;
                }
            }
        }

        _houseTrackers.CheckForHouses();
    }

     GameObject GetWeightedRandomPrefab()
    {
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return buildingPrefabs[i];
            }
        }

        // Fallback in case something goes wrong
        return buildingPrefabs[0];
    }
}
