using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider LifeSlider;

    PlayerController player;

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
}
