using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTable
{
    [System.Serializable]
    struct Reward
    {
        public RewardType Type;
        public int Weight;
    }

    [SerializeField] List<Reward> Table;

    int TotalWeight = 0;
    public void Init()
    {
        foreach (Reward r in Table)
		{
            TotalWeight += r.Weight;
        }
    }
    public RewardType GenerateReward()
	{
        int random = Random.Range(0, TotalWeight);

        foreach (Reward r in Table)
		{
            if (random <= r.Weight)
			{
                return r.Type;
			}
			else
			{
                random -= r.Weight;
			}
		}

        return RewardType.None;
	}
}
