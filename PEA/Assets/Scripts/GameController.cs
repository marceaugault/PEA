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
    public int Difficulty { get; set; }

    PlayerController PlayerController = null;
    UIController UIController = null;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        BlackSreen.enabled = false;

        PlayerController = FindObjectOfType<PlayerController>();
        UIController = FindObjectOfType<UIController>();

        if (PlayerController)
		{
            PlayerController.UpdateStats();
		}
    }

    public void SwitchRoom()
    {
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


}
