using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StatType
{
    Force,
    Dexterity,
    Agility,
    Vitality,
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

    [SerializeField] float ForceMultiplier = 0.8f;
    [SerializeField] float DexterityMultiplier = 0.5f;
    [SerializeField] float VitalityMultiplier = 0.7f;
    [SerializeField] float AgilityMultiplier = 0.5f;
    [SerializeField] float BulletLifeMultiplier = 0.5f;
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

	private void Start()
	{

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
            case StatType.Vitality: CharBonusStats.Vitality++; break;
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
        return CharBaseStats.BaseMaxLife + Mathf.Pow(CharBonusStats.Vitality, VitalityMultiplier);
    }
    float GetAttackDamage()
    {
        return CharBaseStats.BaseDamage + Mathf.Pow(CharBonusStats.Force, ForceMultiplier);
    }

    float GetMoveSpeed()
    {
        return CharBaseStats.BaseMoveSpeed + Mathf.Pow(CharBonusStats.Agility, AgilityMultiplier);
    }

    float GetAttackSpeed()
    {
        float fireRate = (Mathf.Pow(CharBonusStats.Dexterity, DexterityMultiplier) * 0.25f) / CharBaseStats.BaseFireRate;

        return CharBaseStats.BaseFireRate - Mathf.Clamp(fireRate, 0f, CharBaseStats.BaseFireRate - 0.0001f);
    }

    float GetBulletLifetime()
    {
        return CharBaseStats.BaseBulletLifetime + Mathf.Pow(CharBonusStats.BulletLifetime, BulletLifeMultiplier);
    }

    public CharStats GetBonusStats()
	{
        return CharBonusStats;
	}

    #endregion Stats

    #region SaveLoad
    public void SaveStats()
    {
        PlayerPrefs.SetString("CharacterBonusStats", JsonUtility.ToJson(CharBonusStats));
        PlayerPrefs.SetInt("Level", Level);
    }

    public void LoadStats()
    {
        Debug.Log("Loadind save");

        if (PlayerPrefs.HasKey("CharacterBonusStats"))
		{
            CharBonusStats = JsonUtility.FromJson<CharStats>(PlayerPrefs.GetString("CharacterBonusStats"));
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
