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

    List<RewardController> Rewards;

    UIController UIController;
    public RoomRewardType RoomRewardType { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        LeftDoor.SetRewardForNextRoom((RoomRewardType)Random.Range(0, (int)RoomRewardType.CountType));
        RightDoor.SetRewardForNextRoom((RoomRewardType)Random.Range(0, (int)RoomRewardType.CountType));

        Player = FindObjectOfType<PlayerController>();
        GameController = FindObjectOfType<GameController>();
        UIController = FindObjectOfType<UIController>();

        Rewards = new List<RewardController>();

        for (int i = 0; i < 10; i++)
		{
            GameObject go = Instantiate(RewardPrefab, new Vector3(500f, 500f, 0f), Quaternion.Euler(60f, 0f, 0f));
            RewardController rc = go.GetComponent<RewardController>();
            Rewards.Add(rc);
            rc.Init();
        }

        Reset();
    }


    private void Reset()
    {
        LeftDoor.DisableDoor();
        RightDoor.DisableDoor();

        SpawnEnemies();

        foreach (RewardController rc in Rewards)
		{
            rc.Enable(false);
		}
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


    public void EnemyKilled(Vector3 pos, LootTable table)
    {
        NbEnemiesInRoom--;

        RewardType type = table.GenerateReward();

        if (type != RewardType.None)
		{
            int multiplier = 1;

            switch  (type)
			{
                case RewardType.Money1x:    multiplier = 1; break;
                case RewardType.Money5x:    multiplier = 5; break;
                case RewardType.Money10x:   multiplier = 10; break;
                case RewardType.Money50x:   multiplier = 50; break;
            }

            SpawnReward(type, pos, multiplier * GetMoneyAmount());
		}

        if (NbEnemiesInRoom <= 0)
        {
            RoomCleared();
        }
    }

    void RoomCleared()
	{
        Debug.Log("Room Cleared");

        SpawnReward(RewardType.Money1x, new Vector3(0f, 2f, 0f), GetMoneyAmount());

        LeftDoor.EnableDoor();
        RightDoor.EnableDoor();

        UIController.OnRoomCleared();
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

    public void SpawnReward(RewardType rewardType, Vector3 pos, int moneyAmount)
    {
        int i = GetFreeRewardIndex();

        if (i != -1)
		{
            Rewards[i].SpawnReward(rewardType, pos, RewardSpawnForce);
            Rewards[i].Money = moneyAmount;
        }
    }

    int GetFreeRewardIndex()
	{
        for (int i = 0; i < Rewards.Count; i++)
		{
            if (!Rewards[i].IsActive)
			{
                return i;
			}
		}

        return -1;
	}

    int GetMoneyAmount()
	{
        return (int)Mathf.Pow(GameController.Difficulty, 1.2f);
    }
}
