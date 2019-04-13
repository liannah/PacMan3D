using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public int sizeX, sizeY;

    //public MazeCell mazePrefab;
    public Transform obstaclePrefab;
    public Transform plusPrefab;
    public Transform pickUpPrefab;
    public Transform navMeshFloor;
    public Transform navMeshPRefab;
    public Transform mazePrefab;
    private Transform[,] cells;
    public float waitforgenerator;
    private int seed;
    public int countObstcl;
    public int countPick;
    public int plusCount = 3;

    [Range(0f, 1f)]
    public float outlinePercent;

    [Range(0f, 1f)]
    public float obstPercent;

    private List<Coord> allCoord;
    private Queue<Coord> shuffledCoord;
    private Queue<Coord> shuffledOpenCoord;

    public GameObject mapHolder;

    //private Transform[,] transformMaze;
    public void Regenerate()
    {
        Destroy(mapHolder);
        Generate();
    }

    public void Generate()
    {
        seed = Utility.RandomNumber(0);
        Coord mapCentre = new Coord((sizeX) / 2, (sizeY) / 2);
        cells = new Transform[sizeX, sizeY];
        allCoord = new List<Coord>();
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                allCoord.Add(new Coord(x, y));
            }
        }

        shuffledCoord = new Queue<Coord>(Utility.ShuffleArray(allCoord.ToArray(), seed));

        mapHolder = new GameObject("Generated Maze");
        mapHolder.transform.SetParent(this.transform);

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                // yield return delay;
                Vector3 mazePosition = CoordToPosition(i, j);
                Transform mazeCell = Instantiate(mazePrefab) as Transform;

                mazeCell.name = "Maze Cell " + i + ", " + j;
                mazeCell.transform.parent = transform;
                mazeCell.transform.localPosition = mazePosition;
                mazeCell.transform.localScale = Vector3.one * (1 - outlinePercent);
                cells[i, j] = mazeCell;
            }
        }

        List<Coord> allOpenCoord = new List<Coord>(allCoord);
        countObstcl = (int)(sizeX * sizeY * obstPercent);
        bool[,] obstacleMaze = new bool[sizeX, sizeY];
        int currentObstacleCount = 0;
        bool[,] borderMaze = new bool[sizeX, sizeY];

        countPick = Utility.RandomNumber(1);

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (i == 0)
                {
                    Vector3 obstPos = CoordToPosition(i, j);
                    Coord pos = new Coord(i, j);
                    Transform newObstacle = Instantiate(obstaclePrefab, obstPos + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                    borderMaze[i, j] = true;
                    newObstacle.parent = mapHolder.transform;
                    allOpenCoord.Remove(pos);
                    currentObstacleCount++;
                }
                else if (i == sizeX - 1)
                {
                    Vector3 obstPos = CoordToPosition(i, j);
                    Coord pos = new Coord(i, j);

                    Transform newObstacle = Instantiate(obstaclePrefab, obstPos + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                    borderMaze[i, j] = true;
                    newObstacle.parent = mapHolder.transform;
                    allOpenCoord.Remove(pos);
                    currentObstacleCount++;
                }
                else if (j == 0)
                {
                    Vector3 obstPos = CoordToPosition(i, j);
                    Coord pos = new Coord(i, j);

                    Transform newObstacle = Instantiate(obstaclePrefab, obstPos + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                    borderMaze[i, j] = true;
                    newObstacle.parent = mapHolder.transform;
                    allOpenCoord.Remove(pos);
                    currentObstacleCount++;
                }
                else if (j == sizeY - 1)
                {
                    Vector3 obstPos = CoordToPosition(i, j);
                    Coord pos = new Coord(i, j);
                    Transform newObstacle = Instantiate(obstaclePrefab, obstPos + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                    borderMaze[i, j] = true;
                    newObstacle.parent = mapHolder.transform;
                    allOpenCoord.Remove(pos);
                    currentObstacleCount++;
                }
            }
        }

        for (int d = 0; d < countObstcl; d++)
        {
            Coord random1 = RandomCoord();
            obstacleMaze[random1.x, random1.y] = true;
            currentObstacleCount++;

            // obstacleMap[randomCoord.x, randomCoord.y] = true;
            //currentObstacleCount++;
            Vector3 obstPos = CoordToPosition(random1.x, random1.y);
            if (random1 != mapCentre && (borderMaze[random1.x, random1.y] == false) && MazeIsFullyAccessible(obstacleMaze, currentObstacleCount, borderMaze))
            {
                //  if (pick == obstPos)
                //    pick = CoordToPosition(random.x, random.y);
                Transform newObstacle = Instantiate(obstaclePrefab, obstPos + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder.transform;
                allOpenCoord.Remove(random1);
            }
            else
            {
                obstacleMaze[random1.x, random1.y] = false;
                currentObstacleCount--;
            }
        }

        shuffledOpenCoord = new Queue<Coord>(Utility.ShuffleArray(allOpenCoord.ToArray(), seed));

        for (int d = 0; d < countPick; d++)
        {
            Coord random = RandomOpenCoord();
            // obstacleMap[randomCoord.x, randomCoord.y] = true;
            //currentObstacleCount++;
            Vector3 pick = CoordToPosition(random.x, random.y);
            Transform newPickup = Instantiate(pickUpPrefab, pick + Vector3.up * 0.5f, Quaternion.identity) as Transform;
            newPickup.parent = mapHolder.transform;
        }

        for(int b = 0; b < plusCount; b++)
        {
            Coord random = RandomOpenCoord();
            Vector3 pick = CoordToPosition(random.x, random.y);
            Transform newPickup = Instantiate(plusPrefab, pick + Vector3.up * 0.5f, Quaternion.identity) as Transform;
            newPickup.parent = mapHolder.transform;
        }
        //Transform maskLeft = Instantiate(navMeshPRefab, Vector3.left*(sizeX)/4, Quaternion.identity) as Transform;
        //maskLeft.parent = mapHolder;
        //maskLeft.localScale = new Vector3(sizeX / 2, 1, sizeY);
        navMeshFloor.localScale = new Vector3(sizeX, sizeY);
    }

    private bool MazeIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount, bool[,] borderMaze)
    {
        bool[,] mapFlags = new bool[sizeX, sizeY];
        Queue<Coord> queue = new Queue<Coord>();
        Coord mazeCenter = new Coord((sizeX) / 2, (sizeY) / 2);
        queue.Enqueue(mazeCenter);
        mapFlags[mazeCenter.x, mazeCenter.y] = true;

        int accessibleCount = 1;

        while (queue.Count > 0)
        {
            Coord coord = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = coord.x + x;
                    int neighbourY = coord.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < (sizeX) && neighbourY >= 0 && neighbourY < (sizeY))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY] && !borderMaze[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleCount = (int)((sizeX) * (sizeY) - currentObstacleCount);
        return targetAccessibleCount == accessibleCount;
    }

    private Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(x - (sizeX) * 0.5f + 0.5f, 0f, y - (sizeY) * 0.5f + 0.5f);
    }

    public Transform MazefromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x + (sizeX) / 2f);
        int y = Mathf.RoundToInt(position.z + (sizeY) / 2f);
        x = Mathf.Clamp(x, 0, sizeX);
        y = Mathf.Clamp(y, 0, sizeY);
        return cells[x, y];
    }

    public Coord RandomCoord()
    {
        Coord random = shuffledCoord.Dequeue();
        shuffledCoord.Enqueue(random);
        return random;
    }

    public Coord RandomOpenCoord()
    {
        Coord random = shuffledOpenCoord.Dequeue();
        shuffledCoord.Enqueue(random);
        return random;
    }

    public Transform GetRandomOpenMazeCoord()
    {
        Coord randomCoord = shuffledOpenCoord.Dequeue();
        shuffledOpenCoord.Enqueue(randomCoord);
        return cells[randomCoord.x, randomCoord.y];
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int newx, int newy)
        {
            x = newx;
            y = newy;
        }

        public static bool operator ==(Coord x, Coord y)
        {
            return x.x == y.x && x.y == y.y;
        }

        public static bool operator !=(Coord x, Coord y)
        {
            return !(x == y);
        }
    }
}