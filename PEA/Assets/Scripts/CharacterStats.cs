using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StatType
{
    Force,
    Dexterity,
    Agility,
    BulletLife,
}

[Serializable]
public struct BaseStats
{
    public float BaseMaxLife;
    public float BaseDamage;
    public float BaseFireRate;
    public float BaseMoveSpeed;
    public float BaseBulletLifetime;
}

[Serializable]
public struct CharStats
{
    public float Vitality;
    public float Force;
    public float Dexterity;
    public float Agility;
    public float BulletLifetime;
}

public class CharacterStats : MonoBehaviour
{
    [SerializeField] BaseStats CharBaseStats;
    CharStats CharBonusStats;

    public int Level { get; private set; }
    public float MaxLife { get; private set; }
    public float Damage { get; private set; }
    public float FireRate { get; private set; }
    public float MoveSpeed { get; private set; }
    public float BulletLifetime { get; private set; }

    delegate void OnStatsUpdate(CharacterStats stats);
    OnStatsUpdate OnStatsChanged;

    // Start is called before the first frame update

    private void Awake()
	{
        LoadStats();
    }

	private void Start()
	{
        UIController ui = FindObjectOfType<UIController>();
        if (ui)
		{
            OnStatsChanged += ui.UpdateStats;
		}

        UIMenuController menuUI = FindObjectOfType<UIMenuController>();
        if (menuUI)
        {
            OnStatsChanged += menuUI.UpdateStats;
        }
    }
	private void OnDestroy()
	{
        SaveStats();
	}

    #region Stats

    public void AddStat(StatType type)
	{
        switch (type)
		{
            case StatType.Force: CharBonusStats.Force++; break;
            case StatType.Agility: CharBonusStats.Agility++; break;
            case StatType.Dexterity: CharBonusStats.Dexterity++; break;
            case StatType.BulletLife: CharBonusStats.BulletLifetime++; break;
		}

        Level++;
        ComputeStats();
    }
    public void ComputeStats()
	{
        MaxLife = GetMaxLife();
        Damage = GetAttackDamage();
        FireRate = GetAttackSpeed();
        MoveSpeed = GetMoveSpeed();
        BulletLifetime = GetBulletLifetime();

        OnStatsChanged(this);
    }
    float GetMaxLife()
    {
        return CharBaseStats.BaseMaxLife + CharBonusStats.Vitality;
    }
    float GetAttackDamage()
    {
        return CharBaseStats.BaseDamage + CharBonusStats.Force;
    }

    float GetMoveSpeed()
    {
        return CharBaseStats.BaseMoveSpeed + CharBonusStats.Agility;
    }

    float GetAttackSpeed()
    {
        return CharBaseStats.BaseFireRate - CharBonusStats.Dexterity;
    }

    float GetBulletLifetime()
    {
        return CharBaseStats.BaseBulletLifetime + CharBonusStats.BulletLifetime;
    }

    public CharStats GetBonusStats()
	{
        return CharBonusStats;
	}

    #endregion Stats

    #region SaveLoad
    public void SaveStats()
    {
        //PlayerPrefs.SetString("Character Base Stats", JsonUtility.ToJson(CharBaseStats));
        PlayerPrefs.SetString("CharacterBonusStats", JsonUtility.ToJson(CharBonusStats));
        PlayerPrefs.SetInt("Level", Level);
    }

    public void LoadStats()
    {
        //if (PlayerPrefs.HasKey("Character Base Stats"))
        //{
        //    CharBaseStats = JsonUtility.FromJson<BaseStats>(PlayerPrefs.GetString("Character Base Stats"));
        //}
        //else
        //{
        //    CharBaseStats = new BaseStats();
        //
        //}

        Debug.Log("Loadind save");

        if (PlayerPrefs.HasKey("CharacterBonusStats"))
		{
            CharBonusStats = JsonUtility.FromJson<CharStats>(PlayerPrefs.GetString("CharacterBonusStats"));
            Debug.Log("Found stats");
		}
        else
		{
            CharBonusStats = new CharStats();
		}

        if (PlayerPrefs.HasKey("Level"))
		{
            Level = PlayerPrefs.GetInt("Level");
        }
        else
		{
            Level = 0;
		}
    }
	#endregion

}
