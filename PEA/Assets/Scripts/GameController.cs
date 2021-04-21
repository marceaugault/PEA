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

    // Start is called before the first frame update
    void Start()
    {
        BlackSreen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchRoom()
    {
        BlackSreen.color = Color.black;
        BlackSreen.enabled = true;

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
