using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomRewardType
{
    Money = 0,
    SpecialMoney,
    Gear,
    CountType
}

public class RoomController : MonoBehaviour
{
    [SerializeField] DoorController LeftDoor;
    [SerializeField] DoorController RightDoor;

    [SerializeField] Vector2Int EnemiesToSpawn = new Vector2Int(1, 5);

    [SerializeField] List<GameObject> EnemiesPrefabs;
    int NbEnemiesInRoom = 0;

    [SerializeField] GameObject RewardPrefab;
    [SerializeField] float RewardSpawnForce = 10f;

    GameController GameController = null;
    PlayerController Player = null;
    public RoomRewardType RoomRewardType { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        LeftDoor.SetRewardForNextRoom((RoomRewardType)Random.Range(0, (int)RoomRewardType.CountType));
        RightDoor.SetRewardForNextRoom((RoomRewardType)Random.Range(0, (int)RoomRewardType.CountType));

        Player = FindObjectOfType<PlayerController>();
        GameController = FindObjectOfType<GameController>();

        Reset();
    }


    private void Reset()
    {
        LeftDoor.DisableDoor();
        RightDoor.DisableDoor();

        SpawnEnemies();
    }


    void SpawnEnemies()
    {
        if (EnemiesPrefabs.Count == 0)
            return;

        NbEnemiesInRoom = Random.Range(EnemiesToSpawn.x, EnemiesToSpawn.y);
        Debug.Log("Number of enemies to kill: " + NbEnemiesInRoom);
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
            enemy.Init(GameController.Difficulty);
        }
    }


    public void EnemyKilled()
    {
        NbEnemiesInRoom--;

        if (NbEnemiesInRoom <= 0)
        {
            RoomCleared();
        }
    }

    void RoomCleared()
	{
        Debug.Log("Room Cleared");

        GameController.AddMoney((int)Mathf.Pow(GameController.Difficulty, 1.2f));

        SpawnReward(RoomRewardType);

        LeftDoor.EnableDoor();
        RightDoor.EnableDoor();
    }


    public void CreateNewRoom(RoomRewardType rewardType, Vector3 newPlayerPosition)
    {
        GameController.SwitchRoom();

        Reset();

        RoomRewardType = rewardType;

        LeftDoor.SetRewardForNextRoom((RoomRewardType)Random.Range(0, (int)RoomRewardType.CountType));
        RightDoor.SetRewardForNextRoom((RoomRewardType)Random.Range(0, (int)RoomRewardType.CountType));

        Player.transform.position = newPlayerPosition;
    }

    public void SpawnReward(RoomRewardType rewardType)
    {
        GameObject go = Instantiate(RewardPrefab, new Vector3(0f, 2f, 0f), Quaternion.Euler(60f, 0f, 0f));

        go.GetComponent<Rigidbody>()?.AddForce(Vector3.up * RewardSpawnForce);
        SpriteRenderer sprite = go.GetComponent<SpriteRenderer>();

        switch (rewardType)
        {
            case RoomRewardType.Money:
                sprite.sprite = Resources.Load<Sprite>("Sprites/money-reward-icon");
                break;
            case RoomRewardType.SpecialMoney:
                sprite.sprite = Resources.Load<Sprite>("Sprites/special-money-reward-icon");
                break;
            case RoomRewardType.Gear:
                sprite.sprite = Resources.Load<Sprite>("Sprites/gear-reward-icon");
                break;
        }
    }
}
