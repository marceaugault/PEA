using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIController : MonoBehaviour
{
    public Slider LifeSlider;

    PlayerController player;

    public Text DamageText;
    public Text AttackSpeedText;
    public Text MoveSpeedText;
    public Text BulletLifetimeText;

    public Text MoneyText;
    public Text DifficultyText;

    public Text RoomCleared;

    public Text MoneyPickup;

    public GameObject GameOverPanel;

    [SerializeField] float RoomClearedTextFadeSpeed = 1f;

    Coroutine coroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if (!player)
        {
            Debug.LogError("UIController: Player not found");
        }

        GameOverPanel.SetActive(false);

        RoomCleared.enabled = false;
        MoneyPickup.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        LifeSlider.value = player.GetLife01();
    }

    public void UpdateStats(CharacterStats stats)
	{
        DamageText.text = stats.Damage.ToString("0.00");
        AttackSpeedText.text = (1f / stats.FireRate).ToString("0.00");
        MoveSpeedText.text = (stats.MoveSpeed / 100f).ToString("0.00");
        BulletLifetimeText.text = stats.BulletLifetime.ToString("0.00");
	}

    public void OnRoomCleared()
	{
        StartCoroutine(TextFadeOut(RoomCleared));
    }

    public void OnMoneyPickup(int value)
	{
        MoneyPickup.text = "+ " + value.ToString();

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(TextFadeOut(MoneyPickup));
    }

    IEnumerator TextFadeOut(Text text)
    {
        text.enabled = true;

        Color color = text.color;
        color.a = 1f;
        text.color = color;

        yield return new WaitForSeconds(1f);

        while (text.color.a >= 0.01f)
        {
            Color c = text.color;
            c.a -= Time.deltaTime * RoomClearedTextFadeSpeed;
            text.color = c;

            yield return null;
        }

        text.enabled = false;
    }
    public void GameOver()
	{
        GameOverPanel.SetActive(true);
	}
    public void UpdateMoney(int money)
	{
        MoneyText.text = money.ToString();
    }

    public void UpdateDifficulty(int difficulty)
	{
        DifficultyText.text = "Level " + difficulty.ToString();
	}

    public void ToMainMenu()
	{
        SceneManager.LoadScene("Menu");
	}

    public void Quit()
	{
        Application.Quit();
	}
}
