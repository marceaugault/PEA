using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenuController : MonoBehaviour
{
	[SerializeField] Dropdown LevelDropdown;

    [SerializeField] GameObject CharacterPanel;
    [SerializeField] GameObject MainPanel;

	[SerializeField] Button ForceBtn;
	[SerializeField] Button DexterityBtn;
	[SerializeField] Button AgilityBtn;
	[SerializeField] Button BulletLifeBtn;

	[SerializeField] Text ForceText;
    [SerializeField] Text DexterityText;
    [SerializeField] Text AgilityText;
    [SerializeField] Text BulletLifeText;

    [SerializeField] Text DamageText;
    [SerializeField] Text AttackSpeedText;
    [SerializeField] Text CMoveSpeedText;
    [SerializeField] Text CBulletLifeText;

    [SerializeField] Text Money;
    [SerializeField] Text UpgradeCost;

    [SerializeField] Text LevelText;

    CharacterStats cStats = null;
    GameController gameController = null;

    // Start is called before the first frame update
    void Start()
    {
        cStats = FindObjectOfType<CharacterStats>();
        gameController = FindObjectOfType<GameController>();

        ForceBtn.onClick.AddListener(() => { AddStat(StatType.Force); });
        DexterityBtn.onClick.AddListener(() => { AddStat(StatType.Dexterity); });
        AgilityBtn.onClick.AddListener(() => { AddStat(StatType.Agility); });
        BulletLifeBtn.onClick.AddListener(() => { AddStat(StatType.BulletLife); });

        LevelDropdown.ClearOptions();
        int i;
        for (i = 0; i <= gameController.MaxLevel / 2; i += 5)
		{
            LevelDropdown.options.Add(new Dropdown.OptionData() { text = i.ToString() });
        }
        LevelDropdown.value = i;

        MainPanel.SetActive(true);
        CharacterPanel.SetActive(false);
    }

    public void UpdateStats(CharacterStats stats)
    {
        DamageText.text = stats.Damage.ToString("0.00");
        AttackSpeedText.text = (1f / stats.FireRate).ToString("0.00");
        CMoveSpeedText.text = (stats.MoveSpeed / 100f).ToString("0.00");
        CBulletLifeText.text = stats.BulletLifetime.ToString("0.00");

        CharStats cstats = stats.GetBonusStats();

        ForceText.text = cstats.Force.ToString();
        DexterityText.text = cstats.Dexterity.ToString();
        AgilityText.text = cstats.Agility.ToString();
        BulletLifeText.text = cstats.BulletLifetime.ToString();
    }

    //public void UpdateMoney()
	//{
    //    Money.text
	//}

    public void AddStat(StatType type)
	{
        if (cStats)
		{
            int cost = gameController.GetUpgradeCost();
            if (cost > 0 && cost <= gameController.Money)
			{
                cStats.AddStat(type);

                gameController.AddMoney(-cost);

                Money.text = gameController.Money.ToString();
                UpgradeCost.text = gameController.GetUpgradeCost().ToString();

                LevelText.text = "Level " + cStats.Level;
            }
		}
	}

	public void Play()
	{
        gameController.Difficulty = LevelDropdown.value * 5;
        SceneManager.LoadScene("RoomScene");
	}

    public void OpenCharacterUpgradePanel()
	{
        MainPanel.SetActive(false);
        CharacterPanel.SetActive(true);

        Money.text = gameController.Money.ToString();
        UpgradeCost.text = gameController.GetUpgradeCost().ToString();

        LevelText.text = "Level " + cStats.Level;
    }

    public void Back()
	{
        MainPanel.SetActive(true);
        CharacterPanel.SetActive(false);
	}
    public void Quit()
	{
        Application.Quit();
	}
}
