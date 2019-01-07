using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    int ROWS = 8, COLS = 12;

    [Serializable]
    public class Count
    {
        public int min;
        public int max;

        public Count(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
    public Count wallCount = new Count(5, 9);


    // Holds our prefabs during game
    public GameObject[] floorTiles;
    public GameObject[] objectTiles;
    public GameObject[] wallTiles;

    // Keeps our hierarchy clean, making it collapsable
    private Transform boardHolder;

    // Track all our positions
    private List<Vector3> positions = new List<Vector3>();

    // Create board
    void InitializeList()
    {
        positions.Clear();

        // This is random allocation of the map
        for(int x = 1; x < COLS - 1; x++)
        {
            for(int y = 1; y < ROWS - 1; y++)
            {
                positions.Add(new Vector3(x, y, 0));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Gameboard").transform;

        for(int x = -1; x < COLS + 1; x++)
        {
            for(int y = -1; y < ROWS + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                // Check to see if on a wall position
                if (x == -1 || y == -1 || x == COLS || y == ROWS)
                    toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }


    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, positions.Count);
        Vector3 randomPosition = positions[randomIndex];
        positions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tiles, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tiles[Random.Range(0, tiles.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.min, wallCount.max);

    }










    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
