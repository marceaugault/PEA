using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] GameObject LeftDoor;
    [SerializeField] GameObject RightDoor;

    [SerializeField] Material MDisabledDoor;
    [SerializeField] Material MEnabledDoor;

    [SerializeField] Vector2Int EnemiesToSpawn = new Vector2Int(1, 5);

    [SerializeField] List<GameObject> EnemiesPrefabs;
    int NbEnemiesInRoom = 0;

    public static int Difficulty = 0;

    // Start is called before the first frame update
    void Start()
    {
        DisableDoor(LeftDoor);
        DisableDoor(RightDoor);

        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        if (EnemiesPrefabs.Count == 0)
            return;

        NbEnemiesInRoom = Random.Range(EnemiesToSpawn.x, EnemiesToSpawn.y);

        Vector3 roomScale = transform.GetChild(0).transform.localScale;
        Vector3 spawnRange = new Vector3(roomScale.x / 2f - 2f, 0f, roomScale.z / 2f - 2f);
        Vector3 position = Vector3.zero;

        for (int i = 0; i < NbEnemiesInRoom; i++)
        {
            position = new Vector3(Random.Range(-spawnRange.x, spawnRange.x), 1.6f, Random.Range(-spawnRange.z, spawnRange.z));
            int enemyType = Random.Range(0, EnemiesPrefabs.Count);

            GameObject go = Instantiate(EnemiesPrefabs[enemyType], position, Quaternion.identity);
            EnemyController enemy = go.GetComponent<EnemyController>();
            enemy.EnemyKilledDelegate += EnemyKilled;
        }
    }

    void EnableDoor(GameObject door)
    {
        door.GetComponent<MeshRenderer>().material = MEnabledDoor;
    }

    void DisableDoor(GameObject door)
    {
        door.GetComponent<MeshRenderer>().material = MDisabledDoor;
    }

    public void EnemyKilled()
    {
        NbEnemiesInRoom--;

        if (NbEnemiesInRoom <= 0)
        {
            Debug.Log("Doors Enabled");

            EnableDoor(LeftDoor);
            EnableDoor(RightDoor);

            Difficulty++;
        }
    }
}
