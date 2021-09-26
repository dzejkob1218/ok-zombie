using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;



[System.Serializable]
public class LevelNode
{
    public int type;//-1 - reserved, 0 - empty, 1 - wall, 2 - diagonal
    public float wallChance;//Chance that the node gets a regular wall
    public float obstacleChance;
    public int wallChain; //The length of the wall chain leading up to this node
    public Vector2 diagChance;//Chance that a diagonal wall occurs, x is left, y is right


    public LevelNode(float wallchance, int wallchain, Vector2 diagonalchance, float obstaclechance = 0.01f) 
        {
        this.wallChance = wallchance;
        this.wallChain = wallchain;
        this.diagChance = diagonalchance;
        this.obstacleChance = obstaclechance;
        }

}
public class LevelMaker : MonoBehaviour
{
    public float density;   
    public List<GameObject> walls = new List<GameObject>();
    public List<GameObject> obstacles = new List<GameObject>();
    public GameObject weapon;
    public int sizeX;
    public int sizeY;
    public LevelNode[] nodes;//Stores wallchance for all nodes
    int x, y;

    float chanceWallDiagonal = 0.4f;
    float chanceWildDiagonal = 0.05f;

    private void Start()
    {
        StartCoroutine("MakeNewLevel");
    }

    IEnumerator MakeNewLevel()
    {
            // Destroy children
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Initialize nodes
            nodes = new LevelNode[sizeX * sizeY];
            for (int i = 0; i < nodes.Length; i++)
            {
                Node(i); // Get X and Y of node
                float midX = Mathf.Abs(x - (sizeX / 2.0f)) / (sizeX / 2.0f); // Distance to middle of X, 1 is edge, 0 is middle
                float midY = Mathf.Abs(y - (sizeY / 2.0f)) / (sizeY / 2.0f);
                float mod = (midX + midY) / 2;           
                if (mod < 0.6f)mod = Mathf.Pow(mod, 1.5f);
                nodes[i] = new LevelNode(0.15f * density * mod, 0, new Vector2(chanceWildDiagonal * density * mod, chanceWildDiagonal * density * mod), 0.015f * density * (1.0f - mod));
            }

            PlaceObjects();
            yield return null;

            MakeWalls();
            MakeDiagonals();
            PlaceObstacles();
            AstarPath.active.Scan();
            yield return null;
        

    }


    void PlaceObjects()
    {

        x = Mathf.RoundToInt(sizeX / 2);
        y = Mathf.RoundToInt(sizeY / 2);

        // Reserve nodes
        for (int nY = -1; nY < 2; nY++)
        {
            for (int nX = -1; nX < 2; nX++)
            {
                Node(x + nX, y + nY).type = -1;
            }
        }
        // Reserve zombie spawn points
        Node(1, 1).type = -1;
        Node(1, 29).type = -1;
        Node(48, 1).type = -1;
        Node(48, 29).type = -1;

        // Spawn weapons
        for (int i = 0; i < 25; i++) {
            int r = Random.Range(sizeX * 2, nodes.Length - (sizeX * 2));
            Node(r);
            Instantiate(weapon, new Vector2(x, y), Quaternion.identity);
            Node(x, y).type = -1;
        }
    }

    void MakeWalls()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            Node(i);

