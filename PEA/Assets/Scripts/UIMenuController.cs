using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenuController : MonoBehaviour
{
	[SerializeField] Dropdown LevelDropdown;

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
        for (int i = 0; i <= gameController.MaxLevel / 2; i += 5)
		{
            LevelDropdown.options.Add(new Dropdown.OptionData() { text = i.ToString() });
        }
    }

    public void UpdateStats(CharacterStats stats)
    {
        DamageText.text = stats.Damage.ToString(".0");
        AttackSpeedText.text = (1f / stats.FireRate).ToString(".0");
        CMoveSpeedText.text = (stats.MoveSpeed / 100f).ToString(".0");
        CBulletLifeText.text = stats.BulletLifetime.ToString(".0");

        CharStats cstats = stats.GetBonusStats();

        ForceText.text = cstats.Force.ToString();
        DexterityText.text = cstats.Dexterity.ToString();
        AgilityText.text = cstats.Agility.ToString();
        BulletLifeText.text = cstats.BulletLifetime.ToString();
    }

    public void AddStat(StatType type)
	{
        if (cStats)
		{
            cStats.AddStat(type);
		}
	}

	public void Play()
	{
        SceneManager.LoadScene("RoomScene");
        gameController.Difficulty = LevelDropdown.value;
	}

    public void OpenCharacterUpgradePanel()
	{

	}
    public void Quit()
	{
        Application.Quit();
	}
	//   public void AddForce()
	//{

	//}

	//   public void AddDexterity()
	//   {

	//   }

	//   public void AddMoveSpeed()
	//   {

	//   }

	//   public void AddBulletLife()
	//   {

	//   }
}
