using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider LifeSlider;

    PlayerController player;

    public Text DamageText;
    public Text AttackSpeedText;
    public Text MoveSpeedText;
    public Text BulletLifetimeText;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if (!player)
        {
            Debug.LogError("UIController: Player not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        LifeSlider.value = player.GetLife01();
    }

    public void UpdateStats(CharacterStats stats)
	{
        DamageText.text = stats.Damage.ToString(".0");
        AttackSpeedText.text = (1f / stats.FireRate).ToString(".0");
        MoveSpeedText.text = (stats.MoveSpeed / 100f).ToString(".0");
        BulletLifetimeText.text = stats.BulletLifetime.ToString(".0");
	}
}
