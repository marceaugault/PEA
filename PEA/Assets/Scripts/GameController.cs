using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum GameState
{
    Playing,
    SwitchingRoom
}

public class GameController : MonoBehaviour
{
    [SerializeField] RawImage BlackSreen;
    [SerializeField] float BlackScreenDuration = 1f;
    [SerializeField] float ScreenFadeOutSpeed = 1f;

    public int MaxLevel { get; private set; }

    private int difficulty;
    public int Difficulty 
    {
        get { return difficulty; }
        set
        {
            difficulty = value;
            if (difficulty > MaxLevel)
                MaxLevel = difficulty; 
        }
    }
    public int Money { get; private set; }

    PlayerController PlayerController = null;
    UIController UIController = null;

    delegate void OnMoneyAdded(int money);
    OnMoneyAdded OnMoneyChanged;
    private void Awake()
	{
        BlackSreen = GameObject.Find("BlackScreen")?.GetComponent<RawImage>();
        PlayerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<UIController>();

        Load();
    }

	void Start()
    {
        if (BlackSreen)
            BlackSreen.enabled = false;

        if (PlayerController)
		{
            PlayerController.UpdateStats();
		}

        UIController ui = FindObjectOfType<UIController>();
        if (ui)
        {
            OnMoneyChanged += ui.UpdateMoney;
            OnMoneyChanged(Money);
            ui.UpdateDifficulty(Difficulty);
        }

        //UIMenuController menuUI = FindObjectOfType<UIMenuController>();
        //if (menuUI)
        //{
        //    OnMoneyChanged += menuUI.;
        //    OnMoneyChanged(Money);
        //    menuUI.UpdateDifficulty(Difficulty);
        //}
    }
	private void OnDestroy()
	{
        Save();
	}
	public void SwitchRoom()
    {
        Difficulty++;

        UIController.UpdateDifficulty(Difficulty);

        BlackSreen.color = Color.black;
        BlackSreen.enabled = true;

        PlayerController.UpdateStats();
        UIController.UpdateStats(PlayerController.stats);

        StartCoroutine(BlackScreenFadeOut());
    }

    IEnumerator BlackScreenFadeOut()
    {
        yield return new WaitForSeconds(BlackScreenDuration);

        while (BlackSreen.color.a >= 0.01f)
        {
            Color c = BlackSreen.color;
            c.a -= Time.deltaTime * ScreenFadeOutSpeed;
            BlackSreen.color = c;

            yield return null;
        }

        BlackSreen.enabled = false;
    }

    public void AddMoney(int money)
	{
        Money += money;

		OnMoneyChanged?.Invoke(Money);
	}

    public int GetUpgradeCost()
	{
        return (int)(1f + Mathf.Pow(PlayerController.stats.Level, 1.8f));
    }
    public void Load()
	{
        if (PlayerPrefs.HasKey("Difficulty"))
            Difficulty = PlayerPrefs.GetInt("Difficulty");
        else
            Difficulty = 0;

        if (PlayerPrefs.HasKey("MaxLevel"))
            MaxLevel = PlayerPrefs.GetInt("MaxLevel");
        else
            MaxLevel = 0;

        if (PlayerPrefs.HasKey("Money"))
            Money = PlayerPrefs.GetInt("Money");
        else
            Money = 0;
    }
    
    public void Save()
	{
        PlayerPrefs.SetInt("Difficulty", Difficulty);
        PlayerPrefs.SetInt("MaxLevel", MaxLevel);
        PlayerPrefs.SetInt("Money", Money);
	}

}
