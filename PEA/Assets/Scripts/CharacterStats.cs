using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct BaseStats
{
    public float BaseDamage;
    public float BaseAttackSpeed;
    public float BaseMoveSpeed;
    public float BaseBulletLifetime;
}

[Serializable]
public struct Stats
{
    public float Force;
    public float Dexterity;
    public float MoveSpeed;
}

public class CharacterStats : MonoBehaviour
{
    [SerializeField] BaseStats CharBaseStats;
    Stats CharStats;

    [Serializable] List<Stat> Stats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float GetAttackDamage()
    {
        return CharBaseStats.BaseDamage + CharStats.Force;
    }

    public float GetMoveSpeed()
    {
        return CharBaseStats.BaseMoveSpeed + CharStats.MoveSpeed;
    }

    public float GetAttackSpeed()
    {
        return CharBaseStats.BaseAttackSpeed + CharStats.Dexterity;
    }

    public void SaveStats()
    {
        PlayerPrefs.SetString("Character Base Stats", JsonUtility.ToJson(CharBaseStats));
        PlayerPrefs.SetString("Character Stats", JsonUtility.ToJson(CharStats));
    }

    public void LoadStats()
    {
        CharBaseStats = JsonUtility.FromJson<BaseStats>(PlayerPrefs.GetString("Character Base Stats"));
        CharStats = JsonUtility.FromJson<Stats>(PlayerPrefs.GetString("Character Stats"));
    }

}