            // Create wall
            if (Random.value < nodes[i].wallChance && nodes[i].type == 0)
            {
                // Spawn the wall
                nodes[i].type = 1;
                GameObject wall = Instantiate(walls[0], new Vector2(x, y), Quaternion.identity, transform);
                wall.name = "Wall" + i;

                // Decrease the chance of other walls appearing nearby
                for (int nY = -1; nY < 2; nY++)
                {
                    for (int nX = -1; nX < 2; nX++)
                    {
                        LevelNode n = Node(x + nX, y + nY);
                        if (n.diagChance.x != chanceWallDiagonal && n.diagChance.y != chanceWallDiagonal) n.diagChance = new Vector2(-2, -2);
                    }
                }

                // Increase chance of wall appearing next to this one - longer fences
                Node(x + 1, y).wallChain = nodes[i].wallChain + 1;
                Node(x + 1, y).wallChance += 0.85f - (nodes[i].wallChain * 0.4f);

                // Decrease the chance up - wider corridors
                Node(x, y + 1).wallChance -= 2f;
                Node(x - 1, y + 1).wallChance -= 1f;
                Node(x + 1, y + 1).wallChance -= 1f;
                Node(x, y + 2).wallChance -= 0.2f;

                // Increase the chance for diagonal walls
                if (nodes[i].wallChain == 0)
                {
                    if (Random.value < 0.5f)
                    {
                        Node(x - 1, y).diagChance.y = chanceWallDiagonal;
                        Node(x - 1, y - 1).diagChance.x = 0f;
                    }
                    else
                    {
                        Node(x - 1, y).diagChance.y = 0;
                        Node(x - 1, y - 1).diagChance.x = chanceWallDiagonal;
                    }
                }
            }
            else
            {
                // End the chain
                if (nodes[i].wallChain > 0)
                {
                    Node(x + 1, y).wallChance = -1;
                    Node(x + 2, y).wallChance -= 0.2f;

                    // Increase the chance for diagonal walls after the chain ends
                    if (Random.value < 0.5f)
                    {
                        nodes[i].diagChance.x = chanceWallDiagonal;
                        Node(x, y - 1).diagChance.y = 0;
                    }
                    else
                    {
                        nodes[i].diagChance.x = 0;
                        Node(x, y - 1).diagChance.y = chanceWallDiagonal;
                    }
                }
            }
        }
    }

    // TODO: Abstract these similar methods
    void MakeDiagonals()
    {
        // Left-side walls
        for (int i = 0; i < (sizeY * sizeX); i++)
        {
            Node(i);
            // Wall
            if (Random.value < nodes[i].diagChance.x && nodes[i].type == 0)
            {
                // Spawn wall
                nodes[i].type = 2;
                GameObject wall = Instantiate(walls[1], new Vector2(x, y), Quaternion.identity, transform);
                wall.name = "WallDiagonalLeft" + i;
                // When you get a diagonal, decrease chance of others appearing near, but increase chance of continuing
                for (int nY = -1; nY < 2; nY++) {for (int nX = -1; nX < 2; nX++){

                        // But increase chance of continuing, if unclaimed territory
                        if (nX == 1 && nY == 1)
                        {
                            LevelNode n = Node(x + 1, y + 1);
                            if (n.diagChance.x == chanceWildDiagonal)
                            n.diagChance.x = 1f - (nodes[i].wallChain * 0.4f);
                            n.wallChain = nodes[i].wallChain + 1;
                        }
                        else if (nX == -1 && nY == -1)
                        {
                            LevelNode n = Node(x - 1, y - 1);
                            if (n.diagChance.x == chanceWildDiagonal)
                                n.diagChance.x = 1f - (nodes[i].wallChain * 0.4f);
                            n.wallChain = nodes[i].wallChain + 1;
                        }
                        else { Node(x + nX, y + nY).diagChance = new Vector2(-2, -2); }
                    } }
            }
            else
            {

            }
        }

        // Right-side walls
        for (int i = 0; i < (sizeY * sizeX); i++)
        {
            // Get coordinates
            Node(i);

            // Wall
            if (Random.value < nodes[i].diagChance.y && nodes[i].type == 0)
            {
                // Spawn Wall
                nodes[i].type = 2;
                GameObject wall = Instantiate(walls[2], new Vector2(x, y), Quaternion.identity, transform);
                wall.name = "WallDiagonalRight" + i;
                // When you get a diagonal, decrease chance of others appearing near
                for (int nY = -1; nY < 2; nY++)
                {
                    for (int nX = -1; nX < 2; nX++)
                    {
                        // But increase chance of continuing, if unclaimed territory
                        if (nX == -1 && nY == 1)
                        {
                            LevelNode n = Node(x - 1, y + 1);
                            if (n.diagChance.y == chanceWildDiagonal)
                                n.diagChance.y = 1f - (nodes[i].wallChain * 0.4f);
                            n.wallChain = nodes[i].wallChain + 1;
                        }
                        else if (nX == 1 && nY == -1)
                        {
                            LevelNode n = Node(x + 1, y - 1);
                            if (n.diagChance.y == chanceWildDiagonal)
                                n.diagChance.y = 1f - (nodes[i].wallChain * 0.4f);
                            n.wallChain = nodes[i].wallChain + 1;
                        }
                        else { Node(x + nX, y + nY).diagChance = new Vector2(-2, -2); }
                    }
                }
               
            }
            else
            {

            }
        }
    }


    void PlaceObstacles()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            Node(i);
            if (Random.value < nodes[i].obstacleChance && nodes[i].type == 0 && nodes[i].diagChance != new Vector2(-2,-2))
            {
                // Create obstacle
                nodes[i].type = -1;
                int randomObstacle = Random.Range(0, obstacles.Count);
                GameObject wall = Instantiate(obstacles[randomObstacle], new Vector2(x, y), Quaternion.identity, transform);
                wall.name = "Obstacle" + i;
            }
        }
    }
           



    void Node(int n)
    {
        x = n % sizeX;
        y = (n - (n % sizeX)) / sizeX;
    }

    LevelNode Node(int x, int y)
    {
        int i = (y * sizeX) + x;
        // If this function is asked about a nonexistent node, it returns a dummy node. commmand is done on the dummy and code can continue
        if (i < 0 || i > (nodes.Length - 1))
        {return new LevelNode(0f, 0, new Vector2(0, 0));}
        // Else it returns a proper node
        return nodes[i];
    }

}
