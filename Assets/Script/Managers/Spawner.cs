using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public float gunnerChance;
    static public float spite; //0 - 10 
    public float spiteLvl;
    int spiteLock;
    Transform player;
    public float cooldown; // 1-5
    public GameObject[] prefabs;

    public List<Vector3> spawnPositions = new List<Vector3>();

    public List<Vector3> farPoints;

    void Start()
    {
        // Initialize spawn positions
        int[] xs = {1,48};
        var ys = new List<int> { 1, 29 };
        foreach (int x in xs)
        {
            foreach (int y in ys)
            {
                spawnPositions.Add(new Vector3(x, y));
            }
        }

        spite = 3;
        spiteLock = 3;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GameManager.WhenDie += End;
        Invoke("Spawn", 0);
    }

    private void Update()
    {
        spiteLvl = spite;
        spite = Mathf.Clamp(spite, 0, 10);
        if (spite >= (spiteLock + 1)) spiteLock++;
        if (spite > spiteLock) spite -= 0.02f * Time.deltaTime;
    }

    void Spawn()
    {
        // Make a list of spawn points which are far from the player
        farPoints = new List<Vector3>();
        foreach (Vector3 pos in spawnPositions)
        {
            var distance = Vector3.Distance(player.position, pos);
            if (distance > 20)
            {
                farPoints.Add(pos);
            }
        }

        // Pick a random spawn point far from the player
        int randomPosition = Random.Range(0, farPoints.Count);
        Vector3 newPos = farPoints[randomPosition];

        int random_prefab = 0;

        if (Random.value < gunnerChance){ 
            random_prefab = Random.Range(1, prefabs.Length); 
        }
        Instantiate(prefabs[random_prefab], newPos, Quaternion.identity);
        cooldown = 3.3f - (spite * 0.23f);
        Invoke("Spawn", Random.Range(cooldown--, cooldown++));
    }

    void End()
    {
        GameManager.WhenDie -= End;
        Destroy(gameObject);

    }

}
